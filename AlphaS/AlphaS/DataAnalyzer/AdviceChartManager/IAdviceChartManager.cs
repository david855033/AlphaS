using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface IAdviceChartManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();

        List<DateTime> getExistedDate();
        void resetAllChart();

        List<AdviceChartInformation> getAdviceChart(DateTime date);
        void saveDailyChart(DateTime date, IEnumerable<AdviceChartInformation> adviceChart);
    }
}
