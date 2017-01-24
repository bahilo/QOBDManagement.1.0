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
        private GateWayClient _gateWayClient;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALClient()
        {
            _gateWayClient = new GateWayClient();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayClient.PropertyChanged += onCredentialChange_loadClientDataFromWebService;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public GateWayClient GateWayClient
        {
            get { return _gateWayClient; }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayClient.initializeCredential(user);
            }
        }

        private void onCredentialChange_loadClientDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                retrieveGateWayDataClient();
                //DALHelper.doActionAsync();                
            }
        }

        private void retrieveGateWayDataClient()
        {
            int loadUnit = 500;

            ConcurrentBag<Client> clientList = new ConcurrentBag<Client>(new NotifyTaskCompletion<List<Client>>(_gateWayClient.GetClientDataAsync(_loadSize)).Task.Result);
            ConcurrentBag<Contact> contactList = new ConcurrentBag<Contact>();
            ConcurrentBag<Address> addressList = new ConcurrentBag<Address>();
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                if (clientList.Count > 0)
                {
                    List<Client> savedClientList = LoadClient(clientList.ToList());

                    for (int i = 0; i < (savedClientList.Count() / loadUnit) || loadUnit >= savedClientList.Count() && i == 0; i++)
                    {
                        ConcurrentBag<Address> addressFoundList = new ConcurrentBag<Address>(new NotifyTaskCompletion<List<Address>>(_gateWayClient.GetAddressDataByClientListAsync(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                        addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressFoundList)));

                        ConcurrentBag<Contact> contactFoundList = new ConcurrentBag<Contact>(new NotifyTaskCompletion<List<Contact>>(_gateWayClient.GetContactDataByClientListAsync(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                        contactList = new ConcurrentBag<Contact>(contactList.Concat(new ConcurrentBag<Contact>(contactFoundList)));
                    }
                    List<Address> savedAddressList = LoadAddress(addressList.ToList()); // UpdateAddress(addressList.ToList());
                    List<Contact> savedContactList = LoadContact(new NotifyTaskCompletion<List<Contact>>(_gateWayClient.GetContactDataByClientListAsync(clientList.ToList())).Task.Result);
                }
                Log.debug("-- Clients loaded --");
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
            List<Client> result = new List<Client>();
            List<Client> gateWayResultList = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.InsertClientAsync(listClient);

                result = LoadClient(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Contact>> InsertContactAsync(List<Contact> listContact)
        {
            List<Contact> result = new List<Contact>();
            List<Contact> gateWayResultList = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.InsertContactAsync(listContact);
                
                result = LoadContact(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Address>> InsertAddressAsync(List<Address> listAddress)
        {
            List<Address> result = new List<Address>();
            List<Address> gateWayResultList = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.InsertAddressAsync(listAddress);
                
                result = LoadAddress(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Client>> UpdateClientAsync(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            List<Client> gateWayResultList = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            {
                QOBDSet dataSet = new QOBDSet();
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.UpdateClientAsync(clientList);

                foreach (var client in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _clientsTableAdapter.FillById(dataSetLocal.clients, client.ID);
                    dataSet.clients.Merge(dataSetLocal.clients);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _clientsTableAdapter.Update(gateWayResultList.ClientTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Client> LoadClient(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            {
                foreach (var client in clientList)
                {
                    var returnResult = _clientsTableAdapter
                                            .load_data_client(
                                                    client.AgentId,
                                                    client.FirstName,
                                                    client.LastName,
                                                    client.Company,
                                                    client.Email,
                                                    client.Phone,
                                                    client.Fax,
                                                    client.Rib,
                                                    client.CRN,
                                                    client.PayDelay,
                                                    client.Comment,
                                                    client.Status,
                                                    client.MaxCredit,
                                                    client.CompanyName,
                                                    client.ID);
                    if (returnResult > 0)
                        result.Add(client);
                }

            }
            return result;
        }

        public async Task<List<Contact>> UpdateContactAsync(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            List<Contact> gateWayResultList = new List<Contact>();
            QOBDSet dataSet = new QOBDSet();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.UpdateContactAsync(contactList);

                foreach (var contact in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _contactsTableAdapter.FillById(dataSetLocal.contacts, contact.ID);
                    dataSet.contacts.Merge(dataSetLocal.contacts);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _contactsTableAdapter.Update(gateWayResultList.ContactTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Contact> LoadContact(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            {
                foreach (var contact in contactList)
                {
                    int returnResult = _contactsTableAdapter
                                            .load_data_contact(
                                                    contact.ClientId,
                                                    contact.LastName,
                                                    contact.Firstname,
                                                    contact.Position,
                                                    contact.Email,
                                                    contact.Phone,
                                                    contact.Cellphone,
                                                    contact.Fax,
                                                    contact.Comment,
                                                    contact.ID);
                    if (returnResult > 0)
                        result.Add(contact);
                }
            }
            return result;
        }

        public async Task<List<Address>> UpdateAddressAsync(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            List<Address> gateWayResultList = new List<Address>();
            QOBDSet dataSet = new QOBDSet();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.UpdateAddressAsync(addressList);

                foreach (var address in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _addressesTableAdapter.FillById(dataSetLocal.addresses, address.ID);
                    dataSet.addresses.Merge(dataSetLocal.addresses);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _addressesTableAdapter.Update(gateWayResultList.AddressTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Address> LoadAddress(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            {
                foreach (var address in addressList)
                {
                    int returnResult = _addressesTableAdapter
                                            .load_data_address(
                                                    address.ClientId,
                                                    address.Name,
                                                    address.Name2,
                                                    address.CityName,
                                                    address.AddressName,
                                                    address.Postcode,
                                                    address.Country,
                                                    address.Comment,
                                                    address.FirstName,
                                                    address.LastName,
                                                    address.Phone,
                                                    address.Email,
                                                    address.ID);
                    if (returnResult > 0)
                        result.Add(address);
                }
            }
            return result;
        }

        public async Task<List<Client>> DeleteClientAsync(List<Client> listClient)
        {
            List<Client> result = listClient;
            List<Client> gateWayResultList = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.DeleteClientAsync(listClient);
                if (gateWayResultList.Count == 0)
                    foreach (Client client in listClient)
                    {
                        int returnResult = _clientsTableAdapter.Delete1(client.ID);
                        if (returnResult > 0)
                            result.Remove(client);
                    }
            }
            return result;
        }

        public async Task<List<Contact>> DeleteContactAsync(List<Contact> listContact)
        {
            List<Contact> result = listContact;
            List<Contact> gateWayResultList = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.DeleteContactAsync(listContact);
                if (gateWayResultList.Count == 0)
                    foreach (Contact contact in listContact)
                    {
                        int returnResult = _contactsTableAdapter.Delete1(contact.ID);
                        if (returnResult > 0)
                            result.Remove(contact);
                    }
            }
            return result;
        }

        public async Task<List<Address>> DeleteAddressAsync(List<Address> listAddress)
        {
            List<Address> result = listAddress;
            List<Address> gateWayResultList = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            {
                _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayClient.DeleteAddressAsync(listAddress);
                if (gateWayResultList.Count == 0)
                    foreach (Address address in listAddress)
                    {
                        int returnResult = _addressesTableAdapter.Delete1(address.ID);
                        if (returnResult > 0)
                            result.Remove(address);
                    }
            }
            return result;
        }


        public List<Client> GetClientData(int nbLine)
        {
            List<Client> result = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
                result = _clientsTableAdapter.GetData().DataTableTypeToClient();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }


        public async Task<List<Client>> GetClientDataAsync(int nbLine)
        {
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
             _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayClient.GetClientDataByBillListAsync(billList);
        }

        public List<Contact> GetContactData(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
                result = _contactsTableAdapter.GetData().DataTableTypeToContact();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Contact>> GetContactDataAsync(int nbLine)
        {
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayClient.GetContactDataByClientListAsync(clientList);
        }

        public List<Address> GetAddressData(int nbLine)
        {
            List<Address> result = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
                result = _addressesTableAdapter.GetData().DataTableTypeToAddress();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Address>> GetAddressDataAsync(int nbLine)
        {
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
                return _clientsTableAdapter.get_client_by_id(id).DataTableTypeToClient();
        }

        public List<Contact> GetContactDataById(int id)
        {
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
                return _contactsTableAdapter.get_contact_by_id(id).DataTableTypeToContact();
        }

        public List<Address> GetAddressDataById(int id)
        {
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
                return _addressesTableAdapter.get_address_by_id(id).DataTableTypeToAddress();
        }

        public List<Client> searchClient(Client client, ESearchOption filterOperator)
        {
            return client.ClientTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Client>> searchClientAsync(Client client, ESearchOption filterOperator)
        {
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayClient.searchClientAsync(client, filterOperator);
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            return Contact.ContactTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator)
        {
             _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayClient.searchContactAsync(Contact, filterOperator);
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            return Address.AddressTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator)
        {
            _gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayClient.searchAddressAsync(Address, filterOperator);
        }

        public void Dispose()
        {
            _gateWayClient.PropertyChanged -= onCredentialChange_loadClientDataFromWebService;
            _gateWayClient.Dispose();
        }
    } /* end class BlCLient */
}