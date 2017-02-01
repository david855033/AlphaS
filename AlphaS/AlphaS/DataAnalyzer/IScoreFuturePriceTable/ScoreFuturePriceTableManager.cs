using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class ScoreFuturePriceTableManager : IScoreFuturePriceTableManager
    {
        public ScoreFuturePriceTableManager() { }
        public ScoreFuturePriceTableManager(string ANALYZED_DATA_FOLDER)
        {
            setBaseFolder(ANALYZED_DATA_FOLDER);
        }

        private string _baseFolder;
        public void setBaseFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _baseFolder = path;
        }
        public string getBaseFolder()
        {
            return _baseFolder;
        }


        public void appendScoreFuturePriceTable(List<ScoreFuturePriceDataInformation> ScoreFuturePriceTableToWrite)
        {
            string filepath = _baseFolder + $@"\ScoreFuturePriceTable.txt";
            using (var sw = new StreamWriter(filepath, true, Encoding.Default))
            {
                var toWrite = ScoreFuturePriceTableToWrite.ToList();
                toWrite.Sort();
                foreach (var r in toWrite)
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }

        public void resetScoreFuturePriceTable()
        {
            if (File.Exists(_baseFolder + $@"\ScoreFuturePriceTable.txt")) File.Delete(_baseFolder + $@"\ScoreFuturePriceTable.txt");
                
        }

    }
}
