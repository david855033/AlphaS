using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer.ParameterCalculators;

namespace AlphaS.DataAnalyzer
{
    public class DataAnalyzer : IDataAnalyzer
    {
        private string stockType;
        public void setStockType(string type)
        {
            stockType = type;
        }

        private List<BasicDailyDataInformation> basicData0050;
        public void set0050BasicData(List<BasicDailyDataInformation> basicData0050)
        {
            this.basicData0050 = basicData0050;
        }

        private List<AnalyzedDataInformation> analyzedData;
        public void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData)
        {
            this.analyzedData = AnalyzedData.ToList();
        }
        public List<AnalyzedDataInformation> getAnalyzedData()
        {
            return analyzedData;
        }

        private List<BasicDailyDataInformation> basicDailyData;
        public void setBasicDailyData(List<BasicDailyDataInformation> BasicDailyData)
        {
            this.basicDailyData = BasicDailyData.ToList();
        }
        public List<BasicDailyDataInformation> getBasicDailyData()
        {
            return basicDailyData;
        }

        List<DateTime> emptyDateList, recentEmptyDateList;
        public void standarizeAnalyzeData()
        {
            display = "";
            generateEmptyandRecentEmptyDateList();
            DateTime lastDateInAnalyzedData = DateTime.MinValue;
            if (analyzedData.Count > 0)
            {
                lastDateInAnalyzedData = analyzedData.Last().date;
                addDisplay($"Last Date In Analyzed Data = {lastDateInAnalyzedData.ToShortDateString()}");
            }
            else
            {
                addDisplay($"Emtpy Analyzed Data");
            }
            addDisplay($"expected data to calculate = { basicDailyData.Count() - analyzedData.Count() }");

            int startIndexInBasicDailyData = basicDailyData.FindIndex(x => x.date > lastDateInAnalyzedData);
            if (startIndexInBasicDailyData < 0)
            {
                addDisplay("no data to update");
                return;
            }
            addDisplay($"start date  In Basic Daily Data = {basicDailyData.Find(x => x.date > lastDateInAnalyzedData).date.ToShortDateString()}");
            addDisplay($"start Index In Basic Daily Data = {startIndexInBasicDailyData}");

            double startWeight = getStartWeight();
            addDisplay($"start weight  = {startWeight}");
            for (int i = startIndexInBasicDailyData; i < basicDailyData.Count(); i++)
            {
                var newAnalyzedData = new AnalyzedDataInformation(basicDailyData[i]);
                if (stockType == "A")
                {
                    newAnalyzedData.dealedStock /= 1000;
                    newAnalyzedData.volume /= 1000;
                }


                if (i == 0)
                {
                    newAnalyzedData.divideWeight = startWeight;
                }
                else
                {
                    if (newAnalyzedData.close == 0)
                    {
                        newAnalyzedData.close = analyzedData.Last().close;
                        newAnalyzedData.open = analyzedData.Last().open;
                        newAnalyzedData.low = analyzedData.Last().low;
                        newAnalyzedData.high = analyzedData.Last().high;
                        newAnalyzedData.change = 0;
                    }

                    decimal expectChange = newAnalyzedData.close - analyzedData.Last().close;
                    newAnalyzedData.divide = (expectChange - newAnalyzedData.change) * -1;
                    if (basicDailyData[i - 1].close == 0) newAnalyzedData.divide = 0;
                    if (newAnalyzedData.divide == 0)
                    {
                        newAnalyzedData.divideWeight = analyzedData.Last().divideWeight;
                    }
                    else // divide
                    {
                        newAnalyzedData.divideWeight =
                            analyzedData.Last().divideWeight *
                            (1 + newAnalyzedData.divide.getDoubleFromDecimal() / newAnalyzedData.close.getDoubleFromDecimal());
                    }
                }

                if (newAnalyzedData.dealedOrder != 0)
                {
                    newAnalyzedData.volumePerOrder = newAnalyzedData.volume / newAnalyzedData.dealedOrder;
                }
                else { newAnalyzedData.volumePerOrder = 0; }

                newAnalyzedData.setNprice();

                if (recentEmptyDateList.Contains(newAnalyzedData.date))
                {
                    newAnalyzedData.recentEmpty = true;
                }

                analyzedData.Add(newAnalyzedData);
            }
            addDisplay($"last date   = {analyzedData.Last().date.ToShortDateString()}");
        }
        private void generateEmptyandRecentEmptyDateList()
        {
            emptyDateList = new List<DateTime>();
            recentEmptyDateList = new List<DateTime>();
            if (basicData0050 == null)
            {
                addDisplay($"no need to count empty");
            }
            else
            {
                var datesIn0050 = basicData0050.Select(x => x.date).ToArray();
                var dateInThisBasicData = basicDailyData.Select(x => x.date).ToArray();

                int RecentEmptyCount = 0;
                for (int i = 0; i < datesIn0050.Length; i++)
                {
                    if (Array.BinarySearch(dateInThisBasicData, datesIn0050[i]) < 0)
                    {
                        emptyDateList.Add(datesIn0050[i]);
                        RecentEmptyCount = 120;
                    }
                    if (RecentEmptyCount > 0)
                    {
                        recentEmptyDateList.Add(datesIn0050[i]);
                        RecentEmptyCount--;
                    }
                }

            }
            addDisplay($"empty date: {emptyDateList.Count}");
        }

        private double getStartWeight()
        {
            double startWeight;
            if (analyzedData.Count == 0)
            {
                startWeight = 1;
            }
            else
            {
                startWeight = analyzedData.Last().divideWeight;
            }

            return startWeight;
        }

        
        public void calculateParameter()//新增的計算機掛在這邊
        {
            display = "";
            var calculators = new List<BaseParameterCalculator>();
            calculators.Add(new ChangeCalculator(analyzedData, addDisplay));
            calculators.Add(new BiasFromMeanAverageCalculator(analyzedData, addDisplay));
            calculators.Add(new AverageVolumeCalculator(analyzedData, addDisplay));
            calculators.Add(new AverageCostCalculator (analyzedData, addDisplay));
            calculators.Add(new VolumePerOrderCalculator(analyzedData, addDisplay));
            calculators.Add(new KDJCalculator(analyzedData, addDisplay));
            calculators.Add(new RSICalculator(analyzedData, addDisplay));
            foreach (var c in calculators) c.calculate();

        }


        private string display = "";
        private void addDisplay(string s) { display += s + "\r\n"; }
        public string getDisplay()
        {
            return display;
        }


    }
}
