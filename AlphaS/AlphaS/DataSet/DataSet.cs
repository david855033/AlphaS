using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AlphaS.DataSetNS
{
    public class DataSet
    {
        public Dictionary<string, int> index = new Dictionary<string, int>();
        public List<string[]> DataRow = new List<string[]>();

        public void LoadData(string path, char splitter = '\t')
        {
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path, Encoding.Default))
                {
                    string[] titles = sr.ReadLine().Split(splitter);
                    index.Clear();
                    for (int i = 0; i < titles.Length; i++)
                    {
                        index.Add(titles[i], i);
                    }
                    DataRow.Clear();
                    while (!sr.EndOfStream)
                    {
                        DataRow.Add(sr.ReadLine().Split(splitter));
                    }
                }
            }
        }
    }

}
