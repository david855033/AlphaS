using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class TradingProtocal
    {
        public decimal[] valueScoreWeight = new decimal[ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length];
        public decimal[] rankScoreWeight = new decimal[ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length];
        public int divideParts;

        public decimal buyScoreThreshold;

        public decimal sellScoreThreshold;
        public decimal sellScoreThresholdDay;
        public decimal sellRankThreshold;

        public decimal buyPriceFromClose;
        public decimal sellPriceFromClose;

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < valueScoreWeight.Length; i++)
            {
                result += valueScoreWeight[i] + "\t";
            }
            for (int i = 0; i < rankScoreWeight.Length; i++)
            {
                result += rankScoreWeight[i] + "\t";
            }
            result += divideParts + "\t";

            result += buyScoreThreshold + "\t";

            result += sellScoreThreshold + "\t";
            result += sellScoreThreshold + "\t";
            result += sellRankThreshold + "\t";

            result += buyPriceFromClose + "\t";
            result += sellPriceFromClose + "\t";

            result.TrimEnd('\t');
            return result;
        }

        static public string toTitle()
        {
            string result = "";
            for (int i = 0; i < ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length; i++)
            {
                result += "valueScoreWeight" + i + "\t";
            }
            for (int i = 0; i < ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION.Length; i++)
            {
                result += "rankScoreWeight" + i + "\t";
            }
            result += "divideParts" + "\t";

            result += "scoreThreshold" + "\t";

            result += "sellScoreThreshold" + "\t";
            result += "sellScoreThreshold" + "\t";
            result += "sellRankThreshold" + "\t";

            result += "buyPriceFromClose" + "\t";
            result += "sellPriceFromClose" + "\t";

            result.TrimEnd('\t');
            return result;
        }
    }
}
