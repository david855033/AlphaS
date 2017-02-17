using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class TradeRecord
    {
        public string ID;
        public DateTime buyDate;
        public DateTime sellDate;
        public decimal buyPrice;
        public decimal sellPrice;
        public DateTime lastAvailableDate;
        public decimal lastAvailablePrice;
        public int belowThresholdCount;
        public int totalDays;
        public decimal profitPercentage;
        public TradeRecord(string ID, DateTime buyDate, decimal buyPrice)
        {
            this.ID = ID;
            this.buyDate = buyDate;
            this.buyPrice = buyPrice;
        }

        public  void sell(DateTime sellDate, decimal sellPrice)
        {
            this.sellDate = sellDate;
            this.sellPrice = sellPrice;
            //todo計算
        }
    }
}
