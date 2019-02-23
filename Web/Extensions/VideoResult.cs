using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Web.Extensions
{
    public class VideoResult : FileResult
    {
        public string FileName { get;private set; }
        private readonly string[] _videoTypes = { ".mp4",".webm",".ogg"};

        /// <summary>
        /// 實作 VideoResult 方法，使 Action 可以此方法回傳
        /// </summary>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="contentType">檔案 MIME 類型</param>
        public VideoResult(string fileName,string contentType) : base(contentType)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("檔案沒名稱不給存ㄟ！", "fileName");
            }

            //附檔名確認
            //HostingEnvironment (System.Web.Hosting)
            string filePath = HostingEnvironment.MapPath("~/Videos/" + fileName);
            string fileExtension = Path.GetExtension(fileName).ToLower();
            foreach (string videoType in _videoTypes)
            {
                if (String.Equals(videoType, fileExtension))
                {
                    FileName = fileName;
                }
            }
        }

        /// <summary>
        /// 實作方法將檔案轉為二進位字元，並寫入 HttpResponse 資料流中
        /// </summary>
        /// <param name="response">Http Response</param>
        protected override void WriteFile(HttpResponseBase response)
        {
            string filePaht = HostingEnvironment.MapPath("~/Videos/" + FileName);
            FileInfo file = new FileInfo(filePaht);
            if (file.Exists)
            {
                FileStream stream = file.OpenRead();
                byte[] videoStream = new byte[stream.Length];
                //實際寫入 HttpResponse 資料流中
                response.BinaryWrite(videoStream);
            }
            else
            {
                throw new ArgumentException("檔案不存在！", "fileName");
            }
        }
    }
}