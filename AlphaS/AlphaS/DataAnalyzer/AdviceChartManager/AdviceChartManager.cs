using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class AdviceChartManager : IAdviceChartManager
    {
        public AdviceChartManager() { }
        public AdviceChartManager(string ADVICE_CHART_FOLDER)
        {
            setBaseFolder(ADVICE_CHART_FOLDER);
        }

        private string _baseFolder;
        public void setBaseFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _baseFolder = path;
        }
        public string getBaseFolder()
        {
            return _baseFolder;
        }

        public List<DateTime> getExistedDate()
        {
            var ExistedDailyChartFile = Directory.EnumerateFiles(_baseFolder);
            var resultList = new List<DateTime>();
            foreach (var filepath in ExistedDailyChartFile)
            {
                var filename = filepath.Split('\\').Last().Split('.').First();
                resultList.Add(filename.getDateTimeFromFileName());
            }
            return resultList;
        }

        public void resetAllChart()
        {
            var ExistedDailyChartFile = Directory.EnumerateDirectories(_baseFolder);
            foreach (var filepath in ExistedDailyChartFile)
            {
                File.Delete(filepath);
            }
        }

        public List<AdviceChartInformation> getAdviceChart(DateTime date)
        {
            var result = new List<AdviceChartInformation>();
            string filepath = _baseFolder + $@"\{date.getFileNameFromDateTime()}.txt";
            if (File.Exists(_baseFolder + $@"\{date.getFileNameFromDateTime()}.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    if (!sr.EndOfStream)   //readTitle
                    {
                        sr.ReadLine();
                    }
                    while (!sr.EndOfStream)
                    {
                        result.Add(new AdviceChartInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }


        public void saveDailyChart(DateTime date, IEnumerable<AdviceChartInformation> adviceChart)
        {
            var filename = date.getFileNameFromDateTime() + ".txt";
            var filepath = _baseFolder + "\\" + filename;
            using (var sw = new StreamWriter(filepath))
            {
                var toWrite = new StringBuilder();
                toWrite.AppendLine(AdviceChartInformation.toTitle() + "\t" + "(資料日期:" + date.getFileNameFromDateTime() + ")");
                var chart = adviceChart.ToList();
                chart.Sort();
                foreach (var r in chart)
                {
                    if (r != null)
                        toWrite.AppendLine(r.ToString());
                }
                sw.Write(toWrite);
            }
        }


    }
}
