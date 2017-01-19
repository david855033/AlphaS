using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class ScoreDataInformation : IComparable
    {
        public readonly static int[] SCORE_DAY_RANGE_DEFINITION = new int[] { 40, 80 };
        public DateTime date;
        public decimal[] valueScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];
        public decimal[] rankScore = new decimal[SCORE_DAY_RANGE_DEFINITION.Length];

        public int CompareTo(object obj)
        {
            var that = (ScoreDataInformation)obj;
            return this.date.CompareTo(that.date);
        }

        public ScoreDataInformation()
        {
        }
        public ScoreDataInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            int i = 0;
            date = splitline[i++].getDateTimeFromString();
            if (splitline.Length > i)
            {
                int x = i;
                while (x < splitline.Length && x - i < valueScore.Length)
                {
                    if (splitline[x] != "NA")
                    {
                        valueScore[x - i] = splitline[x].getDecimalFromString();
                    }
                    x++;
                }
                i = x;
            }
            if (splitline.Length > i)
            {
                int x = i;
                while (x < splitline.Length && x - i < rankScore.Length)
                {
                    if (splitline[x] != "NA")
                    {
                        rankScore[x - i] = splitline[x].getDecimalFromString();
                    }
                    x++;
                }
                i = x;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date.ToString("yyyy-MM-dd"));
            for (int i = 0; i < valueScore.Length; i++)
            {
                sb.Append("\t" + valueScore[i].round(2).ToString());
            }

            for (int i = 0; i < rankScore.Length; i++)
            {
                sb.Append("\t" + rankScore[i].round(2).ToString());
            }

            return sb.ToString();
        }
    }
}
