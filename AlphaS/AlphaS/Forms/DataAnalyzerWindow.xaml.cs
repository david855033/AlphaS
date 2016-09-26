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
using AlphaS.DataAnalyzer.Divide;
namespace AlphaS.Forms
{
    /// <summary>
    /// DataAnalyzerWindow.xaml 的互動邏輯
    /// </summary>
    public partial class DataAnalyzerWindow : Window
    {
        MainWindow mainWindow;
        public DataAnalyzerWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("DataAnalyzerWindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("DataAnalyzerWindowPostitionTop").getIntFromString();
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
            IDivideCalculator divideCalculator = new DivideCalculator(); ;
            divideCalculator.setBasicDailyData(Core.basicDailyDataManager.getBasicDailyData("1101"));
            divideCalculator.setAnalyzedData(Core.analyzedDataManager.getAnalyzedData("1101"));
            divideCalculator.calculateDivideData();
            Core.basicDailyDataManager.saveBasicDailyData("1101",divideCalculator.getBasicDailyData());

        }
    }
}
