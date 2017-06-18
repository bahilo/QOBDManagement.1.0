using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface ICurrencyManager
    {
        Task<List<Currency>> InsertCurrencyAsync(List<Currency> listCurrency);

        Task<List<Currency>> UpdateCurrencyAsync(List<Currency> listCurrency);

        Task<List<Currency>> DeleteCurrencyAsync(List<Currency> listCurrency);

        Task<List<Currency>> GetCurrencyDataByProvider_itemListAsync(List<Provider_item> provider_itemList);

        Task<List<Currency>> GetCurrencyDataAsync(int nbLine);

        Task<List<Currency>> searchCurrencyAsync(Currency Currency, ESearchOption filterOperator);
    }
}
