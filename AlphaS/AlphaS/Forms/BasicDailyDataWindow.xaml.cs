﻿using System;
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
            basicDailyDataDownloader.setViewModel(viewModel);
            var missionList = new List<BasicDailyDataMission>()
            {
                new BasicDailyDataMission() { ID="1101",year=2016,month=1,type="A"},
                new BasicDailyDataMission() { ID="1101",year=2016,month=2,type="A"},
                new BasicDailyDataMission() { ID="1101",year=2016,month=3,type="A"},
                new BasicDailyDataMission() { ID="1101",year=2016,month=4,type="A"},
                new BasicDailyDataMission() { ID="1101",year=2016,month=5,type="A"},
                new BasicDailyDataMission() { ID="1101",year=2016,month=6,type="A"},
                new BasicDailyDataMission() { ID="1101",year=2016,month=7,type="A"}
            };
            basicDailyDataDownloader.setMission(missionList);
            basicDailyDataDownloader.startMission();
        }

        public static System.Windows.Forms.WebBrowser webBrowser = new System.Windows.Forms.WebBrowser();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();

            host.Child = webBrowser;
            this.webBrowserGrid.Children.Add(host);

            webBrowser.ScriptErrorsSuppressed = true;
        }
    }
}
