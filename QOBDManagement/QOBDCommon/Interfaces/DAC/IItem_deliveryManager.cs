using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IItem_deliveryManager: REMOTE.IItem_deliveryManager
    {
        List<Item_delivery> GetItem_deliveryData(int nbLine);

        List<Item_delivery> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList);

        List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator);

        List<Item_delivery> GetItem_deliveryDataById(int id);
    }
}
