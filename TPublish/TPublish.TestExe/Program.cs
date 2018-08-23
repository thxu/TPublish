using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
            //ZipTest();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            //var tmp = ChangeAndRestartExeApp("TcpService", @"E:\EXE\GroundingResistance\1.0.0.1");

            //var tmp = GetExeAppView("TcpService");
            //ZipTest1();
            UploadTest();
            Console.Out.WriteLine("hello");
            Console.ReadKey();

        }

        public static void ZipTest()
        {
            Stopwatch stopwatch = new Stopwatch();

            List<string> pathList = new List<string>
            {
            @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\csc.exe",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\Microsoft.Build.Tasks.CodeAnalysis.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\Microsoft.CodeAnalysis.CSharp.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\Microsoft.CodeAnalysis.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\Microsoft.CodeAnalysis.VisualBasic.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\Microsoft.CSharp.Core.targets",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\Microsoft.VisualBasic.Core.targets",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\System.Collections.Immutable.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\System.Reflection.Metadata.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\vbc.exe",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\VBCSCompiler.exe",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\roslyn\VBCSCompiler.exe.config",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\WCFConfig\HotHotelService.config",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\WCFConfig\WeChatBizService.config",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\WCFConfig\WeiXinShopUserService.config",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\zh-Hans\System.Net.Http.Formatting.resources.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\zh-Hans\System.Web.Http.resources.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\zh-Hans\System.Web.Http.WebHost.resources.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Antlr3.Runtime.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Antlr3.Runtime.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ApplicationInsights.config",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Chsword.Excel2Object.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\config.json",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\CTRC.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.Foundation.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.Foundation.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.Foundation.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.NetFrameWork.Common.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.NetFrameWork.Enum.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.NetFrameWork.QrCode.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.NetFrameWork.Redis.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.NetFrameWork.WCF.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.NetFrameWork.WeiXin.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.User.Service.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.User.Service.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.User.Service.UserHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.User.Service.UserHelper.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\EastWestWalk.User.Service.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Express.ExpressService.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Express.ExpressService.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Express.ExpressService.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.HotHotel.Service.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.HotHotel.Service.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.HotHotel.Service.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Log.Service.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Log.Service.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Log.Service.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Wallet.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Wallet.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.Wallet.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Common.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Common.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.JavaInterface.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.JavaInterface.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Service.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Service.IService.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Service.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Service.Model.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Service.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.Service.WcfHelper.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.WebApi.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.WebApi.pdb",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WechatBusiness.WeChat.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.OrderService.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.OrderService.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.OrderService.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.ProductService.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.ProductService.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.ProductService.WcfCallHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.UserService.IService.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.UserService.Model.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Go.WeiXinShop.UserService.WcfHelper.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ICSharpCode.SharpZipLib.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.AI.Agent.Intercept.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.AI.DependencyCollector.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.AI.PerfCounterCollector.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.AI.ServerTelemetryChannel.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.AI.Web.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.AI.WindowsServer.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.ApplicationInsights.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Microsoft.Web.Infrastructure.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\Newtonsoft.Json.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\NPOI.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\NPOI.OOXML.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\NPOI.OpenXml4Net.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\NPOI.OpenXmlFormats.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ServiceStack.Common.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ServiceStack.Interfaces.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ServiceStack.Redis.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ServiceStack.Text.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Net.Http.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Net.Http.Formatting.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Cors.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Helpers.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Http.Cors.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Http.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Http.WebHost.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Mvc.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Optimization.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.Razor.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.WebPages.Deployment.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.WebPages.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\System.Web.WebPages.Razor.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\ThoughtWorks.QRCode.dll",
        @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput\bin\WebGrease.dll"
            };

            stopwatch.Start();
            var zipRes = ZipHelper.ZipManyFilesOrDictorys(pathList, @"E:\1.zip", @"F:\Git-10.9\Go.WechatBusiness.WebApi\Go.WechatBusiness.WebApi\bin\Release\PublishOutput");
            stopwatch.Stop();
            var tmp = stopwatch.ElapsedMilliseconds;
            var a = 1;
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
            dic.Add("AppId", "20180708235207453");
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
