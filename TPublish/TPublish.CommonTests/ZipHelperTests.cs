using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPublish.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPublish.Common.Tests
{
    [TestClass()]
    public class ZipHelperTests
    {
        [TestMethod()]
        public void ZipDirectoryTest()
        {
            //var projPath = @"E:\Code\C#\Test\Test\ClassLibrary1\";
            //var outputPath = @"..\..\日志记录Demo\";
            //DirectoryInfo projDirectoryInfo = new DirectoryInfo(projPath);
            //while (outputPath.StartsWith(@"..\"))
            //{
            //    projDirectoryInfo = projDirectoryInfo?.Parent;
            //    char[] tmp = { '.', '.', '\\' };
            //    outputPath = outputPath.Substring(3);
            //}

            //var path = Path.Combine(projDirectoryInfo?.FullName ?? string.Empty, outputPath);

            string dir = @"E:\EXE\GroundingResistance\1.0.0.4";
            string zipName = @"E:\3.zip";
            ZipHelper.ZipDirectory(dir, zipName);
            ZipHelper.ZipDir(dir, @"E:\4.zip");
            Assert.Fail();
        }
    }
}