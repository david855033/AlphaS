using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class ScoreDataManager: IScoreDataManager
    {
        public ScoreDataManager() { }
        public ScoreDataManager(string ANALYZED_DATA_FOLDER)
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

        public List<ScoreDataInformation> getScoreData(string ID)
        {
            var result = new List<ScoreDataInformation>();
            string filepath = _baseFolder + $@"\{ID}_ScoreData.txt";
            if (File.Exists(_baseFolder + $@"\{ID}_ScoreData.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(new ScoreDataInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }


        public void saveScoreData(string ID, List<ScoreDataInformation> scoreDataToWrite)
        {
            string filepath = _baseFolder + $@"\{ID}_ScoreData.txt";
            using (var sw = new StreamWriter(filepath, false, Encoding.Default))
            {
                var toWrite = scoreDataToWrite.ToList();
                toWrite.Sort();
                foreach (var r in toWrite)
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }
    }
}
