using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace QOBDManagement.ViewModel
{
    public class DiscussionViewModel : BindBase, IDiscussionViewModel
    {
        private const int _maxMessage = 5;
        private string _inputMessage;
        private string _outputMessage;
        private int _nbNewMessage;
        private System.Net.Sockets.TcpClient _clientSocket;
        private NetworkStream _serverStream;
        private IChatRoom _chatRoom;
        private List<AgentModel> _userDiscussionGroupList;
        private List<DiscussionModel> _discussionList;
        private Func<object, object> _page;
        private NotifyTaskCompletion<bool> _discussionGroupCreationTask;
        //private bool _isGroupDiscussion;
        //private string _groupId;


        //----------------------------[ Models ]------------------

        private DiscussionModel _discussionModel;
        private AgentModel _selectedAgentModel;
        private IAgentViewModel _agentViewModel;


        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> SendMessageCommand { get; set; }
        public ButtonCommand<AgentModel> SelectUserForDiscussionCommand { get; set; }
        public ButtonCommand<AgentModel> SaveUserForDiscussionGroupCommand { get; set; }
        public ButtonCommand<object> DiscussionGroupCreationCommand { get; set; }
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

        public DiscussionViewModel(Func<object, object> navigation) : this()
        {
            this._page = navigation;
        }

        public DiscussionViewModel(Func<object, object> navigation, IAgentViewModel agentViewModel) : this(navigation)
        {
            _agentViewModel = agentViewModel;
        }


        //----------------------------[ Initialization ]------------------


        private void initEvents()
        {
            PropertyChanged += onDiscussionModelChange;
            _discussionGroupCreationTask.PropertyChanged += onDiscussionGroupCreationTaskCompletion;
        }

        private void instances()
        {
            _clientSocket = new System.Net.Sockets.TcpClient();
            _serverStream = default(NetworkStream);
            _userDiscussionGroupList = new List<AgentModel>();
            _discussionList = new List<DiscussionModel>();
            _discussionGroupCreationTask = new NotifyTaskCompletion<bool>();
        }


        private void instancesModel()
        {
            _selectedAgentModel = new AgentModel();
            _discussionModel = new DiscussionModel();
        }

        private void instancesCommand()
        {
            SendMessageCommand = new ButtonCommand<object>(sendMessage, canSendMessage);
            SelectUserForDiscussionCommand = new ButtonCommand<AgentModel>(selectUserForDiscussion, canSelectUserForDiscussion);
            SaveUserForDiscussionGroupCommand = new ButtonCommand<AgentModel>(saveUserForDiscussionGroup, canSelectUserForDiscussion);
            DiscussionGroupCreationCommand = new ButtonCommand<object>(createDiscussionGroup, canCreateDiscussionGroup);
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

        /*public bool IsGroupDiscussion
        {
            get { return _isGroupDiscussion; }
            set { setProperty(ref _isGroupDiscussion, value); }
        }*/

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

        public System.Net.Sockets.TcpClient ClientSocket
        {
            get { return _clientSocket; }
            set { setProperty(ref _clientSocket, value); }
        }

        public NetworkStream ServerStream
        {
            get { return _serverStream; }
            set { setProperty(ref _serverStream, value); }
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

            if(user.ID != 0)
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
                                List<Agent> userFoundList = BL.BlAgent.searchAgent(new Agent { ID = user_discussionOfOthers.UserId }, QOBDCommon.Enum.ESearchOption.AND);
                                if (discussionList.Count > 0 && userFoundList.Count > 0)
                                    discussionModel.addUser(new AgentModel { Agent = userFoundList[0] });
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

                    _serverStream = _clientSocket.GetStream();
                    int buffSize = 0;
                    buffSize = _clientSocket.ReceiveBufferSize;
                    byte[] inStream = new byte[buffSize];
                    _serverStream.Read(inStream, 0, buffSize);
                    returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    returndata = returndata.Substring(0, returndata.IndexOf("$"));
                    composer = returndata.Split('/').ToList();

                    // current discussion management
                    if (int.TryParse(composer[0], out discussionId)
                        && int.TryParse(composer[1], out userId)
                            && int.TryParse(composer[2], out messageId)
                                && discussionId > 0)
                    {
                        var messageFoundList = await BL.BlChatRoom.GetMessageDataByIdAsync(messageId);
                        var userFoundList = BL.BlAgent.searchAgent(new Agent { ID = userId }, QOBDCommon.Enum.ESearchOption.AND);

                        if (discussionId == DiscussionModel.Discussion.ID)
                        {
                            // new user to the current discussion detected  
                            List<string> discussionUserIDs = composer[3].Split('|').Where(x => !string.IsNullOrEmpty(x)).ToList();
                            if (discussionUserIDs.Count() > DiscussionModel.UserList.Count)
                            {
                                updateDiscussionUserList(DiscussionModel, discussionUserIDs);
                                displayMessage(messageFoundList[0], userFoundList[0]);
                            }

                            // display the new incoming message
                            else if (DiscussionModel.MessageList.Where(x => x.Message.ID == messageId).Count() == 0)
                            {
                                if (messageFoundList.Count > 0 && userFoundList.Count > 0)
                                {
                                    DiscussionModel.addMessage(messageFoundList);
                                    displayMessage(messageFoundList[0], userFoundList[0]);
                                }
                            }
                        }

                        // notification of a new incoming message
                        else
                        {
                            if (messageFoundList.Count > 0)
                            {
                                var discussionModelFound = DiscussionList.Where(x => x.Discussion.ID == discussionId).FirstOrDefault();
                                if (discussionModelFound != null)
                                {
                                    discussionModelFound.addMessage(messageFoundList.Select(x => new MessageModel { Message = x, IsNewMessage = true }).ToList());
                                    TxtNbNewMessage = (_nbNewMessage + 1).ToString();
                                }                                    
                                else
                                    await retrieveUserDiscussions(AuthenticatedUser);
                                
                                System.Media.SystemSounds.Asterisk.Play();
                            }
                        }
                    }

                    // update the authenticated user online status
                    else if (userId != AuthenticatedUser.ID)
                        onPropertyChange("updateStatus");

                }
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }
        }

        private List<Agent> updateDiscussionUserList(DiscussionModel discussionModel, List<string> userIDList)
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
        /// sending the authenticated user last message
        /// </summary>
        /// <param name="obj"></param>
        private async void sendMessage(object obj)
        {
            if (!string.IsNullOrEmpty(InputMessage) && SelectedAgentModel.Agent.ID != 0)
            {
                // creating and saving a new discussion
                if (DiscussionModel.Discussion.ID == 0)
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

                // saving and sending the authenticated user last message
                try
                {
                    Message inputMessage = new Message { DiscussionId = DiscussionModel.Discussion.ID, Content = InputMessage, Date = DateTime.Now, UserId = AuthenticatedUser.ID };
                    displayMessage(inputMessage, AuthenticatedUser);

                    Message savedMdessage = (await BL.BlChatRoom.InsertMessageAsync(new List<Message> { inputMessage })).FirstOrDefault();
                    if (savedMdessage != null)
                    {
                        DiscussionModel.addMessage(new MessageModel { Message = savedMdessage });
                        markMessageAsUnRead(DiscussionModel, new MessageModel { Message = savedMdessage });
                        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(DiscussionModel.TxtID + "/" + AuthenticatedUser.ID + "/" + savedMdessage.ID + "/" + DiscussionModel.TxtGroupName.Split('-')[1] + "/" + InputMessage + "$");
                        _serverStream.Write(outStream, 0, outStream.Length);
                        _serverStream.Flush();
                        InputMessage = "";
                    }
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message);
                    msg("info", "Error while trying to send the message!");
                }
            }
            Dialog.IsChatDialogOpen = false;
        }

        private bool canSendMessage(object arg)
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

        private void createDiscussionGroup(object obj)
        {
            _discussionGroupCreationTask.initializeNewTask(Dialog.showAsync(new Views.ChatGroup(), isChatDialogBox: true));
        }

        private bool canCreateDiscussionGroup(object arg)
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
            Dialog.showSearch("Adding " + obj.TxtLogin + " to discussion...", isChatDialogBox: true);
            Dialog.IsLeftBarClosed = false;
            var user_discussionSavedList = await BL.BlChatRoom.InsertUser_discussionAsync(new List<User_discussion> { new User_discussion { DiscussionId = DiscussionModel.Discussion.ID, UserId = obj.Agent.ID } });
            DiscussionModel.addUser(obj);
            Dialog.IsChatDialogOpen = false;
        }

        private bool canAddUserToCurrentDiscussion(AgentModel arg)
        {
            if (DiscussionModel.Discussion.ID == 0)
                return false;

            if (arg != null && DiscussionModel.UserList.Where(x => x.Agent.ID == arg.Agent.ID).Count() > 0)
                return false;

            return true;
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
                DiscussionModel = new DiscussionModel();
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
