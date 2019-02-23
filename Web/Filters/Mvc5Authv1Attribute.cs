using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Web.Filters
{
    public class Mvc5Authv1Attribute : ActionFilterAttribute, IAuthenticationFilter, IOverrideFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public Type FiltersToOverride => typeof(IAuthenticationFilter);
    }
    public class Mvc5Authv2Attribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //測試角色(注！！：角色資料應由當下 Context 取得)
            filterContext.Principal = new GenericPrincipal(filterContext.HttpContext.User.Identity, new[] { "Admin" });
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null ||
                (!user.Identity.IsAuthenticated && !user.IsInRole("Admin")))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}
