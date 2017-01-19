// FILE: D:/Just IT Training/BillManagment/Classes//INotificationManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000DE7 begin
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000DE7 end

using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.REMOTE
{
    public interface INotificationManager
    {
        // Operations

        Task<List<Notification>> InsertNotificationAsync(List<Notification> notificationList);

        Task<List<Notification>> UpdateNotificationAsync(List<Notification> notificationList);

        Task<List<Notification>> DeleteNotificationAsync(List<Notification> notificationList);

        Task<List<Notification>> GetNotificationDataAsync(int nbLine);

        Task<List<Notification>> SearchNotificationAsync(Notification notification, ESearchOption filterOperator);
    } /* end interface INotificationManager */
}