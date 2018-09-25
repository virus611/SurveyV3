using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyV3.Controllers;
using SurveyV3.Filter;
using Business.Respondent;
using Business.VO;
using Newtonsoft.Json;
using System.Data;
using Business.Question;
  

namespace SurveyV3.Areas.Web.Controllers
{
    public class RespondentDetailController : BaseController
    {
        BRspdQuery queryUtil = new BRspdQuery();

        // GET: Web/RespondentDetail
        public ActionResult Index()
        {
            string rid = Request["rid"];
            ViewBag.rid = rid;
            
            BCheckQuestion checkUtil=new BCheckQuestion();
            List<CheckVO> clist    =   checkUtil.getList();
            if(clist!=null){
                ViewBag.check=clist;
            }

            BSingleQuestion singUtil=new BSingleQuestion();
            List<SingleVO> slist=singUtil.getList();
            if(slist!=null){
                ViewBag.single=slist;
            }
             
            BMutiQuestion mutilUtil=new BMutiQuestion();
            List<MutiVO> mlist=mutilUtil.getList();
            if(mlist!=null){
                ViewBag.muti=mlist;
            }
  
            BSceneSubQuestion scUtil=new BSceneSubQuestion();
            List<SceneSubVO> sclist=scUtil.getDetailList();
            if(scUtil!=null){
                ViewBag.scene=sclist;
            }

            BInfoQuestion infoUtil=new BInfoQuestion();
            List<QInfoVO> flist=infoUtil.getList();
            if(flist!=null){
                ViewBag.info=flist;
            } 
            return View();
        }


        public ActionResult detailJson()
        {
            string rid = Request["rid"];

            Dictionary<string, object> obj = queryUtil.getRspdInfoJson(rid);
            if (obj != null)
            {
                return SerializeContent(obj);
            }
            else
            {
                return Content("");
            } 
        }
    }
}