
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IDeliveryManager: REMOTE.IDeliveryManager
    {
        List<Delivery> GetDeliveryData(int nbLine);

        List<Delivery> GetDeliveryDataByOrderList(List<Order> orderList);

        List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator);
        
        List<Delivery> GetDeliveryDataById(int id);
    }
}
