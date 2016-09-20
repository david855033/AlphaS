using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlphaS.BasicDailyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AlphaS.BasicDailyData.Tests
{
    [TestClass()]
    public class BasicDailyDataManagerTests
    {


        [TestMethod()]
        public void setBaseFolderTest()
        {
            var test = new BasicDailyDataManager();
            if (Directory.Exists(@"D:\AlphaS\TestBasicDailyData"))
            {
                Directory.Delete(@"D:\AlphaS\TestBasicDailyData", true);
            }
            test.setBaseFolder(@"D:\AlphaS\TestBasicDailyData");
            Assert.IsTrue(Directory.Exists(@"D:\AlphaS\TestBasicDailyData"));
            Assert.AreEqual(@"D:\AlphaS\TestBasicDailyData", test.getBaseFolder());
        }

        List<BasicDailyDataFileStatusInformation> testFileStatus = new List<BasicDailyDataFileStatusInformation>() {
            new BasicDailyDataFileStatusInformation("2016\t4\t5\tValid\t2016-09-19 08:00"),
            new BasicDailyDataFileStatusInformation("2016\t5\t5\tValid\t2016-09-19 20:00"),
            new BasicDailyDataFileStatusInformation("2016\t2\t5\tValid\t2016-09-19 21:00"),
            new BasicDailyDataFileStatusInformation("2014\t5\t5\tValid\t2016-09-19 22:00")
        };

        [TestMethod()]
        public void saveFileStatusTest()
        {
            var test = new BasicDailyDataManager();
            if (Directory.Exists(@"D:\AlphaS\TestBasicDailyData"))
            {
                Directory.Delete(@"D:\AlphaS\TestBasicDailyData", true);
            }
            test.setBaseFolder(@"D:\AlphaS\TestBasicDailyData");
            test.saveFileStatus("test", testFileStatus);
            var getResult = test.getFileStatus("test");
            Assert.IsTrue(getResult[0].CompareTo(testFileStatus[3]) == 0);
            Assert.IsTrue(getResult[1].CompareTo(testFileStatus[2]) == 0);
            Assert.IsTrue(getResult[2].CompareTo(testFileStatus[0]) == 0);
            Assert.IsTrue(getResult[3].CompareTo(testFileStatus[1]) == 0);
        }


        List<BasicDailyDataInformation> testBasicDailyData = new List<BasicDailyDataInformation>() {
            new BasicDailyDataInformation("2015-2-10\t12345\t2345\t3.3\t3.4\t3.2\t3.5\t1.1\t20031"),
            new BasicDailyDataInformation("2015-2-12\t12345\t2345\t3.3\t3.4\t3.2\t3.5\t1.1\t20031"),
            new BasicDailyDataInformation("2015-2-11\t12345\t2345\t3.3\t3.4\t3.2\t3.5\t1.1\t20031")
        };
        [TestMethod()]
        public void saveBasicDailyDataTest()
        {
            var test = new BasicDailyDataManager();
            if (Directory.Exists(@"D:\AlphaS\TestBasicDailyData"))
            {
                Directory.Delete(@"D:\AlphaS\TestBasicDailyData", true);
            }
            test.setBaseFolder(@"D:\AlphaS\TestBasicDailyData");
            test.saveBasicDailyData("test", testBasicDailyData);
            var getResult = test.getBasicDailyData("test");
            Assert.IsTrue(getResult[0].CompareTo(testBasicDailyData[0]) == 0);
            Assert.IsTrue(getResult[1].CompareTo(testBasicDailyData[2]) == 0);
            Assert.IsTrue(getResult[2].CompareTo(testBasicDailyData[1]) == 0);
        }
    }
}