using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TPublish.Common;
using TPublish.Common.Model;
using TPublish.Web.Models;

namespace TPublish.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 部署

        /// <summary>
        /// 读取程序列表
        /// </summary>
        /// <param name="type">程序类型</param>
        /// <param name="appName">程序名称</param>
        /// <returns>程序列表</returns>
        public ActionResult QueryAllApp(string type, string appName)
        {
            List<AppView> res = new List<AppView>();
            switch (type.ToUpper())
            {
                case "IIS":
                    // iis
                    res = IISHelper.GetAllIISAppName();
                    break;
                case "EXE":
                    // exe
                    res = GetAllExeAppView();
                    break;
                default:
                    break;
            }
            return new MyJsonResult { Data = res.Where(n => n.AppAlias.Contains(appName) || n.AppName.Contains(appName)) };
        }

        /// <summary>
        /// 读取服务器组信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult QuerySerGroupByApp()
        {
            var res = SettingLogic.GetServiceGroups();
            return new MyJsonResult { Data = res };
        }

        public ActionResult BatchDeploy(string appId, string serGroupId)
        {
            Result res = new Result();
            try
            {
                if (string.IsNullOrWhiteSpace(appId))
                {
                    res.Message = "请选择要部署的程序";
                    return new MyJsonResult { Data = res };
                }
                if (string.IsNullOrWhiteSpace(serGroupId))
                {
                    res.Message = "请选择要部署的环境";
                    return new MyJsonResult { Data = res };
                }

                var map = SettingLogic.GetRemoteAppList().FirstOrDefault(n => n.AppId == appId && n.SerGroupId == serGroupId);
                if (map == null || map.ServiceAdressList == null || !map.ServiceAdressList.Any())
                {
                    res.Message = "请先配置服务器信息";
                    return new MyJsonResult { Data = res };
                }
            }
            catch (Exception e)
            {
                res.IsSucceed = false;
                res.Message = "批量部署失败";
                TxtLogService.WriteLog(e, "批量部署异常，信息：" + new { appId, serGroupId }.SerializeObject());
            }

            return new MyJsonResult { Data = res };
        }

        /// <summary>
        /// 批量回退版本
        /// </summary>
        /// <param name="appId">本地程序的appid</param>
        /// <param name="serGroupId">服务器环境组</param>
        /// <returns>回退结果</returns>
        public ActionResult BatchRollBack(string appId, string serGroupId)
        {
            Result<List<Result<string>>> res = new Result<List<Result<string>>>();
            try
            {
                if (string.IsNullOrWhiteSpace(appId))
                {
                    res.Message = "请选择要部署的程序";
                    return new MyJsonResult { Data = res };
                }
                if (string.IsNullOrWhiteSpace(serGroupId))
                {
                    res.Message = "请选择要部署的环境";
                    return new MyJsonResult { Data = res };
                }

                var map = SettingLogic.GetRemoteAppList().FirstOrDefault(n => n.AppId == appId && n.SerGroupId == serGroupId);
                if (map == null || map.ServiceAdressList == null || !map.ServiceAdressList.Any())
                {
                    res.Message = "请先配置服务器信息";
                    return new MyJsonResult { Data = res };
                }

                Task<Result<string>>[] allTasks = new Task<Result<string>>[map.ServiceAdressList.Count];

                for (int i = 0; i < map.ServiceAdressList.Count; i++)
                {
                    var task = new Service().RollBackAsync(map.ServiceAdressList[i].AppId, map.ServiceAdressList[i].AppType, map.ServiceAdressList[i].ServiceAdress);
                    allTasks[i] = task;
                }

                Task.WaitAll(allTasks);
                List<Result<string>> opRes = new List<Result<string>>();
                foreach (var task in allTasks)
                {
                    opRes.Add(task.Result);
                }
                res.IsSucceed = opRes.Exists(n => n.IsSucceed == false);
                res.Message = !res.IsSucceed ? "部分程序版本切换失败" : "";
                res.Data = opRes;
            }
            catch (Exception e)
            {
                res.IsSucceed = false;
                res.Message = "批量部署失败";
                TxtLogService.WriteLog(e, "批量部署异常，信息：" + new { appId, serGroupId }.SerializeObject());
            }

            return new MyJsonResult { Data = res };
        }


        #endregion

        #region 管理



        #endregion
    }
}