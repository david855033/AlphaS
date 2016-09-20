using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AlphaS.Settings;
using AlphaS.StockList;
using AlphaS.BasicDailyData;
namespace AlphaS.CoreNS
{
    public static class Core
    {
        public static readonly string DEFAULT_FOLDER = @"D:\AlphaS";

        static readonly string SETTING_FILE_NAME = @"Settings.txt";
        static readonly string SETTING_FILE_PATH = DEFAULT_FOLDER + "\\" + SETTING_FILE_NAME;
        public static readonly ISettingManager settingManager = new SettingManager(SETTING_FILE_PATH);

        public static readonly IStockListManager stockListManager = new StockListManager();

        static readonly string BASIC_DAILY_DATA_FOLDER = DEFAULT_FOLDER + "\\" + "BasicDailyData";
        public static readonly IBasicDailyDataManager basicDailyDataManager = new BasicDailyDataManager(BASIC_DAILY_DATA_FOLDER);

        public static bool closeAllWindow=false;

        static Core()
        {
        }
    }
}
