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

        public decimal N_open;
        public decimal N_high;
        public decimal N_low;
        public decimal N_close;
        public decimal divide;
        public double divideWeight;

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
            divide = splitline[9].getDecimalFromString();
            divideWeight = splitline[10].getDoubleFromString();
            N_open = splitline[11].getDecimalFromString();
            N_high = splitline[12].getDecimalFromString();
            N_low = splitline[13].getDecimalFromString();
            N_close = splitline[14].getDecimalFromString();
        }

        public AnalyzedDataInformation(BasicDailyDataInformation basicDailyDataInformation)
        {
            date = basicDailyDataInformation.date;
            dealedStock = basicDailyDataInformation.dealedStock;
            volume = basicDailyDataInformation.volume;
            open = basicDailyDataInformation.open;
            high = basicDailyDataInformation.high;
            low = basicDailyDataInformation.low;
            close = basicDailyDataInformation.close;
            change = basicDailyDataInformation.change;
            dealedOrder = basicDailyDataInformation.dealedOrder;
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
            sb.Append(dealedOrder.ToString() + "\t");
            sb.Append(divide.ToString() + "\t");
            sb.Append(divideWeight.ToString() + "\t");
            sb.Append(N_open.ToString() + "\t");
            sb.Append(N_high.ToString() + "\t");
            sb.Append(N_low.ToString() + "\t");
            sb.Append(N_close.ToString());
            return sb.ToString();
        }

    }
}
