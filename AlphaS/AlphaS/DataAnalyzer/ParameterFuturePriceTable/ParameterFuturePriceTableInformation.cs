using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer.ParameterCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class ParameterFuturePriceTableInformation : IComparable
    {
        public static int[] FUTURE_PRICE_DAYS = new int[] { 10, 20, 30, 40, 50, 60, 70, 80 };

        public decimal parameterValue;
        public decimal?[] futurePriceLogs = new decimal?[FUTURE_PRICE_DAYS.Length];
        public decimal?[] futurePriceRanks = new decimal?[FUTURE_PRICE_DAYS.Length];

        public int CompareTo(object obj)
        {
            var that = (ParameterFuturePriceTableInformation)obj;
            return this.parameterValue.CompareTo(that.parameterValue);
        }

        public ParameterFuturePriceTableInformation()
        {
        }
        public ParameterFuturePriceTableInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;
            parameterValue = splitline[i++].getDecimalFromString();
            if (splitline.Length > i)
            {
                int x = i;
                while (x < splitline.Length && x - i < futurePriceLogs.Length)
                {
                    if (splitline[x] != "NA")
                    {
                        futurePriceLogs[x - i] = splitline[x].getDecimalFromString();
                    }
                    x++;
                }
                i = x;
            }
            if (splitline.Length > i)
            {
                int x = i;
                while (x < splitline.Length && x - i < futurePriceRanks.Length)
                {
                    if (splitline[x] != "NA")
                    {
                        futurePriceRanks[x - i] = splitline[x].getDecimalFromString();
                    }
                    x++;
                }
                i = x;
            }
        }

        public ParameterFuturePriceTableInformation(decimal parameterValue)
        {
            this.parameterValue = parameterValue;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(parameterValue.ToString());
            for (int i = 0; i < futurePriceLogs.Length; i++)
            {
                if (futurePriceLogs[i] != null)
                {
                    sb.Append("\t" + futurePriceLogs[i].round(4).ToString());
                }
                else
                {
                    sb.Append("\tNA");
                }
            }
            for (int i = 0; i < futurePriceRanks.Length; i++)
            {
                if (futurePriceRanks[i] != null)
                {
                    sb.Append("\t" + futurePriceRanks[i].round(4).ToString());
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
