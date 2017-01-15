using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer.ParameterCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class FuturePriceDataInformation
    {
        public static int[] FUTURE_PRICE_DAYS = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80 };

        public DateTime date;
        public decimal?[] futurePrices = new decimal?[FUTURE_PRICE_DAYS.Length];

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
                for (int x = i; x < splitline.Length; x++)
                {
                    if (splitline[x] != "NA")
                    {
                        futurePrices[x - i] = splitline[x].getDecimalFromString();
                    }
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
                    sb.Append("\t" + futurePrices[i].ToString());
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
