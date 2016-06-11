using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.StockList
{
    public class StockListManager
    {
        readonly string STOCKLIST_FILE_PATH;
        public readonly List<StockInfomation> stockList;
        public StockListManager(string stockListFilePath)
        {
            STOCKLIST_FILE_PATH = stockListFilePath;
        }
        private void loadStockList()
        {

        }
        
    }
}
