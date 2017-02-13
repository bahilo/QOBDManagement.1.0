using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IItemManager : REMOTE.IItemManager, IProviderManager, IProvider_itemManager, IItem_deliveryManager, ITax_itemManager, IAuto_refManagement, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);

        void UpdateItemDependencies(List<Item> itemList, bool isActiveProgress = false);

        void progressBarManagement(Func<double, double> progressBarFunc);

        List<Item> GetItemData(int nbLine);

        List<Item> GetItemDataByOrder_itemList(List<Order_item> order_itemList);

        List<Item> searchItem(Item item, ESearchOption filterOperator);

        List<Item> GetItemDataById(int id);
    } /* end interface IItemManager */
}