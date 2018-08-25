using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TPublish.Common.Model;

namespace TPublish.Common
{
    public class SettingLogic
    {
        private static SettingView _setting = new SettingView();
        private static string settingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TPublish.Setting");
        private static object _objLock = new object();

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
            lock (_objLock)
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
        }

        public static string GetMgeProcessFullName()
        {
            string res = null;
            try
            {
                res = _setting.MgeProcessFullName.DeepCopy();
                if (string.IsNullOrWhiteSpace(res))
                {
                    // 从进程中读取信息
                    var allProcesses = Process.GetProcesses();
                    var mgeProcess = allProcesses.FirstOrDefault(n => n.ProcessName == "ProcessManageApplication");
                    if (mgeProcess == null)
                    {
                        throw new Exception("未找到守护进程");
                    }
                    res = mgeProcess.MainModule.FileName;
                    SetMgeProcessFullName(res);
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e,"读取进程守护路径异常");
            }
            
            return res;
        }

        public static void SetMgeProcessFullName(string name)
        {
            try
            {
                lock (_objLock)
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
                return _setting.RemoteAppList.DeepCopy();
            }
            return _setting.RemoteAppList.Where(n => n.AppId == appId).ToList();
        }

        public static void SetRemoteAppList(List<AppSerListMap> data)
        {
            try
            {
                lock (_objLock)
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
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存远程服务器信息异常，信息：" + data.SerializeObject());
            }
        }

        public static List<ServiceGroup> GetServiceGroups()
        {
            return _setting.ServiceGroups.DeepCopy();
        }

        public static void SetServiceGroups(List<ServiceGroup> data)
        {
            try
            {
                lock (_objLock)
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
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存服务器组异常，信息：" + data.SerializeObject());
            }
        }

        public static string GetAppZipFilePath(string key)
        {
            if (_setting.AppZipFileMap.ContainsKey(key))
            {
                return _setting.AppZipFileMap[key];
            }

            return null;
        }

        public static void SetAppZipFilePath(string key, string val)
        {
            try
            {
                lock (_objLock)
                {
                    if (_setting.AppZipFileMap.ContainsKey(key))
                    {
                        _setting.AppZipFileMap[key] = val;
                    }
                    else
                    {
                        _setting.AppZipFileMap.Add(key, val);
                    }

                    SettingView view = _setting;
                    if (File.Exists(settingPath))
                    {
                        var str = File.ReadAllText(settingPath);
                        view = str.DeserializeObject<SettingView>() ?? new SettingView();
                        if (_setting.AppZipFileMap.ContainsKey(key))
                        {
                            _setting.AppZipFileMap[key] = val;
                        }
                        else
                        {
                            _setting.AppZipFileMap.Add(key, val);
                        }
                    }

                    using (StreamWriter writer = File.CreateText(settingPath))
                    {
                        writer.WriteLine(view.SerializeObject());
                        writer.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "保存app程序zip文件路径异常，信息：" + new { key, val }.SerializeObject());
            }
        }
    }
}