using System.Diagnostics;
using System.Web.Mvc;

namespace Web.Filters
{
    public class LogOutputAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext);
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext);
        }

        private void Log(string Method, ControllerContext context) {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            string message = $"{Method} - Controller:{controller}, Action:{action} ";
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}