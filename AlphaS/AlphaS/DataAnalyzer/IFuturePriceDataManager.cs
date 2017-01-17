using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface IFuturePriceDataManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();

        List<FuturePriceDataInformation> getFuturePriceData(string ID);
        void saveFuturePriceData(string ID, List<FuturePriceDataInformation> FuturePriceDataToWrite);
    }
}
