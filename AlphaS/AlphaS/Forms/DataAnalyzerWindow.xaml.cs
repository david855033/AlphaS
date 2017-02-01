using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AlphaS.CoreNS;
using AlphaS.DataAnalyzer;
using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer.ParameterCalculators;

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

        private void UpdateDiv(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            var basicData0050 = Core.basicDailyDataManager.getBasicDailyData("0050");

            foreach (var stock in Core.stockListManager.getStockList())
            {
                string ID = stock.ID, type = stock.type;
                viewModel.display += $"[UpdateDiv ID = {ID}]({type})\r\n";
                dataAnalyzer.setStockType(type);
                if (ID != "0050") dataAnalyzer.set0050BasicData(basicData0050);
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));

                dataAnalyzer.standarizeAnalyzeData();
                viewModel.display += dataAnalyzer.getDisplay();

                Core.analyzedDataManager.saveAnalyzedData(ID, dataAnalyzer.getAnalyzedData());
                viewModel.display += "\r\n";
            }
        }

        private void CalculateParameter(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                viewModel.display += $"[GetFuturePrice ID = {ID}]\r\n";
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));

                dataAnalyzer.calculateParameter();
                viewModel.display += dataAnalyzer.getDisplay();

                Core.analyzedDataManager.saveAnalyzedData(ID, dataAnalyzer.getAnalyzedData());
                viewModel.display += "\r\n";
            }
        }

        private void GetFulturePrice(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            var basicData0050 = Core.basicDailyDataManager.getBasicDailyData("0050");
            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                if (ID != "0050") dataAnalyzer.set0050BasicData(basicData0050);
                viewModel.display += $"[GetFulturePrice ID = {ID}]\r\n";
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));
                dataAnalyzer.setFuturePriceData(Core.futurePriceDataManager.getFuturePriceData(ID));

                dataAnalyzer.calculateFuturePriceData();
                viewModel.display += dataAnalyzer.getDisplay();

                Core.futurePriceDataManager.saveFuturePriceData(ID, dataAnalyzer.getFuturePriceData());
                viewModel.display += "\r\n";
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

            foreach (var dateToCalculate in dateList)
            {
                var futurePriceStockDataInADay = new List<FuturePriceStockInfromation>();
                foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
                {
                    var futurePriceData = Core.futurePriceDataManager.getFuturePriceData(ID);
                    var findIndex = futurePriceData.FindIndex(x => x.date == dateToCalculate);
                    if (findIndex >= 0)
                    {
                        var matchedDateAndID = futurePriceData[findIndex];
                        var newData = new FuturePriceStockInfromation(ID, matchedDateAndID);
                        futurePriceStockDataInADay.Add(newData);
                    }
                }

                viewModel.display +=
                    $"date = {dateToCalculate.ToShortDateString()} stockCount = {futurePriceStockDataInADay.Count()}\r\n";

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
                    var futurePriceData = Core.futurePriceDataManager.getFuturePriceData(ID);
                    var findIndexDate = futurePriceData.FindIndex(x => x.date == dateToCalculate);
                    if (findIndexDate >= 0 && findIndexStock >= 0)
                    {
                        futurePriceData[findIndexDate].futurePriceRank =
                            futurePriceStockDataInADay[findIndexStock].futurePriceRank;
                    }
                    Core.futurePriceDataManager.saveFuturePriceData(ID, futurePriceData);
                }
            }
        }

        private void AppendParameterFuturePriceTable(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            foreach (string parameterName in AnalyzedDataInformation.parameterIndex.Keys)
            {
                Core.parameterFuturePriceTableManager.resetParameterFuturePriceTable(parameterName);
            }

            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                viewModel.display += $"[Append Parameter Future Price, ID = {ID}]\r\n";
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

                viewModel.display += dataAnalyzer.getDisplay();
                viewModel.display += "\r\n";
            }
        }

        private void CalculateParameterFuturePriceTable(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            foreach (string parameterName in AnalyzedDataInformation.parameterIndex.Keys)
            {
                dataAnalyzer.setParameterFuturePriceTableData(
                    Core.parameterFuturePriceTableManager.getParameterFuturePriceTable(parameterName));

                dataAnalyzer.calculateParameterFuturePriceTable();

                Core.finalParameterFuturePriceTableManager.saveParameterFuturePriceTable(
                    parameterName, dataAnalyzer.getFinalParameterFuturePriceTableData());
            }
        }

        private void GetStockScore(object sender, RoutedEventArgs e)
        {
            IDataAnalyzer dataAnalyzer = new DataAnalyzer.DataAnalyzer();
            viewModel.display = "";
            dataAnalyzer.resetParameterFuturePriceDictionary();
            foreach (string parameterName in AnalyzedDataInformation.parameterIndex.Keys)
            {
                dataAnalyzer.appendParameterFuturePriceDictionary(parameterName, Core.finalParameterFuturePriceTableManager.getParameterFuturePriceTable(parameterName));
            }

            foreach (var ID in Core.stockListManager.getStockList().Select(x => x.ID))
            {
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));
                dataAnalyzer.setScoreData(Core.scoreDataManager.getScoreData(ID));

                dataAnalyzer.calculateScoreData();

                Core.scoreDataManager.saveScoreData(ID, dataAnalyzer.getScoreData());

                viewModel.display += dataAnalyzer.getDisplay();
                viewModel.display += "\r\n";
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
            }
        }
    }
}
