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
    public class DiscussionModel : BindBase
    {
        private Discussion _discussion;
        private List<AgentModel> _userList;
        private bool _isGroupDiscussion;
        private string _groupName;

        public DiscussionModel()
        {
            _discussion = new Discussion();
            _userList = new List<AgentModel>();
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

        public string TxtGroupName
        {
            get { return _groupName; }
            set { setProperty(ref _groupName, value); }
        }

        public string TxtID
        {
            get { return _discussion.ID.ToString(); }
            set { _discussion.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtDate
        {
            get { return _discussion.Date.ToString(); }
            set { _discussion.Date = Utility.convertToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public void addUser(AgentModel userModel)
        {
            if (UserList.Where(x => x.Agent.ID == userModel.Agent.ID).Count() == 0)
                UserList.Add(userModel);

            if (UserList.Count > 0)
                IsGroupDiscussion = true;
        }

        public void addUser(List<AgentModel> userModelList)
        {
            foreach (AgentModel userModel in userModelList)
            {
                addUser(userModel);
            }
        }

    }

    
}
