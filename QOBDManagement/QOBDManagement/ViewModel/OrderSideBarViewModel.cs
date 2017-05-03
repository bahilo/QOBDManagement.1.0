﻿using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using Entity = QOBDCommon.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDCommon.Enum;
using QOBDManagement.Models;
using QOBDManagement.Classes;
using System.ComponentModel;
using QOBDBusiness;
using QOBDCommon.Classes;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class OrderSideBarViewModel : BindBase
    {
        private Func<Object, Object> _page;
        private string _navigTo;
        private Cart _cart;
        private NotifyTaskCompletion<List<Entity.Order_item>> _order_itemTask_updateItem;
        private NotifyTaskCompletion<List<Entity.Order_item>> _order_itemTask_updateCommand_Item;
        private NotifyTaskCompletion<object> _quoteTask;

        //----------------------------[ Models ]------------------

        private OrderModel _selectedCommandModel;
        private OrderDetailViewModel _orderDetailViewModel;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> UtilitiesCommand { get; set; }
        public ButtonCommand<string> SetupOrderCommand { get; set; }


        public OrderSideBarViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
        }

        public OrderSideBarViewModel(IMainWindowViewModel mainWindowViewModel, OrderDetailViewModel orderDetail) : this()
        {
            _main = mainWindowViewModel;
            _orderDetailViewModel = orderDetail;
            _page = _main.navigation;
            Cart = _main.Cart;
            initEvents();
        }


        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onNavigToChange;
            PropertyChanged += onSelectedCommandModelChange;
            //_orderDetailViewModel.PropertyChanged += onorder_ItemModelListChange;
        }

        private void instances()
        {
            _order_itemTask_updateItem = new NotifyTaskCompletion<List<Entity.Order_item>>();
            _order_itemTask_updateCommand_Item = new NotifyTaskCompletion<List<Entity.Order_item>>();
            _quoteTask = new NotifyTaskCompletion<object>();
        }

        private void instancesModel()
        {
            _selectedCommandModel = new OrderModel();

        }

        private void instancesCommand()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
            SetupOrderCommand = new Command.ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);

        }

        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public OrderModel SelectedOrderModel
        {
            get { return _selectedCommandModel; }
            set { setProperty(ref _selectedCommandModel, value); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value); }
        }

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value); }
        }


        //----------------------------[ Actions ]------------------


        private void updateCommand()
        {
            UtilitiesCommand.raiseCanExecuteActionChanged();
            SetupOrderCommand.raiseCanExecuteActionChanged();
        }

        private void generateAllBillsPdf()
        {
            foreach (var billModel in SelectedOrderModel.BillModelList)
            {
                Bl.BlOrder
                    .GeneratePdfOrder(new QOBDCommon
                        .Structures
                            .ParamOrderToPdf
                    {
                        BillId = billModel.Bill.ID,
                        OrderId = SelectedOrderModel.Order.ID
                    });
            }
        }

        /// <summary>
        /// Navigate through the application
        /// </summary>
        /// <param name="obj"> the page to navig to</param>
        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "select-client":
                    _page(new ClientViewModel());
                    break;
            }
        }

        public override void Dispose()
        {
            PropertyChanged -= onNavigToChange;
            PropertyChanged -= onSelectedCommandModelChange;
        }

        //----------------------------[ Event Handler ]------------------
        
        /// <summary>
        /// Navigate to the next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onNavigToChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "NavigTo"))
            {
                executeNavig(NavigTo);
            }
        }

        public void onCurrentPageChange_updateCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentViewModel"))
                updateCommand();
        }

        private void onSelectedCommandModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedOrderModel"))
                updateCommand();
        }

        //----------------------------[ Action Commands ]------------------


        private bool canExecuteUtilityAction(string arg)
        {
            if ((_page(null) as OrderDetailViewModel) == null)
                return false;

            if ((_page(null) as OrderDetailViewModel) != null && 
                (SelectedOrderModel.Order.ID == 0
                || string.IsNullOrEmpty(SelectedOrderModel.TxtStatus)))
                return false;

            if ((arg.Equals("close-order") || arg.Equals("close-credit")) && !this._main.securityCheck(EAction.Order_Close, ESecurity._Update)
                || arg.Equals("valid-order") && !this._main.securityCheck(EAction.Order_Valid, ESecurity._Update) && !this._main.securityCheck(EAction.Order_Valid, ESecurity._Write)
                || arg.Equals("convert-orderToQuote") && !this._main.securityCheck(EAction.Order_Quote, ESecurity._Write) && !this._main.securityCheck(EAction.Order, ESecurity._Update)
                || arg.Equals("valid-credit") && !this._main.securityCheck(EAction.Order_Close, ESecurity._Update))
                return false;

            if (!SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Quote.ToString())
                && (arg.Equals("convert-quoteToOrder")
                || arg.Equals("convert-quoteToCredit")
                || arg.Equals("open-email")
                ))
                return false;

            if (SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Quote.ToString())
                && (arg.Equals("close-order")
                   || arg.Equals("close-credit")
                   || arg.Equals("valid-order")
                   || arg.Equals("convert-orderToQuote")
                   ))
                return false;

            if ((SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Order.ToString()) || !SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString()))
                && arg.Equals("valid-order"))
                return false;

            if ((SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Credit.ToString()) || !SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Pre_Credit.ToString()))
                && arg.Equals("valid-credit"))
                return false;

            if ((SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Order_Close.ToString()) || !SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()) )//|| !SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Billed.ToString())
                && arg.Equals("close-order"))
                return false;

            if ((SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Credit_CLose.ToString()) || !SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Bill_Credit.ToString()) ) // || !SelectedOrderModel.TxtStatus.Equals(EOrderStatus.Billed.ToString())
                && arg.Equals("close-credit"))
                return false;

            return true;
        }

        private async void executeUtilityAction(string obj)
        {
            OrderDetailViewModel orderDetail = _main.OrderViewModel.OrderDetailViewModel;
            switch (obj)
            {
                case "convert-quoteToOrder":
                    orderDetail.updateOrderStatus(EOrderStatus.Pre_Order);
                    break;
                case "valid-order":
                    orderDetail.updateOrderStatus(EOrderStatus.Order);
                    break;
                case "valid-credit":
                    if (await Dialog.showAsync("Do you really want to validate this credit?"))
                        orderDetail.updateOrderStatus(EOrderStatus.Credit);
                    break;
                case "convert-orderToQuote":
                    if (await Dialog.showAsync("Do you really want to convert into quote?"))
                        orderDetail.updateOrderStatus(EOrderStatus.Quote);                        
                    break;
                case "convert-quoteToCredit":
                    if (await Dialog.showAsync("Do you really want to convert into credit?"))
                        orderDetail.updateOrderStatus(EOrderStatus.Pre_Credit);
                    break;
                case "close-order":
                    if (await Dialog.showAsync("Do you really want to close this order?"))
                        orderDetail.updateOrderStatus(EOrderStatus.Order_Close);
                    break;
                case "close-credit":
                    if (await Dialog.showAsync("Do you really want to close this credit?"))
                        orderDetail.updateOrderStatus(EOrderStatus.Credit_CLose);
                    break;
            }
            
        }

        /// <summary>
        /// set the value of the next page.
        /// </summary>
        /// <param name="obj"></param>
        private void executeSetupAction(string obj)
        {
            NavigTo = obj;
        }

        private bool canExecuteSetupAction(string arg)
        {
            if ((_page(null) as QuoteViewModel) == null)
                return false;

            if ((_page(null) as QuoteViewModel) != null
                && Cart.Client != null && Cart.Client.Client.ID != 0
                && arg.Equals("select-client"))
                return false;

            return true;
        }




    }
}