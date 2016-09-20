using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.StockList
{
    public interface IStockListManager
    {
        void loadStockList(string StockListFilePath);
        IEnumerable<StockInfomation> getStockList();
    }
}
