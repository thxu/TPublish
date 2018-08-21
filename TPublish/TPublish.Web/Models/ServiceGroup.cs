using System;
using System.Collections.Generic;

namespace TPublish.Web.Models
{
    [Serializable]
    public class ServiceGroup
    {
        public string Guid { get; set; }

        public string GroupName { get; set; }

        public string AppPath { get; set; }

        public List<string> ServiceAdressList { get; set; }
    }
}