using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace TPublish.VsixClient2017.Service
{
    public class ZipHelper
    {
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
    }
}
