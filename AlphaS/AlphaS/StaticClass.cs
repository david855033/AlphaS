using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlphaS
{
    public static class StaticClass
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
            input = Regex.Replace(input, @"[^0-9.-]", "");
            if (input == "")
            {
                success = true;
                return 0;
            }
            success = decimal.TryParse(input, out result);
            return result;
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

        public static DateTime getDateTimeFromStringMK(this string input)
        {
            bool success = false;
            return input.getDateTimeFromStringMK(out success);
        }
        public static DateTime getDateTimeFromStringMK(this string input, out bool success)
        {
            DateTime result;
            int index = input.IndexOf('/');
            if (index < 0) index = input.IndexOf('-');
            int year = 0;
            success = int.TryParse(input.Substring(0, index), out year);
            if (success)
            {
                year += 1911;
                input = year + input.Substring(index, input.Length - index);
                success = DateTime.TryParse(input, out result);
                return result;
            }
            return DateTime.MinValue;
        }

        public static string getFileFolderFromPath(this string input)
        {
            return input.Substring(0, input.LastIndexOf('\\'));
        }
    }
}
