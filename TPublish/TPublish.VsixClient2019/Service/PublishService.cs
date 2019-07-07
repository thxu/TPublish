using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TPublish.Common;
using TPublish.VsixClient2019.Model;

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
            };

            string lastChooseSetting = Path.Combine(model.LibDebugPath, "TPublish.setting");
            FileInfo settingFile = new FileInfo(lastChooseSetting);
            if (settingFile.Exists)
            {
                try
                {
                    model.LastChooseInfo = File.ReadAllText(lastChooseSetting).DeserializeObject<LastChooseInfo>();
                }
                catch (Exception e)
                {
                    model.LastChooseInfo = null;
                }
            }

            return model;
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
            return path;
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
    }
}
