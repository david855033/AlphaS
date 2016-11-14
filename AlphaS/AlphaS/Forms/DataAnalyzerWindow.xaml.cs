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
                viewModel.display += $"[CalculateParameter ID = {ID}]\r\n";
                dataAnalyzer.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData(ID));
                dataAnalyzer.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData(ID));

                dataAnalyzer.calculateParameter();
                viewModel.display += dataAnalyzer.getDisplay();

                Core.analyzedDataManager.saveAnalyzedData(ID, dataAnalyzer.getAnalyzedData());
                viewModel.display += "\r\n";
            }
        }
    }
}
