using QOBDCommon.Entities;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IDiscussionViewModel
    {
        string TxtNbNewMessage { get; set; }
        Task<List<DiscussionModel>> retrieveUserDiscussions(Agent user);
        string generateDiscussionGroupName(int discussionId, List<AgentModel> userList);
    }
}
