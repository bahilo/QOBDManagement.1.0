using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IDiscussionManager : INotifyPropertyChanged, IDisposable
    {
        Task<List<Discussion>> InsertDiscussionAsync(List<Discussion> discussionList);

        Task<List<Discussion>> UpdateDiscussionAsync(List<Discussion> discussionList);

        Task<List<Discussion>> DeleteDiscussionAsync(List<Discussion> discussionList);

        Task<List<Discussion>> GetDiscussionDataAsync(int nbLine);

        Task<List<Discussion>> GetDiscussionDataByIdAsync(int id);

        Task<List<Discussion>> GetDiscussionDataByUser_discussionListAsync(List<User_discussion> user_discussionList);

        Task<List<Discussion>> GetDiscussionDataByMessageListAsync(List<Message> messageList);

        Task<List<Discussion>> searchDiscussionAsync(Discussion discussion, ESearchOption filterOperator);
    }
}
