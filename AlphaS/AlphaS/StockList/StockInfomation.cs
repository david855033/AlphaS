using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.StockList
{
    public class StockInfomation
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public StockInfomation(string[] row, Dictionary<string, int> index)
        {
            this.ID = row[index["ID"]];
            this.name = row[index["name"]];
            this.type = row[index["type"]];
        }
    }
}
