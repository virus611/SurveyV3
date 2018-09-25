using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyV3.Controllers;
using SurveyV3.Filter;
using Business.User;
using Business.VO;
using Newtonsoft.Json;
using System.Data;

namespace SurveyV3.Areas.Web.Controllers
{
      [SessionFilter]
    public class WorkerController : BaseController
    {
          BWorker workerUtil=new BWorker();
        // GET: Web/Worker


        public ActionResult Index()
        {
            string acode = (string)Session["acode"];
            ViewBag.acode = acode;
            return View();
        }

        public ActionResult workerList()
        {
            string acode = Request["acode"];
            List<WorkerVO> list=workerUtil.getList(acode);
  if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            } 
        }

        public ActionResult workerDel()
        {
            string sid = Request["sid"];
            bool flag = workerUtil.delWorker(sid);
              if (flag)
              {
                  return WebSuccess("删除成功");
              }
              else
              {
                  return WebError("删除失败");
              } 
        }

        public ActionResult workderDetail()
        {
            string sid = Request["sid"];
            WorkerVO obj=workerUtil.getWorker(sid);
            if(obj==null){
                return WebError("错误的请求参数");
            }else{
                return WebSuccess(obj);
            }
        }

          [HttpPost]
        public ActionResult workerSave()
        {
              string sid=Request["sid"];
              string sname=Request["sname"];
              string pwd=Request["pwd"];
              string acode=Request["acode"];

              string msg = workerUtil.saveWorker(sid, sname, pwd, acode);
              if (string.IsNullOrWhiteSpace(msg))
              {
                  return WebSuccess("保存成功");
              }
              else
              {
                  return WebError(msg);
              } 
        }

          [HttpPost]
          public ActionResult workderSaveTree()
          {
              string sid = Request["sid"];
              string ids = Request["ids"];
              string names = Request["names"];

              bool flag = workerUtil.updateVName(sid, ids, names);
              if (flag)
              {
                  return WebSuccess("修改成功");
              }
              else
              {
                  return WebError("修改失败");
              } 
          }


          public ActionResult workderTree()
          {
              string sid = Request["sid"];
              string acode = Request["acode"];
              List<CheckTreeVO> tree = workerUtil.getTreeList(sid, acode);
              return SerializeContent(tree);

          }
    }
}