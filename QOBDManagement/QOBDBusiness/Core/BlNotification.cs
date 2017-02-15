using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        // Attributes
        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC { get; set; }

        public BlNotification(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALNotification.initializeCredential(user);
        }


        public void setServiceCredential(object channel)
        {
            DAC.DALNotification.setServiceCredential(channel);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALNotification.progressBarManagement(progressBarFunc);
        }

        public async Task<List<Notification>> InsertNotificationAsync(List<Notification> notificationList)
        {
            if (notificationList == null || notificationList.Count == 0)
                return new List<Notification>();

            List<Notification> result = new List<Notification>();
            try
            {
                result = await DAC.DALNotification.InsertNotificationAsync(notificationList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Notification>> UpdateNotificationAsync(List<Notification> notificationList)
        {
            if (notificationList == null || notificationList.Count == 0)
                return new List<Notification>();

            if (notificationList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating notifications(count = " + notificationList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Notification> result = new List<Notification>();
            try
            {
                result = await DAC.DALNotification.UpdateNotificationAsync(notificationList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Notification>> DeleteNotificationAsync(List<Notification> notificationList)
        {
            if (notificationList == null || notificationList.Count == 0)
                return new List<Notification>();

            if (notificationList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting notifications(count = " + notificationList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Notification> result = new List<Notification>();
            try
            {
                result = await DAC.DALNotification.DeleteNotificationAsync(notificationList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Notification> GetNotificationData(int nbLine)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = DAC.DALNotification.GetNotificationData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Notification>> GetNotificationDataAsync(int nbLine)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = await DAC.DALNotification.GetNotificationDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = DAC.DALNotification.GetNotificationDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALNotification.Dispose();
        }

        public List<Notification> SearchNotification(Notification notification, ESearchOption filterOperator)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = DAC.DALNotification.SearchNotification(notification, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Notification>> SearchNotificationAsync(Notification notification, ESearchOption filterOperator)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = await DAC.DALNotification.SearchNotificationAsync(notification, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }


        // Operations


    } /* end class BlNotification */
}