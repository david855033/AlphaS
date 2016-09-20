using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    interface IBasicDailyDataMissionListGenerator
    {   
        List<BasicDailyDataMission> getMissionList();
    }
}
