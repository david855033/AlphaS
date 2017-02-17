using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    interface ITradeSimulator
    {
        void setTradingTimeSpectrum(DateTime start, DateTime End);
        void clearTradingProtocals();
        void addTradingProtocals(List<TradingProtocal> TradingProtocals);
        void addTradingProtocals(TradingProtocal TradingProtocal);

        void startTradeSim();

        void getTradeResult();
    }
}

