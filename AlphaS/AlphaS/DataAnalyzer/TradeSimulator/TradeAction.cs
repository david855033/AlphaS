using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class TradeAction
    {
        public TradeAction(DateTime date, string action, string ID, decimal bidPrice)
        {
            this.date = date;
            this.action = action;
            this.ID = ID;
            this.bidPrice = bidPrice;
        }
        public DateTime date;
        public string action;
        public string ID;
        public decimal buyAmount;
        public decimal bidPrice;
        public string orderMessage;
        public string excuteMessage;
    }

}
