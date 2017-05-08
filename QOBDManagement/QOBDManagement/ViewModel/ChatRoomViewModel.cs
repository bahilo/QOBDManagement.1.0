using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Enums;
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

        //----------------------------[ ViewModels ]------------------

        public DiscussionViewModel DiscussionViewModel { get; set; }
        public MessageViewModel MessageViewModel { get; set; }



        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> CommandNavig { get; set; }


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
            CommandNavig = new ButtonCommand<string>(appNavig, canAppNavig);
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
            int port = 0;
              
            // load chat user
            await _main.AgentViewModel.loadAgents();

            // loading users dicussions
            await MessageViewModel.loadAsync();

            // getting available port number
            string myIpAddress = GetAddresses();
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                sock.Bind(new IPEndPoint(IPAddress.Parse(myIpAddress), 0));
                port = ((IPEndPoint)sock.LocalEndPoint).Port;
            }

            _startup.Bl.BlSecurity.GetAuthenticatedUser().IPAddress = myIpAddress + ":" + port;

            // listening incoming messages
            await receiverAsync();
        }

        /// <summary>
        /// broadcast 
        /// </summary>
        private async Task receiverAsync()
        {
            Agent authenticatedUser = _startup.Bl.BlSecurity.GetAuthenticatedUser();
            try
            {
                // updating the user status                
                authenticatedUser.IsOnline = true;
                var updatedUserList = await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { authenticatedUser });

                if (updatedUserList.Count > 0)
                {
                    int port = Utility.intTryParse(updatedUserList[0].IPAddress.Split(':')[1]);
                    IPAddress ipAddress = default(IPAddress);
                    IPAddress.TryParse(updatedUserList[0].IPAddress.Split(':')[0], out ipAddress);

                    EndPoint = new IPEndPoint(ipAddress, port);
                    UdpClient = new UdpClient(port);
                    
                    // updating authenticated user online status 
                    DiscussionViewModel.SelectedAgentModel = new AgentModel();
                    DiscussionViewModel.broadcastMessage(DiscussionViewModel.WelcomeMessage);

                    // create discussion thread
                    Thread ctThread = new Thread(DiscussionViewModel.getMessage);
                    ctThread.SetApartmentState(ApartmentState.STA);
                    ctThread.IsBackground = true;
                    ctThread.Start();
                }
                else
                    new ApplicationException("Error while updating the user["+ authenticatedUser.ID + "|"+ authenticatedUser .IPAddress+ "] network information");
            }
            catch (Exception ex)
            {
                _isServerConnectionError = true;
                CurrentViewModel = DiscussionViewModel;
                Log.error("<[" + _startup.Bl.BlSecurity.GetAuthenticatedUser().UserName + "]Localhost =" + _startup.Bl.BlSecurity.GetAuthenticatedUser().IPAddress + "> " + ex.Message, QOBDCommon.Enum.EErrorFrom.CHATROOM);

                // updating the user status
                authenticatedUser.IsOnline = false;
                var updatedUserList = await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { authenticatedUser });
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
                DiscussionViewModel.broadcastMessage(DiscussionViewModel.ByeMessage);

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
        }

        public async Task DisposeAsync()
        {
            unSubscribeEvents();
            await chatLogOutAsync(null);
            DiscussionViewModel.Dispose();
            MessageViewModel.Dispose();
            _startup.Dal.Dispose();
            _startup.ProxyClient.Close();
            cleanUp();
        }

        //----------------------------[ Event Handler ]------------------

        private void onChatRoomChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ChatRoom") && _isServerConnectionError)
            {
                _isServerConnectionError = false;
                DiscussionViewModel.msg("info", "Error while trying to connect to server!");
            }
        }

        /// <summary>
        /// called from the DiscussionViewModel.getMessage()
        /// on status updated reload users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void onUpdateUsersStatusChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("updateStatus"))
            {
                if (Application.Current.Dispatcher.CheckAccess())
                    await _main.AgentViewModel.loadAgents();
                else
                    await Application.Current.Dispatcher.Invoke(async()=> {
                        await _main.AgentViewModel.loadAgents();
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
    }
}
