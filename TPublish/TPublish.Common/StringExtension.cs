namespace TPublish.Common
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="val">要判断得字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string val)
        {
            return string.IsNullOrWhiteSpace(val);
        }
    }
}
