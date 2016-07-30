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
        List<Window> windowList = new List<Window>();
        StockListWindow stockListWindow;

        public MainWindow()
        {
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("MainwindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("MainwindowPostitionTop").getIntFromString();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) //初始化所有子視窗
        {
            stockListWindow = new StockListWindow(this);
            windowList.Add(stockListWindow);
            foreach (var w in windowList)
            {
                if (Core.settingManager.getSetting($"{w.GetType()}isVisible") == "True")
                {
                    stockListWindow.Show();
                }
            }
        }
        private void Window_Unload(object sender, RoutedEventArgs e) //結束所有子視窗
        {
            Core.closeAllWindow = true;
            foreach (var w in windowList)
            {
                Core.settingManager.saveSetting($"{w.GetType()}isVisible", w.IsVisible.ToString());
                w.Close();
            }
            Core.settingManager.saveSetting("MainwindowPostitionLeft", this.Left.ToString());
            Core.settingManager.saveSetting("MainwindowPostitionTop", this.Top.ToString());
        }

        private void toggleStockList(object sender, RoutedEventArgs e)
        {
            if (stockListWindow.IsVisible)
            {
                stockListWindow.Hide();
            }
            else
            {
                stockListWindow.Show();
            }
        }
    }
}
