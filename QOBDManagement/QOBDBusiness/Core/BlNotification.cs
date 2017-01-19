using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDBusiness.Core
{
    public class BlNotification : INotificationManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC;

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

        public void initializeCredential(Agent user)
        {
           
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


        // Operations


    } /* end class BlNotification */
}