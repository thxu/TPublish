using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;

namespace TPublish.Common
{
    public static class IISHelper
    {
        /// <summary>
        /// 切换iis程序版本并回收应用程序池
        /// </summary>
        /// <param name="appName">app名称</param>
        /// <param name="path">app物理路径（全路径）</param>
        /// <returns></returns>
        public static Result ChangeIISAppVersion(this string appName, string path)
        {
            Result res = new Result();
            try
            {
                using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                {
                    var site = mgr.Sites.FirstOrDefault(n => n.Name == appName);
                    if (site == null)
                    {
                        throw new Exception("该应用程序不存在");
                    }

                    site.Applications["/"].VirtualDirectories["/"].PhysicalPath = path;
                    mgr.CommitChanges();

                    var appPool = mgr.ApplicationPools[appName];
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
                res.Message = e.Message;
            }

            return res;
        }

        /// <summary>
        /// 复制文件夹到新版本（版本号自动加1）
        /// </summary>
        /// <param name="appName">应用程序名称</param>
        /// <returns>新文件夹路径</returns>
        public static string CopyIISAppToNewDir(this string appName)
        {
            using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
            {
                var site = mgr.Sites.FirstOrDefault(n => n.Name == appName);
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
        public static List<string> GetAllIISAppName()
        {
            List<string> res = new List<string>();
            try
            {
                using (var mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
                {
                    foreach (var site in mgr.Sites)
                    {
                        res.Add(site.Name);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return res;
        }
    }
}
