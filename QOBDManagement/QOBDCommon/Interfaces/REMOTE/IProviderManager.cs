using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IProviderManager
    {
        Task<List<Provider>> InsertProviderAsync(List<Provider> listProvider);

        Task<List<Provider>> UpdateProviderAsync(List<Provider> listProvider);

        Task<List<Provider>> DeleteProviderAsync(List<Provider> listProvider);

        Task<List<Provider>> GetProviderDataAsync(int nbLine);

        Task<List<Provider>> GetProviderDataByProvider_itemListAsync(List<Provider_item> provider_itemList);

        Task<List<Provider>> searchProviderAsync(Provider Provider, ESearchOption filterOperator);
        
    }
}
