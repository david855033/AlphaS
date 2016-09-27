using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.BasicDailyData;

namespace AlphaS.DataAnalyzer
{
    public class DataAnalyzer : IDataAnalyzer
    {
        private List<AnalyzedDataInformation> analyzedData;
        public void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData)
        {
            this.analyzedData = AnalyzedData.ToList();
        }
        public List<AnalyzedDataInformation> getAnalyzedData()
        {
            return analyzedData;
        }

        private List<BasicDailyDataInformation> basicDailyData;
        public void setBasicDailyData(List<BasicDailyDataInformation> BasicDailyData)
        {
            this.basicDailyData = BasicDailyData.ToList();
        }
        public List<BasicDailyDataInformation> getBasicDailyData()
        {
            return basicDailyData;
        }

        public void calculateDivideData()
        {
            DateTime lastDateInAnalyzedData = DateTime.MinValue;
            if (analyzedData.Count > 0)
            {
                lastDateInAnalyzedData = analyzedData.Last().date;
            }

            int startIndexInBasicDailyData = basicDailyData.FindIndex(x => x.date > lastDateInAnalyzedData);
            double startWeight = getStartWeight();
            for (int i = startIndexInBasicDailyData; i < basicDailyData.Count(); i++)
            {
                var newAnalyzedData = new AnalyzedDataInformation(basicDailyData[i]);
            }
        }
        private double getStartWeight()
        {
            double startWeight;
            if (analyzedData.Count == 0)
            {
                startWeight = 1;
            }
            else
            {
                startWeight = analyzedData.Last().divideWeight;
            }

            return startWeight;
        }


        private string display = "";
        public string getDisplay()
        {
            return display;
        }
    }
}
