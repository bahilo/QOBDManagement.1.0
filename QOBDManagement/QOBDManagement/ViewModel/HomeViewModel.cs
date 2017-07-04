using System;
using System.Collections.Generic;
using System.Linq;
using QOBDBusiness;
using QOBDManagement.Classes;
using LiveCharts.Defaults;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using QOBDCommon.Entities;
using QOBDManagement.Command;
using System.ComponentModel;
using QOBDManagement.Models;
using System.IO;
using System.Xml.Serialization;
using QOBDManagement.Interfaces;
using QOBDCommon.Enum;
using QOBDManagement.Enums;
using QOBDCommon.Classes;
using System.Configuration;

namespace QOBDManagement.ViewModel
{
    public class HomeViewModel : BindBase//, IHomeViewModel
    {
        private IMainWindowViewModel _main;
        private Func<object, object> _page;

        
        public HomeViewModel()
        {
            instances();
            instancesCommand();
        }

        public HomeViewModel(IMainWindowViewModel mainWindowViewModel) : this()
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
            initEvents();
        }

        public HomeViewModel(IMainWindowViewModel mainWindowViewModel, IStartup startup, IConfirmationViewModel dialog) : this(mainWindowViewModel)
        {
            this.Startup = startup;
            this.Dialog = dialog;
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
        }

        private void instances()
        {
           
        }

        private void instancesCommand()
        {
           
        }

        //----------------------------[ Properties ]------------------

        public string TxtMaterialDesignColourName
        {
            get { return Utility.getRandomMaterialDesignColour(); }
        }

        public string TxtColourName
        {
            get { return Utility.getRandomColour(); }
        }

        //----------------------------[ Actions ]------------------

        public void loadData()
        {
            
        }        


    }
}
