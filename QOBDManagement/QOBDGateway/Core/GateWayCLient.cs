using QOBDGateway;
using QOBDCommon.Entities;
using QOBDCommon.Interfaces.REMOTE;
using QOBDGateway.Classes;
using QOBDGateway.Helper.ChannelHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using QOBDGateway.QOBDServiceReference;
using QOBDCommon.Enum;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWayClient : IClientManager, INotifyPropertyChanged
    {
        private ClientProxy _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayClient(ClientProxy servicePortType)
        {
            _channel = servicePortType;
        }

        public void setServiceCredential(object channel)
        {
            //_channel.Close();
            _channel = (ClientProxy)channel; //new QOBDWebServicePortTypeClient("QOBDWebServicePort");
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Client>> InsertClientAsync(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.insert_data_clientAsync(clientList.ClientTypeToArray())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Contact>> InsertContactAsync(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            try
            {                
                result = (await _channel.insert_data_contactAsync(contactList.ContactTypeToArray())).ArrayTypeToContact();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> InsertAddressAsync(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            try
            {                
                result = (await _channel.insert_data_addressAsync(addressList.AddressTypeToArray())).ArrayTypeToAddress();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> UpdateClientAsync(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.update_data_clientAsync(clientList.ClientTypeToArray())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Contact>> UpdateContactAsync(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            try
            {                
                result = (await _channel.update_data_contactAsync(contactList.ContactTypeToArray())).ArrayTypeToContact();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> UpdateAddressAsync(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            try
            {                
                result = (await _channel.update_data_addressAsync(addressList.AddressTypeToArray())).ArrayTypeToAddress();
             }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> DeleteClientAsync(List<Client> clientList)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.delete_data_clientAsync(clientList.ClientTypeToArray())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Contact>> DeleteContactAsync(List<Contact> contactList)
        {
            List<Contact> result = new List<Contact>();
            try
            {                
                result = (await _channel.delete_data_contactAsync(contactList.ContactTypeToArray())).ArrayTypeToContact();
             }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> DeleteAddressAsync(List<Address> addressList)
        {
            List<Address> result = new List<Address>();
            try
            {                
                result = (await _channel.delete_data_addressAsync(addressList.AddressTypeToArray())).ArrayTypeToAddress();
             }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Client>> GetClientDataAsync(int nbLine)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.get_data_clientAsync(nbLine.ToString())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> GetClientMaxCreditOverDataByAgentAsync(int agentId)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = (await _channel.get_data_client_by_max_credit_overAsync(agentId)).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> GetClientDataByBillListAsync(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = (await _channel.get_data_client_by_bill_listAsync(billList.BillTypeToArray())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> GetClientDataByOrderListAsync(List<Order> orderList)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = (await _channel.get_data_client_by_order_listAsync(orderList.OrderTypeToArray())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Contact>> GetContactDataAsync(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            try
            {                
                result = (await _channel.get_data_contactAsync(nbLine.ToString())).ArrayTypeToContact();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Contact>> GetContactDataByClientListAsync(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = (await _channel.get_data_contact_by_client_listAsync(clientList.ClientTypeToArray())).ArrayTypeToContact();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataAsync(int nbLine)
        {
            List<Address> result = new List<Address>();
            try
            {                
                result = (await _channel.get_data_addressAsync(nbLine.ToString())).ArrayTypeToAddress();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByOrderListAsync(List<Order> orderList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = (await _channel.get_data_address_by_order_listAsync(orderList.OrderTypeToArray())).ArrayTypeToAddress();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByClientListAsync(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = (await _channel.get_data_address_by_client_listAsync(clientList.ClientTypeToArray())).ArrayTypeToAddress();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order>> GetOrderClientAsync(int id)
        {
            List<Order> result = new List<Order>();
            try
            {                
                result = (await _channel.get_orders_clientAsync(id.ToString())).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order>> GetQuoteCLientAsync(int id)
        {
            List<Order> result = new List<Order>();
            try
            {                
                result = (await _channel.get_quotes_clientAsync(id.ToString())).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> GetClientDataByIdAsync(int id)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.get_data_client_by_idAsync(id.ToString())).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Contact>> GetContactDataByIdAsync(int id)
        {
            List<Contact> result = new List<Contact>();
            try
            {                
                result = (await _channel.get_data_contact_by_idAsync(id.ToString())).ArrayTypeToContact();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByIdAsync(int id)
        {
            List<Address> result = new List<Address>();
            try
            {                
                result = (await _channel.get_data_address_by_idAsync(id.ToString())).ArrayTypeToAddress();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Client>> searchClientAsync(Client client, ESearchOption filterOperator)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.get_filter_ClientAsync(client.ClientTypeToFilterArray(filterOperator))).ArrayTypeToClient();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator)
        {
            List<Contact> result = new List<Contact>();
            try
            {                
                result = (await _channel.get_filter_contactAsync(Contact.ContactTypeToFilterArray(filterOperator))).ArrayTypeToContact();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator)
        {
            List<Address> result = new List<Address>();
            try
            {                
                result = (await _channel.get_filter_addressAsync(Address.AddressTypeToFilterArray(filterOperator))).ArrayTypeToAddress();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Client>> searchClientFromWebServiceAsync(Client client, ESearchOption filterOperator)
        {
            return await searchClientAsync(client, filterOperator);
        }

        public async Task<List<Contact>> searchContactFromWebServiceAsync(Contact Contact, ESearchOption filterOperator)
        {
            return await searchContactAsync(Contact, filterOperator);
        }

        public async Task<List<Address>> searchAddressFromWebService(Address Address, ESearchOption filterOperator)
        {
            return await searchAddressAsync(Address, filterOperator);
        }

        public void Dispose()
        {
            if (_channel.State == CommunicationState.Opened)
                _channel.Close();
        }
    } /* end class BlCLient */
}