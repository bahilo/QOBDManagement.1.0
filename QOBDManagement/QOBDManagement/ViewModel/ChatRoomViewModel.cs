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
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QOBDManagement.ViewModel
{
    public class ChatRoomViewModel : BindBase
    {
        private System.Net.Sockets.TcpClient _clientSocket;
        private NetworkStream _serverStream;
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
            setLogic();
        }

        public ChatRoomViewModel(IMainWindowViewModel mainWindowViewModel) : this()
        {
            _main = mainWindowViewModel;
            setInitEvents();
        }


        //----------------------------[ Initialization ]------------------
        
        private void initializer()
        {
            _context = new Context(navigation);

            DiscussionViewModel = new DiscussionViewModel(navigation);
            MessageViewModel = new MessageViewModel(navigation, DiscussionViewModel);

            CommandNavig = new ButtonCommand<string>(appNavig, canAppNavig);
            
            DiscussionViewModel.Dialog = Dialog;
            MessageViewModel.Dialog = Dialog;
            appNavig("home");         
        }

        private void setLogic()
        {
            DiscussionViewModel.Startup = _startup;
            MessageViewModel.Startup = _startup;
        }

        private void setInitEvents()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
            
            DiscussionViewModel.PropertyChanged += onChatRoomChange;
            DiscussionViewModel.PropertyChanged += onUpdateUsersStatusChange;
        }


        //----------------------------[ Properties ]------------------


        public Object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { setProperty(ref _currentViewModel, value); }
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

        public async void connectToServer()
        {
            try
            {
                // initialize the communication with the server
                int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                string ipAddress = ConfigurationManager.AppSettings["IP"];
                _clientSocket = new System.Net.Sockets.TcpClient();
                _serverStream = default(NetworkStream);

                // sign in the authenticated user on the server
                _clientSocket.Connect(ipAddress, port);
                DiscussionViewModel.ClientSocket = _clientSocket;
                DiscussionViewModel.ServerStream = _serverStream;
                _serverStream = _clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes((int)EServiceCommunication.Connected + "/" + _startup.Bl.BlSecurity.GetAuthenticatedUser().ID + "/0/" + _startup.Bl.BlSecurity.GetAuthenticatedUser().ID + "|" + "$");//textBox3.Text
                _serverStream.Write(outStream, 0, outStream.Length);
                _serverStream.Flush();

                Agent authenticatedUser = _startup.Bl.BlSecurity.GetAuthenticatedUser();
                authenticatedUser.IsOnline = true;
                var updatedUserList = await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { authenticatedUser });

                // create discussion thread
                Thread ctThread = new Thread(DiscussionViewModel.getMessage);
                ctThread.SetApartmentState(ApartmentState.STA);
                ctThread.Start();
            }
            catch (Exception ex)
            {
                _isServerConnectionError = true;
                CurrentViewModel = DiscussionViewModel;
                Log.error(ex.Message);
            }
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

        private void cleanUp()
        {
            if (_clientSocket != null && _clientSocket.Connected)
            {

                _clientSocket.GetStream().Close();
                _clientSocket.Close();
                _serverStream.Close();
            }
        }

        private void unSubscribeEvents()
        {
            // unsubscribe events
            DiscussionViewModel.PropertyChanged -= onChatRoomChange;
            DiscussionViewModel.PropertyChanged -= onUpdateUsersStatusChange;
        }

        private async Task signOutFromServer(List<DiscussionModel> discussionList)
        {
            try
            {
                if (_serverStream != null && discussionList.Count > 0)
                {
                    List<AgentModel> userList = new List<AgentModel>();
                    discussionList.Select(x => Utility.concat(userList, x.UserList)).First();// 
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes((int)EServiceCommunication.Disconnected + "/"+ _startup.Bl.BlSecurity.GetAuthenticatedUser().ID + "/0/" + DiscussionViewModel.generateDiscussionGroupName(discussionList[0].Discussion.ID, userList).Split('-')[1] + "$");//textBox3.Text
                    _serverStream.Write(outStream, 0, outStream.Length);
                    _serverStream.Flush();
                }
            }
            catch(Exception ex)
            {
                Log.error(ex.Message);
            }
            finally
            {
                // update user status to disconnected
                Agent authenticatedUser = _startup.Bl.BlSecurity.GetAuthenticatedUser();
                authenticatedUser.IsOnline = false;
                await _startup.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { authenticatedUser });
            }
        }

        public async override void Dispose()
        {      
            unSubscribeEvents();
            await chatLogOut(null);
            cleanUp();
            DiscussionViewModel.Dispose();
            MessageViewModel.Dispose();
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
                await Application.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    _main.AgentViewModel.getAgentOnlineStatus();
                }));
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                _startup = (_main.getObject("main") as BindBase).Startup;
                DiscussionViewModel.Startup = Startup;
                MessageViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                MessageViewModel.Dialog = Dialog;
                DiscussionViewModel.Dialog = Dialog;
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

        private async Task chatLogOut(object obj)
        {
            await signOutFromServer(DiscussionViewModel.DiscussionList);           
        }
    }
}
