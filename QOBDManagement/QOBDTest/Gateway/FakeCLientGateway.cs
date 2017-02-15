using QOBDCommon.Interfaces.REMOTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.ComponentModel;

namespace QOBDTest.Gateway
{
    class FakeCLientGateway : IClientManager
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<List<Address>> DeleteAddressAsync(List<Address> listAddress)
        {
            return await Task.Factory.StartNew(()=> { return new List<Address>(); });
        }

        public Task<List<Client>> DeleteClientAsync(List<Client> clientList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> DeleteContactAsync(List<Contact> listContact)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> GetAddressDataAsync(int nbLine)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> GetAddressDataByClientListAsync(List<Client> clientList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> GetAddressDataByOrderListAsync(List<Order> orderList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> GetClientDataAsync(int nbLine)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> GetClientDataByBillListAsync(List<Bill> billList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> GetClientMaxCreditOverDataByAgentAsync(int agentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> GetContactDataAsync(int nbLine)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> GetContactDataByClientListAsync(List<Client> clientList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> InsertAddressAsync(List<Address> listAddress)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> InsertClientAsync(List<Client> clientList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> InsertContactAsync(List<Contact> listContact)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> searchClientAsync(Client client, ESearchOption filterOperator)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator)
        {
            throw new NotImplementedException();
        }

        public void setServiceCredential(object channel)
        {
            throw new NotImplementedException();
        }

        public void setServiceCredential(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Task<List<Address>> UpdateAddressAsync(List<Address> listAddress)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> UpdateClientAsync(List<Client> clientList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> UpdateContactAsync(List<Contact> listContact)
        {
            throw new NotImplementedException();
        }
    }
}
