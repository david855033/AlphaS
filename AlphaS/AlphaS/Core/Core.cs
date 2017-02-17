using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AlphaS.Settings;
using AlphaS.StockList;
using AlphaS.BasicDailyData;
using AlphaS.DataAnalyzer;
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

        static readonly string ANALYZED_DATA_FOLDER = DEFAULT_FOLDER + "\\" + "AnalyzedData";
        public static readonly IAnalyzedDataManager analyzedDataManager = new AnalyzedDataManager(ANALYZED_DATA_FOLDER);

        static readonly string FUTURE_PRICE_DATA_FOLDER = DEFAULT_FOLDER + "\\" + "FuturePriceData";
        public static readonly IFuturePriceDataManager futurePriceDataManager = new FuturePriceDataManager(FUTURE_PRICE_DATA_FOLDER);

        static readonly string PARAMETER_FUTURE_PRICE_TABLE_FOLDER = DEFAULT_FOLDER + "\\" + "ParameterFuturePriceTable";
        public static readonly IParameterFuturePriceTableManager parameterFuturePriceTableManager = new ParameterFuturePriceTableManager(PARAMETER_FUTURE_PRICE_TABLE_FOLDER);

        static readonly string FINAL_PARAMETER_FUTURE_PRICE_TABLE_FOLDER = DEFAULT_FOLDER + "\\" + "FinalParameterFuturePriceTable";
        public static readonly IParameterFuturePriceTableManager finalParameterFuturePriceTableManager = new ParameterFuturePriceTableManager(FINAL_PARAMETER_FUTURE_PRICE_TABLE_FOLDER);

        static readonly string SCORE_DATA_FOLDER = DEFAULT_FOLDER + "\\" + "ScoreData";
        public static readonly IScoreDataManager scoreDataManager = new ScoreDataManager(SCORE_DATA_FOLDER);

        static readonly string SCORE_FUTURE_PRICE_DATA_FOLDER = DEFAULT_FOLDER + "\\" + "ScoreFuturePrice";
        public static readonly IScoreFuturePriceTableManager scoreFuturePriceTableManager = new ScoreFuturePriceTableManager(SCORE_FUTURE_PRICE_DATA_FOLDER);

        static readonly string DAILY_CHART_FOLDER = DEFAULT_FOLDER + "\\" + "DailyChart";
        public static readonly IDailyChartDataManager dailyChartDataManager = new DailyChartDataManager(DAILY_CHART_FOLDER);

        public static bool closeAllWindow = false;

        static Core()
        {
        }
    }
}
