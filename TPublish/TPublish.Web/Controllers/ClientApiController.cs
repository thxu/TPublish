using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.Web.Models;

namespace TPublish.Web.Controllers
{
    public class ClientApiController : Controller
    {

        private static string _MgeProcessFullName = "";

        public static string GetMgeProcessFullName()
        {
            try
            {
                // 如果有了就直接返回
                if (!string.IsNullOrWhiteSpace(_MgeProcessFullName))
                {
                    return _MgeProcessFullName;
                }

                // 如果没有则先查询配置文件中保存的信息

                string settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TPublish.Setting");
                if (System.IO.File.Exists(settingPath))
                {
                    var str = System.IO.File.ReadAllText(settingPath);
                    SettingView view = str.DeserializeObject<SettingView>();
                    _MgeProcessFullName = view?.MgeProcessFullName ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(_MgeProcessFullName) && System.IO.File.Exists(_MgeProcessFullName))
                    {
                        return _MgeProcessFullName;
                    }
                }


                // 配置文件也没有保存则从进程中读取信息

                var allProcesses = Process.GetProcesses();
                var mgeProcess = allProcesses.FirstOrDefault(n => n.ProcessName == "ProcessManageApplication");
                if (mgeProcess == null)
                {
                    throw new Exception("未找到守护进程");
                }
                _MgeProcessFullName = mgeProcess.MainModule.FileName;
                SetMgeProcessFullName(_MgeProcessFullName);
                return _MgeProcessFullName;

            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "读取进程守护路径异常");
            }

            throw new Exception("未找到守护进程");
        }

        public static void SetMgeProcessFullName(string fullName)
        {
            try
            {
                SettingView view = new SettingView();
                string settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TPublish.Setting");
                if (System.IO.File.Exists(settingPath))
                {
                    var str = System.IO.File.ReadAllText(settingPath);
                    view = str.DeserializeObject<SettingView>() ?? new SettingView();
                }

                view.MgeProcessFullName = fullName;
                using (StreamWriter writer = System.IO.File.CreateText(settingPath))
                {
                    writer.WriteLine(view.SerializeObject());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存进程守护路径异常,信息：" + fullName);
            }
        }

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

        /// <summary>
        /// 获取所有EXE程序信息
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public string GetExeAppView(string appName)
        {
            List<AppView> res = new List<AppView>();
            try
            {
                //var allProcesses = Process.GetProcesses();
                //var mgeProcess = allProcesses.FirstOrDefault(n => n.ProcessName == "ProcessManageApplication");
                //if (mgeProcess == null)
                //{
                //    if (!string.IsNullOrWhiteSpace(MgeProcessFullName))
                //    {
                //        mgeProcess = Process.Start(MgeProcessFullName);
                //    }
                //    else
                //    {
                //        throw new Exception("未找到守护进程");
                //    }
                //}
                //string mgeProcessFileName = mgeProcess.MainModule.FileName;
                //string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                //MgeProcessFullName = processMgeXmlFullName;

                string mgeProcessFileName = GetMgeProcessFullName();
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                XElement element = XElement.Load(processMgeXmlFullName);
                foreach (XElement processElement in element.Elements().Where(n => n.Attribute("Name").Value.StartsWith(appName)))
                {
                    AppView view = new AppView
                    {
                        AppName = processElement.Attribute("Desc")?.Value ?? string.Empty,
                        Id = processElement.Attribute("ID")?.Value ?? string.Empty,
                        AppPhysicalPath = processElement.Attribute("Path")?.Value ?? string.Empty,
                        AppAlias = processElement.Attribute("Desc")?.Value ?? string.Empty
                    };
                    res.Add(view);
                }

                /**********************************************/
                //var allProcesses = Process.GetProcesses();
                //var appProcess = allProcesses.Where(n => String.Equals(n.ProcessName, appName, StringComparison.CurrentCultureIgnoreCase)).ToList();
                //if (!appProcess.Any())
                //{
                //    throw new Exception("未找到该进程");
                //}

                //foreach (Process process in appProcess)
                //{
                //    AppView view = new AppView
                //    {
                //        AppName = appName,
                //        AppPhysicalPath = process.MainModule.FileName,
                //        AppAlias = new DirectoryInfo(process.MainModule.FileName)?.Parent?.Name ?? string.Empty,
                //        Pid = process.Id,
                //    };
                //    res.Add(view);
                //}
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取所有EXE程序信息异常，信息：" + appName);
            }
            return res.SerializeObject();
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
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "上传并自动切版本异常,信息：" + new { type, appId, fileName });
                res.Message = e.Message;
            }

            return res.SerializeObject();
        }

        public Result DoIIS(string appId, string fileName, HttpPostedFileBase fileInfo)
        {
            string newVersionPath = appId.CopyIISAppToNewDir();
            string zipPath = Path.Combine(newVersionPath, fileName);
            fileInfo.SaveAs(zipPath);
            ZipHelper.UnZip(zipPath, Directory.GetParent(zipPath).FullName);
            var changeRes = appId.ChangeIISAppVersion(newVersionPath);
            return changeRes;
        }

        public Result DoExe(string appId, string fileName, HttpPostedFileBase fileInfo)
        {
            Result res = new Result();
            try
            {

                var allProcesses = Process.GetProcesses();

                // 读取进程守护信息
                string mgeProcessFileName = GetMgeProcessFullName();
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
                TxtLogService.WriteLog(e, "切换exe版本异常，信息:" + new { appId, fileName });
                res.Message = e.Message;
            }

            return res;
        }
    }
}