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
using QOBDCommon.Classes;

namespace QOBDManagement.ViewModel
{
    public class QuoteViewModel: BindBase, IQuoteViewModel
    {
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
        public Command.ButtonCommand<OrderModel> GetQuoteForUpdateCommand { get; set; }


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
            GetQuoteForUpdateCommand = new ButtonCommand<OrderModel>(selectQuoteForUpdate, canSelectQuoteForUpdate);
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
        }        

        public string MissingCLientMessage
        {
            get { return _missingCLientMessage; }
            set { setProperty(ref _missingCLientMessage, value, "MissingCLientMessage"); }
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
            if (Cart.Client.Client.ID == 0 && (Cart.Client = SelectedClient).Client.ID == 0)
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

        private async void updateQuote()
        {
            Dialog.showSearch("Quote updating...");

            List<Entity.Order_item> order_itemList = new List<Entity.Order_item>();
            SelectedQuoteModel.TxtDate = DateTime.Now.ToString();           

            var savedQuoteList = await Bl.BlOrder.UpdateOrderAsync(new List<Entity.Order> { SelectedQuoteModel.Order });
            if (savedQuoteList.Count > 0)
            {
                foreach (Cart_itemModel cart_itemModel in Cart.CartItemList)
                {
                    // update existing item in the cart
                    Order_itemModel order_itemModelFound = QuoteDetailViewModel.Order_ItemModelList.Where(x=>x.TxtItem_ref == cart_itemModel.TxtRef).FirstOrDefault();
                    if(order_itemModelFound != null)
                    {
                        order_itemModelFound.TxtQuantity = cart_itemModel.TxtQuantity;
                        order_itemModelFound.TxtPrice = cart_itemModel.TxtPrice_sell;
                        order_itemModelFound.TxtPrice_purchase = cart_itemModel.TxtPrice_purchase;
                        order_itemList.Add(order_itemModelFound.Order_Item);
                    }

                    // new item in the cart
                    else
                    {
                        Order_itemModel newOrder_itemModel = new Order_itemModel();
                        newOrder_itemModel.TxtItem_ref = cart_itemModel.TxtRef;
                        newOrder_itemModel.TxtItemId = cart_itemModel.TxtID;
                        newOrder_itemModel.TxtPrice = cart_itemModel.TxtPrice_sell;
                        newOrder_itemModel.TxtPrice_purchase = cart_itemModel.TxtPrice_purchase;
                        newOrder_itemModel.Order_Item.OrderId = savedQuoteList[0].ID;
                        newOrder_itemModel.TxtQuantity = cart_itemModel.TxtQuantity;
                        order_itemList.Add(newOrder_itemModel.Order_Item);
                    }
                }

                // get unselected item from the list for deletion
                var order_itemListToDelete = QuoteDetailViewModel.Order_ItemModelList.Where(x=> Cart.CartItemList.Where(y=>y.TxtRef == x.TxtItem_ref).Count() == 0 ).ToList();
                
                await Bl.BlOrder.UpdateOrder_itemAsync(order_itemList.Where(x=>x.ID != 0).ToList());
                await Bl.BlOrder.InsertOrder_itemAsync(order_itemList.Where(x => x.ID == 0).ToList());
                await Bl.BlOrder.DeleteOrder_itemAsync(order_itemListToDelete.Select(x=>x.Order_Item).ToList());

                foreach (var order_itemModelToDelete in order_itemListToDelete)
                    QuoteDetailViewModel.Order_ItemModelList.Remove(order_itemModelToDelete);

                Cart.CartItemList.Clear();
                Cart.Client.Client = new QOBDCommon.Entities.Client();
                if (savedQuoteList.Count > 0)
                    await Dialog.showAsync("Quote ID(" + new OrderModel { Order = savedQuoteList[0] }.TxtID + ") has been updated successfully!");
            }
            else
            {
                string errorMessage = "Error occurred while updating the quote ID[" + SelectedQuoteModel.TxtID + "]!";
                Log.error(errorMessage);
                await Dialog.showAsync(errorMessage);
            }               

            Dialog.IsDialogOpen = false;
        }

        private async void createNewQuote()
        {
            Dialog.showSearch("Quote creation...");

            OrderModel quote = new OrderModel();
            quote.AddressList = Cart.Client.AddressList;
            quote.CLientModel = Cart.Client;
            quote.AgentModel = new AgentModel { Agent = Bl.BlSecurity.GetAuthenticatedUser() };
            quote.TxtDate = DateTime.Now.ToString();
            quote.TxtStatus = EOrderStatus.Quote.ToString();
            
            var savedQuoteList = await Bl.BlOrder.InsertOrderAsync(new List<Entity.Order> { quote.Order });
            if(savedQuoteList.Count > 0)
            {
                List<Entity.Order_item> order_itemList = Cart.CartItemList.Select(x => new Entity.Order_item
                {
                    Item_ref = x.TxtRef,
                    ItemId = x.Item.ID,
                    Price = x.Item.Price_sell,
                    Price_purchase = x.Item.Price_purchase,
                    OrderId = savedQuoteList[0].ID,
                    Quantity = Utility.intTryParse(x.TxtQuantity)
                }).ToList();

                var savedOrderList = await Bl.BlOrder.InsertOrder_itemAsync(order_itemList);
                Cart.CartItemList.Clear();
                Cart.Client.Client = new QOBDCommon.Entities.Client();
                if (savedQuoteList.Count > 0)
                    await Dialog.showAsync("Quote ID(" + new OrderModel { Order = savedQuoteList[0] }.TxtID + ") has been created successfully!");
            }
            else
            {
                string errorMessage = "Error occurred while creating the quote!";
                Log.error(errorMessage);
                await Dialog.showAsync(errorMessage);
            }

            Dialog.IsDialogOpen = false;
        }

        public override void Dispose()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }          
            _orderViewModel.PropertyChanged -= onOrderModelChange_loadOrder;
        }

        //----------------------------[ Event Handler ]------------------
        

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

        private void onOrderModelChange_loadOrder(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("OrderModelList"))
            {
                QuoteModelList = _orderViewModel.OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Quote.ToString())).ToList();
                Title = _orderViewModel.Title.Replace("Orders", "Quote");
            }
        }

        //----------------------------[ Action Orders ]------------------

        private void saveSelectedQuote(OrderModel obj)
        {
            SelectedQuoteModel = obj;
            executeNavig("quote-detail");
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
                case "catalog":
                    _page(new ItemViewModel());
                    break;
                default:
                    goto case "quote";
            }
        }

        private void createQuote(string obj)
        {
            if (SelectedQuoteModel != null && SelectedQuoteModel.Order.ID != 0)
                updateQuote();
            else
                createNewQuote();

            executeNavig("quote");
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
            executeNavig("quote");
        }

        private bool canDeleteOrder(OrderModel arg)
        {
            return _orderViewModel.canDeleteOrder(arg);
        }

        private void selectQuoteForUpdate(OrderModel obj)
        {
            SelectedQuoteModel = obj;
            Cart.Client = obj.CLientModel;
            QuoteDetailViewModel.OrderSelected = SelectedQuoteModel;
            QuoteDetailViewModel.loadOrder_items();
            foreach (Cart_itemModel cart_itemModel in QuoteDetailViewModel.Order_ItemModelList.Select(x => new Cart_itemModel { Item = x.ItemModel.Item, TxtQuantity = x.TxtQuantity }).ToList())
            {
                // add item to the cart and create an event on quantity change
                if(Cart.CartItemList.Where(x=>x.Item.ID == cart_itemModel.Item.ID).Count() == 0)
                    Cart.AddItem(cart_itemModel);
            }
            
            executeNavig("catalog");           
        }

        private bool canSelectQuoteForUpdate(OrderModel arg)
        {
            return true;
        }


    }
}
