using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPublish.Common.Model;
using TPublish.WinFormClientApp.Utils;
using TPublish.WinFormClientApp.WinForms;

namespace TPublish.WinFormClientApp
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
            //MessageBox.Show($"{args.Length},{args[0]},{args[1]}");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 2)
            {
                ProjectModel model = ProjectHelper.ParseProject(args[1]);
                if (model == null)
                {
                    MessageBox.Show("项目解析失败");
                    return;
                }
                model.ProjType = 1;
                //ProjectModel model = new ProjectModel()
                //{
                //    Key = "abed0270-e96c-45b5-b513-785ba08fc0c8",
                //    //NetFrameworkVersion = "net45",
                //    //ProjName = "ConsoleApp1",
                //    //ProjPath = @"E:\Code\C#\Test\Test\ConsoleApp1\ConsoleApp1.csproj",
                //    //NetFrameworkVersion = "netcoreapp2.2",
                //    ProjName = "ConsoleApp3.0_NetCore",
                //    ProjPath = @"E:\Code\C#\Test\Test\ConsoleApp3.0_NetCore\ConsoleApp3.0_NetCore.csproj",
                //    OutPutType = "Library",
                //    ProjType = 0,
                //};
                var form = new DeployForm(model);
                form.Activate();
                Application.Run(form);
            }
            else
            {
                Application.Run(new DeployForm(new ProjectModel() { Key = Guid.NewGuid().ToString() }));
            }
        }
    }
}
