using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer.ParameterCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class FuturePriceDataInformation : IComparable
    {
        public static int[] FUTURE_PRICE_DAYS = new int[] { 10, 20, 30, 40, 50, 60, 70, 80 };
        // ParameterFuturePriceTableInformation內要一樣

        public DateTime date;
        public decimal?[] futurePrices = new decimal?[FUTURE_PRICE_DAYS.Length];
        public decimal?[] futurePriceRank = new decimal?[FUTURE_PRICE_DAYS.Length];

        public int CompareTo(object obj)
        {
            var that = (FuturePriceDataInformation)obj;
            return this.date.CompareTo(that.date);
        }


        public FuturePriceDataInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;
            date = splitline[i++].getDateTimeFromString();
            if (splitline.Length > i)
            {
                int x = i;
                while (x < splitline.Length && x - i < FUTURE_PRICE_DAYS.Length)
                {
                    if (splitline[x] != "NA")
                    {
                        futurePrices[x - i] = splitline[x].getDecimalFromString();
                    }
                    x++;
                }
                i = x;
            }

            if (splitline.Length > i)
            {
                int x = i;
                while (x < splitline.Length && x - i < FUTURE_PRICE_DAYS.Length)
                {
                    if (splitline[x] != "NA")
                    {
                        futurePriceRank[x - i] = splitline[x].getDecimalFromString();
                    }
                    x++;
                }
            }
        }

        public FuturePriceDataInformation(BasicDailyDataInformation basicDailyDataInformation)
        {
            date = basicDailyDataInformation.date;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.ToString("yyyy-MM-dd"));
            for (int i = 0; i < futurePrices.Length; i++)
            {
                if (futurePrices[i] != null)
                {
                    sb.Append("\t" + futurePrices[i].round(2).ToString());
                }
                else
                {
                    sb.Append("\tNA");
                }
            }
            for (int i = 0; i < futurePriceRank.Length; i++)
            {
                if (futurePriceRank[i] != null)
                {
                    sb.Append("\t" + futurePriceRank[i].round(2).ToString());
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
