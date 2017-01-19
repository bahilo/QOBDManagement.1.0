using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface ITax_orderManager: REMOTE.ITax_orderManager
    {
        List<Tax_order> GetTax_orderData(int nbLine);

        List<Tax_order> GetTax_orderDataByOrderList(List<Order> orderList);

        List<Tax_order> GetTax_orderDataById(int id);

        List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator);

    }
}
