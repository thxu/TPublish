using System;
using System.Collections.Generic;

namespace TPublish.Web.Models
{
    [Serializable]
    public class SettingView
    {
        public string MgeProcessFullName { get; set; }

        public List<KeyValuePair<string, KeyValuePair<string, string>>> RemotePublishAppList { get; set; }
    }
}