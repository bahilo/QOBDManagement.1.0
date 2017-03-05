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

        public async void load()
        {
            Dialog.showSearch("Loading...", isChatDialogBox:true);       
            MessageIndividualHistoryList.Clear();
            MessageGroupHistoryList.Clear();

            // searching all common discussion
            var discussionList = await _discussionViewModel.retrieveUserDiscussions(BL.BlSecurity.GetAuthenticatedUser());

            foreach (var discussionModel in discussionList)
            {                
                if (MessageIndividualHistoryList.Values.Where(x=>x.Message.DiscussionId == discussionModel.Discussion.ID).Count() == 0
                    && MessageGroupHistoryList.Values.Where(x => x.Message.DiscussionId == discussionModel.Discussion.ID).Count() == 0)
                {
                    MessageModel lastMessage = new MessageModel();
                    var allMessages = await BL.BlChatRoom.searchMessageAsync(new Message { DiscussionId = discussionModel.Discussion.ID }, QOBDCommon.Enum.ESearchOption.AND);                    
                    var messageList = allMessages.Where(x => x.UserId != AuthenticatedUser.ID && x.Status == 1).OrderByDescending(x => x.Date).ToList();
                    if (messageList.Count > 0)
                    {
                        int nbCharToDisplay;

                        // get all unread messages
                        if(messageList.Where(x=>x.Status == 1).Count() > 0)
                        {
                            lastMessage = messageList.Where(x => x.Status == 1).Select(x => new MessageModel { Message = x }).First();                            
                        }                            
                        else
                            lastMessage = messageList.Select(x => new MessageModel { Message = x }).First();

                        lastMessage.TxtContent = Utility.decodeBase64ToString(lastMessage.TxtContent);
                        if (lastMessage.TxtContent.Length > 30)
                            nbCharToDisplay = 30;
                        else
                            nbCharToDisplay = lastMessage.TxtContent.Length;
                        lastMessage.TxtContent = lastMessage.TxtContent.Substring(0, nbCharToDisplay) + "...";
                    }
                    else if(allMessages.Count > 0)
                    {      
                        if(discussionModel.UserList.Count() > 0)
                           lastMessage.Message = new Message { Content = "...", DiscussionId = discussionModel.Discussion.ID, UserId = discussionModel.UserList[0].Agent.ID, Status = 0, Date = allMessages.OrderByDescending(x => x.Date).Select(x => x.Date).First() };    
                    }/**/

                    lastMessage.TxtGroupName = discussionModel.TxtGroupName;
                    var userList = discussionModel.UserList.Where(x=> x.Agent.ID == lastMessage.Message.UserId).ToList();// await BL.BlAgent.searchAgentAsync( new Agent { ID = lastMessage.Message.UserId }, QOBDCommon.Enum.ESearchOption.AND);
                    if (userList.Count > 0)
                    {
                        if(discussionModel.UserList.Count == 1)
                            MessageIndividualHistoryList = Utility.concat(MessageIndividualHistoryList, new Dictionary<AgentModel, MessageModel> { { userList.First(), lastMessage } });
                        else
                            MessageGroupHistoryList = Utility.concat(MessageGroupHistoryList, new Dictionary<AgentModel, MessageModel> { { userList.First(), lastMessage } });
                    }                        
                }  
            }
            Dialog.IsChatDialogOpen = false;         
        }

        



    }
}
