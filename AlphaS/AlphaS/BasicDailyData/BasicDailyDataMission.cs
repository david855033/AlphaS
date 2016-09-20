﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataMission
    {
        public string ID;
        public string type;
        public int year;
        public int month;
        public override string ToString()
        {
            return $"Mission: ID:{ID}, type:{type}, year:{year}, month:{month}";
        }
    }
}
