using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IOrder_itemManager: REMOTE.IOrder_itemManager
    {
        List<Order_item> GetOrder_itemByOrderList(List<Order> orderList);

        List<Order_item> GetOrder_itemData(int nbLine);

        List<Order_item> searchOrder_item(Order_item order_item, ESearchOption filterOperator);

        List<Order_item> GetOrder_itemDataById(int id);
    }
}
