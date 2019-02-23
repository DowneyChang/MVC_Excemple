using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Web.Models;
using Web.Models.ViewModels;
using Web.Extensions;

namespace Web.Controllers
{
    public class MvcTypeController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: MvcType
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Demo Content Result(不指定內容類型)
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoContent()
        {
            return Content("Test Content by no type.");
        }

        /// <summary>
        /// Demo Content Result(指定內容為 Html 類型)
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoHtmlContent()
        {
            return Content("<h1>Test Html Content</h1><p>測試 Html 類型內容</p>", "text/html");
        }

        /// <summary>
        /// Demo Content Result(指定以 UTF-8 編碼之 html 類型內容)
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoEncodingContent()
        {
            return Content("<h1>Test html content encoding by UTF-8.</h1><p>測試以 UTF-8 編碼之 html 內容</p><p>テストコンテンツ</p>", "text/html", Encoding.UTF8);
        }

        /// <summary>
        /// Demo Content Result(指定內容為 csv 類型)
        /// 要求此 ActionResult 時瀏覽器會提示是否下載檔案
        /// 下載的檔案附檔名改為 .csv 就可以用 Excel 開啟
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoCsvContent()
        {
            return Content("Name,Age\r\nDowney,24\r\n", "text/csv");
        }

        /// <summary>
        /// Demo Content Result(指定內容為 XML 類型)
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoXmlContent()
        {
            string XmlString = "<root><name>Downey Chang</name></root>";
            return Content(XmlString, "text/xml", Encoding.UTF8);
        }

        /// <summary>
        /// Demo ActionResult 轉型失敗預設以 ContentResult 進行處理
        /// 傳回內容須能被 Convert.ToString() 轉換
        /// </summary>
        /// <returns></returns>
        public int ProductsCount()
        {
            return db.Products.Count();
        }

        #region Demo 需要由 Server 設置的 JS
        /// <summary>
        /// 實體頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineGame()
        {
            return View();
        }

        /// <summary>
        /// 被要求的 JS 設置方
        /// </summary>
        /// <returns></returns>
        public ActionResult NextTime()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("var nextTime = '{0}';\r\n", DateTime.UtcNow);
            return JavaScript(sb.ToString());
        }
        #endregion

        #region Demo 使用 Json 格式傳遞資料
        /// <summary>
        /// 傳遞指定類型物件
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoJson1()
        {
            Models.ViewModels.Person person = new Models.ViewModels.Person()
            {
                Name = "Downey",
                Age = 20
            };

            return Json(person, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 傳遞匿名型別類型物件，及測試時間格式
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoJson2()
        {
            var person = new
            {
                Name = "Downey",
                Age = 20,
                Birthday = new DateTime(2099,9,9)
            };
            return Json(person, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Demo 使用 Json 傳遞 Entity 物件
        /// 順便 Demo HttpStatusCode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DemoJsonModel(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Configuration.LazyLoadingEnabled = false;
            Product product = db.Products.Find(id);
            return Json(product);
        }

        /// <summary>
        /// Demo 以 Json 傳到物件到 Controller 是否能正常 Model Binding
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPersonJson(Models.Person[] p)
        {
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// Demo Redirect 重新導向功能類別
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult DemoRedirect(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                string baseUrl = "https://ithelp.ithome.com.tw/announces/";//重新導向網址(這裡導向 IT 鐵人邦的公告xD)
                Uri url = new Uri(baseUrl + param);
                return Redirect(url.ToString()); // 302
                //return RedirectPermanent(url.ToString()); // 301
            }
            else
            {
                return Content("error");
            }
        }

        /// <summary>
        /// Demo 以實體檔案路徑作為來源的 File Result
        /// File Result 類別代表一個可下載的檔案
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoFilePath()
        {
            string path = Server.MapPath(@"~/Content/Site.css");
            return File(path, "text/css");
        }

        #region Demo Upload File
        //上傳需確認伺服器端有此路徑(程式不會自己建立資料夾)
        public ActionResult UploadToDisk()
        {
            ViewBag.Title = "UploadToDisk";
            return View("UploadFile");
        }

        [HttpPost]
        public ActionResult UploadToDisk(IEnumerable<HttpPostedFileBase> file)
        {
            string message = string.Empty;
            if (file.Count() > 0)
            {
                int i = 0;
                foreach (var F in file)
                {
                    message +=  $"<h3>檔案{i}：</h3>";
                    if (F == null)
                    {
                        message += "沒有選到檔案。";
                    }
                    else
                    {
                        if (F.ContentLength > 0)
                        {
                            string FileName = Path.GetFileName(F.FileName);
                            string UploadPath = Path.Combine(Server.MapPath("~/File/"), FileName);
                            F.SaveAs(UploadPath);
                            message += $" 檔案名稱：{FileName}<br /> 上傳檔案類型(MIME)：{F.ContentType}<br /> 檔案大小：{F.ContentLength} Byte<br /> 上傳成功！<hr />";
                        }
                        else
                        {
                            message += "請勿上傳空白檔案。";
                        }
                    }
                    i += 1;
                }
                TempData["Message"] = message;
            }
            else
            {
                TempData["Message"] = "你好像很不會上傳檔案喔？";
            }
            return View("UploadFile");
            //return RedirectToAction("UploadToDisk");
        }

        //上傳到 DB
        public ActionResult UploadToDb()
        {
            ViewBag.Title = "UploadToDb";
            return View("UploadFile");
        }

        [HttpPost]
        public ActionResult UploadToDb(IEnumerable<HttpPostedFileBase> file)
        {
            string message = string.Empty;
            if (file.Count() > 0)
            {
                //保證資料一致性，將所有資料 Add 完以後，再呼叫 db.SaveChanges() 方法
                //若有上傳失敗的檔案則不會叫到此方法
                try
                {
                    int i = 0;
                    foreach (var F in file)
                    {
                        message += $"<h3>檔案{i}：</h3>";
                        if (F == null)
                        {
                            message += "沒有選到檔案。";
                        }
                        else
                        {
                            if (F.ContentLength > 0)
                            {
                                string FileName = Path.GetFileName(F.FileName);
                                int FileLength = F.ContentLength;
                                byte[] buffer = new byte[FileLength];
                                //讀取 Stream，寫入 Buffer
                                F.InputStream.Read(buffer, 0, FileLength);
                                DbFile dbFile = new DbFile()
                                {
                                    Name = FileName,
                                    MimeType = F.ContentType,
                                    Size = FileLength,
                                    Content = buffer
                                };
                                DbFile NewRow = db.DbFiles.Add(dbFile);

                                message += $" 檔案ID：{NewRow.Id}<br /> 檔案名稱：{FileName}<br /> 上傳檔案類型(MIME)：{F.ContentType}<br /> 檔案大小：{FileLength} Byte<br /> 上傳成功！<hr />";
                            }
                            else
                            {
                                message += "請勿上傳空白檔案。";
                            }
                        }
                        i += 1;
                    }
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    message = $"檔案上傳失敗，錯誤訊息：<br />{ex.Message}";
                }
            }
            else
            {
                message = "你好像很不會上傳檔案喔？";
            }
            TempData["Message"] = message;
            return View("UploadFile");
        }
        #endregion

        #region Get File
        /// <summary>
        /// 下載檔案頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult DemoFileContent()
        {
            var AllFile = (from DbFile D in db.DbFiles
                           select D).ToList();

            List<SelectListItem> selectLists = new List<SelectListItem>();
            foreach (var F in AllFile)
            {
                selectLists.Add(new SelectListItem()
                {
                    Text = F.Name,
                    Value = F.Id.ToString()
                });

            }
            //ViewBag.AllFile = AllFile;
            return View(selectLists);
        }

        /// <summary>
        /// 下載 DB 儲存的檔案
        /// </summary>
        /// <param name="FileId">檔案 ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DemoFileContent(int FileId)
        {
            var file = db.DbFiles.Find(FileId);
            if (file == null)
            {
                TempData["Message"] = $"找不到檔愛";
                return View();
            }
            byte[] buffer = file.Content;
            return File(file.Content, file.MimeType, file.Name);
        }

        /// <summary>
        /// 取得(下載或檢視)磁碟中檔案
        /// 預設資料夾為"/MVC_Excemple/Web/File/"
        /// </summary>
        /// <param name="FileName">檔案名稱</param>
        /// <returns></returns>
        public ActionResult GetDeskFile(string FileName)
        {
            FileName = string.IsNullOrEmpty(FileName) ? @"A.jpg" : FileName;
            string Folder = @"File";
            //檔名(FileName)及檔案路徑(Folder)應為有效來源，例如存在資料庫之類的
            string path = Server.MapPath($@"~/{Folder}/{FileName}");
            byte[] b;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryReader binary = new BinaryReader(stream);
                b = binary.ReadBytes((int)stream.Length);
            }
            return File(b, MimeMapping.GetMimeMapping(FileName), FileName);
        }

        /// <summary>
        /// 將 ActionResult GetDeskFile() 當成頁面上圖片的來源
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowImage()
        {
            List<SelectListItem> ImageList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "A.jpg",Value = "A.jpg"},
                new SelectListItem(){ Text = "123.jpg",Value = "123.jpg"},
                new SelectListItem(){ Text = "喵.jpg",Value = "喵.jpg"},
                new SelectListItem(){ Text = "19.gif",Value = "19.gif"},
                new SelectListItem(){ Text = "Test.pdf",Value = "Test.pdf"}
            };
            return View(ImageList);
        }

        /// <summary>
        /// Demo 傳回檔案
        /// 加上 ChildActionOnly 驗證屬性，表示此方法只能被其它方法呼叫，而不能直接以 URL 呼叫
        /// </summary>
        /// <param name="FileName">檔案名稱</param>
        /// <returns>File</returns>
        [ChildActionOnly]
        public ActionResult DemoFileStream(string FileName)
        {
            FileName = string.IsNullOrEmpty(FileName) ? @"A.jpg" : FileName;
            string Folder = @"File";
            //檔名(FileName)及檔案路徑(Folder)應為有效來源，例如存在資料庫之類的
            string path = Server.MapPath($@"~/{Folder}/{FileName}");
            FileStream fileStream = System.IO.File.OpenRead(path);
            return File(fileStream, MimeMapping.GetMimeMapping(FileName));
        }
        #endregion

        #region Test API Request
        public ActionResult TestApi()
        {
            return View();
        }

        /// <summary>
        /// 測試做一個模擬 API
        /// </summary>
        /// <param name="NameList"></param>
        /// <returns></returns>
        public string Api1(List<string> NameList)
        {
            IDictionary<string, List<string>> J = new Dictionary<string, List<string>>();
            int i = 0;
            if(NameList != null)
            {
                foreach (var name in NameList)
                {
                    List<string> val = new List<string>()
                    {
                        i.ToString(),
                        name
                    };
                    J.Add($"Name{i.ToString()}", val);
                    i += 1;
                }
            }
            return JsonConvert.SerializeObject(J);
        }
        #endregion

        public ActionResult DemoActionName()
        {
            ViewBag.ViewName = "這個頁面的名稱是：";
            return View("DemoActionName2");
        }

        public ActionResult DemoMaster()
        {
            return View(null,"_Layout2");
        }

        #region Demo Partial View 
        public ActionResult DemoPartialView()
        {
            return View();
        }

        public ActionResult GetTime()
        {
            Thread.Sleep(2000);
            return PartialView("_GetTimePartial");
        }
        #endregion

        #region Demo Video Result
        public ActionResult DemoVideo()
        {
            //應該從資料來源(Ex.資料庫)取得相關資訊
            string fileName = "test.mp4";
            return new VideoResult(fileName, MimeMapping.GetMimeMapping(fileName));
        }
        
        public ActionResult ShowVideo()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Demo RequireHttps 驗證屬性
        /// </summary>
        /// <returns></returns>
        [RequireHttps]
        public ActionResult Secure()
        {
            return View();
        }
    }
}