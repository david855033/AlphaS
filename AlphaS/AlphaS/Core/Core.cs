using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AlphaS.FileProcess;
using AlphaS.Settings;
using AlphaS.StockList;

namespace AlphaS.Core
{
    public static class Core
    {
        static readonly string DEFAULT_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\AlphaS";

        static readonly string SETTING_FILE_NAME = @"Settings.txt";
        static readonly string SETTING_FILE_PATH = DEFAULT_FOLDER + "\\" + SETTING_FILE_NAME;
        static readonly SettingManager settingManager = new SettingManager(SETTING_FILE_PATH);

        static readonly string STOCKLIST_FILE_PATH = settingManager.getSetting("STOCKLIST_FILE_PATH");
        static readonly StockListManager stockListManager = new StockListManager(STOCKLIST_FILE_PATH);

        static Core()
        {

        }



    }
}
