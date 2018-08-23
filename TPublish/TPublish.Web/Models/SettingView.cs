using System;
using System.Collections.Generic;

namespace TPublish.Web.Models
{
    [Serializable]
    public class SettingView
    {
        public string MgeProcessFullName { get; set; }

        public List<AppSerListMap> RemoteAppList { get; set; }

        public List<ServiceGroup> ServiceGroups { get; set; }
    }
}