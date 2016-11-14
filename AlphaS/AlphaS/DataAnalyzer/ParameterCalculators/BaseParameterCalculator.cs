using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer.ParameterCalculators
{
    abstract class BaseParameterCalculator
    {
        public BaseParameterCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay)
        {
            setAnalyzedData(AnalyzedData);
            setAddDisplay(addDisplay);
        }
        protected List<AnalyzedDataInformation> AnalyzedData;
        void setAnalyzedData(List<AnalyzedDataInformation> AnalyzedData)
        {
            this.AnalyzedData = AnalyzedData;
        }

        public delegate void addDisplayDel(string toShow);
        protected addDisplayDel addDisplay;
        void setAddDisplay(addDisplayDel theDelegate)
        {
            addDisplay = theDelegate;
        }

        protected int parameterIndex, existAnalyzeDataCount, lastParameterDataIndex, startCalculationIndex;
        protected int PRE_DATA;
        protected string MAIN_PARAMETER;

        public void calculate()
        {
            parameterIndex = AnalyzedDataInformation.parameterIndex[MAIN_PARAMETER];
            addDisplay($"Main Parameter: {MAIN_PARAMETER}, index = {parameterIndex}, Predata = {PRE_DATA}");

            existAnalyzeDataCount = AnalyzedData.Count;
            addDisplay($"- exist Analyze Data Count = {existAnalyzeDataCount}");

            lastParameterDataIndex = AnalyzedData.FindLastIndex(x => x.parameters[parameterIndex].HasValue);
            addDisplay($"- exist Parameter Data Count = {lastParameterDataIndex + 1}");

            if (lastParameterDataIndex >= existAnalyzeDataCount - 1)
            {
                addDisplay("- no new data to calculate");
                return;
            }
            else
            {
                startCalculationIndex = Math.Max(PRE_DATA, lastParameterDataIndex + 1);
                addDisplay($"- Calculate from index={startCalculationIndex}");
                generateParameter();
            }
        }
        abstract public void generateParameter();
    }

}

