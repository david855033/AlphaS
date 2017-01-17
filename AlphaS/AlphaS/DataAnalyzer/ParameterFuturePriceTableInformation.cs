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
        public static int[] FUTURE_PRICE_DAYS = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80 };

        public decimal parameterValue;
        public decimal?[] futurePricesLogs = new decimal?[FUTURE_PRICE_DAYS.Length];

        public int CompareTo(object obj)
        {
            var that = (ParameterFuturePriceTableInformation)obj;
            return this.parameterValue.CompareTo(that.parameterValue);
        }

        public ParameterFuturePriceTableInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;
            parameterValue = splitline[i++].getDecimalFromString();
            if (splitline.Length > i)
            {
                for (int x = i; x < splitline.Length; x++)
                {
                    if (splitline[x] != "NA")
                    {
                        futurePricesLogs[x - i] = splitline[x].getDecimalFromString();
                    }
                }
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
            for (int i = 0; i < futurePricesLogs.Length; i++)
            {
                if (futurePricesLogs[i] != null)
                {
                    sb.Append("\t" + futurePricesLogs[i].round(4).ToString());
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
