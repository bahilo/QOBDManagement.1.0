using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
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
using QOBDGateway.QOBDServiceReference;
using QOBDDAL.Classes;
using QOBDDAL.Interfaces;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;
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
        private Func<double, double> _progressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IClientManager _gateWayClient;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALClient(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gateWayClient = new GateWayClient(_servicePortType);
            _loadSize = Utility.intTryParse(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Utility.intTryParse(ConfigurationManager.AppSettings["progress_step"]);
        }

        public DALClient(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public DALClient(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
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
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                setServiceCredential(_servicePortType);
            }
        }

        public async void cacheWebServiceData()
        {
            await retrieveGateWayClientDataAsync();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayClient.setServiceCredential(_servicePortType);
        }

        private async Task retrieveGateWayClientDataAsync()
        {       
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                checkServiceCommunication();
                ConcurrentBag<Client> loadedClientList = new ConcurrentBag<Client>();
                ConcurrentBag<Client> clientList = new ConcurrentBag<Client>();

                do
                {
                    int lastLoadedClientID = loadedClientList.OrderBy(x => x.ID).Select(x => x.ID).FirstOrDefault();
                    loadedClientList = new ConcurrentBag<Client>(await _gateWayClient.searchClientAsync(new Client { AgentId = AuthenticatedUser.ID, Option = lastLoadedClientID }, ESearchOption.AND));
                    clientList = new ConcurrentBag<Client>(clientList.Concat(loadedClientList).ToList());
                } while (loadedClientList.Count > 0);

                    if (clientList.Count > 0)
                    await UpdateClientDependenciesAsync(clientList.ToList());
                //Log.debug("-- Clients loaded --");
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CLIENT);
            }
            finally
            {
                lock (_lock) IsLodingDataFromWebServiceToLocal = false;
                try
                {
                    _progressBarFunc((double)100 / _progressStep);
                }
                catch (DivideByZeroException ex) { Log.error(ex.Message, EErrorFrom.STATISTIC); }
                // Log.debug("Loaded[" + _progressBarFunc(0) + "%]!", EErrorFrom.CLIENT);
            }
            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }

        private void checkServiceCommunication()
        {
            if (_servicePortType.State == System.ServiceModel.CommunicationState.Closed || _servicePortType.State == System.ServiceModel.CommunicationState.Faulted)
                _serviceCommunication.resetCommunication();
        }

        #region [ Actions ]
        //=================================[ Actions ]================================================

        public async Task<List<Client>> InsertClientAsync(List<Client> listClient)
        {
            checkServiceCommunication();
            List<Client> gateWayResultList = await _gateWayClient.InsertClientAsync(listClient);
            List<Client> result = LoadClient(gateWayResultList);
            return result;
        }

        public async Task<List<Contact>> InsertContactAsync(List<Contact> listContact)
        {
            checkServiceCommunication();
            List<Contact> gateWayResultList = await _gateWayClient.InsertContactAsync(listContact);
            List<Contact> result = LoadContact(gateWayResultList);
            return result;
        }

        public async Task<List<Address>> InsertAddressAsync(List<Address> listAddress)
        {
            checkServiceCommunication();
            List<Address> gateWayResultList = await _gateWayClient.InsertAddressAsync(listAddress);
            List<Address> result = LoadAddress(gateWayResultList);
            return result;
        }

        public async Task<List<Client>> DeleteClientAsync(List<Client> listClient)
        {
            List<Client> result = new List<Client>();
            checkServiceCommunication();
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
            checkServiceCommunication();
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
            checkServiceCommunication();
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
            checkServiceCommunication();
            List<Client> gateWayResultList = await _gateWayClient.UpdateClientAsync(clientList);
            List<Client> result = LoadClient(gateWayResultList);
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
            checkServiceCommunication();
            List<Contact> gateWayResultList = await _gateWayClient.UpdateContactAsync(contactList);
            List<Contact> result = LoadContact(gateWayResultList);
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
            checkServiceCommunication();
            List<Address> gateWayResultList = await _gateWayClient.UpdateAddressAsync(addressList);
            List<Address> result = LoadAddress(gateWayResultList);
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
            checkServiceCommunication();
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
            checkServiceCommunication();
            return await _gateWayClient.GetClientDataByBillListAsync(billList);
        }

        public async Task<List<Client>> GetClientDataByOrderListAsync(List<Order> orderList)
        {
            checkServiceCommunication();
            return await _gateWayClient.GetClientDataByOrderListAsync(orderList);
        }

        public async Task<List<Client>> GetClientMaxCreditOverDataByAgentAsync(int agentId)
        {
            checkServiceCommunication();
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
            checkServiceCommunication();
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
            checkServiceCommunication();
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
            checkServiceCommunication();
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
            checkServiceCommunication();
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
            checkServiceCommunication();
            return await _gateWayClient.GetAddressDataByClientListAsync(clientList);
        }

        public List<Order> GetOrderClient(int id)
        {
            return _dataSet.searchOrder(new Order { ClientId = id }, ESearchOption.AND);
        }

        public List<Order> GetQuoteCLient(int id)
        {            
            return _dataSet.GetOrderDataById(id);
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
            checkServiceCommunication();
            return await _gateWayClient.searchClientAsync(client, filterOperator);
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            return _dataSet.searchContact(Contact, filterOperator);
        }

        public async Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayClient.searchContactAsync(Contact, filterOperator);
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            return _dataSet.searchAddress(Address, filterOperator);
        }

        public async Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayClient.searchAddressAsync(Address, filterOperator);
        }
        #endregion

        public void Dispose()
        {
            if (_gateWayClient != null)
                _gateWayClient.Dispose();
        }

        public async Task UpdateClientDependenciesAsync(List<Client> clientList, bool isActiveProgress = false)
        {
            int loadUnit = 500;
            ConcurrentBag<Contact> contactList = new ConcurrentBag<Contact>();
            ConcurrentBag<Address> addressList = new ConcurrentBag<Address>();

            // saving the clients
            List<Client> savedClientList = LoadClient(clientList.ToList());

            for (int i = 0; i < (savedClientList.Count() / loadUnit) || loadUnit >= savedClientList.Count() && i == 0; i++)
            {
                ConcurrentBag<Address> addressFoundList = new ConcurrentBag<Address>(await _gateWayClient.GetAddressDataByClientListAsync(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())); 
                addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressFoundList)));

                ConcurrentBag<Contact> contactFoundList = new ConcurrentBag<Contact>(await _gateWayClient.GetContactDataByClientListAsync(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList()));
                contactList = new ConcurrentBag<Contact>(contactList.Concat(new ConcurrentBag<Contact>(contactFoundList)));
            }

            // saving the addresses into local database
            List<Address> savedAddressList = LoadAddress(addressList.ToList());

            // saving the contacts into the local database
            List<Contact> savedContactList = LoadContact(contactList.ToList());
        }
    } /* end class BlCLient */
}