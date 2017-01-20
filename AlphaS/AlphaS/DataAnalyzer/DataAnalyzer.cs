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
            calculators.Add(new AverageCostCalculator(analyzedData, addDisplay));
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

        private List<FuturePriceDataInformation> futurePriceData;
        public void setFuturePriceData(List<FuturePriceDataInformation> FuturePriceData)
        {
            this.futurePriceData = FuturePriceData;
        }
        public List<FuturePriceDataInformation> getFuturePriceData()
        {
            return futurePriceData;
        }
        public void calculateFuturePriceData()
        {
            display = "";
            generateEmptyandRecentEmptyDateList();
            int startCalculationIndex = 0;
            int endCalculationIndex = 0;

            int existFuturePriceDataCount = futurePriceData.Count;
            addDisplay($"- exist Future Price Data Count = {existFuturePriceDataCount}");

            int existAnalyzedDataCount = analyzedData.Count;
            addDisplay($"- exist Analyzed Data Count = {existAnalyzedDataCount}");

            int MAX_AFTER_DAYS = FuturePriceDataInformation.FUTURE_PRICE_DAYS.Last();
            if (existFuturePriceDataCount + BaseParameterCalculator.PRE_DATA >= existAnalyzedDataCount - MAX_AFTER_DAYS)
            {
                addDisplay("- no new data to calculate");
                return;
            }
            else
            {
                startCalculationIndex = Math.Max(BaseParameterCalculator.PRE_DATA, existFuturePriceDataCount + 1);
                endCalculationIndex = existAnalyzedDataCount - MAX_AFTER_DAYS;
                addDisplay($"- Calculate from index={startCalculationIndex} to {endCalculationIndex}");
            }


            for (int i = startCalculationIndex; i <= endCalculationIndex; i++)
            {
                var newFuturePriceData = new FuturePriceDataInformation(analyzedData[i]);

                for (int n = 0; n < FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length; n++)
                {
                    int dayAfter = FuturePriceDataInformation.FUTURE_PRICE_DAYS[n];
                    newFuturePriceData.futurePrices[n] =
                        analyzedData[i + dayAfter - 1].N_avg / analyzedData[i].N_avg * 100 - 100;
                }

                futurePriceData.Add(newFuturePriceData);
            }
        }

        private List<ParameterFuturePriceTableInformation> parameterFuturePriceTableData;
        public void setParameterFuturePriceTableData(List<ParameterFuturePriceTableInformation> parameterFuturePriceTableData)
        {
            this.parameterFuturePriceTableData = parameterFuturePriceTableData;
        }
        public List<ParameterFuturePriceTableInformation> getParameterFuturePriceTableData()
        {
            return parameterFuturePriceTableData;
        }
        public void getParameterFuturePriceTableDataToAppend(Dictionary<string, List<ParameterFuturePriceTableInformation>> allDataToAppend)
        {
            display = "";
            var parameterIndex = AnalyzedDataInformation.parameterIndex;

            var dateList = from q in futurePriceData select q.date;

            foreach (var parameterNameAndIndex in parameterIndex)
            {
                allDataToAppend.Add(parameterNameAndIndex.Key, new List<ParameterFuturePriceTableInformation>());
                foreach (var date in dateList)
                {
                    var matchedFuturePriceData = futurePriceData.Find(x => x.date == date);
                    if (matchedFuturePriceData.futurePriceRank.Contains(null) ||
                        matchedFuturePriceData.futurePrices.Contains(null)
                        ) break;

                    var matchedAnalyzedData = analyzedData.Find(x => x.date == date);
                    var newParameterFuturePriceToAdd = new ParameterFuturePriceTableInformation();
                    newParameterFuturePriceToAdd.parameterValue = matchedAnalyzedData.parameters[parameterNameAndIndex.Value].GetValueOrDefault();

                    for (int i = 0; i < matchedFuturePriceData.futurePrices.Length; i++)
                    {
                        var currentFuturePrice = matchedFuturePriceData.futurePrices[i].GetValueOrDefault();
                        var currentFuturePriceLog = Math.Log(((currentFuturePrice / 100) + 1).getDoubleFromDecimal());
                        newParameterFuturePriceToAdd.futurePriceLogs[i] = currentFuturePriceLog.round(4).getDecimalFromDouble();
                    }

                    newParameterFuturePriceToAdd.futurePriceRanks = matchedFuturePriceData.futurePriceRank;

                    allDataToAppend[parameterNameAndIndex.Key].Add(newParameterFuturePriceToAdd);
                }
            }
        }

        private Dictionary<string, List<ParameterFuturePriceTableInformation>> parameterFuturePriceDictionary;
        public void resetParameterFuturePriceDictionary()
        {
            parameterFuturePriceDictionary = new Dictionary<string, List<ParameterFuturePriceTableInformation>>();
        }
        public void appendParameterFuturePriceDictionary(string parameterName, List<ParameterFuturePriceTableInformation> ParameterFuturePriceList)
        {
            parameterFuturePriceDictionary.Add(parameterName, ParameterFuturePriceList);
        }
        public Dictionary<string, List<ParameterFuturePriceTableInformation>> getParameterFuturePriceDictionary()
        {
            return parameterFuturePriceDictionary;
        }

        private List<ScoreDataInformation> scoreData;
        public List<ScoreDataInformation> getScoreData()
        {
            return scoreData;
        }
        public void setScoreData(List<ScoreDataInformation> scoreData)
        {
            this.scoreData = scoreData;
        }
        public void calculateScoreData()
        {
            display = "";
            DateTime latestDate = DateTime.MinValue;
            if (scoreData.Count > 0) latestDate = scoreData.Last().date;
            var analyzedDataToCalculate = from q in analyzedData
                                          where q.date > latestDate
                                          select q;
            display += $"latest date = {latestDate}, to calculate = {analyzedDataToCalculate.Count()}\r\n";
            foreach (var currentAnalyzedData in analyzedData)
            {
                bool hasInvalidData = currentAnalyzedData.parameters.Contains(null);
                if (hasInvalidData) continue;

                var currentData = currentAnalyzedData.date;
                int totalParameterCount = AnalyzedDataInformation.parameterIndex.Count();

                var newScoreData = new ScoreDataInformation();
                newScoreData.date = currentData;

                decimal[] valueScore = new decimal[ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length];
                decimal[] rankScore = new decimal[ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length];
                foreach (var parameterName in AnalyzedDataInformation.parameterIndex.Keys)
                {
                    var lookUpTable = parameterFuturePriceDictionary[parameterName];
                    var index = AnalyzedDataInformation.parameterIndex[parameterName];
                    var parameterValue = currentAnalyzedData.parameters[index].GetValueOrDefault();
                    decimal?[] logArray = lookUpLogFromParameter(parameterValue, lookUpTable);
                    decimal?[] rankArray = lookUpRankFromParameter(parameterValue, lookUpTable);
                    valueScore = valueScore.addUpDecimalArray(getScoreFromArray(logArray));
                    rankScore = rankScore.addUpDecimalArray(getScoreFromArray(rankArray));
                }

                newScoreData.valueScore = valueScore.divideElementBy(AnalyzedDataInformation.parameterIndex.Keys.Count).exp().round(2);
                newScoreData.rankScore = rankScore.divideElementBy(AnalyzedDataInformation.parameterIndex.Keys.Count).round(2);

                scoreData.Add(newScoreData);
            }
        }
        private decimal?[] lookUpLogFromParameter(decimal parameterValue, List<ParameterFuturePriceTableInformation> lookUpTable)
        {
            int i = 0;
            while (parameterValue > lookUpTable[i].parameterValue)
            {
                i++;
                if (i == lookUpTable.Count)
                {
                    i--;
                    break;
                }
            }

            return lookUpTable[i].futurePriceLogs;
        }
        private decimal?[] lookUpRankFromParameter(decimal parameterValue, List<ParameterFuturePriceTableInformation> lookUpTable)
        {
            int i = 0;
            while (parameterValue > lookUpTable[i].parameterValue)
            {
                i++;
                if (i == lookUpTable.Count)
                {
                    i--;
                    break;
                }
            }
            return lookUpTable[i].futurePriceRanks;
        }
        private decimal[] getScoreFromArray(decimal?[] array)
        {
            int j = 0;
            decimal[] result = new decimal[ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length];
            for (int i = 0; i < ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length; i++)
            {
                var correspondingIndex = new List<int>();
                int cutoff = ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION[i];

                for (; j < FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length; j++)
                {
                    if (FuturePriceDataInformation.FUTURE_PRICE_DAYS[j]
                        <= ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION[i])
                    {
                        correspondingIndex.Add(j);
                    }
                    else
                    {
                        break;
                    }
                }
                decimal sum = 0;
                foreach (var index in correspondingIndex)
                {
                    sum += array[index].GetValueOrDefault();
                }
                result[i] = sum / correspondingIndex.Count;
            }
            return result;
        }

        int PARAMETER_GROUP_COUNT = 15;
        private List<ParameterFuturePriceTableInformation> finalParameterFuturePriceTableData;
        public List<ParameterFuturePriceTableInformation> getFinalParameterFuturePriceTableData()
        {
            return finalParameterFuturePriceTableData;
        }
        public void calculateParameterFuturePriceTable()
        {
            display = "";
            var allParameterValues = new List<decimal>();
            foreach (var currentData in parameterFuturePriceTableData)
            {
                var index = allParameterValues.BinarySearch(currentData.parameterValue);
                if (index < 0) index = ~index;
                allParameterValues.Insert(index, currentData.parameterValue);
            }

            var futurePriceLogsGroupByParameter = new List<decimal?[]>[PARAMETER_GROUP_COUNT];
            for (int i = 0; i < PARAMETER_GROUP_COUNT; i++)
            { futurePriceLogsGroupByParameter[i] = new List<decimal?[]>(); }

            var futurePriceRanksGroupByParameter = new List<decimal?[]>[PARAMETER_GROUP_COUNT];
            for (int i = 0; i < PARAMETER_GROUP_COUNT; i++)
            { futurePriceRanksGroupByParameter[i] = new List<decimal?[]>(); }

            decimal[] cutoffs = new decimal[PARAMETER_GROUP_COUNT - 1];
            for (int i = 0; i < cutoffs.Length; i++)
            {
                int indexOfCutoff = allParameterValues.Count * (i + 1) / PARAMETER_GROUP_COUNT;
                cutoffs[i] = allParameterValues[indexOfCutoff];
                if (i > 0 && cutoffs[i] == cutoffs[i - 1]) cutoffs[i] += 0.01M;
            }

            foreach (var data in parameterFuturePriceTableData)
            {
                int i = 0;
                for (; i < cutoffs.Length; i++)
                    if (data.parameterValue < cutoffs[i])
                        break;
                futurePriceLogsGroupByParameter[i].Add(data.futurePriceLogs);
                futurePriceRanksGroupByParameter[i].Add(data.futurePriceRanks);
            }

            decimal[,] finalFuturePriceLogsTable = new decimal[futurePriceLogsGroupByParameter.Length, FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length];

            decimal[,] finalFuturePriceRanksTable = new decimal[futurePriceLogsGroupByParameter.Length, FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length];

            for (int i = 0; i < futurePriceLogsGroupByParameter.Length; i++)
            {
                for (int j = 0; j < FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length; j++)
                {
                    {
                        var queryPriceLogs = from q in futurePriceLogsGroupByParameter[i]
                                             orderby q[j]
                                             select q[j];
                        finalFuturePriceLogsTable[i, j] = queryPriceLogs.ToArray()[queryPriceLogs.Count() / 2].Value;
                    }
                    {
                        var queryPriceRanks = from q in futurePriceRanksGroupByParameter[i]
                                              orderby q[j]
                                              select q[j];
                        finalFuturePriceRanksTable[i, j] = queryPriceRanks.ToArray()[queryPriceRanks.Count() / 2].Value;
                    }
                }
            }

            finalParameterFuturePriceTableData = new List<ParameterFuturePriceTableInformation>();
            for (int i = 0; i < PARAMETER_GROUP_COUNT; i++)
            {
                var newData = new ParameterFuturePriceTableInformation();
                if (i == cutoffs.Length)
                {
                    newData.parameterValue = 0;
                }
                else
                {
                    newData.parameterValue = cutoffs[i];
                }

                newData.futurePriceLogs = new decimal?[FuturePriceDataInformation.FUTURE_PRICE_DAYS.Count()];
                for (int j = 0; j < newData.futurePriceLogs.Length; j++)
                {
                    newData.futurePriceLogs[j] = finalFuturePriceLogsTable[i, j];
                    newData.futurePriceRanks[j] = finalFuturePriceRanksTable[i, j];
                }
                finalParameterFuturePriceTableData.Add(newData);
            }
        }


    }
}
