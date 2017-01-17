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
                foreach (var r in FuturePriceDataToWrite) //不做sort*
                {
                    sw.WriteLine(r.ToString());
                }
            }
        }

        public void appendParameterFuturePrice(string parameterName, ParameterFuturePriceTableInformation dataToAppend)
        {
            string filepath = _baseFolder + $@"\{parameterName}_ParameterFuturePriceTable.txt";
            using (var sw = new StreamWriter(filepath, true, Encoding.Default))
            {
                sw.WriteLine(dataToAppend.ToString());
            }
        }

        public void appendParameterFuturePrice(string parameterName, List<ParameterFuturePriceTableInformation> dataToAppend)
        {
            string filepath = _baseFolder + $@"\{parameterName}_ParameterFuturePriceTable.txt";
            using (var sw = new StreamWriter(filepath, true, Encoding.Default))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var r in dataToAppend)
                {
                    sb.AppendLine(r.ToString());
                }
                sw.WriteLine(sb.ToString());
            }
        }
    }
}
