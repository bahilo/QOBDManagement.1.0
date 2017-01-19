using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface ITax_itemManager: REMOTE.ITax_itemManager
    {
        List<Tax_item> GetTax_itemData(int nbLine);

        List<Tax_item> GetTax_itemDataByItemList(List<Item> itemList);

        List<Tax_item> GetTax_itemDataById(int id);

        List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator);
        
    }
}
