using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace QOBDManagement.ViewModel
{
    public class MessageViewModel : BindBase
    {
        private Func<object, object> navigation;
        private Dictionary<AgentModel, MessageModel> _messageIndividualHistoryList;
        private Dictionary<AgentModel, MessageModel> _messageGroupHistoryList;
        private IDiscussionViewModel _discussionViewModel;

        public MessageViewModel()
        {
            _messageIndividualHistoryList = new Dictionary<AgentModel, MessageModel>();
            _messageGroupHistoryList = new Dictionary<AgentModel, MessageModel>();
        }

        public MessageViewModel(Func<object, object> navigation, IDiscussionViewModel discussionViewModel) : this()
        {
            this.navigation = navigation;
            _discussionViewModel = discussionViewModel;
        }

        public Agent AuthenticatedUser
        {
            get { return BL.BlSecurity.GetAuthenticatedUser(); }
        }

        public QOBDBusiness.BusinessLogic BL
        {
            get { return _startup.Bl; }
        }

        public Dictionary<AgentModel, MessageModel> MessageIndividualHistoryList
        {
            get { return _messageIndividualHistoryList; }
            set { setProperty(ref _messageIndividualHistoryList, value); }
        }

        public Dictionary<AgentModel, MessageModel> MessageGroupHistoryList
        {
            get { return _messageGroupHistoryList; }
            set { setProperty(ref _messageGroupHistoryList, value); }
        }

        public async Task load()
        {
            Dialog.showSearchMessage("Loding...", isChatDialogBox: true);

            MessageIndividualHistoryList.Clear();
            MessageGroupHistoryList.Clear();

            // searching all common discussion
            var discussionList = _discussionViewModel.DiscussionList;
            if (discussionList == null || discussionList.Count == 0)
                discussionList = await _discussionViewModel.retrieveUserDiscussions(BL.BlSecurity.GetAuthenticatedUser());

            foreach (var discussionModel in discussionList)
            {
                if (MessageIndividualHistoryList.Values.Where(x => x.Message.DiscussionId == discussionModel.Discussion.ID).Count() == 0
                    && MessageGroupHistoryList.Values.Where(x => x.Message.DiscussionId == discussionModel.Discussion.ID).Count() == 0)
                {
                    MessageModel lastMessage = new MessageModel();
                    if (discussionModel.MessageList.Count > 0)
                    {
                        lastMessage = discussionModel.MessageList.Where(x => x.IsNewMessage).OrderByDescending(x => x.Message.ID).Select(x => new MessageModel { Message = new Message { Content = x.TxtContent }, IsNewMessage = x.IsNewMessage }).FirstOrDefault();
                        if (lastMessage == null)
                            lastMessage = discussionModel.MessageList.OrderByDescending(x => x.Message.ID).Select(x => new MessageModel { Message = new Message { Content = x.TxtContent }, IsNewMessage = x.IsNewMessage }).First();

                        // limit the amount of message characters to display in the history
                        if (lastMessage.TxtContent.Length > _discussionViewModel.MaxMessageLength)
                            lastMessage.TxtContent = lastMessage.TxtContent.Substring(0, _discussionViewModel.MaxMessageLength) + "...";

                        if (discussionModel.UserList.Count() > 0)
                        {
                            lastMessage.Message.UserId = discussionModel.UserList[0].Agent.ID;
                            lastMessage.TxtGroupName = discussionModel.TxtGroupName;
                            lastMessage.Message.Date = discussionModel.MessageList.OrderByDescending(x => x.Message.Date).Select(x => x.Message.Date).First();
                        }

                        // saving the messages for group and individual discussion
                        var usersFound = discussionModel.UserList.Where(x => x.Agent.ID == lastMessage.Message.UserId).ToList();// await BL.BlAgent.searchAgentAsync( new Agent { ID = lastMessage.Message.UserId }, QOBDCommon.Enum.ESearchOption.AND);
                        if (usersFound.Count > 0)
                        {
                            if (discussionModel.UserList.Count == 1)
                                addMessageIndividualHistoryList(new Dictionary<AgentModel, MessageModel> { { usersFound.First(), lastMessage } });
                            else
                                addMessageGroupHistoryList(new Dictionary<AgentModel, MessageModel> { { usersFound.First(), lastMessage } });
                        }
                    }
                }
            }
            Dialog.IsChatDialogOpen = false;
        }

        private void addMessageGroupHistoryList(Dictionary<AgentModel, MessageModel> dict)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageGroupHistoryList = Utility.concat(MessageGroupHistoryList, dict);
                });
            else
                MessageGroupHistoryList = Utility.concat(MessageGroupHistoryList, dict);
        }

        private void addMessageIndividualHistoryList(Dictionary<AgentModel, MessageModel> dict)
        {
            if(Application.Current != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageIndividualHistoryList = Utility.concat(MessageIndividualHistoryList, dict);
                });
            else
                MessageIndividualHistoryList = Utility.concat(MessageIndividualHistoryList, dict);
        }


    }
}
