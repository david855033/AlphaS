using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public class BasicDailyDataViewModel:ViewModelProtoType
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
    }
}
