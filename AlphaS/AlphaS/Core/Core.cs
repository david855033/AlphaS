using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AlphaS.Settings;
using AlphaS.StockList;

namespace AlphaS.CoreNS
{
    public static class Core
    {
        public static readonly string DEFAULT_FOLDER = @"D:\AlphaS";

        static readonly string SETTING_FILE_NAME = @"Settings.txt";
        static readonly string SETTING_FILE_PATH = DEFAULT_FOLDER + "\\" + SETTING_FILE_NAME;
        public static readonly ISettingManager settingManager = new SettingManager(SETTING_FILE_PATH);

        public static readonly StockListManager stockListManager = new StockListManager();

        static Core()
        {

        }
    }
}
