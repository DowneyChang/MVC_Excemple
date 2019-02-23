using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Filters
{
    /// <summary>
    /// 實作透過 ExceptionFilter 紀錄 Error Log 並輸出至 .txt 檔案中
    /// </summary>
    public class LogToFileAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(DateTime.Now.ToString());
            // TODO: 以下組織要紀錄的內容
            message.AppendLine(filterContext.Exception.ToString());
            message.AppendLine("=============================================================================================================");
            string filterPath = $"~/App_Data/Log {DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath(filterPath),message.ToString());
        }
    }

    /// <summary>
    /// 實作透過 Filter 紀錄 Info Log 並存入至 DB 中
    /// </summary>
    public class LogToDatabaseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //若用戶已驗證則記錄
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                NorthwindEntities db = new NorthwindEntities();
                ActionLog log = new ActionLog() {
                    UserName = filterContext.HttpContext.User.Identity.Name,
                    ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    ActionName = filterContext.ActionDescriptor.ActionName,
                    IPAddress = filterContext.HttpContext.Request.UserHostAddress,
                    CreateDate = filterContext.HttpContext.Timestamp
                };
                db.ActionLogs.Add(log);
                db.SaveChanges();
                base.OnActionExecuting(filterContext);
            }
        }
    }
}