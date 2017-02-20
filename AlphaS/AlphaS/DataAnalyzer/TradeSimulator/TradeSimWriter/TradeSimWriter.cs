using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class TradeSimWriter : ITradeSimWriter
    {
        private string _baseFolder;
        public void setBaseFolder(string folder)
        {
            _baseFolder = folder;
        }
        public TradeSimWriter(string basefolder) { setBaseFolder(basefolder); }

        public void write(string toWrite, string fileName = "", bool append = false)
        {
            var filename = fileName == "" ? "" : (fileName + "_") + (append ? "" : DateTime.Now.ToString("yyyyMMdd_HHmm")) + ".txt";
            if (!Directory.Exists(_baseFolder)) Directory.CreateDirectory(_baseFolder);
            var filepath = _baseFolder + "\\" + filename;
            using (var sw = new StreamWriter(filepath, append))
            {
                sw.Write(toWrite);
            }
        }
    }
}
