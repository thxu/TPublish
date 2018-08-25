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
    public class ExeHelperTests
    {
        [TestMethod()]
        public void GetAllExeAppViewTest()
        {
            var res = ExeHelper.GetAllExeAppInfo();
            Assert.Fail();
        }
    }
}