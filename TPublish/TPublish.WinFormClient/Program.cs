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

            if (args.Length == 2)
            {
                ProjectModel model = args[1].DeserializeObject<ProjectModel>();
                Application.Run(new DeployForm(model));
            }
            else
            {
                Application.Run(new DeployForm());
            }
        }
    }
}
