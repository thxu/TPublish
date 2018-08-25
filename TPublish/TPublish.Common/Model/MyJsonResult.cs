using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace TPublish.Common.Model
{
    /// <summary>
    /// json
    /// </summary>
    public class MyJsonResult : ActionResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyJsonResult()
        {
            ContentEncoding = Encoding.UTF8;
            ContentType = "application/json";
        }

        /// <summary>
        /// 重写ExecuteResult
        /// </summary>
        /// <param name="context">ControllerContext</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding == null)
            {
                ContentEncoding = Encoding.UTF8;
            }
            response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                var data = NewtonsoftSerialize(Data);
                byte[] bytes = ContentEncoding.GetBytes(data);
                response.BufferOutput = true;
                response.AddHeader("Content-Length", bytes.Length.ToString());
                response.BinaryWrite(bytes);
            }
        }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>结果</returns>
        private static string NewtonsoftSerialize(object value)
        {
            return value.SerializeObject();
        }
    }
}