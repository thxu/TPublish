using System;
using System.Collections.Generic;
using System.Linq;

namespace TPublish.WinFormClientApp.Model
{
    /// <summary>
    /// 设置信息
    /// </summary>
    public class MSettingInfo
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string Authour { get; set; }

        /// <summary>
        /// 服务器信息
        /// </summary>
        public List<ServiceInfo> ServiceInfos { get; set; }

        ///// <summary>
        ///// api地址
        ///// </summary>
        //public string ApiIpAdress { get; set; }

        ///// <summary>
        ///// api key
        ///// </summary>
        //public string ApiKey { get; set; }

        /// <summary>
        /// 微软编译器路径
        /// </summary>
        public string MsBuildExePath { get; set; }

        /// <summary>
        /// 最近的项目选择记录
        /// </summary>
        public List<MSelectedItem> SelectedItems { get; set; }

        ///// <summary>
        ///// 获取api地址
        ///// </summary>
        ///// <returns></returns>
        //public string GetApiUrl()
        //{
        //    var apiService = GetCurrServiceInfo();
        //    if (apiService == null)
        //    {
        //        return null;
        //    }
        //    return $"{apiService?.ApiIpAdress}/ClientApi";
        //}

        public ServiceInfo GetCurrServiceInfo()
        {
            ServiceInfo res = null;
            if (this.ServiceInfos != null && this.ServiceInfos.Any())
            {
                res = this.ServiceInfos.FirstOrDefault(n => n.IsDefault) ??
                       this.ServiceInfos.FirstOrDefault();
            }

            //if (res ==  null)
            //{
            //    throw new Exception("请先配置服务器信息");
            //}

            return res;
        }

        /// <summary>
        /// 主题
        /// </summary>
        public int MetroThemeStyle { get; set; }

        /// <summary>
        /// 风格
        /// </summary>
        public int MetroColorStyle { get; set; }
    }

    public class ServiceInfo
    {
        public string Alias { get; set; }

        public bool IsDefault { get; set; }

        /// <summary>
        /// api地址
        /// </summary>
        public string ApiIpAdress { get; set; }

        /// <summary>
        /// api key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// 获取api地址
        /// </summary>
        /// <returns></returns>
        public string GetApiUrl()
        {
            return $"{ApiIpAdress}/ClientApi";
        }
    }

    public class MSelectedItem
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 2=C#项目，3=文件夹
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// type=2的时候，此值为项目csproj文件的完整路径
        /// type=3的时候，此值为要发布的文件夹
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// type=2的时候，此值为项目csproj文件的项目名称
        /// type=3的时候，此值为要发布的文件夹的文件夹名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
