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
            new BasicDailyDataFileStatusInformation("2016\t4\tValid\t2016-09-19 08:00"),
            new BasicDailyDataFileStatusInformation("2016\t5\tValid\t2016-09-19 20:00"),
            new BasicDailyDataFileStatusInformation("2016\t2\tValid\t2016-09-19 21:00"),
            new BasicDailyDataFileStatusInformation("2014\t5\tValid\t2016-09-19 22:00")
        };

        [TestMethod()]
        public void saveFileStatusTest()
        {
            var test = new BasicDailyDataManager();
            test.setBaseFolder(@"D:\AlphaS\TestBasicDailyData");
            test.saveFileStatus("test", testFileStatus.ToList());
            var result = test.getFileStatus("test");
            Assert.IsTrue(result[0].CompareTo(testFileStatus[3]) == 0);
            Assert.IsTrue(result[1].CompareTo(testFileStatus[2]) == 0);
            Assert.IsTrue(result[2].CompareTo(testFileStatus[0]) == 0);
            Assert.IsTrue(result[3].CompareTo(testFileStatus[1]) == 0);
        }
    }
}