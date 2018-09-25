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
    public class AreaController : BaseController
    {
        BArea areaUtil = new BArea();

        // GET: Web/Area

        #region 页面
        public ActionResult AreaPage()
        {
            return View();
        }

        public ActionResult TownPage()
        {
            string aid = Request["aid"];
            ViewBag.aid = aid;
            return View();
        }

        public ActionResult VillagePage()
        {
            string tid = Request["tid"];
            ViewBag.tid = tid;
            return View();
        }

        #endregion


        #region 查询列表
        public ActionResult areaList()
        {
            List<AreaVO> list = areaUtil.getAreaList();
            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count,list);
            }
        }


        public ActionResult townList()
        {
            string aid = Request["aid"];
            List<TownVO> list = areaUtil.getTownList(aid);
            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            }
        }

        public ActionResult villageList()
        {
            string tid = Request["tid"];
            List<VillageVO> list = areaUtil.getVillageList(tid);
            if (list == null)
            {
                return WebErrorList();
            }
            else
            {
                return WebSuccessList(list.Count, list);
            }
        }
        #endregion


        #region 删除
        public ActionResult areaDel()
        {
            string aid = Request["aid"];
            bool flag = areaUtil.delArea(aid);
            if (flag)
            {
                return WebSuccess("删除成功");
            }
            else
            {
                return WebError("删除失败");
            } 
        }

        public ActionResult townDel()
        {
            string tid = Request["tid"];
            bool flag = areaUtil.delTown(tid);
            if (flag)
            {
                return WebSuccess("删除成功");
            }
            else
            {
                return WebError("删除失败");
            }
        }

        public ActionResult villageDel()
        {
            string vid = Request["vid"];
            bool flag = areaUtil.delVillage(vid);
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


        #region 保存

        public ActionResult areaSave()
        {
            string data = Request["data"];
            List<AreaVO> list = JsonConvert.DeserializeObject<List<AreaVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = areaUtil.saveArea(list);
            if (flag)
            {
                return WebSuccess("保存成功");
            }
            else
            {
                return WebError("保存失败");
            }
        }

        public ActionResult townSave()
        {
            string aid = Request["aid"];
            string data = Request["data"];
            List<TownVO> list = JsonConvert.DeserializeObject<List<TownVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = areaUtil.saveTown(aid,list);
            if (flag)
            {
                return WebSuccess("保存成功");
            }
            else
            {
                return WebError("保存失败");
            }
        }

        public ActionResult villageSave()
        {
            string tid = Request["tid"];
            string data = Request["data"];
            List<VillageVO> list = JsonConvert.DeserializeObject<List<VillageVO>>(data);
            if (list == null || list.Count == 0)
            {
                return WebError("提交数据解析失败");
            }
            bool flag = areaUtil.saveVillage(tid, list);
            if (flag)
            {
                return WebSuccess("保存成功");
            }
            else
            {
                return WebError("保存失败");
            }
        }

        #endregion

        #region 下拉用 

        public ActionResult areaComb()
        {
            List<AreaCombVO> list= areaUtil.getAreaCombList();
            if(list==null){
                return Content("[]");
            }else{
                return SerializeContent(list);
            } 
        }

        #endregion
    }
}