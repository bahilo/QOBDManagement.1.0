using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface ITaxManager
    {
        Task<List<Tax>> InsertTaxAsync(List<Tax> listTax);

        Task<List<Tax>> UpdateTaxAsync(List<Tax> listTax);

        Task<List<Tax>> DeleteTaxAsync(List<Tax> listTax);

        Task<List<Tax>> GetTaxDataAsync(int nbLine);

        Task<List<Tax>> searchTaxAsync(Tax Tax, ESearchOption filterOperator);
    }
}
