using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDDAL.Core
{
    public class DALNotification : INotificationManager
    {
        public Agent AuthenticatedUser { get; set; }
        private GateWayNotification _gateWayNotification;
        private int _loadSize;
        private int _progressStep;

        public DALNotification()
        {
            _gateWayNotification = new GateWayNotification();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            _gateWayNotification.initializeCredential(user);
        }

        public Task<List<Notification>> DeleteNotificationAsync(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }

        public List<Notification> GetNotificationData(int nbLine)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetNotificationDataAsync(int nbLine)
        {
            throw new NotImplementedException();
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> InsertNotificationAsync(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }

        public List<Notification> SearchNotification(Notification notification, ESearchOption filterOperator)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> SearchNotificationAsync(Notification notification, ESearchOption filterOperator)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> UpdateNotificationAsync(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    } /* end class BlNotification */
}