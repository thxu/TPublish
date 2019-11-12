using System;
using System.Windows.Forms;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.WinFormClient.WinForms;

namespace TPublish.WinFormClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 1)
            {
                //ProjectModel model = args[1].DeserializeObject<ProjectModel>();
                ProjectModel model = new ProjectModel()
                {
                    Key = "abed0270-e96c-45b5-b513-785ba08fc0c8",
                    MsBuildPath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\MSBuild\\Current\\Bin\\MSBuild.exe",
                    NetFrameworkVersion = "net472",
                    ProjName = "WebApplication1",
                    ProjPath = "E:\\Code\\C#\\Test\\Test\\WebApplication1\\WebApplication1.csproj",
                    ProjType = "Library",
                    PublisherName = "",
                    PublishKey = "",
                    PublishUrl = "",
                };
                Application.Run(new DeployForm(model));
            }
            else
            {
                Application.Run(new DeployForm());
            }
        }
    }
}
