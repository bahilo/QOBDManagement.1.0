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
        void initializeCredential(Agent user);

        void cacheWebServiceData();

        Task UpdateOrderDependenciesAsync(List<Order> orderList);

        void progressBarManagement(Func<double, double> progressBarFunc);

        List<Order> GetOrderData(int nbLine);

        List<Order> GetOrderDataById(int id);

        List<Order> searchOrder(Order order, ESearchOption filterOperator);

    } /* end interface ICommandManager */
}