using QOBDBusiness;
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
        private string _title;
        private string _incomeHeaderWithCurrency;
        private decimal _totalBeforeTax;
        private GeneralInfos.FileWriter _mailFile;
        private ParamOrderToPdf _paramQuoteToPdf;
        private ParamOrderToPdf _paramOrderToPdf;
        private ParamDeliveryToPdf _paramDeliveryToPdf;
        public NotifyTaskCompletion<bool> _updateOrderStatusTask { get; set; }
        private Func<object, object> _page;
        private List<Tax> _taxes;
        private EOrderStatus _orderStatus;

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
        private StatisticModel _statistic;
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
        public ButtonCommand<BillModel> SendEmailCommand { get; set; }
        public ButtonCommand<BillModel> UpdateBillCommand { get; set; }
        public ButtonCommand<object> UpdateCommentCommand { get; set; }

        #endregion

        #region [ Contructors]
        public OrderDetailViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
        }

        public OrderDetailViewModel(IMainWindowViewModel main) : this()
        {
            _main = main;
            _page = _main.navigation;
            initEvents();
        }

        #endregion

        #region [ Initialization ]
        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onOrderSelectedChange;
            PropertyChanged += onOrder_itemModelWorkFlowChange;
            _updateOrderStatusTask.PropertyChanged += onInitializationTaskComplete_UpdateOrderStatus;
        }

        private void instances()
        {            
            _title = "Order Description";
            _incomeHeaderWithCurrency = "Total Income (" + CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + ")";
            _taxes = new List<Tax>();
            _statistic = new StatisticModel();
            _selectedBillToSend = new BillModel();
            _updateOrderStatusTask = new NotifyTaskCompletion<bool>();
            _paramDeliveryToPdf = new ParamDeliveryToPdf();
            _paramQuoteToPdf = new ParamOrderToPdf(EOrderStatus.Quote, 2);
            _paramOrderToPdf = new ParamOrderToPdf(EOrderStatus.Order);
            _paramOrderToPdf.Currency = _paramQuoteToPdf.Currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            _paramOrderToPdf.Lang = _paramQuoteToPdf.Lang = _paramDeliveryToPdf.Lang = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault() ?? "en";

            _mailFile = new GeneralInfos.FileWriter("", EOption.mails);
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
            CancelDeliveryReceiptCreationCommand = new ButtonCommand<Item_deliveryModel>(deleteDeliveryReceiptInProcess, canCancelDeliveryReceiptInProcess);
            GenerateDeliveryReceiptCreatedPdfCommand = new ButtonCommand<DeliveryModel>(generateDeliveryReceiptPdf, canGenerateDeliveryReceiptPdf);
            CancelDeliveryReceiptCreatedCommand = new ButtonCommand<Item_deliveryModel>(cancelDeliveryReceiptCreated, canCancelDeliveryReceiptCreated);
            DeliveryReceiptCreationCommand = new ButtonCommand<Order_itemModel>(createDeliveryReceipt, canCreateDeliveryReceipt);
            BillCreationCommand = new ButtonCommand<Order_itemModel>(createInvoice, canCreateBill);
            CancelBillCreatedCommand = new ButtonCommand<BillModel>(deleteCreatedInvoice, canCancelCreatedInvoice);
            GeneratePdfCreatedBillCommand = new ButtonCommand<BillModel>(generateOrderBillPdf, canGenerateOrderBillPdf);
            DeleteItemCommand = new ButtonCommand<Order_itemModel>(deleteItem, canDeleteItem);
            BilledCommand = new ButtonCommand<object>(billing, canBilling);
            DeliveryAddressSelectionCommand = new ButtonCommand<Address>(selectDeliveryAddress, canSelectDeliveryAddress);
            TaxCommand = new ButtonCommand<Tax>(saveNewTax, canSaveNewTax);
            GeneratePdfCreatedQuoteCommand = new ButtonCommand<object>(generateQuotePdf, canGenerateQuotePdf);
            SendEmailCommand = new ButtonCommand<BillModel>(sendEmail, canSendEmail);
            UpdateBillCommand = new ButtonCommand<BillModel>(updateInvoice, canUpdateInvoice);
            UpdateCommentCommand = new ButtonCommand<object>(updateComment, canUpdateComment);
        }
        #endregion

        #region [ Properties ]
        //----------------------------[ Properties ]------------------        
        
        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public BillModel SelectedBillToSend
        {
            get { return _selectedBillToSend; }
            set { setProperty(ref _selectedBillToSend, value); }
        }

        public List<BillModel> BillModelList
        {
            get { return OrderSelected.BillModelList; }
            set { OrderSelected.BillModelList = value; onPropertyChange(); updateItemListBindingByCallingPropertyChange(); }
        }

        public List<DeliveryModel> DeliveryModelList
        {
            get { return OrderSelected.DeliveryModelList; }
            set { OrderSelected.DeliveryModelList = value; onPropertyChange(); }
        }

        public GeneralInfos.FileWriter EmailFile
        {
            get { return _mailFile; }
            set { setProperty(ref _mailFile, value); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public OrderModel OrderSelected
        {
            get { return _orderSelected; }
            set { setProperty(ref _orderSelected, value); }
        }

        public string TxtQuoteValidityInDays
        {
            get { return _paramQuoteToPdf.ValidityDay.ToString(); }
            set { int convertedNumber; if (int.TryParse(value, out convertedNumber)) { _paramQuoteToPdf.ValidityDay = convertedNumber; onPropertyChange(); } }
        }

        public bool IsQuote
        {
            get
            {
                if (_paramQuoteToPdf.TypeQuoteOrProformat == EOrderStatus.Quote)
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _paramQuoteToPdf.TypeQuoteOrProformat = EOrderStatus.Quote;
                    onPropertyChange();
                }
            }
        }

        public bool IsProForma
        {
            get
            {
                if (_paramQuoteToPdf.TypeQuoteOrProformat == EOrderStatus.Proforma)
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _paramQuoteToPdf.TypeQuoteOrProformat = EOrderStatus.Proforma;
                    onPropertyChange();
                }
            }
        }

        public bool IsQuoteReferencesVisible
        {
            get { return _paramQuoteToPdf.IsQuoteConstructorReferencesVisible; }
            set { _paramQuoteToPdf.IsQuoteConstructorReferencesVisible = value; onPropertyChange(); }
        }

        public bool IsOrderReferencesVisible
        {
            get { return _paramOrderToPdf.IsOrderConstructorReferencesVisible; }
            set { _paramOrderToPdf.IsOrderConstructorReferencesVisible = value; onPropertyChange(); }
        }

        public string TxtIncomeHeaderWithCurrency
        {
            get { return _incomeHeaderWithCurrency; }
            set { _incomeHeaderWithCurrency = value; onPropertyChange(); }
        }

        public StatisticModel StatisticModel
        {
            get { return _statistic; }
            set { setProperty(ref _statistic, value); updateStatisticsByCallingPropertyChange(); }
        }

        public string TxtTotalTaxExcluded
        {
            get { return _statistic.TxtTotalTaxExcluded; }
            set { _statistic.TxtTotalTaxExcluded = value; onPropertyChange(); }
        }

        public string TxtTotalIncomePercent
        {
            get { return _statistic.TxtTotalIncomePercent; }
            set { _statistic.TxtTotalIncomePercent = value; onPropertyChange(); }
        }

        public string TxtTotalIncome
        {
            get { return _statistic.TxtTotalIncome; }
            set { _statistic.TxtTotalIncome = value; onPropertyChange(); }
        }

        public string TxtTotalTaxAmount
        {
            get { return _statistic.TxtTotalTaxAmount; }
            set { _statistic.TxtTotalTaxAmount = value; onPropertyChange(); }
        }

        public Tax Tax
        {
            get { return _orderSelected.Tax; }
            set { _orderSelected.Tax = value; onPropertyChange(); }
        }

        public string TxtTotalTaxIncluded
        {
            get { return _statistic.TxtTotalTaxIncluded; }
            set { setProperty(ref _totalBeforeTax, Convert.ToDecimal(value)); }
        }

        public string TxtTotalPurchase
        {
            get { return _statistic.TxtTotalPurchase; }
            set { _statistic.TxtTotalPurchase = value; onPropertyChange(); }
        }

        public List<Order_itemModel> Order_ItemModelList
        {
            get { return _order_itemList; }
            set { _order_itemList = value; refreshBindingByCallingPropertyChange(); }
        }

        public List<Item_deliveryModel> Item_ModelDeliveryInProcess
        {
            get { return _item_ModelDeliveryInProcess; }
            set { setProperty(ref _item_ModelDeliveryInProcess, value); updateItemListBindingByCallingPropertyChange(); }
        }

        public List<Item_deliveryModel> Item_deliveryModelCreatedList
        {
            get { return _item_deliveryModelCreatedList; }
            set { setProperty(ref _item_deliveryModelCreatedList, value); }
        }

        public List<Item_deliveryModel> Item_deliveryModelBillingInProcess
        {
            get { return _item_deliveryModelBillingInProcessList; }
            set { setProperty(ref _item_deliveryModelBillingInProcessList, value); BillCreationCommand.raiseCanExecuteActionChanged(); onPropertyChange("Item_deliveryModelBillingInProcessSelectionList"); updateItemListBindingByCallingPropertyChange(); }
        }

        public List<Item_deliveryModel> Item_deliveryModelBillingInProcessSelectionList
        {
            get { return Item_deliveryModelBillingInProcess.GroupBy(x => x.Item_delivery.DeliveryId).Select(x => x.First()).ToList(); }
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

        /// <summary>
        /// load the data
        /// </summary>
        public void loadOrder_items()
        {
            Dialog.showSearch("Loading items...");
            Order_ItemModelList = Order_ItemListToModelViewList(Bl.BlOrder.searchOrder_item(new Order_item { OrderId = OrderSelected.Order.ID }, ESearchOption.AND));

            StatisticModel = totalCalcul(Order_ItemModelList);
            refreshBindings();
            loadEmail();
            Dialog.IsDialogOpen = false;

        }

        /// <summary>
        /// gathering order item information
        /// </summary>
        /// <param name="Order_ItemList"></param>
        /// <returns></returns>
        public List<Order_itemModel> Order_ItemListToModelViewList(List<Order_item> Order_ItemList)
        {
            List<Order_itemModel> output = new List<Order_itemModel>();
            foreach (Order_item order_Item in Order_ItemList)
            {
                Order_itemModel localOrder_item = new Order_itemModel();
                localOrder_item.PropertyChanged += onTotalSelling_PriceOrPrice_purchaseChange;
                localOrder_item.Order_Item = order_Item;

                //load item and its inforamtion (delivery and item_delivery)
                localOrder_item.ItemModel = loadOrder_itemItem(order_Item.Item_ref, order_Item.ItemId);

                output.Add(localOrder_item);
            }
            return output;
        }

        /// <summary>
        /// loading the items
        /// </summary>
        /// <param name="itemRef"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ItemModel loadOrder_itemItem(string itemRef, int itemId = 0)
        {
            List<Item> itemFoundList = new List<Item>();
            if(itemId != 0)
                itemFoundList = Bl.BlItem.searchItem(new Item { Ref = itemRef , ID = itemId }, ESearchOption.AND);
            else
                itemFoundList = Bl.BlItem.searchItem(new Item { Ref = itemRef }, ESearchOption.AND);

            if (itemFoundList.Count > 0)
                return ItemListToModelViewList(new List<Item> { itemFoundList[0] }).FirstOrDefault();
            return new ItemModel();
        }

        /// <summary>
        /// gathering item information
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        private List<ItemModel> ItemListToModelViewList(List<Item> itemList)
        {
            List<ItemModel> output = new List<ItemModel>();
            foreach (Item item in itemList)
            {
                ItemModel itemModel = new ItemModel();
                itemModel.Item = item;

                // search for the item's delivery receipt
                var deliveryFoundList = Bl.BlOrder.searchDelivery(new Delivery { OrderId = OrderSelected.Order.ID }, ESearchOption.AND);
                List<Item_deliveryModel> item_deliveryModelList = new List<Item_deliveryModel>();
                foreach (var delivery in deliveryFoundList)
                {
                    // search for the delivery reference of the item
                    Item_deliveryModel item_deliveryModelFound = Bl.BlItem.searchItem_delivery(new Item_delivery { Item_ref = item.Ref, DeliveryId = delivery.ID }, ESearchOption.AND).Select(x => new Item_deliveryModel { Item_delivery = x, ItemModel = new ItemModel { Item = item }, DeliveryModel = new DeliveryModel { Delivery = delivery } }).FirstOrDefault();
                    if (item_deliveryModelFound != null)
                    {
                        item_deliveryModelList.Add(item_deliveryModelFound);
                        break;
                    }                        
                }                                
                itemModel.Item_deliveryModelList = item_deliveryModelList;
                output.Add(itemModel);
            }
            return output;
        }

        /// <summary>
        /// download the email templates form the ftp host
        /// </summary>
        public void loadEmail()
        {
            if (OrderSelected != null)
            {
                var infos = Bl.BlReferential.searchInfo(new Info { Name = "Company_name" }, ESearchOption.AND).FirstOrDefault();
                var infosFTP = Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_" }, ESearchOption.AND).ToList();
                string login = infosFTP.Where(x => x.Name == "ftp_login").Select(x => x.Value).FirstOrDefault() ?? "";
                string password = infosFTP.Where(x => x.Name == "ftp_password").Select(x => x.Value).FirstOrDefault() ?? "";
                switch (OrderSelected.TxtStatus)
                {
                    case "Quote":
                        EmailFile = new GeneralInfos.FileWriter("quote", EOption.mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Quote n°{QUOTE_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Quote n°{QUOTE_ID} **";
                        break;
                    case "Pre_Order":
                        EmailFile = new GeneralInfos.FileWriter("order_confirmation", EOption.mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Invoice n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Order n°{BILL_ID} **";
                        break;
                    case "Pre_Credit":
                        EmailFile = new GeneralInfos.FileWriter("order_confirmation", EOption.mails, ftpLogin: login, ftpPassword: password);
                        EmailFile.read();
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Credit with Invoice n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Credit with Invoice n°{BILL_ID} **";
                        break;
                    case "Order":
                        EmailFile = new GeneralInfos.FileWriter("bill", EOption.mails, ftpLogin: login, ftpPassword: password);
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
        /// refresh the data
        /// </summary>
        private void refreshBindings()
        {
            loadInvoicesAndDeliveryReceipts();

            // check that the item in the list is erasable
            DeleteItemCommand.raiseCanExecuteActionChanged();
        }


        /// <summary>
        /// load all bills of the selected Order
        /// </summary>
        public void loadInvoicesAndDeliveryReceipts()
        {
            // unsucribe events select/unselect items with the same delivery receipt
            foreach (Item_deliveryModel item_deliveryModel in Item_deliveryModelBillingInProcess)
                item_deliveryModel.PropertyChanged -= onItem_ModelDeliveryInProcessIselectedChanged;

            // get the invoice creation list
            Item_deliveryModelBillingInProcess = (from c in Order_ItemModelList
                                                  from d in c.ItemModel.Item_deliveryModelList
                                                  where d.DeliveryModel.TxtStatus == EOrderStatus.Not_Billed.ToString()
                                                  select d).ToList();

            // select/unselect all items with the same delivery receipt
            foreach (Item_deliveryModel item_deliveryModel in Item_deliveryModelBillingInProcess)
                item_deliveryModel.PropertyChanged += onItem_ModelDeliveryInProcessIselectedChanged;

            // get the delivery creation list
            Item_ModelDeliveryInProcess = Order_ItemModelList.Where(x => x.Order_Item.Quantity_current > 0).Select(x => new Item_deliveryModel
            {
                ItemModel = x.ItemModel,
                TxtItem_ref = x.ItemModel.TxtRef,
                TxtQuantity_current = x.TxtQuantity_current,
                TxtQuantity_delivery = x.TxtQuantity_delivery
            }).ToList();

            // get the created bills
            BillModelList = new BillModel().BillListToModelViewList(Bl.BlOrder.searchBill(new Bill { OrderId = OrderSelected.Order.ID }, ESearchOption.AND));

            // get the created delivery list
            DeliveryModelList = new DeliveryModel().DeliveryListToModelViewList(Bl.BlOrder.searchDelivery(new Delivery { OrderId = OrderSelected.Order.ID }, ESearchOption.AND));

        }

        /// <summary>
        /// calcul the total amount of the order
        /// </summary>
        /// <param name="order_itemList"></param>
        /// <returns></returns>
        private StatisticModel totalCalcul(List<Order_itemModel> order_itemList)
        {
            StatisticModel statistic = new StatisticModel();

            if (Order_ItemModelList.Count > 0)
            {
                decimal totalProfit = 0.0m;
                decimal totalTaxExcluded = 0.0m;
                decimal totalTaxIncluded = 0.0m;
                decimal totalPurchase = 0.0m;

                foreach (Order_itemModel order_itemModel in order_itemList)
                {
                    var order_item = order_itemModel.Order_Item;
                    totalProfit += order_item.Quantity * (Math.Abs(order_item.Price) - Math.Abs(order_item.Price_purchase));
                    totalTaxExcluded += Math.Abs(Utility.decimalTryParse(order_itemModel.TxtTotalSelling));
                    totalPurchase += Math.Abs(Utility.decimalTryParse(order_itemModel.TxtTotalPurchase));
                }
                totalTaxIncluded = ((decimal)OrderSelected.Tax_order.Tax_value / 100) * totalTaxExcluded;

                // convert into credit regarding the order status
                totalTaxIncluded *= (OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString())) ? -1 : 1;
                totalProfit *= (OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString())) ? -1 : 1;
                totalTaxExcluded *= (OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString())) ? -1 : 1;
                totalPurchase *= (OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString())) ? -1 : 1;

                statistic.TxtTotalTaxAmount = totalTaxIncluded.ToString();
                statistic.TxtTotalIncome = totalProfit.ToString();
                statistic.TxtTotalTaxExcluded = totalTaxExcluded.ToString();
                statistic.TxtTotalTaxIncluded = string.Format("{0:0.00}", (totalTaxExcluded + (totalTaxExcluded) * ((decimal)OrderSelected.Tax_order.Tax_value) / 100));
                try
                {
                    statistic.TxtTotalIncomePercent = string.Format("{0:0.00}", ((totalProfit / totalTaxExcluded) * 100));
                }
                catch (DivideByZeroException) { this.TxtTotalIncomePercent = "0"; }                

                statistic.TxtTotalPurchase = string.Format("{0:0.00}", totalPurchase);
                statistic.TxtTaxValue = Tax.Value.ToString();
            }
            return statistic;
        }



        /// <summary>
        /// load the client addresses
        /// </summary>
        private void loadAddresses()
        {
            OrderSelected.AddressList = Bl.BlClient.searchAddress(new Address { ClientId = OrderSelected.CLientModel.Client.ID }, ESearchOption.AND);
        }

        /// <summary>
        /// update the order status
        /// </summary>
        /// <param name="status">the status to convert to</param>
        public void updateOrderStatus(EOrderStatus status)
        {
            Dialog.showSearch("Processing...");
            _updateOrderStatusTask.initializeNewTask(updateOrderStatusAsync(status));
        }

        /// <summary>
        /// update the order status async
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> updateOrderStatusAsync(EOrderStatus status)
        {
            bool canChangeStatus = true;
            _orderStatus = status;
            switch (_orderStatus)
            {
                case EOrderStatus.Order:
                    lockOrder_itemItems();
                    break;
                case EOrderStatus.Quote:
                    canChangeStatus = await cleanUpBeforeConvertingToQuoteAsync();
                    break;
                case EOrderStatus.Billed:
                    break;
                case EOrderStatus.Bill_Order:
                    break;
                case EOrderStatus.Bill_Credit:
                    break;
                case EOrderStatus.Pre_Order:
                    break;
                case EOrderStatus.Pre_Credit:
                    createCredit();
                    break;
                case EOrderStatus.Order_Close:
                    canChangeStatus = await Dialog.showAsync("Order Closing: Be careful as it will not be possible to do any change after.");
                    break;
                case EOrderStatus.Credit_CLose:
                    canChangeStatus = await Dialog.showAsync("Credit CLosing: Be careful as it will not be possible to do any change after.");
                    break;
            }
            return canChangeStatus;
        }

        /// <summary>
        /// check that the invoice in parameter is the latest one
        /// in order to prevent holes in the invoice IDs
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<bool> checkIfLastBillAsync(Bill bill, int offset)
        {
            bool isLastBill = false;
            Bill lastBill = await Bl.BlOrder.GetLastBillAsync();

            if (lastBill != null)
            {
                if (lastBill.ID - offset <= bill.ID)
                    isLastBill = true;
                else
                    isLastBill = false;
            }
            return isLastBill;
        }

        /// <summary>
        /// delete the information relative to an order
        /// before converting back to quote
        /// </summary>
        /// <returns></returns>
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
                if (OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                    createCredit(isReset: true);

                canDelete = true;
            }
            return canDelete;
        }

        /// <summary>
        /// prevent erasing items in used in any order
        /// </summary>
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

        /// <summary>
        /// convert the order into credit
        /// </summary>
        /// <param name="isReset"></param>
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

        /// <summary>
        /// disable IU item regarding the order status
        /// </summary>
        /// <param name="obj">the item to disable</param>
        /// <returns>boolean type expected by the IU</returns>
        private bool disableUIElementByBoolean([CallerMemberName]string obj = "")
        {
            // Lock order when all invoices have been generated
            if ((OrderSelected.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()) || OrderSelected.TxtStatus.Equals(EOrderStatus.Bill_Credit.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityReceivedTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            // Prevent updating information when the order has been closed
            if ((OrderSelected.TxtStatus.Equals(EOrderStatus.Order_Close.ToString()) || OrderSelected.TxtStatus.Equals(EOrderStatus.Credit_CLose.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityReceivedTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            // Lock items information when an invoice has been created
            if ((BillModelList.Count > 0)
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            // Prevent updating the items quantity when delivering receipt creation process has started
            if ((Item_deliveryModelBillingInProcess.Count > 0 || Item_ModelDeliveryInProcess.Count > 0)
                && obj.Equals("IsItemListQuantityTextBoxEnable"))
                return false;

            return true;
        }

        /// <summary>
        /// disable IU item regarding the order status
        /// </summary>
        /// <param name="obj">the item to disable</param>
        /// <returns>string type expected by the IU</returns>
        private string disableUIElementByString([CallerMemberName]string obj = "")
        {

            if (!OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString())
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Collapsed";

            // Show order details when converted into order
            else if (OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString())
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Visible";

            if ((!OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString()) && !OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                && (obj.Equals("BlockDeliveryListToIncludeVisibility")
                || obj.Equals("BlockStepOneVisibility")
                || obj.Equals("BlockStepTwoVisibility")
                || obj.Equals("BlockStepThreeVisibility")))
                return "Hidden";

            if (OrderSelected.TxtStatus.Equals(EOrderStatus.Quote.ToString())
                && (obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                || obj.Equals("BlockBillListBoxVisibility")
                ))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString()) || OrderSelected.TxtStatus.Equals(EOrderStatus.Pre_Credit.ToString()))
                && (obj.Equals("BlockEmailVisibility")
                || obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                ))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString()) || OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                && (obj.Equals("BlockStepOneVisibility") && Item_ModelDeliveryInProcess.Count == 0
                || obj.Equals("BlockStepTwoVisibility") && Item_deliveryModelBillingInProcess.Count == 0
                || obj.Equals("BlockStepThreeVisibility") && OrderSelected.BillModelList.Count == 0
                ))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()) || OrderSelected.TxtStatus.Equals(EOrderStatus.Bill_Credit.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            if ((OrderSelected.TxtStatus.Equals(EOrderStatus.Order_Close.ToString()) || OrderSelected.TxtStatus.Equals(EOrderStatus.Credit_CLose.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            return "Visible";
        }

        /// <summary>
        /// fire the IU data refresh events
        /// </summary>
        private void updateStepBinding()
        {
            onPropertyChange("BlockStepOneVisibility");
            onPropertyChange("BlockStepTwoVisibility");
            onPropertyChange("BlockStepThreeVisibility");
        }

        /// <summary>
        /// fire the IU data refresh events
        /// </summary>
        private void refreshBindingByCallingPropertyChange()
        {
            onPropertyChange("Order_ItemModelList");
            onPropertyChange("Item_deliveryModelCreatedList");
            onPropertyChange("Item_ModelDeliveryInProcess");
            onPropertyChange("Item_deliveryModelBillingInProcess");
        }

        /// <summary>
        /// fire the IU data refresh events
        /// </summary>
        private void updateItemListBindingByCallingPropertyChange()
        {
            onPropertyChange("IsItemListCommentTextBoxEnabled");
            onPropertyChange("IsItemListQuantityTextBoxEnable");
            onPropertyChange("IsItemListSellingPriceTextBoxEnable");
            onPropertyChange("IsItemListPurchasePriceTextBoxEnable");
        }

        /// <summary>
        /// fire the IU data refresh events
        /// </summary>
        private void updateStatisticsByCallingPropertyChange()
        {
            onPropertyChange("TxtTotalPurchase");
            onPropertyChange("TxtTotalTaxIncluded");
            onPropertyChange("TxtTotalTaxAmount");
            onPropertyChange("TxtTotalIncome");
            onPropertyChange("TxtTotalIncomePercent");
            onPropertyChange("TxtTotalTaxExcluded");
        }

        /// <summary>
        /// unsuscribe events and dispose
        /// </summary>
        public override void Dispose()
        {
            PropertyChanged -= onOrderSelectedChange;
            PropertyChanged -= onOrder_itemModelWorkFlowChange;
            _updateOrderStatusTask.PropertyChanged -= onInitializationTaskComplete_UpdateOrderStatus;

            foreach (var Order_itemModel in Order_ItemModelList)
                Order_itemModel.PropertyChanged -= onTotalSelling_PriceOrPrice_purchaseChange;

            foreach (Item_deliveryModel item_deliveryModel in Item_deliveryModelBillingInProcess)
                item_deliveryModel.PropertyChanged -= onItem_ModelDeliveryInProcessIselectedChanged;

            foreach (Order_itemModel order_item in Order_ItemModelList)
                order_item.Dispose();

            Bl.BlOrder.Dispose();
        }

        #endregion

        #region [ Event Handler ]
        //----------------------------[ Event Handler ]------------------

        /// <summary>
        /// event listener to load the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onOrderSelectedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("OrderSelected"))
            {
                loadOrder_items();
                loadAddresses();
            }
        }

        /// <summary>
        /// event listener to calculate the total amount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onTotalSelling_PriceOrPrice_purchaseChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "TxtTotalSelling") || string.Equals(e.PropertyName, "TxtPrice") || string.Equals(e.PropertyName, "TxtPrice_purchase"))
                StatisticModel = totalCalcul(Order_ItemModelList);
        }

        /// <summary>
        /// event listener to update the binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onOrder_itemModelWorkFlowChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Item_ModelDeliveryInProcess")
                || e.PropertyName.Equals("Item_deliveryModelBillingInProcess")
                || e.PropertyName.Equals("BillModelList"))
            {
                updateStepBinding();
            }
        }

        /// <summary>
        /// event listener to update the order status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        await Dialog.showAsync(oldStatus + " successfully Converted to " + OrderSelected.TxtStatus);
                        loadEmail();
                        _page(this);
                    }
                    else
                        await Dialog.showAsync("Convertion to " + OrderSelected.TxtStatus + " Failed!");
                }
                else
                    await Dialog.showAsync("Convertion to " + _orderStatus + " Failed! " +
                        Environment.NewLine + "Please make sure that this order bill is the latest.");
                Dialog.IsDialogOpen = false;
            }
            else if (e.PropertyName.Equals("Exception"))
            {
                Dialog.IsDialogOpen = false;
                Dialog.showSearch("Oops! an error occurred while processing your request." +
                    Environment.NewLine + "Please contact your administrator.");
                Log.error("Error while updating the order(ID=+" + OrderSelected.TxtID + ") from " + OrderSelected.TxtStatus + "" + _orderStatus.ToString());
            }

        }

        /// <summary>
        /// when one item of a delivery receipt is selected 
        /// then select all items of the delivery receipt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onItem_ModelDeliveryInProcessIselectedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSelected"))
            {
                foreach (var item_deliveryModel in Item_deliveryModelBillingInProcess.Where(x => x.Item_delivery.DeliveryId == ((Item_deliveryModel)sender).Item_delivery.DeliveryId).ToList())
                    item_deliveryModel.updateIselected(((Item_deliveryModel)sender).IsSelected);
                refreshBindings();
            }
        }

        #endregion

        #region [ Actions Command ]

        //----------------------------[ Action Command ]------------------

        /// <summary>
        /// update the order items
        /// </summary>
        /// <param name="obj"></param>
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
                        
            refreshBindingByCallingPropertyChange();
            refreshBindings();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        /// <summary>
        /// check that all requirements are respected in order to update the order items
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canUpdateOrder_itemData(string arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (!isUpdate)
                return false;

            if (string.IsNullOrEmpty(OrderSelected.TxtStatus)
                || !OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString())
                && !OrderSelected.TxtStatus.Equals(EOrderStatus.Quote.ToString())
                && !OrderSelected.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString())
                && !OrderSelected.TxtStatus.Equals(EOrderStatus.Pre_Credit.ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// delete one item from the order
        /// </summary>
        /// <param name="obj"></param>
        private async void deleteItem(Order_itemModel obj)
        {
            Dialog.showSearch("Deleting...");
            await Bl.BlOrder.DeleteOrder_itemAsync(new List<Order_item> { obj.Order_Item });
            Order_ItemModelList.Remove(obj);

            // refresh binding
            onPropertyChange("Order_ItemModelList");

            Dialog.IsDialogOpen = false;
            _page(this);
        }

        /// <summary>
        /// check that all requirements are respected in order to delete one item from the order
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canDeleteItem(Order_itemModel arg)
        {
            bool isDelete = _main.securityCheck(EAction.Order, ESecurity._Delete);
            if (!isDelete)
                return false;

            if (Item_ModelDeliveryInProcess.Count != 0
                || Item_deliveryModelBillingInProcess.Count != 0
                || OrderSelected.BillModelList.Count != 0)
                return false;

            return true;
        }

        /// <summary>
        /// generate the pdf document of the delivery receipt
        /// </summary>
        /// <param name="obj"></param>
        private void generateDeliveryReceiptPdf(DeliveryModel obj)
        {
            Dialog.showSearch("Pdf creation...");
            _paramDeliveryToPdf.OrderId = OrderSelected.Order.ID;
            _paramDeliveryToPdf.DeliveryId = obj.Delivery.ID;
            Bl.BlOrder.GeneratePdfDelivery(_paramDeliveryToPdf);
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all requirements are respected in order to generate the pdf document
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canGenerateDeliveryReceiptPdf(DeliveryModel arg)
        {
            return true;
        }

        /// <summary>
        /// create a delivery receipt
        /// </summary>
        /// <param name="obj"></param>
        public async void createDeliveryReceipt(Order_itemModel obj)
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
                        delivery.Status = EOrderStatus.Not_Billed.ToString();
                        delivery.Package = item_deliveryModel.DeliveryModel.Delivery.Package;
                        insertNewDeliveryList.Add(delivery);
                        savedDeliveryList = await Bl.BlOrder.InsertDeliveryAsync(insertNewDeliveryList);
                        first++;
                    }

                    // creation of the reference of the delivery created inside item_delivery
                    if (savedDeliveryList.Count > 0)
                    {
                        item_deliveryModel.Item_delivery.DeliveryId = savedDeliveryList[0].ID;
                        var savedItem_deliveryList = await Bl.BlItem.InsertItem_deliveryAsync(new List<Item_delivery> { item_deliveryModel.Item_delivery });

                        var Order_itemModelFound = (from c in Order_ItemModelList
                                                    where c.ItemModel.Item.ID == item_deliveryModel.ItemModel.Item.ID && c.ItemModel.Item.Ref == item_deliveryModel.ItemModel.TxtRef
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

        /// <summary>
        /// check that all requirements are respected in order to create a delivery receipt
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canCreateDeliveryReceipt(Order_itemModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        /// <summary>
        /// remove the delivery receipt from the list of delivery in process
        /// </summary>
        /// <param name="obj">the delivery receipt to process</param>
        private async void deleteDeliveryReceiptInProcess(Item_deliveryModel obj)
        {
            Dialog.showSearch("Delivery receipt creation cancelling...");

            var Order_itemFound = (from c in Order_ItemModelList
                                   where c.ItemModel.Item.Ref == obj.ItemModel.TxtRef && c.ItemModel.Item.ID == obj.ItemModel.Item.ID
                                   select c).FirstOrDefault();

            if (Order_itemFound != null)
            {
                // cancelling the receiption of items
                Order_itemFound.Order_Item.Quantity_delivery = Math.Max(0, Order_itemFound.Order_Item.Quantity_delivery - Order_itemFound.Order_Item.Quantity_current);
                Order_itemFound.Order_Item.Quantity_current = 0;
                var Order_itemSavedList = await Bl.BlOrder.UpdateOrder_itemAsync(new List<Order_item> { Order_itemFound.Order_Item });

                // remove the delivery receipt.
                Item_ModelDeliveryInProcess.Remove(obj);

                // update the bindings
                refreshBindingByCallingPropertyChange();
                refreshBindings();
            }
            _page(this);
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all requirements are respected in order to delete the deleivery receipt in process
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canCancelDeliveryReceiptInProcess(Item_deliveryModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        /// <summary>
        /// delete on delivery receipt of the order
        /// </summary>
        /// <param name="obj">the delivery receipt to process</param>
        private async void cancelDeliveryReceiptCreated(Item_deliveryModel obj)
        {
            Dialog.showSearch("Delivery receipt created cancelling...");

            // Searching the targeting item for processing
            var Order_itemModelTargeted = (from c in Order_ItemModelList
                                           from d in c.ItemModel.Item_deliveryModelList
                                           where d.Item_delivery.DeliveryId == obj.DeliveryModel.Delivery.ID
                                                 && d.TxtItem_ref == obj.TxtItem_ref
                                           select c).FirstOrDefault();

            // getting the previous delivery receipt of the targeted item
            var item_deliveryModelPrevious = (from c in Order_ItemModelList
                                              from d in c.ItemModel.Item_deliveryModelList
                                              where d.Item_delivery.DeliveryId < obj.DeliveryModel.Delivery.ID
                                                      && d.Item_delivery.Quantity_delivery < obj.Item_delivery.Quantity_delivery
                                                      && d.TxtItem_ref == obj.TxtItem_ref
                                              orderby d.Item_delivery.DeliveryId descending
                                              select d.Item_delivery).FirstOrDefault();

            if (Order_itemModelTargeted != null && Order_itemModelTargeted.TxtQuantity_pending != Order_itemModelTargeted.TxtQuantity)
            {
                // calcul of the quantity delivery for resetting
                int quantityDelivery = obj.Item_delivery.Quantity_delivery - (item_deliveryModelPrevious != null ? item_deliveryModelPrevious.Quantity_delivery : 0);

                // push this item back to delivery creation list
                Order_itemModelTargeted.Order_Item.Quantity_current += quantityDelivery;
                Order_itemModelTargeted.ItemModel.Item_deliveryModelList.Remove(obj);
                var savedOrder_itemList = await Bl.BlOrder.UpdateOrder_itemAsync(new List<Order_item> { Order_itemModelTargeted.Order_Item });

                // deldete any delivery receipt regarding this item
                await Bl.BlOrder.DeleteDeliveryAsync(new List<Delivery> { obj.DeliveryModel.Delivery });
                await Bl.BlItem.DeleteItem_deliveryAsync(new List<Item_delivery> { obj.Item_delivery });
            }

            // otherwise delete any delivery receipt created by mistake
            else
            {
                Order_itemModelTargeted.ItemModel.Item_deliveryModelList.Remove(obj);
                await Bl.BlOrder.DeleteDeliveryAsync(new List<Delivery> { obj.DeliveryModel.Delivery });
                await Bl.BlItem.DeleteItem_deliveryAsync(new List<Item_delivery> { obj.Item_delivery });
            }

            refreshBindingByCallingPropertyChange();
            refreshBindings();
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all requirements are respected in order to delete the deleivery receipt
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
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

        /// <summary>
        /// create one invoice of the order
        /// </summary>
        /// <param name="obj"></param>
        private async void createInvoice(Order_itemModel obj)
        {
            Dialog.showSearch("Invoice creation...");

            List<Bill> invoiceSavedList = new List<Bill>();
            var Order_itemInProcess = new List<Order_itemModel>();
            Client searchClient = new Client();
            decimal totalInvoiceAmount = 0m;

            // Limit date of payment Calculation
            searchClient.ID = OrderSelected.CLientModel.Client.ID;
            var foundClients = Bl.BlClient.searchClient(searchClient, ESearchOption.AND);
            int payDelay = (foundClients.Count > 0) ? foundClients[0].PayDelay : 0;
            DateTime expire = DateTime.Now.AddDays(payDelay);
            
            int first = 0;

            List<Item_deliveryModel> item_deliveryModelToRemoveList = new List<Item_deliveryModel>();

            foreach (var item_deliveryModel in _item_deliveryModelBillingInProcessList)
            {
                if (item_deliveryModel.IsSelected)
                {
                    // search of the last inserted bill 
                    Bill lastInvoice = (await Bl.BlOrder.GetLastBillAsync()) ?? new Bill();

                    if (first == 0)
                    {
                        // Manual incrementation of the bill ID 
                        // to make sure that the IDs follow each others                      
                        int billId = lastInvoice.ID + 1;
                        Bill invoice = new Bill();
                        invoice.ID = billId;
                        invoice.OrderId = OrderSelected.Order.ID;
                        invoice.ClientId = OrderSelected.Order.ClientId;
                        invoice.Date = DateTime.Now;
                        invoice.DateLimit = expire;
                        invoice.PayReceived = 0m;

                        // Bill creation
                        invoiceSavedList = await Bl.BlOrder.InsertBillAsync(new List<Bill> { invoice });
                        first = 1;
                    }

                    // Update of delivery bill status
                    if (invoiceSavedList.Count > 0)
                    {
                        Order_itemInProcess = Order_ItemModelList.Where(x => x.TxtItem_ref == item_deliveryModel.TxtItem_ref).ToList();
                        
                        var deliveryModelFoundList = (from o in Order_itemInProcess
                                                      from i in o.ItemModel.Item_deliveryModelList
                                                      where i.DeliveryModel.Delivery.ID == item_deliveryModel.DeliveryModel.Delivery.ID
                                                             && i.DeliveryModel.TxtStatus == EOrderStatus.Not_Billed.ToString()
                                                      select i.DeliveryModel).ToList();

                        foreach (DeliveryModel deliveryModel in deliveryModelFoundList)
                        {
                            if (deliveryModel != null)
                            {
                                deliveryModel.Delivery.Status = EOrderStatus.Billed.ToString();
                                deliveryModel.Delivery.BillId = invoiceSavedList[0].ID;
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

            var invoicelFound = Bl.BlOrder.searchBill(new Bill { ID = invoiceSavedList[0].ID }, ESearchOption.AND).FirstOrDefault();

            if (first == 1)
            {
                // update of the invoice amount                
                if (invoicelFound != null)
                {
                    invoicelFound.Pay = totalInvoiceAmount;
                    invoicelFound.PayDate = DateTime.Now;
                    var savedInvoice = await Bl.BlOrder.UpdateBillAsync(new List<Bill> { invoicelFound });
                }
            }

            Dialog.showSearch("Invoice: creating record in statistics...");

            // calcul and  creating the statistics              
            var order_itemFoundList = Order_ItemModelList.GroupBy(x => x.TxtItem_ref).Select(x => x.First()).Where(x => x.ItemModel.Item_deliveryModelList.Where(y => y.DeliveryModel.Delivery.BillId == invoicelFound.ID).Count() > 0).ToList();
            if (order_itemFoundList.Count > 0)
            {
                StatisticModel statisticModel = new StatisticModel();
                statisticModel = totalCalcul(order_itemFoundList);
                statisticModel.Statistic.InvoiceDate = invoicelFound.Date;
                statisticModel.Statistic.Date_limit = invoicelFound.DateLimit;
                statisticModel.Statistic.InvoiceId = (invoicelFound != null) ? invoicelFound.ID : 0;
                statisticModel.TxtCompanyName = (!string.IsNullOrEmpty(OrderSelected.CLientModel.TxtCompany)) ? OrderSelected.CLientModel.TxtCompany : OrderSelected.CLientModel.TxtCompanyName;

                // statistics saving
                var savedstatisticsList = await Bl.BlStatisitc.InsertStatisticAsync(new List<Statistic> { statisticModel.Statistic });
            }

            // removing processed item from the Qeue
            foreach (var item_deliveryModel in item_deliveryModelToRemoveList)
                _item_deliveryModelBillingInProcessList.Remove(item_deliveryModel);

            refreshBindings();
            Dialog.IsDialogOpen = false;

            // once the invoice created enable the email sending
            SendEmailCommand.raiseCanExecuteActionChanged();
        }


        /// <summary>
        /// check that all requirements are respected in order to create an invoice
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canCreateBill(Order_itemModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            if (Item_deliveryModelBillingInProcess.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// delete one of the order created invoice
        /// </summary>
        /// <param name="obj">the invoice to process</param>
        private async void deleteCreatedInvoice(BillModel obj)
        {
            if (await Dialog.showAsync("do you really want to delete this invoice (" + obj.TxtID + ")"))
            {
                Dialog.showSearch("Invoice cancelling...");

                if (await checkIfLastBillAsync(obj.Bill, offset: 0))
                {
                    List<Bill> billToDelteList = new List<Bill>();
                    List<Delivery> deliveryToUpdateList = new List<Delivery>();

                    // Getting the delivery ID for processing
                    var deliveryModelList = (from c in Order_ItemModelList
                                             from d in c.ItemModel.Item_deliveryModelList
                                             where d.DeliveryModel.TxtBillId == obj.TxtID
                                                     && d.DeliveryModel.TxtStatus == EOrderStatus.Billed.ToString()
                                             select d.DeliveryModel).ToList();

                    foreach (var deliveryModel in deliveryModelList)
                    {
                        deliveryModel.TxtStatus = EOrderStatus.Not_Billed.ToString();
                        deliveryModel.Delivery.BillId = 0;
                        deliveryToUpdateList.Add(deliveryModel.Delivery);
                    }

                    await Bl.BlOrder.DeleteBillAsync(new List<Bill> { obj.Bill });
                    var updatedDeliveryList = await Bl.BlOrder.UpdateDeliveryAsync(deliveryToUpdateList);
                    if (updatedDeliveryList.Count == 0 && deliveryToUpdateList.Count > 0)
                    {
                        string errorMessage = "Error occurred while cancelling the invoice (ID="+obj.TxtID+").";
                        Log.error(errorMessage);
                        await Dialog.showAsync(errorMessage);
                    }                        

                    // deleting the related statistics
                    var statisticsFoundList = await Bl.BlStatisitc.searchStatisticAsync(new Statistic { InvoiceId = obj.Bill.ID }, ESearchOption.AND);
                    if (statisticsFoundList.Count > 0)
                        await Bl.BlStatisitc.DeleteStatisticAsync(new List<Statistic> { statisticsFoundList[0] });

                    // delete the invoice notification information
                    var NotificationFoundList = await Bl.BlNotification.searchNotificationAsync(new Notification { BillId = obj.Bill.ID }, ESearchOption.AND);
                    if (NotificationFoundList.Count > 0)
                        await Bl.BlNotification.DeleteNotificationAsync(NotificationFoundList);

                    // remove from the list 
                    OrderSelected.BillModelList.Remove(obj);                    
                }
                else
                    await Dialog.showAsync("Cancellation Failed! Order invoice is not the latest.");
                Dialog.IsDialogOpen = false;

                // refresh the User Interface
                refreshBindings();

                // once the invoice deleted disable the email sending
                SendEmailCommand.raiseCanExecuteActionChanged();
            }
        }

        /// <summary>
        /// check that all the requirements are respeceted in order to delete an invoice
        /// </summary>
        /// <param name="arg">the invoice to process</param>
        /// <returns></returns>
        private bool canCancelCreatedInvoice(BillModel arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (!isUpdate)
                return false;
            
            if (!(arg != null && BillModelList.Count > 0 && BillModelList.OrderByDescending(x=>x.Bill.ID).First().Bill.ID <= arg.Bill.ID))
                return false;

            return true;
        }

        /// <summary>
        /// update the item list information
        /// </summary>
        /// <param name="obj"></param>
        private async void updateInvoice(BillModel obj)
        {
            Dialog.showSearch("Invoice updating...");
            var savedBillList = await Bl.BlOrder.UpdateBillAsync(new List<Bill> { obj.Bill });
            if (savedBillList.Count > 0)
            {
                var statisticsFoundList = await Bl.BlStatisitc.searchStatisticAsync(new Statistic { InvoiceId = savedBillList[0].ID }, ESearchOption.AND);
                if (statisticsFoundList.Count > 0)
                {
                    var order_itemFoundList = Order_ItemModelList.GroupBy(x => x.TxtItem_ref).Select(x => x.First()).Where(x => x.ItemModel.Item_deliveryModelList.Where(y => y.DeliveryModel.Delivery.BillId == savedBillList[0].ID).Count() > 0).ToList();
                    StatisticModel statisticModel = totalCalcul(order_itemFoundList);

                    statisticsFoundList[0].Date_limit = statisticModel.Statistic.Date_limit;
                    statisticsFoundList[0].InvoiceDate = statisticModel.Statistic.InvoiceDate;
                    statisticsFoundList[0].Pay_date = statisticModel.Statistic.Pay_date;
                    statisticsFoundList[0].Pay_received = statisticModel.Statistic.Pay_received;
                    statisticsFoundList[0].Price_purchase_total = statisticModel.Statistic.Price_purchase_total;
                    statisticsFoundList[0].Tax_value = statisticModel.Statistic.Tax_value;
                    statisticsFoundList[0].Total = statisticModel.Statistic.Total;
                    statisticsFoundList[0].Total_tax_included = statisticModel.Statistic.Total_tax_included;

                    var savedStatisticsList = await Bl.BlStatisitc.UpdateStatisticAsync(new List<Statistic> { statisticsFoundList[0] });
                }
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateInvoice(BillModel arg)
        {
            return true;
        }

        /// <summary>
        /// generate the order bill pdf document
        /// </summary>
        /// <param name="obj"></param>
        private void generateOrderBillPdf(BillModel obj)
        {
            Dialog.showSearch("Invoice pdf generating...");
            _paramOrderToPdf.BillId = obj.Bill.ID;
            _paramOrderToPdf.OrderId = OrderSelected.Order.ID;
            Bl.BlOrder.GeneratePdfOrder(_paramOrderToPdf);
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all the requirements are respecred in order to generate the invoice
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canGenerateOrderBillPdf(BillModel arg)
        {
            return true;
        }

        /// <summary>
        /// generate the quote
        /// </summary>
        /// <param name="obj"></param>
        private void generateQuotePdf(object obj)
        {
            Dialog.showSearch("Quote pdf generating...");
            _paramQuoteToPdf.OrderId = OrderSelected.Order.ID;
            Bl.BlOrder.GeneratePdfQuote(_paramQuoteToPdf);
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all the requirements are respecred in order to generate the quote
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canGenerateQuotePdf(object arg)
        {
            return true;
        }

        /// <summary>
        /// close the order
        /// </summary>
        /// <param name="obj"></param>
        private async void billing(object obj)
        {
            Dialog.showSearch("Billing...");
            updateOrderStatus(EOrderStatus.Bill_Order);
            if (OrderSelected.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()))
            {
                await Dialog.showAsync("Successfully Billed");
                _page(this);
            }
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all the requirements are respecred in order to close the order
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canBilling(object arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order_Billed, ESecurity._Update) && _main.securityCheck(EAction.Order_Billed, ESecurity._Update);
            bool isWrite = _main.securityCheck(EAction.Order_Billed, ESecurity._Write);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        /// <summary>
        /// select the client delivery address
        /// </summary>
        /// <param name="obj"></param>
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
                    await Dialog.showAsync("Delivery Address Successfully Saved!");
                }
                Dialog.IsDialogOpen = false;
            }
        }

        /// <summary>
        /// check that all the requirements are respecred in order select the address
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canSelectDeliveryAddress(Address arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        /// <summary>
        /// set the order tax value
        /// </summary>
        /// <param name="obj">the tax value to process</param>
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
                OrderSelected.Tax_order.Target = EOrderStatus.Order.ToString();
                OrderSelected.Tax_order.Date_insert = DateTime.Now;

                if (OrderSelected.Tax_order.ID == 0)
                    savedOrderList = await Bl.BlOrder.InsertTax_orderAsync(new List<Tax_order> { OrderSelected.Tax_order });
                else
                    savedOrderList = await Bl.BlOrder.UpdateTax_orderAsync(new List<Tax_order> { OrderSelected.Tax_order });

                // update the statistics
                StatisticModel = totalCalcul(Order_ItemModelList);

                Dialog.IsDialogOpen = false;
            }
        }

        /// <summary>
        /// check that all the requirements are respecred in order set a new tax value
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canSaveNewTax(Tax arg)
        {
            bool isUpdate = _main.securityCheck(EAction.Order, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        /// <summary>
        /// email the quote or order bill to the client
        /// </summary>
        /// <param name="obj">the invoice to process</param>
        private async void sendEmail(BillModel obj)
        {
            Dialog.showSearch("Email sending...");
            var paramEmail = new ParamEmail();
            paramEmail.IsCopyToAgent = await Dialog.showAsync("Do you want to receive a copy of the email?");
            paramEmail.Subject = EmailFile.TxtSubject;
            paramEmail.IsSendEmail = true;

            // sending quote email to the client 
            if (EmailFile.TxtFileNameWithoutExtension.Equals("quote"))
            {
                _paramQuoteToPdf.ParamEmail = paramEmail;
                _paramQuoteToPdf.OrderId = OrderSelected.Order.ID;
                Bl.BlOrder.GeneratePdfQuote(_paramQuoteToPdf);
            }

            // sending order email to client
            else
            {
                _paramOrderToPdf.BillId = obj.Bill.ID;
                _paramOrderToPdf.OrderId = OrderSelected.Order.ID;
                paramEmail.Reminder = 0;                               

                var NotificationFoundList = await Bl.BlNotification.searchNotificationAsync(new Notification { BillId = obj.Bill.ID }, ESearchOption.AND);
                
                // create a new notification entry for this invoice
                if (NotificationFoundList.Count == 0)
                    await Bl.BlNotification.InsertNotificationAsync(new List<Notification> { new Notification { Date = DateTime.Now, BillId = obj.Bill.ID, Reminder1 = default(DateTime), Reminder2 = default(DateTime) } });

                // update notification
                else
                {
                    if(NotificationFoundList[0].Reminder1 <= Utility.DateTimeMinValueInSQL2005
                        && NotificationFoundList[0].Reminder2 <= Utility.DateTimeMinValueInSQL2005)
                    {
                        paramEmail.Reminder = 1;
                        NotificationFoundList[0].Reminder1 = DateTime.Now;                        
                    }                        
                    else
                    {
                        paramEmail.Reminder = 2;
                        NotificationFoundList[0].Reminder2 = DateTime.Now;
                    }
                    await Bl.BlNotification.UpdateNotificationAsync(NotificationFoundList);
                }

                _paramOrderToPdf.ParamEmail = paramEmail;
                Bl.BlOrder.GeneratePdfOrder(_paramOrderToPdf);
            }           

            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all the requirements are respecred in order to send the email
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canSendEmail(BillModel arg)
        {
            bool isSendEmailValidOrder = _main.securityCheck(EAction.Order, ESecurity.SendEmail);
            bool isSendEmailValidPreOrder = _main.securityCheck(EAction.Order_Preorder, ESecurity.SendEmail);
            bool isSendEmailQuote = _main.securityCheck(EAction.Quote, ESecurity.SendEmail);

            if (OrderSelected == null)
                return false;

            if (!isSendEmailValidPreOrder && OrderSelected.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString()))
                return false;

            if (!isSendEmailQuote && OrderSelected.TxtStatus.Equals(EOrderStatus.Quote.ToString()))
                return false;

            if (!isSendEmailValidOrder
                && (OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString())
                || OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString())))
                return false;

            if (arg == null
                && (OrderSelected.TxtStatus.Equals(EOrderStatus.Order.ToString())
                || OrderSelected.TxtStatus.Equals(EOrderStatus.Credit.ToString())))
                return false;

            return true;
        }

        /// <summary>
        /// update the comments
        /// </summary>
        /// <param name="obj"></param>
        private async void updateComment(object obj)
        {
            Dialog.showSearch("Comment updating...");
            var savedOrderList = await Bl.BlOrder.UpdateOrderAsync(new List<QOBDCommon.Entities.Order> { OrderSelected.Order });
            if (savedOrderList.Count > 0)
                await Dialog.showAsync("Comment updated successfully!");
            Dialog.IsDialogOpen = false;
        }

        /// <summary>
        /// check that all the requirements are respecred in order update the comments
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canUpdateComment(object arg)
        {
            return true;
        }

        #endregion

    }

}
