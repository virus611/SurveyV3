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
    public class AreaManagerController : BaseController
    {
          BAreaManager amUtil = new BAreaManager();

        // GET: Web/AreaManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult list()
        {
            List<AreaManagerVO> list = amUtil.getList();
            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            } 
        }


        public ActionResult detail()
        {
            string sid = Request["sid"];
            AreaManagerVO obj=amUtil.getObj(sid);
            if (obj == null)
            {
                return WebError("参数错误");
            }
            else
            {
                return WebSuccess(obj);
            } 
        }

          [HttpPost]
        public ActionResult save()
        {
              string sid=Request["sid"];
              string code=Request["code"];
              string name=Request["name"];
              string pwd=Request["pwd"];


              bool flag = amUtil.saveAreaManager(sid, name, pwd, code);
              if (flag)
              {
                  return WebSuccess("操作成功");
              }
              else
              {
                  return WebError("操作失败");
              } 
        }



          public ActionResult del()
          {
              string sid = Request["sid"];
              bool flag = amUtil.delAreaManager(sid);
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