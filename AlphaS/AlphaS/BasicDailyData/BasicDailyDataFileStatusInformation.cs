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
        public FileStatus fileStatus;
        public string modifiedTime;

        public int CompareTo(object obj)
        {
            var that = (BasicDailyDataFileStatusInformation)obj;
            if (this.year.CompareTo(that.year) != 0)
            {
                return this.year.CompareTo(that.year);
            }
            else
            {
                return this.month.CompareTo(that.month);
            }
        }

        public BasicDailyDataFileStatusInformation(string loadString)
        {
            var splitline = loadString.Split('\t');
            year = splitline[0];
            month = splitline[1];
            fileStatus = (FileStatus)Enum.Parse(typeof(FileStatus), splitline[2]);
            modifiedTime = splitline[3];
        }
        public override string ToString()
        {
            return this.year + "\t" + this.month + "\t" + this.fileStatus + "\t" + this.modifiedTime;
        }
        static public string ToTitle()
        {
            return "year\tmonth\tstatus\ttime";
        }
    }
    public enum FileStatus { Valid, Null, Temp }
}

