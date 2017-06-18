using QOBDCommon.Classes;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.ViewModel
{
    public class NotificationSideBarViewModel : BindBase
    {
        private Func<Object, Object> _page;
        private IMainWindowViewModel _main;
        public ButtonCommand<string> UtilitiesCommand;

        public NotificationSideBarViewModel()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }

        public NotificationSideBarViewModel(IMainWindowViewModel mainWindowViewModel) : this()
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
        }

        public string TxtIconColour
        {
            get { return Utility.getRandomColour(); }
        }

        private bool canExecuteUtilityAction(string arg)
        {
            bool isUserAdmin = _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity.SendEmail)
                            && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Delete)
                                && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Read)
                                    && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Update)
                                        && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Write);
            if(isUserAdmin)
                return true;

            return false;
        }

        private async void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "email-unpaid":
                    await Dialog.showAsync("TO DO: Send email for unpaid bill");
                    break;
            }
        }
    }
}
