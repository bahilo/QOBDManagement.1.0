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
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
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
            set { _startup.Bl = value; onPropertyChange( "Bl"); }
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
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }
            OptionDataAndDisplayViewModel.Dispose();
            OptionEmailViewModel.Dispose();
            OptionGeneralViewModel.Dispose();
            OptionSecurityViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------


        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
                _referentialSideBarViewModel.Startup = Startup;
                _optionSecurityViewModel.Startup = Startup;
                _optionGeneralViewModel.Startup = Startup;
                _optionDataAndDisplayViewModel.Startup = Startup;
                _optionEmailViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                _referentialSideBarViewModel.Dialog = Dialog;
                _optionSecurityViewModel.Dialog = Dialog;
                _optionGeneralViewModel.Dialog = Dialog;
                _optionDataAndDisplayViewModel.Dialog = Dialog;
                _optionEmailViewModel.Dialog = Dialog;
            }
        }

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
