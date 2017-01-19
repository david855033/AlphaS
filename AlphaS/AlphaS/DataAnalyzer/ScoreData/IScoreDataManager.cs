using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface IScoreDataManager
    {
        void setBaseFolder(string path);
        string getBaseFolder();

        List<ScoreDataInformation> getScoreData(string ID);
        void saveScoreData(string ID, List<ScoreDataInformation> ScoreDataToWrite);
    }
}
