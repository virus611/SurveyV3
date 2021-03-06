﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyV3.Controllers;
using SurveyV3.Filter;
using Business.Question;
using Business.VO;
using Newtonsoft.Json;
using System.Data;
namespace SurveyV3.Areas.Web.Controllers
{
    /// <summary>
    /// 多选题
    /// </summary>
    [SessionFilter]
    public class MutiController : BaseController
    {
        // GET: Web/Muti
        BMutiQuestion questionUtil = new BMutiQuestion();
         
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult mutiList()
        {
            List<MutiVO> list = questionUtil.getList();
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult mutiSave()
        {
            string data = Request["data"];
            List<MutiVO> list = JsonConvert.DeserializeObject<List<MutiVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = questionUtil.save(list);
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult mutiDel()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }

            string qid = Request["qid"];
            if (string.IsNullOrWhiteSpace(qid))
            {
                return WebError("请选择需要删除的记录");
            }
            bool flag = questionUtil.del(qid);
            if (flag)
            {
                return WebSuccess("删除成功");
            }
            else
            {
                return WebError("删除失败");
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult mutiImport()
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


            string errinfo = "";
            bool resualt = false;


            try
            {
                DataTable table = LoadXls.loadXls(fostFile);
                List<MutiVO> qlist = new List<MutiVO>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DateTime now = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(table.Rows[i][0].ToString()))
                    {

                        MutiVO p = new MutiVO();
                        p.qno = table.Rows[i][0].ToString();
                        p.question = table.Rows[i][1].ToString(); 
                        p.a1 = table.Rows[i][2].ToString();
                        p.a2 = table.Rows[i][3].ToString();
                        p.a3 = table.Rows[i][4].ToString();
                        p.a4 = table.Rows[i][5].ToString();
                        p.a5 = table.Rows[i][6].ToString(); 
                        p.img = "";
                        qlist.Add(p);

                    }
                }
                questionUtil.save(qlist);
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
        /// 更新图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public ActionResult mutiUpFile()
        {
            string stype = (string)Session["stype"];
            if ("admin" != stype)
            {
                return WebError("连接超时，请重新登录");
            }

            string qid = Request["qid"];
            if (string.IsNullOrWhiteSpace(qid))
            {
                return WebError("请选择需要操作的记录");
            }
            string url = getAutoZoomFilePath(200, 200);
            if (url == null)
            {
                return WebError("上传的文件不能为空");
            }

            bool flag = questionUtil.updateImg(qid, url);
            if (flag)
            {
                return WebSuccess("操作成功");
            }
            else
            {
                return WebError("操作失败");
            }
        }
    }
}