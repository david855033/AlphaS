using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AlphaS.DataSetNS;

namespace AlphaS.StockList
{
    public class StockListManager
    {
        public List<StockInfomation> stockList = new List<StockInfomation>();

        public void loadStockList(string StockListFilePath)
        {
            DataSet stockData = new DataSet();
            stockData.LoadData(StockListFilePath);
            foreach (var row in stockData.DataRow)
            {
                stockList.Add(new StockInfomation(row, stockData.index));
            }
        }

    }
}
