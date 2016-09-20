using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.StockList;

namespace AlphaS.BasicDailyData
{
    interface IBasicDailyDataMissionListGenerator
    {   
        List<BasicDailyDataMission> getMissionList(bool readAll);
        void setStartYear(int startYear);
        void setStartMonth(int startMonth);
        void setStockList(IEnumerable<StockInfomation> stockList);
    }
}
