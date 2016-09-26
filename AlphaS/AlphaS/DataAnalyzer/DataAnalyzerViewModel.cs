using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.DataAnalyzer
{
    public class DataAnalyzerViewModel : ViewModelProtoType
    {
        private string _display;
        public string display
        {
            get { return _display; }
            set
            {
                _display = value;
                OnPropertyChanged(nameof(display));
            }
        }
    }
}
