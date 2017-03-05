using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IChatRoomManager: IMessageManager, IDiscussionManager, IUser_discussionManager
    {
        void setServiceCredential(object channel);
    }
}
