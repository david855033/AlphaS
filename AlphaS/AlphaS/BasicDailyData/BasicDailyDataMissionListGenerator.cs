using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaS.CoreNS;
using AlphaS.StockList;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataMissionListGenerator : IBasicDailyDataMissionListGenerator
    {
        private int startMonth;
        public void setStartMonth(int startMonth)
        {
            this.startMonth = startMonth;
        }

        private int startYear;
        public void setStartYear(int startYear)
        {
            this.startYear = startYear;
        }

        private List<StockInfomation> stockList;
        public void setStockList(IEnumerable<StockInfomation> stockList)
        {
            this.stockList = stockList.ToList();
        }

        public List<BasicDailyDataMission> getMissionList(bool readAll)
        {
            var resultList = new List<BasicDailyDataMission>();

            foreach (var currentStock in stockList)
            {
                int currentYear = startYear;
                int currentMonth = startMonth;
                while (currentYear < DateTime.Now.Year ||
                    (currentYear == DateTime.Now.Year && currentMonth <= DateTime.Now.Month))
                {
                    var toAdd = new BasicDailyDataMission()
                    {
                        ID = currentStock.ID,
                        type = currentStock.type,
                        month = currentMonth,
                        year = currentYear
                    };

                    if (!readAll)
                    {
                        var fileStatus = Core.basicDailyDataManager.getFileStatus(currentStock.ID);
                        int index = fileStatus.BinarySearch(new BasicDailyDataFileStatusInformation()
                        { year = currentYear.ToString(), month = currentMonth.ToString() });
                        if (index < 0
                            ||
                            index >= 0 &&
                                (
                                    (
                                        fileStatus[index].fileStatus == FileStatus.Temp
                                        ||
                                        (
                                            fileStatus[index].fileStatus == FileStatus.Null
                                            &&
                                            index >= fileStatus.Count() - 1
                                        )
                                    ) &&
                                        fileStatus[index].modifiedTime.getDateTimeFromString()
                                        .ToString("yyyyMMdd") !=
                                        DateTime.Now.ToString("yyyyMMdd")
                                            ||
                                        (
                                        fileStatus[index].modifiedTime.getDateTimeFromString()
                                        .ToString("yyyyMMdd") ==
                                        DateTime.Now.ToString("yyyyMMdd")
                                            &&
                                        fileStatus[index].modifiedTime.getDateTimeFromString().Hour < 16
                                            &&
                                        DateTime.Now.Hour >= 16
                                        )
                                )
                            )
                        {
                            resultList.Add(toAdd);
                        }
                    }
                    else
                    {
                        resultList.Add(toAdd);
                    }

                    currentMonth++;
                    if (currentMonth > 12)
                    {
                        currentMonth = 1;
                        currentYear++;
                    }
                }
            }
            return resultList;
        }

    }
}
