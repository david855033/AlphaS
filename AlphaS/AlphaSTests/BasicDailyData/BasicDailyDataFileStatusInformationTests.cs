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
    public class BasicDailyDataFileStatusInformationTests
    {
        [TestMethod()]
        public void BasicDailyDataFileStatusInformationTestFileStatus()
        {
            var test = new BasicDailyDataFileStatusInformation("2016\t5\t10\tValid\t2016-09-19 21:00");
            Assert.AreEqual(test.fileStatus, FileStatus.Valid);
        }

        [TestMethod()]
        public void BasicDailyDataFileStatusInformationTestModifiedTime()
        {
            var test = new BasicDailyDataFileStatusInformation("2016\t5\t5\tValid\t2016-09-19 21:00");
            Assert.AreEqual(DateTime.Parse(test.modifiedTime), DateTime.Parse("2016-09-19 21:00"));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var test = new BasicDailyDataFileStatusInformation("2016\t5\t5\tValid\t2016-09-19 21:00");
            var test2 = new BasicDailyDataFileStatusInformation(test.ToString());
            Assert.AreEqual(test.year, test2.year);
            Assert.AreEqual(test.month, test2.month);
            Assert.AreEqual(DateTime.Parse(test.modifiedTime), DateTime.Parse(test2.modifiedTime));
            Assert.AreEqual(test.fileStatus, test2.fileStatus);
        }

        [TestMethod()]
        public void CompareToTest()
        {
            var test1 = new BasicDailyDataFileStatusInformation("2016\t5\t5\tValid\t2016-09-19 21:00");
            var test2 = new BasicDailyDataFileStatusInformation("2016\t6\t5\tValid\t2016-09-19 21:00");
            Assert.IsTrue(test2.CompareTo(test1) > 0);
        }
        [TestMethod()]
        public void CompareToTest2()
        {
            var test1 = new BasicDailyDataFileStatusInformation("2016\t5\t5\tValid\t2016-09-19 21:00");
            var test2 = new BasicDailyDataFileStatusInformation("2014\t6\t5\tValid\t2016-09-19 21:00");
            Assert.IsTrue(test2.CompareTo(test1) < 0);
        }

    }
}