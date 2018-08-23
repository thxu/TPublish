using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TPublish.Common;
using TPublish.Web.Models;

namespace TPublish.Web.Controllers
{
    public class SettingLogic
    {
        private static SettingView _setting = new SettingView();
        private static string settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TPublish.Setting");

        static SettingLogic()
        {
            try
            {
                if (File.Exists(settingPath))
                {
                    var str = File.ReadAllText(settingPath);
                    _setting = str.DeserializeObject<SettingView>() ?? new SettingView();
                }
                else
                {
                    using (StreamWriter writer = File.CreateText(settingPath))
                    {
                        writer.WriteLine(_setting.SerializeObject());
                        writer.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "读取配置文件异常");
            }
        }

        public static void SaveSetting()
        {
            try
            {
                using (StreamWriter writer = File.CreateText(settingPath))
                {
                    writer.WriteLine(_setting.SerializeObject());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "写入配置文件异常,信息：" + _setting.SerializeObject());
            }
        }

        public static string GetMgeProcessFullName()
        {
            return _setting.MgeProcessFullName;
        }

        public static void SetMgeProcessFullName(string name)
        {
            try
            {
                _setting.MgeProcessFullName = name;

                SettingView view = _setting;
                if (File.Exists(settingPath))
                {
                    var str = File.ReadAllText(settingPath);
                    view = str.DeserializeObject<SettingView>() ?? new SettingView();
                    view.MgeProcessFullName = name;
                }

                using (StreamWriter writer = File.CreateText(settingPath))
                {
                    writer.WriteLine(view.SerializeObject());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存进程守护路径异常，信息：" + name);
            }
        }

        public static List<AppSerListMap> GetRemoteAppList(string appId = null)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                return _setting.RemoteAppList;
            }
            return _setting.RemoteAppList.Where(n => n.AppId == appId).ToList();
        }

        public static void SetRemoteAppList(List<AppSerListMap> data)
        {
            try
            {
                _setting.RemoteAppList = data;

                SettingView view = _setting;
                if (File.Exists(settingPath))
                {
                    var str = File.ReadAllText(settingPath);
                    view = str.DeserializeObject<SettingView>() ?? new SettingView();
                    view.RemoteAppList = data;
                }

                using (StreamWriter writer = File.CreateText(settingPath))
                {
                    writer.WriteLine(view.SerializeObject());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存进程守护路径异常，信息：" + data.SerializeObject());
            }
        }

        public static List<ServiceGroup> GetServiceGroups()
        {
            return _setting.ServiceGroups;
        }

        public static void SetServiceGroups(List<ServiceGroup> data)
        {
            try
            {
                _setting.ServiceGroups = data;

                SettingView view = _setting;
                if (File.Exists(settingPath))
                {
                    var str = File.ReadAllText(settingPath);
                    view = str.DeserializeObject<SettingView>() ?? new SettingView();
                    view.ServiceGroups = data;
                }

                using (StreamWriter writer = File.CreateText(settingPath))
                {
                    writer.WriteLine(view.SerializeObject());
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存进程守护路径异常，信息：" + data.SerializeObject());
            }
        }
    }
}