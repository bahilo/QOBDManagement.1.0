using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Func<double, double> _rogressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.INotificationManager _gateWayNotification;
        private QOBDWebServicePortTypeClient _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALNotification(QOBDWebServicePortTypeClient servicePort)
        {
            _servicePortType = servicePort;
            _gateWayNotification = new GateWayNotification(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }


        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayNotification.setServiceCredential(_servicePortType);
                retrieveGateWayNotificationData();
            }
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (QOBDWebServicePortTypeClient)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.Login;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayNotification.setServiceCredential(_servicePortType);
        }

        public void retrieveGateWayNotificationData()
        {
            try
            {
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                var notificationList = new NotifyTaskCompletion<List<Notification>>(_gateWayNotification.GetNotificationDataAsync(_loadSize)).Task.Result;
                if (notificationList.Count > 0)
                    LoadNotification(notificationList);

                //Log.debug("-- Notifications loaded --");
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }
            finally
            {
                lock (_lock)
                {
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    _isLodingDataFromWebServiceToLocal = false;
                }
            }
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }



        public async Task<List<Notification>> InsertNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            List<Notification> gateWayResultList = new List<Notification>();
            using (notificationsTableAdapter _notificationsTableAdapter = new notificationsTableAdapter())
            {
                gateWayResultList = await _gateWayNotification.InsertNotificationAsync(listNotification);

                result = LoadNotification(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Notification>> DeleteNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            List<Notification> gateWayResultList = new List<Notification>();
            using (notificationsTableAdapter _notificationsTableAdapter = new notificationsTableAdapter())
            {
                gateWayResultList = await _gateWayNotification.DeleteNotificationAsync(listNotification);
                if (gateWayResultList.Count == 0)
                    foreach (Notification notification in gateWayResultList)
                    {
                        int returnResult = _notificationsTableAdapter.Delete1(notification.ID);
                        if (returnResult == 0)
                            result.Add(notification);
                    }
            }
            return result;
        }

        public async Task<List<Notification>> UpdateNotificationAsync(List<Notification> notificationList)
        {
            List<Notification> result = new List<Notification>();
            List<Notification> gateWayResultList = new List<Notification>();
            QOBDSet dataSet = new QOBDSet();
            using (notificationsTableAdapter _notificationsTableAdapter = new notificationsTableAdapter())
            {
                gateWayResultList = await _gateWayNotification.UpdateNotificationAsync(notificationList);

                foreach (var notification in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _notificationsTableAdapter.FillById(dataSetLocal.notifications, notification.ID);
                    dataSet.notifications.Merge(dataSetLocal.notifications);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _notificationsTableAdapter.Update(gateWayResultList.NotificationTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Notification> LoadNotification(List<Notification> notificationsList)
        {
            List<Notification> result = new List<Notification>();
            using (notificationsTableAdapter _notificationsTableAdapter = new notificationsTableAdapter())
            {
                foreach (var notification in notificationsList)
                {
                    int returnResult = _notificationsTableAdapter
                                            .load_data_notification(
                                                notification.Reminder1,
                                                notification.Reminder2,
                                                notification.BillId,
                                                notification.Date,
                                                notification.ID);
                    if (returnResult > 0)
                        result.Add(notification);
                }
            }
            return result;
        }

        public List<Notification> GetNotificationData(int nbLine)
        {
            List<Notification> result = new List<Notification>();
            using (notificationsTableAdapter _notificationsTableAdapter = new notificationsTableAdapter())
                result = _notificationsTableAdapter.GetData().DataTableTypeToNotification();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Notification>> GetNotificationDataAsync(int nbLine)
        {
            return await _gateWayNotification.GetNotificationDataAsync(nbLine);
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            using (notificationsTableAdapter _notificationsTableAdapter = new notificationsTableAdapter())
                return _notificationsTableAdapter.get_notification_by_id(id).DataTableTypeToNotification();
        }

        public List<Notification> SearchNotification(Notification notification, ESearchOption filterOperator)
        {
            return notification.NotificationTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Notification>> SearchNotificationAsync(Notification notification, ESearchOption filterOperator)
        {
            return await _gateWayNotification.SearchNotificationAsync(notification, filterOperator);
        }

        public void Dispose()
        {
            _gateWayNotification.Dispose();
        }
    } /* end class BlNotification */
}