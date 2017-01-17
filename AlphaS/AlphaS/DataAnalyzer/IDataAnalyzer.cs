using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.BasicDailyData;
namespace AlphaS.DataAnalyzer
{
    public interface IDataAnalyzer
    {
        void setStockType(string type);

        void set0050BasicData(List<BasicDailyDataInformation> BasicData0050);

        void setBasicDailyData(List<BasicDailyDataInformation> BasicDailyData);
        List<BasicDailyDataInformation> getBasicDailyData();

        void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData);
        List<AnalyzedDataInformation>  getAnalyzedData();

        void setFuturePriceData(List<FuturePriceDataInformation> FuturePriceData);
        List<FuturePriceDataInformation> getFuturePriceData();

        string getDisplay();
       
        void standarizeAnalyzeData();
        void calculateParameter();
        void calculateFuturePriceData();
    }
}
