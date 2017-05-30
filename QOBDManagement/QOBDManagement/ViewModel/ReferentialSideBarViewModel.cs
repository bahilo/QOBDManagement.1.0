using QOBDBusiness;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.ViewModel
{
    public class ReferentialSideBarViewModel : BindBase
    {
        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> UtilitiesCommand { get; set; }
        public ButtonCommand<string> SetupCommand { get; set; }
        private Func<object, object> _page;
        private IMainWindowViewModel _main;


        public ReferentialSideBarViewModel()
        {
            instancesCommand();
        }

        public ReferentialSideBarViewModel(IMainWindowViewModel main): this()
        {
            _main = main;
            _page = _main.navigation;
        }

        //----------------------------[ Initialization ]------------------
        
        private void instancesCommand()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
            SetupCommand = new ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);
        }

        //----------------------------[ Properties ]------------------


        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        //----------------------------[ Actions ]------------------

        public async void executeNavig(string obj)
        {
            switch (obj.ToLower())
            {
                case "credential":
                    _page(new OptionSecurityViewModel());
                    break;
                case "data-display":
                    _page(new OptionDataAndDisplayViewModel());
                    break;
                case "email":
                    _page(new OptionEmailViewModel());
                    break;
                case "setting":
                    _page(new OptionGeneralViewModel());
                    break;
            }
        }
        
        //----------------------------[ Action Commands ]------------------

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "data-display":
                    executeNavig(obj);
                    break;
                case "credential":
                    executeNavig(obj);
                    break;
            }
        }

        private bool canExecuteSetupAction(string arg)
        {
            bool _isUserAdmin = _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity.SendEmail)
                            && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Delete)
                                && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Read)
                                    && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Update)
                                        && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Write);

            if (_isUserAdmin)
                return true;

            return false;
        }

        private bool canExecuteUtilityAction(string arg)
        {
            if (arg.Equals("monitoring"))
                return false;
            return true;
        }

        private void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "monitoring":
                    executeNavig(obj);
                    break;
                case "email":
                    executeNavig(obj);
                    break;
                case "setting":
                    executeNavig(obj);
                    break;
            }
        }
    }
}
