using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.CoreNS;
namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataMissionListGenerator : IBasicDailyDataMissionListGenerator
    {
        public List<BasicDailyDataMission> getMissionList()
        {
            var resultList = new List<BasicDailyDataMission>();

            resultList.Add(new BasicDailyDataMission() { ID = "1101", year = 2016, month = 8, type = "A" });
            resultList.Add(new BasicDailyDataMission() { ID = "1101", year = 2016, month = 9, type = "A" });
            resultList.Add(new BasicDailyDataMission() { ID = "1102", year = 2016, month = 7, type = "A" });
            resultList.Add(new BasicDailyDataMission() { ID = "1102", year = 2016, month = 8, type = "A" });
            resultList.Add(new BasicDailyDataMission() { ID = "1102", year = 2016, month = 9, type = "A" });
            return resultList;
        }
    }
}
