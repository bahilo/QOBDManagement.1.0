using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using QOBDManagement.Classes;
using Entity = QOBDCommon.Entities;
using QOBDCommon.Entities;
using QOBDManagement.Models;
using System.ComponentModel;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class ReferentialViewModel : BindBase, IReferentialViewModel
    {
        private Func<object, object> _page;
        
        //----------------------------[ Models ]------------------

        private ReferentialSideBarViewModel _referentialSideBarViewModel;
        private OptionSecurityViewModel _optionSecurityViewModel;
        private OptionGeneralViewModel _optionGeneralViewModel;
        private OptionDataAndDisplayViewModel _optionDataAndDisplayViewModel;
        private OptionEmailViewModel _optionEmailViewModel;
        private IMainWindowViewModel _main;



        public ReferentialViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this._main = mainWindowViewModel;
            _page = _main.navigation;
            instancesModel(mainWindowViewModel);
        }

        public ReferentialViewModel(MainWindowViewModel mainWindowViewModel, IStartup startup, IConfirmationViewModel dialog) : this(mainWindowViewModel)
        {
            this.Startup = startup;
            this.Dialog = dialog;

            _referentialSideBarViewModel.Dialog = Dialog;
            _optionSecurityViewModel.Dialog = Dialog;
            _optionGeneralViewModel.Dialog = Dialog;
            _optionDataAndDisplayViewModel.Dialog = Dialog;
            _optionEmailViewModel.Dialog = Dialog;

            _referentialSideBarViewModel.Startup = Startup;
            _optionSecurityViewModel.Startup = Startup;
            _optionGeneralViewModel.Startup = Startup;
            _optionDataAndDisplayViewModel.Startup = Startup;
            _optionEmailViewModel.Startup = Startup;
        }

        //----------------------------[ Initialization ]------------------

        private void instancesModel(IMainWindowViewModel main)
        {
            _referentialSideBarViewModel = new ReferentialSideBarViewModel(main);
            _optionSecurityViewModel = new OptionSecurityViewModel(main);
            _optionGeneralViewModel = new OptionGeneralViewModel(main);
            _optionDataAndDisplayViewModel = new OptionDataAndDisplayViewModel(main);
            _optionEmailViewModel = new OptionEmailViewModel(main);
        }

        //----------------------------[ Properties ]------------------
                

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public OptionGeneralViewModel OptionGeneralViewModel
        {
            get { return _optionGeneralViewModel; }
            set { setProperty(ref _optionGeneralViewModel, value, "OptionGeneralViewModel"); }
        }

        public OptionDataAndDisplayViewModel OptionDataAndDisplayViewModel
        {
            get { return _optionDataAndDisplayViewModel; }
            set { setProperty(ref _optionDataAndDisplayViewModel, value, "OptionDataAndDisplayViewModel "); }
        }

        public OptionSecurityViewModel OptionSecurityViewModel
        {
            get { return _optionSecurityViewModel; }
            set { setProperty(ref _optionSecurityViewModel, value, "OptionSecurityViewModel"); }
        }

        public ReferentialSideBarViewModel ReferentialSideBarViewModel
        {
            get { return _referentialSideBarViewModel; }
            set { setProperty(ref _referentialSideBarViewModel, value, "ReferentialSideBarViewModel"); }
        }

        public OptionEmailViewModel OptionEmailViewModel
        {
            get { return _optionEmailViewModel; }
            set { setProperty(ref _optionEmailViewModel, value, "OptionEmailViewModel"); }
        }

        //----------------------------[ Actions ]------------------
        
        public override void Dispose()
        {
            OptionDataAndDisplayViewModel.Dispose();
            OptionEmailViewModel.Dispose();
            OptionGeneralViewModel.Dispose();
            OptionSecurityViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------
        

        //----------------------------[ Action Commands ]------------------
        
        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "option":
                    _page(new OptionGeneralViewModel());
                    break;                
            }
        }
        
    }
}
