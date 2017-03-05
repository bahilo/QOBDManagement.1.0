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
        private bool _isGroupDiscussion;
        private string _groupId;


        //----------------------------[ Models ]------------------

        private DiscussionModel _discussionModel;
        private AgentModel _selectedAgentModel;


        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> SendMessageCommand { get; set; }
        public ButtonCommand<AgentModel> SelectUserForDiscussionCommand { get; set; }
        public ButtonCommand<AgentModel> SaveUserForDiscussionGroupCommand { get; set; }
        public ButtonCommand<object> DiscussionGroupCreationCommand { get; set; }
        public ButtonCommand<object> NavigToHomeCommand { get; set; }
        public ButtonCommand<object> ReadNewMessageCommand { get; set; }
        public ButtonCommand<AgentModel> AddUserToDiscussionCommand { get; set; }
        public ButtonCommand<string> GetDiscussionGroupCommand { get; set; }



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
        }


        //----------------------------[ Properties ]------------------


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

        public bool IsGroupDiscussion
        {
            get { return _isGroupDiscussion; }
            set { setProperty(ref _isGroupDiscussion, value); }
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


        public async void load()
        {
            Dialog.showSearch("Loading...", isChatDialogBox: true);
            // get all discussions where the authenticated user appears

            // find the discussion where the selected user appears
            List<DiscussionModel> discussionFoundList = new List<DiscussionModel>();
            if (IsGroupDiscussion)
                discussionFoundList = DiscussionList.Where(x => x.TxtID == _groupId.Split('-')[2]).ToList();
            else
                discussionFoundList = DiscussionList.Where(x => x.UserList.Where(y => y.Agent.ID == SelectedAgentModel.Agent.ID).Count() > 0 && x.UserList.Count == 1).ToList();

            // display discussion messages
            if (discussionFoundList.Count > 0)
            {
                DiscussionModel = discussionFoundList[0];
                var messageListToUpdate = (await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = discussionFoundList[0].Discussion.ID, Status = 1 }, QOBDCommon.Enum.ESearchOption.AND)).Where(x => x.UserId != AuthenticatedUser.ID).Select(x => new Message
                {
                    ID = x.ID,
                    Content = x.Content,
                    Date = x.Date,
                    DiscussionId = x.DiscussionId,
                    Status = 0,
                    UserId = x.UserId
                }).ToList();

                var updatedMessageList = await BL.BlChatRoom.UpdateMessageAsync(messageListToUpdate);

                DiscussionModel.addUser(new AgentModel { Agent = AuthenticatedUser });
                var messageList = (await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = DiscussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND)).OrderByDescending(x => x.ID).Take(5).ToList();
                foreach (var message in messageList.OrderBy(x => x.ID).ToList())
                    displayMessage(message, DiscussionModel.UserList.Where(x => x.Agent.ID == message.UserId).Select(x => x.Agent).FirstOrDefault());
            }
            Dialog.IsChatDialogOpen = false;
        }

        public async Task<List<DiscussionModel>> retrieveUserDiscussions(Agent user)
        {
            object _lock = new object();
            List<DiscussionModel> output = new List<DiscussionModel>();
            List<User_discussion> allUser_discussionOfAuthencatedUserList = await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { UserId = user.ID }, QOBDCommon.Enum.ESearchOption.AND);
                       
            // display the number of unread messages
            _nbNewMessage = allUser_discussionOfAuthencatedUserList.Where(x => x.Status == 1).Count();
            onPropertyChange("TxtNbNewMessage");

            foreach (User_discussion user_discussionOfAuthenticatedUser in allUser_discussionOfAuthencatedUserList)
            {
                if (output.Where(x => x.Discussion.ID == user_discussionOfAuthenticatedUser.DiscussionId).Count() == 0)
                {    
                    // Get All users appearing in the same discusion as the authenticated user 
                    List<User_discussion> allUser_discussionOfOtherUserList = (await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { DiscussionId = user_discussionOfAuthenticatedUser.DiscussionId }, QOBDCommon.Enum.ESearchOption.AND)).Where(x => x.UserId != user.ID).ToList();

                    List<Discussion> discussionList = await BL.BlChatRoom.GetDiscussionDataByIdAsync(user_discussionOfAuthenticatedUser.DiscussionId);
                    if (discussionList.Count > 0)
                    {
                        DiscussionModel discussion = new DiscussionModel();
                        discussion.Discussion = discussionList[0];

                        // Save all discussions and their users
                        foreach (User_discussion user_discussionOfOthers in allUser_discussionOfOtherUserList)
                        {
                            if (discussion.UserList.Where(x => x.Agent.ID == user_discussionOfOthers.UserId).Count() == 0)
                            {
                                List<Agent> userFoundList = BL.BlAgent.searchAgent(new Agent { ID = user_discussionOfOthers.UserId }, QOBDCommon.Enum.ESearchOption.AND);
                                if (discussionList.Count > 0 && userFoundList.Count > 0)
                                    discussion.addUser(new AgentModel { Agent = userFoundList[0] });
                            }
                        }
                        discussion.TxtGroupName = generateDiscussionGroupName(discussion.Discussion.ID, discussion.UserList);
                        lock (_lock)
                            output.Add(discussion);
                    }
                }
            }
            DiscussionList = output;
            return output;
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

                        if (discussionId == DiscussionModel.Discussion.ID)
                        {
                            // if a new discussion group detected then reload discussion
                            if (composer[3].Split('|').Where(x => !string.IsNullOrEmpty(x)).Count() > DiscussionModel.UserList.Count)
                            {
                                IsGroupDiscussion = true;
                                _groupId = generateDiscussionGroupName(DiscussionModel.Discussion.ID, DiscussionModel.UserList);
                                executeNavig("chatroom");
                            }
                            else
                            {
                                // current discussion messages displaying
                                var userFoundList = BL.BlAgent.searchAgent(new Agent { ID = userId }, QOBDCommon.Enum.ESearchOption.AND);
                                if (messageFoundList.Count > 0 && userFoundList.Count > 0)
                                    displayMessage(messageFoundList[0], userFoundList[0]);
                            }
                        }

                        // notification of a new incoming message
                        else
                        {
                            TxtNbNewMessage = (_nbNewMessage + 1).ToString();
                            messageFoundList[0].Status = 1;
                            var updatedMessageList = await BL.BlChatRoom.UpdateMessageAsync(new List<Message> { messageFoundList[0] });
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                    }

                    // update all user online status
                    else
                        onPropertyChange("updateStatus");

                }
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }
        }

        public void displayMessage(Message message, Agent user)
        {
            if (user != null)
            {
                // update the discussion messages status from unread (status = 1) to read (status = 0)
                markMessageAsRead(DiscussionModel);

                // reset the unread message notification
                TxtNbNewMessage = 0.ToString();

                if (AuthenticatedUser.ID == message.UserId)
                {
                    message.Content = message.Date + Environment.NewLine + Utility.decodeBase64ToString(message.Content);
                    msg("reply", message.Content);
                }
                else
                {
                    message.Content = user.UserName + " Says:" + Environment.NewLine + message.Date + Environment.NewLine + Utility.decodeBase64ToString(message.Content);
                    msg("recipient", message.Content);
                }
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

        public string generateDiscussionGroupName(int discussionId, List<AgentModel> userList)
        {
            string ouput = "";
            string userGroup = "";
            string userIds = "";
            foreach (AgentModel userModel in userList)
            {
                userGroup += userModel.TxtLogin + ";";
                userIds += userModel.TxtID + "|";
            }
            ouput += userGroup + "-" + userIds + "-" + discussionId;

            return ouput;
        }

        /// <summary>
        /// Mak the discussion messages as unread for the other member of the discussion
        /// </summary>
        /// <param name="discussionModel">The current discussion</param>
        public async void markMessageAsUnRead(DiscussionModel discussionModel)
        {
            List<User_discussion> allUser_discussionOfOtherUserList = (await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND)).Where(x => x.UserId != AuthenticatedUser.ID).ToList();
            allUser_discussionOfOtherUserList = allUser_discussionOfOtherUserList.Select(x => new User_discussion { ID = x.ID, DiscussionId = x.DiscussionId, UserId = x.UserId, Status = 1 }).ToList();
            await BL.BlChatRoom.UpdateUser_discussionAsync(allUser_discussionOfOtherUserList);

            var messageFoundList = await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND);
            if(messageFoundList.Count() > 0)
            {
                Message lastMessageFound = messageFoundList.Select(x => new Message { ID = x.ID, Content = x.Content, Date = x.Date, DiscussionId = x.DiscussionId, UserId = x.UserId, Status = 1 }).OrderByDescending(x => x.ID).FirstOrDefault();
                await BL.BlChatRoom.UpdateMessageAsync(new List<Message> { lastMessageFound });
            }            
        }

        /// <summary>
        /// Mark the discussion messages as read for the authenticated member
        /// </summary>
        /// <param name="discussionModel">The current discussion</param>
        public async void markMessageAsRead(DiscussionModel discussionModel)
        {
            // set the discussion unread
            // used for to notify the authenticated user of an unread message
            List<User_discussion> allUser_discussionOfAuthencatedUserList = await BL.BlChatRoom.searchUser_discussionAsync(new User_discussion { UserId = AuthenticatedUser.ID, DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND);
            allUser_discussionOfAuthencatedUserList = allUser_discussionOfAuthencatedUserList.Select(x => new User_discussion { ID = x.ID, DiscussionId = x.DiscussionId, UserId = x.UserId, Status = 0 }).ToList();
            await BL.BlChatRoom.UpdateUser_discussionAsync(allUser_discussionOfAuthencatedUserList);

            // set unread the last message of the discussion
            // used to highlight the unread message
            var messageFoundList = await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND);
            messageFoundList = messageFoundList.Select(x => new Message { ID = x.ID, Content = x.Content, Date = x.Date, DiscussionId = x.DiscussionId, UserId = x.UserId, Status = 1 }).OrderByDescending(x => x.ID).ToList();
            await BL.BlChatRoom.UpdateMessageAsync(messageFoundList);            
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

        private async void sendMessage(object obj)
        {

            if (!string.IsNullOrEmpty(InputMessage) && SelectedAgentModel.Agent.ID != 0)
            {
                if (DiscussionModel.Discussion.ID == 0)
                {
                    Dialog.showSearch("Creating discussion with " + SelectedAgentModel.TxtLogin + "...", isChatDialogBox: true);
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
                try
                {
                    var savedMdessage = await BL.BlChatRoom.InsertMessageAsync(new List<Message> { new Message { DiscussionId = DiscussionModel.Discussion.ID, Content = InputMessage, Date = DateTime.Now, UserId = AuthenticatedUser.ID } });
                    markMessageAsUnRead(DiscussionModel);
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(DiscussionModel.TxtID + "/" + AuthenticatedUser.ID + "/" + savedMdessage[0].ID + "/" + generateDiscussionGroupName(DiscussionModel.Discussion.ID, DiscussionModel.UserList).Split('-')[1] + "/" + InputMessage + "$"); //DiscussionModel.TxtGroupName.Split(';')[1] 
                    _serverStream.Write(outStream, 0, outStream.Length);
                    _serverStream.Flush();
                    InputMessage = "";
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
            IsGroupDiscussion = false;
            _groupId = "";
            DiscussionModel = new DiscussionModel();
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
            IsGroupDiscussion = true;
            DiscussionModel.addUser(obj);
            _groupId = generateDiscussionGroupName(DiscussionModel.Discussion.ID, DiscussionModel.UserList);
            DiscussionModel.TxtGroupName = _groupId;
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
                SelectedAgentModel = new AgentModel { TxtID = obj.Split('-')[1].Split('|')[0] };
                IsGroupDiscussion = true;
                _groupId = obj;
                executeNavig("chatroom");
            }
        }

        private bool canGetDiscussionGroup(string arg)
        {
            return true;
        }




    }
}
