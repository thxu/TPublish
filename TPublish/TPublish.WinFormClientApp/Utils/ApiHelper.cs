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
        public static List<AppView> GetAllIISAppNames(ServiceInfo currService)
        {
            try
            {
                string url = $"{currService.GetApiUrl()}/GetAllIISAppView";
                WebClient client = new WebClient();
                var res = client.DownloadString(url).DeserializeObject<List<AppView>>();
                return res;
            }
            catch (Exception)
            {
                return new List<AppView>();
            }
        }

        public static List<AppView> GetExeAppView(ServiceInfo currService, string appName)
        {
            try
            {
                string url = $"{currService.GetApiUrl()}/GetExeAppView?appName={appName}";

                var res = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 10000);

                return res.DeserializeObject<List<AppView>>();
            }
            catch (Exception)
            {
                return new List<AppView>();
            }
        }

        public static bool Connect(MSettingInfo setting)
        {
            try
            {
                var currService = setting.GetCurrServiceInfo();
                string url = $"{currService.GetApiUrl()}/CheckConnection?apiKey=" + currService.ApiKey;
                WebClient client = new WebClient();
                var res = client.DownloadString(url) == "OK";
                return res;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Connect(ServiceInfo currService)
        {
            try
            {
                string url = $"{currService.GetApiUrl()}/CheckConnection?apiKey=" + currService.ApiKey;
                WebClient client = new WebClient();
                var res = client.DownloadString(url) == "OK";
                return res;
            }
            catch (Exception)
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

                string url = $"{setting.GetCurrServiceInfo()?.GetApiUrl()}/UploadZip";
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

        public static Result UploadZipFile(MSettingInfo setting, ServiceInfo serviceInfo, string projType, string appId, string fullZipPath)
        {
            Result res = new Result();
            try
            {
                NameValueCollection dic = new NameValueCollection();
                dic.Add("Type", projType == "Library" ? "iis" : "exe");
                dic.Add("AppId", appId);

                if (serviceInfo == null || serviceInfo.ApiIpAdress.IsNullOrEmpty())
                {
                    serviceInfo = setting.GetCurrServiceInfo();
                }

                string url = $"{serviceInfo?.GetApiUrl()}/UploadZip";
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
