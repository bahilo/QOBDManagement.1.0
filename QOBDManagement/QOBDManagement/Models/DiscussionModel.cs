using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class DiscussionModel : BindBase
    {
        private Discussion _discussion;
        private List<AgentModel> _userList;
        private bool _isGroupDiscussion;
        private string _groupName;
        private List<MessageModel> _messageList;

        public DiscussionModel()
        {
            _discussion = new Discussion();
            _userList = new List<AgentModel>();
            _messageList = new List<MessageModel>();
        }


        public Discussion Discussion
        {
            get { return _discussion; }
            set { setProperty(ref _discussion, value); }
        }


        public bool IsGroupDiscussion
        {
            get { return _isGroupDiscussion; }
            set { setProperty(ref _isGroupDiscussion, value); }
        }

        public List<AgentModel> UserList
        {
            get { return _userList; }
            set { setProperty(ref _userList, value); }
        }

        public List<MessageModel> MessageList
        {
            get { return _messageList; }
            set { setProperty(ref _messageList, value); }
        }

        public string TxtGroupName
        {
            get { return _groupName; }
            set { setProperty(ref _groupName, value); }
        }

        public string TxtID
        {
            get { return "#"+_discussion.ID.ToString(); }
            set { _discussion.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtDate
        {
            get { return _discussion.Date.ToString(); }
            set { _discussion.Date = Utility.convertToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public bool addUser(AgentModel userModel)
        {
            if (userModel != null)
            {
                if (UserList.Where(x => x.Agent.ID == userModel.Agent.ID).Count() == 0)
                    UserList.Add(userModel);
                if (UserList.Count > 2)
                    IsGroupDiscussion = true;
                onPropertyChange("UserList");
                TxtGroupName = generateDiscussionGroupName(Discussion.ID, UserList);
                return true;
            }
            return false;
        }

        public void addUser(List<AgentModel> userModelList)
        {
            foreach (AgentModel userModel in userModelList)
                addUser(userModel);
        }

        public void addUser(List<Agent> userList)
        {
            foreach (Agent user in userList)
                addUser(new AgentModel { Agent = user });
        }

        public bool addMessage(MessageModel messageModel)
        {
            if (messageModel != null && MessageList.Where(x=>x.Message.ID == messageModel.Message.ID).Count() == 0)
            {
                if (MessageList.Where(x => x.Message.ID == messageModel.Message.ID).Count() == 0)
                    MessageList.Add(messageModel);
                onPropertyChange("MessageList");
                return true;             
            }
            return false;
        }

        public void addMessage(List<MessageModel> messageModelList)
        {
            foreach (MessageModel messageModel in messageModelList)
                addMessage(messageModel);
        }

        public void addMessage(List<Message> messageList)
        {
            foreach (Message message in messageList)
                addMessage(new MessageModel { Message = message });
        }

        private string generateDiscussionGroupName(int discussionId, List<AgentModel> userList)
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

    }


}
