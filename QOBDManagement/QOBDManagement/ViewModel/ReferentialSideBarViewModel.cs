﻿using QOBDBusiness;
using QOBDCommon.Classes;
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

        public string TxtIconColour
        {
            get { return Utility.getRandomColour(); }
        }

        //----------------------------[ Actions ]------------------

        public void executeNavig(string obj)
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
            bool isUserAdmin = _main.AgentViewModel.IsAuthenticatedAgentAdmin;

            if (isUserAdmin && arg.Equals("credential") && _page(null) as OptionSecurityViewModel == null)
                return true;

            if (isUserAdmin && arg.Equals("data-display") && _page(null) as OptionDataAndDisplayViewModel == null)
                return true;

            return false;
        }

        private bool canExecuteUtilityAction(string arg)
        {
            bool canRead = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Read);

            if (arg.Equals("setting") && _page(null) as OptionGeneralViewModel != null)
                return false;

            if (!canRead && (arg.Equals("email") || arg.Equals("setting")))
                return false;

            if (arg.Equals("setting") && _page(null) as OptionGeneralViewModel != null)
                return false;

            if (arg.Equals("email") && _page(null) as OptionEmailViewModel != null)
                return false;

            return true;
        }

        private void executeUtilityAction(string obj)
        {
            switch (obj)
            {
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
