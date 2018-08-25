using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPublish.Common;
using System;
using System.Collections.Generic;
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
            string dir = @"E:\EXE\GroundingResistance\1.0.0.4";
            string zipName = @"E:\3.zip";
            ZipHelper.ZipDirectory(dir, zipName);
            ZipHelper.ZipDir(dir, @"E:\4.zip");
            Assert.Fail();
        }
    }
}