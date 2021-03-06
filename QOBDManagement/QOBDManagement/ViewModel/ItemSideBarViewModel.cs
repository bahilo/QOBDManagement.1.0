﻿using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDManagement.Interfaces;
using QOBDCommon.Classes;

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

        public string TxtIconColour
        {
            get { return Utility.getRandomColour(); }
        }

        //----------------------------[ Action Commands ]------------------

        private void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "catalogue":
                    _page(_main.ItemViewModel);
                    break;
                case "provider":
                    _page(new ProviderModel());
                    break;
            }
        }

        private bool canExecuteUtilityAction(string arg)
        {
            if (arg.Equals("catalogue") && _page(null) as ItemViewModel != null)
                return false;

            if (arg.Equals("provider") && _page(null) as ProviderModel != null)
                return false;

            return true;
        }

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "new-item":
                    // resetting the selected item
                    if(_main.ItemViewModel.SelectedItemModel != null)
                    {
                        _main.ItemViewModel.SelectedItemModel.PropertyChanged -= _main.ItemViewModel.ItemDetailViewModel.onItemNameChange_generateReference;
                        if(_main.ItemViewModel.SelectedItemModel.Image != null)
                            _main.ItemViewModel.SelectedItemModel.Image.Dispose();
                    }
                    
                    _main.ItemViewModel.SelectedItemModel = new ItemModel();
                    _main.ItemViewModel.SelectedItemModel.PropertyChanged += _main.ItemViewModel.ItemDetailViewModel.onItemNameChange_generateReference;
                    _page(_main.ItemViewModel.ItemDetailViewModel);
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

            if (arg.Equals("catalogue") && _page(null) as ItemViewModel != null)
                return false;

            return true;
        }  

    }
}
