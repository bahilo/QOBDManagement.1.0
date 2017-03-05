
using QOBDCommon.Entities;
using System;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.DAC
{
    public interface IDataAccessManager: IDisposable
    {
        IAgentManager DALAgent { get; set; }        
        IStatisticManager DALStatistic { get; set; }        
        IOrderManager DALOrder { get; set; }        
        IClientManager DALClient { get; set; }        
        IItemManager DALItem { get; set; }
        IReferentialManager DALReferential { get; set; }
        ISecurityManager DALSecurity { get; set; }
        INotificationManager DALNotification { get; set; }
        IChatRoomManager DALChatRoom { get; set; }

        void SetUserCredential(Agent authenticatedUser, bool isNewAgentAuthentication = false);
    } /* end interface IDataAccessManager */
}