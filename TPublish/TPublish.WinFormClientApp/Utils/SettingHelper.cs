using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using TPublish.Common;
using TPublish.WinFormClientApp.Model;

namespace TPublish.WinFormClientApp.Utils
{
    public class SettingHelper
    {
        /// <summary>
        /// 获取插件根目录
        /// </summary>
        /// <returns></returns>
        public static string GetPluginConfigPath()
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var folderName = Path.Combine(path, "TPublish");
                if (!string.IsNullOrEmpty(folderName))
                {
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    return folderName;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取日志文件夹
        /// </summary>
        /// <returns></returns>
        public static string GetLogDirPath()
        {
            try
            {
                var path = Path.Combine(GetPluginConfigPath(), "Logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Msbuild的路径
        /// </summary>
        /// <returns></returns>
        public static string GetMsBuildPath()
        {
            try
            {
                var instances = MSBuildLocator.QueryVisualStudioInstances().ToList();
                var instance_laster = instances.OrderByDescending(r => r.Version).FirstOrDefault();
                if (instance_laster != null && !string.IsNullOrEmpty(instance_laster.MSBuildPath))
                {
                    return Path.Combine(instance_laster.MSBuildPath, "MSBuild.exe");
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public static MSettingInfo LoadSettingInfo()
        {
            MSettingInfo res = new MSettingInfo();
            var settingFile = Path.Combine(GetPluginConfigPath(), "Setting.json");
            if (!File.Exists(settingFile))
            {
                res = new MSettingInfo();
            }
            else
            {
                var str = File.ReadAllText(settingFile);
                res = str.DeserializeObject<MSettingInfo>() ?? new MSettingInfo();
            }

            if (string.IsNullOrWhiteSpace(res.MsBuildExePath))
            {
                res.MsBuildExePath = GetMsBuildPath();
            }

            if (res.SelectedItems == null)
            {
                res.SelectedItems = new List<MSelectedItem>();
            }
            if (res.SelectedItems.Count > 20)
            {
                res.SelectedItems = res.SelectedItems.OrderByDescending(n => n.CreateTime).Take(20).ToList();
            }
            return res;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="settingInfo"></param>
        /// <returns></returns>
        public static bool SaveSettingInfo(MSettingInfo settingInfo)
        {
            try
            {
                var settingFile = Path.Combine(GetPluginConfigPath(), "Setting.json");
                using (StreamWriter writer = File.CreateText(settingFile))
                {
                    writer.WriteLine(settingInfo.SerializeObject().FormatJsonString());
                    writer.Flush();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
