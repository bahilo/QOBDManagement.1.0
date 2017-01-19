using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IDeliveryManager
    {
        Task<List<Delivery>> InsertDeliveryAsync(List<Delivery> listDelivery);

        Task<List<Delivery>> UpdateDeliveryAsync(List<Delivery> listDelivery);

        Task<List<Delivery>> DeleteDeliveryAsync(List<Delivery> listDelivery);

        Task<List<Delivery>> GetDeliveryDataAsync(int nbLine);

        Task<List<Delivery>> GetDeliveryDataByOrderListAsync(List<Order> orderList);

        Task<List<Delivery>> searchDeliveryAsync(Delivery Delivery, ESearchOption filterOperator);
    }
}
