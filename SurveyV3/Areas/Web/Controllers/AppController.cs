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
     [SessionFilter]
    public class AppController : BaseController
    {
         BApp appUtil = new BApp();


        // GET: Web/App
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult appList()
        {
            List<AppVO> list = appUtil.getList();

            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            }
        }

        [HttpPost]
        public ActionResult appSave()
        {
            string url = getAppPath();
            if (string.IsNullOrWhiteSpace(url))
            {
                return WebError("上传失败");
            }
            int least = getIntParam("least");
            int current = getIntParam("least");
            string name = Request["vname"];

            bool flag = appUtil.addObj(name, least, current, url);
            if (flag)
            {
                return WebSuccess("操作成功");
            }
            else
            {
                return WebError("操作失败");
            }
        }

        public ActionResult appDel()
        {
            string sid = Request["sid"];
            bool flag = appUtil.delObj(sid);
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