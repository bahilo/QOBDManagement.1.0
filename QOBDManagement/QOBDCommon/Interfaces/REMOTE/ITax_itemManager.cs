using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface ITax_itemManager
    {
        Task<List<Tax_item>> InsertTax_itemAsync(List<Tax_item> listTax_item);

        Task<List<Tax_item>> UpdateTax_itemAsync(List<Tax_item> listTax_item);

        Task<List<Tax_item>> DeleteTax_itemAsync(List<Tax_item> listTax_item);

        Task<List<Tax_item>> GetTax_itemDataByItemListAsync(List<Item> itemList);

        Task<List<Tax_item>> GetTax_itemDataAsync(int nbLine);

        Task<List<Tax_item>> searchTax_itemAsync(Tax_item Tax_item, ESearchOption filterOperator);
    }
}
