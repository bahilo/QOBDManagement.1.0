
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDBusiness;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class CLientSideBarViewModel : BindBase
    {
        private Func<Object, Object> _page;        
        private Cart _cart;


        //----------------------------[ Models ]------------------

        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> CLientSetupCommand { get; set; }
        public ButtonCommand<string> ClientUtilitiesCommand { get; set; }



        public CLientSideBarViewModel() : base()
        {
            instances();
            instancesCommand();
        }

        public CLientSideBarViewModel(IMainWindowViewModel main): this()
        {
            _main = main;
            _page = _main.navigation;
            Cart = _main.ItemViewModel.Cart;
            initEvents();
        }

        //----------------------------[ Initialization ]------------------
        
        private void initEvents()
        {            
        }

        private void instances()
        {
            _cart = new Cart();
        }

        private void instancesCommand()
        {
            CLientSetupCommand = new ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);
            ClientUtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);

        }

        //----------------------------[ Properties ]------------------

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value, "Cart"); }
        }

        public ClientModel SelectedClient
        {
            get { return _main.ClientViewModel.SelectedCLientModel; }
            set { _main.ClientViewModel.SelectedCLientModel = value; onPropertyChange(); }
        }

        //----------------------------[ Actions ]------------------
        
        private object getCurrentPage()
        {
            if (_page != null)
            {
                Object PageViewModel = _page(null) as ClientDetailViewModel;
                if (PageViewModel != null)
                    return PageViewModel;

                PageViewModel = _page(null) as ClientViewModel;
                if (PageViewModel != null)
                    return PageViewModel;
            }
            return null;
        }

        private void updateCommand()
        {
            ClientUtilitiesCommand.raiseCanExecuteActionChanged();
            CLientSetupCommand.raiseCanExecuteActionChanged();
        }

        public override void Dispose()
        {
        }

        //----------------------------[ Event Handler ]------------------

        private void onSelectedCLientChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedClient"))
                updateCommand();
        }

        public void onCurrentPageChange_updateCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentViewModel"))
                updateCommand();
        }

        //----------------------------[ Action Commands ]------------------
        
        private void executeUtilityAction(string obj)
        {
            if (_main != null)
            {
                switch (obj)
                {
                    case "select-quote-client":
                        Cart.Client = SelectedClient;
                        _page(_main.QuoteViewModel);
                        break;
                    case "client-command":
                        _main.OrderViewModel.SelectedClient = SelectedClient;
                        _page(_main.OrderViewModel);
                        break;
                    case "client-quote":
                        _main.QuoteViewModel.SelectedClient = SelectedClient;
                        _page(_main.QuoteViewModel);
                        break;
                    case "client":
                        _page(_main.ClientViewModel);
                        break;
                }
            }
        }

        private bool canExecuteUtilityAction(string arg)
        {
            object currentPage = getCurrentPage();

            if (currentPage == null)
                return false;
            if (currentPage.GetType() == typeof(ClientDetailViewModel)
                && arg.Equals("search-client"))
                return false;
            if ((currentPage.GetType() != typeof(ClientDetailViewModel) || SelectedClient.Client.ID == 0)
                && (arg.Equals("client-command")
                || arg.Equals("client-quote")
                || arg.Equals("select-quote-client")))
                return false;

            return true;
        }

        private void executeSetupAction(string obj)
        {
            ClientDetailViewModel clientDetail = new ClientDetailViewModel();

            switch (obj)
            {
                case "new-client":
                    SelectedClient.Client = new QOBDCommon.Entities.Client();
                    SelectedClient.Address = new QOBDCommon.Entities.Address();
                    SelectedClient.AddressList = new List<QOBDCommon.Entities.Address>();
                    SelectedClient.Contact = new QOBDCommon.Entities.Contact();
                    SelectedClient.ContactList = new List<QOBDCommon.Entities.Contact>();
                    _page(new ClientDetailViewModel());
                    break;
                case "new-address":
                    SelectedClient.Address = new QOBDCommon.Entities.Address();
                    break;
                case "new-contact":
                    SelectedClient.Contact = new QOBDCommon.Entities.Contact();
                    break;
            }
        }

        private bool canExecuteSetupAction(string arg)
        {
            object currentPage = getCurrentPage();

            bool isUpdate = _main.securityCheck(QOBDCommon.Enum.EAction.Client, QOBDCommon.Enum.ESecurity._Update);
            bool isWrite = _main.securityCheck(QOBDCommon.Enum.EAction.Client, QOBDCommon.Enum.ESecurity._Write);
            if ((!isUpdate || !isWrite)
                && (arg.Equals("new-client")
                || arg.Equals("new-contact")
                || arg.Equals("new-address")))
                return false;

            if (currentPage == null)
                return false;
            if ((currentPage.GetType() != typeof(ClientDetailViewModel) || SelectedClient.Client.ID == 0)
                && (arg.Equals("new-contact")
                || arg.Equals("new-address")))
                return false;

            if (SelectedClient.AddressList.Count == 0
                && arg.Equals("new-address"))
                return false;

            if (SelectedClient.ContactList.Count == 0
                && arg.Equals("new-contact"))
                return false;

            return true;
        }

        

        

    }
}
