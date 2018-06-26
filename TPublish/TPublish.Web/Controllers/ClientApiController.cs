﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Administration;
using TPublish.Common;

namespace TPublish.Web.Controllers
{
    public class ClientApiController : Controller
    {
        public string Index()
        {
            return "hello";
        }

        public string PublishToIIS()
        {
            using (var mgr = new ServerManager())
            {
                var site = mgr.Sites.FirstOrDefault(n => n.Name == "test");
                if (site == null)
                {
                    return "";
                }
                var lastVersionPath = site?.Applications["/"]?.VirtualDirectories["/"]?.PhysicalPath;
                string newVersion = lastVersionPath.AddVersion();
                lastVersionPath.CopyDirectoryTo(newVersion);
                site.Applications["/"].VirtualDirectories["/"].PhysicalPath = newVersion;
                mgr.CommitChanges();
            }
            return null;
        }

        public string UploadTest(HttpPostedFileBase uploadFile)
        {
            var f1 = Request.Files[0];
            f1.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, f1.FileName));

            string strPars = "";
            foreach (var key in Request.Form.AllKeys)
            {
                string val = Request[key];
                strPars += "[" + key + ":" + val + "] ";
            }
            return strPars;
        }
    }
}