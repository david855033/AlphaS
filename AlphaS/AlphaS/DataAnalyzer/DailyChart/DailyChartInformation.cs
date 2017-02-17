using AlphaS.BasicDailyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class DailyChartInformation : IComparable
    {
        static int[] SCORE_DAY_RANGE_DEFINITION =
            ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION;

        public string stockID;

        public decimal volume;
        public decimal open;
        public decimal high;
        public decimal low;
        public decimal close;
        public decimal change;

        public decimal avg;
        public decimal divide;
        public bool recentEmpty; //120day
        public decimal recentMinVolume;
        public bool isLowVolume;

        public decimal N_avg;
        public decimal N_open;
        public decimal N_high;
        public decimal N_low;
        public decimal N_close;

        public decimal[] valueScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];
        public decimal[] rankScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];

        public decimal weightedScore;//不儲存 給trader運算用

        public DailyChartInformation() { }
        public DailyChartInformation(string stockID, AnalyzedDataInformation analyzedData, ScoreDataInformation scoreData)
        {
            this.stockID = stockID;

            this.volume = analyzedData.volume;
            this.open = analyzedData.open;
            this.high = analyzedData.high;
            this.low = analyzedData.low;
            this.close = analyzedData.close;
            this.change = analyzedData.change;

            this.avg = analyzedData.avg;
            this.divide = analyzedData.divide;
            this.recentEmpty = analyzedData.recentEmpty;
            this.recentMinVolume = analyzedData.recentMinVolume;
            this.isLowVolume = recentMinVolume < DataAnalyzer.MIN_VOLUME_THRESHOLD;

            this.N_avg = analyzedData.N_avg;
            this.N_open = analyzedData.N_open;
            this.N_high = analyzedData.N_high;
            this.N_low = analyzedData.N_low;
            this.N_close = analyzedData.N_close;

            for (int i = 0; i < valueScore.Length; i++)
            {
                this.valueScore[i] = scoreData.valueScore[i];
            }
            for (int i = 0; i < rankScore.Length; i++)
            {
                this.rankScore[i] = scoreData.rankScore[i];
            }
        }
        public DailyChartInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;

            this.stockID = splitline[i++];

            this.volume = splitline[i++].getDecimalFromString();
            this.open = splitline[i++].getDecimalFromString();
            this.high = splitline[i++].getDecimalFromString();
            this.low = splitline[i++].getDecimalFromString();
            this.close = splitline[i++].getDecimalFromString();
            this.change = splitline[i++].getDecimalFromString();

            this.avg = splitline[i++].getDecimalFromString();
            this.divide = splitline[i++].getDecimalFromString();
            this.recentEmpty = splitline[i++].getBoolFromString();
            this.recentMinVolume = splitline[i++].getDecimalFromString();
            this.isLowVolume = splitline[i++].getBoolFromString();

            this.N_avg = splitline[i++].getDecimalFromString();
            this.N_open = splitline[i++].getDecimalFromString();
            this.N_high = splitline[i++].getDecimalFromString();
            this.N_low = splitline[i++].getDecimalFromString();
            this.N_close = splitline[i++].getDecimalFromString();

            for (int x = 0; x < valueScore.Length; x++)
            {
                this.valueScore[x] = splitline[i++].getDecimalFromString();
            }
            for (int x = 0; x < rankScore.Length; x++)
            {
                this.rankScore[x] = splitline[i++].getDecimalFromString();
            }
        }

        static public string toTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("stockID");
            sb.Append("\t" + "volume");
            sb.Append("\t" + "open");
            sb.Append("\t" + "high");
            sb.Append("\t" + "low");
            sb.Append("\t" + "close");
            sb.Append("\t" + "change");

            sb.Append("\t" + "avg");
            sb.Append("\t" + "divide");
            sb.Append("\t" + "recentEmpty");
            sb.Append("\t" + "recentMinVolume");
            sb.Append("\t" + "isLowVolume");

            sb.Append("\t" + "N_open");
            sb.Append("\t" + "N_high");
            sb.Append("\t" + "N_low");
            sb.Append("\t" + "N_close");
            sb.Append("\t" + "N_avg");
            for (int i = 0; i < SCORE_DAY_RANGE_DEFINITION.Length; i++)
            {
                sb.Append("\t" + "valueScore" + i);
            }
            for (int i = 0; i < SCORE_DAY_RANGE_DEFINITION.Length; i++)
            {
                sb.Append("\t" + "rankScore" + i);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(stockID);
            sb.Append("\t" + volume.round(2).ToString());
            sb.Append("\t" + open.round(2).ToString());
            sb.Append("\t" + high.round(2).ToString());
            sb.Append("\t" + low.round(2).ToString());
            sb.Append("\t" + close.round(2).ToString());
            sb.Append("\t" + change.round(2).ToString());

            sb.Append("\t" + avg.round(2).ToString());
            sb.Append("\t" + divide.round(2).ToString());
            sb.Append("\t" + (recentEmpty ? "1" : "0"));
            sb.Append("\t" + recentMinVolume.round(2).ToString());
            sb.Append("\t" + (isLowVolume ? "1" : "0"));

            sb.Append("\t" + N_open.round(2).ToString());
            sb.Append("\t" + N_high.round(2).ToString());
            sb.Append("\t" + N_low.round(2).ToString());
            sb.Append("\t" + N_close.round(2).ToString());
            sb.Append("\t" + N_avg.round(2).ToString());
            for (int i = 0; i < valueScore.Length; i++)
            {
                sb.Append("\t" + valueScore[i].round(2));
            }
            for (int i = 0; i < rankScore.Length; i++)
            {
                sb.Append("\t" + rankScore[i].round(2));
            }
            return sb.ToString();
        }


        public int CompareTo(object obj)
        {
            return this.stockID.CompareTo((obj as DailyChartInformation).stockID);
        }

    }
}
