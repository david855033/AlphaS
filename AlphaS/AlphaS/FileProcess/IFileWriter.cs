using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.FileProcess
{
    public interface IFileWriter
    {
        void WriteFile(string filepath, string content);
        void WriteFile(string folder, string filename, string content);
    }
}
