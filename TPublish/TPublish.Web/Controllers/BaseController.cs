using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.Web.Models;

namespace TPublish.Web.Controllers
{
    public class BaseController : Controller
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
                SettingView view = GetSettingView();
                _MgeProcessFullName = view?.MgeProcessFullName ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(_MgeProcessFullName) && System.IO.File.Exists(_MgeProcessFullName))
                {
                    return _MgeProcessFullName;
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

        public static void SaveRemotePublishAppSetting(List<KeyValuePair<string, KeyValuePair<string, string>>> list)
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

                view.RemotePublishAppList = list;
                using (StreamWriter writer = System.IO.File.CreateText(settingPath))
                {
                    writer.WriteLine(view.SerializeObject());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存设置信息异常，信息：" + list.SerializeObject());
            }
        }

        public static SettingView GetSettingView()
        {
            SettingView res = null;
            try
            {
                string settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TPublish.Setting");
                if (System.IO.File.Exists(settingPath))
                {
                    var str = System.IO.File.ReadAllText(settingPath);
                    res = str.DeserializeObject<SettingView>();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "读取设置信息异常");
            }

            return res;
        }

        /// <summary>
        /// 获取所有EXE程序信息
        /// </summary>
        /// <returns></returns>
        public static List<AppView> GetAllExeAppView()
        {
            List<AppView> res = new List<AppView>();
            try
            {
                string mgeProcessFileName = GetMgeProcessFullName();
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                XElement element = XElement.Load(processMgeXmlFullName);
                foreach (XElement processElement in element.Elements())
                {
                    AppView view = new AppView
                    {
                        AppName = processElement.Attribute("Desc")?.Value ?? string.Empty,
                        Id = processElement.Attribute("ID")?.Value ?? string.Empty,
                        AppPhysicalPath = processElement.Attribute("Path")?.Value ?? string.Empty,
                        AppAlias = processElement.Attribute("Name")?.Value ?? string.Empty
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