
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IBillManager: REMOTE.IBillManager
    {
        List<Bill> GetBillData(int nbLine);

        List<Bill> GetBillDataByOrderList(List<Order> orderList);

        List<Bill> searchBill(Bill Bill, ESearchOption filterOperator);

        List<Bill> GetBillDataById(int id);
    }
}
