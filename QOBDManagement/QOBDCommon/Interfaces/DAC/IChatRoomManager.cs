using QOBDCommon.Entities;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IChatRoomManager: REMOTE.IChatRoomManager
    {
        void initializeCredential(Agent user);
    }
}