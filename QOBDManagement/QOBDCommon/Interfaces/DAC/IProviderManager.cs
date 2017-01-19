using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IProviderManager: REMOTE.IProviderManager
    {
        List<Provider> GetProviderData(int nbLine);

        List<Provider> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList);

        List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator);

        List<Provider> GetProviderDataById(int id);
    }
}
