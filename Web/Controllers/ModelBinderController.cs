using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// 測試 ModelBinder 擴充
    /// </summary>
    public class ModelBinderController : Controller
    {
        // GET: ModelBinder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test(decimal amount)
        {
            //需要先去 Global.asax.cs 新增擴充類別
            return View(amount);
        }
    }
}