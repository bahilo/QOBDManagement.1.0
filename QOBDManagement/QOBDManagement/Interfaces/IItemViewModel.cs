using QOBDCommon.Entities;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IItemViewModel
    {
        void loadItems();
        List<Item_deliveryModel> item_deliveryListToModelList(List<Item_delivery> item_deliveryList);
        void Dispose();
    }
}
