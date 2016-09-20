using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataManager : IBasicDailyDataManager
    {
        public BasicDailyDataManager() { }
        public BasicDailyDataManager(string BASIC_DAILY_DATA_FOLDER)
        {
            setBaseFolder(BASIC_DAILY_DATA_FOLDER);
        }

        private string _baseFolder;
        public string getBaseFolder()
        {
            return _baseFolder;
        }

        public void setBaseFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _baseFolder = path;
        }

        public List<BasicDailyDataFileStatusInformation> getFileStatus(string ID)
        {
            var result = new List<BasicDailyDataFileStatusInformation>();
            string filepath = _baseFolder + $@"\{ID}_status.txt";
            if (File.Exists(_baseFolder + $@"\{ID}_status.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(new BasicDailyDataFileStatusInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }

        public void saveFileStatus(string ID, List<BasicDailyDataFileStatusInformation> fileStatusToWrite)
        {
            string filepath = _baseFolder + $@"\{ID}_status.txt";
            using (var sw = new StreamWriter(filepath, false, Encoding.Default))
            {
                var toWrite = fileStatusToWrite.ToList();
                toWrite.Sort();
                foreach (var r in toWrite)
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }

        public List<BasicDailyDataInformation> getBasicDailyData(string ID)
        {
            var result = new List<BasicDailyDataInformation>();
            string filepath = _baseFolder + $@"\{ID}_BasicDailyData.txt";
            if (File.Exists(_baseFolder + $@"\{ID}_BasicDailyData.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(new BasicDailyDataInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }

        public void saveBasicDailyData(string ID, List<BasicDailyDataInformation> basicDailyDataToWrite)
        {
            string filepath = _baseFolder + $@"\{ID}_BasicDailyData.txt";
            using (var sw = new StreamWriter(filepath, false, Encoding.Default))
            {
                var toWrite = basicDailyDataToWrite.ToList();
                toWrite.Sort();
                foreach (var r in toWrite)
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }
    }
}
