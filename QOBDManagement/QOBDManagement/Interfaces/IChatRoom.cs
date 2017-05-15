using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IChatRoom
    {      
        void showRecipientReply(MessageModel messageModel, bool isNewDiscussion = false);
        void showMyReply(MessageModel messageModel, bool isNewDiscussion = false);
        void showInfo(MessageModel messageModel);
    }
}
