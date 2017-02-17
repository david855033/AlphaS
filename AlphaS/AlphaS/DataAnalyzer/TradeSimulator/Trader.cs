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
        List<TradeRecord> tradeRecordList = new List<TradeRecord>();
        List<TradeRecord> currentStockPositions = new List<TradeRecord>();
        List<TradeAction> actionHistory = new List<TradeAction>();
        List<TradeAction> actionList = new List<TradeAction>();

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

            makeBuyAction(copyOfNextDayDailyChart);

            makeSellAction(currentDate, copyOfNextDayDailyChart);
        }

        private void checkBuyAction(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            foreach (var action in actionList.FindAll(x => x.action == "buy"))
            {
                var matchStock = copyOfNextDayDailyChart.Find(x => x.stockID == action.ID);
                if (matchStock.N_low <= action.bidPrice)
                {
                    decimal actualBuyPrice = Math.Min(matchStock.N_open, action.bidPrice);
                    currentStockPositions.Add(new TradeRecord(matchStock.stockID, currentDate, actualBuyPrice));
                    action.excuteMessage = $"success buy on {currentDate.ToString("yyyy-MM-dd")} for ${actualBuyPrice.round(2)}";
                }
                else
                {
                    action.excuteMessage = $"fail buy on {currentDate.ToString("yyyy-MM-dd")}";
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
                if (matchStock.N_high >= action.bidPrice)
                {
                    decimal actualSellPrice = Math.Max(matchStock.N_open, action.bidPrice);
                    var matchedTradeRecords = currentStockPositions.FindAll(x => x.ID == action.ID);
                    foreach (var matchedTradeRecord in matchedTradeRecords)
                    {
                        sell(currentDate, actualSellPrice, matchedTradeRecord);
                    }
                    action.excuteMessage = $"success sell on {currentDate.ToString("yyyy-MM-dd")} for ${actualSellPrice.round(2)}";
                    if (matchedTradeRecords.Count > 0)
                        action.excuteMessage += $" ({matchedTradeRecords.Count})";
                }
                else
                {
                    action.excuteMessage = $"fail sell on {currentDate.ToString("yyyy-MM-dd")}";
                }
                actionHistory.Add(action);
                actionList.Remove(action);
            }
        }
        private void sell(DateTime currentDate, decimal actualSellPrice, TradeRecord matchedTradeRecord)
        {
            matchedTradeRecord.sell(currentDate, actualSellPrice);
            tradeRecordList.Add(matchedTradeRecord);
            currentStockPositions.Remove(matchedTradeRecord);
        }


        private void makeBuyAction(List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            if (currentStockPositions.Count <= tradeProtocal.divideParts)
            {
                var stockAboveThreshold = copyOfNextDayDailyChart.FindAll(x => x.weightedScore >= tradeProtocal.buyThreshold);
                if (stockAboveThreshold.Count() >= 0)
                {
                    stockAboveThreshold.Sort((x, y) => { return -x.weightedScore.CompareTo(y.weightedScore); }); //由大至小排列
                    int buyActionNumber = tradeProtocal.divideParts - currentStockPositions.Count;
                    for (int i = 0; i < buyActionNumber && i < stockAboveThreshold.Count; i++)
                    {
                        actionList.Add(new TradeAction()
                        {
                            ID = stockAboveThreshold[i].stockID,
                            action = "buy",
                            bidPrice = stockAboveThreshold[i].N_close * tradeProtocal.buyPriceFromClose,
                            settingMessage = ""//todo
                        });
                    }
                }
            }
        }
        private void makeSellAction(DateTime currentDate, List<DailyChartInformation> copyOfNextDayDailyChart)
        {
            foreach (var stockPositionToSell in currentStockPositions)
            {
                var matchedStockIndex = copyOfNextDayDailyChart.FindIndex(x => x.stockID == stockPositionToSell.ID);

                if (matchedStockIndex >= 0)
                {
                    var matchedStock = copyOfNextDayDailyChart[matchedStockIndex];
                    decimal percentageRank = 1 - matchedStockIndex / (copyOfNextDayDailyChart.Count() - 1);
                    stockPositionToSell.lastAvailableDate = currentDate;
                    stockPositionToSell.lastAvailablePrice = matchedStock.N_low;

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
                        actionList.Add(new TradeAction()
                        {
                            ID = stockPositionToSell.ID,
                            action = "sell",
                            bidPrice = matchedStock.N_close * tradeProtocal.sellPriceFromClose
                        });
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
            stockPositionToSell.sell(stockPositionToSell.lastAvailableDate, stockPositionToSell.lastAvailablePrice);
            tradeRecordList.Add(stockPositionToSell);
            currentStockPositions.Remove(stockPositionToSell);
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


        public void endSimulation(DateTime date)
        {
            foreach (var stockPositionToSell in currentStockPositions)
            {
                forceSell(stockPositionToSell);
            }

        }

        public string getResult() { return ""; }
    }
}
