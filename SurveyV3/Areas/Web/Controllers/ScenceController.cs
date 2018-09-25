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
    /// 场景题
    /// </summary>
    [SessionFilter]
    public class ScenceController : BaseController
    {
        BSceneQuestion mainUtil = new BSceneQuestion();
        BSceneSubQuestion subUtil = new BSceneSubQuestion();

        // GET: Web/Scence


        #region 主界面
        public ActionResult Index()
        {
            return View();
        }

 

        public ActionResult mainList()
        {
            List<SceneVO> list = mainUtil.getList();
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
        public ActionResult mainSave()
        {
            string data = Request["data"];
            List<SceneVO> list = JsonConvert.DeserializeObject<List<SceneVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = mainUtil.save(list);
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
        public ActionResult mainDel()
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
            bool flag = mainUtil.del(qid);
            if (flag)
            {
                return WebSuccess("删除成功");
            }
            else
            {
                return WebError("删除失败");
            }
        }

        #endregion

        #region 子界面
        public ActionResult SubIndex()
        {
            string pid = Request["pid"];
            ViewBag.pid = pid;
            return View();
        }

        public ActionResult subList()
        {
            string pid = Request["pid"];
            List<SceneSubVO> list = subUtil.getList(pid);
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
        public ActionResult subSave()
        {
            string data = Request["data"];
            string pid = Request["pid"];
            List<SceneSubVO> list = JsonConvert.DeserializeObject<List<SceneSubVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = subUtil.save(pid,list);
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
        public ActionResult subDel()
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
            bool flag = subUtil.del(qid);
            if (flag)
            {
                return WebSuccess("删除成功");
            }
            else
            {
                return WebError("删除失败");
            }
        }

        #endregion
    }
}