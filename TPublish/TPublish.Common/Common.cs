using System;
using System.IO;

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
    }
}
