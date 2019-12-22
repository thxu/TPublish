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
                    NetFrameworkVersion = "net45",
                    ProjName = "Go.WeiXinShop.Api",
                    ProjPath = @"E:\Git-10.9\Go.WeiXinShop.Api\Go.WeiXinShop.Api\Go.WeiXinShop.Api\Go.WeiXinShop.Api.csproj",
                    OutPutType = "Library",
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
