using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TPublish.Common;
using Timer = System.Timers.Timer;

namespace TPublish.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Timer _timer = new Timer();

        private ManualResetEvent waitOne;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            _timer.Interval = 10000;
            _timer.Elapsed += (sender, args) => { TxtLogService.WriteLog("111"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "E:\\PublishTest"); };

            _timer.Start();

            this.waitOne.WaitOne(10000);
        }
    }
}
