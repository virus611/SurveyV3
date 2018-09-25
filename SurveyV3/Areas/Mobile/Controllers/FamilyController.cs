using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.VO;
using Business.Sys; 
using Newtonsoft.Json;
using SurveyV3.Controllers;
using Business.Mobile;
using Business.User;

namespace SurveyV3.Areas.Mobile.Controllers
{
    public class FamilyController : BaseController
    {
        BFamily familyUtil = new BFamily();
        BWorker workerUtil = new BWorker();
        // GET: Mobile/Family


        /// <summary>
        /// 保存家庭成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult saveFamily()
        {
            string strJson = Request["strJson"];
            SaveFamilyMemberCommitVO commitvo = JsonConvert.DeserializeObject<SaveFamilyMemberCommitVO>(strJson);
            if (commitvo == null  )
            {
                return MobileError("请求数据解析失败");
            }
            string code = commitvo.code;
            List<FamilyMemberVO> list = commitvo.list;
            bool flag = familyUtil.saveMember(code, list);
            if (flag)
            {
                return MobileSuccess("");
            }
            else
            {
                return MobileError("保存失败");
            }
        }
         
        /// <summary>
        /// 读取家庭成员
        /// </summary>
        /// <returns></returns>
        public ActionResult loadFamily()
        {
            string code = Request["code"];
            List<FamilyMemberVO> list =familyUtil.loadMember(code);
            if (list == null)
            {
                list = new List<FamilyMemberVO>();
            }
            return MobileSuccess(list);
        }


        /// <summary>
        /// 读取家庭列表(保留)
        /// </summary>
        /// <returns></returns>
        public ActionResult loadFamilyList()
        {
            string sid = Request["sid"];
            WorkerVO wvo = workerUtil.getWorker(sid);
            if (wvo == null)
            {
                return MobileError("错误的请求参数");
            }
            else
            { 
                if (string.IsNullOrWhiteSpace(wvo.vids))
                {
                    return MobileError("该帐号下的家庭列表为空");
                }

                List<FamilyVO> flist = familyUtil.getMobileFamilyList(wvo.vids.Split(','));
                if (flist == null)
                {
                    return MobileError("该帐号下的家庭列表为空");
                }
                Dictionary<string, object> sub = new Dictionary<string, object>();
                sub["family"] = flist;
                return MobileSuccess(sub);
            }
        }

    }
}