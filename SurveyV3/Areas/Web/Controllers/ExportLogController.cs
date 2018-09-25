using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.ORMModel; 
using System.IO;
using SurveyV3.Areas.Web.Models;
using System.Threading;
using SurveyV3.Controllers;
using SurveyV3.Filter;
using Business.Respondent;
using Business.VO;
using Newtonsoft.Json;
using System.Data;

namespace SurveyV3.Areas.Web.Controllers
{
      [SessionFilter]
    public class ExportLogController : BaseController
    {
          BExportLog logUtil = new BExportLog();

        // GET: Web/ExportLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult list()
        {
            int page = getIntParam("page");
            int rows = getIntParam("rows");
            string acode = (string)Session["acode"];
            if (acode == "admin")
            {
                acode = "";
            }
            List<ExportLog> list = logUtil.getList(acode, page, rows);

            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                int c=logUtil.getCount(acode);
                return WebSuccessList(c, list);
            }
        }
    }
}