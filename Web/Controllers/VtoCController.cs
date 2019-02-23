using System.Collections.Generic;
using System.Web.Mvc;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class VtoCController : Controller
    {
        // GET: VtoC
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 演示 QueryString 取得資料
        /// 使用網址：/VtoC/DemoQueryString?id=5
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoQueryString()
        {
            ViewBag.id = int.Parse(Request.QueryString["id"]);
            return View();
        }

        /// <summary>
        /// 演示 RouteData：用 Url 傳值(Get)
        /// 使用網址：/VtoC/DemoRouteData/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DemoRouteData(int id)
        {
            ViewBag.id = id;
            return View();
        }

        /// <summary>
        /// 演示 Model Binding：用 From 傳值(Post)
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BasicModelBinding(string name)
        {
            ViewBag.Name = name;
            return View();
        }

        /// <summary>
        /// 使用 Model 傳值
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public ActionResult BasicModelBindingByModel(string name)
        {
            ViewData.Model = name;
            return View();
        }

        /// <summary>
        /// 使用 FormCollection 類別傳遞資料
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult DemoFormCollection(FormCollection form)
        {
            ViewBag.Name = form["name"];
            ViewBag.Age = form["age"];
            return View();
        }

        /// <summary>
        /// 測試自訂類別的 ModelBinding
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public ActionResult PersonModelBinding(Person person)
        {
            return View(person);
        }

        /// <summary>
        /// 測試多個自訂類別的 List<ModelBinding> 傳遞資料
        /// </summary>
        /// <param name="man"></param>
        /// <param name="woman"></param>
        /// <returns></returns>
        public ActionResult MultiPersonModelBinding(Person man,Person woman)
        {
            List<Person> person = new List<Person>
            {
                man,
                woman
            };
            return View(person);
        }

        #region 是用 ViewModel 傳遞多自訂類別資料
        public ActionResult ViewModelBinding()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ViewModelBinding(PersonViewModel person)
        {
            return View("ShowViewModelBinding", person);
        }
        #endregion
    }
}