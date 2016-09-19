﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;
using System.Reflection;
using mshtml;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System;

namespace AlphaS.BasicDailyData
{
    abstract public class BasicDailyDataDownloaderProtoType
    {
        abstract public void setMission(List<BasicDailyDataMission> mission);
        abstract public void startMission();
        abstract public List<BasicDailyDataInformation> getResult();
    }

    public class BasicDailyDataDownloader : BasicDailyDataDownloaderProtoType
    {
        private WebBrowser webBrowser;
        public BasicDailyDataDownloader(System.Windows.Forms.WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
        }
        private BasicDailyDataViewModel viewModel;
        public void setViewModel(BasicDailyDataViewModel viewModel) { this.viewModel = viewModel; }

        List<BasicDailyDataMission> missionList;
        override public void setMission(List<BasicDailyDataMission> mission)
        {
            this.missionList = mission.ToList();
        }

        List<BasicDailyDataInformation> BasicDailyDatas = new List<BasicDailyDataInformation>();
        public override List<BasicDailyDataInformation> getResult()
        {
            return BasicDailyDatas;
        }


        public bool webBrowserWorking = false;

        public override void startMission()
        {
            webBrowser.DocumentCompleted += loadComplete;
            viewModel.acquiredData = BasicDailyDataInformation.ToTitle();
            Thread.Sleep(300);
            webBrowser.Navigate(@"http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY/STOCK_DAYMAIN.php");

        }

        BasicDailyDataMission currentMission;
        void loadComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var doc = webBrowser.Document;
            if (currentMission != null)
            {
                var tables = doc.GetElementsByTagName("table");
                HtmlElement dataTable = tables[0];
                foreach (HtmlElement table in tables)
                {
                    if (table.InnerHtml.Contains("各日成交資訊"))
                    {
                        dataTable = table;
                        break;
                    }
                }
                foreach (var data in analysisDataTable(dataTable.InnerHtml))
                    viewModel.acquiredData += data.ToString();
            }
            if (missionList.Count > 0)
            {
                Thread.Sleep(1000);

                currentMission = missionList.First();

                var query_year = doc.GetElementById("query_year");
                var query_month = doc.GetElementById("query_month");
                var CO_ID = doc.GetElementById("CO_ID");
                var query_button = doc.GetElementById("query-button");
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
                query_button.InvokeMember("click");
                viewModel.acquiredData += currentMission.ToString() + "\r\n";
                Thread.Sleep(1000);
                missionList.Remove(currentMission);
            }
            else
            {
                webBrowser.DocumentCompleted -= loadComplete;
            }
        }

        List<BasicDailyDataInformation> analysisDataTable(string tableInnerHTML)
        {
            string s = getContentFromtbody(tableInnerHTML);
            List<string> dataByRows = getListByTableRow(s);
            List<string[]> dataByRowsAndCols = splitRowContentIntoCols(dataByRows);
            List<BasicDailyDataInformation> result = fillInBasicDailyDataInformation(dataByRowsAndCols);

            return result;
        }

        private List<BasicDailyDataInformation> fillInBasicDailyDataInformation(List<string[]> dataByRowsAndCols)
        {
            List<BasicDailyDataInformation> result = new List<BasicDailyDataInformation>();
            foreach (var row in dataByRowsAndCols)
            {
                var toAdd = new BasicDailyDataInformation()
                {
                    date = row[0].getDateTimeFromString().transferMKtoBC(),
                    dealedStock = row[1].getDecimalFromString(),
                    volume= row[2].getDecimalFromString(),
                    open = row[3].getDecimalFromString()
                };
                result.Add(toAdd);
            }
            return result;
        }

        string getContentFromtbody(string input)
        {
            var m = Regex.Match(input, @"<tbody>.*<\/tbody>");
            if (m.Success)
            {
                var result = m.Value.Replace("<tbody>", "").Replace(@"<\/tbody>", "");
                return result;
            }
            else
            { return ""; }
        }
        List<string> getListByTableRow(string input)
        {
            var m = Regex.Match(input, @"<tr>.*?<\/tr>");
            var resultList = new List<string>();
            while (m.Success)
            {
                resultList.Add(m.Value.Replace("<tr>", "").Replace(@"<\/tr>", ""));
                m = m.NextMatch();
            }
            return resultList;
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
    }
}
