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
using QOBDCommon.Classes;
using System.Reflection;
using QOBDManagement.Classes.Themes;
using System.Configuration;

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
        private InfoManager.Display _headerImageDisplay;
        private InfoManager.Display _logoImageDisplay;
        private InfoManager.Display _billImageDisplay;
        //private InfoManager.Display.Image _profileImageDisplay;
        private bool _isThroughContext;
        private bool _isRefresh;
        private int _heightDataList;
        private int _widthDataList;

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
            _widthDataList = 1000;
            _heightDataList = 600;

            //------[ Images ]
            _headerImageDisplay = new InfoManager.Display("header_image", new List<string> { "header_image", "header_image_width", "header_image_height" },ConfigurationManager.AppSettings["ftp_image_folder"], ConfigurationManager.AppSettings["local_image_folder"], "","");
            _logoImageDisplay = new InfoManager.Display("logo_image", new List<string> { "logo_image", "logo_image_width", "logo_image_height" }, ConfigurationManager.AppSettings["ftp_image_folder"], ConfigurationManager.AppSettings["local_image_folder"], "", "");
            _billImageDisplay = new InfoManager.Display("bill_image", new List<string> { "bill_image", "bill_image_width", "bill_image_height" }, ConfigurationManager.AppSettings["ftp_image_folder"], ConfigurationManager.AppSettings["local_image_folder"], "", "");
            
            Startup = startup;
            Dialog = new ConfirmationViewModel();

            //------[ ViewModels ]
            ItemViewModel = new ItemViewModel(this, Startup, Dialog);
            ClientViewModel = new ClientViewModel(this, Startup, Dialog);
            AgentViewModel = new AgentViewModel(this, Startup, Dialog);
            ChatRoomViewModel = new ChatRoomViewModel(this, Startup, Dialog);
            HomeViewModel = new HomeViewModel(this, Startup, Dialog);
            NotificationViewModel = new NotificationViewModel(this, Startup, Dialog);
            ReferentialViewModel = new ReferentialViewModel(this, Startup, Dialog);
            StatisticViewModel = new StatisticViewModel(this, Startup, Dialog);
            OrderViewModel = new OrderViewModel(this, Startup, Dialog);
            QuoteViewModel = new QuoteViewModel(this, Startup, Dialog);
            SecurityLoginViewModel = new SecurityLoginViewModel(this, Startup, Dialog);

            
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
            PropertyChanged += AgentViewModel.AgentSideBarViewModel.onCurrentPageChange_updateCommand;
            PropertyChanged += OrderViewModel.OrderSideBarViewModel.onCurrentPageChange_updateCommand;
            PropertyChanged += ClientViewModel.ClientSideBarViewModel.onCurrentPageChange_updateCommand;
        }

        //----------------------------[ Properties ]------------------

        public string TxtUserName
        {
            get { return (AuthenticatedUserModel != null) ? AuthenticatedUserModel.TxtLogin : ""; }
        }

        public AgentModel AuthenticatedUserModel
        {
            get { return new AgentModel { Agent = _startup.Bl.BlSecurity.GetAuthenticatedUser() }; }
        }

        public string TxtHeightDataList
        {
            get { return _heightDataList.ToString(); }
            set { setProperty(ref _heightDataList, Utility.intTryParse(value)); }
        }

        public string TxtWidthDataList
        {
            get { return _widthDataList.ToString(); }
            set { setProperty(ref _widthDataList, Utility.intTryParse(value)); }
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

        public InfoManager.Display HeaderImageDisplay
        {
            get { return _headerImageDisplay; }
            set { setProperty(ref _headerImageDisplay, value); }
        }

        public InfoManager.Display LogoImageDisplay
        {
            get { return _logoImageDisplay; }
            set { setProperty(ref _logoImageDisplay, value); }
        }

        public InfoManager.Display BillImageDisplay
        {
            get { return _billImageDisplay; }
            set { setProperty(ref _billImageDisplay, value); }
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
            set
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        setProperty(ref _progressBarPercentValue, value);
                    });
            }
        }

        //----------------------------[ Actions ]------------------


        /// <summary>
        /// Initializing the User Interface
        /// </summary>
        private void load()
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
                _startup.Dal.SetUserCredential(AuthenticatedUserModel.Agent);
                _startup.Dal.DALReferential.PropertyChanged += onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage;
                
            }

            CommandNavig.raiseCanExecuteActionChanged();
            AgentViewModel.GetCurrentAgentCommand.raiseCanExecuteActionChanged();

            // display the chat view
            ChatRoomCurrentView = ChatRoomViewModel;
        }

        /*public async void loadChatRoom()
        {
            // load chat user
            await AgentViewModel.loadAgents();

            // display the chat view
            ChatRoomCurrentView = ChatRoomViewModel;

            // connect user to the chat server
            ChatRoomViewModel.start();
        }*/

        private void downloadHeaderImages()
        {
            // set ftp credentials
            if (string.IsNullOrEmpty(_headerImageDisplay.TxtLogin) || string.IsNullOrEmpty(_logoImageDisplay.TxtLogin) || string.IsNullOrEmpty(_billImageDisplay.TxtLogin))
            {
                 _headerImageDisplay.TxtLogin = _logoImageDisplay.TxtLogin = _billImageDisplay.TxtLogin = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_login" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
                _headerImageDisplay.TxtPassword = _logoImageDisplay.TxtPassword = _billImageDisplay.TxtPassword = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_password" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
            }

            // download header image
            if (string.IsNullOrEmpty(_headerImageDisplay.TxtFileFullPath))
            {
                var headerImageFoundDisplay = loadImage(HeaderImageDisplay);
                if (!string.IsNullOrEmpty(headerImageFoundDisplay.TxtFileFullPath) && File.Exists(headerImageFoundDisplay.TxtFileFullPath))
                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            HeaderImageDisplay = headerImageFoundDisplay;
                        });
            }

            // download header logo
            if (string.IsNullOrEmpty(_logoImageDisplay.TxtFileFullPath))
            {
                var logoImageFoundDisplay = loadImage(LogoImageDisplay);
                if (!string.IsNullOrEmpty(logoImageFoundDisplay.TxtFileFullPath) && File.Exists(logoImageFoundDisplay.TxtFileFullPath))
                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LogoImageDisplay = logoImageFoundDisplay;
                        });
            }

            // download the bill image
            if (string.IsNullOrEmpty(_billImageDisplay.TxtFileFullPath))
            {
                var billImageFoundDisplay = loadImage(BillImageDisplay);
                if (!string.IsNullOrEmpty(billImageFoundDisplay.TxtFileFullPath) && File.Exists(billImageFoundDisplay.TxtFileFullPath))
                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            BillImageDisplay = billImageFoundDisplay;
                        });
            }
        }

        public InfoManager.Display loadImage(InfoManager.Display image)
        {
            image.InfoDataList = _startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = image.TxtFileNameWithoutExtension }, ESearchOption.AND);
            image.downloadFile();
            return image;
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

        public InfoManager.Display ImageManagement(InfoManager.Display newImage = null, string fileType = null)
        {
            switch (fileType.ToUpper())
            {
                case "HEADER":
                    if (newImage != null)
                        HeaderImageDisplay = newImage;
                    return HeaderImageDisplay;
                case "LOGO":
                    if (newImage != null)
                        LogoImageDisplay = newImage;

                    return LogoImageDisplay;
                case "BILL":
                    if (newImage != null)
                        BillImageDisplay = newImage;

                    return BillImageDisplay;
            }

            return new InfoManager.Display();
        }

        public bool securityCheck(EAction action, ESecurity right)
        {
            return SecurityLoginViewModel.securityCheck(action, right);
        }

        private void unsubscribeEvents()
        {
            SecurityLoginViewModel.AgentModel.PropertyChanged -= onAuthenticatedAgentChange;
            _startup.Dal.DALReferential.PropertyChanged -= onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage;
            ChatRoomViewModel.DiscussionViewModel.PropertyChanged -= onNewMessageReceived;
            PropertyChanged -= AgentViewModel.AgentSideBarViewModel.onCurrentPageChange_updateCommand;
            PropertyChanged -= ClientViewModel.ClientSideBarViewModel.onCurrentPageChange_updateCommand;
            PropertyChanged -= OrderViewModel.OrderSideBarViewModel.onCurrentPageChange_updateCommand;
        }

        public async Task<bool> DisposeAsync()
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
            ChatRoomCurrentView = null;
            await ChatRoomViewModel.DisposeAsync();
            return true;
        }

        //----------------------------[ Event Handler ]------------------

        /// <summary>
        /// event listener to load UI data on authenticated user change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onAuthenticatedAgentChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
            {
                if ( _startup.Bl.BlSecurity.IsUserAuthenticated())
                    load();
                onPropertyChange("AuthenticatedUserModel");
                onPropertyChange("TxtUserName");
                CommandNavig.raiseCanExecuteActionChanged();
            }

        }

        /// <summary>
        /// event listener to download the IU images on caching executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onLodingGeneralInfosDataFromWebServiceToLocalChange_loadHeaderImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsLodingDataFromWebServiceToLocal"))
            {
                // if not unit testing download images
                if (Application.Current != null)
                    downloadHeaderImages();
            }
        }

        private void onNewMessageReceived(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtNbNewMessage"))
            {
                // if not unit testing update displaying
                if (Application.Current != null)
                    NewMessageHomePageCommand.raiseCanExecuteActionChanged();
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
            if (AuthenticatedUserModel == null || AuthenticatedUserModel.TxtStatus == EStatus.Deactivated.ToString())
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
