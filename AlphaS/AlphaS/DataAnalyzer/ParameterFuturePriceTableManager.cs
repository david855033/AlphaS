using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class ParameterFuturePriceTableManager : IParameterFuturePriceTableManager
    {
        public ParameterFuturePriceTableManager() { }
        public ParameterFuturePriceTableManager(string PARAMETER_FUTURE_PRICE_TABLE_FOLDER)
        {
            setBaseFolder(PARAMETER_FUTURE_PRICE_TABLE_FOLDER);
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

        public List<ParameterFuturePriceTableInformation> getParameterFuturePriceTable(string parameterName)
        {
            var result = new List<ParameterFuturePriceTableInformation>();
            string filepath = _baseFolder + $@"\{parameterName}_ParameterFuturePriceTable.txt";
            if (File.Exists(_baseFolder + $@"\{parameterName}_ParameterFuturePriceTable.txt"))
            {
                using (var sr = new StreamReader(filepath, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(new ParameterFuturePriceTableInformation(sr.ReadLine()));
                    }
                }
            }
            return result;
        }

        public void saveParameterFuturePriceTable(string parameterName, List<ParameterFuturePriceTableInformation> FuturePriceDataToWrite)
        {
            string filepath = _baseFolder + $@"\{parameterName}_ParameterFuturePriceTable.txt";
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
