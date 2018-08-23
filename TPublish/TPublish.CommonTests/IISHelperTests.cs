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
    public class IISHelperTests
    {
        [TestMethod()]
        public void GetAllIISAppNameTest()
        {
            var res = IISHelper.GetAllIISAppName();
            Assert.Fail();
        }
    }
}