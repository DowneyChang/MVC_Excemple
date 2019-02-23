using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using Web.Models;

namespace Web.Controllers
{
    public class ValidationsController : Controller
    {
        /// <summary>
        /// 實作 Remote 驗證屬性
        /// </summary>
        /// <param name="product">要驗證的物件</param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]//取消 Output 快取，防止回傳的 Json 資料被瀏覽器快取
        public ActionResult ProductName([Bind(Include = "ProductName")] Product product)
        {
            string msg = null; //錯誤訊息
            if (!string.IsNullOrEmpty(product.ProductName))
            {
                bool hasData = false;
                using (NorthwindEntities db = new NorthwindEntities())//從資料庫取得資料，實作上此部分應寫在 MVC 的 Model 中
                {
                    List<string> inSqlName = (from P in db.Products
                                              where P.ProductName == product.ProductName
                                              select P.ProductName).ToList();
                    hasData = inSqlName.Count > 0;
                }

                if(hasData)
                {
                    msg = "抱歉這個產品已經登記過了喔";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg = "阿賣這個叫什麼？";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}