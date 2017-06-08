using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System;
using AlphaS.CoreNS;
using System.Net;
using System.IO;
using System.Threading.Tasks;

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

        List<BasicDailyDataMission> missionList = new List<BasicDailyDataMission>();
        public void setMission(List<BasicDailyDataMission> mission)
        {
            this.missionList = mission.ToList();
        }
        public int getMissionCount()
        {
            return missionList != null ? missionList.Count() : 0;
        }

        List<BasicDailyDataInformation> BasicDailyDatas = new List<BasicDailyDataInformation>();
        public List<BasicDailyDataInformation> getResult()
        {
            return BasicDailyDatas;
        }

        public bool webBrowserWorking = false;


        public DateTime missionStartTime;

        BasicDailyDataMission currentMission;

        public void startMainMission()
        {
            missionStartTime = DateTime.Now;
            viewModel.acquiredData = BasicDailyDataInformation.ToTitle();

            for (int i = 0; i < missionList.Count(); i++)
            {
                printMissionList(i);
                Application.DoEvents();
                currentMission = missionList[i];
                var basicDailyDataList = Core.basicDailyDataManager.getBasicDailyData(currentMission.ID);
                var fileStatusList = Core.basicDailyDataManager.getFileStatus(currentMission.ID);
                var analyzedDataList = new List<BasicDailyDataInformation>();
                if (currentMission.type == "A")
                {
                    var s = GetCSV($"http://www.tse.com.tw/exchangeReport/STOCK_DAY?response=csv&date={currentMission.year}{currentMission.month.ToString("D2")}01&stockNo={currentMission.ID}");
                    analyzedDataList = analysisDataCSV(s);
                }
                else if (currentMission.type == "B")
                {

                    var s = GetJSON(
                     $"http://www.tpex.org.tw/web/stock/aftertrading/daily_trading_info/st43_result.php?l=zh-tw&d={currentMission.year - 1911}/{currentMission.month.ToString("D2")}&stkno={currentMission.ID}"
                     );
                    var jsonResult = s.ParseJSON();
                    analyzedDataList = analysisDataJSON(jsonResult["aaData"].ParseTable());
                }
                FileStatus currentFileStatus = getCurrentFileStatus(analyzedDataList);
                renewFileStatus(fileStatusList, currentFileStatus, analyzedDataList.Count);
                renewBasicDailyDataList(basicDailyDataList, analyzedDataList);
                Core.basicDailyDataManager.saveBasicDailyData(currentMission.ID, basicDailyDataList);
                Core.basicDailyDataManager.saveFileStatus(currentMission.ID, fileStatusList);
            }
            printMissionList(missionList.Count);
        }

        public string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.Default);
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
        }

        public string GetJSON(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.Default);
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
        }

        private HtmlElement getResultTable(HtmlDocument doc)
        {
            return doc.GetElementById("st43_result"); ;
        }

        private FileStatus getCurrentFileStatus(List<BasicDailyDataInformation> analyzedDataList)
        {
            FileStatus filestatus;
            if (analyzedDataList.Count == 0)
            {
                filestatus = FileStatus.Null;
            }
            else if (DateTime.Now.Year == currentMission.year && DateTime.Now.Month == currentMission.month)
            {
                filestatus = FileStatus.Temp;
            }
            else
            {
                filestatus = FileStatus.Valid;
            }

            return filestatus;
        }
        private void renewFileStatus(List<BasicDailyDataFileStatusInformation> fileStatusList, FileStatus fileStatus, int dataCount)
        {
            var currentFileStatus = new BasicDailyDataFileStatusInformation()
            {
                month = currentMission.month.ToString(),
                year = currentMission.year.ToString(),
                dataCount = dataCount,
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
        }
        private void renewBasicDailyDataList(List<BasicDailyDataInformation> originalList, List<BasicDailyDataInformation> grabbedList)
        {
            viewModel.acquiredData = BasicDailyDataInformation.ToTitle() + "\r\n";
            foreach (var currentBasicDailyData in grabbedList)
            {
                int index = originalList.BinarySearch(currentBasicDailyData);
                if (index >= 0)
                {
                    originalList[index] = currentBasicDailyData;
                }
                else
                {
                    originalList.Insert(~index, currentBasicDailyData);
                }
                viewModel.acquiredData += currentBasicDailyData.ToString() + "\r\n";
            }
        }

        private List<BasicDailyDataInformation> analysisDataCSV(string s)
        {
            var lines = s.Split('\n');
            List<BasicDailyDataInformation> result = new List<BasicDailyDataInformation>();
            foreach (var l in lines)
            {
                Regex regex = new Regex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1");
                var matches = regex.Matches(l);
                string[] row = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    row[i] = matches[i].Groups[0].Value.Trim('"');
                }
                if (row.Length != 9 || row[0] == "日期")
                {
                    continue;
                }
                var toAdd = new BasicDailyDataInformation()
                {
                    date = row[0].getRidOfPostStar().getDateTimeFromStringMK(),
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

        private List<BasicDailyDataInformation> analysisDataJSON(List<List<string>> table)
        {
            List<BasicDailyDataInformation> result = new List<BasicDailyDataInformation>();
            foreach (var row in table)
            {
                var toAdd = new BasicDailyDataInformation()
                {
                    date = row[0].getRidOfPostStar().getDateTimeFromStringMK(),
                    dealedStock = row[1].getDecimalFromString(),
                    volume = row[2].getDecimalFromString() ,
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
        List<BasicDailyDataInformation> analysisDataTable(string tableInnerHTML)
        {
            string s = getContentFromtbody(tableInnerHTML);
            List<string> dataByRows = getListByTableRow(s);
            List<string[]> dataByRowsAndCols = splitRowContentIntoCols(dataByRows);
            return fillInBasicDailyDataInformation(dataByRowsAndCols);
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
                    date = row[0].getRidOfPostStar().getDateTimeFromStringMK(),
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

        private void printMissionList(int i)
        {
            viewModel.missionList = "currently ";
            if (currentMission != null)
            {
                viewModel.missionList += currentMission.ToString() + "\r\n";
            }
            else
            {
                viewModel.missionList += "null\r\n";
            }
            viewModel.missionList += "--------------------------\r\n";
            double percentage = missionList.Count != 0 ?
                (100 - ((missionList.Count - i - 1) * 1.0 / missionList.Count * 100)).round(4) : 0;

            viewModel.missionList += $"queue: {missionList.Count - i - 1} / {missionList.Count} ({percentage}%)\r\n";

            double elapsedTime = DateTime.Now.Subtract(missionStartTime).TotalSeconds.round(1);
            double estimatedTime = (elapsedTime * (100 - percentage) / percentage).round(1);
            viewModel.missionList += $"elapsed time: {elapsedTime.getTimeString()}, estimate time left: {estimatedTime.getTimeString()}\r\n";

            viewModel.missionList += String.Join("\r\n", missionList.GetRange(i, Math.Min(20, missionList.Count - i)).Select(x => x.ToString()));
        }

    }
}
