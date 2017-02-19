using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public interface ITradeSimWriter
    {
        void setBaseFolder(string folder);
        void write(string toWrite, string fileName, bool append);
    }
}
