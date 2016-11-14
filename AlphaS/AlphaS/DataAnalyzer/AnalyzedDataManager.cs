using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class AnalyzedDataManager : IAnalyzedDataManager
    {
        public AnalyzedDataManager() { }
        public AnalyzedDataManager(string ANALYZED_DATA_FOLDER)
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

        public List<AnalyzedDataInformation> getAnalyzedData(string ID)
        {
            var result = new List<AnalyzedDataInformation>();
            string filepath = _baseFolder + $@"\{ID}_AnalyzedData.txt";
            if (File.Exists(_baseFolder + $@"\{ID}_AnalyzedData.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(new AnalyzedDataInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }


        public void saveAnalyzedData(string ID, List<AnalyzedDataInformation> AnalyzedDataToWrite)
        {
            string filepath = _baseFolder + $@"\{ID}_AnalyzedData.txt";
            using (var sw = new StreamWriter(filepath, false, Encoding.Default))
            {
                var toWrite = AnalyzedDataToWrite.ToList();
                toWrite.Sort();
                foreach (var r in toWrite)
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }

    }
}
