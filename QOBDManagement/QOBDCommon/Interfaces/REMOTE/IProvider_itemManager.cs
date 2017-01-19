using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IProvider_itemManager
    {
        Task<List<Provider_item>> InsertProvider_itemAsync(List<Provider_item> listProvider_item);

        Task<List<Provider_item>> UpdateProvider_itemAsync(List<Provider_item> listProvider_item);

        Task<List<Provider_item>> DeleteProvider_itemAsync(List<Provider_item> listProvider_item);

        Task<List<Provider_item>> GetProvider_itemDataByItemListAsync(List<Item> itemList);

        Task<List<Provider_item>> GetProvider_itemDataAsync(int nbLine);

        Task<List<Provider_item>> searchProvider_itemAsync(Provider_item Provider_item, ESearchOption filterOperator);
    }
}
