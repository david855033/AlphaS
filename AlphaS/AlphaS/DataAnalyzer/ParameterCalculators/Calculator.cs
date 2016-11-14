using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer.ParameterCalculators
{
    static class paramterIndexInitializer
    {
        public static void initialize()
        {
            addParameter("Change");
            addParameter("ChangeAvg");
            addParameter("BA3");
            addParameter("BA5");
            addParameter("BA10");
            addParameter("BA20");
            addParameter("BA40");
            addParameter("BA60");
            addParameter("BA120");
        }
        static void addParameter(string parameterName)
        {
            AnalyzedDataInformation.parameterIndex.Add(parameterName ,AnalyzedDataInformation.parameterIndex.Count);
        }
    }

    class ChangeCalculator : BaseParameterCalculator
    {
        public ChangeCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }

        void setInitialValues()
        {
            PRE_DATA = 1;
            MAIN_PARAMETER = "Change";
        }
        public override void generateParameter()
        {
            int parameterIndexOfChange = AnalyzedDataInformation.parameterIndex["Change"];
            int parameterIndexOfChangeAvg = AnalyzedDataInformation.parameterIndex["ChangeAvg"];
            addDisplay("- analyze: Change");
            addDisplay("- analyze: ChangeAvg");
            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                AnalyzedData[i].parameters[parameterIndexOfChange] =
                    Math.Round((AnalyzedData[i].N_close - AnalyzedData[i - 1].N_close) /
                    AnalyzedData[i - 1].N_close * 100, 2);

                AnalyzedData[i].parameters[parameterIndexOfChangeAvg] =
                    Math.Round((AnalyzedData[i].N_avg - AnalyzedData[i - 1].N_avg) /
                    AnalyzedData[i - 1].N_avg * 100, 2);
            }
        }
    }
    class BiasFromMeanAverageCalculator : BaseParameterCalculator
    {
        public BiasFromMeanAverageCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }

        void setInitialValues()
        {
            PRE_DATA = 1;
            MAIN_PARAMETER = "BiasFromMeanAverageCalculator";
        }
        public override void generateParameter()
        {
            int parameterIndexOfChange = AnalyzedDataInformation.parameterIndex["Change"];
            int parameterIndexOfChangeAvg = AnalyzedDataInformation.parameterIndex["ChangeAvg"];
            addDisplay("- analyze: Change");
            addDisplay("- analyze: ChangeAvg");
            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                AnalyzedData[i].parameters[parameterIndexOfChange] =
                    Math.Round((AnalyzedData[i].N_close - AnalyzedData[i - 1].N_close) /
                    AnalyzedData[i - 1].N_close * 100, 2);

                AnalyzedData[i].parameters[parameterIndexOfChangeAvg] =
                    Math.Round((AnalyzedData[i].N_avg - AnalyzedData[i - 1].N_avg) /
                    AnalyzedData[i - 1].N_avg * 100, 2);
            }
        }
    }
}
