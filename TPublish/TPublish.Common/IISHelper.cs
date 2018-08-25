using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;
using TPublish.Common.Model;

namespace TPublish.Common
{
    public static class IISHelper
    {
        /// <summary>
        /// 切换iis程序版本并回收应用程序池
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="path">app物理路径（全路径）</param>
        /// <returns></returns>
        public static Result ChangeIISAppVersion(this string appId, string path)
        {
            Result res = new Result();
            try
            {
                using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                {
                    var site = mgr.Sites.FirstOrDefault(n => n.Id.ToString() == appId);
                    if (site == null)
                    {
                        throw new Exception("该应用程序不存在");
                    }

                    site.Applications["/"].VirtualDirectories["/"].PhysicalPath = path;
                    mgr.CommitChanges();

                    var appPool = mgr.ApplicationPools[site.Name];
                    if (appPool == null)
                    {
                        throw new Exception("该应用程序池不存在");
                    }
                    if (appPool.State == ObjectState.Stopped)
                    {
                        appPool.Start();
                    }
                    else
                    {
                        appPool.Recycle();
                    }
                    mgr.CommitChanges();
                }

                res.IsSucceed = true;
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "切换IIS版本、回收程序池异常，信息:" + new { appId, path });
                res.Message = e.Message;
            }

            return res;
        }

        /// <summary>
        /// IIS程序版本回退
        /// </summary>
        /// <param name="appId">appId</param>
        /// <returns>回退结果</returns>
        public static Result IISAppVersionRollBack(this string appId)
        {
            Result res = new Result();
            try
            {
                using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                {
                    var site = mgr.Sites.FirstOrDefault(n => n.Id.ToString() == appId);
                    if (site == null)
                    {
                        throw new Exception("该应用程序不存在");
                    }
                    var lastVersionPath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath;
                    string oldVersion = lastVersionPath.SubVersion();
                    site.Applications["/"].VirtualDirectories["/"].PhysicalPath = oldVersion;
                    mgr.CommitChanges();

                    var appPool = mgr.ApplicationPools[site.Name];
                    if (appPool == null)
                    {
                        throw new Exception("该应用程序池不存在");
                    }
                    if (appPool.State == ObjectState.Stopped)
                    {
                        appPool.Start();
                    }
                    else
                    {
                        appPool.Recycle();
                    }
                    mgr.CommitChanges();
                }
            }
            catch (Exception e)
            {
                res.Message = e.Message;
                TxtLogService.WriteLog(e, "回退iis版本异常，信息:" + appId);
            }
            return res;
        }

        /// <summary>
        /// 复制文件夹到新版本（版本号自动加1）
        /// </summary>
        /// <param name="appId">appId</param>
        /// <returns>新文件夹路径</returns>
        public static string CopyIISAppToNewDir(this string appId)
        {
            using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
            {
                var site = mgr.Sites.FirstOrDefault(n => n.Id.ToString() == appId);
                if (site == null)
                {
                    throw new Exception("该应用程序不存在");
                }
                var lastVersionPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath;
                string newVersion = lastVersionPath.AddVersion();
                lastVersionPath.CopyDirectoryTo(newVersion);
                return newVersion;
            }
        }

        /// <summary>
        /// 获取所有iis应用程序名称
        /// </summary>
        /// <returns>应用程序名称集合</returns>
        public static List<AppView> GetAllIISAppInfo()
        {
            List<AppView> res = new List<AppView>();
            try
            {
                using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                {
                    foreach (var site in mgr.Sites)
                    {
                        res.Add(new AppView
                        {
                            AppName = site.Name,
                            AppPhysicalPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                            AppAlias = site.Name,
                            Id = site.Id.ToString(),
                            Status = site.State == ObjectState.Started ? 0 : 1,
                        });
                    }
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取所有iis应用程序名称异常");
            }
            return res;
        }

        /// <summary>
        /// 获取所有iis应用程序名称
        /// </summary>
        /// <returns>应用程序信息</returns>
        public static AppView GetIISAppInfoById(this string id)
        {
            AppView res = new AppView();
            try
            {
                using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                {
                    var site = mgr.Sites.FirstOrDefault(n => n.Id.ToString() == id);
                    if (site != null)
                    {
                        res = new AppView
                        {
                            AppName = site.Name,
                            AppPhysicalPath = site.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath ?? string.Empty,
                            AppAlias = site.Name,
                            Id = site.Id.ToString(),
                            Status = site.State == ObjectState.Started ? 0 : 1,
                        };
                    }
                }
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "获取指定iis应用程序名称异常,id=" + id);
            }
            return res;
        }
    }
}
