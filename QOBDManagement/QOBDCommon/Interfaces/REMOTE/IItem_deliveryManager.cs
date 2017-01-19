using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IItem_deliveryManager
    {
        Task<List<Item_delivery>> InsertItem_deliveryAsync(List<Item_delivery> listItem_delivery);

        Task<List<Item_delivery>> UpdateItem_deliveryAsync(List<Item_delivery> listItem_delivery);

        Task<List<Item_delivery>> DeleteItem_deliveryAsync(List<Item_delivery> listItem_delivery);

        Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryListAsync(List<Delivery> deliveryList);

        Task<List<Item_delivery>> GetItem_deliveryDataAsync(int nbLine);

        Task<List<Item_delivery>> searchItem_deliveryAsync(Item_delivery Item_delivery, ESearchOption filterOperator);
    }
}
