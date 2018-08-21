using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using TPublish.Common;
using TPublish.Web.Models;

namespace TPublish.Web.Controllers
{
    public class BaseController : Controller
    {
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
    }
}