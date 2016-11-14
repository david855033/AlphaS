using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlphaS.DataAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.BasicDailyData;

namespace AlphaS.DataAnalyzer.Tests
{
    [TestClass()]
    public class DataAnalyzerTests
    {

        List<BasicDailyDataInformation> basicDailyData = new List<BasicDailyDataInformation>()
        {
            new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-1"), open=1, close=1, high=1, low=1, change=0 },
            new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-2"), open=2, close=2, high=2, low=2, change=1 },
            new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-3"), open=4, close=4, high=4, low=4, change=2 },
            new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-4"), open=2, close=2, high=2, low=2, change=0 },
            new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-5"), open=1, close=1, high=1, low=1, change=-1 },
            new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-5"), open=2, close=2, high=2, low=2, change=0 }
        };
        List<AnalyzedDataInformation> analyzedData = new List<AnalyzedDataInformation>()
        {
               new AnalyzedDataInformation(new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-1"), open=1, close=1, high=1, low=1, change=0})
               { divideWeight = 1.5},
               new AnalyzedDataInformation(new BasicDailyDataInformation() { date = DateTime.Parse("2016-1-2"), open=2, close=2, high=2, low=2, change=1 })
                { divideWeight = 1.5}
        };

        List<AnalyzedDataInformation> analyzedDataNull = new List<AnalyzedDataInformation>();

        [TestMethod()]
        public void calculateDivideDataTest_DataInbasicDailyAndAnalyzedDataShouldEqual()
        {
            DataAnalyzer dataAnalyzer = new DataAnalyzer();
            dataAnalyzer.setBasicDailyData(basicDailyData);
            dataAnalyzer.setAnalyzedData(analyzedDataNull);

            dataAnalyzer.standarizeAnalyzeData();

            for (int i = 0; i < basicDailyData.Count; i++)
            {
                Assert.IsTrue(dataAnalyzer.getAnalyzedData()[i].date == dataAnalyzer.getAnalyzedData()[i].date);
            }
        }

        [TestMethod()]
        public void calculateDivideDataTest_DataInbasicDailyAndAnalyzedDataShouldEqualWhileHasPreviousAnalyzedData()
        {
            DataAnalyzer dataAnalyzer = new DataAnalyzer();
            dataAnalyzer.setBasicDailyData(basicDailyData);
            dataAnalyzer.setAnalyzedData(analyzedData);

            dataAnalyzer.standarizeAnalyzeData();

            for (int i = 0; i < basicDailyData.Count; i++)
            {
                Assert.IsTrue(dataAnalyzer.getAnalyzedData()[i].date == dataAnalyzer.getAnalyzedData()[i].date);
            }
        }

        [TestMethod()]
        public void calculateDivideDataTest_WeightTestNullAnalyzeData()
        {
            DataAnalyzer dataAnalyzer = new DataAnalyzer();
            dataAnalyzer.setBasicDailyData(basicDailyData);
            dataAnalyzer.setAnalyzedData(analyzedDataNull);
            dataAnalyzer.standarizeAnalyzeData();
            Assert.AreEqual(1, dataAnalyzer.getAnalyzedData()[0].divideWeight);
            Assert.AreEqual(1, dataAnalyzer.getAnalyzedData()[1].divideWeight);
            Assert.AreEqual(1, dataAnalyzer.getAnalyzedData()[2].divideWeight);
            Assert.AreEqual(2, dataAnalyzer.getAnalyzedData()[3].divideWeight);
            Assert.AreEqual(2, dataAnalyzer.getAnalyzedData()[4].divideWeight);
            Assert.AreEqual(1, dataAnalyzer.getAnalyzedData()[5].divideWeight);
        }

        [TestMethod()]
        public void calculateDivideDataTest_WeightTestPartialAnalyzeData()
        {
            DataAnalyzer dataAnalyzer = new DataAnalyzer();
            dataAnalyzer.setBasicDailyData(basicDailyData);
            foreach (var a in analyzedData) a.setNprice();
            dataAnalyzer.setAnalyzedData(analyzedData);
            dataAnalyzer.standarizeAnalyzeData();
            Assert.AreEqual(1.5, dataAnalyzer.getAnalyzedData()[0].divideWeight);
            Assert.AreEqual(1.5, dataAnalyzer.getAnalyzedData()[1].divideWeight);
            Assert.AreEqual(1.5, dataAnalyzer.getAnalyzedData()[2].divideWeight);
            Assert.AreEqual(3, dataAnalyzer.getAnalyzedData()[3].divideWeight);
            Assert.AreEqual(3, dataAnalyzer.getAnalyzedData()[4].divideWeight);
            Assert.AreEqual(1.5, dataAnalyzer.getAnalyzedData()[5].divideWeight);
        }
    }
}