using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class ItemSideBarViewModel : BindBase
    {
        
        private Func<object, object> _page;

        //----------------------------[ Models ]------------------

        private ItemModel _selectedItem;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> SetupItemCommand { get; set; }
        public ButtonCommand<string> UtilitiesCommand { get; set; }



        public ItemSideBarViewModel()
        {
            instances();
            instancesCommand();
        }

        public ItemSideBarViewModel(IMainWindowViewModel main) :this()
        {
            this._main = main;
            _page = _main.navigation;
        }

        //----------------------------[ Initialization ]------------------

        private void instances()
        {
            _selectedItem = new ItemModel();
        }

        private void instancesCommand()
        {
            SetupItemCommand = new ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }

        //----------------------------[ Properties ]------------------

        public ItemModel SelectedItem
        {
            get { return _selectedItem; }
            set { setProperty(ref _selectedItem, value, "SelectedItem"); }
        }

        //----------------------------[ Actions ]------------------
          
        
        //----------------------------[ Event Handler ]------------------


        //----------------------------[ Action Commands ]------------------

        private async void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "update-item":
                    await Dialog.show("Update Item");
                    break;
            }
        }

        private bool canExecuteUtilityAction(string arg)
        {
            return false;
        }

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "new-item":
                    SelectedItem.IsRefModifyEnable = true;
                    SelectedItem.Item = new QOBDCommon.Entities.Item();
                    _page(new ItemDetailViewModel());
                    break;
            }
        }

        private bool canExecuteSetupAction(string arg)
        {
            bool isUpdate = _main.securityCheck(QOBDCommon.Enum.EAction.Item, QOBDCommon.Enum.ESecurity._Update);
            bool isWrite = _main.securityCheck(QOBDCommon.Enum.EAction.Item, QOBDCommon.Enum.ESecurity._Write);
            if ((!isUpdate || !isWrite)
                && arg.Equals("new-item"))
                return false;

            return true;
        }  

    }
}
