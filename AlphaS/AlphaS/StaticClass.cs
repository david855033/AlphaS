using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlphaS
{
    static class StaticClass
    {
        public static int getIntFromString(this string input)
        {
            bool success = false;
            return input.getIntFromString(out success);
        }
        public static int getIntFromString(this string input, out bool success)
        {
            int result;
            input = Regex.Replace(input, @"[^0-9.]", "");
            if (input == "")
            {
                success = true;
                return 0;
            }
            success = int.TryParse(input, out result);
            return result;
        }
        public static decimal getDecimalFromString(this string input)
        {
            bool success = false;
            return input.getDecimalFromString(out success);
        }
        public static decimal getDecimalFromString(this string input, out bool success)
        {
            decimal result;
            input = Regex.Replace(input, @"[^0-9.]", "");
            if (input == "")
            {
                success = true;
                return 0;
            }
            success = decimal.TryParse(input, out result);
            return result;
        }

        public static DateTime transferMKtoBC(this DateTime input)
        {
            return input.AddYears(1911);
        }

        public static DateTime getDateTimeFromString(this string input)
        {
            bool success = false;
            return input.getDateTimeFromString(out success);
        }
        public static DateTime getDateTimeFromString(this string input, out bool success)
        {
            DateTime result;
            success = DateTime.TryParse(input, out result);
            return result;
        }

        public static string getFileFolderFromPath(this string input)
        {
            return input.Substring(0, input.LastIndexOf('\\'));
        }
    }
}
