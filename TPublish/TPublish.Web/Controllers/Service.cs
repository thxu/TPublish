using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPublish.Common;

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
        public  async Task<Result<string>> RollBackAsync(string appid, string type, string serAdress)
        {
            return await Task.Run((() =>
            {
                Thread.Sleep(10000);
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
    }
}