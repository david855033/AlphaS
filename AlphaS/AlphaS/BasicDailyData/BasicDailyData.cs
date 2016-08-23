using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataInformation
    {
        public DateTime date;
        public decimal dealedStock;
        public decimal volume;
        public decimal open;
        public decimal high;
        public decimal low;
        public decimal close;
        public decimal change;
        public decimal dealedOrder;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.ToString("yyyy-MM-dd") + "\t");
            sb.Append(dealedStock.ToString() +"\t");
            sb.Append(volume.ToString() + "\t");
            sb.Append(open.ToString() + "\t");
            sb.Append(high.ToString() + "\t");
            sb.Append(low.ToString() + "\t");
            sb.Append(close.ToString() + "\t");
            sb.Append(change.ToString() + "\t");
            sb.Append(dealedOrder.ToString()+"\r\n");
            return sb.ToString();
        }

        public static string ToTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("日期\t");
            sb.Append("成交股數\t");
            sb.Append("成交金額\t");
            sb.Append("開盤價\t");
            sb.Append("最高價\t");
            sb.Append("最低價\t");
            sb.Append("收盤價\t");
            sb.Append("漲跌價差\t");
            sb.Append("成交筆數+\r\n");
            return sb.ToString();           
        }
    }
}
