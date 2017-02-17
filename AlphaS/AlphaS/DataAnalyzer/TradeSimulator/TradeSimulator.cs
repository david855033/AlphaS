﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class TradeSimulator
    {
        List<TradingProtocal> protocals = new List<TradingProtocal>();
        public void clearTradingProtocals() { protocals = new List<TradingProtocal>(); }
        public void addTradingProtocals(List<TradingProtocal> TradingProtocals) { protocals.AddRange(TradingProtocals); }
        public void addTradingProtocals(TradingProtocal TradingProtocal) { protocals.Add(TradingProtocal); }

        List<Trader> traders = new List<Trader>();

        public void initializedTradeSim()
        {
            foreach (var protocal in protocals)
            {
                traders.Add(new Trader(protocal));
            }
        }

        public void goNextDay(DateTime currentDate, List<DailyChartInformation> dailyChart)
        {
            foreach (var trader in traders)
            {
                trader.goNextDay(currentDate, dailyChart);
            }
        }


        public List<string> getTradeResult()
        {
            List<string> resultList = new List<string>();
            foreach (var trader in traders)
            {
                resultList.Add(trader.getResult());
            }
            return resultList;
        }
    }
}