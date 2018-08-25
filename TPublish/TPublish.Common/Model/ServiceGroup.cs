using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace TPublish.Common.Model
{
    /// <summary>
    /// 服务器组信息
    /// </summary>
    [Serializable]
    public class ServiceGroup
    {
        public string Guid { get; set; }

        public string GroupName { get; set; }
    }

    /// <summary>
    /// app-服务器列表映射关系（体现当前app对应多少个服务器）
    /// </summary>
    [Serializable]
    public class AppSerListMap
    {
        public string AppId { get; set; }

        public string AppName { get; set; }

        public string AppType { get; set; }

        public string SerGroupId { get; set; }

        public List<SerAppMap> ServiceAdressList { get; set; }
    }

    /// <summary>
    /// 服务器-app映射关系(体现某个服务器对应了哪一个app）
    /// </summary>
    [Serializable]
    public class SerAppMap
    {
        public string ServiceAdress { get; set; }

        public string AppType { get; set; }

        public string AppId { get; set; }
    }
}