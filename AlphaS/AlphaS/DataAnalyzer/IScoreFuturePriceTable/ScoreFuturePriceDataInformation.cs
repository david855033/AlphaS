using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class ScoreFuturePriceDataInformation : IComparable
    {
        public static int[] SCORE_DAY_RANGE_DEFINITION = ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION;
        public static int[] FUTURE_PRICE_DAYS = FuturePriceDataInformation.FUTURE_PRICE_DAYS;

        public decimal[] rankScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];
        public decimal[] valueScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];
        public decimal?[] futurePrices = new decimal?[FUTURE_PRICE_DAYS.Length];
        public decimal?[] futurePriceRank = new decimal?[FUTURE_PRICE_DAYS.Length];

        public int CompareTo(object obj)
        {
            var that = (ScoreFuturePriceDataInformation)obj;
            return this.valueScore[0].CompareTo(that.valueScore[0]);
        }

        public ScoreFuturePriceDataInformation()
        {
        }

        public ScoreFuturePriceDataInformation(decimal score, decimal?[] futurePrices, decimal?[] futurePriceRank)
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < valueScore.Length; i++)
            {
                if (i != 0) sb.Append("\t");
                sb.Append(valueScore[i].round(2).ToString());
            }

            for (int i = 0; i < rankScore.Length; i++)
            {
                sb.Append("\t" + rankScore[i].round(2).ToString());
            }

            for (int i = 0; i < futurePrices.Length; i++)
            {
                sb.Append("\t" + Math.Log(Convert.ToDouble((futurePrices[i].Value+100) / 100)).round(4).ToString());
            }

            for (int i = 0; i < futurePriceRank.Length; i++)
            {
                sb.Append("\t" + futurePriceRank[i].round(2).ToString());
            }
            return sb.ToString();
        }
    }
}
