using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Structures;
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
namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IOrderManager : ITax_orderManager, IOrder_itemManager, ITaxManager, IBillManager, IDeliveryManager, IGeneratePDF, IDisposable, INotifyPropertyChanged
    {
        // Operations

        void setServiceCredential(string login, string password);

        Task<List<Order>> InsertOrderAsync(List<Order> orderList);

        Task<List<Order>> UpdateOrderAsync(List<Order> orderList);

        Task<List<Order>> DeleteOrderAsync(List<Order> orderList);

        Task<List<Order>> GetOrderDataAsync(int nbLine);

        Task<List<Order>> searchOrderAsync(Order order, ESearchOption filterOperator);

    } /* end interface ICommandManager */
}