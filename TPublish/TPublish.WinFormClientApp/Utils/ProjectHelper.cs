using System;
using System.IO;

namespace TPublish.WinFormClientApp.Utils
{
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
