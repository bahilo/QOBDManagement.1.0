using System;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.Core;
using QOBDCommon.Entities;
using System.Threading.Tasks;
using System.Threading;

public class DataAccess : IDataAccessManager
{

    public IAgentManager DALAgent { get; set; }
    public IStatisticManager DALStatistic { get; set; }
    public IOrderManager DALOrder { get; set; }
    public IClientManager DALClient { get; set; }
    public IItemManager DALItem { get; set; }
    public IReferentialManager DALReferential { get; set; }
    public ISecurityManager DALSecurity { get; set; }
    public INotificationManager DALNotification { get; set; }
    public IChatRoomManager DALChatRoom { get; set; }
    public Func<double, double> ProgressBarFunc { get; set; }

    public DataAccess(
                        IAgentManager inDALAgent,
                        IClientManager inDALClient,
                        IItemManager inDALItem,
                        IOrderManager inDALCommande,
                        ISecurityManager inDALSecurity,
                        IStatisticManager inDALStatistic,
                        IReferentialManager inDALReferential,
                        INotificationManager inDALNotification,
                        IChatRoomManager inDALChatRoom
        )
    {
        this.DALAgent = inDALAgent;
        this.DALClient = inDALClient;
        this.DALOrder = inDALCommande;
        this.DALItem = inDALItem;
        this.DALStatistic = inDALStatistic;
        this.DALReferential = inDALReferential;
        this.DALSecurity = inDALSecurity;
        this.DALNotification = inDALNotification;
        this.DALChatRoom = inDALChatRoom;
    }

    public void SetUserCredential(Agent authenticatedUser, bool isNewAgentAuthentication = false)
    {
        if (isNewAgentAuthentication)
        {
            // Order
            DALOrder.progressBarManagement(ProgressBarFunc);
            DALOrder.initializeCredential(authenticatedUser);
            DALOrder.cacheWebServiceData();

            // Client
            DALClient.progressBarManagement(ProgressBarFunc);
            DALClient.initializeCredential(authenticatedUser);
            DALClient.cacheWebServiceData();            
        }
        else
        {
            /*/// Order
            DALOrder.progressBarManagement(ProgressBarFunc);
            DALOrder.initializeCredential(authenticatedUser);
            DALOrder.cacheWebServiceData();
            */
            // Agent
            DALAgent.progressBarManagement(ProgressBarFunc);
            DALAgent.initializeCredential(authenticatedUser);
            DALAgent.cacheWebServiceData();
            /*
            // Client 
            DALClient.progressBarManagement(ProgressBarFunc);
            DALClient.initializeCredential(authenticatedUser);
            DALClient.cacheWebServiceData();

            // Security
            DALSecurity.progressBarManagement(ProgressBarFunc);
            DALSecurity.initializeCredential(authenticatedUser);

            // Item 
            DALItem.progressBarManagement(ProgressBarFunc);
            DALItem.initializeCredential(authenticatedUser);
            DALItem.cacheWebServiceData();

            // Referential
            DALReferential.progressBarManagement(ProgressBarFunc);
            DALReferential.initializeCredential(authenticatedUser);
            DALReferential.cacheWebServiceData();

            // Notification
            DALNotification.progressBarManagement(ProgressBarFunc);
            DALNotification.initializeCredential(authenticatedUser);
            DALNotification.cacheWebServiceData();

            // Statistic
            DALStatistic.progressBarManagement(ProgressBarFunc);
            DALStatistic.initializeCredential(authenticatedUser);
            DALStatistic.cacheWebServiceData();     */      
        }

        // ChatRoom
        DALChatRoom.initializeCredential(authenticatedUser);
    }

    public void Dispose()
    {
        this.DALAgent.Dispose();
        this.DALClient.Dispose();
        this.DALOrder.Dispose();
        this.DALItem.Dispose();
        this.DALStatistic.Dispose();
        this.DALReferential.Dispose();
        this.DALSecurity.Dispose();
    }
} /* end class DataAccess */
