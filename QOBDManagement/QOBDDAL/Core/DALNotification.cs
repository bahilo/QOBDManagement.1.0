using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using QOBDDAL.Classes;
using QOBDDAL.Interfaces;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;
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
        private Func<double, double> _progressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.INotificationManager _gateWayNotification;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALNotification(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gateWayNotification = new GateWayNotification(_servicePortType);
            _loadSize = Utility.intTryParse(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Utility.intTryParse(ConfigurationManager.AppSettings["progress_step"]);
        }

        public DALNotification(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public DALNotification(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }


        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayNotification.setServiceCredential(_servicePortType);
            }
        }

        public async void cacheWebServiceData()
        {
            await retrieveGateWayNotificationDataAsync();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayNotification.setServiceCredential(_servicePortType);
        }

        public async Task retrieveGateWayNotificationDataAsync()
        {
            try
            {
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                var notificationList = await _gateWayNotification.GetNotificationDataAsync(_loadSize);
                if (notificationList.Count > 0)
                    LoadNotification(notificationList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.NOTIFICATION);
            }
            finally
            {
                lock (_lock) IsLodingDataFromWebServiceToLocal = false;
                try
                {
                    _progressBarFunc((double)100 / _progressStep);
                }
                catch (DivideByZeroException ex) { Log.error(ex.Message, EErrorFrom.STATISTIC); }
                // Log.debug("Loaded[" + _progressBarFunc(0) + "%]!", EErrorFrom.NOTIFICATION);
            }
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }

        private void checkServiceCommunication()
        {
            if (_servicePortType.State == System.ServiceModel.CommunicationState.Closed || _servicePortType.State == System.ServiceModel.CommunicationState.Faulted)
                _serviceCommunication.resetCommunication();
        }

        #region [ Actions ]
        //=================================[ Actions ]================================================

        public async Task<List<Notification>> InsertNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            checkServiceCommunication();
            List<Notification> gateWayResultList = await _gateWayNotification.InsertNotificationAsync(listNotification);
            result = LoadNotification(gateWayResultList);
            return result;
        }

        public async Task<List<Notification>> DeleteNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            checkServiceCommunication();
            List<Notification> gateWayResultList = await _gateWayNotification.DeleteNotificationAsync(listNotification);
            if (gateWayResultList.Count == 0)
                foreach (Notification notification in gateWayResultList)
                {
                    int returnResult = _dataSet.DeleteNotification(notification.ID);
                    if (returnResult == 0)
                        result.Add(notification);
                }
            return result;
        }

        public async Task<List<Notification>> UpdateNotificationAsync(List<Notification> notificationList)
        {
            checkServiceCommunication();
            List<Notification> gateWayResultList = await _gateWayNotification.UpdateNotificationAsync(notificationList);
            List<Notification> result = LoadNotification(gateWayResultList);
            return result;
        }

        public List<Notification> LoadNotification(List<Notification> notificationsList)
        {
            List<Notification> result = new List<Notification>();
            foreach (var notification in notificationsList)
            {
                int returnResult = _dataSet.LoadNotification(notification);
                if (returnResult > 0)
                    result.Add(notification);
            }
            return result;
        }

        public List<Notification> GetNotificationData(int nbLine)
        {
            List<Notification> result = _dataSet.GetNotificationData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Notification>> GetNotificationDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayNotification.GetNotificationDataAsync(nbLine);
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            return _dataSet.GetNotificationDataById(id);
        }

        public List<Notification> searchNotification(Notification notification, ESearchOption filterOperator)
        {
            return _dataSet.searchNotification(notification, filterOperator);
        }

        public async Task<List<Notification>> searchNotificationAsync(Notification notification, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayNotification.searchNotificationAsync(notification, filterOperator);
        }

        #endregion

        public void Dispose()
        {
            if (_gateWayNotification != null)
                _gateWayNotification.Dispose();
        }
    } /* end class BlNotification */
}