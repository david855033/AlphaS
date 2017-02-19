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
        public decimal buyShare;
        public DateTime sellDate;
        public decimal buyPrice;
        public decimal sellPrice;
        public DateTime lastAvailableDate;
        public decimal lastAvailablePrice;
        public int belowThresholdCount;
        public int totalTradeDays;
        public decimal inMoney;
        public decimal currentValue {
            get { return (inMoney * profitRatio).round(4); }
        }

        public int totalDays
        {
            get
            {
                if (sellDate != default(DateTime))
                    return sellDate.Subtract(buyDate).Days;
                if (lastAvailableDate != default(DateTime))
                    return lastAvailableDate.Subtract(buyDate).Days;
                return 0;
            }
        }
        public decimal profitRatio
        {
            get
            {
                if (sellPrice != default(decimal))
                    return (sellPrice * (1 - 0.003m - 0.001425m) / (buyPrice * (1.003m))).round(4);
                if (lastAvailablePrice != default(decimal))
                    return (lastAvailablePrice * (1 - 0.003m - 0.001425m) / (buyPrice * (1.003m))).round(4);
                return 1;
            }
        }
        private TradeRecord() { }
        public TradeRecord(string ID, DateTime buyDate, decimal buyPrice)
        {
            this.ID = ID;
            this.buyDate = buyDate;
            this.buyPrice = buyPrice;
            totalTradeDays = 0;
        }

        public void sell(DateTime sellDate, decimal sellPrice)
        {
            this.sellDate = sellDate;
            this.sellPrice = sellPrice;
        }

        public TradeRecord getMinor()
        {
            return new TradeRecord()
            {
                ID = ID,
                buyDate = buyDate,
                buyPrice = buyPrice,
                buyShare = buyShare,
                sellDate = sellDate,
                sellPrice = sellPrice,
                belowThresholdCount = belowThresholdCount,
                totalTradeDays = totalTradeDays,
                lastAvailableDate = lastAvailableDate,
                lastAvailablePrice = lastAvailablePrice,
                inMoney = inMoney
            };
        }
    }
}
