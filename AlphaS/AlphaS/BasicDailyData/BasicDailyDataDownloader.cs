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
        abstract public void setMission(List<BasicDailyDataMission> mission);
        abstract public void startMission();
        abstract public List<BasicDailyDataInformation> getResult();
        abstract public bool checkAllWorkDone();
    }

    public class BasicDailyDataDownloader : BasicDailyDataDownloaderProtoType
    {
        private WebBrowser webBrowser;
        public BasicDailyDataDownloader(WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
        }


        List<BasicDailyDataMission> missionList;
        override public void setMission(List<BasicDailyDataMission> mission)
        {
            this.missionList = mission.ToList();
            isAllWorkDone = false;
        }

        List<BasicDailyDataInformation> BasicDailyDatas;
        public override List<BasicDailyDataInformation> getResult()
        {
            return BasicDailyDatas;
        }

        bool isAllWorkDone = false;
        public override bool checkAllWorkDone()
        {
            return isAllWorkDone;
        }

        public bool working = false;
        public override void startMission()
        {
            while (missionList.Count > 0)
            {
                BasicDailyDataMission currentMission = missionList.First();
                getBasicDailyDataByYearMonth(currentMission);


                missionList.Remove(currentMission);
            }
        }
        
        void getBasicDailyDataByYearMonth(BasicDailyDataMission currentMission)
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
