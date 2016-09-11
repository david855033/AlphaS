using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading;
using System.Windows;

namespace AlphaS.BasicDailyData
{
    abstract public class BasicDailyDataDownloaderProtoType
    {
        abstract public void getBasicDailyDataByYearMonth(string ID, int year, int month);
    }

    public class BasicDailyDataDownloader : BasicDailyDataDownloaderProtoType
    {
        private WebBrowser webBrowser;
        public bool working = true;
        public List<BasicDailyDataInformation> BasicDailyDatas;

        public BasicDailyDataDownloader(WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;

        }

        public override void getBasicDailyDataByYearMonth(string ID, int year, int month)
        {
            working = true;
            BasicDailyDatas = new List<BasicDailyDataInformation>();
            webBrowser.LoadCompleted += loadComplete;
            webBrowser.Navigate(@"http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY/STOCK_DAYMAIN.php");
        }

        void loadComplete(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var HTMLDocument = webBrowser.Document;
            BasicDailyDatas.Add(new BasicDailyDataInformation());
            working = false;
            MessageBox.Show("work done");
        }

    }
}
