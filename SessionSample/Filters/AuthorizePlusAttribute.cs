using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SessionSample.Filters
{
    public class AuthorizePlusAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //取得 Request 中的 Token 資料
            var token = Convert.ToString(filterContext.HttpContext.Request.Cookies["mvcAuth"].Value);
            if (string.IsNullOrWhiteSpace(token))
            {
                //回傳 401
                base.HandleUnauthorizedRequest(filterContext);
            }
            var loginTime = Convert.ToDateTime(filterContext.HttpContext.Application[token]);

            if (Convert.ToBoolean(filterContext.HttpContext.Session["auth"]) && loginTime > DateTime.UtcNow)
            {
                //驗證成功
            }
            else
            {
                //回傳 401
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}