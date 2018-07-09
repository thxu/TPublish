using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TPublish.Common;
using TPublish.Common.Model;

namespace TPublish.TestExe
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //var tmp = ChangeAndRestartExeApp("TcpService", @"E:\EXE\GroundingResistance\1.0.0.1");

            //var tmp = GetExeAppView("TcpService");
            //ZipTest1();
            //UploadTest();
            Console.Out.WriteLine("hello");
            Console.ReadKey();

        }

        public static void ZipTest()
        {
            List<string> pathList = new List<string>();
            pathList.Add(@"E:\Code\C#\Git\GroundingResistance\WebApi\WebApi\bin\Release");
            pathList.Add(@"E:\Code\C#\Git\GroundingResistance\WebApi\WebApi\Content");
            pathList.Add(@"E:\Code\C#\Git\GroundingResistance\WebApi\WebApi\Views");
            pathList.Add(@"E:\Code\C#\Git\GroundingResistance\WebApi\WebApi\WCFConfig");
            pathList.Add(@"E:\Code\C#\Git\GroundingResistance\WebApi\WebApi\Web.config");

            var zipRes = ZipHelper.ZipManyFilesOrDictorys(pathList, @"E:\1.zip", @"E:\Code\C#\Git\GroundingResistance\WebApi\WebApi");
        }

        public static void ZipTest1()
        {
            List<string> pathList = new List<string>();
            //pathList.Add(@"E:\Code\C#\Git\GroundingResistance\WebApi\TcpService\bin\Debug");

            DirectoryInfo fileDire = new DirectoryInfo(@"E:\Code\C#\Git\GroundingResistance\WebApi\TcpService\bin\Debug");

            foreach (var directory in fileDire.GetDirectories())
            {
                pathList.Add(directory.FullName);
            }
            foreach (FileInfo file in fileDire.GetFiles("*.*"))
            {
                pathList.Add(file.FullName);
            }

            //var hashtable = new ZipHelper().GetAllFies(@"E:\Code\C#\Git\GroundingResistance\WebApi\TcpService\bin\Debug");
            //foreach (DictionaryEntry entry in hashtable)
            //{
            //    pathList.Add(entry.Key.ToString());
            //}

            var zipRes = ZipHelper.ZipManyFilesOrDictorys(pathList, @"E:\2.zip", @"E:\Code\C#\Git\GroundingResistance\WebApi\TcpService\bin\Debug");
        }

        public static void UnZipTest()
        {
            string zipPath = @"E:\1\1.zip";
            string unZipPath = @"E:\1";

            ZipHelper.UnZip(zipPath, unZipPath);
        }

        public static AppView GetExeAppView(string appName)
        {
            try
            {
                string url = $"http://localhost:8083/ClientApi/GetExeAppView?appName={appName}";
                //string url = $"http://localhost:8083/ClientApi/GetExeAppView";

                var res = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 10000);

                return res.DeserializeObject<AppView>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static void UploadTest()
        {
            string path = @"E:\2.zip";
            string url = "http://localhost:11722/ClientApi/UploadZip";
            //string url = "http://localhost:8083/ClientApi/UploadZip";
            //string url = "http://10.0.0.4:11722/ClientApi/UploadZip";
            //WebClient client = new WebClient();
            //client.UploadFile(url,path);

            NameValueCollection dic = new NameValueCollection();
            dic.Add("Type", "exe");
            dic.Add("AppName", "TcpService");
            var res = Common.Common.HttpPostData(url, 100000, "1.zip", path, dic).DeserializeObject<Result>();
        }

        private static void GetAllIISAppName()
        {
            string url = "http://localhost:11722/ClientApi/GetAllIISAppName";

            WebClient client = new WebClient();
            var res = client.DownloadString(url).DeserializeObject<List<string>>();
        }

        private static void test1()
        {
            string appName = "TcpService.exe";

            XElement element = XElement.Load(@"D:\ProgramFile\ProcessManager\ProcessInfo.xml");
            var progressInfo1 = element.Elements().FirstOrDefault(n => n.Attribute("Name")?.Value == appName);
            progressInfo1.Attribute("Path").Value = @"E:\EXE\GroundingResistance\1.0.0.0";
            element.Save(@"E:\123.xml");
            int a = 1;
        }

        public static Result ChangeAndRestartExeApp(string appName, string newAppPath)
        {
            Result res = new Result();
            try
            {
                // 找到源程序

                var allProcesses = System.Diagnostics.Process.GetProcesses();
                var appProcess = allProcesses.FirstOrDefault(n => n.ProcessName == appName);
                if (appProcess == null)
                {
                    throw new Exception("未找到该进程");
                }

                // 关闭进程守护
                var mgeProcess = allProcesses.FirstOrDefault(n => n.ProcessName == "ProcessManageApplication");
                if (mgeProcess == null)
                {
                    throw new Exception("未找到守护进程");
                }
                string mgeProcessFileName = mgeProcess.MainModule.FileName;
                string processMgeXmlFullName = Path.Combine(Directory.GetParent(mgeProcessFileName).FullName, "ProcessInfo.xml");
                mgeProcess.Kill();

                // 更新版本号
                XElement element = XElement.Load(processMgeXmlFullName);
                var appProcessXml = element.Elements().FirstOrDefault(n => n.Attribute("Name")?.Value == appName + ".exe");
                if (appProcessXml == null)
                {
                    throw new Exception("该进程未纳入到守护进程中，无法自动部署");
                }
                appProcessXml.Attribute("Path").Value = newAppPath;
                element.Save(processMgeXmlFullName);
                // 关闭源程序

                appProcess.Kill();
                // 启动进程守护

                System.Diagnostics.Process.Start(mgeProcessFileName);

            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }
            return res;
        }
    }
}
