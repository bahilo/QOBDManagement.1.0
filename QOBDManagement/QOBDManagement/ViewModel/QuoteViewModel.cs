using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using Entity = QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Models;
using QOBDCommon.Enum;
using System.ComponentModel;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class QuoteViewModel: BindBase, IQuoteViewModel
    {
        private string _navigTo;
        private bool _isCurrentPage;
        private Func<Object, Object> _page;
        private string _missingCLientMessage;
        private Cart _cart;
        private string _title;
        private string _defaultClientMissingMessage;

        //----------------------------[ Models ]------------------

        private OrderModel _orderModel;
        private OrderViewModel _orderViewModel;
        private OrderDetailViewModel _quoteDetailViewModel;
        private List<OrderModel> _quoteModelList;
        private ClientModel _selectedClient;
        private ItemModel _itemModel;
        private IMainWindowViewModel _main;


        //----------------------------[ Commands ]------------------

        public Command.ButtonCommand<string> NavigCommand { get; set; }
        public Command.ButtonCommand<OrderModel> GetCurrentCommandCommand { get; set; }
        public Command.ButtonCommand<string> ValidCartToQuoteCommand { get; set; }
        public Command.ButtonCommand<OrderModel> DeleteCommand { get; set; }


        public QuoteViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
        }

        public QuoteViewModel(MainWindowViewModel mainWindowViewModel): this()
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
            Cart = _main.Cart;
            ItemModel = _main.ItemViewModel.ItemModel;
            initEvents();
        }


        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            _quoteDetailViewModel.PropertyChanged += onSelectedQuoteModelChange;
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }

        }

        private void instances()
        {
            _defaultClientMissingMessage = @"/!\ No Client Selected";
        }

        private void instancesModel()
        {
            _orderModel = new OrderModel();
            _orderViewModel = new OrderViewModel();
            _quoteDetailViewModel = new OrderDetailViewModel();
            _selectedClient = new ClientModel();
        }

        private void instancesCommand()
        {
            NavigCommand = new Command.ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentCommandCommand = new Command.ButtonCommand<OrderModel>(saveSelectedQuote, canSaveSelectedOrder);
            ValidCartToQuoteCommand = new ButtonCommand<string>(createQuote, canCreateQuote);
            DeleteCommand = new Command.ButtonCommand<OrderModel>(deleteOrder, canDeleteOrder);
        }

        //----------------------------[ Properties ]------------------
               
        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value, "ItemModel"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public bool IsCurrentPage
        {
            get { return _isCurrentPage; }
            set { _isCurrentPage = false; setProperty(ref _isCurrentPage, value, "IsCurrentPage"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }        

        public string MissingCLientMessage
        {
            get { return _missingCLientMessage; }
            set { setProperty(ref _missingCLientMessage, value, "MissingCLientMessage"); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value,"NavigTo"); }
        }

        public OrderModel SelectedQuoteModel
        {
            get { return QuoteDetailViewModel.OrderSelected; }
            set { QuoteDetailViewModel.OrderSelected = value; onPropertyChange("SelectedQuoteModel"); }
        }

        public OrderDetailViewModel QuoteDetailViewModel
        {
            get { return _quoteDetailViewModel; }
            set { _quoteDetailViewModel = value; onPropertyChange("QuoteDetailViewModel"); }
        }

        public List<OrderModel> QuoteModelList
        {
            get { return _quoteModelList; }
            set { setProperty(ref _quoteModelList, value, "QuoteModelList"); }
        }

        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set { setProperty(ref _selectedClient, value, "SelectedClient"); }
        }

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value, "Cart"); }
        }


        //----------------------------[ Actions ]------------------

        public void loadQuotations()
        {
            _orderViewModel = _main.OrderViewModel;

            //-------[ loading initializing the items brand and family list ]
            _main.ItemViewModel.loadItems();

            //-------[ check cart client ]
            if (Cart.Client.Client.ID == 0)
                MissingCLientMessage = _defaultClientMissingMessage;
            else
                MissingCLientMessage = "";

            //-------[ filter on client's quotes ]
            if (SelectedClient.Client.ID != 0)
            {
                _orderViewModel.SelectedClient = SelectedClient;
                SelectedClient = new ClientModel();
            }

            //-------[ retrieve quotes ]
            if (_orderViewModel != null)
            {
                _orderViewModel.PropertyChanged += onOrderModelChange_loadOrder;
                _orderViewModel.loadOrders();                
            }
       }

        private void onOrderModelChange_loadOrder(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("OrderModelList"))
            {
                QuoteModelList = _orderViewModel.OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Quote.ToString())).ToList();
                Title = _orderViewModel.Title.Replace("Orders", "Quote");
            }
        }

        private List<OrderModel> QuoteListToModelViewList(List<Entity.Order> OrderList)
        {
            List<OrderModel> output = new List<OrderModel>();
            foreach (Entity.Order order in OrderList)
            {
                OrderModel cmdvm = new OrderModel();

                var resultAgent = Bl.BlAgent.GetAgentDataById(order.AgentId);
                cmdvm.AgentModel.Agent = (resultAgent.Count > 0) ? resultAgent[0] : new Entity.Agent();
                var resultClient = Bl.BlClient.GetClientDataById(order.ClientId);
                cmdvm.CLientModel.Client = (resultClient.Count > 0) ? resultClient[0] : new Entity.Client();

                cmdvm.Order = order;

                output.Add(cmdvm);
            }
            return output;
        }

        public override void Dispose()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }
            _quoteDetailViewModel.PropertyChanged -= onSelectedQuoteModelChange;            
            _orderViewModel.PropertyChanged -= onOrderModelChange_loadOrder;
        }

        //----------------------------[ Event Handler ]------------------

        private void onNavigToChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "NavigTo"))
            {
                executeNavig(NavigTo);
            }
        }

        private void onSelectedQuoteModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedQuoteModel"))
            {
                NavigTo = "quote-detail";
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
                QuoteDetailViewModel.Startup = Startup;           
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                QuoteDetailViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Orders ]------------------

        private void saveSelectedQuote(OrderModel obj)
        {
            SelectedQuoteModel = obj;
        }

        private bool canSaveSelectedOrder(OrderModel arg)
        {
            return true;
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "quote":
                    _page(this);
                    break;
                case "quote-detail":
                    _page(QuoteDetailViewModel);
                    break;
                default:
                    goto case "quote";
            }
        }

        private async void createQuote(string obj)
        {
            Dialog.showSearch("Quote creation...");
            OrderModel quote = new OrderModel();
            List<Entity.Order_item> order_itemList = new List<Entity.Order_item>();
            List<Entity.Order> quoteList = new List<Entity.Order>();

            quote.AddressList = Cart.Client.AddressList;
            quote.CLientModel = Cart.Client;
            quote.AgentModel = new AgentModel { Agent = Bl.BlSecurity.GetAuthenticatedUser() };
            quote.TxtDate = DateTime.Now.ToString();
            quote.TxtStatus = EOrderStatus.Quote.ToString();

            quoteList.Add(quote.Order);

            var savedQuoteList = await Bl.BlOrder.InsertOrderAsync(quoteList);
            var savedQuote = (savedQuoteList.Count > 0) ? savedQuoteList[0] : new Entity.Order();

            foreach (var itemModel in Cart.CartItemList)
            {
                var order_item                    = new Order_itemModel();
                order_item.ItemModel.Item         = itemModel.Item;
                order_item.TxtItem_ref           = itemModel.TxtRef;
                order_item.TxtItemId              = itemModel.TxtID;
                order_item.TxtPrice               = itemModel.TxtPrice_sell;
                order_item.TxtPrice_purchase      = itemModel.TxtPrice_purchase;
                order_item.TxtTotalPurchase       = itemModel.TxtTotalPurchasePrice;
                order_item.TxtTotalSelling        = itemModel.TxtTotalSellingPrice;
                order_item.TxtOrderId           = savedQuote.ID.ToString();
                order_item.TxtQuantity            = itemModel.TxtQuantity;

                order_itemList.Add(order_item.Order_Item);
            }    
            var savedOrderList = await Bl.BlOrder.InsertOrder_itemAsync(order_itemList);
            Cart.CartItemList.Clear();
            Cart.Client.Client = new QOBDCommon.Entities.Client();
            if (savedQuoteList.Count > 0)
                await Dialog.show("Quote ID("+new OrderModel { Order = savedQuoteList[0] }.TxtID+") has been created successfully!");
            Dialog.IsDialogOpen = false;
            _page(new QuoteViewModel());
        }

        private bool canCreateQuote(string arg)
        {
            if (Cart.Client != null 
                && Cart.Client.Client.ID != 0
                && Cart.CartItemList.Count() > 0)
            {
                MissingCLientMessage = "";
                return true;                
            }
            else
                MissingCLientMessage = _defaultClientMissingMessage;

            return false;
        }

        private async void deleteOrder(OrderModel obj)
        {
            var order_itemFoundList = Bl.BlOrder.GetOrder_itemByOrderList(new List<Entity.Order> { obj.Order });
            await Bl.BlOrder.DeleteOrder_itemAsync(order_itemFoundList);
            await Bl.BlOrder.DeleteOrderAsync(new List<Entity.Order> { obj.Order });
            _page(this);
        }

        private bool canDeleteOrder(OrderModel arg)
        {
            return _orderViewModel.canDeleteOrder(arg);
        }


    }
}
