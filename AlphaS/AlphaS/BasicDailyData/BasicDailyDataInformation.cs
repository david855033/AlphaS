using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataInformation : IComparable
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


        public BasicDailyDataInformation() { }
        public BasicDailyDataInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            date = splitline[0].getDateTimeFromString();
            dealedStock = splitline[1].getDecimalFromString();
            volume = splitline[2].getDecimalFromString();
            open = splitline[3].getDecimalFromString();
            high = splitline[4].getDecimalFromString();
            low = splitline[5].getDecimalFromString();
            close = splitline[6].getDecimalFromString();
            change = splitline[7].getDecimalFromString();
            dealedOrder = splitline[8].getDecimalFromString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.ToString("yyyy-MM-dd") + "\t");
            sb.Append(dealedStock.ToString() + "\t");
            sb.Append(volume.ToString() + "\t");
            sb.Append(open.ToString() + "\t");
            sb.Append(high.ToString() + "\t");
            sb.Append(low.ToString() + "\t");
            sb.Append(close.ToString() + "\t");
            sb.Append(change.ToString() + "\t");
            sb.Append(dealedOrder.ToString()+"\t");
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
            sb.Append("成交筆數");
            return sb.ToString();
        }

        public int CompareTo(object obj)
        {
            var that = (BasicDailyDataInformation)obj;
            return this.date.CompareTo(that.date);

        }
    }
}
