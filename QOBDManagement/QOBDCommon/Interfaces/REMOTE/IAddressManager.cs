using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IAddressManager
    {
        Task<List<Address>> InsertAddressAsync(List<Address> listAddress);

        Task<List<Address>> UpdateAddressAsync(List<Address> listAddress);

        Task<List<Address>> DeleteAddressAsync(List<Address> listAddress);

        Task<List<Address>> GetAddressDataAsync(int nbLine);

        Task<List<Address>> GetAddressDataByOrderListAsync(List<Order> orderList);

        Task<List<Address>> GetAddressDataByClientListAsync(List<Client> clientList);

        Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator);
    }
}
