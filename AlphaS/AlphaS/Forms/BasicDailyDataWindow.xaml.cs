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
using AlphaS.BasicDailyData;

namespace AlphaS.Forms
{
    /// <summary>
    /// BasicDailyDataWindow.xaml 的互動邏輯
    /// </summary>
    public partial class BasicDailyDataWindow : Window
    {
        MainWindow mainWindow;
        public BasicDailyDataWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("BasicDailyDataWindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("BasicDailyDataWindowPostitionTop").getIntFromString();
        }

    

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Core.settingManager.saveSetting("BasicDailyDataWindowPostitionLeft", this.Left.ToString());
            Core.settingManager.saveSetting("BasicDailyDataWindowPostitionTop", this.Top.ToString());
            if (!Core.closeAllWindow)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void Button_Navigate_Click(object sender, RoutedEventArgs e)
        {
            var basicDailyDataDownloader = new BasicDailyDataDownloader(webBrowser);
            acquiredText.Text = BasicDailyDataInformation.ToTitle();
            foreach (var line in basicDailyDataDownloader.getBasicDailyDataByYearMonth("1101", 101, 5))
            {
                acquiredText.Text += line + "\n";
            }
        }

    
    }
}
