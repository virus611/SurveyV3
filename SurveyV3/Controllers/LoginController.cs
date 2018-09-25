using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.ORMModel;
using Business.User;


namespace SurveyV3.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult loginCommit()
        {
            string username = getStringParam("username","");
            string pwd =getStringParam( "pwd","");
            string type = getStringParam("stype", "area");

            if(type == "admin"){
                if(username == "1重庆市1" && pwd == "123321"){
                    Session["sid"]="admin";
                    Session["stype"]="admin";
                    Session["acode"]="admin";
                    return WebSuccess("ok");
                }else{
                    return WebError("用户名密码错误");
                }
            }else if(type=="area"){
                AreaManager obj = new BAreaManager().getAreaManager(username, pwd);
                if (obj == null)
                {
                    return WebError("用户名密码错误");
                }
                else
                { 
                    Session["sid"] = obj.SID;
                    Session["stype"] = "area";
                    Session["acode"] = obj.ACode;
                    return WebSuccess("登录成功");
                } 
            }else{
                   return WebError("用户名密码错误");
            }
             
        }


        /// <summary>
        /// 退出系统
        /// </summary>
        public ActionResult LoginOut()
        {
            Session.Abandon() ;
            return RedirectToAction("Index");
        }
    }
}