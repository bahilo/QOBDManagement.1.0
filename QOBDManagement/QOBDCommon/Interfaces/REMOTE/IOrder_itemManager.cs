using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IOrder_itemManager
    {
        Task<List<Order_item>> InsertOrder_itemAsync(List<Order_item> listOrder_item);

        Task<List<Order_item>> UpdateOrder_itemAsync(List<Order_item> listOrder_item);

        Task<List<Order_item>> DeleteOrder_itemAsync(List<Order_item> listOrder_item);

        Task<List<Order_item>> GetOrder_itemDataAsync(int nbLine);

        Task<List<Order_item>> GetOrder_itemByOrderListAsync(List<Order> orderList);

        Task<List<Order_item>> searchOrder_itemAsync(Order_item Order_item, ESearchOption filterOperator);
    }
}
