using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TPublish.ClientVsix.Model;
using TPublish.ClientVsix.Setting;

namespace TPublish.ClientVsix.Service
{
    public static class TPublishService
    {
        private static PushPackage GetSettingPackage()
        {
            if (Package.GetGlobalService(typeof(SVsShell)) is IVsShell shell)
            {
                Guid guid = new Guid(PushPackage.PackageGuidString);
                if (ErrorHandler.Succeeded(shell.IsPackageLoaded(ref guid, out var package)))
                {
                    return package as PushPackage;
                }

                if (ErrorHandler.Succeeded(shell.LoadPackage(ref guid, out package)))
                {
                    return package as PushPackage;
                }
            }
            return null;
        }

        public static OptionPageGrid GetSettingPage()
        {
            PushPackage package = GetSettingPackage();
            return package?.GetDialogPage(typeof(OptionPageGrid)) as OptionPageGrid;
        }

        public static ProjModel AnalysisProject(this Project project)
        {
            string projName = project.FullName;
            ProjModel model = new ProjModel
            {
                Key = Guid.NewGuid().ToString(),
                LibName = Path.GetFileNameWithoutExtension(projName),
                PublishDir = new List<string>(),
                LastChooseInfo = new LastChooseInfo(),
                LibDebugPath = string.Empty,
                LibReleasePath = string.Empty,
                ProjType = string.Empty,
            };
            string txt = File.ReadAllText(projName);

            Regex regex = new Regex(@"<PropertyGroup(\s|\S)*?>(\s|\S)*?</PropertyGroup>");
            var regexRes = regex.Matches(txt);
            foreach (Match match in regexRes)
            {
                if (match.Value.Contains("Debug|AnyCPU"))
                {
                    Regex reg = new Regex(@"<OutputPath>(?<Word>(\s|\S)*?)</OutputPath>");
                    var typeMatch = reg.Match(match.Value);
                    if (typeMatch.Success)
                    {
                        model.LibDebugPath = typeMatch.Value.Contains(":")
                            ? typeMatch.Groups["Word"].Value
                            : Path.Combine(Path.GetDirectoryName(projName) ?? string.Empty, typeMatch.Groups["Word"].Value);
                    }
                }
                if (match.Value.Contains("Release|AnyCPU"))
                {
                    Regex reg = new Regex(@"<OutputPath>(?<Word>(\s|\S)*?)</OutputPath>");
                    var typeMatch = reg.Match(match.Value);
                    if (typeMatch.Success)
                    {
                        model.LibReleasePath = typeMatch.Value.Contains(":")
                            ? typeMatch.Groups["Word"].Value
                            : Path.Combine(Path.GetDirectoryName(projName) ?? string.Empty, typeMatch.Groups["Word"].Value);
                    }
                }
                if (match.Value.Contains("OutputType"))
                {
                    Regex reg = new Regex(@"<OutputType>(?<Word>(\s|\S)*?)</OutputType>");
                    var typeMatch = reg.Match(match.Value);
                    if (typeMatch.Success)
                    {
                        model.ProjType = typeMatch.Groups["Word"].Value;
                    }
                }
            }

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
                            if (str != null && !model.PublishDir.Contains(str))
                            {
                                model.PublishDir.Add(str);
                            }
                        }
                    }
                }
            }

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

        public static AppView GetExeAppView(string appName)
        {
            try
            {
                OptionPageGrid setting = GetSettingPage();
                string url = $"{setting.GetApiUrl()}/GetExeAppView?appName={appName}";

                var res = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 3000);

                return res.DeserializeObject<AppView>();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
