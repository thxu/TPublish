using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TPublish.Common;
using TPublish.Common.Model;

namespace TPublish.Web.Controllers
{
    public class Service
    {
        /// <summary>
        /// 回退版本（单个程序，异步）
        /// </summary>
        /// <param name="appid">远程服务器上的程序appid</param>
        /// <param name="type">程序类型</param>
        /// <param name="serAdress">远程服务器地址</param>
        /// <returns>回退结果</returns>
        public async Task<Result<string>> RollBackAsync(string appid, string type, string serAdress)
        {
            return await Task.Run((() =>
            {
                Result<string> res = new Result<string>();
                try
                {
                    string url = $"{serAdress}/GetExeAppView?appId={appid}&type={type}";
                    var executeRes = new HttpHelper().HttpGet(url, null, Encoding.UTF8, false, false, 60000).DeserializeObject<Result>();
                    res.IsSucceed = executeRes.IsSucceed;
                    res.Data = $"{appid}-{serAdress}";
                    res.Message = executeRes.Message;
                }
                catch (Exception e)
                {
                    TxtLogService.WriteLog(e, "执行版本回退异常，信息：" + new { appid, type }.SerializeObject());
                }
                finally
                {
                    TxtLogService.SaveLog("GetExeAppView", new { appid, type, serAdress }, res);
                }
                return res;
            }));
        }

        /// <summary>
        /// 部署
        /// </summary>
        /// <param name="appid">应用程序appid</param>
        /// <param name="type">应用程序类型</param>
        /// <param name="serAdress">远程服务地址</param>
        /// <param name="zipFileFullName">压缩包地址</param>
        /// <returns>部署结果</returns>
        public async Task<Result<string>> DeployAsync(string appid, string type, string serAdress, string zipFileFullName)
        {
            return await Task.Run((() =>
            {
                Result<string> res = new Result<string>();
                try
                {
                    string url = $"{serAdress}/UploadZip";
                    FileInfo zipFile = new FileInfo(zipFileFullName);
                    NameValueCollection dic = new NameValueCollection();
                    dic.Add("Type", type);
                    dic.Add("AppId", appid);
                    string uploadResStr = HttpHelper.HttpPostData(url, 30000, zipFile.Name, zipFileFullName, dic);
                    var executeRes = uploadResStr.DeserializeObject<Result>();
                    res.IsSucceed = executeRes.IsSucceed;
                    res.Data = $"{appid}-{serAdress}";
                    res.Message = executeRes.Message;
                }
                catch (Exception e)
                {
                    TxtLogService.WriteLog(e, "执行版本回退异常，信息：" + new { appid, type }.SerializeObject());
                }
                finally
                {
                    TxtLogService.SaveLog("GetExeAppView", new { appid, type, serAdress }, res);
                }
                return res;
            }));
        }

        /// <summary>
        /// 获取app信息
        /// </summary>
        /// <param name="appId">appid</param>
        /// <param name="type">app类型</param>
        /// <returns>app信息</returns>
        public AppView GetAppInfoById(string appId, string type)
        {
            AppView res = new AppView();
            try
            {
                switch (type.ToUpper())
                {
                    case "IIS":
                        return appId.GetIISAppInfoById();
                    case "EXE":
                        return appId.GetExeAppInfoById();
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取app信息异常，信息：" + new { appId, type }.SerializeObject());
            }
            return res;
        }
    }
}