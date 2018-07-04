using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TPublish.ClientVsix.Setting;

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
    }
}
