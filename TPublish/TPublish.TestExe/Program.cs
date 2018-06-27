using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TPublish.Common;

namespace TPublish.TestExe
{
    class Program
    {
        static void Main(string[] args)
        {
            UploadTest();
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

            var zipRes = new ZipHelper().ZipManyFilesOrDictorys(pathList, @"E:\1.zip");
        }

        public static void UnZipTest()
        {
            string zipPath = @"E:\1\1.zip";
            string unZipPath = @"E:\1";

            new ZipHelper().UnZip(zipPath, unZipPath);
        }

        private static void UploadTest()
        {
            string path = @"E:\1.zip";
            string url = "http://localhost:11722/ClientApi/UploadTest";
            //string url = "http://10.0.0.4:11722/ClientApi/UploadTest";
            //WebClient client = new WebClient();
            //client.UploadFile(url,path);

            NameValueCollection dic = new NameValueCollection();
            dic.Add("Type", "iis");
            dic.Add("AppName", "GroundingResistance");
            var res = Common.Common.HttpPostData(url, 100000, "1.zip", path, dic);
        }

        private static void GetAllIISAppName()
        {
            string url = "http://localhost:11722/ClientApi/GetAllIISAppName";

            WebClient client = new WebClient();
            var res = client.DownloadString(url);
        }
    }
}
