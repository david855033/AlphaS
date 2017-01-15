using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer.ParameterCalculators
{
    static class paramterIndexInitializer
    {
        public static void initialize()
        {
            addParameter("Change");
            addParameter("ChangeAvg");

            foreach (int day in new int[] { 5, 20, 40, 60, 120 })
            {
                addParameter($"MA{day}");
                addParameter($"BA{day}");
                foreach (int day_base in new int[] { 5, 20, 40, 60, 120 })
                {
                    if (day_base > day)
                    {
                        addParameter($"BA{day}_{day_base}");
                    }
                }
            }

            foreach (int day in new int[] { 5, 20, 60, 120 })
            {
                addParameter($"VMA{day}");
                addParameter($"VBA{day}");
                foreach (int day_base in new int[] { 5, 20, 60, 120 })
                {
                    if (day_base > day)
                    {
                        addParameter($"VBA{day}_{day_base}");
                    }
                }
            }

            foreach (int day in new int[] { 5, 20, 40, 60, 120 })
            {
                addParameter($"VolumeSum{day}");
                addParameter($"DealedStockSum{day}");
                addParameter($"MeanCost{day}");
                addParameter($"PriceCostBias{day}");
                addParameter($"AverageCostBias{day}");
            }

            foreach (int day in new int[] { 5, 20, 40, 60 })
            {
                addParameter($"MeanVolumePerOrder{day}");
                addParameter($"BiasVolumePerOrder{day}");
                foreach (int day_base in new int[] { 5, 20, 40, 60 })
                {
                    if (day_base > day)
                    {
                        addParameter($"BiasVolumePerOrder{day}_{day_base}");
                    }
                }
            }

            foreach (int day in new int[] { 5, 20, 40, 60 })
            {
                addParameter($"RSV{day}");
                addParameter($"K{day}");
                addParameter($"D{day}");
                addParameter($"J{day}");
            }

            foreach (int day in new int[] { 5, 20, 40, 60 })
            {
                addParameter($"RSI{day}");
                foreach (int day_base in new int[] { 5, 20, 40, 60 })
                {
                    if (day_base > day)
                    {
                        addParameter($"RSI{day}_{day_base}");
                    }
                }
            }

            using (var sw = new StreamWriter(CoreNS.Core.DEFAULT_FOLDER + @"\parameters.txt"))
            {
                foreach (var p in AnalyzedDataInformation.parameterIndex.Keys)
                {
                    sw.Write(p + "\t");
                }
            }
        }

        static void addParameter(string parameterName)
        {
            AnalyzedDataInformation.parameterIndex.Add(parameterName, AnalyzedDataInformation.parameterIndex.Count);
        }
    }

    class ChangeCalculator : BaseParameterCalculator
    {
        public ChangeCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }

        void setInitialValues()
        {
            MAIN_PARAMETER = "Change";
        }
        public override void generateParameter()
        {
            int parameterIndexOfChange = AnalyzedDataInformation.parameterIndex["Change"];
            int parameterIndexOfChangeAvg = AnalyzedDataInformation.parameterIndex["ChangeAvg"];
            addDisplay("- analyze: Change/ChangeAvg");
            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                AnalyzedData[i].parameters[parameterIndexOfChange] =
                   ((AnalyzedData[i].N_close - AnalyzedData[i - 1].N_close) /
                    AnalyzedData[i - 1].N_close * 100).round(2);

                AnalyzedData[i].parameters[parameterIndexOfChangeAvg] =
                    ((AnalyzedData[i].N_avg - AnalyzedData[i - 1].N_avg) /
                    AnalyzedData[i - 1].N_avg * 100).round(2);
            }
        }
    }
    class BiasFromMeanAverageCalculator : BaseParameterCalculator
    {
        public BiasFromMeanAverageCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }

        void setInitialValues()
        {
            MAIN_PARAMETER = "MA5";
        }
        public override void generateParameter()
        {
            int[] BAs = { 5, 20, 40, 60, 120 };

            string s = "- analyze: MA&BA";
            foreach (var averageDay in BAs)
            {
                int parameterIndexMA = AnalyzedDataInformation.parameterIndex[$"MA{averageDay}"];
                int parameterIndexBA = AnalyzedDataInformation.parameterIndex[$"BA{averageDay}"];
                s += averageDay + "/";
                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal divider = Convert.ToDecimal(averageDay);
                    decimal MA;
                    if (i == PRE_DATA)
                    {
                        decimal sum = 0;
                        for (int j = PRE_DATA; j > PRE_DATA - averageDay; j--)
                            sum += AnalyzedData[j].N_avg;
                        MA = sum / averageDay;
                    }
                    else {
                        decimal lastMA = AnalyzedData[i - 1].parameters[parameterIndexMA].Value;
                        decimal newN_avg = AnalyzedData[i].N_avg;
                        decimal oldN_avg = AnalyzedData[Math.Max(i - averageDay, 0)].N_avg;
                        MA = (lastMA + (newN_avg - oldN_avg) / divider);
                    }


                    AnalyzedData[i].parameters[parameterIndexMA] = MA;

                    AnalyzedData[i].parameters[parameterIndexBA] =
                        ((AnalyzedData[i].N_avg - MA) / MA * 100).round(2);
                }
            }
            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                for (int x = 0; x < BAs.Length - 1; x++)
                {
                    for (int y = x + 1; y < BAs.Length; y++)
                    {
                        int day1 = BAs[x];
                        int day2 = BAs[y];
                        int parameterIndexBA = AnalyzedDataInformation.parameterIndex[$"BA{day1}_{day2}"];
                        int parameterIndexMA1 = AnalyzedDataInformation.parameterIndex[$"MA{day1}"];
                        int parameterIndexMA2 = AnalyzedDataInformation.parameterIndex[$"MA{day2}"];

                        AnalyzedData[i].parameters[parameterIndexBA] =
                            (AnalyzedData[i].parameters[parameterIndexMA1] /
                            AnalyzedData[i].parameters[parameterIndexMA2] * 100 - 100).round(2);
                    }
                }
            }
            addDisplay(s.TrimEnd('/'));
        }
    }

    class AverageVolumeCalculator : BaseParameterCalculator
    {
        public AverageVolumeCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }
        void setInitialValues()
        {
            MAIN_PARAMETER = "VMA5";
        }

        public override void generateParameter()
        {
            int[] VBAs = { 5, 20, 60, 120 };

            string s = "- analyze: VMA&VBA";
            foreach (var averageDay in VBAs)
            {
                int parameterIndexVMA = AnalyzedDataInformation.parameterIndex[$"VMA{averageDay}"];
                int parameterIndexVBA = AnalyzedDataInformation.parameterIndex[$"VBA{averageDay}"];
                s += averageDay + "/";
                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal divider = Convert.ToDecimal(averageDay);
                    decimal VMA;
                    if (i == PRE_DATA)
                    {
                        decimal sum = 0;
                        for (int j = PRE_DATA; j > PRE_DATA - averageDay; j--)
                            sum += AnalyzedData[j].volume;
                        VMA = sum / averageDay;
                    }
                    else {
                        decimal lastVMA = AnalyzedData[i - 1].parameters[parameterIndexVMA].Value;
                        decimal newVol = AnalyzedData[i].volume;
                        decimal oldVol = AnalyzedData[Math.Max(i - averageDay, 0)].volume;
                        VMA = (lastVMA + (newVol - oldVol) / divider);
                    }


                    AnalyzedData[i].parameters[parameterIndexVMA] = VMA;

                    AnalyzedData[i].parameters[parameterIndexVBA] =
                        ((AnalyzedData[i].volume - VMA) / VMA * 100).round(2);
                }
            }

            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                for (int x = 0; x < VBAs.Length - 1; x++)
                {
                    for (int y = x + 1; y < VBAs.Length; y++)
                    {
                        int day1 = VBAs[x];
                        int day2 = VBAs[y];
                        int parameterIndexVBA = AnalyzedDataInformation.parameterIndex[$"VBA{day1}_{day2}"];
                        int parameterIndexVMA1 = AnalyzedDataInformation.parameterIndex[$"VMA{day1}"];
                        int parameterIndexVMA2 = AnalyzedDataInformation.parameterIndex[$"VMA{day2}"];

                        AnalyzedData[i].parameters[parameterIndexVBA] =
                            (AnalyzedData[i].parameters[parameterIndexVMA1] /
                            AnalyzedData[i].parameters[parameterIndexVMA2] * 100 - 100).round(2);
                    }
                }
            }
            addDisplay(s.TrimEnd('/'));
        }
    }

    class AverageCostCalculator : BaseParameterCalculator
    {
        public AverageCostCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }
        void setInitialValues()
        {
            MAIN_PARAMETER = "VolumeSum5";
        }
        public override void generateParameter()
        {
            int[] averageDays = { 5, 20, 40, 60, 120 };

            string s = "- analyze: Cost Bias";
            foreach (var averageDay in averageDays)
            {
                int parameterIndexVolumeSum = AnalyzedDataInformation.parameterIndex[$"VolumeSum{averageDay}"];
                int parameterIndexDealedStockSum = AnalyzedDataInformation.parameterIndex[$"DealedStockSum{averageDay}"];
                int parameterIndexMeanCost = AnalyzedDataInformation.parameterIndex[$"MeanCost{averageDay}"];
                int parameterIndexPriceCostBias = AnalyzedDataInformation.parameterIndex[$"PriceCostBias{averageDay}"];
                int parameterIndexAverageCostBias = AnalyzedDataInformation.parameterIndex[$"AverageCostBias{averageDay}"];
                int parameterIndexMA = AnalyzedDataInformation.parameterIndex[$"MA{averageDay}"];
                s += averageDay + "/";
                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal volumeSum = 0;
                    decimal dealedStockSum = 0;
                    if (i == PRE_DATA)
                    {

                        for (int j = PRE_DATA; j > PRE_DATA - averageDay; j--)
                        {
                            volumeSum += AnalyzedData[j].volume * AnalyzedData[j].divideWeight.getDecimalFromDouble();
                            dealedStockSum += AnalyzedData[j].dealedStock;
                        }
                        AnalyzedData[i].parameters[parameterIndexVolumeSum] = volumeSum;
                        AnalyzedData[i].parameters[parameterIndexDealedStockSum] = dealedStockSum;
                    }
                    else {
                        decimal lastVolumeSum = AnalyzedData[i - 1].parameters[parameterIndexVolumeSum].Value;
                        decimal newVolume = AnalyzedData[i].volume * AnalyzedData[i].divideWeight.getDecimalFromDouble();
                        decimal oldVolume = AnalyzedData[Math.Max(i - averageDay, 0)].volume *
                                            AnalyzedData[Math.Max(i - averageDay, 0)].divideWeight.getDecimalFromDouble();
                        volumeSum = lastVolumeSum + newVolume - oldVolume;

                        decimal lastDealedStockSum = AnalyzedData[i - 1].parameters[parameterIndexDealedStockSum].Value;
                        decimal newDealedStock = AnalyzedData[i].dealedStock;
                        decimal oldDealedStock = AnalyzedData[Math.Max(i - averageDay, 0)].dealedStock;
                        dealedStockSum = lastDealedStockSum + newDealedStock - oldDealedStock;
                    }
                    decimal MeanCost = dealedStockSum == 0 ? 0 : (volumeSum / dealedStockSum);
                    AnalyzedData[i].parameters[parameterIndexVolumeSum] = volumeSum;
                    AnalyzedData[i].parameters[parameterIndexDealedStockSum] = dealedStockSum;
                    AnalyzedData[i].parameters[parameterIndexMeanCost] = MeanCost;

                    AnalyzedData[i].parameters[parameterIndexPriceCostBias] = MeanCost == 0 ? 0 :
                        (((AnalyzedData[i].N_avg - MeanCost) / MeanCost * 100).round(2));

                    decimal MA = AnalyzedData[i].parameters[parameterIndexMA].Value;

                    AnalyzedData[i].parameters[parameterIndexAverageCostBias] = MeanCost == 0 ? 0 :
                        (((MA - MeanCost) / MeanCost * 100).round(2));
                }
            }

            addDisplay(s.TrimEnd('/'));
        }
    }

    class VolumePerOrderCalculator : BaseParameterCalculator
    {
        public VolumePerOrderCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }
        void setInitialValues()
        {
            MAIN_PARAMETER = "MeanVolumePerOrder5";
        }
        public override void generateParameter()
        {
            int[] days = { 5, 20, 40, 60 };

            string s = "- analyze: Mean Volume Per Order";
            foreach (var averageDay in days)
            {
                int parameterIndexMA = AnalyzedDataInformation.parameterIndex[$"MeanVolumePerOrder{averageDay}"];
                int parameterIndexBA = AnalyzedDataInformation.parameterIndex[$"BiasVolumePerOrder{averageDay}"];
                s += averageDay + "/";
                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal divider = Convert.ToDecimal(averageDay);
                    decimal MeanVolumePerOrder;
                    if (i == PRE_DATA)
                    {
                        decimal sum = 0;
                        for (int j = PRE_DATA; j > PRE_DATA - averageDay; j--)
                            sum += AnalyzedData[j].volumePerOrder;
                        MeanVolumePerOrder = sum / averageDay;
                    }
                    else {
                        decimal lastMA = AnalyzedData[i - 1].parameters[parameterIndexMA].Value;
                        decimal new_volumePerOrder = AnalyzedData[i].volumePerOrder;
                        decimal old_volumePerOrder = AnalyzedData[Math.Max(i - averageDay, 0)].volumePerOrder;
                        MeanVolumePerOrder = (lastMA + (new_volumePerOrder - old_volumePerOrder) / divider);
                    }


                    AnalyzedData[i].parameters[parameterIndexMA] = MeanVolumePerOrder;

                    AnalyzedData[i].parameters[parameterIndexBA] =
                        ((AnalyzedData[i].volumePerOrder - MeanVolumePerOrder) / MeanVolumePerOrder * 100).round(2);
                }
            }
            for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
            {
                for (int x = 0; x < days.Length - 1; x++)
                {
                    for (int y = x + 1; y < days.Length; y++)
                    {
                        int day1 = days[x];
                        int day2 = days[y];
                        int parameterIndexBA = AnalyzedDataInformation.parameterIndex[$"BiasVolumePerOrder{day1}_{day2}"];
                        int parameterIndexMA1 = AnalyzedDataInformation.parameterIndex[$"MeanVolumePerOrder{day1}"];
                        int parameterIndexMA2 = AnalyzedDataInformation.parameterIndex[$"MeanVolumePerOrder{day2}"];

                        AnalyzedData[i].parameters[parameterIndexBA] =
                            (AnalyzedData[i].parameters[parameterIndexMA1] /
                            AnalyzedData[i].parameters[parameterIndexMA2] * 100 - 100).round(2);
                    }
                }
            }
            addDisplay(s.TrimEnd('/'));
        }
    }


    class KDJCalculator : BaseParameterCalculator
    {
        public KDJCalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }
        void setInitialValues()
        {
            MAIN_PARAMETER = "RSV5";
        }
        public override void generateParameter()
        {
            int[] dayToCountList = { 5, 20, 40, 60 };

            string s = "- analyze: KDJ calculator";
            foreach (var dayToCount in dayToCountList)
            {
                int parameterIndexRSV = AnalyzedDataInformation.parameterIndex[$"RSV{dayToCount}"];
                int parameterIndexK = AnalyzedDataInformation.parameterIndex[$"K{dayToCount}"];
                int parameterIndexD = AnalyzedDataInformation.parameterIndex[$"D{dayToCount}"];
                int parameterIndexJ = AnalyzedDataInformation.parameterIndex[$"J{dayToCount}"];

                s += dayToCount + "/";
                decimal lastK = 50, lastD = 50;
                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal highInDays = 0, lowInDays = 0;
                    for (int d = Math.Max(i - dayToCount + 1, 0); d <= i; d++)
                    {
                        highInDays = Math.Max(AnalyzedData[d].N_high, highInDays);
                        if (lowInDays == 0) lowInDays = AnalyzedData[d].N_low;
                        else { lowInDays = Math.Min(AnalyzedData[d].N_low, lowInDays); }
                    }


                    decimal currentRSV = 50;
                    if (lowInDays != highInDays)
                    {
                        currentRSV = ((AnalyzedData[i].N_close - lowInDays) / (highInDays - lowInDays) * 100).round(2);
                    }
                    decimal currentK = ((lastK * 2 + currentRSV) / 3).round(2);
                    decimal currentD = ((lastD * 2 + currentK) / 3).round(2);
                    decimal currentJ = 3 * currentD - 2 * currentK;

                    AnalyzedData[i].parameters[parameterIndexRSV] = currentRSV;
                    AnalyzedData[i].parameters[parameterIndexK] = lastK = currentK;
                    AnalyzedData[i].parameters[parameterIndexD] = lastD = currentD;
                    AnalyzedData[i].parameters[parameterIndexJ] = currentJ;

                }
            }


            addDisplay(s.TrimEnd('/'));
        }
    }

    class RSICalculator : BaseParameterCalculator
    {
        public RSICalculator(List<AnalyzedDataInformation> AnalyzedData, addDisplayDel addDisplay) : base(AnalyzedData, addDisplay) { setInitialValues(); }
        void setInitialValues()
        {
            MAIN_PARAMETER = "RSI5";
        }
        public override void generateParameter()
        {
            int[] days = { 5, 20, 40, 60 };

            string s = "- analyze: RSI";
            foreach (var dayToCount in days)
            {
                int parameterIndexRSI = AnalyzedDataInformation.parameterIndex[$"RSI{dayToCount}"];
                s += dayToCount + "/";

                decimal incresingSum = 0, decreasingSum = 0;
                for (int i = startCalculationIndex - dayToCount; i < startCalculationIndex; i++)
                {
                    decimal N_change = AnalyzedData[i].N_close - AnalyzedData[i - 1].N_close;
                    if (N_change > 0) { incresingSum += N_change; }
                    else { decreasingSum += N_change * -1; }
                }

                for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                {
                    decimal N_change_head = AnalyzedData[i].N_close - AnalyzedData[i - 1].N_close;
                    if (N_change_head > 0) { incresingSum += N_change_head; }
                    else { decreasingSum += N_change_head * -1; }

                    decimal N_change_tail = AnalyzedData[i - dayToCount].N_close - AnalyzedData[i - 1 - dayToCount].N_close;
                    if (N_change_tail > 0) { incresingSum -= N_change_tail; }
                    else { decreasingSum -= N_change_tail * -1; }

                    decimal currentRSI = 50;
                    if (incresingSum + decreasingSum != 0)
                    {
                        currentRSI = (incresingSum / (incresingSum + decreasingSum) * 100).round(2);
                    }

                    AnalyzedData[i].parameters[parameterIndexRSI] = currentRSI;
                }
            }

            foreach (var dayToCount in days)
            {
                foreach (int day_base in new int[] { 5, 20, 40, 60 })
                {
                    int parameterIndexRSI = AnalyzedDataInformation.parameterIndex[$"RSI{dayToCount}"];
                    if (day_base > dayToCount)
                    {
                        int parameterIndexRSI_Cross;
                        parameterIndexRSI_Cross = AnalyzedDataInformation.parameterIndex[$"RSI{dayToCount}_{day_base}"];
                        int parameterIndexRSI_Base = AnalyzedDataInformation.parameterIndex[$"RSI{day_base}"]; ;
                        for (int i = startCalculationIndex; i < existAnalyzeDataCount; i++)
                        {
                            AnalyzedData[i].parameters[parameterIndexRSI_Cross] =
                                AnalyzedData[i].parameters[parameterIndexRSI] -
                                AnalyzedData[i].parameters[parameterIndexRSI_Base];
                        }
                    }
                }
            }

            addDisplay(s.TrimEnd('/'));
        }
    }



}