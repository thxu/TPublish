﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace TPublish.Common
{
    public class ZipHelper
    {

        #region IO.Compression
        private static readonly char s_pathSeperator = '/';
        public static bool BatchZip(List<string> folderOrFileList, string zipedFile, string baseDir = "", Func<int, bool> progress = null)
        {
            var zipBytes = BatchZip(folderOrFileList, CompressionLevel.Optimal, baseDir,
                null,
                progress);

            using (var fs = new FileStream(zipedFile, FileMode.Create, FileAccess.Write))
            {
                fs.Write(zipBytes, 0, zipBytes.Length);
            }

            return true;
        }

        public static byte[] BatchZip(List<string> fileList, CompressionLevel compressionLevel, string baseDirectory,
      List<string> ignoreList = null, Func<int, bool> progress = null)
        {
            var allFile = new List<FileSystemInfo>();

            foreach (string file in fileList)
            {
                FileAttributes attr = File.GetAttributes(file);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    allFile.Add(new DirectoryInfo(file));
                }
                else
                {
                    allFile.Add(new FileInfo(file));
                }
            }
            var allFileLength = allFile.Count();
            var index = 0;
            using (var outStream = new MemoryStream())
            {
                using (ZipArchive destination = new ZipArchive(outStream, ZipArchiveMode.Create, false))
                {
                    foreach (FileSystemInfo enumerateFileSystemInfo in allFile)
                    {
                        index++;
                        var lastProgressNumber = (((long)index * 100 / allFileLength));
                        if (progress != null)
                        {
                            var r = progress.Invoke((int)lastProgressNumber);
                            if (r)
                            {
                                throw new Exception("deploy task was canceled!");
                            }
                        }

                        int length = enumerateFileSystemInfo.FullName.Length - baseDirectory.Length;
                        string entryName = EntryFromPath(enumerateFileSystemInfo.FullName, baseDirectory.Length, length);

                        if (enumerateFileSystemInfo is FileInfo)
                        {
                            if (entryName.Contains("Dockerfile"))
                            {
                                //logger?.Info($"Find Dockerfile In Package: {entryName}");
                            }
                            DoCreateEntryFromFile(destination, enumerateFileSystemInfo.FullName, entryName, compressionLevel);
                        }
                        else
                        {
                            DirectoryInfo possiblyEmptyDir = enumerateFileSystemInfo as DirectoryInfo;
                            if (possiblyEmptyDir != null && IsDirEmpty(possiblyEmptyDir))
                                destination.CreateEntry(entryName + s_pathSeperator);
                        }
                    }
                }
                return outStream.ToArray();
            }
        }

        public static string EntryFromPath(string entry, int offset, int length)
        {
            for (; length > 0 && ((int)entry[offset] == (int)Path.DirectorySeparatorChar || (int)entry[offset] == (int)Path.AltDirectorySeparatorChar); --length)
                ++offset;
            if (length == 0)
                return string.Empty;
            char[] charArray = entry.ToCharArray(offset, length);
            for (int index = 0; index < charArray.Length; ++index)
            {
                if ((int)charArray[index] == (int)Path.DirectorySeparatorChar || (int)charArray[index] == (int)Path.AltDirectorySeparatorChar)
                    charArray[index] = s_pathSeperator;
            }
            return new string(charArray);
        }

        private static bool IsDirEmpty(DirectoryInfo possiblyEmptyDir)
        {
            using (IEnumerator<FileSystemInfo> enumerator = possiblyEmptyDir.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    FileSystemInfo current = enumerator.Current;
                    return false;
                }
            }
            return true;
        }

        internal static ZipArchiveEntry DoCreateEntryFromFile(
            ZipArchive destination,
            string sourceFileName,
            string entryName,
            CompressionLevel? compressionLevel)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (sourceFileName == null)
                throw new ArgumentNullException(nameof(sourceFileName));
            if (entryName == null)
                throw new ArgumentNullException(nameof(entryName));
            using (Stream stream = (Stream)File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ZipArchiveEntry zipArchiveEntry = compressionLevel.HasValue ? destination.CreateEntry(entryName, compressionLevel.Value) : destination.CreateEntry(entryName);
                DateTime dateTime = File.GetLastWriteTime(sourceFileName);
                if (dateTime.Year < 1980 || dateTime.Year > 2107)
                    dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
                zipArchiveEntry.LastWriteTime = (DateTimeOffset)dateTime;
                using (Stream destination1 = zipArchiveEntry.Open())
                    stream.CopyTo(destination1);
                return zipArchiveEntry;
            }
        }
        #endregion


        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dirToZip"></param>
        /// <param name="zipedFileName"></param>
        /// <param name="compressionLevel">压缩率0（无压缩）9（压缩率最高）</param>
        public static void ZipDir(string dirToZip, string zipedFileName, int compressionLevel = 5)
        {
            if (Path.GetExtension(zipedFileName) != ".zip")
            {
                zipedFileName = zipedFileName + ".zip";
            }
            using (var zipoutputstream = new ZipOutputStream(File.Create(zipedFileName)))
            {
                zipoutputstream.SetLevel(compressionLevel);
                Crc32 crc = new Crc32();
                Hashtable fileList = GetAllFies(dirToZip);
                foreach (DictionaryEntry item in fileList)
                {
                    FileStream fs = new FileStream(item.Key.ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    // ZipEntry entry = new ZipEntry(item.Key.ToString().Substring(dirToZip.Length + 1));
                    ZipEntry entry = new ZipEntry(Path.GetFileName(item.Key.ToString()))
                    {
                        DateTime = (DateTime)item.Value,
                        Size = fs.Length
                    };
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipoutputstream.PutNextEntry(entry);
                    zipoutputstream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>  
        /// 获取所有文件  
        /// </summary>  
        /// <returns></returns>  
        public static Hashtable GetAllFies(string dir)
        {
            Hashtable filesList = new Hashtable();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }

            GetAllDirFiles(fileDire, filesList);
            GetAllDirsFiles(fileDire.GetDirectories(), filesList);
            return filesList;
        }

        /// <summary>  
        /// 获取一个文件夹下的所有文件夹里的文件  
        /// </summary>  
        /// <param name="dirs"></param>  
        /// <param name="filesList"></param>  
        public static void GetAllDirsFiles(IEnumerable<DirectoryInfo> dirs, Hashtable filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                if (dir.Name.ToLower().Contains("log") || dir.Name.ToLower().Contains("wcfconfig"))
                {
                    continue;
                }
                foreach (FileInfo file in dir.GetFiles("*.*").Where(n => !n.Name.ToLower().EndsWith("xml") && !n.Name.Equals("TPublish.setting")))
                {
                    if (file.Extension.ToLower() == ".config" || file.Extension.ToLower() == ".manifest" || file.Extension.ToLower() == ".asax")
                    {
                        continue;
                    }
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }

        /// <summary>  
        /// 获取一个文件夹下的文件  
        /// </summary>  
        /// <param name="dir">目录名称</param>
        /// <param name="filesList">文件列表HastTable</param>  
        public static void GetAllDirFiles(DirectoryInfo dir, Hashtable filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*").Where(n => !n.Name.ToLower().EndsWith("xml") && !n.Name.Equals("TPublish.setting")))
            {
                if (file.Extension.ToLower() == ".config" || file.Extension.ToLower() == ".manifest" || file.Extension.ToLower() == ".asax")
                {
                    continue;
                }
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }

        /// <summary>  
        /// 功能：解压zip格式的文件。  
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>  
        /// <returns>解压是否成功</returns>  
        public static void UnZip(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("/"))
                unZipDir += "/";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            using (var s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(unZipDir + directoryName))
                    {
                        Directory.CreateDirectory(unZipDir + directoryName);
                    }
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                        {
                            int size;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="filePath">被压缩的文件名称(包含文件路径)，文件的全路径</param>
        /// <param name="zipedFileName">压缩后的文件名称(包含文件路径)，保存的文件名称</param>
        /// <param name="compressionLevel">压缩率0（无压缩）到 9（压缩率最高）</param>
        public static void ZipFile(string filePath, string zipedFileName, int compressionLevel = 9)
        {
            // 如果文件没有找到，则报错 
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件：" + filePath + "没有找到！");
            }
            // 如果压缩后名字为空就默认使用源文件名称作为压缩文件名称
            if (string.IsNullOrEmpty(zipedFileName))
            {
                string oldValue = Path.GetFileName(filePath);
                if (oldValue != null)
                {
                    zipedFileName = filePath.Replace(oldValue, "") + Path.GetFileNameWithoutExtension(filePath) + ".zip";
                }
            }
            // 如果压缩后的文件名称后缀名不是zip，就是加上zip，防止是一个乱码文件
            if (Path.GetExtension(zipedFileName) != ".zip")
            {
                zipedFileName = zipedFileName + ".zip";
            }
            // 如果指定位置目录不存在，创建该目录  C:\Users\yhl\Desktop\大汉三通
            string zipedDir = zipedFileName.Substring(0, zipedFileName.LastIndexOf("\\", StringComparison.Ordinal));
            if (!Directory.Exists(zipedDir))
            {
                Directory.CreateDirectory(zipedDir);
            }
            // 被压缩文件名称
            string filename = filePath.Substring(filePath.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            var streamToZip = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var zipFile = File.Create(zipedFileName);
            var zipStream = new ZipOutputStream(zipFile);
            var zipEntry = new ZipEntry(filename);
            zipStream.PutNextEntry(zipEntry);
            zipStream.SetLevel(compressionLevel);
            var buffer = new byte[2048];
            Int32 size = streamToZip.Read(buffer, 0, buffer.Length);
            zipStream.Write(buffer, 0, size);
            try
            {
                while (size < streamToZip.Length)
                {
                    int sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            finally
            {
                zipStream.Finish();
                zipStream.Close();
                streamToZip.Close();
            }
        }

        /// <summary> 
        /// 压缩单个文件 
        /// </summary> 
        /// <param name="fileToZip">要进行压缩的文件名，全路径</param> 
        /// <param name="zipedFile">压缩后生成的压缩文件名,全路径</param> 
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            // 如果文件没有找到，则报错 
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }
            using (FileStream fileStream = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Close();
                using (FileStream zipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream zipOutputStream = new ZipOutputStream(zipFile))
                    {
                        // string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        string fileName = Path.GetFileName(fileToZip);
                        var zipEntry = new ZipEntry(fileName)
                        {
                            DateTime = DateTime.Now,
                            IsUnicodeText = true
                        };
                        zipOutputStream.PutNextEntry(zipEntry);
                        zipOutputStream.SetLevel(5);
                        zipOutputStream.Write(buffer, 0, buffer.Length);
                        zipOutputStream.Finish();
                        zipOutputStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多个目录或文件
        /// </summary>
        /// <param name="folderOrFileList">待压缩的文件夹或者文件，全路径格式,是一个集合</param>
        /// <param name="zipedFile">压缩后的文件名，全路径格式</param>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public static async Task<bool> ZipManyFilesOrDictorysAsync(IEnumerable<string> folderOrFileList, string zipedFile, string baseDir = "")
        {
            return await Task.Run((() =>
            {
                bool res = true;
                using (var s = new ZipOutputStream(File.Create(zipedFile)))
                {
                    s.SetLevel(0);
                    foreach (string fileOrDir in folderOrFileList)
                    {
                        string parentPath = "";
                        if (fileOrDir.StartsWith(baseDir))
                        {
                            var parentPaths = GetParentPaths(fileOrDir, baseDir);
                            while (parentPaths.Count > 0)
                            {
                                string path = parentPaths.Pop();
                                AddEmptyDir(path, s, parentPath);
                                var fileName = Path.GetFileName(path);
                                if (string.IsNullOrWhiteSpace(fileName))
                                {
                                    throw new InvalidOperationException();
                                }
                                parentPath = Path.Combine(parentPath, fileName);
                            }
                        }
                        //是文件夹
                        if (Directory.Exists(fileOrDir))
                        {
                            res = ZipFileDictory(fileOrDir, s, parentPath);
                        }
                        else
                        {
                            //文件
                            res = ZipFileWithStream(fileOrDir, s, parentPath);
                        }
                    }
                    s.Finish();
                    s.Close();
                    return res;
                }
            }));
        }

        /// <summary>
        /// 压缩多个目录或文件
        /// </summary>
        /// <param name="folderOrFileList">待压缩的文件夹或者文件，全路径格式,是一个集合</param>
        /// <param name="zipedFile">压缩后的文件名，全路径格式</param>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public static bool ZipManyFilesOrDictorys(IEnumerable<string> folderOrFileList, string zipedFile, string baseDir = "")
        {
            bool res = true;
            using (var s = new ZipOutputStream(File.Create(zipedFile)))
            {
                s.SetLevel(1);
                foreach (string fileOrDir in folderOrFileList)
                {
                    string parentPath = "";
                    if (fileOrDir.StartsWith(baseDir))
                    {
                        var parentPaths = GetParentPaths(fileOrDir, baseDir);
                        while (parentPaths.Count > 0)
                        {
                            string path = parentPaths.Pop();
                            AddEmptyDir(path, s, parentPath);
                            var fileName = Path.GetFileName(path);
                            if (string.IsNullOrWhiteSpace(fileName))
                            {
                                throw new InvalidOperationException();
                            }
                            parentPath = Path.Combine(parentPath, fileName);
                        }
                    }
                    //是文件夹
                    if (Directory.Exists(fileOrDir))
                    {
                        res = ZipFileDictory(fileOrDir, s, parentPath);
                    }
                    else
                    {
                        //文件
                        res = ZipFileWithStream(fileOrDir, s, parentPath);
                    }
                }
                s.Finish();
                s.Close();
                return res;
            }
        }

        private static Stack<string> GetParentPaths(string file, string baseDir)
        {
            Stack<string> res = new Stack<string>();
            var path = file;
            DirectoryInfo a;

            while (path != baseDir && (a = Directory.GetParent(path)) != null)
            {
                path = a.FullName;
                if (path != baseDir)
                {
                    res.Push(path);
                }
            }

            return res;
        }

        private static void AddEmptyDir(string folderToZip, ZipOutputStream s, string parentFolderName)
        {
            try
            {
                ZipEntry entry = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                s.PutNextEntry(entry);
                s.Flush();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 带压缩流压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipStream"></param>
        /// <param name="parentFolderName"></param>
        /// <returns></returns>
        private static bool ZipFileWithStream(string fileToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }
            //FileStream fs = null;
            FileStream zipFile = null;
            ZipEntry zipEntry = null;
            bool res = true;
            try
            {
                zipFile = File.OpenRead(fileToZip);
                byte[] buffer = new byte[zipFile.Length];
                zipFile.Read(buffer, 0, buffer.Length);
                zipFile.Close();
                zipEntry = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(fileToZip)));
                zipStream.PutNextEntry(zipEntry);
                zipStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                }

                if (zipFile != null)
                {
                    zipFile.Close();
                }
                GC.Collect();
                GC.Collect(1);
            }
            return res;

        }

        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="s"></param>
        /// <param name="parentFolderName"></param>
        private static bool ZipFileDictory(string folderToZip, ZipOutputStream s, string parentFolderName)
        {
            bool res = true;
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {
                //创建当前文件夹
                entry = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/")); //加上 “/” 才会当成是文件夹创建
                s.PutNextEntry(entry);
                s.Flush();
                //先压缩文件，再递归压缩文件夹
                var filenames = Directory.GetFiles(folderToZip);
                foreach (string file in filenames)
                {
                    //打开压缩文件
                    fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (entry != null)
                {
                }
                GC.Collect();
                GC.Collect(1);
            }
            var folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
            {
                if (!ZipFileDictory(folder, s, Path.Combine(parentFolderName, Path.GetFileName(folderToZip))))
                {
                    return false;
                }
            }
            return res;
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="zipName">压缩后的文件名称</param>
        /// <returns>压缩结果</returns>
        public static bool ZipDirectory(string dir, string zipName)
        {
            bool res = false;
            try
            {
                List<string> pathList = new List<string>();
                DirectoryInfo fileDire = new DirectoryInfo(dir);

                foreach (var directory in fileDire.GetDirectories())
                {
                    if (directory.Name.ToLower().Contains("log") || directory.Name.ToLower().Contains("wcfconfig"))
                    {
                        continue;
                    }
                    pathList.Add(directory.FullName);
                }
                foreach (FileInfo file in fileDire.GetFiles("*.*").Where(n => !n.Name.ToLower().EndsWith("xml") && !n.Name.Equals("TPublish.setting")))
                {
                    if (file.Extension.ToLower() == ".config" || file.Extension.ToLower() == ".manifest" || file.Extension.ToLower() == ".asax")
                    {
                        continue;
                    }
                    pathList.Add(file.FullName);
                }
                res = ZipManyFilesOrDictorys(pathList, zipName, dir);
            }
            catch (Exception e)
            {
                TxtLogService.WriteLog(e, "压缩文件夹异常，信息：" + new { dir, zipName }.SerializeObject());
            }

            return res;
        }
    }
}
