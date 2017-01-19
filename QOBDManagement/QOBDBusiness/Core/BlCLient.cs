using QOBDBusiness.Helper;
using QOBDCommon;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDBusiness.Core
{
    public class BlCLient : IClientManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC { get; set; }

        public BlCLient(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALClient.initializeCredential(user);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALClient.progressBarManagement(progressBarFunc);
        }

        public async Task<List<Client>> MoveClientAgentBySelection(List<Client> clientList, Agent toAgent)
        {
            List<Client> result = new List<Client>();
            try
            {
                foreach (var client in clientList)
                    client.AgentId = toAgent.ID;
                result = await DAC.DALClient.UpdateClientAsync(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Client>> InsertClientAsync(List<Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return new List<Client>();

            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.InsertClientAsync(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Contact>> InsertContactAsync(List<Contact> contactList)
        {
            if (contactList == null || contactList.Count == 0)
                return new List<Contact>();

            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.InsertContactAsync(contactList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> InsertAddressAsync(List<Address> addressList)
        {
            if (addressList == null || addressList.Count == 0)
                return new List<Address>();

            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.InsertAddressAsync(addressList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Client>> UpdateClientAsync(List<Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return new List<Client>();

            if (clientList.Where(x => x.ID == 0).Count() > 0)
                Log.warning("Updating clients(count = " + clientList.Where(x => x.ID == 0).Count() + ") with ID = 0");

            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.UpdateClientAsync(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Contact>> UpdateContactAsync(List<Contact> contactList)
        {
            if (contactList == null || contactList.Count == 0)
                return new List<Contact>();

            if (contactList.Where(x => x.ID == 0).Count() > 0)
                Log.warning("Updating contacts(count = " + contactList.Where(x => x.ID == 0).Count() + ") with ID = 0");

            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.UpdateContactAsync(contactList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> UpdateAddressAsync(List<Address> addressList)
        {
            if (addressList == null || addressList.Count == 0)
                return new List<Address>();

            if (addressList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating addresses(count = " + addressList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.UpdateAddressAsync(addressList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }


        public async Task<List<Client>> DeleteClientAsync(List<Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return new List<Client>();

            if (clientList.Where(x => x.ID == 0).Count() > 0)
                Log.warning("Deleting clients(count = " + clientList.Where(x => x.ID == 0).Count() + ") with ID = 0");

            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.DeleteClientAsync(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Contact>> DeleteContactAsync(List<Contact> contactList)
        {
            if (contactList == null || contactList.Count == 0)
                return new List<Contact>();

            if (contactList.Where(x => x.ID == 0).Count() > 0)
                Log.warning("Deleting contacts(count = " + contactList.Where(x => x.ID == 0).Count() + ") with ID = 0");

            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.DeleteContactAsync(contactList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> DeleteAddressAsync(List<Address> addressList)
        {
            if (addressList == null || addressList.Count == 0)
                return new List<Address>();

            if (addressList.Where(x => x.ID == 0).Count() > 0)
                Log.warning("Deleting addresses(count = " + addressList.Where(x => x.ID == 0).Count() + ") with ID = 0");

            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.DeleteAddressAsync(addressList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Client> GetClientData(int nbLine)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = DAC.DALClient.GetClientData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Client>> GetClientDataAsync(int nbLine)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.GetClientDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Client> GetClientDataByBillList(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = DAC.DALClient.GetClientDataByBillList(billList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Client>> GetClientDataByBillListAsync(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.GetClientDataByBillListAsync(billList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Contact> GetContactData(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = DAC.DALClient.GetContactData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Contact>> GetContactDataAsync(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.GetContactDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Contact> GetContactDataByClientList(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = DAC.DALClient.GetContactDataByClientList(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Contact>> GetContactDataByClientListAsync(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.GetContactDataByClientListAsync(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Address> GetAddressData(int nbLine)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = DAC.DALClient.GetAddressData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataAsync(int nbLine)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Address> GetAddressDataByOrderList(List<Order> orderList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = DAC.DALClient.GetAddressDataByOrderList(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByOrderListAsync(List<Order> orderList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressDataByOrderListAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Address> GetAddressDataByClientList(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = DAC.DALClient.GetAddressDataByClientList(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByClientListAsync(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressDataByClientListAsync(clientList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }
        

        public List<Order> GetQuoteCLient(int id)
        {
            List<Order> result = new List<Order>();
            try
            {
                var order = new Order();
                order.ClientId = id;
                order.Status = EStatus.Quote.ToString();
                result = DAC.DALOrder.searchOrder(order, ESearchOption.AND);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order> GetOrderClient(int id)
        {
            List<Order> result = new List<Order>();
            try
            {
                var command = new Order();
                command.ClientId = id;
                command.Status = EStatus.Command.ToString();
                result = DAC.DALOrder.searchOrder(command, ESearchOption.AND);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Client> GetClientDataById(int id)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = DAC.DALClient.GetClientDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Contact> GetContactDataById(int id)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = DAC.DALClient.GetContactDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Address> GetAddressDataById(int id)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = DAC.DALClient.GetAddressDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Client> searchClient(Client client, ESearchOption filterOperator)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = DAC.DALClient.searchClient(client, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Client>> searchClientAsync(Client client, ESearchOption filterOperator)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.searchClientAsync(client, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = DAC.DALClient.searchContact(Contact, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.searchContactAsync(Contact, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = DAC.DALClient.searchAddress(Address, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Address>> searchAddressAsync(Address Address, ESearchOption filterOperator)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.searchAddressAsync(Address, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALClient.Dispose();
        }
    }
}