using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (input == "")
            {
                success = true;
                return 0;
            }
            success = int.TryParse(input, out result);
            return result;
        }

        public static string getFileFolderFromPath(this string input)
        {
            return input.Substring(0, input.LastIndexOf('\\'));
        }
    }
}
