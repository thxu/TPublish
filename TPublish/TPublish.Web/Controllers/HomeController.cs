using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TPublish.Common;
using TPublish.Common.Model;

namespace TPublish.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Default()
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
                    res = IISHelper.GetAllIISAppInfo();
                    break;
                case "EXE":
                    // exe
                    res = ExeHelper.GetAllExeAppInfo();
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
        public ActionResult QuerySerGroupByApp(string appId, string type)
        {
            var groups = SettingLogic.GetServiceGroups();
            List<AppServiceGroupView> res = new List<AppServiceGroupView>();
            foreach (var item in groups)
            {
                var serList = SettingLogic.GetRemoteAppList(appId).Where(n => n.SerGroupId == item.GroupGuid);
                res.Add(new AppServiceGroupView
                {
                    AppId = appId,
                    GroupName = item.GroupName,
                    Guid = item.GroupName,
                    ServiceCount = serList.Count(),
                });
            }
            return new MyJsonResult { Data = res };
        }

        /// <summary>
        /// 查询指定服务器组所有负载程序信息
        /// </summary>
        /// <param name="appId">本地程序appid</param>
        /// <param name="type">类型</param>
        /// <param name="groupId">服务器组id</param>
        /// <returns>程序信息集合</returns>
        public ActionResult QueryRemoteAppInfoList(string appId, string type, string groupId)
        {
            List<AppSerView> res = new List<AppSerView>();
            try
            {
                var serList = SettingLogic.GetRemoteAppList(appId).FirstOrDefault(n => n.SerGroupId == groupId);
                if (serList != null)
                {
                    foreach (var appSerListMap in serList.ServiceAdressList)
                    {
                        var remoteAppInfo = new Service().GetRemoteAppInfoById(appSerListMap.AppId, appSerListMap.AppType, appSerListMap.ServiceAdress);
                        res.Add(new AppSerView
                        {
                            AppId = appId,
                            AppType = type,
                            GroupGuid = groupId,
                            RemoteAppView = remoteAppInfo,
                            ServiceAdress = appSerListMap.ServiceAdress,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "查询指定服务器组上所有负载程序信息异常，信息：" + new { appId, type, groupId }.SerializeObject());
            }
            return new MyJsonResult { Data = res };
        }

        /// <summary>
        /// 获取远程服务器负载程序信息
        /// </summary>
        /// <param name="remoteAppId">负载程序appid</param>
        /// <param name="type">类型</param>
        /// <param name="serAdress">远程服务器地址</param>
        /// <returns></returns>
        public ActionResult QueryRemoteAppInfoByRemoteAppId(string remoteAppId, string type, string serAdress)
        {
            AppView appInfo = new Service().GetRemoteAppInfoById(remoteAppId, type, serAdress);
            return new MyJsonResult { Data = appInfo };
        }

        public ActionResult BatchDeploy(string appId, string serGroupId)
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
                if (map?.ServiceAdressList == null || !map.ServiceAdressList.Any())
                {
                    res.Message = "请先配置服务器信息";
                    return new MyJsonResult { Data = res };
                }

                // 读取压缩文件路径
                string key = $"{map.AppType}-{map.AppId}";
                var zipFile = SettingLogic.GetAppZipFilePath(key);
                if (string.IsNullOrWhiteSpace(zipFile))
                {
                    var appInfo = new Service().GetAppInfoById(map.AppId, map.AppType);
                    zipFile = appInfo.AppName + ".zip";
                    ZipHelper.ZipDir(appInfo.AppPhysicalPath, zipFile);
                }

                Task<Result<string>>[] allTasks = new Task<Result<string>>[map.ServiceAdressList.Count];
                for (int i = 0; i < map.ServiceAdressList.Count; i++)
                {
                    var task = new Service().DeployAsync(map.ServiceAdressList[i].AppId, map.ServiceAdressList[i].AppType, map.ServiceAdressList[i].ServiceAdress, zipFile);
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
                    res.Message = "请选择要回退的程序";
                    return new MyJsonResult { Data = res };
                }
                if (string.IsNullOrWhiteSpace(serGroupId))
                {
                    res.Message = "请选择要部署的环境";
                    return new MyJsonResult { Data = res };
                }

                var map = SettingLogic.GetRemoteAppList().FirstOrDefault(n => n.AppId == appId && n.SerGroupId == serGroupId);
                if (map?.ServiceAdressList == null || !map.ServiceAdressList.Any())
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
                res.Message = !res.IsSucceed ? "部分程序版本回退失败" : "";
                res.Data = opRes;
            }
            catch (Exception e)
            {
                res.IsSucceed = false;
                res.Message = "批量回退失败";
                TxtLogService.WriteLog(e, "批量回退异常，信息：" + new { appId, serGroupId }.SerializeObject());
            }

            return new MyJsonResult { Data = res };
        }

        /// <summary>
        /// 单个程序回退
        /// </summary>
        /// <param name="remoteAppId">远程服务器上的appid</param>
        /// <param name="type">远程服务器上的type</param>
        /// <param name="serAdress">远程服务器地址</param>
        /// <returns></returns>
        public ActionResult RollBack(string remoteAppId, string type, string serAdress)
        {
            Result<string> res = new Result<string>();
            try
            {
                if (string.IsNullOrWhiteSpace(remoteAppId))
                {
                    res.Message = "请选择要回退的程序";
                    return new MyJsonResult { Data = res };
                }

                var rollbackRes = new Service().RollBackAsync(remoteAppId, type, serAdress).Result;
                res = rollbackRes;
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "回退单个程序异常，信息：" + new { remoteAppId, type, serAdress }.SerializeObject());
            }

            return new MyJsonResult { Data = res };
        }


        #endregion

        #region 管理
        /// <summary>
        /// 查询远程服务器上的应用程序信息
        /// </summary>
        /// <param name="appName">app程序名称</param>
        /// <param name="type">类型</param>
        /// <param name="serAdress">远程服务器地址</param>
        /// <returns>应用程序信息</returns>
        public ActionResult QueryRemoteAppInfoByName(string appName, string type, string serAdress)
        {
            List<AppView> res = new List<AppView>();
            switch (type.ToUpper())
            {
                case "IIS":
                    res = IISHelper.GetAllRemoteIISAppInfo(serAdress);
                    break;
                case "EXE":
                    res = ExeHelper.GetRemoteExeAppInfoByName(appName, serAdress);
                    break;
            }
            return new MyJsonResult { Data = res };
        }

        public ActionResult AddOrUpdateRemoteAppInfo()
        {
            return null;
        }
        #endregion
    }
}