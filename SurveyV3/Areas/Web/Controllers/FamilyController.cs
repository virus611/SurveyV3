using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyV3.Controllers;
using SurveyV3.Filter;
using Business.Sys;
using Business.VO;
using Newtonsoft.Json;
using System.Data;

namespace SurveyV3.Areas.Web.Controllers
{
    public class FamilyController : BaseController
    {
        BFamily familyUtil = new BFamily();
        BArea areaUtil = new BArea();
        // GET: Web/Family


        public ActionResult Index()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }
        
            return View();
        }

        /// <summary>
        /// 家庭成员页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberPage()
        {
            string code = Request["code"];
            ViewBag.code = code;
            return View();
        }

        /// <summary>
        /// 查询成员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult memberlist()
        {
            string code = Request["code"];
            DataTable dt = familyUtil.getMemberList(code);
            if (dt == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(dt.Rows.Count,dt);
            } 
        }


        public ActionResult AreaIndex()
        {
            string acode = (string)Session["acode"];
            ViewBag.acode = acode;
            return View();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult familyTownDel()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }
            string ids = Request["ids"];
            if(string.IsNullOrWhiteSpace(ids)){
                return WebSuccess("操作成功");
            }
            List<string> list=new List<string>();
            list.AddRange(ids.Split(','));
            bool flag=familyUtil.delFamily(list);
            if(flag){
                return WebSuccess("操作成功");
            }else{
                return WebError("操作失败");
            } 
        }


        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult familyChange()
        {
   
            string data = Request["data"];
            List<FamilyVO> list = JsonConvert.DeserializeObject<List<FamilyVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = familyUtil.saveFamily(list);
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
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult familyTownImport()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }
           

            HttpPostedFileBase fostFile = Request.Files["file"];
            if (fostFile == null)
            {
                return WebError("上传的文件不能为空");
            }

            string acode = Request["acode"];
            string tcode = Request["tcode"];
            string vcode = Request["vcode"];

            

            string errinfo = "";
            bool resualt = false;


            try
            {
                DataTable table = LoadXls.loadXls(fostFile);
                List<FamilyVO> qlist = new List<FamilyVO>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DateTime now = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(table.Rows[i][0].ToString()))
                    {

                        FamilyVO p = new FamilyVO();
                        p.fcode = table.Rows[i][0].ToString().PadLeft(4, '0');
                        p.name = table.Rows[i][1].ToString();
                        p.address = table.Rows[i][2].ToString();
                        p.kish = table.Rows[i][3].ToString();
  
                        p.acode = acode;
                        p.vcode = vcode;
                        p.tcode = tcode;
                        qlist.Add(p);

                    }
                }
                familyUtil.importFamily(qlist);
                resualt = true;
            }
            catch
            {
                resualt = false;
                errinfo = "导入Excel模板格式错误";
            }
            if (resualt)
            {
                return WebSuccess("操作成功");
            }
            else
            {
                return WebError(errinfo);
            }  
        }


        /// <summary>
        /// 批量重置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult familyResetRids()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }

            string ids = Request["ids"];
            if (string.IsNullOrWhiteSpace(ids))
            {
                return WebSuccess("操作成功");
            }
            List<string> list = new List<string>();
            list.AddRange(ids.Split(','));
            bool flag = familyUtil.resetRids(list);
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
        /// 家庭列表
        /// </summary>
        /// <returns></returns>
        public ActionResult familyTownList()
        {
            string acode = Request["acode"];
            string tcode = Request["tcode"];
            string vcode = Request["vcode"];
            int p = getIntParam("page");
            int row = getIntParam("rows");
            List<FamilyVO> list = familyUtil.getFamilyList(acode, tcode, vcode, p, row);

            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                int cnt = familyUtil.getFamilyListCount(acode, tcode, vcode);
                return WebSuccessList(cnt, list);
            }
        }



        public ActionResult AreaComb()
        {
            List<AreaCombVO> list = areaUtil.getAreaCombList();
            if (list == null)
            {
                list = new List<AreaCombVO>();
            }
            return SerializeContent(list);
        }

        public ActionResult TownComb()
        {
            string aid = Request["aid"];
            List<AreaCombVO> list = areaUtil.getTownCombList(aid);
            if (list == null)
            {
                list = new List<AreaCombVO>();
            }
            return SerializeContent(list);
        }

        public ActionResult TownCombArea()
        {
            string acode = (string)Session["acode"];
            List<AreaCombVO> list = areaUtil.getTownCombListByACode(acode);
            if (list == null)
            {
                list = new List<AreaCombVO>();
            }
            return SerializeContent(list);
        }

        public ActionResult VillageComb()
        {
            string tid = Request["tid"];
            List<AreaCombVO> list = areaUtil.getVillageCombList(tid);
            if (list == null)
            {
                list = new List<AreaCombVO>();
            }
            return SerializeContent(list);
        }
    }
}