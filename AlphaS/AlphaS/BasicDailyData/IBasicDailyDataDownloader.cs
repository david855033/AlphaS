using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaS.BasicDailyData
{
    public interface IBasicDailyDataDownloader
    {
        void setMission(List<BasicDailyDataMission> mission);
        int getMissionCount();
        void startMainMissionLoop();
        List<BasicDailyDataInformation> getResult();
        void setViewModel(BasicDailyDataViewModel viewModel);
        void setWebBrowser(System.Windows.Forms.WebBrowser webBrowser);
    }
}
