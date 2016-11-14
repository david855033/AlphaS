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
            addParameter("MA3");
            addParameter("MA5");
            addParameter("MA10");
            addParameter("MA20");
            addParameter("MA40");
            addParameter("MA60");
            addParameter("MA120");
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
            AnalyzedDataInformation.parameterIndex.Add(parameterName, AnalyzedDataInformation.parameterIndex.Count);
        }
    }

    class ChangeCalculator : BaseParameterCalculator
    {
        public ChangeCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }

        void setInitialValues()
        {
            MAIN_PARAMETER = "Change";
        }
        public override void generateParameter()
        {
            int parameterIndexOfChange = AnalyzedDataInformation.parameterIndex["Change"];
            int parameterIndexOfChangeAvg = AnalyzedDataInformation.parameterIndex["ChangeAvg"];
            addDisplay("- analyze: Change/ChangeAvg");
            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                AnalyzedData[i].parameters[parameterIndexOfChange] =
                   ((AnalyzedData[i].N_close - AnalyzedData[i - 1].N_close) /
                    AnalyzedData[i - 1].N_close * 100).round(2);

                AnalyzedData[i].parameters[parameterIndexOfChangeAvg] =
                    ((AnalyzedData[i].N_avg - AnalyzedData[i - 1].N_avg) /
                    AnalyzedData[i - 1].N_avg * 100).round(2);
            }
        }
    }
    class BiasFromMeanAverageCalculator : BaseParameterCalculator
    {
        public BiasFromMeanAverageCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }

        void setInitialValues()
        {
            MAIN_PARAMETER = "BA3";
        }
        public override void generateParameter()
        {
            int[] BAs = { 3, 5, 10, 20, 40, 60, 120 };

            string s = "- analyze: MA&BA";
            foreach (var averageDay in BAs)
            {
                int parameterIndexMA = AnalyzedDataInformation.parameterIndex[$"MA{averageDay}"];
                int parameterIndexBA = AnalyzedDataInformation.parameterIndex[$"BA{averageDay}"];
                s += averageDay + "/";
                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal divider = Convert.ToDecimal(averageDay);
                    decimal MA;
                    if (i == PRE_DATA)
                    {
                        decimal sum = 0;
                        for (int j = PRE_DATA; j > PRE_DATA - averageDay; j--)
                            sum += AnalyzedData[j].N_avg;
                        MA = sum / averageDay;
                    }
                    else {
                        decimal lastMA = AnalyzedData[i - 1].parameters[parameterIndexMA].Value;
                        decimal newN_avg = AnalyzedData[i].N_avg;
                        decimal oldN_avg = AnalyzedData[Math.Max(i - averageDay, 0)].N_avg;
                        MA = (lastMA + (newN_avg - oldN_avg) / divider);
                    }


                    AnalyzedData[i].parameters[parameterIndexMA] = MA;

                    AnalyzedData[i].parameters[parameterIndexBA] =
                        ((AnalyzedData[i].N_avg - MA) / MA * 100).round(2);
                }
            }
            addDisplay(s.TrimEnd('/'));
        }
    }

    class AverageVolumeCaculator : BaseParameterCalculator
    {
        public AverageVolumeCaculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }
        void setInitialValues()
        {
            MAIN_PARAMETER = "BA3";
        }

        public override void generateParameter()
        {
            throw new NotImplementedException();
        }
    }
}