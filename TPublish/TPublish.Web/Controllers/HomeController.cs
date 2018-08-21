using System.Collections.Generic;
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

        public ActionResult QueryAllApp(string type)
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
                    res = BaseController.GetAllExeAppView();
                    break;
                default:
                    break;
            }
            return new MyJsonResult { Data = res };
        }

        public ActionResult QuerySerGroupByApp(int appId, string type)
        {
            switch (type.ToUpper())
            {
                case "IIS":
                    // iis
                    break;
                case "EXE":
                    // exe
                    break;
                default:
                    break;
            }
            return null;
        }


        #endregion

        #region 管理



        #endregion
    }
}