using AlphaS.BasicDailyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class AdviceChartInformation : IComparable
    {
        public static readonly decimal BUY_PRICE_FROM_CLOSE = 1.01m;
        public static readonly decimal[] VALUE_SCORE_WEIGHT = { 1m, 2m };
        public static readonly decimal[] RANK_SCORE_WEIGHT = { 0.5m, 0.5m };
        public static readonly decimal BUY_SCORE = 0.3m;
        public static readonly decimal SELL_SCORE = -0.08m;
        public static readonly int SELL_SCORE_DAY = 3;

        public string stockID;
        public string stockName;

        public string suggestAction = "";
        public decimal suggestBuyPrice
        {
            get
            {
                decimal suggestBuy = close * BUY_PRICE_FROM_CLOSE;
                if (suggestBuy < 10)
                {
                    return suggestBuy.round(2);
                }
                else if (suggestBuy < 50)
                {
                    return (suggestBuy * 20).round(0) / 20;
                }
                else if (suggestBuy < 100)
                {
                    return (suggestBuy * 10).round(0) / 10;
                }
                else
                {
                    return (suggestBuy * 2).round(0) / 2;
                }
            }
        }

        public decimal weightedScore;
        public decimal volume;
        public decimal open;
        public decimal high;
        public decimal low;
        public decimal close;
        public decimal change;

        public decimal avg;

        public decimal divide;
        public bool recentEmpty; //120day
        public bool isLowVolume;


        public AdviceChartInformation() { }
        public AdviceChartInformation(DailyChartInformation dailyChart)
        {
            this.stockID = dailyChart.stockID;

            this.weightedScore = getWeightedScore(dailyChart);

            this.volume = dailyChart.volume;
            this.open = dailyChart.open;
            this.high = dailyChart.high;
            this.low = dailyChart.low;
            this.close = dailyChart.close;
            this.change = dailyChart.change;

            this.avg = dailyChart.avg;
            this.divide = dailyChart.divide;
            this.isLowVolume = dailyChart.isLowVolume;
            this.recentEmpty = dailyChart.recentEmpty;

        }
        private decimal getWeightedScore(DailyChartInformation StockDaily)
        {
            decimal score;
            decimal sumWeight = 0, sum = 0;
            for (int i = 0; i < VALUE_SCORE_WEIGHT.Length; i++)
            {
                sum += StockDaily.valueScore[i] * VALUE_SCORE_WEIGHT[i];
                sum += StockDaily.rankScore[i] * RANK_SCORE_WEIGHT[i] / 100 - 0.25m;
                sumWeight += VALUE_SCORE_WEIGHT[i];
                sumWeight += RANK_SCORE_WEIGHT[i];
            }
            score = (sum / sumWeight).round(2);
            return score;
        }

        public AdviceChartInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;

            this.stockID = splitline[i++];
            this.stockName = splitline[i++];

            this.weightedScore = splitline[i++].getDecimalFromString();
            this.suggestAction = splitline[i++];
            i++; //suggestBuyPrice

            this.volume = splitline[i++].getDecimalFromString();
            this.open = splitline[i++].getDecimalFromString();
            this.high = splitline[i++].getDecimalFromString();
            this.low = splitline[i++].getDecimalFromString();
            this.close = splitline[i++].getDecimalFromString();
            this.change = splitline[i++].getDecimalFromString();

            this.avg = splitline[i++].getDecimalFromString();
            this.divide = splitline[i++].getDecimalFromString();
            this.isLowVolume = splitline[i++].getBoolFromString();
            this.recentEmpty = splitline[i++].getBoolFromString();
        }

        static public string toTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("stockID");
            sb.Append("\t" + "stockName");
            sb.Append("\t" + "weightedScore");
            sb.Append("\t" + "suggestAction");
            sb.Append("\t" + "suggestBuyPrice");

            sb.Append("\t" + "volume(k)");
            sb.Append("\t" + "open");
            sb.Append("\t" + "high");
            sb.Append("\t" + "low");
            sb.Append("\t" + "close");
            sb.Append("\t" + "change");

            sb.Append("\t" + "avg");

            sb.Append("\t" + "divide");
            sb.Append("\t" + "recentEmpty");
            sb.Append("\t" + "isLowVolume");

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(stockID);
            sb.Append("\t" + stockName);
            sb.Append("\t" + weightedScore);
            sb.Append("\t" + suggestAction);
            sb.Append("\t" + suggestBuyPrice.round(2).ToString());

            sb.Append("\t" + volume.round(0).ToString());
            sb.Append("\t" + open.round(2).ToString());
            sb.Append("\t" + high.round(2).ToString());
            sb.Append("\t" + low.round(2).ToString());
            sb.Append("\t" + close.round(2).ToString());
            sb.Append("\t" + change.round(2).ToString());

            sb.Append("\t" + avg.round(2).ToString());
            sb.Append("\t" + divide.round(2).ToString());
            sb.Append("\t" + (recentEmpty ? "1" : "0"));
            sb.Append("\t" + (isLowVolume ? "1" : "0"));

            return sb.ToString();
        }


        public int CompareTo(object obj)
        {
            var that = (obj as AdviceChartInformation);
            if(this.isLowVolume != that.isLowVolume)
                return this.isLowVolume.CompareTo(that.isLowVolume);
            if (this.recentEmpty != that.recentEmpty)
                return this.recentEmpty.CompareTo(that.recentEmpty);
            return this.weightedScore.CompareTo(that.weightedScore) * -1;
        }

    }
}
