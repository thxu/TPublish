using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using TPublish.Common;
using TPublish.Common.Model;
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
                res = new MProjectSettingInfo() { ProjectName = projectName };
            }
            else
            {
                var str = File.ReadAllText(settingFile);
                res = str.DeserializeObject<MProjectSettingInfo>() ?? new MProjectSettingInfo() { ProjectName = projectName };
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

        /// <summary>
        /// 解析项目信息
        /// </summary>
        /// <param name="projPath"></param>
        /// <returns></returns>
        public static ProjectModel ParseProject(string projPath)
        {
            try
            {
                ProjectModel res = new ProjectModel()
                {
                    Key = Guid.NewGuid().ToString(),
                    ProjName = Path.GetFileNameWithoutExtension(projPath),
                    ProjPath = projPath,
                    ProjType = 2,
                    OutPutType = string.Empty,
                    IsNetCore = false,
                };

                XDocument doc = XDocument.Load(projPath);
                var rootElement = doc.Root;
                if (rootElement == null)
                {
                    throw new Exception("无法解析此项目文件");
                }

                var sdkInfo = rootElement.Attribute("Sdk");
                res.IsNetCore = sdkInfo?.Value != null;

                if (!res.IsNetCore)
                {
                    var allPropGroups = rootElement.Elements().Where(n => n.Name.LocalName == "PropertyGroup").ToList();
                    if (allPropGroups != null && allPropGroups.Any())
                    {
                        foreach (XElement propGroup in allPropGroups)
                        {
                            var outputTypeElement = propGroup.Elements().FirstOrDefault(n => n.Name.LocalName == "OutputType");
                            if (outputTypeElement != null)
                            {
                                res.OutPutType = outputTypeElement.Value;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    switch (sdkInfo.Value)
                    {
                        case "Microsoft.NET.Sdk":
                        case "Microsoft.NET.Sdk.WindowsDesktop":
                            res.OutPutType = "Exe";
                            break;
                        case "Microsoft.NET.Sdk.Web":
                            res.OutPutType = "Library";
                            break;
                        case "Microsoft.NET.Sdk.Razor":
                        case "Microsoft.NET.Sdk.Worker":
                            break;
                        default:
                            break;
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("项目解析失败，" + ex.Message);
                return new ProjectModel();
            }
        }
    }
}
