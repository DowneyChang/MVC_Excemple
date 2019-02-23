using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Web.Controllers
{
    public class FilterController : Controller
    {
        // GET: Filters
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Demo OutputCache 屬性
        /// 可使輸出結果存至快取，以供之後同樣要求(Request)時能更快存取；並設定秒數使快取失效
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 10)]
        public ActionResult GetCacheTime()
        {
            ViewBag.Time = DateTime.Now;
            Thread.Sleep(2000);
            return View();
        }

        #region Demo OutputCache With ChildActionOnly
        [OutputCache(NoStore = true,Location = OutputCacheLocation.None)]
        public ActionResult GetTime()
        {
            ViewBag.Time = DateTime.Now;
            return View();
        }

        [ChildActionOnly]
        [OutputCache(Duration = 10/*,Location = OutputCacheLocation.Any*/)]
        public ActionResult GetCacheTimeForChildAction()
        {
            ViewBag.Time = DateTime.Now;
            return View();
        }
        #endregion

        /// <summary>
        /// Demo HandelError
        /// </summary>
        /// <returns></returns>
        [HandleError(View = "Error",ExceptionType = typeof(Exception))]
        public ActionResult DemoHandelError()
        {
            throw new Exception("測試 Error。");
            return View();
        }

        public ActionResult DemoException()
        {
            throw new Exception("測試例外狀況");
            return View();
        }
    }
}