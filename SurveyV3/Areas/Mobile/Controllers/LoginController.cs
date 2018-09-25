using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.VO;
using Business.User;
using Business.Sys;
using Newtonsoft.Json;
using SurveyV3.Controllers; 

namespace SurveyV3.Areas.Mobile.Controllers
{
    public class LoginController : BaseController
    {
       
        // GET: Mobile/Login
        BApp appUtil=new BApp();
        BWorker workerUtil=new BWorker();
        BFamily familyUtil=new BFamily();

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult loginCommit()
        {
            string strJson = Request["strJson"];
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(strJson);
            if (dict == null || dict.Count == 0)
            {
                return MobileError("请求数据解析失败");
            }
            string username = dict["username"];
            string password = dict["password"];
            string vid = dict["vid"];
            string _current = dict["current"];
            try
            {
                double current = Double.Parse(_current);
                double tnow = DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds;
                if (Math.Abs(tnow - current) > 86400000)
                {
                    return MobileError("终端时间异常，禁止登录");
                }
            }
            catch (Exception e)
            {
                return MobileError("请求数据解析失败");
            }
            AppVO app=appUtil.getLastObj();
            if(app!=null){
                try
                {
                    int _vid = Int16.Parse(vid);
                    if (_vid < app.least)
                    {
                        return MobileError("当前版本过低，请更新版本");
                    }
                }
                catch (Exception)
                {
                    return MobileError("请求数据解析失败");
                }
              
            }
            WorkerVO wvo=workerUtil.getObjByName(username);
            if(wvo==null){
                  return MobileError("没有这个帐户");
            }else{
                if(wvo.pwd!=password){
                     return MobileError("用户名密码错误");
                }
                if(string.IsNullOrWhiteSpace(wvo.vids)){
                     return MobileError("没有设置调查范围的帐号不能登录");
                }
                if (string.IsNullOrWhiteSpace(wvo.vids))
                {
                    return MobileError("该帐号下的家庭列表为空，不能登录");
                }

                List<FamilyVO> flist=familyUtil.getMobileFamilyList(wvo.vids.Split(','));
                if(flist==null){
                     return MobileError("该帐号下的家庭列表为空，不能登录");
                }
                Dictionary<string,object> sub=new Dictionary<string,object>();

                sub["sid"]=wvo.sid;
                sub["sname"]=wvo.sname;
                sub["family"]=flist;
                return MobileSuccess (sub);
            }
        }

        /// <summary>
        /// 版本检测
        /// </summary>
        /// <returns></returns> 
        public ActionResult versionCheck()
        {
            int vid = getIntParam("vid");
            AppVO app = appUtil.getLastObj();
            if (app != null)
            {

                if (vid < app.current)
                    {
                        return MobileSuccess(app.url);
                    }
                    else
                    {
                        return MobileSuccess("");
                    }
             
            }
            else
            {
                return MobileSuccess("");
            } 
        }
         

        /// <summary>
        /// 日志上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult mobileLog()
        {
            //只管写日志
            getLogPath();
            return MobileSuccess("");
        }
    }
}