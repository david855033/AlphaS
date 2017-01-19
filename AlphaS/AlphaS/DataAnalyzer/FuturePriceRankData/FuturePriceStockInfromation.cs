using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class FuturePriceStockInfromation : IComparable
    {
        public string stockID;
        public decimal?[] futurePrice = new decimal?[FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length];
        public decimal?[] futurePriceRank = new decimal?[FuturePriceDataInformation.FUTURE_PRICE_DAYS.Length];
        public FuturePriceStockInfromation(string stockID, FuturePriceDataInformation CopyFrom)
        {
            this.stockID = stockID;
            futurePrice = CopyFrom.futurePrices;
        }

        public int CompareTo(object obj)
        {
            return this.stockID.CompareTo((obj as FuturePriceStockInfromation).stockID);
        }
    }

    class FuturePriceStockComparer : IComparer<FuturePriceStockInfromation>
    {
        private int selectedFutureDataIndex;
        public FuturePriceStockComparer(int selectedFutureDataIndex)
        {
            this.selectedFutureDataIndex = selectedFutureDataIndex;
        }

        public int Compare(FuturePriceStockInfromation x, FuturePriceStockInfromation y)
        {
            return (x.futurePrice[selectedFutureDataIndex] ?? 0).CompareTo((y.futurePrice[selectedFutureDataIndex] ?? 0));
        }
    }
}
