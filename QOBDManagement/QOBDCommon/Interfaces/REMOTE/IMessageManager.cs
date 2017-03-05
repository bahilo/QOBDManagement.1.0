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
    public interface IMessageManager: INotifyPropertyChanged, IDisposable
    {
        Task<List<Message>> InsertMessageAsync(List<Message> discussionList);

        Task<List<Message>> UpdateMessageAsync(List<Message> discussionList);

        Task<List<Message>> DeleteMessageAsync(List<Message> discussionList);

        Task<List<Message>> GetMessageDataAsync(int nbLine);

        Task<List<Message>> GetMessageDataByIdAsync(int id);

        Task<List<Message>> searchMessageAsync(Message message, ESearchOption filterOperator);
    }
}
