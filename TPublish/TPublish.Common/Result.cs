using System;
using System.Runtime.Serialization;

namespace TPublish.Common
{
    /// <summary>
    /// 消息
    /// </summary>
    [Serializable]
    [DataContract]
    public class Result
    {
        /// <summary>
        /// 成功/失败
        /// </summary>
        [DataMember]
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }

    /// <summary>
    /// 处理结果
    /// </summary>
    /// <typeparam name="T">扩展数据</typeparam>
    [Serializable]
    [DataContract]
    public class Result<T> : Result
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        [DataMember]
        public T Data { get; set; }
    }
}
