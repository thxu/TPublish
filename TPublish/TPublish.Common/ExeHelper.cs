using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TPublish.Common.Model;

namespace TPublish.Common
{
    public static class ExeHelper
    {
        public static Result ExeAppVersionRollBack(this string appId)
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
                string newAppPath = appPath.SubVersion();

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
                res.Message = e.Message;
                TxtLogService.WriteLog(e, "Exe程序版本回退异常，信息：" + appId);
            }
            return res;
        }

        /// <summary>
        /// 获取EXE程序信息
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static List<AppView> GetExeAppInfoByName(this string appName)
        {
            List<AppView> res = new List<AppView>();
            try
            {
                string mgeProcessFileName = SettingLogic.GetMgeProcessFullName();
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                XElement element = XElement.Load(processMgeXmlFullName);
                foreach (XElement processElement in element.Elements().Where(n => n.Attribute("Name").Value.ToLower().StartsWith(appName.ToLower())))
                {
                    AppView view = new AppView
                    {
                        AppName = processElement.Attribute("Name")?.Value ?? string.Empty,
                        Id = processElement.Attribute("ID")?.Value ?? string.Empty,
                        AppPhysicalPath = processElement.Attribute("Path")?.Value ?? string.Empty,
                        AppAlias = processElement.Attribute("Desc")?.Value ?? string.Empty,
                        Status = ((processElement.Attribute("IsDie")?.Value ?? "") == "False") ? 0 : 1,
                    };
                    res.Add(view);
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取EXE程序信息异常，信息：" + appName);
            }
            return res;
        }

        /// <summary>
        /// 获取EXE程序信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AppView GetExeAppInfoById(this string id)
        {
            AppView res = null;
            try
            {
                string mgeProcessFileName = SettingLogic.GetMgeProcessFullName();
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                XElement element = XElement.Load(processMgeXmlFullName);
                XElement processElement = element.Elements().FirstOrDefault(n => n.Attribute("ID")?.Value == id);
                if (processElement == null)
                {
                    return null;
                }
                return new AppView
                {
                    AppName = processElement.Attribute("Name")?.Value ?? string.Empty,
                    Id = processElement.Attribute("ID")?.Value ?? string.Empty,
                    AppPhysicalPath = processElement.Attribute("Path")?.Value ?? string.Empty,
                    AppAlias = processElement.Attribute("Desc")?.Value ?? string.Empty,
                    Status = ((processElement.Attribute("IsDie")?.Value ?? "") == "False") ? 0 : 1,
                };
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取EXE程序信息异常，信息：" + id);
            }
            return res;
        }

        /// <summary>
        /// 获取远程服务器负载EXE程序信息
        /// </summary>
        /// <param name="remoteAppId"></param>
        /// <param name="serAdress">负载服务器地址</param>
        /// <returns></returns>
        public static AppView GetRemoteExeAppInfoById(this string remoteAppId, string serAdress)
        {
            AppView res = new AppView();
            try
            {
                string url = $"{serAdress}/GetExeAppViewByAppId?appId={remoteAppId}";

                var appInfo = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 10000);

                return appInfo.DeserializeObject<AppView>();
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "查询远程服务器EXE负载程序信息异常，信息：" + new { remoteAppId, serAdress }.SerializeObject());
            }
            return res;
        }

        /// <summary>
        /// 获取远程服务器负载EXE程序信息
        /// </summary>
        /// <param name="remoteAppId"></param>
        /// <param name="serAdress">负载服务器地址</param>
        /// <returns></returns>
        public static List<AppView> GetRemoteExeAppInfoByName(this string remoteAppName, string serAdress)
        {
            List<AppView> res = new List<AppView>();
            try
            {
                string url = $"{serAdress}/GetExeAppView?appName={remoteAppName}";

                var appInfo = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 10000);

                return appInfo.DeserializeObject<List<AppView>>();
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "查询远程服务器EXE负载程序信息异常，信息：" + new { remoteAppName, serAdress }.SerializeObject());
            }
            return res;
        }

        /// <summary>
        /// 获取所有EXE程序信息
        /// </summary>
        /// <returns></returns>
        public static List<AppView> GetAllExeAppInfo()
        {
            List<AppView> res = new List<AppView>();
            try
            {
                string mgeProcessFileName = SettingLogic.GetMgeProcessFullName();
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                XElement element = XElement.Load(processMgeXmlFullName);
                foreach (XElement processElement in element.Elements())
                {
                    AppView view = new AppView
                    {
                        AppName = processElement.Attribute("Name")?.Value ?? string.Empty,
                        Id = processElement.Attribute("ID")?.Value ?? string.Empty,
                        AppPhysicalPath = processElement.Attribute("Path")?.Value ?? string.Empty,
                        AppAlias = processElement.Attribute("Desc")?.Value ?? string.Empty,
                        Status = ((processElement.Attribute("IsDie")?.Value ?? "") == "False") ? 0 : 1,
                    };
                    res.Add(view);
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取所有EXE程序信息异常");
            }
            return res;
        }
    }
}
