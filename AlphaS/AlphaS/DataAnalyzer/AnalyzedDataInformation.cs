using AlphaS.BasicDailyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class AnalyzedDataInformation : BasicDailyDataInformation
    {
        public AnalyzedDataInformation(string loadString)
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
            sb.Append(dealedOrder.ToString());
            return sb.ToString();
        }

        new public static string ToTitle()
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

        public decimal N_open;
        public decimal N_high;
        public decimal N_low;
        public decimal N_close;
        public decimal divide;
        public double divideWeight;
    }
}
