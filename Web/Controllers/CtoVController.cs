using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Web.Models;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class CtoVController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        // GET: CtoV
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 使用 ViewData 傳遞資料
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoViewData()
        {
            //Key:          Name
            //Data(Value):  Downey
            ViewData["Name"] = "Downey";
            return View();
        }

        /// <summary>
        /// 使用 ViewBag 傳遞資料
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoViewBag()
        {
            ViewBag.Name = "Downey";
            return View();
        }

        /// <summary>
        /// 使用 ViewData 傳遞 List 型態的資料
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoVDModel()
        {
            ViewData["products"] = db.Products.ToList();
            return View();
        }

        /// <summary>
        /// 使用 ViewData.Model(ViewModel) 傳遞 List 型態的資料
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoStronglytyped()
        {
            return View(db.Products.ToList());
        }

        #region 測試使用 TempData 傳遞資料
        public ActionResult DemoInput()
        {
            return View();
        }

        public ActionResult CheckInput(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                TempData["Error"] = "輸入不得空白";
                return RedirectToAction("DemoInput");
            }
            ViewBag.name = Name;
            return View();
        }
        #endregion

        #region 測試使用個項傳遞物件傳遞資料，在單一 Requse 中會有什麼情況(可搭配中斷點觀察)
        public ActionResult DemoTempDate()
        {
            ViewData["Msg1"] = "VD 資料(ViewDate)";
            ViewBag.Msg2 = "VB 資料(ViewBag)";
            TempData["Msg3"] = "TD 資料(TempDate)";
            return RedirectToAction("Redirect1");
        }

        public ActionResult Redirect1()
        {
            //處理資料
            TempData["Msg4"] = TempData["Msg3"];
            return RedirectToAction("GetRedirectData");
        }

        public ActionResult GetRedirectData()
        {
            return View();
        }
        #endregion

        #region 測試 TempData 傳遞資料(多 Requse)
        public ActionResult DemoTempdataProduct()
        {
            TempData["Products"] = db.Products.ToList();
            return Redirect("DemoTempDataKeep");
        }

        public ActionResult DemoTempDataKeep()
        {            
            return View();
        }
        #endregion

        #region 多 Model 物件傳遞
        public ActionResult DemoInclude()
        {
            var products = db.Products
                .Include(x => x.Category)
                .Include(x => x.Supplier);
            return View();
        }
        #endregion

        #region SelectList 物件傳遞
        public ActionResult DemoSelectList()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }
        #endregion

        #region 多類別(物件)傳遞；比較麻煩的範例
        public ActionResult DemoMultiModelObject()
        {
            ViewBag.Author = "Downey";
            ViewBag.Book = "Book Name書名";
            ViewBag.Product = (from p in db.Products select p).Take(10).ToList();
            ViewBag.Category = (from c in db.Categories select c).Take(10).ToList();
            return View();
        }
        #endregion

        #region 多類別(物件)傳遞；較好的方式
        public ActionResult DemoViewModel()
        {
            return View(new ProductCategoryViewModel() {
                Name = "Downey",
                Book = "Book Name書名",
                Product = (from p in db.Products select p).Take(10).ToList(),
                Category = (from c in db.Categories select c).Take(10).ToList()
            });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}