using System;
using System.Collections.Generic;
using System.IO;
using TPublish.Common;
using TPublish.WinFormClientApp.Model;

namespace TPublish.WinFormClientApp.Utils
{
    public class ProjectHelper
    {
        /// <summary>
        /// 获取编译生成后的文件存放的路径
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetBuildToPath(string projectName)
        {
            try
            {
                var projPath = GetProjPath(projectName);
                if (string.IsNullOrWhiteSpace(projPath))
                {
                    return string.Empty;
                }
                var projFilePath = Path.Combine(projPath, $"{projectName}_Files");
                if (!Directory.Exists(projFilePath))
                {
                    Directory.CreateDirectory(projFilePath);
                }
                return projFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取压缩包存放路径
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetZipPath(string projectName)
        {
            try
            {
                var projPath = GetProjPath(projectName);
                if (string.IsNullOrWhiteSpace(projPath))
                {
                    return string.Empty;
                }
                var projFilePath = Path.Combine(projPath, $"{projectName}.zip");
                return projFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取项目配置信息存放路径
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetProjConfigPath(string projectName)
        {
            try
            {
                var projPath = GetProjPath(projectName);
                if (string.IsNullOrWhiteSpace(projPath))
                {
                    return string.Empty;
                }
                var projConfigPath = Path.Combine(projPath, $"{projectName}.json");
                return projConfigPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取项目文件夹路径
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetProjPath(string projectName)
        {
            try
            {
                var pluginConfigPath = SettingHelper.GetPluginConfigPath();
                if (string.IsNullOrWhiteSpace(pluginConfigPath))
                {
                    return string.Empty;
                }
                var projPath = Path.Combine(pluginConfigPath, projectName);
                if (!string.IsNullOrEmpty(projPath))
                {
                    if (!Directory.Exists(projPath))
                    {
                        Directory.CreateDirectory(projPath);
                    }
                    return projPath;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 加载项目设置文件
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static MProjectSettingInfo LoadProjectSettingInfo(string projectName)
        {
            MProjectSettingInfo res = new MProjectSettingInfo();
            var settingFile = GetProjConfigPath(projectName);
            if (!File.Exists(settingFile))
            {
                res = new MProjectSettingInfo();
            }
            else
            {
                var str = File.ReadAllText(settingFile);
                res = str.DeserializeObject<MProjectSettingInfo>() ?? new MProjectSettingInfo();
            }

            if (res.SelectedFiles == null)
            {
                res.SelectedFiles = new List<string>();
            }
            return res;
        }

        /// <summary>
        /// 保存项目设置文件
        /// </summary>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public static bool SaveProjectSettingInfo(MProjectSettingInfo projectInfo)
        {
            try
            {
                var settingFile = GetProjConfigPath(projectInfo.ProjectName);
                using (StreamWriter writer = File.CreateText(settingFile))
                {
                    writer.WriteLine(projectInfo.SerializeObject().FormatJsonString());
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
