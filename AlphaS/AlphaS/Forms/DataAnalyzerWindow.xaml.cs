using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AlphaS.CoreNS;
using AlphaS.DataAnalyzer;
using AlphaS.DataAnalyzer.ParameterCalculators;
using System.Windows.Threading;
using System.Threading;
using System;
using System.IO;

namespace AlphaS.Forms
{
    /// <summary>
    /// DataAnalyzerWindow.xaml 的互動邏輯
    /// </summary>
    public partial class DataAnalyzerWindow : Window
    {
        MainWindow mainWindow;
        DataAnalyzerViewModel viewModel = new DataAnalyzerViewModel();
        public DataAnalyzerWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("DataAnalyzerWindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("DataAnalyzerWindowPostitionTop").getIntFromString();
            this.DataContext = viewModel;
            viewModel.display = "console\r\n";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Core.settingManager.saveSetting("DataAnalyzerWindowPostitionLeft", this.Left.ToString());
            Core.settingManager.saveSetting("DataAnalyzerWindowPostitionTop", this.Top.ToString());
            if (!Core.closeAllWindow)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
        private void refreshText()
        {
            displayText.Text = viewModel.display;
            this.displayText.Refresh();
        }
        private void UpdateDiv(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            var basicData0050 = Core.basicDailyDataManager.getBasicDailyData("0050");
            int count = 0, all = Core.stockListManager.getStockList().Count();
            foreach (var stock in Core.stockListManager.getStockList())
            {
                string ID = stock.ID, type = stock.type;
                string output = $"[UpdateDiv ID = {ID}]({type}) ({++count}/{all})\r\n";
                dataAnalyzer.setStockType(type);
                if (ID != "0050") dataAnalyzer.set0050BasicData(basicData0050);
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));

                dataAnalyzer.standarizeAnalyzeData();
                output += dataAnalyzer.getDisplay();
                viewModel.display = output + "\r\n" + viewModel.display;

                Core.analyzedDataManager.saveAnalyzedData(ID, dataAnalyzer.getAnalyzedData());

                refreshText();
            }
        }

        private void CalculateParameter(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            int count = 0, all = Core.stockListManager.getStockList().Select(x => x.ID).Count();
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                string output =
                 $"[CalculateParameter ID = {ID}]  ({++count}/{all})\r\n";
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));

                dataAnalyzer.calculateParameter();
                output += dataAnalyzer.getDisplay();

                Core.analyzedDataManager.saveAnalyzedData(ID, dataAnalyzer.getAnalyzedData());

                viewModel.display = output + "\r\n" + viewModel.display;
                refreshText();
            }
        }

        private void GetFulturePrice(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            var basicData0050 = Core.basicDailyDataManager.getBasicDailyData("0050");
            int count = 0, all = Core.stockListManager.getStockList().Select(x => x.ID).Count();
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                if (ID != "0050") dataAnalyzer.set0050BasicData(basicData0050);
                string output = $"[GetFulturePrice ID = {ID}]  ({++count}/{all})\r\n";
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));
                dataAnalyzer.setFuturePriceData(Core.futurePriceDataManager.getFuturePriceData(ID));

                dataAnalyzer.calculateFuturePriceData();
                output += dataAnalyzer.getDisplay();

                Core.futurePriceDataManager.saveFuturePriceData(ID, dataAnalyzer.getFuturePriceData());
                viewModel.display = output + "\r\n" + viewModel.display;
                refreshText();
            }
        }

        private void GetFulturePriceRank(object sender, RoutedEventArgs e)
        {
            viewModel.display = "";

            var basicData0050 = Core.basicDailyDataManager.getBasicDailyData("0050");
            var dateList = basicData0050.Select(x => x.date).ToList();
            int MAX_FUTURE_PRICE_DAYAFTER = FuturePriceDataInformation.FUTURE_PRICE_DAYS.Last();
            dateList.RemoveRange(0, BaseParameterCalculator.PRE_DATA);
            dateList.RemoveRange(dateList.Count - 1 - MAX_FUTURE_PRICE_DAYAFTER + 2, MAX_FUTURE_PRICE_DAYAFTER - 2);
            int count = 0, all = dateList.Count();

            var futurePriceDataHolder = new Dictionary<string, List<FuturePriceDataInformation>>();
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                futurePriceDataHolder.Add(ID, Core.futurePriceDataManager.getFuturePriceData(ID));
            }
            viewModel.display += $"load {futurePriceDataHolder.Count()} future price files";
            refreshText();

            foreach (var dateToCalculate in dateList)
            {
                var futurePriceStockDataInADay = new List<FuturePriceStockInfromation>();
                foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
                {
                    var futurePriceData = futurePriceDataHolder[ID];
                    var indexInFuturePriceData = futurePriceData.FindIndex(x => x.date == dateToCalculate);
                    if (indexInFuturePriceData >= 0)
                    {
                        var matchedDateAndID = futurePriceData[indexInFuturePriceData];
                        if (matchedDateAndID.isLowVolume) { continue; }

                        var newData = new FuturePriceStockInfromation(ID, matchedDateAndID);
                        futurePriceStockDataInADay.Add(newData);
                    }
                }

                for (int i = 0; i < FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length; i++)
                {
                    futurePriceStockDataInADay.Sort(new FuturePriceStockComparer(i));
                    for (int rank = 0; rank < futurePriceStockDataInADay.Count(); rank++)
                    {
                        futurePriceStockDataInADay[rank].futurePriceRank[i] =
                            (
                            100M / (futurePriceStockDataInADay.Count() - 1) * rank
                            ).round(2);
                    }
                }
                futurePriceStockDataInADay.Sort();

                foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
                {
                    var findIndexStock = futurePriceStockDataInADay.FindIndex(x => x.stockID == ID);
                    if (findIndexStock >= 0 && futurePriceStockDataInADay[findIndexStock].futurePriceRank.Contains(null))
                        break;
                    var futurePriceData = futurePriceDataHolder[ID];
                    var findIndexDate = futurePriceData.FindIndex(x => x.date == dateToCalculate);
                    if (findIndexDate >= 0 && findIndexStock >= 0)
                    {
                        futurePriceData[findIndexDate].futurePriceRank =
                            futurePriceStockDataInADay[findIndexStock].futurePriceRank;
                    }
                }
                viewModel.display =
                    $"date = {dateToCalculate.ToShortDateString()} stockCount = {futurePriceStockDataInADay.Count()} ({++count}/{all})\r\n" + viewModel.display;
                refreshText();
            }
            viewModel.display =
                    $"saving result...\r\n" + viewModel.display;
            refreshText();
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                Core.futurePriceDataManager.saveFuturePriceData(ID, futurePriceDataHolder[ID]);
            }
            viewModel.display =
                    $"done!\r\n" + viewModel.display;
            refreshText();
        }

        private void AppendParameterFuturePriceTable(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            foreach (string parameterName in AnalyzedDataInformation.parameterIndexForScore.Keys)
            {
                Core.parameterFuturePriceTableManager.resetParameterFuturePriceTable(parameterName);
            }
            int count = 0, all = Core.stockListManager.getStockList().Select(x => x.ID).Count();
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                string output = $"[Append Parameter Future Price, ID = {ID}]   ({++count}/{all})\r\n";
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));
                dataAnalyzer.setFuturePriceData(Core.futurePriceDataManager.getFuturePriceData(ID));

                var allDataToAppend = new Dictionary<string, List<ParameterFuturePriceTableInformation>>();
                dataAnalyzer.getParameterFuturePriceTableDataToAppend(allDataToAppend);

                foreach (var dataPair in allDataToAppend)
                {
                    string parameterName = dataPair.Key;
                    List<ParameterFuturePriceTableInformation> dataToAppend = dataPair.Value;
                    Core.parameterFuturePriceTableManager.appendParameterFuturePrice(parameterName, dataToAppend);
                }

                output += dataAnalyzer.getDisplay();
                viewModel.display = output + "\r\n" + viewModel.display;
                refreshText();
            }
        }

        private void CalculateParameterFuturePriceTable(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            int count = 0, all = AnalyzedDataInformation.parameterIndexForScore.Keys.Count();
            foreach (string parameterName in AnalyzedDataInformation.parameterIndexForScore.Keys)
            {
                dataAnalyzer.setParameterFuturePriceTableData(
                    Core.parameterFuturePriceTableManager.getParameterFuturePriceTable(parameterName));

                dataAnalyzer.calculateParameterFuturePriceTable();
                viewModel.display = $"CalculateParameterFuturePriceTable: {parameterName} ({++count}/{all})\r\n" + viewModel.display;

                Core.finalParameterFuturePriceTableManager.saveParameterFuturePriceTable(
                    parameterName, dataAnalyzer.getFinalParameterFuturePriceTableData());

                refreshText();
            }
        }

        private void GetStockScore(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            dataAnalyzer.resetParameterFuturePriceDictionary();
            foreach (string parameterName in AnalyzedDataInformation.parameterIndexForScore.Keys)
            {
                dataAnalyzer.appendParameterFuturePriceDictionary(parameterName, Core.finalParameterFuturePriceTableManager.getParameterFuturePriceTable(parameterName));
            }

            int count = 0, all = Core.stockListManager.getStockList().Select(x => x.ID).Count();
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));
                dataAnalyzer.setScoreData(Core.scoreDataManager.getScoreData(ID));

                dataAnalyzer.calculateScoreData();

                Core.scoreDataManager.saveScoreData(ID, dataAnalyzer.getScoreData());

                viewModel.display = $"get stock score{ID}  ({++count}/{all})\r\n" + dataAnalyzer.getDisplay() + "\r\n" + viewModel.display;
                refreshText();
            }
        }

        private void ScoreFuturePriceEvaluationTable(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "Score vs Future Price \r\n";
            Core.scoreFuturePriceTableManager.resetScoreFuturePriceTable();

            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                viewModel.display += $"record ID {ID}\r\n";
                dataAnalyzer.setFuturePriceData(Core.futurePriceDataManager.getFuturePriceData(ID));
                dataAnalyzer.setScoreData(Core.scoreDataManager.getScoreData(ID));

                dataAnalyzer.MakeScoreFuturePriceEvaluationTable();

                Core.scoreFuturePriceTableManager.appendScoreFuturePriceTable(dataAnalyzer.getScoreFuturePriceTable());

                viewModel.display += dataAnalyzer.getDisplay();
                viewModel.display += "\r\n";
                refreshText();
            }
        }

        private void MakeDailyChart(object sender, RoutedEventArgs e)
        {
            viewModel.display = "";

            var stockList = Core.stockListManager.getStockList().Select(x => x.ID);

            var basicData0050 = Core.basicDailyDataManager.getBasicDailyData("0050");
            var dateList = basicData0050.Select(x => x.date).ToList();
            var existedDateList = Core.dailyChartDataManager.getExistedDate();
            existedDateList.Sort();
            var toCalculateDateList = from q in dateList where existedDateList.BinarySearch(q) < 0 select q;

            int count = 0,
                allCount = dateList.Count(),
                existedCount = existedDateList.Count(),
                toCalculateCount = toCalculateDateList.Count(); ;

            viewModel.display = $"all count = {allCount}, existed count = {existedCount}, to Calculate = {toCalculateCount}";
            refreshText();

            var AnalyzedDataHolder = new Dictionary<string, List<AnalyzedDataInformation>>();
            var ScoreDataHolder = new Dictionary<string, List<ScoreDataInformation>>();
            int stockCount = 0;
            foreach (var ID in stockList)
            {
                var analyzedDataToCalculate = (from q in Core.analyzedDataManager.getAnalyzedData(ID)
                                               where toCalculateDateList.Contains(q.date)
                                               select q).ToList();
                AnalyzedDataHolder.Add(ID, analyzedDataToCalculate);

                var scoreDataToCalculate = (from q in Core.scoreDataManager.getScoreData(ID)
                                            where toCalculateDateList.Contains(q.date)
                                            select q).ToList();
                ScoreDataHolder.Add(ID, scoreDataToCalculate);
                viewModel.display = $"load stock: {ID} for analyzed & score files ({++stockCount}/{stockList.Count()})" + "\r\n" + viewModel.display;
                refreshText();
            }
            viewModel.display = $"load {AnalyzedDataHolder.Count()} analyzed & score files" + "\r\n" + viewModel.display;
            refreshText();

            foreach (var currentDate in toCalculateDateList)
            {
                viewModel.display = $"date: {currentDate.ToShortDateString()} ({++count}/{toCalculateCount})" +
                    "\r\n" + viewModel.display;
                refreshText();
                var newDailyChart = new List<DailyChartInformation>();
                foreach (var ID in stockList)
                {
                    var selectedAnalyzedData = AnalyzedDataHolder[ID].Find(x => x.date == currentDate);
                    var selectedScoreData = ScoreDataHolder[ID].Find(x => x.date == currentDate);
                    if (selectedAnalyzedData != null && selectedScoreData != null)
                    {
                        var newDailyChartData = new DailyChartInformation(ID, selectedAnalyzedData, selectedScoreData);
                        newDailyChart.Add(newDailyChartData);
                    }
                }
                Core.dailyChartDataManager.saveDailyChart(currentDate, newDailyChart);
            }

            viewModel.display = $"done!!" + "\r\n" + viewModel.display;
            refreshText();
        }

        private void TradeSimulation(object sender, RoutedEventArgs e)
        {
            var dateList = Core.dailyChartDataManager.getExistedDate().FindAll(x => x.Date >= "2007-01-01".getDateTimeFromFileName());
            dateList.Sort();
            TradeSimulator tradeSimulator = new TradeSimulator();
            var protocals = generateTradProtocals();
            tradeSimulator.addTradingProtocals(protocals);
            tradeSimulator.initializedTradeSim();
            foreach (var currentDate in dateList)
            {
                tradeSimulator.goNextDay(currentDate, Core.dailyChartDataManager.getDailyChart(currentDate));
                viewModel.display = "trade simulating: " + currentDate.getFileNameFromDateTime();
                refreshText();
            }
            tradeSimulator.endSimulation(dateList.Last());
            viewModel.display = $"done!, total {protocals.Count} protocals";
            refreshText();
        }

        private List<TradingProtocal> generateTradProtocals()
        {
            List<TradingProtocal> result = new List<TradingProtocal>();

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    result.Add(new TradingProtocal()
                    {
                        valueScoreWeight = new decimal[] { 1m, 2m },
                        rankScoreWeight = new decimal[] { 0.5m, 0.5m },
                        divideParts = 20,
                        buyScoreThreshold = 0.3m,
                        sellScoreThreshold = -0.08m,
                        sellScoreThresholdDay = 3,
                        sellRankThreshold = 0,
                        buyPriceFromClose = 0.98m + i * 0.005m,
                        sellPriceFromClose = 0.90m + j * 0.005m
                    });
                }
            }

            return result;
        }

        private void GroupOrder(object sender, RoutedEventArgs e)
        {
            UpdateDiv(sender, e);
            CalculateParameter(sender, e);
            GetFulturePrice(sender, e);
            GetFulturePriceRank(sender, e);
            AppendParameterFuturePriceTable(sender, e);
            CalculateParameterFuturePriceTable(sender, e);
            GetStockScore(sender, e);
            MakeDailyChart(sender, e);
        }

    }
}
