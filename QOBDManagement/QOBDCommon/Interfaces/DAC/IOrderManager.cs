using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.DAC
{
    public interface IOrderManager : REMOTE.IOrderManager, ITax_orderManager, IOrder_itemManager, ITaxManager, IBillManager, IDeliveryManager, IGeneratePDF , INotifyPropertyChanged, IDisposable
    {
        //Agent AuthenticatedUser { get; set; }

        void initializeCredential(Agent user);

        void UpdateOrderDependencies(List<Order> orderList, bool isActiveProgress = false);

        void progressBarManagement(Func<double, double> progressBarFunc);

        List<Order> GetOrderData(int nbLine);

        List<Order> GetOrderDataById(int id);

        List<Order> searchOrder(Order order, ESearchOption filterOperator);
    } /* end interface ICommandManager */
}