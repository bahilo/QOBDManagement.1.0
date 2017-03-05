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
    public interface IUser_discussionManager: INotifyPropertyChanged, IDisposable
    {
        Task<List<User_discussion>> InsertUser_discussionAsync(List<User_discussion> user_discussionList);

        Task<List<User_discussion>> UpdateUser_discussionAsync(List<User_discussion> user_discussionList);

        Task<List<User_discussion>> DeleteUser_discussionAsync(List<User_discussion> user_discussionList);

        Task<List<User_discussion>> GetUser_discussionDataAsync(int nbLine);

        Task<List<User_discussion>> GetUser_discussionDataByIdAsync(int id);

        Task<List<User_discussion>> searchUser_discussionAsync(User_discussion user_discussion, ESearchOption filterOperator);
    }
}
