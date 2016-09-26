using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class DataAnalyzerViewModel : ViewModelProtoType
    {
        private string _console;
        public string console
        {
            get { return _console; }
            set
            {
                _console = value;
                OnPropertyChanged(nameof(console));
            }
        }
    }
}
