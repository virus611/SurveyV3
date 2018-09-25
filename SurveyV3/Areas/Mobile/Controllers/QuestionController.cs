using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.VO;
using Business.Question;
using Business.Respondent;
using Newtonsoft.Json;
using SurveyV3.Controllers;
using Business.Mobile;

namespace SurveyV3.Areas.Mobile.Controllers
{
    public class QuestionController : BaseController
    {
        BUpload uploadUtil = new BUpload();
        // GET: Mobile/Question
         
        public ActionResult info()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            BCheckQuestion checkUtil = new BCheckQuestion();
            List<CheckVO> clist = checkUtil.getList();
            if (clist != null)
            {
                map["check"] = clist;
            }

            BSingleQuestion singUtil = new BSingleQuestion();
            List<SingleVO> slist = singUtil.getList();
            if (slist != null)
            {
                map["single"] = slist;
            }

            BMutiQuestion mutilUtil = new BMutiQuestion();
            List<MutiVO> mlist = mutilUtil.getList();
            if (mlist != null)
            {
                map["muti"] = mlist;
            }

            BSceneSubQuestion scUtil = new BSceneSubQuestion();
            List<SceneSubVO> sclist = scUtil.getDetailList();
            if (scUtil != null)
            {
                map["scene"] = sclist;
            }

            BInfoQuestion infoUtil = new BInfoQuestion();
            List<MInfoVO> flist = infoUtil.getMobileList();
            if (flist != null)
            {
                map["info"] = flist;
            }
            return MobileSuccess(map);
        }

        /// <summary>
        /// 上传答案 
        /// </summary>
        /// <returns></returns>
          [HttpPost]
         public ActionResult upload()
        {
            string strJson = Request["strJson"];
            RspdVO dict = JsonConvert.DeserializeObject<RspdVO>(strJson);
            if (dict == null  )
            {
                return MobileError("请求数据解析失败");
            }
            string rid;
            string msg;

            bool flag = uploadUtil.saveUpload(dict,out rid,out msg);
            if (flag)
            {
                return MobileSuccess(rid);
            }
            else
            {
                return MobileError(msg);
            } 
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <returns></returns>
          [HttpPost]
          public ActionResult updateState()
          {
              string strJson = Request["strJson"];
              Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(strJson);
              if (dict == null || dict.Count == 0)
              {
                  return MobileError("请求数据解析失败");
              }

              string code = dict["code"];
              string result = dict["result"];
              string msg;
              bool flag = uploadUtil.saveState(code, result, out msg);
              if (flag)
              {
                  return MobileSuccess("");
              }
              else
              {
                  return MobileError(msg);
              }
          }

         
 
    }
}