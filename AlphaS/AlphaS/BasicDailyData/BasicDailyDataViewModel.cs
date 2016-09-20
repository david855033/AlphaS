﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataViewModel : ViewModelProtoType
    {
        private string _acquiredData;
        public string acquiredData
        {
            get { return _acquiredData; }
            set
            {
                _acquiredData = value;
                OnPropertyChanged(nameof(acquiredData));
            }
        }
        private int _startYear;
        public string startYear
        {
            get { return _startYear.ToString(); }
            set
            {
                var r = value.getIntFromString();
                if (r >= 2000 && r <= 2020)
                {
                    _startYear = r;
                }
                OnPropertyChanged(nameof(startYear));
            }
        }
        private int _startMonth;
        public string startMonth
        {
            get { return _startMonth.ToString(); }
            set
            {
                var r = value.getIntFromString();
                if (r >= 1 && r <= 12)
                {
                    _startMonth = r;
                }
                OnPropertyChanged(nameof(startMonth));
            }
        }
    }
}
