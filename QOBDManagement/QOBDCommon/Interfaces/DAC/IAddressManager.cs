using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IAddressManager: REMOTE.IAddressManager
    {
        List<Address> GetAddressData(int nbLine);

        List<Address> GetAddressDataByOrderList(List<Order> orderList);

        List<Address> GetAddressDataByClientList(List<Client> clientList);

        List<Address> searchAddress(Address Address, ESearchOption filterOperator);

        List<Address> GetAddressDataById(int id);
    }
}
