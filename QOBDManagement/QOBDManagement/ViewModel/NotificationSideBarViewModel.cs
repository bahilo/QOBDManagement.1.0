using QOBDManagement.Classes;
using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.ViewModel
{
    public class NotificationSideBarViewModel : BindBase
    {
        public ButtonCommand<string> UtilitiesCommand;

        public NotificationSideBarViewModel()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }

        private bool canExecuteUtilityAction(string arg)
        {
            return true;
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
