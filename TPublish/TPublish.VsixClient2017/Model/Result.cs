using System;

namespace TPublish.VsixClient2017.Model
{
    /// <summary>
    /// 消息
    /// </summary>
    [Serializable]
    public class Result
    {
        /// <summary>
        /// 成功/失败
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 处理结果
    /// </summary>
    /// <typeparam name="T">扩展数据</typeparam>
    [Serializable]
    public class Result<T> : Result
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        public T Data { get; set; }
    }
}
