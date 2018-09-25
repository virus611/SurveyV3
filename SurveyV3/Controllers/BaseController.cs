using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters; 
using System.IO;


namespace SurveyV3.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected ViewResult errorView(string msg)
        {

            ViewBag.errmsgs = msg;
            return View("~/Views/error_new.cshtml");
        }

        protected int getIntParam(string key, int defaultValue, int minValue)
        {
            string tmp = (string)Request[key];
            int r = defaultValue;
            if (tmp != null && int.TryParse(tmp, out r))
            {
                if (r < minValue)
                {
                    r = minValue;
                }
            }
            return r;
        }

        protected string getStringParam(string key, string defaultValue)
        {
            string tmp = (string)Request[key];
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            else
            {
                return tmp;
            } 
        }


        protected float getFloatParam(string key)
        {
            string tmp = (string)Request[key];
            float r = 0;
            if (tmp != null && float.TryParse(tmp, out r))
            {
                if (r < 0)
                {
                    r = 0;
                }
            }
            return r;
        }


        protected int getIntParam(string key)
        {
            string tmp = (string)Request[key];
            int r = 1;
            if (tmp != null && int.TryParse(tmp, out r))
            {
                if (r < 1)
                {
                    r = 1;
                }
            }
            return r;
        }

        protected List<string> toArray(string s)
        {
            List<string> lst = new List<string>();
            if (string.IsNullOrWhiteSpace(s))
            {
                return lst;
            }
            string[] tmp = s.Split(',');
            foreach (string m in tmp)
            {
                lst.Add(m);
            }
            return lst;
        }


        #region 序列化返回值封装
        /// <summary>
        /// 返回值处理封装
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ContentResult SerializeContent(object obj)
        {
            return Content(JsonConvert.SerializeObject(obj)); 
        }

        /// <summary>
        /// 接口返回值处理封装(用于处理时间)
        /// </summary>
        /// <returns></returns>
        protected ContentResult SerializeContentDate(object obj, string datetimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            idtc.DateTimeFormat = datetimeFormat;


            return Content(JsonConvert.SerializeObject(obj, idtc));
        }
        #endregion



        protected string getFilePath(int pic_width, int pic_height, int pic_quality)
        {
            try
            {
                HttpPostedFileBase fostFile = Request.Files[0];
                string filename = fostFile.FileName;
                int p = filename.LastIndexOf(".");
                if (p > -1)
                {
                    filename = filename.Substring(p);
                }
                else
                {
                    filename = "";
                }

                filename = Business.Tools.createID() + filename;
                string root = HttpContext.Server.MapPath("/productimg");
                if (System.IO.Directory.Exists(root) == false)
                {
                    System.IO.Directory.CreateDirectory(root);
                }
                string filepath = Path.Combine(root, filename);
                string urlpath = "/productimg/" + filename;
                Image.CutForCustom(fostFile.InputStream, filepath, pic_width, pic_height, pic_quality);
                return urlpath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string getAppPath( )
        {
            try
            {
                HttpPostedFileBase fostFile = Request.Files[0];
                string filename = fostFile.FileName;
                int p = filename.LastIndexOf(".");
                if (p > -1)
                {
                    filename = filename.Substring(p);
                }
                else
                {
                    filename = "";
                }

                filename = Business.Tools.createID() + filename;
                string root = HttpContext.Server.MapPath("/app");
                if (System.IO.Directory.Exists(root) == false)
                {
                    System.IO.Directory.CreateDirectory(root);
                }
                string filepath = Path.Combine(root, filename);
                string urlpath = "/app/" + filename;
                fostFile.SaveAs(filepath); 
                return urlpath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 批量处理
        /// </summary>
        /// <returns></returns>
        protected string getLogPath()
        {
            try
            {
                HttpPostedFileBase fostFile = Request.Files[0];
                string filename = fostFile.FileName;
                int p = filename.LastIndexOf(".");
                if (p > -1)
                {
                    filename = filename.Substring(p);
                }
                else
                {
                    filename = "";
                }

                filename = Business.Tools.createID() + filename;
                string root = HttpContext.Server.MapPath("/log");
                if (System.IO.Directory.Exists(root) == false)
                {
                    System.IO.Directory.CreateDirectory(root);
                }
                string filepath = Path.Combine(root, filename);
                string urlpath = "/log/" + filename;
                fostFile.SaveAs(filepath);
                return urlpath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string getAutoZoomFilePath(int pic_width, int pic_height)
        {
            try
            {
                HttpPostedFileBase fostFile = Request.Files[0];
                string filename = fostFile.FileName;
                int p = filename.LastIndexOf(".");
                if (p > -1)
                {
                    filename = filename.Substring(p);
                }
                else
                {
                    filename = "";
                }

                filename = Business.Tools.createID() + filename;
                string root = HttpContext.Server.MapPath("/productimg");
                if (System.IO.Directory.Exists(root) == false)
                {
                    System.IO.Directory.CreateDirectory(root);
                }
                string filepath = Path.Combine(HttpContext.Server.MapPath("/productimg"), filename);
                string urlpath = "/productimg/" + filename;
                Image.ZoomAuto(fostFile.InputStream, filepath, pic_width, pic_height, "", "");

                return urlpath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region web端返回
        protected ContentResult WebError(string errmsg)
        {
            return Content(JsonConvert.SerializeObject(new { msg = errmsg })); 
        }

         protected ContentResult WebErrorList( )
        {
            return Content(JsonConvert.SerializeObject( new { total = 0 ,rows=new string[]{} }) ); 
        }

        protected ContentResult WebSuccess(object obj = null)
        {
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            if (obj == null)
            {
                var tmp = new {   msg = "" };
                return Content(JsonConvert.SerializeObject(tmp, idtc));
            }
            else
            {
                var tmp = new {  msg = "", data = obj };
                return Content(JsonConvert.SerializeObject(tmp, idtc));
            } 
        }

        protected ContentResult WebSuccessList(int total, object obj)
        {
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
             
            var tmp = new { total = total, rows = obj };
            return Content(JsonConvert.SerializeObject(tmp, idtc)); 
        }


     

        #endregion

        #region mobile端返回
         protected ContentResult MobileError(string errmsg, int code = 0)
        {
            return Content(JsonConvert.SerializeObject(new { code = code, msg = errmsg }));

        }

        protected ContentResult MobileSuccess(object obj = null)
        {
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            if (obj == null)
            {
                var tmp = new { code = 1, msg = "" };
                return Content(JsonConvert.SerializeObject(tmp, idtc));
            }
            else
            {
                var tmp = new { code = 1, msg = "", data = obj };
                return Content(JsonConvert.SerializeObject(tmp, idtc));
            }

        }
 
        #endregion
    }
}