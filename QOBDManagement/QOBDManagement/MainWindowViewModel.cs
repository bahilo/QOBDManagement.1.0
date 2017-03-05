using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDCommon.Entities;
using QOBDManagement.Models;
using QOBDManagement.Interfaces;
using System.Windows.Threading;
using System.IO;
using System.Threading;
using QOBDCommon.Enum;
using System.Windows;

namespace QOBDManagement
{

    public class MainWindowViewModel : BindBase, IMainWindowViewModel
    {
        public bool isNewAgentAuthentication { get; set; }
        private Object _currentViewModel;
        private object _chatRoomCurrentView;
        private Cart _cart;
        private double _progressBarPercentValue;
        private string _searchProgressVisibolity;
        private Context _context;
        private DisplayAndData.Display.Image _headerImageDisplay;
        private DisplayAndData.Display.Image _logoImageDisplay;
        private DisplayAndData.Display.Image _billImageDisplay;
        private bool _isThroughContext;
        private bool _isRefresh;

        //----------------------------[ Models ]------------------

        public ClientViewModel ClientViewModel { get; set; }
        public ItemViewModel ItemViewModel { get; set; }
        public OrderViewModel OrderViewModel { get; set; }
        public AgentViewModel AgentViewModel { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public ReferentialViewModel ReferentialViewModel { get; set; }
        public StatisticViewModel StatisticViewModel { get; set; }
        public QuoteViewModel QuoteViewModel { get; set; }
        public SecurityLoginViewModel SecurityLoginViewModel { get; set; }
        public ChatRoomViewModel ChatRoomViewModel { get; set; }


        //----------------------------[ Orders ]------------------        

        public ButtonCommand<string> CommandNavig { get; set; }
        public ButtonCommand<string> NewMessageHomePageCommand { get; set; }


        public MainWindowViewModel(IStartup startup) : base()
        {
            init(startup);
            instancesOrder();
            setInitEvents();  
        }

        //----------------------------[ Initialization ]------------------

        private void init(IStartup startup)
        {
            _searchProgressVisibolity = "Visible";
            _context = new Context(navigation);
            _currentViewModel = null;
            _cart = new Cart();

            //------[ Image ]
            _headerImageDisplay = new DisplayAndData.Display.Image();
            _headerImageDisplay.TxtFileNameWithoutExtension = "header_image";
            _headerImageDisplay.TxtName = "Header Image";
            _logoImageDisplay = new DisplayAndData.Display.Image();
            _logoImageDisplay.TxtFileNameWithoutExtension = "logo_image";
            _logoImageDisplay.TxtName = "Logo Image";
            _billImageDisplay = new DisplayAndData.Display.Image();
            _billImageDisplay.TxtFileNameWithoutExtension = "bill_image";
            _billImageDisplay.TxtName = "BIll Image";

            //------[ ViewModel ]
            ItemViewModel = new ItemViewModel(this);
            ClientViewModel = new ClientViewModel(this);
            ChatRoomViewModel = new ChatRoomViewModel(this);
            AgentViewModel = new AgentViewModel(this, ChatRoomViewModel.DiscussionViewModel);
            HomeViewModel = new HomeViewModel(this);
            NotificationViewModel = new NotificationViewModel(this);
            ReferentialViewModel = new ReferentialViewModel(this);
            StatisticViewModel = new StatisticViewModel(this);
            OrderViewModel = new OrderViewModel(this);
            QuoteViewModel = new QuoteViewModel(this);            

            SecurityLoginViewModel = new SecurityLoginViewModel(this);

            Startup = startup;
            Dialog = new ConfirmationViewModel();            
        }

        private void instancesOrder()
        {
            CommandNavig = new ButtonCommand<string>(appNavig, canAppNavig);
            NewMessageHomePageCommand = new ButtonCommand<string>(goToMessageHome, canGoToMessageHome);
        }

        private void setInitEvents()
        {
            SecurityLoginViewModel.AgentModel.PropertyChanged += onAuthenticatedAgentChange;
            ChatRoomViewModel.DiscussionViewModel.PropertyChanged += onNewMessageReceived;
        }

        //----------------------------[ Properties ]------------------

        public string TxtUserName
        {
            get { return (AuthenticatedUser != null) ? AuthenticatedUser.FirstName + " " + AuthenticatedUser.LastName : ""; }
        }

        public Agent AuthenticatedUser
        {
            get { return _startup.Bl.BlSecurity.GetAuthenticatedUser(); }
        }

        public bool IsThroughContext
        {
            get { return _isThroughContext; }
            set { setProperty(ref _isThroughContext, value); }
        }

        public bool IsRefresh
        {
            get { return _isRefresh; }
            set { setProperty(ref _isRefresh, value); }
        }

        public Context Context
        {
            get { return _context; }
            set { setProperty(ref _context, value); }
        }

        public DisplayAndData.Display.Image HeaderImageDisplay
        {
            get { return _headerImageDisplay; }
            set {
                        setProperty(ref _headerImageDisplay, value, "HeaderImageDisplay");
                }
        }

        public DisplayAndData.Display.Image LogoImageDisplay
        {
            get { return _logoImageDisplay; }
            set {
                        setProperty(ref _logoImageDisplay, value, "LogoImageDisplay");
                }
        }

        public DisplayAndData.Display.Image BillImageDisplay
        {
            get { return _billImageDisplay; }
            set
            {
                    setProperty(ref _billImageDisplay, value, "BillImageDisplay");
            }
        }

        public string SearchProgressVisibility
        {
            get { return _searchProgressVisibolity; }
            set { setProperty(ref _searchProgressVisibolity, value); }
        }

        public Cart Cart
        {
            get { return _cart; }
        }

        public OrderSideBarViewModel OrderQuoteSideBar
        {
            get { return OrderViewModel.OrderSideBarViewModel; }
        }

        public Object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        setProperty(ref _currentViewModel, value);
                    });
            }
        }

        public Object ChatRoomCurrentView
        {
            get { return _chatRoomCurrentView; }
            set
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        setProperty(ref _chatRoomCurrentView, value);
                    });
            }
        }

        public double ProgressBarPercentValue
        {
            get { return _progressBarPercentValue; }
            set {
                if(Application.Current != null)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        setProperty(ref _progressBarPercentValue, value);
                    });
            }
        }

        //----------------------------[ Actions ]------------------

        private void downloadHeaderImages()
        {
            if (string.IsNullOrEmpty(_headerImageDisplay.TxtLogin) || string.IsNullOrEmpty(_logoImageDisplay.TxtLogin) || string.IsNullOrEmpty(_billImageDisplay.TxtLogin))
            {
                _headerImageDisplay.TxtLogin = _logoImageDisplay.TxtLogin = _billImageDisplay.TxtLogin = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_login" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
                _headerImageDisplay.TxtPassword = _logoImageDisplay.TxtPassword = _billImageDisplay.TxtPassword = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_password" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
            }

            if (string.IsNullOrEmpty(_headerImageDisplay.TxtFileFullPath))
            {                
                var headerImageFoundDisplay = loadImage(_headerImageDisplay.TxtFileNameWithoutExtension, _headerImageDisplay.TxtName, _headerImageDisplay.TxtLogin, _headerImageDisplay.TxtPassword);
                if (!string.IsNullOrEmpty(headerImageFoundDisplay.TxtFileFullPath) && File.Exists(headerImageFoundDisplay.TxtFileFullPath))
                    if (Application.Current != null)
                            Application.Current.Dispatcher.Invoke(()=> {
                            HeaderImageDisplay = headerImageFoundDisplay;
                        });                    
            }

            if (string.IsNullOrEmpty(_logoImageDisplay.TxtFileFullPath))
            {
                var logoImageFoundDisplay = loadImage(_logoImageDisplay.TxtFileNameWithoutExtension, _logoImageDisplay.TxtName, _logoImageDisplay.TxtLogin, _logoImageDisplay.TxtPassword);
                if (!string.IsNullOrEmpty(logoImageFoundDisplay.TxtFileFullPath) && File.Exists(logoImageFoundDisplay.TxtFileFullPath))
                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(() => {
                            LogoImageDisplay = logoImageFoundDisplay;
                        });                
            }

            if (string.IsNullOrEmpty(_billImageDisplay.TxtFileFullPath))
            {
                var billImageFoundDisplay = loadImage(_billImageDisplay.TxtFileNameWithoutExtension, _billImageDisplay.TxtName, _billImageDisplay.TxtLogin, _billImageDisplay.TxtPassword);
                if (!string.IsNullOrEmpty(billImageFoundDisplay.TxtFileFullPath) && File.Exists(billImageFoundDisplay.TxtFileFullPath))
                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(() => {
                            BillImageDisplay = billImageFoundDisplay;
                        });                
            }
        }

        private DisplayAndData.Display.Image loadImage(string fileName, string imageName, string login, string password)
        {
            var imageDataList = new List<Info>();

            var infosFoundImage = _startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = fileName }, ESearchOption.AND).FirstOrDefault();
            DisplayAndData.Display.Image imageObject = new DisplayAndData.Display.Image();

            if (infosFoundImage != null)
            {
                imageObject.TxtName = imageName;
                imageObject.TxtLogin = login;
                imageObject.TxtPassword = password;
                imageObject.TxtFileNameWithoutExtension = fileName;
                imageObject.ImageInfos = infosFoundImage;
                var infosWidthFound = _startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = fileName + "_width" }, ESearchOption.AND).FirstOrDefault();
                var infosHeightFound = _startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = fileName + "_height" }, ESearchOption.AND).FirstOrDefault();

                if (infosWidthFound != null)
                    imageDataList.Add(infosWidthFound);
                if (infosHeightFound != null)
                    imageDataList.Add(infosHeightFound);
                if (infosFoundImage != null)
                    imageDataList.Add(infosFoundImage);

                imageObject.ImageDataList = imageDataList;
            }
            return imageObject;
        }

        public Object navigation(Object centralPageContent = null)
        {
            if (centralPageContent != null)
            {
                // reset the navigation to previous page
                IsThroughContext = false;

                // reset page refreshing
                IsRefresh = false;

                // save the previous page for later navigation
                Context.PreviousState = CurrentViewModel as IState;

                // set the current page 
                CurrentViewModel = centralPageContent;
                
                Context.NextState = centralPageContent as IState;
            }

            return CurrentViewModel;
        }

        public double progressBarManagement(double status = 0)
        {
            object _lock = new object();

            lock (_lock)
                if (status != 0)
                {
                    ProgressBarPercentValue = status;
                    if (status > 0)
                        SearchProgressVisibility = "Hidden";
                }
            return ProgressBarPercentValue;
        }

        public DisplayAndData.Display.Image ImageManagement(DisplayAndData.Display.Image newImage = null, string fileType = null)
        {
            if (fileType.ToUpper().Equals("HEADER"))
            {
                if (newImage != null)
                    HeaderImageDisplay = newImage;

                return HeaderImageDisplay;
            }

            if (fileType.ToUpper().Equals("LOGO"))
            {
                if (newImage != null)
                    LogoImageDisplay = newImage;

                return LogoImageDisplay;
            }

            if (fileType.ToUpper().Equals("BILL"))
            {
                if (newImage != null)
                    BillImageDisplay = newImage;

                return BillImageDisplay;
            }

            return new DisplayAndData.Display.Image();
        }

        public object getObject(string objectName)
        {
            object ObjectToReturn = new object();
            switch (objectName.ToUpper())
            {
                case "CLIENT":
                    ObjectToReturn = ClientViewModel;
                    break;
                case "ITEM":
                    ObjectToReturn = ItemViewModel;
                    break;
                case "ORDER":
                    ObjectToReturn = OrderViewModel;
                    break;
                case "QUOTE":
                    ObjectToReturn = QuoteViewModel;
                    break;
                case "AGENT":
                    ObjectToReturn = AgentViewModel;
                    break;
                case "REFERENTIAL":
                    ObjectToReturn = ReferentialViewModel;
                    break;
                case "SECURITY":
                    ObjectToReturn = SecurityLoginViewModel;
                    break;
                case "NOTIFICATION":
                    ObjectToReturn = NotificationViewModel;
                    break;
                case "HOME":
                    ObjectToReturn = HomeViewModel;
                    break;
                case "STATISTIC":
                    ObjectToReturn = StatisticViewModel;
                    break;
                case "MAIN":
                    ObjectToReturn = this;
                    break;
                case "CART":
                    ObjectToReturn = Cart;
                    break;
                case "CONTEXT":
                    ObjectToReturn = Context;
                    break;
                case "DIALOG":
                    ObjectToReturn = _dialog;
                    break;
                case "ORDER_SIDEBAR":
                    ObjectToReturn = OrderViewModel.OrderSideBarViewModel;
                    break;
            }

            return ObjectToReturn;
        }

        /// <summary>
        /// Initializing the User Interface
        /// </summary>
        private void loadUIData()
        {
             Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                SearchProgressVisibility = "Visible";
                if (isNewAgentAuthentication)
                {
                    ProgressBarPercentValue = -1;
                    _startup.Dal.SetUserCredential(SecurityLoginViewModel.Bl.BlSecurity.GetAuthenticatedUser(), isNewAgentAuthentication);
                    isNewAgentAuthentication = false;
                    ProgressBarPercentValue = 100;
                }
                else if (SecurityLoginViewModel.AgentModel.Agent.ID != 0)
                {
                    _startup.Dal.ProgressBarFunc = progressBarManagement;
                    _startup.Dal.SetUserCredential(AuthenticatedUser);
                    _startup.Dal.DALReferential.PropertyChanged += onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage;
                    _startup.Dal.DALItem.PropertyChanged += onLodingIsLodingDataFromWebServiceToLocalChange_loadStatistics;
                    //downloadHeaderImages();                    
                }
                CommandNavig.raiseCanExecuteActionChanged();
                onPropertyChange("TxtUserName");

                //========================= [ Chat Room ]====================

                // load chat user
                //AgentViewModel.loadAgents();

                // display the chat view
                ChatRoomCurrentView = ChatRoomViewModel;

                // connect user to the chat server
                ChatRoomViewModel.connectToServer();
            });
        }

        public bool securityCheck(EAction action, ESecurity right)
        {
            if (_startup != null)
            {
                Agent agent = _startup.Bl.BlSecurity.GetAuthenticatedUser();
                if (agent.RoleList != null)
                {
                    foreach (var role in agent.RoleList)
                    {
                        var actionFound = role.ActionList.Where(x => x.Name.Equals(action.ToString())).FirstOrDefault();
                        if (actionFound != null)
                        {
                            switch (right)
                            {
                                case ESecurity._Delete:
                                    if(actionFound.Right.IsDelete)
                                        return actionFound.Right.IsDelete;
                                    break;
                                case ESecurity._Read:
                                    if (actionFound.Right.IsRead)
                                        return actionFound.Right.IsRead;
                                    break;
                                case ESecurity._Update:
                                    if (actionFound.Right.IsUpdate)
                                        return actionFound.Right.IsUpdate;
                                    break;
                                case ESecurity._Write:
                                    if (actionFound.Right.IsWrite)
                                        return actionFound.Right.IsWrite;
                                    break;
                                case ESecurity.SendEmail:
                                    if (actionFound.Right.IsSendMail)
                                        return actionFound.Right.IsSendMail;
                                    break;
                            }
                        }
                    }                    
                }
                    
            }
            return false;
        }

        private void unsubscribeEvents()
        {
            SecurityLoginViewModel.AgentModel.PropertyChanged -= onAuthenticatedAgentChange;
            _startup.Dal.DALReferential.PropertyChanged -= onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage;
            ChatRoomViewModel.DiscussionViewModel.PropertyChanged -= onNewMessageReceived;
        }

        public override void Dispose()
        {
            Dialog.showSearch("Closing...");
            unsubscribeEvents();
            ItemViewModel.Dispose();
            ClientViewModel.Dispose();
            QuoteViewModel.Dispose();
            OrderViewModel.Dispose();
            ReferentialViewModel.Dispose();
            AgentViewModel.Dispose();
            NotificationViewModel.Dispose();
            SecurityLoginViewModel.Dispose();
            HomeViewModel.Dispose();            
            ChatRoomViewModel.Dispose();
            _startup.Dal.Dispose();
            GC.Collect();
        }

        //----------------------------[ Event Handler ]------------------

        private void onAuthenticatedAgentChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
            {
                loadUIData();

            }
        }

        private void onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsLodingDataFromWebServiceToLocal"))
            {
                if(Application.Current != null)
                    Application.Current.Dispatcher.Invoke(() => {
                        downloadHeaderImages();
                    });                
            }
        }

        private void onLodingIsLodingDataFromWebServiceToLocalChange_loadStatistics(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsLodingDataFromWebServiceToLocal"))
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.Invoke(() => {
                    HomeViewModel.loadData();
                });
            }
        }

        private void onNewMessageReceived(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtNbNewMessage"))
            {
                Application.Current.Dispatcher.Invoke(()=> {
                    NewMessageHomePageCommand.raiseCanExecuteActionChanged();
                });                
            }                
        }


        //----------------------------[ Action Commands ]------------------

        private void goToMessageHome(string obj)
        {
            appNavig("home");
            ChatRoomViewModel.DiscussionViewModel.readNewMessages(null);
        }

        private bool canGoToMessageHome(string arg)
        {
            if (ChatRoomViewModel.DiscussionViewModel.TxtNbNewMessage != "0")
                return true;
            return false;
        }

        private void appNavig(string propertyName)
        {
            
            switch (propertyName)
            {
                case "home":
                    HomeViewModel.executeNavig(propertyName);
                    break;
                case "client":
                    ClientViewModel.executeNavig(propertyName);
                    break;
                case "item":
                    ItemViewModel.executeNavig(propertyName);
                    break;
                case "order":
                    OrderViewModel.executeNavig(propertyName);
                    break;
                case "quote":
                    QuoteViewModel.executeNavig(propertyName);
                    break;
                case "agent":
                    AgentViewModel.executeNavig(propertyName);
                    break;
                case "notification":
                    CurrentViewModel = NotificationViewModel;
                    break;
                case "option":
                    ReferentialViewModel.executeNavig(propertyName);
                    break;
                case "statistic":
                    CurrentViewModel = StatisticViewModel;
                    break;
                case "back":
                    Context.Request();
                    IsThroughContext = true;
                    break;
                case "refresh":
                    onPropertyChange("CurrentViewModel");
                    IsRefresh = true;
                    break;
            }              
        }

        private bool canAppNavig(string arg)
        {
            if (_startup == null)
                return false;
            if (AuthenticatedUser == null || AuthenticatedUser.Status == EStatus.Deactivated.ToString())
                return false;
            if (arg.Equals("client"))
                return securityCheck(QOBDCommon.Enum.EAction.Client, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("item"))
                return securityCheck(QOBDCommon.Enum.EAction.Item, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("agent"))
                return securityCheck(QOBDCommon.Enum.EAction.Agent, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("notification"))
                return securityCheck(QOBDCommon.Enum.EAction.Notification, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("quote"))
                return securityCheck(QOBDCommon.Enum.EAction.Quote, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("order"))
                return securityCheck(QOBDCommon.Enum.EAction.Order, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("statistic"))
                return false;// securityCheck(QOBDCommon.Enum.EAction.Statistic, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("option"))
                return securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Read);
            if (arg.Equals("home") || arg.Equals("back") || arg.Equals("refresh"))
                return true;

            return false;
        }
    }
}
