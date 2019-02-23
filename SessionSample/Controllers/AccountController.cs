using SessionSample.Filters;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SessionSample.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            //判斷帳號密碼
            if ((account == "Downey" || account == "Shen") && password == "0")
            {
                #region 使用表單驗證方式實作登入機制
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,//·票證版本號
                    account,//--------------------------------------------------------票證名稱(通常是帳號)
                    DateTime.Now,//···················································票證發放時間
                    DateTime.Now.AddDays(1),//----------------------------------------票證有效期限
                    true,//···························································將管理者登入的 Cookie 設定成 Session Cookie
                    "UserDate",//-----------------------------------------------------使用者資料
                    FormsAuthentication.FormsCookiePath);//···························票證在 Cookies 中儲存的位址

                //建立表單驗證使用的票證 Cookie
                string encTicket = FormsAuthentication.Encrypt(ticket);
                var cookieTicket = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
                {
                    HttpOnly = true
                };
                Response.Cookies.Add(cookieTicket);
                #endregion

                #region 使用自訂 Cookie 方式實作登入機制
                //設定 Cookie 名稱
                /*var cookieName = "mvcAuth";
                //判斷是否有已存在相同名稱的 Cookie
                if (Response.Cookies.AllKeys.Contains(cookieName))
                {
                    //若有同名稱的 Cookie 則先移除
                    var cookieVal = Response.Cookies[cookieName].Value;
                    HttpContext.Application.Remove(cookieVal);

                    Response.Cookies.Remove(cookieName);
                }

                //若登入成功則產生一組 Token
                var token = Guid.NewGuid().ToString();

                //將 Token 存放到 Application 中，實作則應該存進資料庫
                HttpContext.Application[token] = DateTime.UtcNow.AddHours(1);

                //將產生的 Token 存入 Cookie 並傳回 Client 端
                var hc = new HttpCookie(cookieName, token)
                {
                    Expires = DateTime.Now.AddDays(1),//設定時效為 1 天
                    HttpOnly = true
                };
                Response.Cookies.Add(hc);*/
                #endregion

                #region 使用自訂 Session 實作登入機制
                //加上 Session
                //Session["auth"] = true;
                #endregion
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
            }
            return View(); ;
        }

        public ActionResult Logout()
        {
            //移除票證 Cookie
            FormsAuthentication.SignOut();

            //移除 Session
            //Session["auth"] = false;
            return RedirectToAction("Login");
        }

        [AuthorizePlus]
        public ActionResult Backend()
        {
            return View();
        }

        
        public ActionResult TicketBackend()
        {
            return Content("登入成功");
        }
    }
}