using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlphaS.BasicDailyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData.Tests
{
    [TestClass()]
    public class BasicDailyDataDownloaderTests
    {
        [TestMethod()]
        public void getContentFromtbodyTest()
        {
            var test = new BasicDailyDataDownloader();
            Assert.AreEqual("TEST STRING", test.getContentFromtbody("<tbody ewqewqe>TEST STRING</tbody>"));
        }
    }
}