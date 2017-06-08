using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    
    public static class JSONParser
    {
        public static Dictionary<string, string> ParseJSON(this string input)
        {
            var result = new Dictionary<string, string>();
            input = input.GetWithin("{", "}");
            int currentIndex = 0;
            do
            {
                var field = input.GetOneUnit(currentIndex, out currentIndex);
                currentIndex++;
                var value = input.GetOneUnit(currentIndex, out currentIndex);
                currentIndex++;
                result[field.Trim('"')] = value.Trim('"');
            } while (currentIndex < input.Length);

            return result;
        }

        public static string GetOneUnit(this string s, int startIndex, out int endIndex)
        {
            var result = "";
            int i = startIndex;
            var expectedStack = new List<Char>();
            for (; i < s.Length; i++)
            {
                if (expectedStack.Count == 0)
                {
                    if (s[i] == ':') break;
                    if (s[i] == ',') break;
                }
                else if (s[i] == expectedStack.Last())
                {
                    expectedStack.RemoveAt(expectedStack.Count - 1);
                    result += s[i];
                    continue;
                }

                if (s[i] == '"') { expectedStack.Add('"'); }
                else if (s[i] == '[') { expectedStack.Add(']'); }
                else if (s[i] == '{') { expectedStack.Add('}'); }


                result += s[i];
            }

            endIndex = Math.Min(s.Length - 1, i++);
            return result;
        }

        public static string GetWithin(this string s, string startChar, string endChar)
        {
            int startIndex = s.IndexOf(startChar);
            int endIndex = s.LastIndexOf(endChar);
            if (startIndex >= 0 && endIndex >= 0 && endIndex >= startIndex + startChar.Length)
            {
                startIndex += startChar.Length;
                return s.Substring(startIndex, endIndex - startIndex);
            }

            return s.Trim();
        }

        public static List<List<string>> ParseTable(this string input)
        {
            var resultTable = new List<List<string>>();

            var content = input.GetWithin("[", "]");
            int currentIndex = 0;
            if(content.Trim()=="") return resultTable;
            do
            {
                var row = content.GetOneUnit(currentIndex, out currentIndex).GetWithin("[", "]");
                int insideIndex = 0;
                var rowList = new List<string>();
                do
                {
                    var col = row.GetOneUnit(insideIndex, out insideIndex).Trim('"');
                    insideIndex++;
                    rowList.Add(col.Replace("\\", ""));
                } while (insideIndex < row.Length);
                currentIndex++;
                resultTable.Add(rowList);
            } while (currentIndex < content.Length);

            return resultTable;
        }
    }
}
