using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
        public static bool getBoolFromString(this string input)
        {
            return input == "1";
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

        public static decimal getDecimalFromDouble(this double input)
        {
            return Convert.ToDecimal(input);
        }

        public static double getDoubleFromDecimal(this decimal input)
        {
            return Convert.ToDouble(input);
        }

        public static double round(this double input, int deci)
        {
            return Math.Round(input, deci);
        }
        public static decimal round(this decimal? input, int deci)
        {
            return Math.Round(input.Value, deci);
        }
        public static decimal round(this decimal input, int deci)
        {
            return Math.Round(input, deci);
        }

        public static Double getDoubleFromString(this string input)
        {
            bool success = false;
            return input.getDoubleFromString(out success);
        }
        public static Double getDoubleFromString(this string input, out bool success)
        {
            Double result;
            input = Regex.Replace(input, @"[^0-9.-]", "");
            if (input == "")
            {
                success = true;
                return 0;
            }
            success = Double.TryParse(input, out result);
            return result;
        }

        public static string getRidOfPostStar(this string input)
        {
            return input.Replace("＊", "");
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

        public static string getElaplsedTime(this double input)
        {
            if (input < 60)
            {
                return $"{input.round(0)}秒";
            }
            else if (input < 60 * 60)
            {
                return $"{(input - input % 60) / 60}分{(input % 60).round(0)}秒";
            }
            else
            {
                return $"{(input - input % 3600) / 3600}時{(input - input % 60) % 60}分{(input % 60).round(0)}秒";
            }
        }

        public static decimal[] addUpDecimalArray(this decimal[] input, decimal[] toAdd)
        {
            int toCount = Math.Min(input.Length, toAdd.Length);
            var result = new decimal[toCount];
            for (int i = 0; i < toCount; i++)
            {
                result[i] = input[i] + toAdd[i];
            }
            return result;
        }
        public static decimal[] divideElementBy(this decimal[] input, int divider)
        {
            var result = new decimal[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i] / divider;
            }
            return result;
        }
        public static decimal[] round(this decimal[] input, int deci)
        {
            var result = new decimal[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i].round(deci);
            }
            return result;
        }
        public static decimal[] exp(this decimal[] input)
        {
            var result = new decimal[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ((Math.Exp(Convert.ToDouble(input[i])) - 1) * 100).getDecimalFromDouble();
            }
            return result;
        }

        public static decimal getAverage(this IEnumerable<decimal> input)
        {
            decimal sum = 0;
            foreach (var i in input)
                sum += i;
            return sum / input.Count();
        }


        private static Action EmptyDelegate = delegate () { };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}

