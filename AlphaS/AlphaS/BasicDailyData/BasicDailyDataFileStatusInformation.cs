using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataFileStatusInformation : IComparable
    {
        public string year;
        public string month;
        public int dataCount;
        public FileStatus fileStatus;
        public string modifiedTime;

        public int CompareTo(object obj)
        {
            var that = (BasicDailyDataFileStatusInformation)obj;
            if (this.year.getIntFromString().CompareTo(that.year.getIntFromString()) != 0)
            {
                return this.year.getIntFromString().CompareTo(that.year.getIntFromString());
            }
            else
            {
                return this.month.getIntFromString().CompareTo(that.month.getIntFromString());
            }
        }

        public BasicDailyDataFileStatusInformation() { }
        public BasicDailyDataFileStatusInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            if (splitline.Length == 5)
            {
                year = splitline[0];
                month = splitline[1];
                dataCount = splitline[2].getIntFromString();
                fileStatus = (FileStatus)Enum.Parse(typeof(FileStatus), splitline[3]);
                modifiedTime = splitline[4];
            }
        }
        public override string ToString()
        {
            return this.year + "\t" +
                this.month + "\t" +
                this.dataCount + "\t" +
                this.fileStatus + "\t" +
                this.modifiedTime;
        }
        static public string ToTitle()
        {
            return "year\tmonth\tdataCount\tstatus\ttime";
        }
    }
    public enum FileStatus { Valid, Null, Temp }
}

