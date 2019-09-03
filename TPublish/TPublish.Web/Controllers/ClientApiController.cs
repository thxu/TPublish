using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TPublish.Common;
using TPublish.Common.Model;

namespace TPublish.Web.Controllers
{
    public class ClientApiController : BaseController
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
            return IISHelper.GetAllIISAppInfo().SerializeObject();
        }

        /// <summary>
        /// 获取iis应用程序名称
        /// </summary>
        /// <returns>应用程序名称</returns>
        public string GetIISAppViewByAppId(string appId)
        {
            return appId.GetIISAppInfoById().SerializeObject();
        }

        /// <summary>
        /// 获取EXE程序信息
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public string GetExeAppView(string appName)
        {
            return appName.GetExeAppInfoByName().SerializeObject();
        }

        /// <summary>
        /// 获取EXE程序信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public string GetExeAppViewByAppId(string appId)
        {
            return appId.GetExeAppInfoById().SerializeObject();
        }

        /// <summary>
        /// 获取所有EXE程序信息
        /// </summary>
        /// <returns></returns>
        public string GetAllExeAppView()
        {
            return ExeHelper.GetAllExeAppInfo().SerializeObject();
        }

        public string UploadZip()
        {
            string type = string.Empty;
            string appId = string.Empty;
            string fileName = string.Empty;
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
                fileName = Path.GetFileName(fileInfo.FileName);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new Exception("文件名无法读取");
                }

                if (!Request.Form.AllKeys.Contains("Type") || !Request.Form.AllKeys.Contains("AppId"))
                {
                    throw new Exception("缺少参数");
                }

                type = Request["Type"].TrimEnd('\r', '\n');
                appId = Request["AppId"].TrimEnd('\r', '\n');

                switch (type.ToUpper())
                {
                    case "IIS":
                        {
                            res = DoIIS(appId, fileName, fileInfo);
                            break;
                        }
                    case "EXE":
                        {
                            res = DoExe(appId, fileName, fileInfo);
                            break;
                        }
                    default:
                        res.Message = "暂不支持该程序类型";
                        break;
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "上传并自动切版本异常,信息：" + new { type, appId, fileName }.SerializeObject());
                res.Message = e.Message;
            }

            return res.SerializeObject();
        }

        private Result DoIIS(string appId, string fileName, HttpPostedFileBase fileInfo)
        {
            string newVersionPath = appId.CopyIISAppToNewDir();
            string zipPath = Path.Combine(newVersionPath, fileName);
            fileInfo.SaveAs(zipPath);

            // 复制一份压缩包到本网站目录，部署外网的时候就直接从网站目录里面拿取压缩包了。
            string zipBackDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipFiles");
            if (!Directory.Exists(zipBackDir))
            {
                Directory.CreateDirectory(zipBackDir);
            }
            string zipBackPath = Path.Combine(zipBackDir, fileName);
            fileInfo.SaveAs(zipBackPath);
            SettingLogic.SetAppZipFilePath($"IIS-{appId}", zipBackPath);

            ZipHelper.UnZip(zipPath, Directory.GetParent(zipPath).FullName);
            var changeRes = appId.ChangeIISAppVersion(newVersionPath);
            return changeRes;
        }

        private Result DoExe(string appId, string fileName, HttpPostedFileBase fileInfo)
        {
            Result res = new Result();
            try
            {
                var allProcesses = Process.GetProcesses();

                // 读取进程守护信息
                string mgeProcessFileName = SettingLogic.GetMgeProcessFullName();
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                XElement element = XElement.Load(processMgeXmlFullName);
                var appProcessXml = element.Elements().FirstOrDefault(n => n.Attribute("ID")?.Value == appId);
                if (appProcessXml == null)
                {
                    throw new Exception("该进程未纳入到守护进程中，无法自动部署");
                }

                var appProcess = allProcesses.FirstOrDefault(n => n.Id.ToString() == appProcessXml.Attribute("PID").Value);
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

                // 复制一份压缩包到本网站目录，部署外网的时候就直接从网站目录里面拿取压缩包了。
                string zipBackDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipFiles");
                if (!Directory.Exists(zipBackDir))
                {
                    Directory.CreateDirectory(zipBackDir);
                }
                string zipBackPath = Path.Combine(zipBackDir, fileName);
                fileInfo.SaveAs(zipBackPath);
                SettingLogic.SetAppZipFilePath($"EXE-{appId}", zipBackPath);

                ZipHelper.UnZip(zipPath, Directory.GetParent(zipPath).FullName);

                // 关闭进程守护
                var mgeProcess = allProcesses.FirstOrDefault(n => String.Equals(n.ProcessName, "ProcessManageApplication", StringComparison.CurrentCultureIgnoreCase));
                mgeProcess?.Kill();

                // 更新版本号
                appProcessXml.Attribute("Path").Value = newAppPath;
                element.Save(processMgeXmlFullName);

                // 关闭源程序
                appProcess.Kill();

                // 启动进程守护
                Process.Start(mgeProcessFileName);

                res.IsSucceed = true;
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "切换exe版本异常，信息:" + new { appId, fileName }.SerializeObject());
                res.Message = e.Message;
            }

            return res;
        }

        /// <summary>
        /// 版本回退接口
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="type">程序类型</param>
        /// <returns>回退类型</returns>
        public string VersionRollBack(string appId, string type)
        {
            Result res = new Result();
            try
            {
                switch (type.ToUpper())
                {
                    case "IIS":
                        {
                            res = appId.IISAppVersionRollBack();
                            break;
                        }
                    case "EXE":
                        {
                            res = appId.ExeAppVersionRollBack();
                            break;
                        }
                    default:
                        res.Message = "暂不支持该程序类型";
                        break;
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "版本回退异常，信息:" + new { appId, type }.SerializeObject());
            }
            return res.SerializeObject();
        }
    }
}