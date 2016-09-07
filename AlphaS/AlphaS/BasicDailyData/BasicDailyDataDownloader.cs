using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading;
namespace AlphaS.BasicDailyData
{
    abstract public class BasicDailyDataDownloaderProtoType
    {
        abstract public List<BasicDailyDataInformation> getBasicDailyDataByYearMonth(string ID, int year, int month);
    }

    public class BasicDailyDataDownloader : BasicDailyDataDownloaderProtoType
    {
        private WebBrowser webBrowser;
        public BasicDailyDataDownloader(WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
            
        }

        public override List<BasicDailyDataInformation> getBasicDailyDataByYearMonth(string ID, int year, int month)
        {
            List<BasicDailyDataInformation> BasicDailyDatas = new List<BasicDailyDataInformation>();

            webBrowser.LoadCompleted += loadComplete;
            webBrowser.Navigate(@"http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY/STOCK_DAYMAIN.php");
            return BasicDailyDatas;
        }

        void loadComplete(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            dynamic HTMLDocument = webBrowser.Document;
            var htmlText = HTMLDocument.documentElement.InnerHtml;
            System.Windows.MessageBox.Show(htmlText);
        }

    }
}
