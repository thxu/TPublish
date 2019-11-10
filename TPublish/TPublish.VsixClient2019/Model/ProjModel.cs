using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TPublish.VsixClient2019.Model
{
    public class ProjModel
    {
        public string Key { get; set; }

        public string ProjPath { get; set; }

        public string ProjType { get; set; }

        public string NetFrameworkVersion { get; set; }

        public string LibName { get; set; }

        public string LibDebugPath { get; set; }

        public string LibReleasePath { get; set; }

        public List<string> PublishDir { get; set; }

        public LastChooseInfo LastChooseInfo { get; set; }

        public string MsBuildPath { get; set; }

        public static List<DirSimpleName> ToDirSimpleNames(List<string> files)
        {
            List<DirSimpleName> res = new List<DirSimpleName>();
            if (files != null && files.Any())
            {
                foreach (string path in files)
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        continue;
                    }
                    DirectoryInfo dir = new DirectoryInfo(path);
                    res.Add(new DirSimpleName
                    {
                        Name = dir.Name,
                        FullName = dir.FullName,
                    });
                }
            }

            return res;
        }

        public bool IsNetCore()
        {
            return this.NetFrameworkVersion.Contains("netcoreapp");
        }
    }

    public class LastChooseInfo
    {
        public string LastChooseAppName { get; set; }

        public string LastChoosePublishDir { get; set; }

        public List<string> LastChoosePublishFiles { get; set; }
    }

    public class DirSimpleName
    {
        public string Name { get; set; }

        public string FullName { get; set; }
    }

    public class ProjectHelper
    {
        public static string GetBuildToPath(string projectName)
        {
            try
            {
                var projPath = GetProjPath(projectName);
                if (string.IsNullOrWhiteSpace(projPath))
                {
                    return string.Empty;
                }
                var projFilePath = Path.Combine(projPath, projectName);
                return projFilePath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

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
            catch (Exception e)
            {
                return string.Empty;
            }
        }

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
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static string GetProjPath(string projectName)
        {
            try
            {
                var pluginConfigPath = GetPluginConfigPath();
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
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
