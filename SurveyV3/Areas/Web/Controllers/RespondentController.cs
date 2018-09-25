using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyV3.Controllers;
using SurveyV3.Filter;
using Business.Respondent;
using Business.VO;
using Newtonsoft.Json;
using System.Data;

using Model.ORMModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using SurveyV3.Areas.Web.Models;
using System.Threading;

namespace SurveyV3.Areas.Web.Controllers
{
    [SessionFilter]
    public class RespondentController : BaseController
    {
        BExportLog logUtil = new BExportLog();
        BRspdQuery rspdUtil = new BRspdQuery();

        // GET: Web/Respondent
        public ActionResult Index()
        {
            return View();
        }

       /// <summary>
       /// 区域管理员访问的页面
       /// </summary>
       /// <returns></returns>
        public ActionResult AreaIndex()
        {
            string acode = (string)Session["acode"];
            ViewBag.acode = acode;
            return View();
        }

        

        /// <summary>
        /// 管理员用的答案编辑页
        /// </summary>
        /// <returns></returns>
        public ActionResult AnswersEditPage()
        {
            string rid = Request["rid"];
            ViewBag.rid=rid;
            return View();
        }

        public ActionResult AnswersEditPageF()
        {
            string rid = Request["rid"];
            ViewBag.rid = rid;
            return View();
        }

        [HttpPost]
        public ActionResult rspdAnswerSave()
        {
            string data = Request["data"];
            List<RspdAnswerVO> list = JsonConvert.DeserializeObject<List<RspdAnswerVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = rspdUtil.rspdAnswerSave(list);
            if (flag)
            {
                return WebSuccess("保存成功");
            }
            else
            {
                return WebError("保存失败");
            }
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public ActionResult respAllList()
        {
            int page = getIntParam("page");
            int rows = getIntParam("rows");
            string start = Request["start"];
            string end = Request["end"];
            string qcode = Request["code"];
            string acode = (string)Session["acode"];
            if (acode == "admin")
            {
                acode = "";
            }

            List<RspdBase> list = rspdUtil.getList(start, end, acode, qcode, page, rows);
            if (list == null || list.Count == 0)
            {
                return WebErrorList();
            }
            else
            {
                int c = rspdUtil.getCount(start, end, acode, qcode);
                return WebSuccessList(c, list);
            }

        }

        public ActionResult rspdMainDel()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }
            string rid = Request["rid"];
            bool flag = rspdUtil.delRecord(rid);
            if (flag)
            {
                return WebSuccess("操作成功");
            }
            else
            {
                return WebError("操作失败");
            }
        }

        /// <summary>
        /// 答案列表
        /// </summary>
        /// <returns></returns>
        public ActionResult rspdAnswerList()
        {
            string rid = Request["rid"];
            List<RspdAnswerVO> list = rspdUtil.getRspdAnswerList(rid);
            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            }
        }

        /// <summary>
        /// 只有F的答案列表
        /// </summary>
        /// <returns></returns>
        public ActionResult rspdAnswerListF()
        {
            string rid = Request["rid"];
            List<RspdAnswerVO> list = rspdUtil.getRspdAnswerListF(rid);
            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            }
        }

        /// <summary>
        /// 修改受访记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult rspdBaseSave()
        {
            string rid = Request["rid"];
            string oldname = Request["oldname"];
            string name = Request["name"];
            string phone = Request["phone"];

            bool flag = rspdUtil.editRecord(rid, oldname, name, phone);
            if (flag)
            {
                return WebSuccess("操作成功");
            }
            else
            {
                return WebError("操作失败");
            }
        }


        /// <summary>
        /// 区域管理员导出工人的调查记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult exportWorkerExcel()
        {
            string start = Request["start"];
            string end = Request["end"];
            string acode = (string)Session["acode"];

            DataTable dt = rspdUtil.getExportRspdWorker(start, end, acode);
            if (dt == null)
            {
                return Content("失败!");
            }
            try
            {
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet(start);
                int i = 0;
                int j = dt.Columns.Count;
                //创建栏目
                NPOI.SS.UserModel.IRow row = sheet1.CreateRow(0);
                for (i = 0; i < j; i++)
                {
                    row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                }
                i = 1;
                if (dt != null)
                {

                    foreach (DataRow tbrow in dt.Rows)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i);
                        for (int col = 0; col < j; col++)
                        {
                            rowtemp.CreateCell(col).SetCellValue(tbrow[col].ToString());
                        }
                        i++;
                    }
                }
                // 写入到客户端
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", "worker_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                Response.BinaryWrite(ms.ToArray());
                book = null;
                ms.Close();
                ms.Dispose();
                return Content("");
            }
            catch (Exception e)
            {
                return Content("失败!");
            }
        }

        /// <summary>
        /// 调查结果表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult exportAnswerFile()
        {
            string acode = Request["acode"];
            string qcode = Request["qcode"];
            string start = Request["start"];
            string end = Request["end"];
            string rid = Business.Tools.createID();
            
            //只可能是admin使用
            bool flag = logUtil.addObj(rid, start, end, "", "调查详情");
            if (flag == false)
            {
                return WebError("导出失败");

            }

            AnswerFileVO ob = new AnswerFileVO();
            ob.start = start;
            ob.end = end;
            ob.qcode = qcode;
            ob.acode = acode;
            ob.rid = rid;

            Thread th = new Thread(new ParameterizedThreadStart(createFileXls));
            th.Start(ob);


            return WebError("导出操作开始执行。本操作耗时较长，完成后在导出历史中下载");
        }

        /// <summary>
        /// 调查完成情况登记表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult exportCountToExcel()
        {
            
            string rid = Business.Tools.createID();

            //只可能是admin使用
            bool flag = logUtil.addObj(rid, "不限", "不限", "", "调查完成情况登记表");
            if (flag == false)
            {
                return WebError("导出失败");

            }

            AnswerFileVO ob = new AnswerFileVO(); 
            ob.rid = rid;

            Thread th = new Thread(new ParameterizedThreadStart(createCountXls));
            th.Start(ob);


            return WebError("导出操作开始执行。本操作耗时较长，完成后在导出历史中下载");
        }

        /// <summary>
        /// 线程生成excel,操作成功后更新导出字段
        /// </summary>
        /// <param name="obj"></param>
        void createFileXls(object obj)
        {
            AnswerFileVO avo = (AnswerFileVO)obj;

            string filename = avo.start + ".xls";

            string root = HttpContext.Server.MapPath("/excel");
            if (System.IO.Directory.Exists(root) == false)
            {
                System.IO.Directory.CreateDirectory(root);
            }
            //物理路径
            string filepath = Path.Combine(root, filename);
            //网络路径
            string urlpath = "/excel/" + filename;
            string second="";
            bool flag = false;
            try
            {
               // DataTable dt = rspdUtil.getExportRspdBase(avo.start, avo.end, avo.acode, avo.qcode);
               
                DataTable dt = rspdUtil.getExportRspdBaseForArea(avo.start, avo.end,out second);
                if (dt == null)
                {
                    flag = false;
                }
                else
                {

                    NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet(avo.start);
                    int i = 0;
                    int j = dt.Columns.Count;
                    //创建栏目
                    NPOI.SS.UserModel.IRow row = sheet1.CreateRow(0);
                    for (i = 0; i < j; i++)
                    {
                        row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    }
                    i = 1;
                    if (dt != null)
                    {

                        foreach (DataRow tbrow in dt.Rows)
                        {
                            NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i);
                            for (int col = 0; col < j; col++)
                            {
                                rowtemp.CreateCell(col).SetCellValue(tbrow[col].ToString());
                            }
                            i++;
                        }
                    }
                    // 写入到客户端
                    MemoryStream stream = new MemoryStream();
                    book.Write(stream);
                    var buf = stream.ToArray();

                    //保存为Excel文件  
                    using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                    }
                    book = null;
                    flag = true;
                }
            }
            catch (Exception e)
            {
                Log.i("createFileXls", e.StackTrace);
                flag = false;
            }
           
            if (flag)
            {
                logUtil.updateState(avo.rid, urlpath, "生成成功",second);
            }
            else
            {
                logUtil.updateState(avo.rid, "", "生成失败",second);
            }
        }


        void createCountXls(object obj)
        {
            AnswerFileVO avo = (AnswerFileVO)obj;

            string filename ="SurveyCode_"+Business.Tools.createID() + ".xls";

            string root = HttpContext.Server.MapPath("/excel");
            if (System.IO.Directory.Exists(root) == false)
            {
                System.IO.Directory.CreateDirectory(root);
            }
            //物理路径
            string filepath = Path.Combine(root, filename);
            //网络路径
            string urlpath = "/excel/" + filename;
            string second = "";
            bool flag = false;
            try
            {
              //  DataTable dt = rspdUtil.getExportRspdCount();
         
                DataTable dt = rspdUtil.getExportRspdCountNew(out second);
                if (dt == null)
                {
                    flag = false;
                }
                else
                {

                    NPOI.HSSF.UserModel.HSSFWorkbook book = getMobanHSSFWorkbook();
                    NPOI.SS.UserModel.ISheet sheet1 = book.GetSheetAt(0);
                    int i = 4;
                    int j = dt.Columns.Count;
                    if (dt != null)
                    {
                        ICellStyle cellstyle = book.CreateCellStyle();
                        IFont font1 = book.CreateFont();
                        font1.FontHeightInPoints=10;
                        cellstyle.SetFont(font1);
                        foreach (DataRow tbrow in dt.Rows)
                        {
                            NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i);
                            for (int col = 0; col < j; col++)
                            {
                                ICell ic = rowtemp.CreateCell(col);
                                ic.CellStyle = cellstyle;
                                ic.SetCellValue(tbrow[col].ToString());
                            }
                            i++;
                        }
                    }
                    // 写入到客户端
                    MemoryStream stream = new MemoryStream();
                    book.Write(stream);
                    var buf = stream.ToArray();

                    //保存为Excel文件  
                    using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(buf, 0, buf.Length);
                        fs.Flush();
                    }
                    book = null;
                    flag = true;
                }
            }
            catch (Exception e)
            {
                Log.i("createCountXls",e.StackTrace);
                flag = false;
            }

            if (flag)
            {
                logUtil.updateState(avo.rid, urlpath, "生成成功",second);
            }
            else
            {
                logUtil.updateState(avo.rid, "", "生成失败",second);
            }
        }


        /// <summary>
        /// 从模板中创建一个新的HSSFWorkbook
        /// </summary> 
        /// <returns></returns>
        public HSSFWorkbook getMobanHSSFWorkbook( )
        {
               
            string templetfilepath = HttpContext.Server.MapPath("/Content") ;
            templetfilepath = Path.Combine(templetfilepath, "tj_moban.xls");
             
            FileStream fileRead = new FileStream(templetfilepath, FileMode.Open, FileAccess.Read);
            //模板
            HSSFWorkbook mobanbook = new HSSFWorkbook(fileRead);
            fileRead.Close();


            return mobanbook;
        }
    }
}