using QOBDCommon.Entities;
using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using QOBDManagement.Classes;
using QOBDManagement.Interfaces;
using System.ComponentModel;
using QOBDManagement.Models;
using QOBDCommon.Structures;
using QOBDCommon.Classes;
using System.Globalization;
using QOBDCommon.Enum;

namespace QOBDManagement.ViewModel
{
    public class NotificationViewModel : BindBase
    {
        private Func<Object, Object> _page;
        private Notification _notification;
        private List<Notification> _notifications;
        private string _title;
        private IMainWindowViewModel _main;

        //----------------------------[ Models ]------------------

        
        private List<BillModel> _billNotPaidList;
        private List<OrderModel> _orderWaitingValidationList;
        private List<ClientModel> _clientList;
        private NotificationSideBarViewModel _notificationSideBarViewModel;


        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> BtnDeleteCommand { get; set; }
        public ButtonCommand<ClientModel> DetailSelectedClientCommand { get; set; }
        public ButtonCommand<BillModel> SendBillCommand { get; set; }
        public ButtonCommand<BillModel> ValidChangeCommand { get; set; }

        public NotificationViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            
        }

        public NotificationViewModel(MainWindowViewModel mainWindowViewModel) : this()
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
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
            _title = "Notification Management";
            _notification = new Notification();
            _notifications = new List<Notification>();                        
        }

        private void instancesModel()
        {
            _notificationSideBarViewModel = new NotificationSideBarViewModel();
            _billNotPaidList = new List<BillModel>();
            _clientList = new List<ClientModel>();
            _orderWaitingValidationList = new List<OrderModel>();
        }

        private void instancesCommand()
        {
            BtnDeleteCommand = new ButtonCommand<string>(deleteClient, canDeleteClient);
            DetailSelectedClientCommand = new ButtonCommand<ClientModel>(showClientDetails, canShowCLientDetails);
            SendBillCommand = new ButtonCommand<BillModel>(sendUnpaidInvoiceReminderEmail, canSendUnpaidInvoiceReminderEmail);
            ValidChangeCommand = new ButtonCommand<BillModel>(validateChanges, canValidateChanges);
        }

        //----------------------------[ Properties ]------------------

        public NotificationSideBarViewModel NotificationSideBarViewModel
        {
            get { return _notificationSideBarViewModel; }
            set { setProperty(ref _notificationSideBarViewModel, value); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public string Title
        {
            get { return _title; }
        }

        public List<BillModel> BillNotPaidList
        {
            get { return _billNotPaidList; }
            set { setProperty(ref _billNotPaidList, value); }
        }

        public List<OrderModel> OrderWaitingValidationList
        {
            get { return _orderWaitingValidationList; }
            set { setProperty(ref _orderWaitingValidationList, value); }
        }

        public List<ClientModel> ClientList
        {
            get { return _clientList; }
            set { setProperty(ref _clientList, value); }
        }


        //----------------------------[ Actions ]------------------

        public async void load()
        {
            Dialog.showSearch("Loading...");
            ClientList = (await Bl.BlClient.GetClientMaxCreditOverDataByAgentAsync(Bl.BlSecurity.GetAuthenticatedUser().ID)).Select(x=> new ClientModel { Client = x }).ToList();
            BillNotPaidList = await billListToModelViewList(await Bl.BlOrder.GetUnpaidBillDataByAgentAsync(Bl.BlSecurity.GetAuthenticatedUser().ID));

            // getting the orders waiting to be validated for more than a week
            _main.OrderViewModel.loadOrders();
            OrderWaitingValidationList = _main.OrderViewModel.OrderModelList.Where(x=> x.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString()) && x.Order.Date < DateTime.Now.AddDays(-7)).ToList();
            Dialog.IsDialogOpen = false;
        }

        public async Task<List<BillModel>> billListToModelViewList(List<Bill> billList)
        {
            List<BillModel> output = new List<BillModel>();
            foreach (Bill bill in billList)
            {
                BillModel bvm = new BillModel();
                bvm.Bill = bill;

                var clientFound = (await Bl.BlClient.searchClientAsync(new Client { ID = bill.ClientId }, QOBDCommon.Enum.ESearchOption.AND)).FirstOrDefault();
                if(clientFound != null)
                    bvm.ClientModel = new ClientModel { Client = clientFound };

                var orderFound = (await Bl.BlOrder.searchOrderAsync(new Order { ID = bill.OrderId }, QOBDCommon.Enum.ESearchOption.AND)).FirstOrDefault();
                if (orderFound != null)
                    bvm.OrderModel = new OrderModel { Order = orderFound };

                var notificationFound = (await Bl.BlNotification.SearchNotificationAsync(new Notification { BillId = bill.ID }, QOBDCommon.Enum.ESearchOption.AND)).FirstOrDefault();
                if (notificationFound != null)
                    bvm.NotificationModel = new NotificationModel { Notification = notificationFound };

                output.Add(bvm);
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
            NotificationSideBarViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
                NotificationSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                NotificationSideBarViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------
        
        private void validateChanges(BillModel obj)
        {
            
        }

        private bool canValidateChanges(BillModel arg)
        {
            return true;
        }

        private async void sendUnpaidInvoiceReminderEmail(BillModel obj)
        {
            Dialog.showSearch("Email sending...");
            ParamOrderToPdf paramOrderToPdf = new ParamOrderToPdf();
            var paramEmail = new ParamEmail();
            paramEmail.IsCopyToAgent = await Dialog.show("Do you want to receive a copy?");
            paramEmail.IsSendEmail = true;

            paramOrderToPdf.BillId = obj.Bill.ID;
            paramOrderToPdf.OrderId = obj.Bill.OrderId;
            paramOrderToPdf.Lang = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault() ?? "en";
            paramOrderToPdf.ParamEmail = paramEmail;

            var billNotPaidFoundList = BillNotPaidList.Where(x => x.Bill.ID == obj.Bill.ID).ToList();
            var notificationFoundList = await Bl.BlNotification.SearchNotificationAsync(new Notification { BillId = obj.Bill.ID }, QOBDCommon.Enum.ESearchOption.AND) ;
            if(notificationFoundList.Count > 0)
            {
                // the first reminder of unpaid invoice
                if (notificationFoundList[0].Reminder1 <= Utility.DateTimeMinValueInSQL2005 && notificationFoundList[0].Reminder2 <= Utility.DateTimeMinValueInSQL2005)
                {
                    paramEmail.Reminder = 1;
                    notificationFoundList[0].Reminder1 = DateTime.Now;

                    // update the invoice notification
                    if (billNotPaidFoundList.Count > 0)
                        billNotPaidFoundList[0].TxtDateFirstReminder = notificationFoundList[0].Reminder1.ToString();
                }

                // the second reminder of unpaid invoice
                else
                {
                    paramEmail.Reminder = 2;
                    notificationFoundList[0].Reminder2 = DateTime.Now;

                    // update the invoice notification
                    if (billNotPaidFoundList.Count > 0)
                        billNotPaidFoundList[0].TxtDateSecondReminder = notificationFoundList[0].Reminder2.ToString();
                }

                // save that a reminder has been sent
                var savedNotificationList = await Bl.BlNotification.UpdateNotificationAsync(new List<Notification> { notificationFoundList[0] });
                     
                // generate and send the invoice 
                Bl.BlOrder.GeneratePdfOrder(paramOrderToPdf);
            }           
            
            Dialog.IsDialogOpen = false;
        }

        private bool canSendUnpaidInvoiceReminderEmail(BillModel arg)
        {
            return true;
        }

        private void showClientDetails(ClientModel obj)
        {
            
        }

        private bool canShowCLientDetails(ClientModel arg)
        {
            return true;
        }

        private void deleteClient(string obj)
        {
            
        }

        private bool canDeleteClient(string arg)
        {
            return true;
        }

    }
}
