using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.REMOTE;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                _channel.ClientCredentials.UserName.UserName = value.Login;
                _channel.ClientCredentials.UserName.Password = value.HashedPassword;
                onPropertyChange("Credential");
            }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public Task<List<Notification>> DeleteNotificationAsync(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetNotificationDataAsync(int nbLine)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetNotificationDataByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> InsertNotificationAsync(List<Notification> notificationList)
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
            throw new NotImplementedException();
        }
    } /* end class BlNotification */
}