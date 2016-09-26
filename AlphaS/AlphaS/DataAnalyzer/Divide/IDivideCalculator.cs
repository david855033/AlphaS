using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.BasicDailyData;
namespace AlphaS.DataAnalyzer.Divide
{
    public interface IDivideCalculator
    {
        void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData);
        List<AnalyzedDataInformation>  getAnalyzedData();

        void setBasicDailyData(List<BasicDailyDataInformation> setBasicDailyData);
        List<BasicDailyDataInformation> getBasicDailyData();

        void setViewModel(DataAnalyzerViewModel viewModel);

        void calculateDivideData();
    }
}
