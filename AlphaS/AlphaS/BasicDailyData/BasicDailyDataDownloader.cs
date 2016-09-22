using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;
using System.Reflection;
using mshtml;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System;
using AlphaS.CoreNS;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataDownloader : IBasicDailyDataDownloader
    {
        private WebBrowser webBrowser;
        public void setWebBrowser(System.Windows.Forms.WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
        }
        public BasicDailyDataDownloader()
        {

        }

        private BasicDailyDataViewModel viewModel;
        public void setViewModel(BasicDailyDataViewModel viewModel) { this.viewModel = viewModel; }

        List<BasicDailyDataMission> missionList;
        public void setMission(List<BasicDailyDataMission> mission)
        {
            this.missionList = mission.ToList();
        }

        List<BasicDailyDataInformation> BasicDailyDatas = new List<BasicDailyDataInformation>();
        public List<BasicDailyDataInformation> getResult()
        {
            return BasicDailyDatas;
        }

        public bool webBrowserWorking = false;

        public void startMission()
        {
            webBrowser.DocumentCompleted += loadNextMission;
            viewModel.acquiredData = BasicDailyDataInformation.ToTitle();
            Thread.Sleep(300);
            webBrowser.Navigate(@"http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY/STOCK_DAYMAIN.php");
            currentWebSiteStockType = "A";
            currentMission = null;
        }

        int nullcount = 0;
        BasicDailyDataMission currentMission;
        string currentWebSiteStockType = "";

        void loadNextMission(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (missionList.Count == 0)
            {
                webBrowser.DocumentCompleted -= loadNextMission;
            }
            else if (currentMission == missionList.First())
            {
                recordDataInWebBrowser(webBrowser.Document);
            }
            
            if (missionList.Count > 0)
            {
                currentMission = missionList.First();
                if (currentMission.type != currentWebSiteStockType)
                {
                    changeWebSite(webBrowser, currentMission.type);
                }
                else
                {
                    Thread.Sleep(500);
                    selectIDandDateThenDoQuery(currentWebSiteStockType);
                    Thread.Sleep(500);
                }
            }
        }
    
    private void recordDataInWebBrowser(HtmlDocument doc)
    {

        viewModel.acquiredData = currentMission.ToString() + "\r\n";
        viewModel.acquiredData += BasicDailyDataInformation.ToTitle() + "\r\n";
        var basicDailyDataList = Core.basicDailyDataManager.getBasicDailyData(currentMission.ID);
        var fileStatusList = Core.basicDailyDataManager.getFileStatus(currentMission.ID);

        HtmlElement resultTable = getResultTable(doc);
        List<BasicDailyDataInformation> analyzedDataList = analysisDataTable(resultTable.InnerHtml);

        foreach (var currentBasicDailyData in analyzedDataList)
        {
            int index = basicDailyDataList.BinarySearch(currentBasicDailyData);
            if (index >= 0)
            {
                basicDailyDataList[index] = currentBasicDailyData;
            }
            else
            {
                basicDailyDataList.Insert(~index, currentBasicDailyData);
            }
            viewModel.acquiredData += currentBasicDailyData.ToString() + "\r\n";
        }

        var fileStatus = new FileStatus();
        if (analyzedDataList.Count == 0)
        {
            fileStatus = FileStatus.Null;
            if (nullcount++ > 2)
            {
                nullcount = 0;
                missionList.Remove(currentMission);
            }
        }
        else if (DateTime.Now.Year == currentMission.year && DateTime.Now.Month == currentMission.month)
        {
            fileStatus = FileStatus.Temp;
            missionList.Remove(currentMission);
        }
        else
        {
            fileStatus = FileStatus.Valid;
            missionList.Remove(currentMission);
        }

        var currentFileStatus = new BasicDailyDataFileStatusInformation()
        {
            month = currentMission.month.ToString(),
            year = currentMission.year.ToString(),
            dataCount = analyzedDataList.Count(),
            modifiedTime = DateTime.Now.ToString(),
            fileStatus = fileStatus
        };
        var indexFileStatus = fileStatusList.BinarySearch(currentFileStatus);
        if (indexFileStatus >= 0)
        {
            fileStatusList[indexFileStatus] = currentFileStatus;
        }
        else
        {
            fileStatusList.Insert(~indexFileStatus, currentFileStatus);
        }

        Core.basicDailyDataManager.saveBasicDailyData(currentMission.ID, basicDailyDataList);
        Core.basicDailyDataManager.saveFileStatus(currentMission.ID, fileStatusList);
    }

    private HtmlElement getResultTable(HtmlDocument doc)
    {
        if (currentWebSiteStockType == "A")
        {
            HtmlElementCollection tables = doc.GetElementsByTagName("table");
            foreach (HtmlElement table in tables)
            {
                if (table.InnerHtml.Contains("各日成交資訊"))
                {
                    return table;
                }
            }
        }
        else if (currentWebSiteStockType == "B")
        {
            return doc.GetElementById("st43_result");
        }
        throw new InvalidOperationException();
    }

    List<BasicDailyDataInformation> analysisDataTable(string tableInnerHTML)
    {
        string s = getContentFromtbody(tableInnerHTML);
        if (currentWebSiteStockType == "A")
        {
            List<string> dataByRows = getListByTableRow(s);
            List<string[]> dataByRowsAndCols = splitRowContentIntoCols(dataByRows);
            return fillInBasicDailyDataInformation(dataByRowsAndCols);
        }
        else if (currentWebSiteStockType == "B")
        {
            List<string> dataByRows = getListByTableRow(s);
            List<string[]> dataByRowsAndCols = splitRowContentIntoCols(dataByRows);
            return fillInBasicDailyDataInformation(dataByRowsAndCols);
        }
        throw new InvalidOperationException();
    }
    public string getContentFromtbody(string input)
    {
        var m = Regex.Match(input, @"<tbody[^>]*>.*<\/tbody>");
        if (m.Success)
        {
            return Regex.Replace(m.Value, @"<tbody[^>]*>|<\/tbody>", "");
        }
        else
        {
            return "";
        }
    }
    List<string> getListByTableRow(string input)
    {
        var m = Regex.Match(input, @"<tr[^>]*>.*?<\/tr>");
        var resultList = new List<string>();
        while (m.Success)
        {
            resultList.Add(Regex.Replace(m.Value, @"<tr[^>]*>|<\/tr>", ""));
            m = m.NextMatch();
        }
        return resultList;
    }
    private List<BasicDailyDataInformation> fillInBasicDailyDataInformation(List<string[]> dataByRowsAndCols)
    {
        List<BasicDailyDataInformation> result = new List<BasicDailyDataInformation>();
        foreach (var row in dataByRowsAndCols)
        {
            if (row.Length != 9)
            {
                continue;
            }
            var toAdd = new BasicDailyDataInformation()
            {
                date = row[0].getDateTimeFromStringMK(),
                dealedStock = row[1].getDecimalFromString(),
                volume = row[2].getDecimalFromString(),
                open = row[3].getDecimalFromString(),
                high = row[4].getDecimalFromString(),
                low = row[5].getDecimalFromString(),
                close = row[6].getDecimalFromString(),
                change = row[7].getDecimalFromString(),
                dealedOrder = row[8].getDecimalFromString()
            };
            result.Add(toAdd);
        }
        return result;
    }
    List<string[]> splitRowContentIntoCols(List<string> input)
    {
        List<string[]> result = new List<string[]>();

        foreach (var s in input)
        {
            var m = Regex.Match(s, @"<td[^>]*>.*?<\/td>");
            var thisRow = new List<string>();
            while (m.Success)
            {
                thisRow.Add(Regex.Replace(m.Value, @"<td[^>]*>|<\/td>", ""));
                m = m.NextMatch();
            }
            result.Add(thisRow.ToArray());
        }
        return result;
    }

    private void changeWebSite(WebBrowser webBrowser, string type)
    {
        if (type == "A")
        {
            webBrowser.Navigate(@"http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY/STOCK_DAYMAIN.php");
        }
        else if (type == "B")
        {
            webBrowser.Navigate(@"http://www.tpex.org.tw/web/stock/aftertrading/daily_trading_info/st43.php?l=zh-tw");
        }
        currentWebSiteStockType = type;
        currentMission = null;
    }

    private void selectIDandDateThenDoQuery(string currentWebSiteStockType)
    {
        if (currentWebSiteStockType == "A")
        { selectIDandDateThenDoQueryA(); }
        else
        { selectIDandDateThenDoQueryB(); }
    }
    private void selectIDandDateThenDoQueryA()
    {
        var query_year = webBrowser.Document.GetElementById("query_year");
        var query_month = webBrowser.Document.GetElementById("query_month");
        var CO_ID = webBrowser.Document.GetElementById("CO_ID");
        var query_button = webBrowser.Document.GetElementById("query-button");
        CO_ID.InnerText = currentMission.ID;
        foreach (HtmlElement opt in query_year.Children)
        {
            if (opt.GetAttribute("value") == currentMission.year.ToString())
            {
                opt.SetAttribute("selected", "selected");
                break;
            }
        }
        foreach (HtmlElement opt in query_month.Children)
        {
            if (opt.GetAttribute("value") == currentMission.month.ToString())
            {
                opt.SetAttribute("selected", "selected");
                break;
            }
        }
        Thread.Sleep(200);
        query_button.InvokeMember("click");
    }
    private void selectIDandDateThenDoQueryB()
    {
        changeEnglishTyping();
        var input_date = webBrowser.Document.GetElementById("input_date");
        var input_stock_code = webBrowser.Document.GetElementById("input_stock_code");
        input_date.Focus();
        Thread.Sleep(300);
        sendDels();
        Thread.Sleep(300);
        System.Windows.Forms.SendKeys.SendWait($"{currentMission.year - 1911}/{currentMission.month}");
        Thread.Sleep(300);
        input_stock_code.Focus();
        Thread.Sleep(300);
        sendDels();
        Thread.Sleep(300);
        System.Windows.Forms.SendKeys.SendWait($"{currentMission.ID}");
        Thread.Sleep(300);
        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        Thread.Sleep(1000);
        input_stock_code.RemoveFocus();
        //****
        loadNextMission(this, new WebBrowserDocumentCompletedEventArgs(new Uri("http://www.tpex.org.tw/web/stock/aftertrading/daily_trading_info/st43.php?l=zh-tw")));
    }

    private void changeEnglishTyping()
    {
        foreach (InputLanguage language in InputLanguage.InstalledInputLanguages)
        {
            if (language.LayoutName.Contains("US"))
            {
                InputLanguage.CurrentInputLanguage = language;
                return;
            }
        }
    }
    private static void sendDels()
    {
        int c = 0;
        while (c++ < 20)
        {
            System.Windows.Forms.SendKeys.SendWait("{END}");
            System.Windows.Forms.SendKeys.SendWait("{BACKSPACE}");
        }
    }

}
}
