using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SurveyV3.Controllers;
using SurveyV3.Filter;

namespace SurveyV3.Areas.Web.Controllers
{
    [SessionFilter]
    public class MainController : BaseController
    {
        // GET: Web/Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult rightTree()
        {
            string stype = (string)Session["stype"];
            List<TreeVO> re = new List<TreeVO>();
            if(stype=="admin"){
                TreeVO sub1=new TreeVO("问卷管理","");
                sub1.children=createQuestion();
                re.Add(sub1);

                 TreeVO sub2=new TreeVO("平台设置","");
                sub2.children=createPlant();
                re.Add(sub2);

                    TreeVO sub3=new TreeVO("调查结果","");
                sub3.children=createResults(stype);
                re.Add(sub3);
            }
            else if (stype == "area")
            {
                TreeVO sub1 = new TreeVO("家庭和调查人员管理", "");
                sub1.children = createArea();
                re.Add(sub1);

                TreeVO sub2 = new TreeVO("调查结果", "");
                sub2.children = createResults(stype);
                re.Add(sub2);

            } 
            return SerializeContent(re);
        }





        #region 创建菜单树

        private List<TreeVO> createQuestion()
        {
            List<TreeVO> re = new List<TreeVO>();
            re.Add(new TreeVO("判断题管理", "/Web/Check/Index"));
            re.Add(new TreeVO("单选题管理", "/Web/Single/Index"));
            re.Add(new TreeVO("多选题管理", "/Web/Muti/Index"));
            re.Add(new TreeVO("情景题管理", "/Web/Scence/Index"));
            re.Add(new TreeVO("基本信息题管理", "/Web/QInfo/Index"));
            return re;
        }

        private List<TreeVO> createPlant()
        { 
            List<TreeVO> re = new List<TreeVO>();
            re.Add(new TreeVO("区域设置", "/Web/Area/AreaPage"));
            re.Add(new TreeVO("区域帐号管理", "/Web/AreaManager"));
            re.Add(new TreeVO("家庭信息查看", "/Web/Family/Index"));
            re.Add(new TreeVO("Pad版本管理", "/Web/App/Index")); 
            return re;
        }

        private List<TreeVO> createArea()
        { 
            List<TreeVO> re = new List<TreeVO>();
            re.Add(new TreeVO("调查人员管理", "/Web/Worker"));
            re.Add(new TreeVO("家庭信息查看", "/Web/Family/AreaIndex")); 
            return re;
        }

        private List<TreeVO> createResults(string stype)
        {
            List<TreeVO> re = new List<TreeVO>();
            if(stype == "area"){
                re.Add(new TreeVO("查看调查结果", "/Web/Respondent/AreaIndex"));
            }else{
                re.Add(new TreeVO("查看调查结果", "/Web/Respondent/Index"));
            }
            if(stype !="area"){
                re.Add(new TreeVO("导出历史", "/Web/ExportLog/Index"));
            } 
            return re;
        }

        class TreeVO
        {
            public TreeVO(string _text, string _url)
            {
                text = _text;
                url = _url;
            }

            public string url { get; set; }
           public string text { get; set; }

           public List<TreeVO> children { get; set; }
        }

        #endregion
    }
}