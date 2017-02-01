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
using System.Threading;

namespace AlphaS.Forms
{
    /// <summary>
    /// BasicDailyDataWindow.xaml 的互動邏輯
    /// </summary>
    public partial class BasicDailyDataWindow : Window
    {
        MainWindow mainWindow;
        BasicDailyDataViewModel viewModel = new BasicDailyDataViewModel();
        public BasicDailyDataWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            this.Left = Core.settingManager.getSetting("BasicDailyDataWindowPostitionLeft").getIntFromString();
            this.Top = Core.settingManager.getSetting("BasicDailyDataWindowPostitionTop").getIntFromString();
            this.DataContext = viewModel;
            viewModel.acquiredData = "acquired data";
            viewModel.missionList = "missionList";
            viewModel.startYear = Core.settingManager.getSetting("BasicDailyDataWindowStartYear");
            viewModel.startMonth = Core.settingManager.getSetting("BasicDailyDataWindowStartMonth");
            viewModel.IsReadAll = false;
        }

        public static System.Windows.Forms.WebBrowser webBrowser = new System.Windows.Forms.WebBrowser();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
            host.Child = webBrowser;
            this.webBrowserGrid.Children.Add(host);
            webBrowser.ScriptErrorsSuppressed = true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Core.settingManager.saveSetting("BasicDailyDataWindowPostitionLeft", this.Left.ToString());
            Core.settingManager.saveSetting("BasicDailyDataWindowPostitionTop", this.Top.ToString());
            Core.settingManager.saveSetting("BasicDailyDataWindowStartYear", this.viewModel.startYear);
            Core.settingManager.saveSetting("BasicDailyDataWindowStartMonth", this.viewModel.startMonth);
            if (!Core.closeAllWindow)
            {
                this.Hide();
                e.Cancel = true;
            }
        }


        private void Button_Navigate_Click(object sender, RoutedEventArgs e)
        {
            initializeDownloadMission();
        }

        private void initializeDownloadMission()
        {
            IBasicDailyDataDownloader basicDailyDataDownloader = new BasicDailyDataDownloader();
            basicDailyDataDownloader.setWebBrowser(webBrowser);
            basicDailyDataDownloader.setViewModel(viewModel);

            IBasicDailyDataMissionListGenerator missionListGenerator = new BasicDailyDataMissionListGenerator();
            missionListGenerator.setStartYear(viewModel.startYear.getIntFromString());
            missionListGenerator.setStartMonth(viewModel.startMonth.getIntFromString());
            missionListGenerator.setStockList(Core.stockListManager.getStockList());

            var missionList = missionListGenerator.getMissionList(viewModel.IsReadAll);

            while (missionList.Count != 0)
            {
                basicDailyDataDownloader.setMission(missionList);
                basicDailyDataDownloader.startMission();
            }

        }

    }
}
