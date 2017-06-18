using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface ICurrencyManager : REMOTE.ICurrencyManager
    {
        List<Currency> GetCurrencyData(int nbLine);

        List<Currency> searchCurrency(Currency Currency, ESearchOption filterOperator);

        List<Currency> GetCurrencyDataById(int id);

        List<Currency> GetCurrencyDataByProvider_itemList(List<Provider_item> provider_itemList);
    }
}
