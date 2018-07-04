using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TPublish.ClientVsix.Model;
using TPublish.ClientVsix.Setting;
using TPublish.Common;

namespace TPublish.ClientVsix.Service
{
    public static class TPublishService
    {
        public static PushPackage GetSettingPackage()
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
            };
            string txt = File.ReadAllText(projName);

            Regex regex = new Regex(@"<PropertyGroup(\s|\S)*?>(\s|\S)*?</PropertyGroup>");
            var regexRes = regex.Matches(txt);
            bool canAdd = false;
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
                    if (typeMatch.Success && typeMatch.Groups["Word"].Value == "Library")
                    {
                        model.ProjType = typeMatch.Groups["Word"].Value;
                    }
                }
            }

            return null;
        }

        public static List<string> GetAllIISAppNames()
        {
            //var res  = ThreadHelper.JoinableTaskFactory.Run();

            OptionPageGrid setting = GetSettingPage();
            string url = $"{setting.GetApiUrl()}/ClientApi/GetAllIISAppName";
            WebClient client = new WebClient();
            var res = client.DownloadString(url).DeserializeObject<List<string>>();

            return res;
        }
    }
}
