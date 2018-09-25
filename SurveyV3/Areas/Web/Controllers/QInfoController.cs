using System;
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
    /// 基础信息题
    /// </summary>
    [SessionFilter]
    public class QInfoController : BaseController
    {
        // GET: Web/QInfo
        BInfoQuestion questionUtil = new BInfoQuestion();
         
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult infoList()
        {
            List<QInfoVO> list = questionUtil.getList();
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
        public ActionResult infoSave()
        {
            string data = Request["data"];
            List<QInfoVO> list = JsonConvert.DeserializeObject<List<QInfoVO>>(data);
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
        public ActionResult infoDel()
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

        
    }
}