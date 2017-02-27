using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.DAC
{
    public interface INotificationManager : REMOTE.INotificationManager
    {
        void initializeCredential(Agent user);

        void progressBarManagement(Func<double, double> progressBarFunc);

        List<Notification> searchNotification(Notification notification, ESearchOption filterOperator);

        List<Notification> GetNotificationData(int nbLine);

        List<Notification> GetNotificationDataById(int id);

    } /* end interface INotificationManager */
}