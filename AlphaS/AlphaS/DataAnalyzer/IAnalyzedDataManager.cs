using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface IAnalyzedDataManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();

        List<AnalyzedDataInformation> getAnalyzedData(string ID);
        void saveAnalyzedData(string ID, List<AnalyzedDataInformation> AnalyzedDataToWrite);
    }
}
