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
            throw new NotImplementedException();
        }

        private string display = "";
        public string getDisplay()
        {
            return display;
        }
    }
}
