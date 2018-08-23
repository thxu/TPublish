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
        private static string _mgeProcessFullName = "";
        private static SettingView _setting = null;

        public static string GetMgeProcessFullName()
        {
            try
            {
                // 如果有了就直接返回
                if (!string.IsNullOrWhiteSpace(_mgeProcessFullName))
                {
                    return _mgeProcessFullName;
                }

                // 如果没有则先查询配置文件中保存的信息
                _mgeProcessFullName = SettingLogic.GetMgeProcessFullName();
                if (!string.IsNullOrWhiteSpace(_mgeProcessFullName) && System.IO.File.Exists(_mgeProcessFullName))
                {
                    return _mgeProcessFullName;
                }

                // 配置文件也没有保存则从进程中读取信息

                var allProcesses = Process.GetProcesses();
                var mgeProcess = allProcesses.FirstOrDefault(n => n.ProcessName == "ProcessManageApplication");
                if (mgeProcess == null)
                {
                    throw new Exception("未找到守护进程");
                }
                _mgeProcessFullName = mgeProcess.MainModule.FileName;
                SettingLogic.SetMgeProcessFullName(_mgeProcessFullName);
                return _mgeProcessFullName;

            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "读取进程守护路径异常");
            }

            throw new Exception("未找到守护进程");
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