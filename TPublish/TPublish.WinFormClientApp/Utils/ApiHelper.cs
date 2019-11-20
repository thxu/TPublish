using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.WinFormClientApp.Model;

namespace TPublish.WinFormClientApp.Utils
{
    public class ApiHelper
    {
        public static List<AppView> GetAllIISAppNames(MSettingInfo setting)
        {
            try
            {
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

        public static List<AppView> GetExeAppView(MSettingInfo setting, string appName)
        {
            try
            {
                string url = $"{setting.GetApiUrl()}/GetExeAppView?appName={appName}";

                var res = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 10000);

                return res.DeserializeObject<List<AppView>>();
            }
            catch (Exception e)
            {
                return new List<AppView>();
            }
        }

        public static bool Connect(MSettingInfo setting)
        {
            try
            {
                string url = $"{setting.GetApiUrl()}/CheckConnection?apiKey=" + setting.ApiKey;
                WebClient client = new WebClient();
                var res = client.DownloadString(url) == "OK";
                return res;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Result UploadZipFile(MSettingInfo setting, string projType, string appId, string fullZipPath)
        {
            Result res = new Result();
            try
            {
                NameValueCollection dic = new NameValueCollection();
                dic.Add("Type", projType == "Library" ? "iis" : "exe");
                dic.Add("AppId", appId);

                string url = $"{setting.GetApiUrl()}/UploadZip";
                string uploadResStr = HttpHelper.HttpPostData(url, 30000, Path.GetFileName(fullZipPath), fullZipPath, dic);
                var uploadRes = uploadResStr.DeserializeObject<Result>();
                return uploadRes;
            }
            catch (Exception e)
            {
                res.Message = e.Message;
            }
            return res;
        }
    }
}
