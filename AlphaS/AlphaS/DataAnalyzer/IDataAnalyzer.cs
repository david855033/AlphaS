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
         void setID(string ID);
        void set0050BasicData(List<BasicDailyDataInformation> BasicData0050);

        void setBasicDailyData(List<BasicDailyDataInformation> BasicDailyData);
        List<BasicDailyDataInformation> getBasicDailyData();

        void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData);
        List<AnalyzedDataInformation> getAnalyzedData();

        void setFuturePriceData(List<FuturePriceDataInformation> FuturePriceData);
        List<FuturePriceDataInformation> getFuturePriceData();

        void setParameterFuturePriceTableData(List<ParameterFuturePriceTableInformation> FuturePriceData);
        List<ParameterFuturePriceTableInformation> getParameterFuturePriceTableData();
        List<ParameterFuturePriceTableInformation> getFinalParameterFuturePriceTableData();

        void resetParameterFuturePriceDictionary();
        Dictionary<string, List<ParameterFuturePriceTableInformation>> getParameterFuturePriceDictionary();
        void appendParameterFuturePriceDictionary(string parameterName, List<ParameterFuturePriceTableInformation> ParameterFuturePriceList);

        void setScoreData(List<ScoreDataInformation> scoreData);
        List<ScoreDataInformation> getScoreData();

        List<ScoreFuturePriceDataInformation> getScoreFuturePriceTable();

        string getDisplay();

        void standarizeAnalyzeData();
        void calculateParameter();
        void calculateFuturePriceData();
        void calculateParameterFuturePriceTable();
        void getParameterFuturePriceTableDataToAppend(Dictionary<string, List<ParameterFuturePriceTableInformation>> allDataToAppend);
        void calculateScoreData();
        void MakeScoreFuturePriceEvaluationTable();
    }
}
