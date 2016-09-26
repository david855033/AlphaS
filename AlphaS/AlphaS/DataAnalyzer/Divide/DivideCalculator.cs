using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.BasicDailyData;

namespace AlphaS.DataAnalyzer.Divide
{
    public class DivideCalculator : IDivideCalculator
    {

        public void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData)
        {
            throw new NotImplementedException();
        }
        public List<AnalyzedDataInformation> getAnalyzedData()
        {
            throw new NotImplementedException();
        }

        public void setBasicDailyData(List<BasicDailyDataInformation> setBasicDailyData)
        {
            throw new NotImplementedException();
        }
        public List<BasicDailyDataInformation> getBasicDailyData()
        {
            throw new NotImplementedException();
        }

        private DataAnalyzerViewModel viewModel;
        public void setViewModel(DataAnalyzerViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public void calculateDivideData()
        {
            throw new NotImplementedException();
        }
    }
}
