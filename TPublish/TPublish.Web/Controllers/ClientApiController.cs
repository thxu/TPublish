using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.Web.Administration;
using TPublish.Common;
using TPublish.Common.Model;

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
        public string GetAllIISAppView()
        {
            return IISHelper.GetAllIISAppName().SerializeObject();
        }

        public string GetExeAppView(string appName)
        {
            AppView view = new AppView();
            try
            {
                var allProcesses = System.Diagnostics.Process.GetProcesses();
                var appProcess = allProcesses.FirstOrDefault(n => n.ProcessName == appName);
                if (appProcess == null)
                {
                    throw new Exception("未找到该进程");
                }
                string appFullPath = appProcess.MainModule.FileName;
                view.AppName = appName;
                view.AppPhysicalPath = appFullPath;
            }
            catch (Exception e)
            {

            }
            return view.SerializeObject();
        }

        public string UploadZip()
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

                string type = Request["Type"].TrimEnd('\r', '\n');
                string appName = Request["AppName"].TrimEnd('\r', '\n');

                switch (type.ToUpper())
                {
                    case "IIS":
                        {
                            res = DoIIS(appName, fileName, fileInfo);
                            break;
                        }
                    case "EXE":
                        {
                            res = DoExe(appName, fileName, fileInfo);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }

            return res.SerializeObject();
        }

        public Result DoIIS(string appName, string fileName, HttpPostedFileBase fileInfo)
        {
            string newVersionPath = appName.CopyIISAppToNewDir();
            string zipPath = Path.Combine(newVersionPath, fileName);
            fileInfo.SaveAs(zipPath);
            ZipHelper.UnZip(zipPath, Directory.GetParent(zipPath).FullName);
            var changeRes = appName.ChangeIISAppVersion(newVersionPath);
            return changeRes;
        }

        public Result DoExe(string appName, string fileName, HttpPostedFileBase fileInfo)
        {
            Result res = new Result();
            try
            {
                var allProcesses = System.Diagnostics.Process.GetProcesses();
                var appProcess = allProcesses.FirstOrDefault(n => n.ProcessName == appName);
                if (appProcess == null)
                {
                    throw new Exception("未找到该进程");
                }
                string appFullPath = appProcess.MainModule.FileName;
                string appPath = Directory.GetParent(appFullPath).FullName;
                string newAppPath = appPath.AddVersion();
                string zipPath = Path.Combine(newAppPath, fileName);
                appPath.CopyDirectoryTo(newAppPath);
                fileInfo.SaveAs(zipPath);
                ZipHelper.UnZip(zipPath, Directory.GetParent(zipPath).FullName);

                // 关闭进程守护
                var mgeProcess = allProcesses.FirstOrDefault(n => n.ProcessName == "ProcessManageApplication");
                if (mgeProcess == null)
                {
                    throw new Exception("未找到守护进程");
                }
                string mgeProcessFileName = mgeProcess.MainModule.FileName;
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                mgeProcess.Kill();

                // 更新版本号
                XElement element = XElement.Load(processMgeXmlFullName);
                var appProcessXml = element.Elements().FirstOrDefault(n => n.Attribute("Name")?.Value == appName + ".exe");
                if (appProcessXml == null)
                {
                    throw new Exception("该进程未纳入到守护进程中，无法自动部署");
                }
                appProcessXml.Attribute("Path").Value = newAppPath;
                element.Save(processMgeXmlFullName);

                // 关闭源程序
                appProcess.Kill();

                // 启动进程守护
                System.Diagnostics.Process.Start(mgeProcessFileName);

                res.IsSucceed = true;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }

            return res;
        }

        
    }
}