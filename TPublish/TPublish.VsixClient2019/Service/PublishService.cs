using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.Build.Locator;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using NuGet.Frameworks;
using TPublish.VsixClient2019.Model;
using TPublish.VsixClient2019.Settings;

namespace TPublish.VsixClient2019.Service
{
    public static class PublishService
    {
        /// <summary>
        /// 解析项目信息
        /// </summary>
        /// <param name="project">项目</param>
        /// <returns>项目信息</returns>
        public static ProjModel AnalysisProject(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string projName = project.FullName;
            ProjModel model = new ProjModel
            {
                Key = Guid.NewGuid().ToString(),
                LibName = Path.GetFileNameWithoutExtension(projName),
                PublishDir = new List<string>(),
                LastChooseInfo = new LastChooseInfo(),
                LibDebugPath = project.GetFullOutputPath(),
                LibReleasePath = string.Empty,
                ProjType = project.Properties.Item("OutputType").Value.ToString(),
                NetFrameworkVersion = NuGetFramework.Parse(project.GetProjectProperty("TargetFrameworkMoniker")).GetShortFolderName(),
                MsBuildPath = GetMsBuildPath(),
                ProjPath = project.GetProjectProperty("TargetFrameworkMoniker"),
            };
            if (string.IsNullOrWhiteSpace(model.MsBuildPath))
            {
                model.MsBuildPath = GetMsBuildPath1();
            }
            model.ProjType = model.ProjType == "2" ? "Library" : "";

            DirectoryInfo dir = new DirectoryInfo(model.LibDebugPath);
            dir = dir.Parent;
            if (dir != null)
            {
                if (model.ProjType == "Library")
                {
                    model.PublishDir.Add(dir.FullName);
                }
                else
                {
                    model.PublishDir.Add(model.LibDebugPath);
                    model.PublishDir.Add(model.LibReleasePath);
                }
                var propDir = dir.GetDirectories("Properties");
                if (propDir.Any())
                {
                    propDir = propDir[0].GetDirectories("PublishProfiles");
                    if (propDir.Any())
                    {
                        var files = propDir[0].GetFiles("*.pubxml");
                        foreach (FileInfo file in files)
                        {
                            var str = GetFilePath(file.FullName, dir.FullName);
                            if (!string.IsNullOrWhiteSpace(str) && !model.PublishDir.Contains(str))
                            {
                                model.PublishDir.Add(str);
                            }
                        }
                    }
                }
            }

            string lastChooseSetting = Path.Combine(model.LibDebugPath, "Publish.setting");
            FileInfo settingFile = new FileInfo(lastChooseSetting);
            if (settingFile.Exists)
            {
                try
                {
                    model.LastChooseInfo = JsonConvert.DeserializeObject<LastChooseInfo>(File.ReadAllText(lastChooseSetting));
                }
                catch (Exception e)
                {
                    model.LastChooseInfo = null;
                }
            }

            return model;
        }

        public static string GetProjectProperty(this Project project, string key)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                return (string)project.Properties.Item(key).Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetMsBuildPath()
        {
            try
            {
                var getmS = Microsoft.Build.Utilities.ToolLocationHelper.GetPathToBuildTools(Microsoft.Build.Utilities.ToolLocationHelper.CurrentToolsVersion);
                return getmS;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取Msbuild的路径
        /// </summary>
        /// <returns></returns>
        public static string GetMsBuildPath1()
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

        private static string GetFilePath(string xmlPath, string basePath)
        {
            string res = null;
            XElement root = XElement.Load(xmlPath);
            var propGroupElement = root.Elements().FirstOrDefault(n => n.Name.LocalName == "PropertyGroup");
            var providerElement = propGroupElement?.Elements().FirstOrDefault(n => n.Name.LocalName == "PublishProvider");
            if (providerElement != null && providerElement.Value == "FileSystem")
            {
                var pathElement = propGroupElement?.Elements().FirstOrDefault(n => n.Name.LocalName == "publishUrl");
                if (pathElement != null)
                {
                    res = !pathElement.Value.Contains(":") ? Path.Combine(basePath, pathElement.Value) : pathElement.Value;
                    DirectoryInfo dir = new DirectoryInfo(res);
                    if (!dir.Exists)
                    {
                        return null;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 获取当前选中项目的输出目录
        /// </summary>
        /// <param name="project">项目信息</param>
        /// <returns>输出路径</returns>
        private static string GetFullOutputPath(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var outputPath = (string)project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value;
            var projFullPath = project.Properties.Item("FullPath").Value?.ToString();
            if (string.IsNullOrWhiteSpace(projFullPath))
            {
                return string.Empty;
            }
            DirectoryInfo projDirectoryInfo = new DirectoryInfo(projFullPath);

            while (outputPath.StartsWith(@"..\"))
            {
                projDirectoryInfo = projDirectoryInfo?.Parent;
                char[] tmp = { '.', '.', '\\' };
                outputPath = outputPath.Substring(3);
            }

            outputPath = outputPath.TrimStart('.', '\\');

            var path = Path.Combine(projDirectoryInfo?.FullName ?? string.Empty, outputPath);
            return path.TrimEnd('\\');
        }

        private static PublishPackage GetSettingPackage()
        {
            if (Package.GetGlobalService(typeof(SVsShell)) is IVsShell shell)
            {
                Guid guid = new Guid(PublishPackage.PackageGuidString);
                if (ErrorHandler.Succeeded(shell.IsPackageLoaded(ref guid, out var package)))
                {
                    return package as PublishPackage;
                }

                if (ErrorHandler.Succeeded(shell.LoadPackage(ref guid, out package)))
                {
                    return package as PublishPackage;
                }
            }
            return null;
        }

        public static OptionPageGrid GetSettingPage()
        {
            PublishPackage package = GetSettingPackage();
            return package?.GetDialogPage(typeof(OptionPageGrid)) as OptionPageGrid;
        }

        public static List<AppView> GetAllIISAppNames()
        {
            try
            {
                OptionPageGrid setting = GetSettingPage();
                string url = $"{setting.GetApiUrl()}/GetAllIISAppView";
                WebClient client = new WebClient();
                var res = client.DownloadString(url).DeserializeObject<List<AppView>>();
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<AppView> GetExeAppView(string appName)
        {
            try
            {
                OptionPageGrid setting = GetSettingPage();
                string url = $"{setting.GetApiUrl()}/GetExeAppView?appName={appName}";

                var res = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 10000);

                return res.DeserializeObject<List<AppView>>();
            }
            catch (Exception e)
            {
                return new List<AppView>();
            }
        }

        public static bool CheckConnection()
        {
            try
            {
                OptionPageGrid setting = GetSettingPage();
                string url = $"{setting.GetApiUrl()}/CheckConnection";
                WebClient client = new WebClient();
                var res = client.DownloadString(url);
                return res == "OK";
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
