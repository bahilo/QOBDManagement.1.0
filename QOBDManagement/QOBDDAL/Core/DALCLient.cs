using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;
using QOBDCommon.Classes;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDGateway.QOBDServiceReference;
using QOBDDAL.Classes;
using QOBDDAL.Interfaces;
using QOBDGateway.Classes;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDDAL.Core
{
    public class DALClient : IClientManager
    {
        private Func<double, double> _rogressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IClientManager _gateWayClient;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();
        private Interfaces.IQOBDSet _dataSet;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALClient(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gateWayClient = new GateWayClient(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public DALClient(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public QOBDCommon.Interfaces.REMOTE.IClientManager GateWayClient
        {
            get { return _gateWayClient; }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                setServiceCredential(_servicePortType);
                retrieveGateWayClientData();
            }
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.Login;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayClient.setServiceCredential(_servicePortType);
        }

        private void retrieveGateWayClientData()
        {       
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                ConcurrentBag<Client> clientList = new ConcurrentBag<Client>(new NotifyTaskCompletion<List<Client>>(_gateWayClient.GetClientDataAsync(_loadSize)).Task.Result);

                if (clientList.Count > 0)
                    UpdateClientDependencies(clientList.ToList());
                //Log.debug("-- Clients loaded --");
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }
            finally
            {
                lock (_lock)
                {
                    IsLodingDataFromWebServiceToLocal = false;
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                }
            }
            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public async Task<List<Client>> InsertClientAsync(List<Client> listClient)
        {
            List<Client> gateWayResultList = await _gateWayClient.InsertClientAsync(listClient);
            List<Client> result = LoadClient(gateWayResultList);
            return result;
        }

        public async Task<List<Contact>> InsertContactAsync(List<Contact> listContact)
        {
            List<Contact> gateWayResultList = await _gateWayClient.InsertContactAsync(listContact);
            List<Contact> result = LoadContact(gateWayResultList);
            return result;
        }

        public async Task<List<Address>> InsertAddressAsync(List<Address> listAddress)
        {
            List<Address> gateWayResultList = await _gateWayClient.InsertAddressAsync(listAddress);
            List<Address> result = LoadAddress(gateWayResultList);
            return result;
        }

        public async Task<List<Client>> DeleteClientAsync(List<Client> listClient)
        {
            List<Client> result = new List<Client>();
            List<Client> gateWayResultList = await _gateWayClient.DeleteClientAsync(listClient);
                if (gateWayResultList.Count == 0)
                    foreach (Client client in listClient)
                    {
                        int returnResult = _dataSet.DeleteClient(client.ID);
                        if (returnResult == 0)
                            result.Add(client);
                    }
            return result;
        }

        public async Task<List<Contact>> DeleteContactAsync(List<Contact> listContact)
        {
            List<Contact> result = new List<Contact>();
            List<Contact> gateWayResultList = await _gateWayClient.DeleteContactAsync(listContact);
                if (gateWayResultList.Count == 0)
                    foreach (Contact contact in listContact)
                    {
                        int returnResult = _dataSet.DeleteContact(contact.ID);
                        if (returnResult == 0)
                            result.Add(contact);
                    }
            return result;
        }

        public async Task<List<Address>> DeleteAddressAsync(List<Address> listAddress)
        {
            List<Address> result = new List<Address>();
            List<Address> gateWayResultList = await _gateWayClient.DeleteAddressAsync(listAddress);
                if (gateWayResultList.Count == 0)
                    foreach (Address address in listAddress)
                    {
                        int returnResult = _dataSet.DeleteAddress(address.ID);
                        if (returnResult == 0)
                            result.Add(address);
                    }
            return result;
        }

        public async Task<List<Client>> UpdateClientAsync(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            QOBDSet dataSet = new QOBDSet();
            List<Client> gateWayResultList = await _gateWayClient.UpdateClientAsync(clientList);

                foreach (var client in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _dataSet.FillClientDataTableById(dataSetLocal.clients, client.ID);
                    dataSet.clients.Merge(dataSetLocal.clients);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _dataSet.UpdateClient(gateWayResultList.ClientTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            return result;
        }

        public List<Client> LoadClient(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            foreach (var client in clientList)
                {
                    var returnResult = _dataSet.LoadClient(client);
                    if (returnResult > 0)
                        result.Add(client);
                }
            return result;
        }

        public async Task<List<Contact>> UpdateContactAsync(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            QOBDSet dataSet = new QOBDSet();
            List<Contact> gateWayResultList = await _gateWayClient.UpdateContactAsync(contactList);

                foreach (var contact in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _dataSet.FillContactDataTableById(dataSetLocal.contacts, contact.ID);
                    dataSet.contacts.Merge(dataSetLocal.contacts);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _dataSet.UpdateContact(gateWayResultList.ContactTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            return result;
        }

        public List<Contact> LoadContact(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            foreach (var contact in contactList)
                {
                    int returnResult = _dataSet.LoadContact(contact);
                    if (returnResult > 0)
                        result.Add(contact);
                }
            return result;
        }

        public async Task<List<Address>> UpdateAddressAsync(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            QOBDSet dataSet = new QOBDSet();
            List<Address> gateWayResultList = await _gateWayClient.UpdateAddressAsync(addressList);

                foreach (var address in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _dataSet.FilladdressDataTableById(dataSetLocal.addresses, address.ID);
                    dataSet.addresses.Merge(dataSetLocal.addresses);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _dataSet.UpdateAddress(gateWayResultList.AddressTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            return result;
        }

        public List<Address> LoadAddress(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            foreach (var address in addressList)
                {
                    int returnResult = _dataSet.LoadAddress(address);
                    if (returnResult > 0)
                        result.Add(address);
                }
            return result;
        }


        public List<Client> GetClientData(int nbLine)
        {
            List<Client> result = _dataSet.GetClientData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }


        public async Task<List<Client>> GetClientDataAsync(int nbLine)
        {
            return await _gateWayClient.GetClientDataAsync(nbLine);
        }

        public List<Client> GetClientDataByBillList(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            foreach (Bill bill in billList)
            {
                var clientList = searchClient(new Client { ID = bill.ClientId }, ESearchOption.AND);
                if (clientList.Count() > 0)
                    result.Add(clientList.First());
            }
            return result;
        }

        public async Task<List<Client>> GetClientDataByBillListAsync(List<Bill> billList)
        {
            return await _gateWayClient.GetClientDataByBillListAsync(billList);
        }

        public async Task<List<Client>> GetClientMaxCreditOverDataByAgentAsync(int agentId)
        {
            return await _gateWayClient.GetClientMaxCreditOverDataByAgentAsync(agentId);
        }

        public List<Contact> GetContactData(int nbLine)
        {
            List<Contact> result = _dataSet.GetContactData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Contact>> GetContactDataAsync(int nbLine)
        {
            return await _gateWayClient.GetContactDataAsync(nbLine);
        }

        public List<Contact> GetContactDataByClientList(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            foreach (Client client in clientList)
            {
                var contactList = searchContact(new Contact { ClientId = client.ID }, ESearchOption.AND);
                if (contactList.Count() > 0)
                    result.Add(contactList.First());
            }
            return result;
        }

        public async Task<List<Contact>> GetContactDataByClientListAsync(List<Client> clientList)
        {
            return await _gateWayClient.GetContactDataByClientListAsync(clientList);
        }

        public List<Address> GetAddressData(int nbLine)
        {
            List<Address> result = _dataSet.GetAddressData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Address>> GetAddressDataAsync(int nbLine)
        {
            return await _gateWayClient.GetAddressDataAsync(nbLine);
        }

        public List<Address> GetAddressDataByOrderList(List<Order> orderList)
        {
            List<Address> result = new List<Address>();
            List<int> idList = new List<int>();
            foreach (Order order in orderList)
            {
                var billAddressList = searchAddress(new Address { ID = order.BillAddress }, ESearchOption.AND);
                var deliveryAddressList = searchAddress(new Address { ID = order.DeliveryAddress }, ESearchOption.AND);
                if (billAddressList.Count() > 0 && !idList.Contains(billAddressList.First().ID))
                {
                    result.Add(billAddressList.First());
                    idList.Add(billAddressList.First().ID);
                }

                if (deliveryAddressList.Count() > 0 && !idList.Contains(deliveryAddressList.First().ID))
                {
                    result.Add(deliveryAddressList.First());
                    idList.Add(deliveryAddressList.First().ID);
                }
            }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByOrderListAsync(List<Order> orderList)
        {
            return await _gateWayClient.GetAddressDataByOrderListAsync(orderList);
        }

        public List<Address> GetAddressDataByClientList(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            foreach (Client client in clientList)
            {
                var clientAddressList = searchAddress(new Address { ClientId = client.ID }, ESearchOption.AND);

                if (clientAddressList.Count() > 0)
                    result.Add(clientAddressList.First());
            }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByClientListAsync(List<Client> clientList)
        {
            return await _gateWayClient.GetAddressDataByClientListAsync(clientList);
        }

        public List<Order> GetOrderClient(int id)
        {
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
                return _ordersTableAdapter.get_order_by_id(id).DataTableTypeToOrder();
        }

        public List<Order> GetQuoteCLient(int id)
        {
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
                return _ordersTableAdapter.get_order_by_id(id).DataTableTypeToOrder();
        }

        public List<Client> GetClientDataById(int id)
        {
            return _dataSet.GetClientDataById(id);
        }

        public List<Contact> GetContactDataById(int id)
        {
            return _dataSet.GetContactDataById(id);
        }

        public List<Address> GetAddressDataById(int id)
        {
            return _dataSet.GetAddressDataById(id);
        }

        public List<Client> searchClient(Client client, ESearchOption filterOperator)
        {
            return _dataSet.searchClient(client, filterOperator);
        }

        public async Task<List<Client>> searchClientAsync(Client client, ESearchOption filterOperator)
        {
            return await _gateWayClient.searchClientAsync(client, filterOperator);
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            return _dataSet.searchContact(Contact, filterOperator);
        }

        public async Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator)
        {
            return await _gateWayClient.searchContactAsync(Contact, filterOperator);
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            return _dataSet.searchAddress(Address, filterOperator);
        }

        public async Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator)
        {
            return await _gateWayClient.searchAddressAsync(Address, filterOperator);
        }

        public void Dispose()
        {
            _gateWayClient.Dispose();
            _dataSet.Dispose();
        }

        public void UpdateClientDependencies(List<Client> clientList, bool isActiveProgress = false)
        {
            int loadUnit = 500;
            ConcurrentBag<Contact> contactList = new ConcurrentBag<Contact>();
            ConcurrentBag<Address> addressList = new ConcurrentBag<Address>();

            // saving the clients
            List<Client> savedClientList = LoadClient(clientList.ToList());

            for (int i = 0; i < (savedClientList.Count() / loadUnit) || loadUnit >= savedClientList.Count() && i == 0; i++)
            {
                ConcurrentBag<Address> addressFoundList = new ConcurrentBag<Address>(new NotifyTaskCompletion<List<Address>>(_gateWayClient.GetAddressDataByClientListAsync(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressFoundList)));

                ConcurrentBag<Contact> contactFoundList = new ConcurrentBag<Contact>(new NotifyTaskCompletion<List<Contact>>(_gateWayClient.GetContactDataByClientListAsync(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                contactList = new ConcurrentBag<Contact>(contactList.Concat(new ConcurrentBag<Contact>(contactFoundList)));
            }

            // saving the addresses into local database
            List<Address> savedAddressList = LoadAddress(addressList.ToList());

            // saving the contacts into the local database
            List<Contact> savedContactList = LoadContact(contactList.ToList());
        }
    } /* end class BlCLient */
}