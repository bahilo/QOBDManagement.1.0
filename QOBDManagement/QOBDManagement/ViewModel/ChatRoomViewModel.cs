using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Enums;
using QOBDManagement.Helper;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QOBDManagement.ViewModel
{
    public class ChatRoomViewModel : BindBase, IChatRoomViewModel
    {
        private Context _context;
        private Object _currentViewModel;
        private bool _isServerConnectionError;
        private IMainWindowViewModel _main;
        private AgentModel _authenticatedAgent;

        //----------------------------[ ViewModels ]------------------

        public DiscussionViewModel DiscussionViewModel { get; set; }
        public MessageViewModel MessageViewModel { get; set; }



        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> CommandNavig { get; set; }
        public ButtonCommand<object> DisplayAccountCommand { get; set; }
        public ButtonCommand<object> ChatValidUserAccountCommand { get; set; }


        public ChatRoomViewModel()
        {
            initializer();
        }

        public ChatRoomViewModel(IMainWindowViewModel mainWindowViewModel) : this()
        {
            _main = mainWindowViewModel;
            setInitEvents();
        }

        public ChatRoomViewModel(IMainWindowViewModel mainWindowViewModel, IStartup startup, IConfirmationViewModel dialog) : this(mainWindowViewModel)
        {
            this.Startup = startup;
            this.Dialog = dialog;

            MessageViewModel.Dialog = Dialog;
            DiscussionViewModel.Dialog = Dialog;
            DiscussionViewModel.Startup = Startup;
            MessageViewModel.Startup = Startup;                        
        }


        //----------------------------[ Initialization ]------------------

        private void initializer()
        {
            DiscussionViewModel = new DiscussionViewModel(this);
            MessageViewModel = new MessageViewModel(this);
            _context = new Context(navigation);
            CurrentViewModel = MessageViewModel;
            CommandNavig = new ButtonCommand<string>(appNavig, canAppNavig);
            DisplayAccountCommand = new ButtonCommand<object>(displayChatAgentAccount, canDisplayChatAgentAccount);
            ChatValidUserAccountCommand = new ButtonCommand<object>(validChatAccount, canValidChatAccount);
        }

        private void setInitEvents()
        {
            DiscussionViewModel.PropertyChanged += onChatRoomChange;
            DiscussionViewModel.PropertyChanged += onUpdateUsersStatusChange;
        }


        //----------------------------[ Properties ]------------------

        public Object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { setProperty(ref _currentViewModel, value); }
        }

        public AgentModel AuthenticatedAgent
        {
            get { return _authenticatedAgent; }
            set { _authenticatedAgent = value; onPropertyChange(); }
        }

        public string ChatAuthenticatedWelcomeMessage
        {
            get { return _startup.Bl.BlSecurity.GetAuthenticatedUser().Comment; }
            set { _startup.Bl.BlSecurity.GetAuthenticatedUser().Comment = value; onPropertyChange(); }
        }

        public AgentViewModel AgentViewModel
        {
            get { return _main.AgentViewModel; }
        }

        public IPEndPoint EndPoint
        {
            get { return DiscussionViewModel.EndPoint; }
            set { DiscussionViewModel.EndPoint = value; onPropertyChange(); }
        }

        public UdpClient UdpClient
        {
            get { return DiscussionViewModel.UdpClient; }
            set { DiscussionViewModel.UdpClient = value; onPropertyChange(); }
        }

        public IMainWindowViewModel MainWindowViewModel
        {
            get { return _main; }
        }

        public string TxtUserName
        {
            get { return (_startup.Bl != null) ? _startup.Bl.BlSecurity.GetAuthenticatedUser().UserName : ""; }
        }

        public Context Context
        {
            get { return _context; }
            set { setProperty(ref _context, value); }
        }

        public AgentViewModel UserViewModel
        {
            get { return _main.AgentViewModel; }
        }


        //----------------------------[ Actions ]------------------
        
        /// <summary>
        /// start the chat server
        /// </summary>
        public async void start()
        {        
            // loading chat information
            loadChatData();

            // listening incoming messages
            await receiverAsync();
        }

        private async void loadChatData()
        {
            int port = 0;

            // get user details
            getChatUserInformation();

            // loading users dicussions
            await MessageViewModel.loadAsync();

            // getting available port number
            string myIpAddress = GetAddresses();
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                sock.Bind(new IPEndPoint(IPAddress.Parse(myIpAddress), 0));
                port = ((IPEndPoint)sock.LocalEndPoint).Port;
            }

            AuthenticatedAgent.TxtIPAddress = myIpAddress + ":" + port;
        }

        public async void getChatUserInformation()
        {
            _authenticatedAgent = new AgentModel { Agent = _startup.Bl.BlSecurity.GetAuthenticatedUser() };

            // load chat user
            await _main.AgentViewModel.loadAgents();

            // close user images
            foreach (AgentModel agentModel in _main.AgentViewModel.AgentModelList)
            {
                if (agentModel.Image != null)
                    agentModel.Image.closeImageSource();
            }                

            // download chat user's picture
            foreach (AgentModel agentModel in _main.AgentViewModel.AgentModelList)
                agentModel.Image = agentModel.Image.downloadPicture(ConfigurationManager.AppSettings["ftp_profile_image_folder"], ConfigurationManager.AppSettings["local_profile_image_folder"], agentModel.TxtPicture, agentModel.TxtProfileImageFileNameBase + "_" + agentModel.Agent.ID, _startup.Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, QOBDCommon.Enum.ESearchOption.AND));

            DiscussionViewModel.ChatAgentModelList = _main.AgentViewModel.AgentModelList.Where(x => x.Agent.ID != _startup.Bl.BlSecurity.GetAuthenticatedUser().ID).ToList();
            AuthenticatedAgent.Image = _main.AgentViewModel.AgentModelList.Where(x => x.TxtID == AuthenticatedAgent.TxtID).Select(x => x.Image).SingleOrDefault();

            Dialog.IsChatDialogOpen = false;
        }

        /// <summary>
        /// broadcast 
        /// </summary>
        private async Task receiverAsync()
        {
            try
            {
                // updating the user status                
                AuthenticatedAgent.IsOnline = true;
                var updatedUserList = await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { AuthenticatedAgent.Agent });

                if (updatedUserList.Count > 0)
                {
                    int port = Utility.intTryParse(updatedUserList[0].IPAddress.Split(':')[1]);
                    IPAddress ipAddress = default(IPAddress);
                    IPAddress.TryParse(updatedUserList[0].IPAddress.Split(':')[0], out ipAddress);

                    EndPoint = new IPEndPoint(ipAddress, port);
                    UdpClient = new UdpClient(port);
                    
                    // updating authenticated user online status 
                    DiscussionViewModel.SelectedAgentModel = new AgentModel();
                    await DiscussionViewModel.broadcastMessageAsync(DiscussionViewModel.WelcomeMessage);

                    // create discussion thread
                    Thread ctThread = new Thread(DiscussionViewModel.getMessage);
                    ctThread.SetApartmentState(ApartmentState.STA);
                    ctThread.IsBackground = true;
                    ctThread.Start();
                }
                else
                    new ApplicationException("Error while updating the user["+ AuthenticatedAgent.TxtID + "|"+ AuthenticatedAgent.TxtIPAddress+ "] network information");
            }
            catch (Exception ex)
            {
                _isServerConnectionError = true;
                CurrentViewModel = DiscussionViewModel;
                Log.error("<[" + _startup.Bl.BlSecurity.GetAuthenticatedUser().UserName + "]Localhost =" + _startup.Bl.BlSecurity.GetAuthenticatedUser().IPAddress + "> " + ex.Message, QOBDCommon.Enum.EErrorFrom.CHATROOM);

                // updating the user status
                AuthenticatedAgent.IsOnline = false;
                var updatedUserList = await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { AuthenticatedAgent.Agent });
            }
        }

        public static string GetAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            var addressList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
            foreach (string address in addressList)
            {
                IPAddress ipAddress = GetIPAddress(address);
                if (ipAddress != null)
                    return ipAddress.ToString();
            }
            return "";
        }

        public static IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        private void updateUsersOnlineStatus()
        {
            getChatUserInformation();
            DiscussionViewModel.SelectUserForDiscussionCommand.raiseCanExecuteActionChanged();
            DiscussionViewModel.DiscussionAddUserCommand.raiseCanExecuteActionChanged();
        }

        public object navigation(object centralPageContent = null)
        {
            if (centralPageContent != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Context.PreviousState = CurrentViewModel as IState;
                    CurrentViewModel = centralPageContent;
                    Context.NextState = centralPageContent as IState;
                });
            }
            return CurrentViewModel;
        }

        private void unSubscribeEvents()
        {
            // unsubscribe events
            DiscussionViewModel.PropertyChanged -= onChatRoomChange;
            DiscussionViewModel.PropertyChanged -= onUpdateUsersStatusChange;
        }

        private async Task signOutFromServerAsync(List<DiscussionModel> discussionList)
        {
            try
            {
                // disconnect the authenticated user
                Agent authenticatedUser = _startup.Bl.BlSecurity.GetAuthenticatedUser();
                authenticatedUser.IsOnline = false;
                await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { _startup.Bl.BlSecurity.GetAuthenticatedUser() });

                // ending the discussions
                DiscussionViewModel.SelectedAgentModel = new AgentModel();
                await DiscussionViewModel.broadcastMessageAsync(DiscussionViewModel.ByeMessage);

                foreach (var ClientElement in DiscussionViewModel.ServerClientsDictionary)
                {
                    ClientElement.Value.Item2.Close();
                }
            }
            catch (System.IO.IOException) { }
            catch (System.Net.Sockets.SocketException) { }
            catch (Exception ex)
            {
                Log.error("<[" + _startup.Bl.BlSecurity.GetAuthenticatedUser().UserName + "]Localhost =" + _startup.Bl.BlSecurity.GetAuthenticatedUser().IPAddress + "> " + ex.Message, QOBDCommon.Enum.EErrorFrom.CHATROOM);
            }
        }

        public void cleanUp()
        {
            if (UdpClient != null)
                UdpClient.Close();
            
            DiscussionViewModel.Dispose();
            MessageViewModel.Dispose();
            _startup.Dal.Dispose();

            if (_startup.ProxyClient.State == System.ServiceModel.CommunicationState.Opened)
                _startup.ProxyClient.Close();
        }

        public async Task DisposeAsync()
        {
            unSubscribeEvents();
            await chatLogOutAsync(null);            
            cleanUp();
        }

        //----------------------------[ Event Handler ]------------------

        private void onChatRoomChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ChatRoom") && _isServerConnectionError)
            {
                _isServerConnectionError = false;
                DiscussionViewModel.msg("info", new MessageModel { Message = new Message { Content = "Error while trying to connect to server!" } });
            }
        }

        /// <summary>
        /// called from the DiscussionViewModel.getMessage()
        /// on status updated reload users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUpdateUsersStatusChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("updateStatus"))
            {
                if (Application.Current.Dispatcher.CheckAccess())
                    updateUsersOnlineStatus();
                else
                    Application.Current.Dispatcher.Invoke(()=> {
                        updateUsersOnlineStatus();
                    });
            }
        }


        //----------------------------[ Action Commands ]------------------


        private void appNavig(string obj)
        {
            switch (obj)
            {
                case "chat":
                    CurrentViewModel = DiscussionViewModel;
                    break;
                case "back":
                    Context.Request();
                    break;
                case "home":
                    CurrentViewModel = MessageViewModel;
                    break;
                default:
                    goto case "home";
            }
        }

        private bool canAppNavig(string arg)
        {
            return true;
        }

        private async Task chatLogOutAsync(object obj)
        {
            await signOutFromServerAsync(DiscussionViewModel.DiscussionList);
            DiscussionViewModel.DiscussionList = new List<DiscussionModel>();
            CurrentViewModel = null;
        }

        private void displayChatAgentAccount(object obj)
        {
            Dialog.showAsync(new Views.ChatAccount(), isChatDialogBox: true);
        }

        private bool canDisplayChatAgentAccount(object arg)
        {
            return true;
        }

        private async void validChatAccount(object obj)
        {
            var savedAgentList = await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { AuthenticatedAgent.Agent });
            if (savedAgentList.Count > 0)
                await Dialog.showAsync("Account successfully updated!", isChatDialogBox: true);
        }

        private bool canValidChatAccount(object arg)
        {
            return true;
        }




    }
}
