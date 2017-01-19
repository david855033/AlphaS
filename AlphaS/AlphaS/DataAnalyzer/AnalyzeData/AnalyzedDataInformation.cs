using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer.ParameterCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class AnalyzedDataInformation : BasicDailyDataInformation
    {
        public decimal avg;
        public decimal N_avg;
        public decimal N_open;
        public decimal N_high;
        public decimal N_low;
        public decimal N_close;
        public decimal divide;
        public decimal volumePerOrder;
        public double divideWeight;
        public bool recentEmpty; //120day

        static public Dictionary<string, int> parameterIndex = new Dictionary<string, int>();
        static AnalyzedDataInformation() { paramterIndexInitializer.initialize(); }

        public Nullable<decimal>[] parameters =
            new Nullable<decimal>[parameterIndex.Count()];

        public AnalyzedDataInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;
            date = splitline[i++].getDateTimeFromString();
            dealedStock = splitline[i++].getDecimalFromString();
            volume = splitline[i++].getDecimalFromString();
            open = splitline[i++].getDecimalFromString();
            high = splitline[i++].getDecimalFromString();
            low = splitline[i++].getDecimalFromString();
            close = splitline[i++].getDecimalFromString();
            change = splitline[i++].getDecimalFromString();
            dealedOrder = splitline[i++].getDecimalFromString();
            avg = splitline[i++].getDecimalFromString();
            volumePerOrder = splitline[i++].getDecimalFromString();
            recentEmpty = splitline[i++].getBoolFromString();
            divide = splitline[i++].getDecimalFromString();
            divideWeight = splitline[i++].getDoubleFromString();
            N_open = splitline[i++].getDecimalFromString();
            N_high = splitline[i++].getDecimalFromString();
            N_low = splitline[i++].getDecimalFromString();
            N_close = splitline[i++].getDecimalFromString();
            N_avg = splitline[i++].getDecimalFromString();
            if (splitline.Length > i)
            {
                for (int x = i ; x < splitline.Length; x++)
                {
                    if (splitline[x] != "NA")
                    {
                        parameters[x - i] = splitline[x].getDecimalFromString();
                    }
                }
            }
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
            divideWeight = 0;

        }

        public void setNprice()
        {
            if (dealedStock >= 100) { avg = volume / dealedStock; } else { avg = close; }
            N_open = open * divideWeight.getDecimalFromDouble();
            N_high = high * divideWeight.getDecimalFromDouble();
            N_low = low * divideWeight.getDecimalFromDouble();
            N_close = close * divideWeight.getDecimalFromDouble();
            N_avg = avg * divideWeight.getDecimalFromDouble();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.ToString("yyyy-MM-dd"));
            sb.Append("\t" + dealedStock.round(2).ToString());
            sb.Append("\t" + volume.round(2).ToString());
            sb.Append("\t" + open.round(2).ToString());
            sb.Append("\t" + high.round(2).ToString());
            sb.Append("\t" + low.round(2).ToString());
            sb.Append("\t" + close.round(2).ToString());
            sb.Append("\t" + change.round(2).ToString());
            sb.Append("\t" + dealedOrder.round(2).ToString());
            sb.Append("\t" + avg.round(2).ToString());
            sb.Append("\t" + volumePerOrder.round(2).round(2).ToString());
            sb.Append("\t" + (recentEmpty ? "1" : "0"));
            sb.Append("\t" + divide.round(2).ToString());
            sb.Append("\t" + divideWeight.round(2).ToString());
            sb.Append("\t" + N_open.round(2).ToString());
            sb.Append("\t" + N_high.round(2).ToString());
            sb.Append("\t" + N_low.round(2).ToString());
            sb.Append("\t" + N_close.round(2).ToString());
            sb.Append("\t" + N_avg.round(2).ToString());
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].HasValue)
                {
                    sb.Append("\t" + parameters[i].round(2).ToString());
                }
                else
                {
                    sb.Append("\tNA");
                }
            }
            return sb.ToString();
        }

    }
}
