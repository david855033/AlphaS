using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    class Trader
    {
        TradingProtocal tradeProtocal;
        List<TradeRecord> dealedTradeList = new List<TradeRecord>();
        List<TradeRecord> currentStockPositions = new List<TradeRecord>();
        List<TradeAction> actionHistory = new List<TradeAction>();
        List<TradeAction> actionList = new List<TradeAction>();
        decimal currentMoney = 1;

        public Trader(TradingProtocal tradeProtocal)
        {
            this.tradeProtocal = tradeProtocal;
        }

        public void goNextDay(DateTime currentDate, List<DailyChartInformation> nextDayDailyChart)
        {
            
            var copyOfNextDayDailyChart = nextDayDailyChart.ToList();
            foreach (var StockDaily in copyOfNextDayDailyChart)
            {
                StockDaily.weightedScore = getWeightedScore(StockDaily);
            }
            copyOfNextDayDailyChart.Sort((x, y) => { return -x.weightedScore.CompareTo(y.weightedScore); });

            checkBuyAction(currentDate, copyOfNextDayDailyChart);

            checkSellAction(currentDate, copyOfNextDayDailyChart);

            checkStockPosition(currentDate, copyOfNextDayDailyChart);

            makeBuyAction(currentDate, copyOfNextDayDailyChart);

            makeSellAction(currentDate, copyOfNextDayDailyChart);

            currentMoneyHistory.Add(currentDate, currentMoney);
        }


        private void checkBuyAction(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            int currentStockPositionCount = currentStockPositions.Count();
            foreach (var action in actionList.FindAll(x => x.action == "buy"))
            {
                var matchStock = copyOfNextDayDailyChart.Find(x => x.stockID == action.ID);
                if (matchStock != null && matchStock.N_low <= action.bidPrice)
                {
                    decimal actualBuyPrice = Math.Min(matchStock.N_open, action.bidPrice);
                    var newStockPosition = new TradeRecord(matchStock.stockID, currentDate, actualBuyPrice);

                    decimal desireInMoney = (currentMoney / (tradeProtocal.divideParts - currentStockPositionCount)).round(4);
                    newStockPosition.inMoney = desireInMoney;
                    currentMoney -= desireInMoney;

                    currentStockPositions.Add(newStockPosition);
                    action.excuteMessage = $"success to buy for ${actualBuyPrice.round(2)}";
                }
                else
                {
                    action.excuteMessage = $"fail to buy, N_low is ${matchStock.N_low}";
                }
                actionHistory.Add(action);
                actionList.Remove(action);
            }
        }
        private void checkSellAction(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            foreach (var action in actionList.FindAll(x => x.action == "sell"))
            {
                var matchStock = copyOfNextDayDailyChart.Find(x => x.stockID == action.ID);
                if (matchStock != null && matchStock.N_high >= action.bidPrice)
                {
                    decimal actualSellPrice = Math.Max(matchStock.N_open, action.bidPrice);
                    var matchedTradeRecords = currentStockPositions.FindAll(x => x.ID == action.ID);
                    foreach (var matchedTradeRecord in matchedTradeRecords)
                    {
                        currentMoney += matchedTradeRecord.currentValue;
                        matchedTradeRecord.sell(currentDate, actualSellPrice);
                    }
                    action.excuteMessage = $"success to sell for ${actualSellPrice.round(2)}";
                    if (matchedTradeRecords.Count > 1)
                        action.excuteMessage += $" ({matchedTradeRecords.Count})";
                }
                else
                {
                    action.excuteMessage = $"fail to sell, N_high is {matchStock.N_high}";
                }
                actionHistory.Add(action);
                actionList.Remove(action);
            }
        }


        private void checkStockPosition(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            stockPositionHistory.Add(currentDate, new List<TradeRecord>());
            foreach (var currentStockPosition in currentStockPositions.ToList())
            {
                currentStockPosition.totalTradeDays++;
                var matchedStock = copyOfNextDayDailyChart.Find(x => x.stockID == currentStockPosition.ID);
                if (matchedStock != null)
                {
                    currentStockPosition.lastAvailableDate = currentDate;
                    currentStockPosition.lastAvailablePrice = matchedStock.N_low;
                }

                stockPositionHistory[currentDate].Add(currentStockPosition.getMinor());
                if (currentStockPosition.sellDate != default(DateTime))
                {
                    dealedTradeList.Add(currentStockPosition);
                    currentStockPositions.Remove(currentStockPosition);
                }
            }
        }

        private void makeBuyAction(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            if (currentStockPositions.Count <= tradeProtocal.divideParts)
            {
                var stockAboveThreshold = copyOfNextDayDailyChart.FindAll(x =>
                x.weightedScore >= tradeProtocal.buyThreshold &&
                x.recentMinVolume >= DataAnalyzer.MIN_VOLUME_THRESHOLD);
                if (stockAboveThreshold.Count() >= 0)
                {
                    stockAboveThreshold.Sort((x, y) => { return -x.weightedScore.CompareTo(y.weightedScore); }); //由大至小排列
                    int buyActionNumber = tradeProtocal.divideParts - currentStockPositions.Count;
                    for (int i = 0; i < buyActionNumber && i < stockAboveThreshold.Count; i++)
                    {
                        var newAction = new TradeAction
                            (currentDate, "buy", stockAboveThreshold[i].stockID, stockAboveThreshold[i].N_close * tradeProtocal.buyPriceFromClose);
                        newAction.orderMessage = $"buy order for ${newAction.bidPrice}, score: { stockAboveThreshold[i].weightedScore.round(2)}";
                        actionList.Add(newAction);
                    }
                }
            }
        }

        private void makeSellAction(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            foreach (var stockPositionToSell in currentStockPositions.ToList())
            {

                var matchedStockIndex = copyOfNextDayDailyChart.FindIndex(x => x.stockID == stockPositionToSell.ID);

                if (matchedStockIndex >= 0)
                {
                    var matchedStock = copyOfNextDayDailyChart[matchedStockIndex];
                    decimal percentageRank = 1 - matchedStockIndex / (copyOfNextDayDailyChart.Count() - 1);

                    if (matchedStock.weightedScore < tradeProtocal.sellThreshold)
                    {
                        stockPositionToSell.belowThresholdCount++;
                    }
                    else
                    {
                        stockPositionToSell.belowThresholdCount = 0;
                    }

                    if (stockPositionToSell.belowThresholdCount >= tradeProtocal.sellThresholdDay ||
                         percentageRank <= tradeProtocal.sellRankThreshold)
                    {
                        var newAction = new TradeAction
                            (currentDate, "sell", stockPositionToSell.ID, matchedStock.N_close * tradeProtocal.sellPriceFromClose);

                        newAction.orderMessage = $"sell for ${newAction.bidPrice}, score: {matchedStock.weightedScore.round(2)}, rank:{(percentageRank * 100).round(2).ToString()}%";
                        if (actionList.Find(x => x.action == "sell" && x.ID == newAction.ID) == null)
                            actionList.Add(newAction);
                    }
                }
                else
                {
                    forceSell(stockPositionToSell);
                }
            }
        }


        private void forceSell(TradeRecord stockPositionToSell)
        {
            var price = stockPositionToSell.lastAvailablePrice;
            var date = stockPositionToSell.lastAvailableDate;
            var newAction = new TradeAction(date, "force sell", stockPositionToSell.ID, price);
            newAction.excuteMessage = $"force sell order on {date.ToString("yyyy-MM-dd")} for ${price}";
            actionHistory.Add(newAction);

            currentMoney += stockPositionToSell.currentValue;
            stockPositionToSell.sell(stockPositionToSell.lastAvailableDate, stockPositionToSell.lastAvailablePrice);
        }

        private decimal getWeightedScore(DailyChartInformation StockDaily)
        {
            decimal score;
            decimal sumWeight = 0, sum = 0;
            for (int i = 0; i < tradeProtocal.valueScoreWeight.Length; i++)
            {
                sum += StockDaily.valueScore[i] * tradeProtocal.valueScoreWeight[i];
                sum += StockDaily.rankScore[i] * tradeProtocal.rankScoreWeight[i];
                sumWeight += tradeProtocal.valueScoreWeight[i];
                sumWeight += tradeProtocal.rankScoreWeight[i];
            }
            score = (sum / sumWeight).round(2);
            return score;
        }


        public void endSimulation(DateTime currentDate)
        {
            while (currentStockPositions.Count() > 0)
            {
                forceSell(currentStockPositions.First());
                dealedTradeList.Add(currentStockPositions.First());
                currentStockPositions.Remove(currentStockPositions.First());
            }
            currentMoneyHistory[currentDate] = currentMoney;
        }

        Dictionary<DateTime, List<TradeRecord>> stockPositionHistory = new Dictionary<DateTime, List<TradeRecord>>();
        Dictionary<DateTime, decimal> currentMoneyHistory = new Dictionary<DateTime, decimal>();
        public string getResult()
        {
            var result = new StringBuilder();
            var dateList = (from q in actionHistory
                            select q.date).ToList();
            dateList.AddRange(stockPositionHistory.Keys.ToList());
            dateList = dateList.Distinct().ToList();
            dateList.Sort();


            foreach (var thisDate in dateList)
            {
                var outputTable = new List<string[]>();

                outputTable.Add(new string[] { thisDate.ToString("yyyy-MM-dd") ,$"current Money = {currentMoneyHistory[thisDate]}"});
                var title = new string[] { "ID","Action", "Bid Price", "order", "excute", "",
                    "ID","buyDate","buyPrice","CurrentDate","CurentPrice","sellDate","sellPrice","inMoney","Current Value","profit","TotalDay","TotalTradeDay"};
                outputTable.Add(title);
                int TITLE_COUNT = outputTable.Count();
                var actionHistoryThisDay = actionHistory.FindAll(x => x.date == thisDate);

                for (int i = 0; i < Math.Max(actionHistoryThisDay.Count, stockPositionHistory[thisDate].Count()); i++)
                {
                    outputTable.Add(new string[title.Length]);
                }

                for (int i = 0; i < actionHistoryThisDay.Count(); i++)
                {
                    var thisData = actionHistoryThisDay[i];
                    int j = 0; int k = i + TITLE_COUNT;
                    outputTable[k][j++] = thisData.ID;
                    outputTable[k][j++] = thisData.action;
                    outputTable[k][j++] = thisData.bidPrice.ToString();
                    outputTable[k][j++] = thisData.orderMessage;
                    outputTable[k][j++] = thisData.excuteMessage;
                }

                stockPositionHistory[thisDate].Sort((x, y) => x.ID.CompareTo(y.ID));
                for (int i = 0; i < stockPositionHistory[thisDate].Count(); i++)
                {
                    var thisData = stockPositionHistory[thisDate][i];
                    int j = 6; int k = i + TITLE_COUNT;
                    outputTable[k][j++] = thisData.ID;
                    outputTable[k][j++] = thisData.buyDate.ToString("yyyy-MM-dd");
                    outputTable[k][j++] = thisData.buyPrice.ToString();
                    outputTable[k][j++] = thisData.sellDate != default(DateTime) ? "" : thisData.lastAvailableDate.ToString("yyyy-MM-dd"); ;
                    outputTable[k][j++] = thisData.sellDate != default(DateTime) ? "" : thisData.lastAvailablePrice.ToString();
                    outputTable[k][j++] = thisData.sellDate == default(DateTime) ? "" : thisData.sellDate.ToString("yyyy-MM-dd");
                    outputTable[k][j++] = thisData.sellDate == default(DateTime) ? "" : thisData.sellPrice.ToString();
                    outputTable[k][j++] = thisData.inMoney.ToString();
                    outputTable[k][j++] = thisData.currentValue.ToString();
                    outputTable[k][j++] = thisData.profitPercentage.ToString();
                    outputTable[k][j++] = thisData.totalDays.ToString();
                    outputTable[k][j++] = thisData.totalTradeDays.ToString();
                }
                foreach (var line in outputTable)
                {
                    result.AppendLine(string.Join("\t", line));
                }
            }

            result.AppendLine("");
            result.AppendLine("Dealed Trade");
            {
                var outputTable = new List<string[]>();
                outputTable.Add(new string[] { "ID", "buyDate", "buyPrice", "sellDate", "sellPrice", "TotalDay", "TotalTradeDay", "profit" });

                foreach (var dealedTrade in dealedTradeList)
                {
                    var newLine = new string[10];
                    int j = 0;
                    newLine[j++] = dealedTrade.ID;
                    newLine[j++] = dealedTrade.buyDate.ToString("yyyy-MM-dd");
                    newLine[j++] = dealedTrade.buyPrice.ToString();
                    newLine[j++] = dealedTrade.sellDate == default(DateTime) ? "" : dealedTrade.sellDate.ToString("yyyy-MM-dd");
                    newLine[j++] = dealedTrade.sellDate == default(DateTime) ? "" : dealedTrade.sellPrice.ToString();
                    newLine[j++] = dealedTrade.totalDays.ToString();
                    newLine[j++] = dealedTrade.totalTradeDays.ToString();
                    newLine[j++] = dealedTrade.profitPercentage.ToString();
                    outputTable.Add(newLine);
                }

                foreach (var line in outputTable)
                {
                    result.AppendLine(string.Join("\t", line));
                }
            }
            return result.ToString();
        }
    }
}
