using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IBillManager
    {
        Task<List<Bill>> InsertBillAsync(List<Bill> listBill);

        Task<List<Bill>> UpdateBillAsync(List<Bill> listBill);

        Task<List<Bill>> DeleteBillAsync(List<Bill> listBill);

        Task<List<Bill>> GetBillDataAsync(int nbLine);

        Task<List<Bill>> GetBillDataByOrderListAsync(List<Order> orderList);

        Task<List<Bill>> GetUnpaidBillDataByAgentAsync(int agentId);

        Task<Bill> GetLastBillAsync();

        Task<List<Bill>> searchBillAsync(Bill Bill, ESearchOption filterOperator);
    }
}
