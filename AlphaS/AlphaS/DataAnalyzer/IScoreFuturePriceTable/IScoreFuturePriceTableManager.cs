using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface IScoreFuturePriceTableManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();

        void resetScoreFuturePriceTable();
        void appendScoreFuturePriceTable(List<ScoreFuturePriceDataInformation> ScoreFuturePriceTableToWrite);
    }
}
