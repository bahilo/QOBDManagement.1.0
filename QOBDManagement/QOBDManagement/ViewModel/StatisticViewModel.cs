using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;

namespace QOBDManagement.ViewModel
{
    public class StatisticViewModel
    {
        private MainWindowViewModel mainWindowViewModel;
        private BusinessLogic _bl;

        public StatisticViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        internal void setLogicAccess(BusinessLogic bl)
        {
            _bl = bl;
        }
    }
}
