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
        int MaxMessageLength { get; }
        string TxtNbNewMessage { get; set; }
        List<DiscussionModel> DiscussionList { get; set; }
        DiscussionModel DiscussionModel { get; set; }
        Task<List<DiscussionModel>> retrieveUserDiscussions(Agent user);
    }
}
