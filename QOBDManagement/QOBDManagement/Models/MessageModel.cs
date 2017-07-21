using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class MessageModel : BindBase
    {
        private Message _message;
        private string _groupName;
        private string _userName;

        public MessageModel()
        {
            _message = new Message();
        }

        public Message Message
        {
            get { return _message; }
            set { setProperty(ref _message, value); }
        }

        public string TxtGroupName
        {
            get { return _groupName; }
            set { setProperty(ref _groupName, value); }
        }

        public string TxtID
        {
            get { return _message.ID.ToString(); }
            set { _message.ID = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtDiscussion
        {
            get { return _message.DiscussionId.ToString(); }
            set { _message.DiscussionId = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtUserId
        {
            get { return _message.UserId.ToString(); }
            set { _message.UserId = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtDate
        {
            get { return _message.Date.ToString(); }
            set { _message.Date = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public bool IsNewMessage
        {
            get { return _message.Status == 1 ? true : false; }
            set { _message.Status = value ? 1 : 0; onPropertyChange(); }
        }

        public string TxtContent
        {
            get { return _message.Content; }
            set { _message.Content = value; onPropertyChange(); }
        }

        /*public string TxtStatus
        {
            get { return _message.Status.ToString(); }
            set { _message.Status = Convert.ToInt32(value); onPropertyChange(); }
        }*/

        public string TxtUserName
        {
            get { return _userName; }
            set { _userName = value; onPropertyChange(); }
        }
    }
}
