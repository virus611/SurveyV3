using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyV3.Filter
{
    /// <summary>
    /// Session验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string path = "/Login/Index";
            //校验是否登录
            if (HttpContext.Current.Session["acode"] == null || string.IsNullOrWhiteSpace((string)HttpContext.Current.Session["acode"]))
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Write("<script>window.top.location.href='" + path + "'</script>");
                    filterContext.Result = new EmptyResult();
                }
                else
                {
                    //设置报文头自定义属性，前台可以接受。(请求超时)
                    filterContext.HttpContext.Response.Headers.Set("sessionstatus", "filterError");
                    filterContext.HttpContext.Response.Headers.Set("sessionUrl", path);
                    //修改返回的状态码，定义为错误。（执行ajax的error事件）
                    filterContext.HttpContext.Response.StatusCode = 404;
                }
                //中断请求
                filterContext.Result = new EmptyResult();
                return;
            }

        }
    }
}