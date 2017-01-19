using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IProvider_itemManager: REMOTE.IProvider_itemManager
    {
        List<Provider_item> GetProvider_itemData(int nbLine);

        List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator);

        List<Provider_item> GetProvider_itemDataById(int id);

        List<Provider_item> GetProvider_itemDataByItemList(List<Item> itemList);
    }
}
