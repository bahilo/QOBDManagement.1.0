using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class StatisticViewModel
    {
        private IConfirmationViewModel dialog;
        private MainWindowViewModel mainWindowViewModel;
        private IStartup startup;

        public StatisticViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public StatisticViewModel(MainWindowViewModel mainWindowViewModel, IStartup startup, IConfirmationViewModel dialog) : this(mainWindowViewModel)
        {
            this.startup = startup;
            this.dialog = dialog;
        }
    }
}
