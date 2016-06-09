using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AlphaS.Core
{
    public static class Core
    {
        static readonly string DEFALT_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\AlphaS";
        static readonly string SETTING_FILE_NAME = DEFALT_FOLDER + @"\Settings.txt";


        static Core()
        {

        }

        public static string getSetting(string fieldName)
        {
            if (settings.ContainsKey(fieldName))
            {
                return settings[fieldName];
            }
            return "";
        }
        public static void saveSetting(string fieldName, string Content)
        {

        }
        static Dictionary<string, string> settings;
        static void loadSettings()
        {
            bool isSettingFileExist = File.Exists(SETTING_FILE_NAME);
            if (isSettingFileExist)
            {
                using (StreamReader sr = new StreamReader(SETTING_FILE_NAME, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] spltline = sr.ReadLine().Split('=');
                        settings.Add(spltline.First(), spltline.Last());
                    }
                }
            }
        }
        static void writeSettingFile()
        {
            using (StreamWriter sw = new StreamWriter(SETTING_FILE_NAME, false, Encoding.Default))
            {

            }
        }


        static List<StockInfomation> stockList = new List<StockInfomation>();

    }
}
