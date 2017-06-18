using QOBDDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDCommon.Entities;
using QOBDDAL.Helper.ChannelHelper;
using QOBDCommon.Enum;
using QOBDCommon.Classes;

namespace QOBDTest.DAC
{
    public class MockDataSet : IQOBDSet
    {

        #region [ Agent ]
        //================================[ Agent ]===================================================
        
        public int DeleteAgent(int agentId)
        {
            return 1;
        }

        public List<Agent> GetAgentData()
        {
            return new List<Agent> {
                new Agent { ID = 1, Status = "Active", FirstName = "First name", LastName = "Last name", UserName = "username", HashedPassword = "password", ListSize = 25 },
                new Agent { ID = 50, Status = "Deactivated", FirstName = "First name", LastName = "Last name", UserName = "username", HashedPassword = "password", ListSize = 25 }
            };
        }

        public List<Agent> GetAgentDataById(int id)
        {
            return new List<Agent> { new Agent { ID = id, Status = "Active", FirstName = "First name", LastName = "Last name", UserName = "username", HashedPassword = "password", ListSize = 25 } };
        }

        public int LoadAgent(Agent agent)
        {
            return 1;
        }

        public List<Agent> searchAgent(Agent agent, ESearchOption filterOperator)
        {
            return new List<Agent> { agent };
        }

        public int UpdateAgent(List<Agent> agentList)
        {
            return agentList.Count;
        }

        #endregion

        #region [ CLient ]
        //================================[ Client ]===================================================
        
        public int DeleteAddress(int addressId)
        {
            return 1;
        }

        public int DeleteClient(int clientId)
        {
            return 1;
        }

        public int DeleteContact(int contactId)
        {
            return 1;
        }

        public List<Address> GetAddressData()
        {
            return new List<Address> {
                new Address { ID = 1, ClientId = 1, Name = "name", Name2 = "name 2" },
                new Address { ID = 2, ClientId = 1, Name = "name", Name2 = "name 2" },
            };
        }

        public List<Client> GetClientData()
        {
            return new List<Client> {
                new Client { ID = 1, AgentId = 1, MaxCredit = 200, FirstName = "", LastName = "" },
                new Client { ID = 3, AgentId = 50, MaxCredit = 200, FirstName = "", LastName = "" },
            };
        }

        public List<Contact> GetContactData()
        {
            return new List<Contact> {
                new Contact { ID = 1, ClientId = 1, Firstname = "", LastName = "" },
                new Contact { ID = 2, ClientId = 1, Firstname = "", LastName = "" },
            };
        }

        public List<Client> GetClientDataById(int id)
        {
            return new List<Client> { new Client { ID = id, AgentId = 1, MaxCredit = 200, FirstName = "", LastName = "" } };
        }

        public List<Contact> GetContactDataById(int id)
        {
            return new List<Contact> { new Contact { ID = id, ClientId = 1, Firstname = "", LastName = "" } };
        }

        public List<Address> GetAddressDataById(int id)
        {
            return new List<Address> { new Address { ID = id, ClientId = 1, Name = "Name", Name2 = "Name 2", LastName = "", FirstName = "" } };
        }

        public int LoadClient(Client client)
        {
            return 1;
        }

        public int LoadContact(Contact contact)
        {
            return 1;
        }

        public int LoadAddress(Address address)
        {
            return 1;
        }

        public List<Client> searchClient(Client Client, ESearchOption filterOperator)
        {
            return new List<Client> { Client };
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            return new List<Contact> { Contact };
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            return new List<Address> { Address };
        }

        public int UpdateClient(List<Client> clientList)
        {
            return clientList.Count;
        }

        public int UpdateContact(List<Contact> contactList)
        {
            return contactList.Count;
        }

        public int UpdateAddress(List<Address> addressList)
        {
            return addressList.Count;
        }

        #endregion

        #region [ Order ]
        //================================[ Order ]===================================================

        public int DeleteBill(int billId)
        {
            return 1;
        }

        public int DeleteDelivery(int deliveryId)
        {
            return 1;
        }

        public int DeleteOrder(int orderId)
        {
            return 1;
        }

        public int DeleteOrder_item(int order_itemId)
        {
            return 1;
        }

        public int DeleteTax_order(int tax_orderId)
        {
            return 1;
        }

        public int DeleteTax(int taxId)
        {
            return 1;
        }

        public List<Order> GetOrderData()
        {
            return new List<Order> {
                new Order { ID = 1, ClientId = 1, AgentId = 1, Date = DateTime.Now, BillAddress = 1, DeliveryAddress = 1, Status = "Order" },
                new Order { ID = 2, ClientId = 1, AgentId = 1, Date = DateTime.Now, BillAddress = 1, DeliveryAddress = 1, Status = "Order" },
            };
        }

        public List<Order> GetOrderDataById(int id)
        {
            return new List<Order> { new Order { ID = id, ClientId = 1, AgentId = 1, Date = DateTime.Now, BillAddress = 1, DeliveryAddress = 1, Status = "Order" } };
        }

        public List<Order_item> GetOrder_itemData()
        {
            return new List<Order_item> {
                new Order_item { ID = 1, OrderId = 1, ItemId = 1, Item_ref = "item ref", Price = 10, Price_purchase = 50, Quantity = 1, Quantity_current = 0, Quantity_delivery = 1 },
                new Order_item { ID = 2, OrderId = 1, ItemId = 1, Item_ref = "item ref", Price = 10, Price_purchase = 50, Quantity = 1, Quantity_current = 0, Quantity_delivery = 1 },
            };
        }

        public List<Order_item> GetOrder_itemDataById(int id)
        {
            return new List<Order_item> { new Order_item { ID = id, OrderId = 1, ItemId = 1, Item_ref = "item ref", Price = 10, Price_purchase = 50, Quantity = 1, Quantity_current = 0, Quantity_delivery = 1 } };
        }

        public List<Order_item> GetOrder_itemDataByOrderId(int orderId)
        {
            return new List<Order_item> { new Order_item { ID = 1, OrderId = orderId, ItemId = 1, Item_ref = "item ref", Price = 10, Price_purchase = 50, Quantity = 1, Quantity_current = 0, Quantity_delivery = 1 } };
        }
        public List<Bill> GetBillData()
        {
            return new List<Bill> {
                new Bill { ID = 1, OrderId = 1, ClientId = 1, Date = DateTime.Now, DateLimit = DateTime.Now, DatePay = DateTime.Now, Pay = 50, PayReceived = 10 },
                new Bill { ID = 2, OrderId = 1, ClientId = 1, Date = DateTime.Now, DateLimit = DateTime.Now, DatePay = DateTime.Now, Pay = 50, PayReceived = 10 }
            };
        }

        public List<Bill> GetBillDataById(int id)
        {
            return new List<Bill> { new Bill { ID = 1, OrderId = 1, ClientId = 1, Date = DateTime.Now, DateLimit = DateTime.Now, DatePay = DateTime.Now, Pay = 50, PayReceived = 10 } };
        }

        public List<Bill> GetBillDataByOrderId(int orderId)
        {
            return new List<Bill> { new Bill { ID = 1, OrderId = orderId, ClientId = 1, Date = DateTime.Now, DateLimit = DateTime.Now, DatePay = DateTime.Now, Pay = 50, PayReceived = 10 } };
        }

        public List<Delivery> GetDeliveryData()
        {
            return new List<Delivery> {
                new Delivery { ID = 1, BillId = 1, OrderId = 1, Package = 1, Date = DateTime.Now },
                new Delivery { ID = 2, BillId = 1, OrderId = 1, Package = 1, Date = DateTime.Now },
            };
        }

        public List<Delivery> GetDeliveryDataById(int id)
        {
            return new List<Delivery> { new Delivery { ID = 1, BillId = 1, OrderId = 1, Package = 1, Date = DateTime.Now } };
        }

        public List<Delivery> GetDeliveryDataByOrderId(int orderId)
        {
            return new List<Delivery> { new Delivery { ID = 1, OrderId = orderId, Package = 1, Date = DateTime.Now } };
        }

        public List<Tax_order> GetTax_orderByOrderId(int orderId)
        {
            return new List<Tax_order> { new Tax_order { ID = 1, OrderId = orderId, Date_insert = DateTime.Now, TaxId = 1, Tax_value = 10 } };
        }

        public List<Tax_order> GetTax_orderData()
        {
            return new List<Tax_order> {
                new Tax_order { ID = 1, OrderId = 1, Date_insert = DateTime.Now, TaxId = 1, Tax_value = 10 },
                new Tax_order { ID = 2, OrderId = 1, Date_insert = DateTime.Now, TaxId = 1, Tax_value = 10 },
            };
        }

        public List<Tax_order> GetTax_orderDataById(int id)
        {
            return new List<Tax_order> { new Tax_order { ID = id, OrderId = 1, Date_insert = DateTime.Now, TaxId = 1, Tax_value = 10 } };
        }

        public List<Tax> GetTaxData()
        {
            return new List<Tax> {
                new Tax { ID = 1, Tax_current = 1, Date_insert = DateTime.Now, Type = "bill", Value = 1 },
                new Tax { ID = 2, Tax_current = 1, Date_insert = DateTime.Now, Type = "bill", Value = 1 },
            };
        }

        public List<Tax> GetTaxDataById(int id)
        {
            return new List<Tax> { new Tax { ID = id, Tax_current = 1, Date_insert = DateTime.Now, Type = "bill", Value = 1 } };
        }

        public int LoadOrder(Order order)
        {
            return 1;
        }

        public int LoadOrder_item(Order_item order_item)
        {
            return 1;
        }

        public int LoadBill(Bill bill)
        {
            return 1;
        }

        public int LoadDelivery(Delivery delivery)
        {
            return 1;
        }

        public int LoadTax(Tax tax)
        {
            return 1;
        }

        public int LoadTax_order(Tax_order tax_order)
        {
            return 1;
        }

        public List<Order> searchOrder(Order Order, ESearchOption filterOperator)
        {
            if (Order.Status == null)
                Order.Status = "Order";
            if (Order.Date == null)
                Order.Date = DateTime.Now;
            return new List<Order> { Order };
        }

        public List<Order_item> searchOrder_item(Order_item Order_item, ESearchOption filterOperator)
        {
            return new List<Order_item> { Order_item };
        }

        public List<Bill> searchBill(Bill Bill, ESearchOption filterOperator)
        {
            return new List<Bill> { Bill };
        }

        public List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator)
        {
            return new List<Delivery> { Delivery };
        }

        public List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator)
        {
            return new List<Tax_order> { Tax_order };
        }

        public List<Tax> searchTax(Tax Tax, ESearchOption filterOperator)
        {
            return new List<Tax> { Tax };
        }

        public int UpdateTax(List<Tax> taxList)
        {
            return taxList.Count;
        }

        public int UpdateTax_order(List<Tax_order> tax_orderList)
        {
            return tax_orderList.Count;
        }

        public int UpdateOrder(List<Order> orderList)
        {
            return orderList.Count;
        }

        public int UpdateOrder_item(List<Order_item> order_itemList)
        {
            return order_itemList.Count;
        }

        public int UpdateBill(List<Bill> billList)
        {
            return billList.Count;
        }

        public int UpdateDelivery(Delivery delivery)
        {
            return 1;
        }

        public int UpdateDelivery(List<Delivery> deliveryList)
        {
            return deliveryList.Count;
        }

        #endregion

        #region [ Item ]
        //================================[ Item ]===================================================

        public int DeleteAuto_ref(int auto_refId)
        {
            return 1;
        }

        public int DeleteItem(int itemId)
        {
            return 1;
        }

        public int DeleteItem_delivery(int item_deliveryId)
        {
            return 1;
        }

        public int DeleteProvider(int providerId)
        {
            return 1;
        }

        public int DeleteProvider_item(int provider_itemId)
        {
            return 1;
        }

        public int DeleteTax_item(int tax_itemId)
        {
            return 1;
        }

        public List<Item> GetItemData()
        {
            return new List<Item> {
                new Item { ID = 1, Ref = "", Type = "Type", Source = 1, Type_sub = "Type_sub", Price_purchase = 10, Price_sell = 20, Name = "Name" },
                new Item { ID = 2, Ref = "", Type = "Type", Source = 1, Type_sub = "Type_sub", Price_purchase = 10, Price_sell = 20, Name = "Name" },
            };
        }

        public List<Item> GetItemDataById(int id)
        {
            return new List<Item> { new Item { ID = id, Ref = "", Type = "Type", Source = 1, Type_sub = "Type_sub", Price_purchase = 10, Price_sell = 20, Name = "Name" } };
        }

        public List<Item_delivery> GetItem_deliveryData()
        {
            return new List<Item_delivery> {
                new Item_delivery { ID = 1, DeliveryId = 1, ItemId = 1, Item_ref = "", Quantity_delivery = 10 },
                new Item_delivery { ID = 2, DeliveryId = 1, ItemId = 1, Item_ref = "", Quantity_delivery = 10 },
            };
        }

        public List<Item_delivery> GetItem_deliveryDataByDeliveryId(int deliveryId)
        {
            return new List<Item_delivery> { new Item_delivery { ID = 1, DeliveryId = deliveryId, ItemId = 1, Item_ref = "", Quantity_delivery = 10 } };
        }

        public List<Item_delivery> GetItem_deliveryDataById(int id)
        {
            return new List<Item_delivery> { new Item_delivery { ID = id, DeliveryId = 1, ItemId = 1, Item_ref = "", Quantity_delivery = 10 } };
        }

        public List<Item_delivery> GetItem_deliveryDataByItemRefId(string itemRef)
        {
            return new List<Item_delivery> { new Item_delivery { ID = 1, DeliveryId = 1, Item_ref = itemRef, ItemId = 1, Quantity_delivery = 10 } };
        }

        public List<Auto_ref> GetAuto_refData()
        {
            return new List<Auto_ref> {
                new Auto_ref { ID = 1, RefId = 1 },
                new Auto_ref { ID = 2, RefId = 1 },
            };
        }

        public List<Auto_ref> GetAuto_refDataById(int id)
        {
            return new List<Auto_ref> { new Auto_ref { ID = id, RefId = 1 } };
        }

        public List<Provider> GetProviderData()
        {
            return new List<Provider> {
                new Provider { ID = 1, Name = "", Source = 1 },
                new Provider { ID = 2, Name = "", Source = 1 },
            };
        }

        public List<Provider> GetProviderDataById(int id)
        {
            return new List<Provider> { new Provider { ID = id, Name = "", Source = 1 } };
        }

        public List<Provider_item> GetProvider_itemData()
        {
            return new List<Provider_item> {
                new Provider_item { ID = 1, ItemId = 1, ProviderId = 1 },
                new Provider_item { ID = 2, ItemId = 1, ProviderId = 1 },
            };
        }

        public List<Provider_item> GetProvider_itemDataById(int id)
        {
            return new List<Provider_item> { new Provider_item { ID = id, ItemId = 1, ProviderId = 1 } };
        }

        public List<Tax_item> GetTax_itemData()
        {
            return new List<Tax_item> {
                new Tax_item { ID = 1, Item_ref = "", itemId = 1, TaxId = 1, Tax_type = "", Tax_value = 10 },
                new Tax_item { ID = 2, Item_ref = "", itemId = 1, TaxId = 1, Tax_type = "", Tax_value = 10 },
            };
        }

        public List<Tax_item> GetTax_itemDataById(int id)
        {
            return new List<Tax_item> { new Tax_item { ID = id, Item_ref = "", itemId = 1, TaxId = 1, Tax_type = "", Tax_value = 10 } };
        }

        public int LoadItem(Item item)
        {
            return 1;
        }

        public int LoadItem_delivery(Item_delivery item_delivery)
        {
            return 1;
        }

        public int LoadAuto_ref(Auto_ref auto_ref)
        {
            return 1;
        }

        public int LoadTax_item(Tax_item tax_item)
        {
            return 1;
        }

        public int LoadProvider(Provider provider)
        {
            return 1;
        }

        public int LoadProvider_item(Provider_item provider_item)
        {
            return 1;
        }
        public List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator)
        {
            return new List<Tax_item> { Tax_item };
        }

        public List<Item> searchItem(Item Item, ESearchOption filterOperator)
        {
            return new List<Item> { Item };
        }

        public List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            return new List<Item_delivery> { Item_delivery };
        }

        public List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            return new List<Auto_ref> { Auto_ref };
        }

        public List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator)
        {
            return new List<Provider> { Provider };
        }

        public List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator)
        {
            return new List<Provider_item> { Provider_item };
        }

        public int UpdateTax_item(List<Tax_item> tax_itemList)
        {
            return tax_itemList.Count;
        }

        public int UpdateProvider(List<Provider> providerList)
        {
            return providerList.Count;
        }

        public int UpdateProvider_item(List<Provider_item> provider_itemList)
        {
            return provider_itemList.Count;
        }

        public int UpdateItem(List<Item> itemList)
        {
            return itemList.Count;
        }

        public int UpdateItem_delivery(List<Item_delivery> item_deliveryList)
        {
            return item_deliveryList.Count;
        }

        public int UpdateAuto_ref(List<Auto_ref> auto_refList)
        {
            return auto_refList.Count;
        }

        #endregion

        #region [ Notification ]
        //================================[ Notification ]===================================================

        public int DeleteNotification(int notificationId)
        {
            return 1;
        }

        public List<Notification> GetNotificationData()
        {
            return new List<Notification> {
                new Notification { ID = 1, BillId = 1, Date = DateTime.Now, Reminder1 = DateTime.Now, Reminder2 = DateTime.Now },
                new Notification { ID = 1, BillId = 1, Date = DateTime.Now, Reminder1 = DateTime.Now, Reminder2 = DateTime.Now },
            };
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            return new List<Notification> { new Notification { ID = id, BillId = 1, Date = DateTime.Now, Reminder1 = DateTime.Now, Reminder2 = DateTime.Now } };
        }

        public int LoadNotification(Notification notification)
        {
            return 1;
        }

        public List<Notification> searchNotification(Notification Notification, ESearchOption filterOperator)
        {
            return new List<Notification> { Notification };
        }

        public int UpdateNotification(List<Notification> notificationList)
        {
            return notificationList.Count;
        }

        #endregion

        #region [ Referential ]
        //================================[ Referential ]===================================================

        public int DeleteInfo(int infoId)
        {
            return 1;
        }

        public List<Info> GetInfosData()
        {
            return new List<Info> {
                new Info { ID = 1, Name = "", Value = "" },
                new Info { ID = 2, Name = "", Value = "" },
            };
        }

        public List<Info> GetInfosDataById(int id)
        {
            return new List<Info> { new Info { ID = id, Name = "", Value = "" } };
        }

        public int LoadInfo(Info info)
        {
            return 1;
        }

        public List<Info> searchInfo(Info Info, ESearchOption filterOperator)
        {
            return new List<Info> { Info };
        }

        public int UpdateInfo(List<Info> infoList)
        {
            return infoList.Count;
        }

        #endregion

        #region [ Statisitc ]
        //================================[ Statistic ]===================================================

        public int DeleteStatistic(int statisticId)
        {
            return 1;
        }

        public List<Statistic> GetStatisticData()
        {
            return new List<Statistic> {
                new Statistic { ID = 1, BillId = 1, Date_limit = DateTime.Now, Bill_datetime = DateTime.Now, Pay_date = DateTime.Now, Income = 200, Total = 200, Income_percent = 10, Pay_received = 200, Price_purchase_total = 200, Total_tax_included = 200, Tax_value = 20  },
                new Statistic { ID = 2, BillId = 1, Date_limit = DateTime.Now, Bill_datetime = DateTime.Now, Pay_date = DateTime.Now, Income = 200, Total = 200, Income_percent = 10, Pay_received = 200, Price_purchase_total = 200, Total_tax_included = 200, Tax_value = 20  },
            };
        }

        public List<Statistic> GetStatisticDataById(int id)
        {
            return new List<Statistic> { new Statistic { ID = id, BillId = 1, Date_limit = DateTime.Now, Bill_datetime = DateTime.Now, Pay_date = DateTime.Now, Income = 200, Total = 200, Income_percent = 10, Pay_received = 200, Price_purchase_total = 200, Total_tax_included = 200, Tax_value = 20 } };
        }

        public int LoadStatistic(Statistic statistic)
        {
            return 1;
        }

        public List<Statistic> searchStatistic(Statistic Statistic, ESearchOption filterOperator)
        {
            return new List<Statistic> { Statistic };
        }

        public int UpdateStatistic(List<Statistic> statisticList)
        {
            return statisticList.Count;
        }

        #endregion


        public void Dispose()
        {
            
        }

        public int DeleteCurrency(int currencyId)
        {
            throw new NotImplementedException();
        }

        public int UpdateCurrency(List<Currency> currencyList)
        {
            throw new NotImplementedException();
        }

        public int LoadCurrency(Currency currency)
        {
            throw new NotImplementedException();
        }

        public List<Currency> GetCurrencyData()
        {
            throw new NotImplementedException();
        }

        public List<Currency> GetCurrencyDataById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Currency> GetCurrencyDataByProvider_item(Provider_item provider_item)
        {
            throw new NotImplementedException();
        }

        public List<Currency> searchCurrency(Currency Currency, ESearchOption filterOperator)
        {
            throw new NotImplementedException();
        }
    }
}
