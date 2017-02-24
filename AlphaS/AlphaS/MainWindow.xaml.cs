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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlphaS.Forms;
using AlphaS.CoreNS;
namespace AlphaS
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string,Window> windowList = new Dictionary<string, Window> ();

        public MainWindow()
        {
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("MainwindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("MainwindowPostitionTop").getIntFromString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) //初始化所有子視窗
        {
           windowList.Add("StockListWindow", new StockListWindow(this));
           windowList.Add("BasicDailyDataWindow", new BasicDailyDataWindow(this));
            windowList.Add("DataAnalyzerWindow", new DataAnalyzerWindow(this));

            foreach (var pair in windowList)
            {
                if (Core.settingManager.getSetting($"{pair.Key}isVisible") == "True")
                {
                    pair.Value.Show();
                }
            }
        }
        
        private void Window_Unload(object sender, RoutedEventArgs e) //結束所有子視窗
        {
            Core.closeAllWindow = true;
            foreach (var pair in windowList)
            {
                Core.settingManager.saveSetting($"{pair.Key}isVisible", pair.Value.IsVisible.ToString());
                pair.Value.Close();
            }
            Core.settingManager.saveSetting("MainwindowPostitionLeft", this.Left.ToString());
            Core.settingManager.saveSetting("MainwindowPostitionTop", this.Top.ToString());
        }

        private void toggleWindow(string windowName)
        {
            if (windowList[windowName].IsVisible)
            {
                windowList[windowName].Hide();
            }
            else
            {
                windowList[windowName].Show();
            }
        }

        private void toggleStockList(object sender, RoutedEventArgs e)
        {
            toggleWindow("StockListWindow");
        }

        private void toggleBasicDailyData(object sender, RoutedEventArgs e)
        {
            toggleWindow("BasicDailyDataWindow");
        }

        private void toggleDataAnalyzer(object sender, RoutedEventArgs e)
        {
            toggleWindow("DataAnalyzerWindow");
        }
    }
}
