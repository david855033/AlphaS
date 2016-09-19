using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    interface IBasicDailyDataManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();
        List<BasicDailyDataFileStatusInformation> getFileStatus(string ID);
        void saveFileStatus(string ID, List<BasicDailyDataFileStatusInformation> fileStatusToWrite);

        List<BasicDailyDataInformation> getBasicDailyData(string ID);
        void saveBasicDailyData(string ID, List<BasicDailyDataInformation> basicDailyDataToWrite);
    }
}
