using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface ITax_orderManager
    {
        Task<List<Tax_order>> InsertTax_orderAsync(List<Tax_order> tax_orderList);

        Task<List<Tax_order>> UpdateTax_orderAsync(List<Tax_order> tax_orderList);

        Task<List<Tax_order>> DeleteTax_orderAsync(List<Tax_order> tax_orderList);

        Task<List<Tax_order>> GetTax_orderDataAsync(int nbLine);

        Task<List<Tax_order>> GetTax_orderDataByOrderListAsync(List<Order> orderList);

        Task<List<Tax_order>> searchTax_orderAsync(Tax_order Tax_order, ESearchOption filterOperator);

    }
}
