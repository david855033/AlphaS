using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class FuturePriceDataManager : IFuturePriceDataManager
    {
        public FuturePriceDataManager() { }
        public FuturePriceDataManager(string FUTURE_PRICE_DATA_FOLDER)
        {
            setBaseFolder(FUTURE_PRICE_DATA_FOLDER);
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

        public List<FuturePriceDataInformation> getFuturePriceData(string ID)
        {
            var result = new List<FuturePriceDataInformation>();
            string filepath = _baseFolder + $@"\{ID}_FuturePriceData.txt";
            if (File.Exists(_baseFolder + $@"\{ID}_FuturePriceData.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(new FuturePriceDataInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }
       

        public void saveFuturePriceData(string ID, List<FuturePriceDataInformation> FuturePriceDataToWrite)
        {
            string filepath = _baseFolder + $@"\{ID}_FuturePriceData.txt";
            using (var sw = new StreamWriter(filepath, false, Encoding.Default))
            {
                var toWrite = FuturePriceDataToWrite.ToList();
                toWrite.Sort();
                foreach (var r in toWrite)
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }

    }
}
