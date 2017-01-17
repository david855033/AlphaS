using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface IParameterFuturePriceTableManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();

        List<ParameterFuturePriceTableInformation> getParameterFuturePriceTable(string parameterName);
        void saveParameterFuturePriceTable(string parameterName,List<ParameterFuturePriceTableInformation> FuturePriceDataToWrite);
    }
}
