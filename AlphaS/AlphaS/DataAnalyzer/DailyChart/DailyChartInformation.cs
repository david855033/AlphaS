using AlphaS.BasicDailyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class DailyChartInformation : IComparable
    {
        static int[] SCORE_DAY_RANGE_DEFINITION =
            ScoreDataInformation.SCORE_DAY_RANGE_DEFINITION;

        public string stockID;
        public decimal[] valueScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];
        public decimal[] rankScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];

        public decimal dealedStock;
        public decimal volume;
        public decimal open;
        public decimal high;
        public decimal low;
        public decimal close;
        public decimal change;
        public decimal dealedOrder;

        public decimal avg;
        public decimal N_avg;
        public decimal N_open;
        public decimal N_high;
        public decimal N_low;
        public decimal N_close;
        public bool recentEmpty; //120day
        public decimal recentMinVolume;
        public DateTime recentMinVolumeDate; //60day
        
        public DailyChartInformation(){}
        public DailyChartInformation(string stockID, AnalyzedDataInformation analyzedData, ScoreDataInformation scoreData)
        {
            this.stockID = stockID;
            this.dealedStock = analyzedData.dealedStock;
            this.volume = analyzedData.volume;
            this.open = analyzedData.open;
            this.high = analyzedData.high;
            this.low = analyzedData.low;
            this.close = analyzedData.close;
            this.change = analyzedData.change;
            this.dealedOrder = analyzedData.dealedOrder;

            this.avg = analyzedData.avg;
            this.N_avg = analyzedData.N_avg;
            this.N_open = analyzedData.N_open;
            this.N_high = analyzedData.N_high;
            this.N_low = analyzedData.N_low;
            this.N_close = analyzedData.N_close;
        }

        public int CompareTo(object obj)
        {
            return this.stockID.CompareTo((obj as DailyChartInformation).stockID);
        }
    }
}
