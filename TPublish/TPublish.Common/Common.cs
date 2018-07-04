using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TPublish.Common
{
    public static class Common
    {
        /// <summary>
        /// 复制所有文件到指定目录
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        public static void CopyDirectoryTo(this string srcPath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)     //判断是否文件夹
                {
                    if (i.Name.ToLower().Contains("log"))
                    {
                        continue;
                    }
                    if (!Directory.Exists(destPath + "\\" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }
                    i.FullName.CopyDirectoryTo(destPath + "\\" + i.Name);    //递归调用复制子文件夹
                }
                else
                {
                    File.Copy(i.FullName, destPath + "\\" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
        }

        /// <summary>
        /// 版本号自动加1
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string AddVersion(this string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return "1.0.0.1";
            }
            string[] tmp = version.Split('.');
            int val = Convert.ToInt32(tmp[tmp.Length - 1]) + 1;
            tmp[tmp.Length - 1] = val.ToString();
            return string.Join(".", tmp);
        }

        /// <summary>
        /// 带参数的文件上传
        /// </summary>
        /// <param name="url">服务器url</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="fileKeyName">文件名称</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="stringDict">参数集合</param>
        /// <returns>上传结果</returns>
        public static string HttpPostData(string url, int timeOut, string fileKeyName, string filePath, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            // 写入字符串的Key
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream() ?? throw new InvalidOperationException(),
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }
    }
}
