using System;
using System.Collections.Generic;

namespace TPublish.Common.Model
{
    [Serializable]
    public class SettingView
    {
        public string MgeProcessFullName { get; set; }

        public List<AppSerListMap> RemoteAppList { get; set; }

        public List<ServiceGroup> ServiceGroups { get; set; }

        public Dictionary<string, string> AppZipFileMap { get; set; }
    }
}