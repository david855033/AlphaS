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
        public decimal buyThreshold;

        public decimal buyPriceFromClose;
        public decimal sellPriceFromClose;

        public decimal sellThreshold;
        public decimal sellThresholdDay;
        public decimal sellRankThreshold;
        
        public int divideParts;
        
    }
}
