﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Entity = QOBDCommon.Entities;
using QOBDCommon.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using QOBDManagement.Models;
using QOBDManagement.Classes;
using QOBDCommon.Enum;
using System.Collections.Concurrent;
using QOBDCommon.Classes;
using System.IO;
using System.Xml.Serialization;
using QOBDManagement.Interfaces;
using System.Windows.Threading;
using System.Windows;
using System.Configuration;

namespace QOBDManagement.ViewModel
{
    public class OrderViewModel : BindBase, IOrderViewModel
    {
        private string _navigTo;
        private Func<Object, Object> _page;
        private List<CurrencyModel> _currenciesList;
        public NotifyTaskCompletion<List<Entity.Order>> OrderTask { get; set; }
        public NotifyTaskCompletion<List<OrderModel>> OrderModelTask { get; set; }
        public NotifyTaskCompletion<List<Entity.Tax>> TaxTask { get; set; }
        private string _title;
        private OrderSearchModel _orderSearchModel;
        private string _blockOrderVisibility;
        private string _blockSearchResultVisibility;

        //----------------------------[ POCOs ]------------------

        private Entity.Order _order;
        private List<Entity.Tax> _taxesList;

        //----------------------------[ Models ]------------------

        public OrderSideBarViewModel OrderSideBarViewModel { get; set; }
        private OrderDetailViewModel _orderDetailViewModel;
        private List<OrderModel> _orderModelList;
        private List<OrderModel> _waitValidOrders;
        private List<OrderModel> _waitValidClientOrders;
        private List<OrderModel> _inProcessOrders;
        private List<OrderModel> _waitPayOrders;
        private List<OrderModel> _closedOrders;
        private ClientModel _selectedClient;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public Command.ButtonCommand<string> NavigCommand { get; set; }
        public Command.ButtonCommand<OrderModel> GetCurrentOrderCommand { get; set; }
        public Command.ButtonCommand<OrderModel> DeleteCommand { get; set; }
        public Command.ButtonCommand<object> SearchCommand { get; set; }


        public OrderViewModel()
        {
            instances();
            instancesCommand();
        }

        public OrderViewModel(IMainWindowViewModel mainWindowViewModel) : this()
        {
            this._main = mainWindowViewModel;
            _page = _main.navigation;
            instancesModel(mainWindowViewModel);
            initEvents();
        }

        public OrderViewModel(IMainWindowViewModel mainWindowViewModel, IStartup startup, IConfirmationViewModel dialog) : this(mainWindowViewModel)
        {
            this.Startup = startup;
            this.Dialog = dialog;
            
            OrderDetailViewModel.Dialog = Dialog;
            OrderSideBarViewModel.Dialog = Dialog;

            OrderDetailViewModel.Startup = Startup;
            OrderSideBarViewModel.Startup = Startup;

            OutputStringFormat = "F";
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onBlockSearchResultVisibilityChange;
            TaxTask.PropertyChanged += onTaxTaskCompletion_getTax;

        }

        private void instances()
        {
            _taxesList = new List<QOBDCommon.Entities.Tax>();
            _currenciesList = new List<CurrencyModel>();
            _order = new Entity.Order();
            OrderTask = new NotifyTaskCompletion<List<QOBDCommon.Entities.Order>>();
            OrderModelTask = new NotifyTaskCompletion<List<OrderModel>>();
            TaxTask = new NotifyTaskCompletion<List<QOBDCommon.Entities.Tax>>();
            _title = "";
            _orderSearchModel = new OrderSearchModel();
            _blockOrderVisibility = "Visible";
            _blockSearchResultVisibility = "Hidden";
        }

        private void instancesModel(IMainWindowViewModel main)
        {
            _waitValidOrders = new List<OrderModel>();
            _waitValidClientOrders = new List<OrderModel>();
            _orderModelList = new List<OrderModel>();
            _inProcessOrders = new List<OrderModel>();
            _waitPayOrders = new List<OrderModel>();
            _closedOrders = new List<OrderModel>();
            _orderDetailViewModel = new OrderDetailViewModel(main);
            OrderSideBarViewModel = new OrderSideBarViewModel(main, _orderDetailViewModel);
            _selectedClient = new ClientModel();
        }

        private void instancesCommand()
        {
            NavigCommand = new Command.ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentOrderCommand = new Command.ButtonCommand<OrderModel>(saveSelectedOrder, canSaveSelectedOrder);
            DeleteCommand = new Command.ButtonCommand<OrderModel>(deleteOrder, canDeleteOrder);
            SearchCommand = new Command.ButtonCommand<object>(searchOrder, canSearchOrder);
        }

        //----------------------------[ Properties ]------------------

        public OrderSearchModel OrderSearchModel
        {
            get { return _orderSearchModel; }
            set { setProperty(ref _orderSearchModel, value); }
        }

        public string OutputStringFormat
        {
            get { return _orderDetailViewModel.IntegerOutputStringFormat; }
            set { _orderDetailViewModel.IntegerOutputStringFormat = value; onPropertyChange(); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public ClientModel SelectedClient
        {
            get { return (_selectedClient != null) ? _selectedClient : new ClientModel(); }
            set { setProperty(ref _selectedClient, value); }

        }

        public List<Entity.Tax> TaxList
        {
            get { return _taxesList; }
            set { setProperty(ref _taxesList, value); }
        }

        public List<CurrencyModel> CurrenciesList
        {
            get { return _currenciesList; }
            set { setProperty(ref _currenciesList, value); }
        }

        public OrderDetailViewModel OrderDetailViewModel
        {
            get { return _orderDetailViewModel; }
            set { setProperty(ref _orderDetailViewModel, value); }
        }

        public OrderModel SelectedOrderModel
        {
            get { return OrderDetailViewModel.OrderSelected; }
            set { OrderDetailViewModel.OrderSelected = value; OrderSideBarViewModel.SelectedOrderModel = value; onPropertyChange("SelectedOrderModel"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value, "NavigTo"); }
        }

        public List<OrderModel> WaitValidClientOrderList
        {
            get { return getOrderModelListFilterBy("WaitValidClientOrderList"); }
        }

        public List<OrderModel> OrderModelList
        {
            get { return _orderModelList; }
            set { _orderModelList = value; onPropertyChange("OrderModelList"); updateOrderModelListBinding(); }
        }

        public List<OrderModel> InProcessOrderList
        {
            get { return getOrderModelListFilterBy("InProcessOrderList"); }
        }

        public List<OrderModel> WaitValidOrderList
        {
            get { return getOrderModelListFilterBy("WaitValidOrderList"); }
        }

        public List<OrderModel> ClosedOrderList
        {
            get { return getOrderModelListFilterBy("ClosedOrderList"); }
        }

        public List<OrderModel> WaitPayOrderList
        {
            get { return getOrderModelListFilterBy("WaitPayOrderList"); }
        }

        public CurrencyModel CurrencyModel
        {
            get { return OrderDetailViewModel.CurrencyModel; }
            set { OrderDetailViewModel.CurrencyModel = value; onPropertyChange(); }
        }

        public string BlockSearchResultVisibility
        {
            get { return _blockSearchResultVisibility; }
            set { setProperty(ref _blockSearchResultVisibility, value, "BlockSearchResultVisibility"); }
        }

        public string BlockOrderVisibility
        {
            get { return _blockOrderVisibility; }
            set { setProperty(ref _blockOrderVisibility, value, "BlockOrderVisibility"); }
        }

        //----------------------------[ Actions ]------------------

        /// <summary>
        /// Load all orders in defferent sections according to their status
        /// </summary>
        public void loadOrders()
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    load();
                });
            else
                load();
        }

        private void load()
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["load_message"]);
            TaxList = Bl.BlOrder.GetTaxData(999);
            OrderSearchModel.AgentList = Bl.BlAgent.GetAgentData(999);
            
            if (SelectedClient.Client.ID != 0)
            {
                Title = string.Format("Orders for the Company {0}", SelectedClient.Client.Company);

                OrderModelList = (OrderListToModelList(Bl.BlOrder.searchOrder(new Entity.Order { ClientId = SelectedClient.Client.ID }, ESearchOption.AND))).OrderByDescending(x => x.Order.ID).ToList();
                SelectedClient = new ClientModel();
            }
            else
            {
                Title = "Orders Management";
                OrderModelList = (OrderListToModelList(Bl.BlOrder.searchOrder(new QOBDCommon.Entities.Order { AgentId = Bl.BlSecurity.GetAuthenticatedUser().ID }, ESearchOption.AND))).OrderByDescending(x => x.Order.ID).ToList();
            }
            BlockSearchResultVisibility = "Hidden";
            Dialog.IsDialogOpen = false;
        }


        /// <summary>
        /// Convert order object into a order model list
        /// </summary>
        /// <param name="OrderList"></param>
        /// <returns></returns>
        public List<OrderModel> OrderListToModelList(List<Entity.Order> OrderList)
        {
            List<OrderModel> output = new List<OrderModel>();
            ConcurrentBag<OrderModel> concurrentOrderModelList = new ConcurrentBag<OrderModel>();
            foreach (var order in OrderList)
            {
                OrderModel ovm = new OrderModel();

                // getting the order agent
                var resultAgent = Bl.BlAgent.GetAgentDataById(order.AgentId);
                ovm.AgentModel.Agent = (resultAgent.Count > 0) ? resultAgent[0] : new Entity.Agent();

                // getting the order client
                var resultClient = Bl.BlClient.GetClientDataById(order.ClientId);
                ovm.CLientModel.Client = (resultClient.Count > 0) ? resultClient[0] : new Entity.Client();

                // getting the order tax_order
                var resultSearchOrderTaxList = Bl.BlOrder.searchTax_order(new Tax_order { OrderId = order.ID }, ESearchOption.AND);
                ovm.Tax_orderModel = (resultSearchOrderTaxList.Count > 0) ? new Tax_orderModel { Tax_order = resultSearchOrderTaxList[0] } : new Tax_orderModel();

                // getting the order tax
                Entity.Tax taxFound = TaxList.Where(x => x.ID == ovm.Tax_orderModel.Tax_order.TaxId).OrderBy(x => x.Date_insert).LastOrDefault();// await Bl.BlOrder.GetTaxDataById(cmdvm.Tax_command.TaxId);
                ovm.Tax = (taxFound != null) ? taxFound : new Entity.Tax();

                ovm.Order = order;
                concurrentOrderModelList.Add(ovm);
            }
            output = new List<OrderModel>(concurrentOrderModelList);
            return output;
        }

        private void updateOrderModelListBinding()
        {
            onPropertyChange("WaitValidClientOrderList");
            onPropertyChange("InProcessOrderList");
            onPropertyChange("WaitValidOrderList");
            onPropertyChange("ClosedOrderList");
            onPropertyChange("WaitPayOrderList");
        }

        private List<OrderModel> getOrderModelListFilterBy(string filterName)
        {
            object _lock = new object();
            ConcurrentBag<OrderModel> result = new ConcurrentBag<OrderModel>();
            lock (_lock)
                if (OrderModelList != null && OrderModelList.Count > 0)
                {
                    switch (filterName)
                    {
                        case "WaitValidClientOrderList":
                            result = new ConcurrentBag<OrderModel>(OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Pre_Client_Validation.ToString())).ToList());
                            break;
                        case "InProcessOrderList":
                            result = new ConcurrentBag<OrderModel>(OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Order.ToString()) || x.TxtStatus.Equals(EOrderStatus.Credit.ToString())).ToList());
                            break;
                        case "WaitValidOrderList":
                            result = new ConcurrentBag<OrderModel>(OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString()) || x.TxtStatus.Equals(EOrderStatus.Pre_Credit.ToString())).ToList());
                            break;
                        case "ClosedOrderList":
                            result = new ConcurrentBag<OrderModel>(OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Order_Close.ToString()) || x.TxtStatus.Equals(EOrderStatus.Credit_CLose.ToString())).ToList());
                            break;
                        case "WaitPayOrderList":
                            result = new ConcurrentBag<OrderModel>(OrderModelList.Where(x => x.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()) || x.TxtStatus.Equals(EOrderStatus.Bill_Credit.ToString())).ToList());
                            break;
                    }
                }

            return result.ToList();
        }

        public async Task deleteOrderDataAsync(OrderModel orderModel)
        {
            if (await Dialog.showAsync("do you really want to delete this order (" + orderModel.TxtID + ")"))
            {
                List<Bill> billFoundList = await Bl.BlOrder.GetBillDataByOrderListAsync(new List<Entity.Order> { orderModel.Order });
                if (billFoundList.Count() == 0 || await OrderDetailViewModel.checkIfLastBillAsync(billFoundList.Select(x => new BillModel { Bill = x }).ToList()))
                {
                    Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);

                    OrderDetailViewModel.OrderSelected = orderModel;
                    var order_itemModelFoundList = OrderDetailViewModel.Order_ItemListToModelViewList(Bl.BlOrder.GetOrder_itemByOrderList(new List<Entity.Order> { orderModel.Order }));
                    var deliveryFoundList = Bl.BlOrder.GetDeliveryDataByOrderList(new List<Entity.Order> { orderModel.Order });
                    var Item_deliveryFoundList = Bl.BlItem.GetItem_deliveryDataByDeliveryList(deliveryFoundList);
                    var tax_orderFoundList = Bl.BlOrder.GetTax_orderDataByOrderList(new List<Entity.Order> { orderModel.Order });

                    // deleting everything generated by this order
                    await Bl.BlOrder.DeleteTax_orderAsync(tax_orderFoundList);
                    await Bl.BlItem.DeleteItem_deliveryAsync(Item_deliveryFoundList);
                    await Bl.BlOrder.DeleteDeliveryAsync(deliveryFoundList);
                    await Bl.BlOrder.DeleteBillAsync(billFoundList);
                    await Bl.BlOrder.DeleteOrder_itemAsync(order_itemModelFoundList.Select(x => x.Order_Item).ToList());
                    await Bl.BlOrder.DeleteOrderAsync(new List<Entity.Order> { orderModel.Order });

                    // unlock the item to allow deletion
                    OrderDetailViewModel.lockOrUnlockedOrder_itemItems(order_itemModelFoundList, isLocked: false);

                    OrderModelList.Remove(orderModel);
                    updateOrderModelListBinding();
                    Dialog.IsDialogOpen = false;
                }
                else
                    await Dialog.showAsync("Order invoice is not the latest.");
            }
        }

        public void loadCurrencies()
        {
            object _lock = new object();
            lock (_lock)
            {
                CurrenciesList = Bl.BlOrder.GetCurrencyData((int)EOrderStatus.ALL).Select(x => new CurrencyModel { Currency = x }).ToList();
            }
        }

        public async Task loadCurrenciesAsync()
        {
            CurrenciesList = (await Bl.BlOrder.GetCurrencyDataAsync((int)EOrderStatus.ALL)).Select(x=> new CurrencyModel { Currency = x }).ToList();
        }


        public override void Dispose()
        {
            PropertyChanged -= onBlockSearchResultVisibilityChange;
            TaxTask.PropertyChanged -= onTaxTaskCompletion_getTax;
            Bl.BlOrder.Dispose();
            OrderDetailViewModel.Dispose();
            OrderSideBarViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------

        private void onTaxTaskCompletion_getTax(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                TaxList = TaxTask.Result;
            }
        }

        private void onBlockSearchResultVisibilityChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("BlockSearchResultVisibility"))
            {
                if (BlockSearchResultVisibility.Equals("Visible"))
                    BlockOrderVisibility = "Hidden";
                else
                    BlockOrderVisibility = "Visible";
            }
        }


        //----------------------------[ Action Command ]------------------

        /// <summary>
        /// Save the selected order
        /// </summary>
        /// <param name="obj"></param>
        public void saveSelectedOrder(OrderModel obj)
        {
            obj.CurrencyModel = CurrencyModel;
            SelectedOrderModel = obj;
            executeNavig("order-detail");
            _main.IsRefresh = true;
        }

        private bool canSaveSelectedOrder(OrderModel arg)
        {
            return true;
        }


        /// <summary>
        /// Navigate through the application
        /// </summary>
        /// <param name="obj"></param>
        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "order":
                    _page(this);
                    break;
                case "order-detail":
                    _page(OrderDetailViewModel);
                    break;
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        private async void deleteOrder(OrderModel obj)
        {
            await deleteOrderDataAsync(obj);
            _page(this);
        }

        public bool canDeleteOrder(OrderModel arg)
        {
            bool isAdmin = _main.securityCheck(EAction.Security, ESecurity.SendEmail)
                             && _main.securityCheck(EAction.Security, ESecurity._Delete)
                                && _main.securityCheck(EAction.Security, ESecurity._Read)
                                    && _main.securityCheck(EAction.Security, ESecurity._Update)
                                        && _main.securityCheck(EAction.Security, ESecurity._Write);
            if (isAdmin)
                return true;
            return false;
        }

        private async void searchOrder(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["search_message"]);

            List<Entity.Order> billOrderList = new List<Entity.Order>();
            List<Entity.Order> CLientOrderList = new List<Entity.Order>();
            List<Entity.Order> orderTotal = new List<Entity.Order>();
            List<Entity.Order> orderFilterByDate = new List<Entity.Order>();
            List<Entity.Order> orderList = new List<Entity.Order>();

            orderList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchOrderAsync(new Entity.Order { ID = OrderSearchModel.OrderSearch.OrderId }, ESearchOption.AND) : Bl.BlOrder.GetOrderDataById(OrderSearchModel.OrderSearch.OrderId);

            var billFoundList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchBillAsync(new Entity.Bill { ID = OrderSearchModel.OrderSearch.BillId }, ESearchOption.AND) : Bl.BlOrder.GetBillDataById(OrderSearchModel.OrderSearch.BillId);
            if (billFoundList.Count > 0)
                billOrderList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchOrderAsync(new Entity.Order { ID = billFoundList[0].OrderId }, ESearchOption.AND) : Bl.BlOrder.searchOrder(new Entity.Order { ID = billFoundList[0].OrderId }, ESearchOption.OR);

            var clientFoundList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlClient.searchClientAsync(new Entity.Client { ID = OrderSearchModel.OrderSearch.ClientId, Company = OrderSearchModel.TxtCompanyName, CompanyName = OrderSearchModel.TxtCompanyName }, ESearchOption.OR) : Bl.BlClient.searchClient(new Entity.Client { ID = OrderSearchModel.OrderSearch.ClientId, Company = OrderSearchModel.TxtCompanyName, CompanyName = OrderSearchModel.TxtCompanyName }, ESearchOption.OR);
            foreach (var client in clientFoundList)
            {
                var clientOrderFound = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchOrderAsync(new Entity.Order { ClientId = client.ID }, ESearchOption.AND) : Bl.BlOrder.searchOrder(new Entity.Order { ClientId = client.ID }, ESearchOption.OR);
                CLientOrderList = new List<Entity.Order>(CLientOrderList.Concat(clientOrderFound));
            }

            List<Order> orderFoundList = new List<Order>();
            if (!string.IsNullOrEmpty(OrderSearchModel.TxtSelectedStatus) && OrderSearchModel.SelectedAgent != null)
                orderFoundList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchOrderAsync(new Entity.Order { Status = OrderSearchModel.TxtSelectedStatus, AgentId = OrderSearchModel.SelectedAgent.ID }, ESearchOption.OR) : Bl.BlOrder.searchOrder(new Entity.Order { Status = OrderSearchModel.TxtSelectedStatus, AgentId = OrderSearchModel.SelectedAgent.ID }, ESearchOption.OR);
            else if (!string.IsNullOrEmpty(OrderSearchModel.TxtSelectedStatus))
                orderFoundList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchOrderAsync(new Entity.Order { Status = OrderSearchModel.TxtSelectedStatus }, ESearchOption.OR) : Bl.BlOrder.searchOrder(new Entity.Order { Status = OrderSearchModel.TxtSelectedStatus }, ESearchOption.OR);
            else if (OrderSearchModel.SelectedAgent != null)
                orderFoundList = (OrderSearchModel.IsDeepSearch) ? await Bl.BlOrder.searchOrderAsync(new Entity.Order { AgentId = OrderSearchModel.SelectedAgent.ID }, ESearchOption.OR) : Bl.BlOrder.searchOrder(new Entity.Order { AgentId = OrderSearchModel.SelectedAgent.ID }, ESearchOption.OR);

            orderTotal = orderList.Concat(orderFoundList).ToList();
            orderTotal = new List<Entity.Order>(orderTotal.Concat(billOrderList));
            orderTotal = new List<Entity.Order>(orderTotal.Concat(CLientOrderList));

            orderFilterByDate = orderTotal;

            if (OrderSearchModel.OrderSearch.StartDate != Utility.DateTimeMinValueInSQL2005)
                orderFilterByDate = orderFilterByDate.Where(x => x.Date >= OrderSearchModel.OrderSearch.StartDate).ToList();
            if (OrderSearchModel.OrderSearch.EndDate != Utility.DateTimeMinValueInSQL2005)
                orderFilterByDate = orderFilterByDate.Where(x => x.Date <= OrderSearchModel.OrderSearch.EndDate).ToList();

            OrderModelList = OrderListToModelList(orderFilterByDate);

            BlockSearchResultVisibility = "Visible";

            Dialog.IsDialogOpen = false;
        }

        private bool canSearchOrder(object arg)
        {
            bool isAdmin = _main.securityCheck(EAction.Security, ESecurity.SendEmail)
                             && _main.securityCheck(EAction.Security, ESecurity._Delete)
                                && _main.securityCheck(EAction.Security, ESecurity._Read)
                                    && _main.securityCheck(EAction.Security, ESecurity._Update)
                                        && _main.securityCheck(EAction.Security, ESecurity._Write);
            if (isAdmin)
                return true;
            return false;
        }
    }
}
