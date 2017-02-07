// FILE: D:/Just IT Training/BillManagment/Classes//IItemManager.cs

// In this section you can add your own using directives
using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IItemManager : IProviderManager, IProvider_itemManager, IItem_deliveryManager, ITax_itemManager, IAuto_refManagement, INotifyPropertyChanged, IDisposable
    {
        // Operations

        void setServiceCredential(string login, string password);

        Task<List<Item>> InsertItemAsync(List<Item> itemList);

        Task<List<Item>> UpdateItemAsync(List<Item> itemList);

        Task<List<Item>> DeleteItemAsync(List<Item> itemList);

        Task<List<Item>> GetItemDataAsync(int nbLine);

        Task<List<Item>> GetItemDataByOrder_itemListAsync(List<Order_item> order_itemList);

        Task<List<Item>> searchItemAsync(Item item, ESearchOption filterOperator);

    } /* end interface IItemManager */
}