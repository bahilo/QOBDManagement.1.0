using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using chatcommon.Interfaces;
using QOBDManagement.Classes;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using QOBDCommon.Classes;
using QOBDManagement.Command;
using QOBDCommon.Entities;
using QOBDManagement.Enums;
using System.Configuration;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace QOBDManagement.ViewModel
{
    public class DiscussionViewModel : BindBase, IDiscussionViewModel
    {
        private int _nbNewMessage;
        private IChatRoom _chatRoom;
        private string _inputMessage;
        private string _outputMessage;
        private const int _maxMessage = 5;
        private Func<object, object> _page;
        private IPEndPoint _endPoint;
        private UdpClient _udpClient;
        private IChatRoomViewModel _mainChatRoom;
        private List<DiscussionModel> _discussionList;
        private List<AgentModel> _userDiscussionGroupList;
        public static Dictionary<AgentModel, Tuple<Guid, UdpClient>> _clientsList;
        private NotifyTaskCompletion<bool> _discussionGroupCreationTask;

        //----------------------------[ Models ]------------------

        private DiscussionModel _discussionModel;
        private AgentModel _selectedAgentModel;


        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> SendMessageCommand { get; set; }
        public ButtonCommand<AgentModel> SelectUserForDiscussionCommand { get; set; }
        public ButtonCommand<AgentModel> SaveUserForDiscussionGroupCommand { get; set; }
        public ButtonCommand<object> OpenDiscussionGroupCommand { get; set; }
        public ButtonCommand<object> NavigToHomeCommand { get; set; }
        public ButtonCommand<object> ReadNewMessageCommand { get; set; }
        public ButtonCommand<AgentModel> AddUserToDiscussionCommand { get; set; }
        public ButtonCommand<string> GetDiscussionGroupCommand { get; set; }
        public ButtonCommand<string> GetIndividualDiscussionCommand { get; set; }



        public DiscussionViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        public DiscussionViewModel(IChatRoomViewModel mainChatRoom) : this()
        {
            _mainChatRoom = mainChatRoom;
            this._page = _mainChatRoom.navigation;
        }


        //----------------------------[ Initialization ]------------------


        private void initEvents()
        {
            PropertyChanged += onDiscussionModelChange;
            _discussionGroupCreationTask.PropertyChanged += onDiscussionGroupCreationTaskCompletion;
        }

        private void instances()
        {
            _endPoint = default(IPEndPoint);
            _discussionList = new List<DiscussionModel>();
            _userDiscussionGroupList = new List<AgentModel>();
            _clientsList = new Dictionary<AgentModel, Tuple<Guid, UdpClient>>();
            _discussionGroupCreationTask = new NotifyTaskCompletion<bool>();
        }


        private void instancesModel()
        {
            _selectedAgentModel = new AgentModel();
            _discussionModel = new DiscussionModel();
        }

        private void instancesCommand()
        {
            SendMessageCommand = new ButtonCommand<string>(broadcast, canBroadcast);
            SelectUserForDiscussionCommand = new ButtonCommand<AgentModel>(selectUserForDiscussion, canSelectUserForDiscussion);
            SaveUserForDiscussionGroupCommand = new ButtonCommand<AgentModel>(saveUserForDiscussionGroup, canSelectUserForDiscussion);
            OpenDiscussionGroupCommand = new ButtonCommand<object>(displayDiscussionGroupMenu, canDisplayDiscussionGroupMenu);
            NavigToHomeCommand = new ButtonCommand<object>(goToHomePage, canGoToHomePage);
            ReadNewMessageCommand = new ButtonCommand<object>(readNewMessages, canReadNewMessages);
            AddUserToDiscussionCommand = new ButtonCommand<AgentModel>(addUserToCurrentDiscussion, canAddUserToCurrentDiscussion);
            GetDiscussionGroupCommand = new ButtonCommand<string>(getDiscussionGroup, canGetDiscussionGroup);
            GetIndividualDiscussionCommand = new ButtonCommand<string>(getIndividualDiscussion, canGetIndividualDiscussion);
        }


        //----------------------------[ Properties ]------------------

        public int MaxMessageLength
        {
            get { return _maxMessage; }
        }

        public IChatRoomViewModel MainChatRoom
        {
            get { return _mainChatRoom; }
        }

        public Agent AuthenticatedUser
        {
            get { return BL.BlSecurity.GetAuthenticatedUser(); }
        }

        public string TxtNbNewMessage
        {
            get { return _nbNewMessage.ToString(); }
            set { setProperty(ref _nbNewMessage, Utility.intTryParse(value)); }
        }

        public QOBDBusiness.BusinessLogic BL
        {
            get { return _startup.Bl; }
        }

        public DiscussionModel DiscussionModel
        {
            get { return _discussionModel; }
            set { setProperty(ref _discussionModel, value); }
        }

        public IChatRoom ChatRoom
        {
            get { return _chatRoom; }
            set { setProperty(ref _chatRoom, value); }
        }

        public AgentModel SelectedAgentModel
        {
            get { return _selectedAgentModel; }
            set { setProperty(ref _selectedAgentModel, value); }
        }

        public List<DiscussionModel> DiscussionList
        {
            get { return _discussionList; }
            set { setProperty(ref _discussionList, value); }
        }

        public string InputMessage
        {
            get { return _inputMessage; }
            set { setProperty(ref _inputMessage, value); }
        }

        public string OutputMessage
        {
            get { return _outputMessage; }
            set { setProperty(ref _outputMessage, value); }
        }

        public System.Net.Sockets.UdpClient UdpClient
        {
            get { return _udpClient; }
            set { setProperty(ref _udpClient, value); }
        }

        public IPEndPoint EndPoint
        {
            get { return _endPoint; }
            set { setProperty(ref _endPoint, value); }
        }

        public Dictionary<AgentModel, Tuple<Guid, UdpClient>> ServerClientsDictionary
        {
            get { return _clientsList; }
            set { setProperty(ref _clientsList, value); }
        }

        public string ByeMessage
        {
            get { return (int)EServiceCommunication.Disconnected + "/" + BL.BlSecurity.GetAuthenticatedUser().ID + "/0/" + BL.BlSecurity.GetAuthenticatedUser().ID + "|" + "$"; }
        }

        public string WelcomeMessage
        {
            get { return (int)EServiceCommunication.Connected + "/" + BL.BlSecurity.GetAuthenticatedUser().ID + "/0/" + BL.BlSecurity.GetAuthenticatedUser().ID + "|" + "$"; }
        }


        //----------------------------[ Actions ]------------------


        public void load()
        {
            Dialog.showSearch("Loading...", isChatDialogBox: true);

            // find the discussion where the selected user appears
            List<DiscussionModel> discussionFoundList = new List<DiscussionModel>();

            if (string.IsNullOrEmpty(DiscussionModel.TxtGroupName))
                discussionFoundList = DiscussionList.Where(x => x.UserList.Where(y => y.Agent.ID == SelectedAgentModel.Agent.ID).Count() > 0 && x.UserList.Count == 1).ToList();
            else
                discussionFoundList = DiscussionList.Where(x => x.TxtGroupName == DiscussionModel.TxtGroupName).ToList();

            // display discussion messages
            if (discussionFoundList.Count > 0)
            {
                DiscussionModel = discussionFoundList[0];
                var messageList = DiscussionModel.MessageList.Where(x => x.Message.DiscussionId == DiscussionModel.Discussion.ID).OrderByDescending(x => x.Message.ID).Take(MaxMessageLength).ToList();// (await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = DiscussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND)).OrderByDescending(x => x.ID).Take(5).ToList();
                foreach (var messageModel in messageList.OrderBy(x => x.Message.ID).ToList())
                {
                    Agent user = DiscussionModel.UserList.Where(x => x.Agent.ID == messageModel.Message.UserId).Select(x => x.Agent).FirstOrDefault();
                    if (user != null)
                        displayMessage(messageModel.Message, user);
                    else
                        displayMessage(messageModel.Message, AuthenticatedUser);
                }
                // update the displayed group name ( calling the on property change event )
                DiscussionModel.TxtGroupName = DiscussionModel.TxtGroupName;
            }

            Dialog.IsChatDialogOpen = false;
        }

        public async Task<List<DiscussionModel>> retrieveUserDiscussions(Agent user)
        {
            object _lock = new object();
            List<User_discussion> allUser_discussionOfAuthencatedUserList = new List<User_discussion>();
            lock (_lock)
                DiscussionList = new List<DiscussionModel>();

            if (user.ID != 0)
                allUser_discussionOfAuthencatedUserList = await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { UserId = user.ID }, QOBDCommon.Enum.ESearchOption.AND);

            _nbNewMessage = 0;

            foreach (User_discussion user_discussionOfAuthenticatedUser in allUser_discussionOfAuthencatedUserList)
            {
                if (DiscussionList.Where(x => x.Discussion.ID == user_discussionOfAuthenticatedUser.DiscussionId).Count() == 0)
                {
                    // Get All users appearing in the same discusion as the authenticated user 
                    List<User_discussion> allUser_discussionOfOtherUserList = (await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { DiscussionId = user_discussionOfAuthenticatedUser.DiscussionId }, QOBDCommon.Enum.ESearchOption.AND)).Where(x => x.UserId != user.ID).ToList();

                    List<Discussion> discussionList = await BL.BlChatRoom.GetDiscussionDataByIdAsync(user_discussionOfAuthenticatedUser.DiscussionId);
                    if (discussionList.Count > 0)
                    {
                        DiscussionModel discussionModel = new DiscussionModel();
                        discussionModel.Discussion = discussionList[0];

                        // retrieving the discussion's messages
                        discussionModel.addMessage(await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND));

                        // display the number of unread messages
                        if (user_discussionOfAuthenticatedUser.Status == 1)
                        {
                            _nbNewMessage += 1;
                            var lastMessage = discussionModel.MessageList.Where(x => x.Message.DiscussionId == user_discussionOfAuthenticatedUser.DiscussionId).OrderByDescending(x => x.Message.ID).FirstOrDefault();
                            if (lastMessage != null)
                                lastMessage.IsNewMessage = true;
                        }

                        // Save all discussions and their users
                        foreach (User_discussion user_discussionOfOthers in allUser_discussionOfOtherUserList)
                        {
                            if (discussionModel.UserList.Where(x => x.Agent.ID == user_discussionOfOthers.UserId).Count() == 0)
                            {
                                AgentModel agentModelFound = _mainChatRoom.AgentViewModel.AgentModelList.Where(x => x.Agent.ID == user_discussionOfOthers.UserId).SingleOrDefault();
                                if (discussionList.Count > 0 && agentModelFound != null)
                                {
                                    discussionModel.addUser(agentModelFound);
                                }
                            }
                        }
                        lock (_lock)
                            DiscussionList.Add(discussionModel);
                    }
                }
            }

            // display the number of unread messages updating ( calling the on property change event )
            onPropertyChange("TxtNbNewMessage");

            // update the discussion List content ( calling the on property change event )
            onPropertyChange("DiscussionList");

            return DiscussionList;
        }

        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "chatroom":
                    _page(this);
                    break;
                case "home":
                    _page(new MessageViewModel());
                    break;
                default:
                    goto case "home";
            }
        }

        public async void getMessage()
        {
            try
            {
                while (true)
                {
                    int discussionId = 0;
                    int userId = 0;
                    int messageId = 0;
                    List<string> composer = new List<string>();
                    string returndata = "";

                    byte[] inStream = UdpClient.Receive(ref _endPoint);
                    returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    returndata = returndata.Substring(0, returndata.IndexOf("$"));
                    
                    composer = returndata.Split('/').ToList();
                    int.TryParse(composer[0], out discussionId);
                    int.TryParse(composer[1], out userId);

                    if (composer.Count > 3
                            && int.TryParse(composer[0], out discussionId)
                                && int.TryParse(composer[1], out userId)
                                    && int.TryParse(composer[2], out messageId)
                                        && discussionId != (int)EServiceCommunication.Disconnected
                                            && discussionId != (int)EServiceCommunication.Connected)
                    {
                        var message = new Message { ID = messageId, DiscussionId = discussionId, UserId = userId, Content = composer[4], Date = Utility.convertToDateTime(composer[5]) };
                        var userFoundList = BL.BlAgent.searchAgent(new Agent { ID = userId }, QOBDCommon.Enum.ESearchOption.AND);

                        if (discussionId == DiscussionModel.Discussion.ID)
                        {
                            // new user to the current discussion detected  
                            List<string> discussionUserIDs = composer[3].Split('|').Where(x => !string.IsNullOrEmpty(x)).ToList();
                            if (discussionUserIDs.Count() > DiscussionModel.UserList.Count)
                            {
                                updateDiscussionUserList(DiscussionModel, discussionUserIDs);
                                displayMessage(message, userFoundList[0]);
                            }

                            // display the new incoming message
                            else if (DiscussionModel.MessageList.Where(x => x.Message.ID == messageId).Count() == 0)
                            {
                                if (message.ID > 0 && userFoundList.Count > 0)
                                {
                                    DiscussionModel.addMessage(new MessageModel { Message = message });
                                    displayMessage(message, userFoundList[0]);
                                }
                            }
                        }

                        // notification of a new incoming message
                        else
                        {
                            if (message.ID > 0)
                            {
                                var discussionModelFound = DiscussionList.Where(x => x.Discussion.ID == discussionId).FirstOrDefault();
                                if (discussionModelFound != null)
                                {
                                    discussionModelFound.addMessage(new MessageModel { Message = message, IsNewMessage = true });
                                    TxtNbNewMessage = (Utility.intTryParse(TxtNbNewMessage) + 1).ToString();
                                }
                                else
                                    await retrieveUserDiscussions(AuthenticatedUser);

                                System.Media.SystemSounds.Asterisk.Play();
                            }
                        }

                        // checking if user already connected (same user id)
                        AgentModel agentFound = (await BL.BlAgent.searchAgentAsync(new Agent { ID = userId }, QOBDCommon.Enum.ESearchOption.AND)).Select(x => new AgentModel { Agent = x }).SingleOrDefault();
                        if (agentFound != null)
                        {
                            if (!string.IsNullOrEmpty(agentFound.TxtIPAddress))
                            {
                                int port = Utility.intTryParse(agentFound.TxtIPAddress.Split(':')[1]);
                                string ipAddress = agentFound.TxtIPAddress.Split(':')[0];
                                setUserCommunicationSocket(agentFound, new Tuple<Guid, UdpClient>(Guid.NewGuid(), new UdpClient(ipAddress, port)), discussionId);
                            }
                        }
                    }

                    // update the authenticated user online status
                    if (userId != AuthenticatedUser.ID && ( discussionId == (int)EServiceCommunication.Connected || discussionId == (int)EServiceCommunication.Disconnected) )
                        onPropertyChange("updateStatus");
                }
            }
            catch (System.IO.IOException) { }
            catch (System.Net.Sockets.SocketException) { }
            catch (Exception ex)
            {
                Log.error("<["+ BL.BlSecurity.GetAuthenticatedUser().UserName+ "]Localhost =" + BL.BlSecurity.GetAuthenticatedUser().IPAddress + "> " + ex.Message, QOBDCommon.Enum.EErrorFrom.CHATROOM);
                throw;
            }
        }

        public List<Agent> updateDiscussionUserList(DiscussionModel discussionModel, List<string> userIDList)
        {
            List<Agent> newUserList = new List<Agent>();
            foreach (string id in userIDList)
            {
                if (id != AuthenticatedUser.ID.ToString() && discussionModel.UserList.Where(x => x.TxtID == id).Count() == 0)
                {
                    var userFoundList = BL.BlAgent.searchAgent(new Agent { ID = Utility.intTryParse(id) }, QOBDCommon.Enum.ESearchOption.AND);
                    if (userFoundList.Count > 0)
                    {
                        discussionModel.addUser(userFoundList);
                        newUserList.Add(userFoundList[0]);
                    }
                }
            }
            return newUserList;
        }


        public void displayMessage(Message message, Agent user)
        {
            if (user != null)
            {
                // update the discussion messages status from unread (status = 1) to read (status = 0)
                markMessageAsRead(DiscussionModel, new MessageModel { Message = message });

                if (AuthenticatedUser.ID == message.UserId)
                    msg("reply", message.Date + Environment.NewLine + message.Content);
                else
                    msg("recipient", user.UserName + " Says:" + Environment.NewLine + message.Date + Environment.NewLine + message.Content);
            }
        }

        public void msg(string type, string message)
        {
            if (ChatRoom != null)
                switch (type)
                {
                    case "info":
                        ChatRoom.showInfo(message);
                        break;
                    case "reply":
                        ChatRoom.showMyReply(Environment.NewLine + " >> " + message);
                        break;
                    case "recipient":
                        ChatRoom.showRecipientReply(Environment.NewLine + " >> " + message);
                        break;
                }
        }

        private async void validateDiscussionGroup()
        {
            if (_userDiscussionGroupList.Count > 0)
            {
                var discussionCreatedList = await BL.BlChatRoom.InsertDiscussionAsync(new List<Discussion> { new Discussion { Date = DateTime.Now } });
                if (discussionCreatedList.Count > 0)
                {
                    // creating the link between the user and the discussion
                    _userDiscussionGroupList.Add(new AgentModel { Agent = AuthenticatedUser });
                    var user_discussionCreatedList = await BL.BlChatRoom.InsertUser_discussionAsync(_userDiscussionGroupList.Select(x => new User_discussion { DiscussionId = discussionCreatedList[0].ID, UserId = x.Agent.ID }).ToList());

                    // setting the current dicussion to the new discussion
                    DiscussionModel = new DiscussionModel { Discussion = discussionCreatedList[0] };
                    DiscussionModel.addUser(_userDiscussionGroupList.Where(x => x.Agent.ID != AuthenticatedUser.ID).ToList());
                    _selectedAgentModel = _userDiscussionGroupList[0];

                    // navigate to the discussion view
                    executeNavig("chatroom");
                }
            }
        }

        /// <summary>
        /// Mak the discussion messages as unread for the other member of the discussion
        /// </summary>
        /// <param name="discussionModel">The current discussion</param>
        public async void markMessageAsUnRead(DiscussionModel discussionModel, MessageModel messageModel)
        {
            List<User_discussion> authenticatedUserDiscussionList = (await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND)).Where(x => x.UserId != AuthenticatedUser.ID).ToList();
            authenticatedUserDiscussionList = authenticatedUserDiscussionList.Select(x => new User_discussion { ID = x.ID, DiscussionId = x.DiscussionId, UserId = x.UserId, Status = 1 }).ToList();
            await BL.BlChatRoom.UpdateUser_discussionAsync(authenticatedUserDiscussionList);

            MessageModel messageFound = discussionModel.MessageList.Where(x => x.Message.DiscussionId == discussionModel.Discussion.ID && x.Message.ID == messageModel.Message.ID).FirstOrDefault();
            if (messageFound != null)
                messageFound.IsNewMessage = true;
        }

        /// <summary>
        /// Mark the discussion messages as read for the authenticated member
        /// </summary>
        /// <param name="discussionModel">The current discussion</param>
        public async void markMessageAsRead(DiscussionModel discussionModel, MessageModel messageModel)
        {
            // set the discussion message as read
            List<User_discussion> authenticatedUserDiscussionList = await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { DiscussionId = discussionModel.Discussion.ID, UserId = AuthenticatedUser.ID }, QOBDCommon.Enum.ESearchOption.AND);
            authenticatedUserDiscussionList = authenticatedUserDiscussionList.Select(x => new User_discussion { ID = x.ID, DiscussionId = x.DiscussionId, UserId = x.UserId, Status = 0 }).ToList();
            await BL.BlChatRoom.UpdateUser_discussionAsync(authenticatedUserDiscussionList);

            MessageModel messageFound = discussionModel.MessageList.Where(x => x.IsNewMessage && x.Message.ID == messageModel.Message.ID).FirstOrDefault();
            if (messageFound != null)
            {
                messageFound.IsNewMessage = false;

                // reset the unread message notification
                TxtNbNewMessage = ((_nbNewMessage > 0) ? _nbNewMessage - 1 : 0).ToString();
            }
        }

        /// <summary>
        /// sending the authenticated user last message
        /// </summary>
        /// <param name="obj"></param>
        private void sendMessage(UdpClient udpClient, string messageToSend)
        {
            UdpClient broadcastSocket;
            broadcastSocket = udpClient;
            try
            {
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(messageToSend);
                broadcastSocket.Send(outStream, outStream.Length);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Log.error("<[" + BL.BlSecurity.GetAuthenticatedUser().UserName + "]Localhost =" + BL.BlSecurity.GetAuthenticatedUser().IPAddress + "> " + ex.Message, QOBDCommon.Enum.EErrorFrom.CHATROOM);
                msg("info", "Error while trying to send the message!");
            }
            finally
            {
                InputMessage = "";
            }

            Dialog.IsChatDialogOpen = false;
        }

        private async Task<Message> saveMessageToDBAsync(Message message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                Message savedMdessage = (await BL.BlChatRoom.InsertMessageAsync(new List<Message> { message })).FirstOrDefault();
                if (savedMdessage != null)
                {
                    DiscussionModel.addMessage(new MessageModel { Message = savedMdessage });
                    markMessageAsUnRead(DiscussionModel, new MessageModel { Message = savedMdessage });
                    displayMessage(savedMdessage, AuthenticatedUser);
                    return savedMdessage;
                }
            }
            return new Message();
        }

        public async Task broadcastMessageAsync(string message)
        {
            // broadcast the message to the current discussion users only
            if (DiscussionModel.Discussion.ID != 0 && SelectedAgentModel.Agent.ID != 0)
            {
                foreach (var agentModel in DiscussionModel.UserList)
                {
                    try
                    {
                        AgentModel agentFound = (await BL.BlAgent.searchAgentAsync(new Agent { ID = agentModel.Agent.ID }, QOBDCommon.Enum.ESearchOption.AND)).Select(x => new AgentModel { Agent = x }).SingleOrDefault();
                        if (agentFound != null)
                        {
                            if (message.Split('/').Count() > 2)
                                sendMessage(setUserCommunicationSocket(agentFound, null, Utility.intTryParse(message.Split('/')[0])), message);
                            else
                                sendMessage(setUserCommunicationSocket(agentFound, null), message);
                        }
                    }
                    catch (Exception)
                    { }
                }
            }

            // broadcasting message to all users
            else
            {
                foreach (AgentModel agentModel in MainChatRoom.AgentViewModel.AgentModelList.Where(x => x.Agent.ID != AuthenticatedUser.ID).ToList())
                {
                    try
                    {
                        AgentModel agentFound = (await BL.BlAgent.searchAgentAsync(new Agent { ID = agentModel.Agent.ID }, QOBDCommon.Enum.ESearchOption.AND)).Select(x => new AgentModel { Agent = x }).SingleOrDefault();
                        if (agentFound != null)
                        {
                            if (message.Split('/').Count() > 2)
                                sendMessage(setUserCommunicationSocket(agentFound, null, Utility.intTryParse(message.Split('/')[0])), message);
                            else
                                sendMessage(setUserCommunicationSocket(agentFound, null), message);
                        }
                        //sendMessageToDiscussionUser(agentModel, message);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private UdpClient setUserCommunicationSocket(AgentModel agentModel, Tuple<Guid, UdpClient> udpClient, int discussionId = 0)
        {
            int port = 0;
            UdpClient outputUdpClient = default(UdpClient);
            string ipAddress = agentModel.TxtIPAddress.Split(':')[0];
            int.TryParse(agentModel.TxtIPAddress.Split(':')[1], out port);

            // checking if user already connected
            var clientsToUpdate = ServerClientsDictionary.Where(x => x.Key.Agent.ID == agentModel.Agent.ID).Select(x => x.Key).SingleOrDefault();
            if (clientsToUpdate != null)
            {
                // check the socket for update
                if (udpClient != null && ServerClientsDictionary[clientsToUpdate].Item1.ToString() != udpClient.Item1.ToString())
                {
                    ServerClientsDictionary[clientsToUpdate] = udpClient;
                    outputUdpClient = udpClient.Item2;
                }
                else
                    outputUdpClient = ServerClientsDictionary[clientsToUpdate].Item2;
            }
            else if (discussionId != (int)EServiceCommunication.Disconnected)
            {
                var agentFound = MainChatRoom.AgentViewModel.AgentModelList.SingleOrDefault(x => x.Agent.ID == agentModel.Agent.ID);
                if (agentFound != null)
                {
                    var tcpClientTuple = new Tuple<Guid, UdpClient>(Guid.NewGuid(), new UdpClient(ipAddress, port));
                    ServerClientsDictionary.Add(agentFound, tcpClientTuple);
                    outputUdpClient = tcpClientTuple.Item2;
                }
            }

            return outputUdpClient;
        }

        public override void Dispose()
        {
            base.Dispose();
            PropertyChanged -= onDiscussionModelChange;
            _discussionGroupCreationTask.PropertyChanged -= onDiscussionGroupCreationTaskCompletion;
        }

        //----------------------------[ Event Handler ]------------------

        private void onDiscussionGroupCreationTaskCompletion(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                if (Dialog.Response)
                    validateDiscussionGroup();
                else
                    executeNavig("home");
            }
        }

        private void onDiscussionModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "DiscussionModel"))
            {
                AddUserToDiscussionCommand.raiseCanExecuteActionChanged();
            }
        }


        //----------------------------[ Action Commands ]------------------

        /// <summary>
        /// broadcast message to discussion members ()
        /// </summary>
        /// <param name="msg">message to send (discussion ID / sender ID / message ID / discussion members IDs)</param>
        /// <param name="flag"></param>
        public async void broadcast(string msg)//(string msg, bool flag = false)
        {
            // creating and saving a new discussion
            if (DiscussionModel.Discussion.ID == 0 && SelectedAgentModel.Agent.ID != 0)
            {
                Dialog.showSearch("Creating a discussion with " + SelectedAgentModel.TxtLogin + "...", isChatDialogBox: true);
                var discussionCreatedList = await BL.BlChatRoom.InsertDiscussionAsync(new List<Discussion> { new Discussion { Date = DateTime.Now } });
                if (discussionCreatedList.Count > 0)
                {
                    var user_discussionCreatedList = await BL.BlChatRoom.InsertUser_discussionAsync(new List<User_discussion> {
                            new User_discussion { DiscussionId = discussionCreatedList[0].ID, UserId = SelectedAgentModel.Agent.ID },
                            new User_discussion { DiscussionId = discussionCreatedList[0].ID, UserId = AuthenticatedUser.ID }
                        });
                    var discussionList = await retrieveUserDiscussions(AuthenticatedUser);
                    DiscussionModel = discussionList.Where(x => x.Discussion.ID == discussionCreatedList[0].ID).FirstOrDefault() ?? new DiscussionModel { Discussion = discussionCreatedList[0] };
                }
            }

            Message messageToSend = new Message { DiscussionId = DiscussionModel.Discussion.ID, Content = InputMessage, Date = DateTime.Now, UserId = AuthenticatedUser.ID };
            Message savedMessage = await saveMessageToDBAsync(messageToSend);

            // creating the message for
            if (msg.Split('/').Count() < 2)
            {
                msg = DiscussionModel.TxtID;
                msg += "/" + AuthenticatedUser.ID;
                msg += "/" + savedMessage.ID;
                msg += "/" + DiscussionModel.TxtGroupName.Split('-')[1];
                msg += "/" + savedMessage.Content;
                msg += "/" + savedMessage.Date.ToString("yyyy-MM-dd H:mm:ss");
                msg += "$";
            }


            await broadcastMessageAsync(msg);
            Dialog.IsChatDialogOpen = false;
        }

        private bool canBroadcast(string arg)
        {
            return true;
        }

        private void selectUserForDiscussion(AgentModel obj)
        {
            Dialog.IsLeftBarClosed = false;
            DiscussionModel = new DiscussionModel();
            DiscussionModel.IsGroupDiscussion = false;
            SelectedAgentModel = obj;

            // navig to discussion page
            executeNavig("chatroom");
        }

        private bool canSelectUserForDiscussion(AgentModel arg)
        {
            return true;
        }

        public void saveUserForDiscussionGroup(AgentModel param)
        {
            if (!_userDiscussionGroupList.Contains(param))
                _userDiscussionGroupList.Add(param);
            else
                _userDiscussionGroupList.Remove(param);
        }

        private bool canaveUserForDiscussionGroup(AgentModel arg)
        {
            return true;
        }

        private void displayDiscussionGroupMenu(object obj)
        {
            _discussionGroupCreationTask.initializeNewTask(Dialog.showAsync(new Views.ChatGroup(), isChatDialogBox: true));
        }

        private bool canDisplayDiscussionGroupMenu(object arg)
        {
            return true;
        }

        private void goToHomePage(object obj)
        {
            executeNavig("home");
        }

        private bool canGoToHomePage(object arg)
        {
            return true;
        }

        public void readNewMessages(object obj)
        {
            goToHomePage(obj);
        }

        private bool canReadNewMessages(object arg)
        {
            return true;
        }

        private async void addUserToCurrentDiscussion(AgentModel obj)
        {
            if (DiscussionModel != null && DiscussionModel.Discussion.ID != 0 && DiscussionModel.addUser(obj))
            {
                Dialog.showSearch("Adding " + obj.TxtLogin + " to discussion...", isChatDialogBox: true);
                Dialog.IsLeftBarClosed = false;
                var user_discussionSavedList = await BL.BlChatRoom.InsertUser_discussionAsync(new List<User_discussion> { new User_discussion { DiscussionId = DiscussionModel.Discussion.ID, UserId = obj.Agent.ID } });
                Dialog.IsChatDialogOpen = false;
            }            
        }

        private bool canAddUserToCurrentDiscussion(AgentModel arg)
        {
            if (arg != null && DiscussionModel != null && DiscussionModel.Discussion.ID != 0 && DiscussionModel.UserList.Where(x => x.Agent.ID == arg.Agent.ID).Count() == 0)
                return true;

            return false;
        }

        private void getDiscussionGroup(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                Dialog.IsLeftBarClosed = false;
                DiscussionModel = new DiscussionModel();
                SelectedAgentModel = new AgentModel { TxtID = obj.Split('-')[1].Split('|')[0] };
                DiscussionModel.IsGroupDiscussion = true;
                DiscussionModel.TxtGroupName = obj;
                executeNavig("chatroom");
            }
        }

        private bool canGetDiscussionGroup(string arg)
        {
            return true;
        }

        private void getIndividualDiscussion(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                Dialog.IsLeftBarClosed = false;
                SelectedAgentModel = new AgentModel { TxtID = obj.Split('-')[1].Split('|')[0] };
                DiscussionModel.IsGroupDiscussion = false;
                DiscussionModel.TxtGroupName = obj;
                executeNavig("chatroom");
            }
        }

        private bool canGetIndividualDiscussion(string arg)
        {
            return true;
        }




    }
}
