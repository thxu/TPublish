using System;
using System.Collections.Generic;

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
        /// api地址
        /// </summary>
        public string ApiIpAdress { get; set; }

        /// <summary>
        /// api key
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// 微软编译器路径
        /// </summary>
        public string MsBuildExePath { get; set; }

        /// <summary>
        /// 最近的项目选择记录
        /// </summary>
        public List<MSelectedItem> SelectedItems { get; set; }

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
