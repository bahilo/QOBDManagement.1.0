﻿using QOBDBusiness;
using Entity = QOBDCommon.Entities;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Structures;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDCommon.Classes;
using System.Collections.Concurrent;
using QOBDManagement.Interfaces;
using System.Windows.Threading;
using System.Threading;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace QOBDManagement.ViewModel
{
    public class OrderDetailViewModel : BindBase
    {
        #region [ Variables ]
        private decimal _totalPurchase;
        private decimal _totalAfterTax;
        private decimal _totalPercentProfit;
        private decimal _totalProfit;
        private decimal _totalTaxAmount;
        private decimal _totalBeforeTax;
        private GeneralInfos.FileWriter _mailFile;
        private ParamOrderToPdf _paramQuoteToPdf;
        private ParamOrderToPdf _paramOrderToPdf;
        private ParamDeliveryToPdf _paramDeliveryToPdf;
        public NotifyTaskCompletion<List<Order_item>> _order_ItemTask { get; set; }
        public NotifyTaskCompletion<bool> _updateOrderStatusTask { get; set; }
        private Func<object, object> _page;        
        private List<Tax> _taxes;
        private EStatusOrder _orderStatus;

        #endregion

        #region [ Models Variables ]
        //----------------------------[ Models ]------------------

        private List<Order_itemModel> _order_itemList;
        private List<Item_deliveryModel> _item_deliveryModelBillingInProcessList;
        private OrderModel _orderSelected;
        private BillModel _selectedBillToSend;
        private List<Item_deliveryModel> _item_ModelDeliveryInProcess;
        private List<Item_deliveryModel> _item_deliveryModelCreatedList;
        private IMainWindowViewModel _main;
        #endregion

        #region [ Commands Variables ]
        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> UpdateOrder_itemListCommand { get; set; }
        public ButtonCommand<Item_deliveryModel> CancelDeliveryReceiptCreationCommand { get; set; }
        public ButtonCommand<Item_deliveryModel> CancelDeliveryReceiptCreatedCommand { get; set; }
        public ButtonCommand<BillModel> CancelBillCreatedCommand { get; set; }
        public ButtonCommand<DeliveryModel> GenerateDeliveryReceiptCreatedPdfCommand { get; set; }
        public ButtonCommand<BillModel> GeneratePdfCreatedBillCommand { get; set; }
        public ButtonCommand<Order_itemModel> DeliveryReceiptCreationCommand { get; set; }
        public ButtonCommand<Order_itemModel> BillCreationCommand { get; set; }
        public ButtonCommand<Order_itemModel> DeleteItemCommand { get; set; }
        public ButtonCommand<object> BilledCommand { get; set; }
        public ButtonCommand<Address> DeliveryAddressSelectionCommand { get; set; }
        public ButtonCommand<Tax> TaxCommand { get; set; }
        public ButtonCommand<object> GeneratePdfCreatedQuoteCommand { get; set; }
        public ButtonCommand<string> SendEmailCommand { get; set; }
        public ButtonCommand<BillModel> UpdateBillCommand { get; set; }
        public ButtonCommand<object> UpdateCommentCommand { get; set; }

        #endregion

        #region [ Contructors]
        public OrderDetailViewModel()
        {
            instances();
            instancesPoco();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        public OrderDetailViewModel(IMainWindowViewModel main): this()
        {
            _main = main;
            _page = _main.navigation;
        }

        #endregion

        #region [ Initialization ]
        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onOrderSelectedChange;
            PropertyChanged += onOrder_itemModelWorkFlowChange;
            _order_ItemTask.PropertyChanged += onOrder_itemTaskComplete_getOrderModel;
            _updateOrderStatusTask.PropertyChanged += onInitializationTaskComplete_UpdateOrderStatus;
        }

        private void instances()
        {
            _selectedBillToSend = new BillModel();
            _order_ItemTask = new NotifyTaskCompletion<List<Order_item>>();
            _updateOrderStatusTask = new NotifyTaskCompletion<bool>();
            _paramDeliveryToPdf = new ParamDeliveryToPdf();
            _paramQuoteToPdf = new ParamOrderToPdf(EStatusOrder.Quote, 2);
            _paramOrderToPdf = new ParamOrderToPdf(EStatusOrder.Order);
            _paramOrderToPdf.Currency = _paramQuoteToPdf.Currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            _paramOrderToPdf.Lang = _paramQuoteToPdf.Lang = _paramDeliveryToPdf.Lang = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault() ?? "en";

            _mailFile = new GeneralInfos.FileWriter("", EOption.Mails);
            //_quoteEmailFile.TxtSubject = "** CODSIMEX – Votre devis n°{BILL_ID} **";
        }

        private void instancesPoco()
        {
            _taxes = new List<Tax>();
        }

        private void instancesModel()
        {
            _orderSelected = new OrderModel();
            _order_itemList = new List<Order_itemModel>();
            _item_deliveryModelBillingInProcessList = new List<Item_deliveryModel>();
            _item_deliveryModelCreatedList = new List<Item_deliveryModel>();
            _item_ModelDeliveryInProcess = new List<Item_deliveryModel>();
        }

        private void instancesCommand()
        {
            UpdateOrder_itemListCommand = new ButtonCommand<string>(updateOrder_itemData, canUpdateOrder_itemData);
            CancelDeliveryReceiptCreationCommand = new ButtonCommand<Item_deliveryModel>(cancelDeliveryReceiptInProcess, canCancelDeliveryReceiptInProcess);
            GenerateDeliveryReceiptCreatedPdfCommand = new ButtonCommand<DeliveryModel>(generateDeliveryReceiptPdf, canGenerateDeliveryReceiptPdf);
            CancelDeliveryReceiptCreatedCommand = new ButtonCommand<Item_deliveryModel>(cancelDeliveryReceiptCreated, canCancelDeliveryReceiptCreated);
            DeliveryReceiptCreationCommand = new ButtonCommand<Order_itemModel>(createDeliveryReceipt, canCreateDeliveryReceipt);
            BillCreationCommand = new ButtonCommand<Order_itemModel>(createBill, canCreateBill);
            CancelBillCreatedCommand = new ButtonCommand<BillModel>(cancelBillCreated, canCancelBillCreated);
            GeneratePdfCreatedBillCommand = new ButtonCommand<BillModel>(generateOrderBillPdf, canGenerateOrderBillPdf);
            DeleteItemCommand = new ButtonCommand<Order_itemModel>(deleteItem, canDeleteItem);
            BilledCommand = new ButtonCommand<object>(billing, canBilling);
            DeliveryAddressSelectionCommand = new ButtonCommand<Address>(selectDeliveryAddress, canSelectDeliveryAddress);
            TaxCommand = new ButtonCommand<Tax>(saveNewTax, canSaveNewTax);
            GeneratePdfCreatedQuoteCommand = new ButtonCommand<object>(generateQuotePdf, canGenerateQuotePdf);
            SendEmailCommand = new ButtonCommand<string>(sendEmail, canSendEmail);
            UpdateBillCommand = new ButtonCommand<BillModel>(updateBill, canUpdateBill);
            UpdateCommentCommand = new ButtonCommand<object>(updateComment, canUpdateComment);
        }
        #endregion

        #region [ Properties ]
        //----------------------------[ Properties ]------------------        

        public BillModel SelectedBillToSend
        {
            get { return _selectedBillToSend; }
            set { setProperty(ref _selectedBillToSend, value); }
        }

        public List<BillModel> BillModelList
        {
            get { return OrderSelected.BillModelList; }
            set { OrderSelected.BillModelList = value; onPropertyChange("BillModelList"); }
        }

        public List<DeliveryModel> DeliveryModelList
        {
            get { return OrderSelected.DeliveryModelList; }
            set { OrderSelected.DeliveryModelList = value; onPropertyChange("DeliveryModelList"); }
        }

        public GeneralInfos.FileWriter EmailFile
        {
            get { return _mailFile; }
            set { setProperty(ref _mailFile, value); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public OrderModel OrderSelected
        {
            get { return _orderSelected; }
            set { setProperty(ref _orderSelected, value); }
        }

        public string TxtQuoteValidityInDays
        {
            get { return _paramQuoteToPdf.ValidityDay.ToString(); }
            set { int convertedNumber; if (int.TryParse(value, out convertedNumber)) { _paramQuoteToPdf.ValidityDay = convertedNumber; onPropertyChange("QuoteValidityInDays"); } }
        }

        public bool IsQuote
        {
            get
            {
                if (_paramQuoteToPdf.TypeQuoteOrProformat == EStatusOrder.Quote)
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _paramQuoteToPdf.TypeQuoteOrProformat = EStatusOrder.Quote;
                    onPropertyChange("IsQuote");
                }
            }
        }

        public bool IsProForma
        {
            get
            {
                if (_paramQuoteToPdf.TypeQuoteOrProformat == EStatusOrder.Proforma)
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _paramQuoteToPdf.TypeQuoteOrProformat = EStatusOrder.Proforma;
                    onPropertyChange("IsProForma");
                }
            }
        }

        public bool IsQuoteReferencesVisible
        {
            get { return _paramQuoteToPdf.IsQuoteConstructorReferencesVisible; }
            set { _paramQuoteToPdf.IsQuoteConstructorReferencesVisible = value; onPropertyChange("IsQuoteReferencesVisible"); }
        }

        public bool IsOrderReferencesVisible
        {
            get { return _paramOrderToPdf.IsOrderConstructorReferencesVisible; }
            set { _paramOrderToPdf.IsOrderConstructorReferencesVisible = value; onPropertyChange("IsOrderReferencesVisible"); }
        }

        public string TxtTotalAfterTax
        {
            get { return _totalAfterTax.ToString(); }
            set { setProperty(ref _totalAfterTax, Convert.ToDecimal(value)); }
        }

        public string TxtTotalPercentProfit
        {
            get { return _totalPercentProfit.ToString(); }
            set { setProperty(ref _totalPercentProfit, Convert.ToDecimal(value)); }
        }

        public string TxtTotalProfit
        {
            get { return _totalProfit.ToString(); }
            set { setProperty(ref _totalProfit, Convert.ToDecimal(value), ""); }
        }

        public string TxtTotalTaxAmount
        {
            get { return _totalTaxAmount.ToString(); }
            set { setProperty(ref _totalTaxAmount, Convert.ToDecimal(value)); }
        }

        public Tax Tax
        {
            get { return _orderSelected.Tax; }
            set { _orderSelected.Tax = value; onPropertyChange("Tax"); }
        }

        public string TxtTotalBeforeTax
        {
            get { return _totalBeforeTax.ToString(); }
            set { setProperty(ref _totalBeforeTax, Convert.ToDecimal(value)); }
        }

        public string TxtTotalPurchase
        {
            get { return _totalPurchase.ToString(); }
            set { setProperty(ref _totalPurchase, Convert.ToDecimal(value)); }
        }

        public List<Order_itemModel> Order_ItemModelList
        {
            get { return _order_itemList; }
            set { _order_itemList = value; updateDeliveryAndInvoiceListBindingByCallingPropertyChange(); }
        }

        public List<Item_deliveryModel> Item_ModelDeliveryInProcess
        {
            get { return _item_ModelDeliveryInProcess; }
            set { setProperty(ref _item_ModelDeliveryInProcess, value); }
        }

        public List<Item_deliveryModel> Item_deliveryModelCreatedList
        {
            get { return _item_deliveryModelCreatedList; }
            set { setProperty(ref _item_deliveryModelCreatedList, value); }
        }

        public List<Item_deliveryModel> Item_deliveryModelBillingInProcess
        {
            get { return _item_deliveryModelBillingInProcessList; }
            set { setProperty(ref _item_deliveryModelBillingInProcessList, value); BillCreationCommand.raiseCanExecuteActionChanged(); }
        }

        #endregion

        #region [ Signaling ]
        //---------------------------[ Signaling ]------------------

        public bool IsItemListCommentTextBoxEnabled
        {
            get { return disableUIElementByBoolean(); }
        }

        public bool IsItemListQuantityReceivedTextBoxEnabled
        {
            get { return disableUIElementByBoolean(); }
        }

        public bool IsItemListPurchasePriceTextBoxEnable
        {
            get { return disableUIElementByBoolean(); }
        }

        public bool IsItemListSellingPriceTextBoxEnable
        {
            get { return disableUIElementByBoolean(); }
        }

        public bool IsItemListQuantityTextBoxEnable
        {
            get { return disableUIElementByBoolean(); }
        }

        public string BlockItemListDetailVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockEmailVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockBillCreationVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockDeliveryReceiptCreationVisiblity
        {
            get { return disableUIElementByString(); }
        }

        public string BlockDeliveryAddressVisiblity
        {
            get { return disableUIElementByString(); }
        }

        public string BlockDeliveryReceiptCreatedVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockBillCreatedVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockStepOneVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockStepTwoVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockStepThreeVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockDeliveryListToIncludeVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockBillListBoxVisibility
        {
            get { return disableUIElementByString(); }
        }

        #endregion

        #region [ Actions ]
        //----------------------------[ Actions ]------------------

        public List<Order_itemModel> Order_ItemListToModelViewList(List<Order_item> Order_ItemList)
        {
            List<Order_itemModel> output = new List<Order_itemModel>();
            foreach (Order_item order_Item in Order_ItemList)
            {
                Order_itemModel localOrder_item = new Order_itemModel();
                localOrder_item.PropertyChanged += onTotalSelling_PriceOrPrice_purchaseChange;
                localOrder_item.Order_Item = order_Item;

                //load item and its inforamtion (delivery and item_delivery)
                localOrder_item.ItemModel = loadOrder_itemItem(order_Item.Item_ref, localOrder_item.Order_Item.ItemId);

                output.Add(localOrder_item);
            }

            return output;
        }

        private List<ItemModel> ItemListToModelViewList(List<Item> itemList)
        {
            List<ItemModel> output = new List<ItemModel>();
            foreach (Item item in itemList)
            {
                ItemModel itemModel = new ItemModel();
                itemModel.Item = item;

                var itemDelivery = new Item_delivery();
                itemDelivery.Item_ref = item.Ref;
                itemDelivery.ItemId = item.ID;

                // search for the delivery reference of the item
                var item_DeliveryFoundList = Bl.BlItem.searchItem_delivery(itemDelivery, ESearchOption.AND).Select(x => new Item_deliveryModel { Item_delivery = x, Item = item }).ToList();
                if (item_DeliveryFoundList != null && item_DeliveryFoundList.Count > 0)
                    itemModel.Item_deliveryModelList = item_DeliveryFoundList;

                // search for the item's delivery receipt
                foreach (var item_delivery in itemModel.Item_deliveryModelList)
                {
                    var deliveryFoundList = Bl.BlOrder.searchDelivery(new Delivery { ID = item_delivery.Item_delivery.DeliveryId }, ESearchOption.AND);
                    if (deliveryFoundList.Count > 0)
                        item_delivery.DeliveryModel = deliveryFoundList.Select(x => new DeliveryModel { Delivery = x }).FirstOrDefault();
                }
                output.Add(itemModel);
            }
            return output;
        }

        public void loadOrder_items()
        {
            Dialog.showSearch("Loading items...");
            Order_ItemModelList = Order_ItemListToModelViewList( Bl.BlOrder.searchOrder_item(new Order_item { OrderId = OrderSelected.Order.ID }, ESearchOption.AND));

            totalCalcul();
            refreshBindings();
            loadEmail();
            Dialog.IsDialogOpen = false;

        }

        public void loadEmail()
        {
            if (OrderSelected != null)
            {
                var infos = Bl.BlReferential.searchInfos(new Info { Name = "Company_name" }, ESearchOption.AND).FirstOrDefault();
                var infosFTP =  Bl.BlReferential.searchInfos(new QOBDCommon.Entities.Info { Name = "ftp_" }, ESearchOption.AND).ToList();
                string login = infosFTP.Where(x => x.Name == "ftp_login").Select(x => x.Value).FirstOrDefault() ?? "";
                string password = infosFTP.Where(x => x.Name == "ftp_password").Select(x => x.Value).FirstOrDefault() ?? "";
                switch (OrderSelected.TxtStatus)
                {
                    case "Quote":
                        EmailFile = new GeneralInfos.FileWriter("quote", EOption.Mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Quote n°{QUOTE_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Quote n°{QUOTE_ID} **";
                        break;
                    case "Pre_Order":
                        EmailFile = new GeneralInfos.FileWriter("order_confirmation", EOption.Mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Invoice n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Order n°{BILL_ID} **";
                        break;
                    case "Pre_Credit":
                        EmailFile = new GeneralInfos.FileWriter("order_confirmation", EOption.Mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Credit with Invoice n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Credit with Invoice n°{BILL_ID} **";
                        break;
                    case "Order":
                        EmailFile = new GeneralInfos.FileWriter("bill", EOption.Mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Bill n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Bill n°{BILL_ID} **";
                        break;
                }
            }

        }
        

        /// <summary>
        /// load all bills of the selected Order
        /// </summary>
        public void loadInvoicesAndDeliveryReceipts()
        {
            Item_deliveryModelBillingInProcess = (from c in Order_ItemModelList
                                                  from d in c.ItemModel.Item_deliveryModelList
                                                  where d.DeliveryModel.TxtStatus == EStatusOrder.Not_Billed.ToString()
                                                  select d).ToList();

            Item_ModelDeliveryInProcess = Order_ItemModelList.Where(x => x.Order_Item.Quantity_current > 0).Select(x => new Item_deliveryModel { Item = x.ItemModel.Item, TxtQuantity_current = x.TxtQuantity_current, TxtQuantity_delivery = x.TxtQuantity_delivery }).ToList();

            BillModelList = new BillModel().BillListToModelViewList(Bl.BlOrder.searchBill(new Bill { OrderId = OrderSelected.Order.ID }, ESearchOption.AND));
            DeliveryModelList = new DeliveryModel().DeliveryListToModelViewList(Bl.BlOrder.searchDelivery(new Delivery { OrderId = OrderSelected.Order.ID }, ESearchOption.AND));

        }
        
        private void totalCalcul()
        {
            if (Order_ItemModelList.Count > 0)
            {
                decimal totalProfit = 0.0m;
                decimal totalBeforeTax = 0.0m;
                decimal totalPurchase = 0.0m;

                foreach (Order_itemModel order_itemModel in Order_ItemModelList)
                {
                    var order_item = order_itemModel.Order_Item;
                    totalProfit += order_item.Quantity * (order_item.Price - order_item.Price_purchase);
                    totalBeforeTax += Convert.ToDecimal(order_itemModel.TxtTotalSelling);
                    totalPurchase += Convert.ToDecimal(order_itemModel.TxtTotalPurchase);
                }
                this.TxtTotalTaxAmount = Convert.ToString(((decimal)OrderSelected.Tax_order.Tax_value / 100) * totalBeforeTax);
                this.TxtTotalProfit = totalProfit.ToString();
                this.TxtTotalBeforeTax = totalBeforeTax.ToString();
                this.TxtTotalAfterTax = string.Format("{0:0.00}", (totalBeforeTax + (totalBeforeTax) * ((decimal)OrderSelected.Tax_order.Tax_value) / 100));
                try
                {
                    this.TxtTotalPercentProfit = string.Format("{0:0.00}", ((totalProfit / totalBeforeTax) * 100));
                }
                catch (DivideByZeroException) { this.TxtTotalPercentProfit = "0"; }

                this.TxtTotalPurchase = string.Format("{0:0.00}", totalPurchase);
            }
        }

        private void loadAddresses()
        {
            OrderSelected.AddressList = Bl.BlClient.searchAddress(new Address { ClientId = OrderSelected.CLientModel.Client.ID }, ESearchOption.AND);
        }

        public ItemModel loadOrder_itemItem(string itemRef, int itemId)
        {
            var itemFoundList = Bl.BlItem.searchItem(new Item { ID = itemId, Ref = itemRef }, ESearchOption.AND);
            if (itemFoundList.Count > 0)
                return ItemListToModelViewList(new List<Item> { itemFoundList[0] }).FirstOrDefault();
            return new ItemModel();
        }

        private bool disableUIElementByBoolean([CallerMemberName]string obj = "")
        {

            if ((OrderSelected.TxtStatus.Equals(EStatusOrder.Bill_Order.ToString()) || OrderSelected.TxtStatus.Equals(EStatusOrder.Bill_Credit.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityReceivedTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;
            if ((OrderSelected.TxtStatus.Equals(EStatusOrder.Order_Close.ToString()) || OrderSelected.TxtStatus.Equals(EStatusOrder.Credit_CLose.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityReceivedTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            return true;
        }

        private string disableUIElementByString([CallerMemberName]string obj = "")
        {
            if (!OrderSelected.TxtStatus.Equals(EStatusOrder.Order.ToString())
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Collapsed";

            else if (OrderSelected.TxtStatus.Equals(EStatusOrder.Order.ToString())
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Visible";// "VisibleWhenSelected";

            if ((!OrderSelected.TxtStatus.Equals(EStatusOrder.Order.ToString()) && !OrderSelected.TxtStatus.Equals(EStatusOrder.Credit.ToString()))
                && (obj.Equals("BlockDeliveryListToIncludeVisibility")
                || obj.Equals("BlockStepOneVisibility")
                || obj.Equals("BlockStepTwoVisibility")
                || obj.Equals("BlockStepThreeVisibility")))
                return "Hidden";
            
            if (OrderSelected.TxtStatus.Equals(EStatusOrder.Quote.ToString())
                && (obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                || obj.Equals("BlockBillListBoxVisibility")
                ))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EStatusOrder.Pre_Order.ToString()) || OrderSelected.TxtStatus.Equals(EStatusOrder.Pre_Credit.ToString()))
                && (obj.Equals("BlockEmailVisibility")
                || obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                ))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EStatusOrder.Order.ToString()) || OrderSelected.TxtStatus.Equals(EStatusOrder.Credit.ToString()))
                && (obj.Equals("BlockStepOneVisibility") && Item_ModelDeliveryInProcess.Count == 0
                || obj.Equals("BlockStepTwoVisibility") && Item_deliveryModelBillingInProcess.Count == 0
                || obj.Equals("BlockStepThreeVisibility") && OrderSelected.BillModelList.Count == 0
                ))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EStatusOrder.Bill_Order.ToString()) || OrderSelected.TxtStatus.Equals(EStatusOrder.Bill_Credit.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EStatusOrder.Order_Close.ToString()) || OrderSelected.TxtStatus.Equals(EStatusOrder.Credit_CLose.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            return "Visible";
        }


        private void updateStepBinding()
        {
            onPropertyChange("BlockStepOneVisibility");
            onPropertyChange("BlockStepTwoVisibility");
            onPropertyChange("BlockStepThreeVisibility");
        }

        private void updateDeliveryAndInvoiceListBindingByCallingPropertyChange()
        {
            onPropertyChange("Order_ItemModelList");
            onPropertyChange("Item_deliveryModelCreatedList");
            onPropertyChange("Item_ModelDeliveryInProcess");
            onPropertyChange("Item_deliveryModelBillingInProcess");
        }
                
        public void updateOrderStatus(EStatusOrder status)
        {
            Dialog.showSearch("Processing...");
            _updateOrderStatusTask.initializeNewTask(updateOrderStatusAsync(status));
        }

        public async Task<bool> updateOrderStatusAsync(EStatusOrder status)
        {            
            bool canChangeStatus = true;
            _orderStatus = status;
            switch (_orderStatus)
            {
                case EStatusOrder.Order:
                    lockOrder_itemItems();
                    break;
                case EStatusOrder.Quote:
                    canChangeStatus = await cleanUpBeforeConvertingToQuoteAsync();
                    break;
                case EStatusOrder.Billed:
                    break;
                case EStatusOrder.Bill_Order:
                    break;
                case EStatusOrder.Bill_Credit:
                    break;
                case EStatusOrder.Pre_Order:
                    break;
                case EStatusOrder.Pre_Credit:
                    createCredit();
                    break;
                case EStatusOrder.Order_Close:
                    canChangeStatus = await Dialog.show("Order Closing: Be careful as it will not be possible to do any change after.");
                    break;
                case EStatusOrder.Credit_CLose:
                    canChangeStatus = await Dialog.show("Credit CLosing: Be careful as it will not be possible to do any change after.");
                    break;
            }
            //if (canChangeStatus)
            //{
            //    OrderSelected.TxtStatus = status.ToString();
            //    OrderSelected.Order.Date = DateTime.Now;
            //    var savedOrderList = await Bl.BlOrder.UpdateOrderAsync(new List<Entity.Order> { { OrderSelected.Order } });
            //    if (savedOrderList.Count > 0)
            //        OrderSelected.Order = savedOrderList[0];
            //}
            return canChangeStatus;
        }

        private async Task<bool> checkIfLastBillAsync(Bill bill, int offset)
        {
            bool isLastBill = false;
            Bill lastBill = await Bl.BlOrder.GetLastBillAsync();

            if(lastBill != null)
            {
                if (lastBill.ID - offset <= bill.ID)
                    isLastBill = true;
                else
                    isLastBill = false;
            }            
            return isLastBill;
        }

        private async Task<bool> cleanUpBeforeConvertingToQuoteAsync()
        {
            bool canDelete = false;
            var billList = BillModelList.OrderByDescending(x => x.Bill.ID).ToList();
            for (int i = 0; i < billList.Count(); i++)
            {
                if (await checkIfLastBillAsync(billList[i].Bill, i))
                    canDelete = true;
                else
                {
                    canDelete = false;
                    break;
                }
            }
            if (canDelete || BillModelList.Count == 0)
            {
                var Item_deliveryFoundListToDelete = Bl.BlItem.GetItem_deliveryDataByDeliveryList(DeliveryModelList.Select(x => x.Delivery).ToList());
                var tax_OrderFoundListToDelete = await Bl.BlOrder.GetTax_orderDataByOrderListAsync(new List<Entity.Order> { OrderSelected.Order });

                // deleting
                await Bl.BlOrder.DeleteTax_orderAsync(tax_OrderFoundListToDelete);
                await Bl.BlItem.DeleteItem_deliveryAsync(Item_deliveryFoundListToDelete);
                await Bl.BlOrder.DeleteDeliveryAsync(DeliveryModelList.Select(x => x.Delivery).ToList());
                await Bl.BlOrder.DeleteBillAsync(BillModelList.Select(x => x.Bill).ToList());
                foreach (var Order_itemToUpdate in Order_ItemModelList)
                {
                    Order_itemToUpdate.Order_Item.Quantity_current = 0;
                    Order_itemToUpdate.Order_Item.Quantity_delivery = 0;
                    await Bl.BlOrder.UpdateOrder_itemAsync(new List<Order_item> { Order_itemToUpdate.Order_Item });
                }
                BillModelList = new List<BillModel>();
                DeliveryModelList = new List<DeliveryModel>();
                if (OrderSelected.TxtStatus.Equals(EStatusOrder.Credit.ToString()))
                    createCredit(isReset: true);

                canDelete = true;
            }
            //else
            //    await Dialog.show("Convertion to Quote Failed! Order bills are not the latest.");

            return canDelete;
        }

        private void refreshBindings()
        {
            loadInvoicesAndDeliveryReceipts();
        }


        private async void lockOrder_itemItems()
        {
            List<Item> itemToSaveList = new List<Item>();
            foreach (var Order_itemModel in Order_ItemModelList)
            {
                Order_itemModel.ItemModel.TxtErasable = EItem.No.ToString();
                itemToSaveList.Add(Order_itemModel.ItemModel.Item);
            }
            await Bl.BlItem.UpdateItemAsync(itemToSaveList);
        }


        private async void createCredit(bool isReset = false)
        {
            List<Order_item> Order_itemToSave = new List<Order_item>();
            foreach (Order_itemModel Order_itemModel in Order_ItemModelList)
            {
                Order_itemModel.Order_Item.Price = (!isReset) ? Math.Abs(Order_itemModel.Order_Item.Price) * (-1) : Math.Abs(Order_itemModel.Order_Item.Price);
                Order_itemModel.Order_Item.Price_purchase = (!isReset) ? Math.Abs(Order_itemModel.Order_Item.Price_purchase) * (-1) : Math.Abs(Order_itemModel.Order_Item.Price);
                Order_itemToSave.Add(Order_itemModel.Order_Item);
            }
            var savedOrder_item = await Bl.BlOrder.UpdateOrder_itemAsync(Order_itemToSave);
        }

        public override void Dispose()
        {            
            PropertyChanged -= onOrderSelectedChange;
            PropertyChanged -= onOrder_itemModelWorkFlowChange;
            _order_ItemTask.PropertyChanged -= onOrder_itemTaskComplete_getOrderModel;
            _updateOrderStatusTask.PropertyChanged -= onInitializationTaskComplete_UpdateOrderStatus;
            foreach (var Order_itemModel in Order_ItemModelList)
                Order_itemModel.PropertyChanged -= onTotalSelling_PriceOrPrice_purchaseChange;
            Bl.BlOrder.Dispose();
        }

        #endregion

        #region [ Event Handler ]
        //----------------------------[ Event Handler ]------------------

        private void onOrderSelectedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("OrderSelected"))
            {
                loadOrder_items();
                loadAddresses();
            }
        }

        private void onTotalSelling_PriceOrPrice_purchaseChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "TxtTotalSelling") || string.Equals(e.PropertyName, "TxtPrice") || string.Equals(e.PropertyName, "TxtPrice_purchase"))
                totalCalcul();
        }

        private void onOrder_itemTaskComplete_getOrderModel(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                Dialog.showSearch("Loading items...");
                Order_ItemModelList = Order_ItemListToModelViewList(_order_ItemTask.Result);

                totalCalcul();
                refreshBindings();
                Dialog.IsDialogOpen = false;
            }
        }

        private void onOrder_itemModelWorkFlowChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Item_ModelDeliveryInProcess")
                || e.PropertyName.Equals("Item_deliveryModelBillingInProcess")
                || e.PropertyName.Equals("BillModelList"))
            {
                updateStepBinding();
            }
        }

        private async void onInitializationTaskComplete_UpdateOrderStatus(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                if (_updateOrderStatusTask.Result)
                {
                    string oldStatus = OrderSelected.TxtStatus;
                    OrderSelected.TxtStatus = _orderStatus.ToString();
                    OrderSelected.Order.Date = DateTime.Now;
                    var savedOrderList = await Bl.BlOrder.UpdateOrderAsync(new List<Entity.Order> { { OrderSelected.Order } });
                    if (savedOrderList.Count > 0 && oldStatus != OrderSelected.TxtStatus)
                    {
                        OrderSelected.Order = savedOrderList[0];
                        await Dialog.show(oldStatus+" successfully Converted to "+ OrderSelected.TxtStatus);
                        loadEmail();
                        _page(this);
                    }
                    else
                        await Dialog.show("Convertion to " + OrderSelected.TxtStatus + " Failed!");                    
                }
                else
                    await Dialog.show("Convertion to " + _orderStatus + " Failed! "+
                        Environment.NewLine+"Please make sure that this order bill is the latest.");
                Dialog.IsDialogOpen = false;
            }
            else if (e.PropertyName.Equals("Exception"))
            {
                Dialog.IsDialogOpen = false;
                Dialog.showSearch("Oops! an error occurred while processing your request."+
                    Environment.NewLine+"Please contact your administrator.");
                Log.error("Error while updating the order(ID=+"+OrderSelected.TxtID+") from "+OrderSelected.TxtStatus+""+_orderStatus.ToString());
            }
                
        }

        #endregion

        #region [ Actions Command ]

        //----------------------------[ Action Command ]------------------

        private async void updateOrder_itemData(string obj)
        {
            Dialog.showSearch("Updating...");
            List<Order_item> Order_itemToSave = new List<Order_item>();
            foreach (var Order_itemModelNew in Order_ItemModelList)
            {
                int quantityReceived = Convert.ToInt32(Order_itemModelNew.TxtQuantity_received);
                int quantity = Order_itemModelNew.Order_Item.Quantity;
                int quantityDelivery = Order_itemModelNew.Order_Item.Quantity_delivery;
                int quantityCurrent = Order_itemModelNew.Order_Item.Quantity_current;

                if (quantityReceived > 0)
                {
                    int quentityPending = quantity - (quantityDelivery + quantityReceived);
                    if (quentityPending >= 0)
                    {
                        // Checking that the number of received Item matches the expected number
                        if (quantityReceived > (quantity - quantityDelivery))
                            quantityReceived = (quantity - quantityDelivery);

                        quantityDelivery += quantityReceived;
                        quantityCurrent += quantityReceived;
                        Order_itemModelNew.Order_Item.Quantity_current = quantityCurrent;
                        Order_itemModelNew.Order_Item.Quantity_delivery = quantityDelivery;

                    }
                }
                Order_itemToSave.Add(Order_itemModelNew.Order_Item);
                Order_itemModelNew.TxtQuantity_received = 0.ToString();
            }

            var savedOrder_itemList = await Bl.BlOrder.UpdateOrder_itemAsync(Order_itemToSave);

            updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
            refreshBindings();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canUpdateOrder_itemData(string arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (!isUpdate)
                return false;

            if (string.IsNullOrEmpty(OrderSelected.TxtStatus)
                || !OrderSelected.TxtStatus.Equals(EStatusOrder.Order.ToString())
                && !OrderSelected.TxtStatus.Equals(EStatusOrder.Quote.ToString())
                && !OrderSelected.TxtStatus.Equals(EStatusOrder.Pre_Order.ToString())
                && !OrderSelected.TxtStatus.Equals(EStatusOrder.Pre_Credit.ToString()))
                return false;
            return true;
        }

        private void deleteItem(Order_itemModel obj)
        {
            Dialog.showSearch("Deleting...");
            Bl.BlOrder.DeleteOrder_itemAsync(new List<Order_item> { obj.Order_Item });
            Order_ItemModelList.Remove(obj);
            updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
            Dialog.IsDialogOpen = false;
        }

        private bool canDeleteItem(Order_itemModel arg)
        {
            bool isDelete = _main.securityCheck(EAction.Order, ESecurity._Delete);
            if (isDelete)
                return true;

            return false;
        }

        private void generateDeliveryReceiptPdf(DeliveryModel obj)
        {
            Dialog.showSearch("Pdf creation...");
            _paramDeliveryToPdf.OrderId = OrderSelected.Order.ID;
            _paramDeliveryToPdf.DeliveryId = obj.Delivery.ID;
            Bl.BlOrder.GeneratePdfDelivery(_paramDeliveryToPdf);
            Dialog.IsDialogOpen = false;
        }

        private bool canGenerateDeliveryReceiptPdf(DeliveryModel arg)
        {
            return true;
        }

        private async void createDeliveryReceipt(Order_itemModel obj)
        {
            Dialog.showSearch("Delivery receipt creation...");
            int first = 0;
            List<Delivery> insertNewDeliveryList = new List<Delivery>();
            List<Delivery> savedDeliveryList = new List<Delivery>();
            List<Order_item> Order_itemListToUpdate = new List<Order_item>();

            foreach (var item_deliveryModel in Item_ModelDeliveryInProcess)
            {
                if (item_deliveryModel.TxtQuantity_current != 0.ToString())
                {
                    if (first == 0)
                    {
                        // creation of the delivery receipt
                        Delivery delivery = new Delivery();
                        delivery.OrderId = OrderSelected.Order.ID;
                        delivery.Date = DateTime.Now;
                        delivery.Status = EStatusOrder.Not_Billed.ToString();
                        delivery.Package = item_deliveryModel.DeliveryModel.Delivery.Package;
                        insertNewDeliveryList.Add(delivery);
                        savedDeliveryList = await Bl.BlOrder.InsertDeliveryAsync(insertNewDeliveryList);
                        first++;
                    }

                    // creation of the reference of the delivery created inside item_delivery
                    if (savedDeliveryList.Count > 0)
                    {
                        Item_delivery item_delivery = new Item_delivery();
                        item_delivery.DeliveryId = savedDeliveryList[0].ID;
                        item_delivery.Item_ref = item_deliveryModel.Item.Ref;
                        item_delivery.ItemId = item_deliveryModel.Item.ID;
                        item_delivery.Quantity_delivery = Convert.ToInt32(item_deliveryModel.TxtQuantity_delivery);
                        var savedItem_deliveryList = await Bl.BlItem.InsertItem_deliveryAsync(new List<Item_delivery> { item_delivery });
                        var Order_itemModelFound = (from c in Order_ItemModelList
                                                      where c.ItemModel.Item.ID == item_deliveryModel.Item.ID && c.ItemModel.Item.Ref == item_deliveryModel.Item.Ref
                                                      select c).FirstOrDefault();
                        if (savedItem_deliveryList.Count > 0 && Order_itemModelFound != null)
                        {


                            Order_itemModelFound.Order_Item.Quantity_current = 0;
                            Order_itemModelFound.ItemModel.Item_deliveryModelList.Add(savedItem_deliveryList.Select(x => new Item_deliveryModel { Item_delivery = x, DeliveryModel = new DeliveryModel { Delivery = savedDeliveryList[0] } }).FirstOrDefault());
                            Order_itemListToUpdate.Add(Order_itemModelFound.Order_Item);
                        }
                    }
                }
            }
            var savedOrder_itemList = await Bl.BlOrder.UpdateOrder_itemAsync(Order_itemListToUpdate);
            refreshBindings();
            Dialog.IsDialogOpen = false;
        }

        private bool canCreateDeliveryReceipt(Order_itemModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void cancelDeliveryReceiptInProcess(Item_deliveryModel obj)
        {
            Dialog.showSearch("Delivery receipt creation cancelling...");

            var Order_itemFound = (from c in Order_ItemModelList
                                   where c.ItemModel.Item.Ref == obj.Item.Ref && c.ItemModel.Item.ID == obj.Item.ID
                                   select c).FirstOrDefault();

            if (Order_itemFound != null)
            {
                Order_itemFound.Order_Item.Quantity_delivery = Math.Max(0, Order_itemFound.Order_Item.Quantity_delivery - Order_itemFound.Order_Item.Quantity_current);
                Order_itemFound.Order_Item.Quantity_current = 0;
                var Order_itemSavedList = await Bl.BlOrder.UpdateOrder_itemAsync(new List<Order_item> { Order_itemFound.Order_Item });
                Item_ModelDeliveryInProcess.Remove(obj);
                updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
                refreshBindings();
            }
            _page(this);
            Dialog.IsDialogOpen = false;
        }

        private bool canCancelDeliveryReceiptInProcess(Item_deliveryModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void cancelDeliveryReceiptCreated(Item_deliveryModel obj)
        {
            Dialog.showSearch("Delivery receipt created cancelling...");
            
            // Searching the targeting item for processing
            var Order_itemModelTargeted = (from c in Order_ItemModelList
                                          from d in c.ItemModel.Item_deliveryModelList
                                          where d.Item_delivery.DeliveryId == obj.DeliveryModel.Delivery.ID
                                                && d.TxtItem_ref == obj.TxtItem_ref
                                           select c).FirstOrDefault();

            // search of the previous delivery record of the particular item
            var item_deliveryModelPrevious = (from c in Order_ItemModelList
                                        from d in c.ItemModel.Item_deliveryModelList
                                        where   d.Item_delivery.DeliveryId < obj.DeliveryModel.Delivery.ID
                                                && d.Item_delivery.Quantity_delivery < obj.Item_delivery.Quantity_delivery
                                                && d.TxtItem_ref == obj.TxtItem_ref
                                        orderby d.Item_delivery.DeliveryId descending
                                        select d.Item_delivery).FirstOrDefault();

            if (Order_itemModelTargeted != null && Order_itemModelTargeted.TxtQuantity_pending != Order_itemModelTargeted.TxtQuantity)
            {
                // calcul of the quantity delivery for resetting
                int quantityDelivery = obj.Item_delivery.Quantity_delivery - (item_deliveryModelPrevious != null ? item_deliveryModelPrevious.Quantity_delivery : 0);
                
                Order_itemModelTargeted.Order_Item.Quantity_current = quantityDelivery ;

                Order_itemModelTargeted.ItemModel.Item_deliveryModelList.Remove(obj);
                var savedOrder_itemList = await Bl.BlOrder.UpdateOrder_itemAsync(new List<Order_item> { Order_itemModelTargeted.Order_Item });
                await Bl.BlOrder.DeleteDeliveryAsync(new List<Delivery> { obj.DeliveryModel.Delivery });
                await Bl.BlItem.DeleteItem_deliveryAsync(new List<Item_delivery> { obj.Item_delivery });
            }
            else
            {
                Order_itemModelTargeted.ItemModel.Item_deliveryModelList.Remove(obj);
                await Bl.BlOrder.DeleteDeliveryAsync(new List<Delivery> { obj.DeliveryModel.Delivery });
                await Bl.BlItem.DeleteItem_deliveryAsync(new List<Item_delivery> { obj.Item_delivery });
            }

            updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
            refreshBindings();
            Dialog.IsDialogOpen = false;
        }

        private bool canCancelDeliveryReceiptCreated(Item_deliveryModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (!isUpdate)
                return false;

            if (arg != null)
            {
                var item_deliveryModelList = from c in Order_ItemModelList
                                             from d in c.ItemModel.Item_deliveryModelList
                                             where d.TxtItem_ref == arg.TxtItem_ref
                                             orderby d.Item_delivery.Quantity_delivery descending
                                             select d.Item_delivery;

                if (item_deliveryModelList.Where(x => x.Quantity_delivery > arg.Item_delivery.Quantity_delivery).Count() > 0)
                    return false;
            }
            


            return true;
        }

        private async void createBill(Order_itemModel obj)
        {
            Dialog.showSearch("Bill creation...");

            List<Bill> billSavedList = new List<Bill>();
            var Order_itemInProcess = new List<Order_itemModel>();
            Client searchClient = new Client();
            decimal totalInvoiceAmount = 0m;

            // Limit date of payment Calculation
            searchClient.ID = OrderSelected.CLientModel.Client.ID;
            var foundClients = Bl.BlClient.searchClient(searchClient, ESearchOption.AND);
            int payDelay = (foundClients.Count > 0) ? foundClients[0].PayDelay : 0;
            int months = (((DateTime.Now.Day + payDelay) / 30) != 0) ? (DateTime.Now.Day + payDelay) / 30 : 1;
            int days = (((DateTime.Now.Day + payDelay) % 30) != 0) ? (DateTime.Now.Day + payDelay) % 30 : 1;
            DateTime expire = new DateTime(DateTime.Now.Year, DateTime.Now.Month + months, days, 23, 59, 58);

            int first = 0;

            List<Item_deliveryModel> item_deliveryModelToRemoveList = new List<Item_deliveryModel>();

            foreach (var item_deliveryModel in Item_deliveryModelBillingInProcess)
            {
                if (item_deliveryModel.IsSelected)
                {
                    // search of the last inserted bill 
                    Bill lastBill = (await Bl.BlOrder.GetLastBillAsync()) ?? new Bill();

                    if (first == 0)
                    {
                        // Manual incrementation of the bill ID 
                        // to make sure the IDs follow each others                      
                        int billId = lastBill.ID + 1;
                        Bill bill = new Bill();
                        bill.ID = billId;
                        bill.OrderId = OrderSelected.Order.ID;
                        bill.ClientId = OrderSelected.Order.ClientId;
                        bill.Date = DateTime.Now;
                        bill.DateLimit = expire;
                        bill.PayReceived = 0m;

                        // Bill creation
                        billSavedList = await Bl.BlOrder.InsertBillAsync(new List<Bill> { bill });
                        first = 1;
                    }

                    // Update of delivery bill status
                    if (billSavedList.Count > 0)
                    {
                        Order_itemInProcess = Order_ItemModelList.Where(x => x.TxtItem_ref == item_deliveryModel.TxtItem_ref).ToList();
                        //var deliveryModelFoundList = Order_itemInProcess.Select(x => x.ItemModel.Item_deliveryModelList.Where(y => y.DeliveryModel.Delivery.ID == item_deliveryModel.DeliveryModel.Delivery.ID && y.DeliveryModel.TxtStatus == EStatusOrder.Not_Billed.ToString()).Select(z => z.DeliveryModel)).FirstOrDefault().ToList();

                        var deliveryModelFoundList = (from o in Order_itemInProcess
                                                     from i in o.ItemModel.Item_deliveryModelList
                                                     where  i.DeliveryModel.Delivery.ID == item_deliveryModel.DeliveryModel.Delivery.ID 
                                                            && i.DeliveryModel.TxtStatus == EStatusOrder.Not_Billed.ToString()
                                                     select i.DeliveryModel).ToList();

                        // var deliveryModelFoundList = Order_itemInProcess.Where(x => x.ItemModel.Item_deliveryModelList.Where(y=> y.DeliveryModel.Delivery.ID == item_deliveryModel.DeliveryModel.Delivery.ID && y.DeliveryModel.TxtStatus == EStatusOrder.Not_Billed.ToString()).Count() > 0).SelectMany(z=>z.ItemModel.Item_deliveryModelList.Select(w=>w.DeliveryModel)).ToList();

                        foreach (DeliveryModel deliveryModel in deliveryModelFoundList)
                        {
                            if (deliveryModel != null)
                            {
                                deliveryModel.Delivery.Status = EStatusOrder.Billed.ToString();
                                deliveryModel.Delivery.BillId = billSavedList[0].ID;
                                var savedDeliveryList = await Bl.BlOrder.UpdateDeliveryAsync(new List<Delivery> { deliveryModel.Delivery });

                                if (savedDeliveryList.Count > 0)
                                    deliveryModel.Delivery = savedDeliveryList[0];
                            }
                        }    
                        if (Order_itemInProcess.Count > 0)
                            totalInvoiceAmount += Order_itemInProcess[0].Order_Item.Price * item_deliveryModel.Item_delivery.Quantity_delivery;

                        item_deliveryModelToRemoveList.Add(item_deliveryModel);
                    }                    
                }                
            }

            if (first == 1)
            {
                // update of the invoice amount
                var billFound = Bl.BlOrder.searchBill(new Bill { ID = billSavedList[0].ID }, ESearchOption.AND).FirstOrDefault();
                if (billFound != null)
                {
                    billFound.Pay = totalInvoiceAmount;
                    billFound.PayDate = DateTime.Now;
                    var savedBill = await Bl.BlOrder.UpdateBillAsync(new List<Bill> { billFound });
                }
            }

            // removing processed item from the Qeue
            foreach (var item_deliveryModel in item_deliveryModelToRemoveList)
                Item_deliveryModelBillingInProcess.Remove(item_deliveryModel);

            refreshBindings();
            Dialog.IsDialogOpen = false;
        }

        private bool canCreateBill(Order_itemModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            if (Item_deliveryModelBillingInProcess.Count > 0)
                return true;
            return false;
        }

        private async void cancelBillCreated(BillModel obj)
        {
            if (await Dialog.show("do you really want to delete this bill ("+obj.TxtID+")"))
            {
                Dialog.showSearch("Bill cancelling...");

                if (await checkIfLastBillAsync(obj.Bill, offset: 0))
                {
                    List<Bill> billToDelteList = new List<Bill>();
                    List<Delivery> deliveryToUpdateList = new List<Delivery>();

                    // Getting the delivery ID for processing
                    var deliveryModelList = (from c in Order_ItemModelList
                                             from d in c.ItemModel.Item_deliveryModelList
                                             where d.DeliveryModel.TxtBillId == obj.TxtID
                                                     && d.DeliveryModel.TxtStatus == EStatusOrder.Billed.ToString()
                                             select d.DeliveryModel).ToList();

                    foreach (var deliveryModel in deliveryModelList)
                    {
                        deliveryModel.TxtStatus = EStatusOrder.Not_Billed.ToString();
                        deliveryModel.Delivery.BillId = 0;
                        deliveryToUpdateList.Add(deliveryModel.Delivery);
                    }

                    await Bl.BlOrder.DeleteBillAsync(new List<Bill> { obj.Bill });
                    await Bl.BlOrder.UpdateDeliveryAsync(deliveryToUpdateList);
                    OrderSelected.BillModelList.Remove(obj);
                    refreshBindings();
                }
                else
                    await Dialog.show("Cancellation Failed! Order bill is not the latest.");
                Dialog.IsDialogOpen = false;

                refreshBindings();
            }            
        }

        private bool canCancelBillCreated(BillModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            // only the last Invoice can be deleted.
            Bill lastBill = new NotifyTaskCompletion<Bill>(Bl.BlOrder.GetLastBillAsync()).Task.Result;
            if (lastBill.ID == arg.Bill.ID)
                return true;

            return false;
        }

        private async void updateBill(BillModel obj)
        {
            Dialog.showSearch("Bill updating...");
            var savedBillList = await Bl.BlOrder.UpdateBillAsync(new List<Bill> { obj.Bill });
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateBill(BillModel arg)
        {
            return true;
        }


        private void generateOrderBillPdf(BillModel obj)
        {
            Dialog.showSearch("Bill pdf creation...");
            _paramOrderToPdf.BillId = obj.Bill.ID;
            _paramOrderToPdf.OrderId = OrderSelected.Order.ID;
            Bl.BlOrder.GeneratePdfOrder(_paramOrderToPdf);
            Dialog.IsDialogOpen = false;
        }

        private bool canGenerateOrderBillPdf(BillModel arg)
        {
            return true;
        }

        private void generateQuotePdf(object obj)
        {
            Dialog.showSearch("Quote pdf creation...");
            _paramQuoteToPdf.OrderId = OrderSelected.Order.ID;
            Bl.BlOrder.GeneratePdfQuote(_paramQuoteToPdf);
            Dialog.IsDialogOpen = false;
        }

        private bool canGenerateQuotePdf(object arg)
        {
            return true;
        }

        private async void billing(object obj)
        {
            Dialog.showSearch("Billing...");
            updateOrderStatus(EStatusOrder.Bill_Order);
            if (OrderSelected.TxtStatus.Equals(EStatusOrder.Bill_Order.ToString()))
            {
                await Dialog.show("Successfully Billed");
                _page(this);
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canBilling(object arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order_Billed, ESecurity._Update) && _main.securityCheck(EAction.Order_Billed, ESecurity._Update);
            bool isWrite = _main.securityCheck(EAction.Order_Billed, ESecurity._Write);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        private async void selectDeliveryAddress(Address obj)
        {
            if (obj != null)
            {
                Dialog.showSearch("Address updating...");
                OrderSelected.TxtDeliveryAddress = obj.ID.ToString();
                var savedOrderList = await Bl.BlOrder.UpdateOrderAsync(new List<Entity.Order> { OrderSelected.Order });
                if (savedOrderList.Count > 0)
                {
                    var deliveryAddressFoundList = OrderSelected.AddressList.Where(x => x.ID == savedOrderList[0].DeliveryAddress).ToList();
                    OrderSelected.DeliveryAddress = (deliveryAddressFoundList.Count() > 0) ? deliveryAddressFoundList[0] : new Address();
                    await Dialog.show("Delivery Address Successfully Saved!");
                }
                Dialog.IsDialogOpen = false;
            }
        }

        private bool canSelectDeliveryAddress(Address arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void saveNewTax(Tax obj)
        {
            if (obj != null)
            {
                Dialog.showSearch("Tax updating...");
                List<Tax_order> savedOrderList = new List<Tax_order>();
                OrderSelected.Tax = obj;
                OrderSelected.Tax_order.OrderId = OrderSelected.Order.ID;
                OrderSelected.Tax_order.TaxId = obj.ID;
                OrderSelected.Tax_order.Tax_value = obj.Value;
                OrderSelected.Tax_order.Target = EStatusOrder.Order.ToString();
                OrderSelected.Tax_order.Date_insert = DateTime.Now;

                if (OrderSelected.Tax_order.ID == 0)
                    savedOrderList = await Bl.BlOrder.InsertTax_orderAsync(new List<Tax_order> { OrderSelected.Tax_order });
                else
                    savedOrderList = await Bl.BlOrder.UpdateTax_orderAsync(new List<Tax_order> { OrderSelected.Tax_order });

                totalCalcul();
                Dialog.IsDialogOpen = false;
            }
        }

        private bool canSaveNewTax(Tax arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void sendEmail(string obj)
        {
            Dialog.showSearch("Email sending...");
            var paramEmail = new ParamEmail();
            paramEmail.IsCopyToAgent = await Dialog.show("Do you want to receive an copy of the email?");
            paramEmail.Subject = EmailFile.TxtSubject;
            paramEmail.IsSendEmail = true;

            if (EmailFile.TxtFileNameWithoutExtension.Equals("quote"))
            {
                _paramQuoteToPdf.ParamEmail = paramEmail;
                _paramQuoteToPdf.OrderId = OrderSelected.Order.ID;
                Bl.BlOrder.GeneratePdfQuote(_paramQuoteToPdf);
            }
            else
            {
                _paramOrderToPdf.BillId = SelectedBillToSend.Bill.ID;
                _paramOrderToPdf.OrderId = OrderSelected.Order.ID;
                paramEmail.Reminder = 0;
                _paramOrderToPdf.ParamEmail = paramEmail;

                Bl.BlOrder.GeneratePdfOrder(_paramOrderToPdf);
            }

            Dialog.IsDialogOpen = false;
            
        }

        private bool canSendEmail(string arg)
        {
            bool isSendEmailValidOrder = _main.securityCheck(EAction.Order, ESecurity.SendEmail);
            bool isSendEmailValidPreOrder = _main.securityCheck(EAction.Order_Preorder, ESecurity.SendEmail);
            bool isSendEmailQuote = _main.securityCheck(EAction.Quote, ESecurity.SendEmail);

            if (OrderSelected == null)
                return false;

            if (isSendEmailValidPreOrder && OrderSelected.TxtStatus.Equals(EStatusOrder.Pre_Order.ToString()))
                return true;

            if (isSendEmailQuote && OrderSelected.TxtStatus.Equals(EStatusOrder.Quote.ToString()))
                return true;

            if (isSendEmailValidOrder
                && (OrderSelected.TxtStatus.Equals(EStatusOrder.Order.ToString())
                || OrderSelected.TxtStatus.Equals(EStatusOrder.Credit.ToString())))
                return true;

            return false;
        }

        private async void updateComment(object obj)
        {
            Dialog.showSearch("Comment updaing...");
            var savedOrderList = await Bl.BlOrder.UpdateOrderAsync(new List<QOBDCommon.Entities.Order> { OrderSelected.Order });
            if (savedOrderList.Count > 0)
                await Dialog.show("Comment updated successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateComment(object arg)
        {
            return true;
        }

        #endregion

    }

}
