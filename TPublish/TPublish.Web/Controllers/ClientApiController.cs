using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Administration;
using TPublish.Common;

namespace TPublish.Web.Controllers
{
    public class ClientApiController : Controller
    {
        public string Index()
        {
            return "hello";
        }

        /// <summary>
        /// 获取所有iis应用程序名称
        /// </summary>
        /// <returns>应用程序名称集合</returns>
        public List<string> GetAllIISAppName()
        {
            return IISHelper.GetAllIISAppName();
        }

        public Result UploadTest()
        {
            Result res = new Result();
            try
            {
                if (Request.Files.Count <= 0)
                {
                    throw new Exception("未接收到文件");
                }
                var fileInfo = Request.Files[0];
                if (fileInfo == null)
                {
                    throw new Exception("文件已损坏");
                }
                var fileName = Path.GetFileName(fileInfo.FileName);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new Exception("文件名无法读取");
                }

                if (!Request.Form.AllKeys.Contains("Type") || !Request.Form.AllKeys.Contains("AppName"))
                {
                    throw new Exception("缺少参数");
                }

                string type = Request["Type"];
                string appName = Request["AppName"];

                switch (type.ToUpper())
                {
                    case "IIS":
                        res = DoIIS(appName, fileName, fileInfo);
                        break;
                    case "EXE":
                        break;
                }


                var allProcesses = System.Diagnostics.Process.GetProcesses();
                
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }

            return res;
        }

        public Result DoIIS(string appName, string fileName, HttpPostedFileBase fileInfo)
        {
            string newVersionPath = appName.CopyIISAppToNewDir();
            string zipPath = Path.Combine(newVersionPath, fileName);
            fileInfo.SaveAs(zipPath);
            new ZipHelper().UnZip(zipPath, Directory.GetParent(zipPath).FullName);
            var changeRes = appName.ChangeIISAppVersion(newVersionPath);
            return changeRes;
        }



    }
}