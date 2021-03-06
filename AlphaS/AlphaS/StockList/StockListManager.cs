﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AlphaS.DataSetNS;
using System.Collections;

namespace AlphaS.StockList
{
    public class StockListManager : IStockListManager
    {
        List<StockInfomation> stockList= new List<StockInfomation>();

        public IEnumerable<StockInfomation> getStockList()
        {
            return stockList.ToArray();
        }

        public void loadStockList(string StockListFilePath)
        {
            DataSet stockData = new DataSet();
            stockData.LoadData(StockListFilePath);
            stockList = new List<StockInfomation>();
            foreach (var row in stockData.DataRow)
            {
                try
                {
                    stockList.Add(new StockInfomation(row, stockData.index));
                }
                catch { }
            }
        }

    }
}
