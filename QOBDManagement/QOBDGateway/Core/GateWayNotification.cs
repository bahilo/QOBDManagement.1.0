using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.REMOTE;
using QOBDGateway.Helper.ChannelHelper;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWayNotification : INotificationManager, INotifyPropertyChanged
    {
        private QOBDWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayNotification()
        {
            _channel = new QOBDWebServicePortTypeClient("QOBDWebServicePort");// (binding, endPoint);
        }

        public void initializeCredential(Agent user)
        {
            Credential = user;
        }

        public Agent Credential
        {
            set
            {
                setServiceCredential(value.Login, value.HashedPassword);
                onPropertyChange("Credential");
            }
        }


        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QOBDWebServicePortTypeClient("QOBDWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public QOBDWebServicePortTypeClient NotificationGatWayChannel
        {
            get
            {
                return _channel;
            }
        }

        public async Task<List<Notification>> DeleteNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.delete_data_notificationAsync(listNotification.NotificationTypeToArray())).ArrayTypeToNotification();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Notification>> GetNotificationDataAsync(int nbLine)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.get_data_notificationAsync(nbLine.ToString())).ArrayTypeToNotification().OrderBy(x => x.ID).ToList();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Notification>> GetNotificationDataByIdAsync(int id)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.get_data_notification_by_idAsync(id.ToString())).ArrayTypeToNotification();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Notification>> GetNotificationDataByOrderListAsync(List<Order> commandList)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.get_data_notification_by_command_listAsync(commandList.OrderTypeToArray())).ArrayTypeToNotification();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Notification>> InsertNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.insert_data_notificationAsync(listNotification.NotificationTypeToArray())).ArrayTypeToNotification();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Notification>> UpdateNotificationAsync(List<Notification> listNotification)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.update_data_notificationAsync(listNotification.NotificationTypeToArray())).ArrayTypeToNotification();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Notification>> SearchNotificationAsync(Notification notification, ESearchOption filterOperator)
        {
            List<Notification> result = new List<Notification>();
            try
            {
                result = (await _channel.get_filter_notificationAsync(notification.NotificationTypeToFilterArray(filterOperator))).ArrayTypeToNotification();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public void Dispose()
        {
            _channel.Close();
        }
    } /* end class BlNotification */
}