using QOBDDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDDAL.Helper.ChannelHelper;

namespace QOBDDAL.Classes
{
    public class QOBDDataSet : IQOBDSet
    {    


        //----------------------------[ Actions ]------------------

        #region[ Order Commands ]

        // delete

        public int DeleteOrder(int orderId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Orders", DALHelper.getColumDictionary(new Order { ID = orderId })));
            return new Order { ID = orderId }.filterDataTableToOrderType(ESearchOption.AND).Count;
        }

        public int DeleteTax_order(int tax_orderId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Tax_orders", DALHelper.getColumDictionary(new Tax_order { ID = tax_orderId })));
            return new Tax_order { ID = tax_orderId }.filterDataTableToTax_orderType(ESearchOption.AND).Count;
        }

        public int DeleteOrder_item(int order_itemId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Order_items", DALHelper.getColumDictionary(new Order_item { ID = order_itemId })));
            return new Order_item { ID = order_itemId }.filterDataTableToOrder_itemType(ESearchOption.AND).Count;
        }

        public int DeleteTax(int taxId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Taxes", DALHelper.getColumDictionary(new Tax { ID = taxId })));
            return new Tax { ID = taxId }.filterDataTableToTaxType(ESearchOption.AND).Count;
        }

        public int DeleteBill(int billId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Bills", DALHelper.getColumDictionary(new Bill { ID = billId })));
            return new Bill { ID = billId }.filterDataTableToBillType(ESearchOption.AND).Count;
        }

        public int DeleteDelivery(int deliveryId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Deliveries", DALHelper.getColumDictionary(new Delivery { ID = deliveryId })));
            return new Delivery { ID = deliveryId }.filterDataTableToDeliveryType(ESearchOption.AND).Count;
        }


        // update

        public int UpdateOrder(List<Order> orderList)
        {
            int count = 0;
            foreach (var order in orderList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Orders", DALHelper.getColumDictionary(order)));
                count += order.filterDataTableToOrderType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateTax_order(List<Tax_order> tax_orderList)
        {
            int count = 0;
            foreach (var tax_order in tax_orderList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Tax_orders", DALHelper.getColumDictionary(tax_order)));
                count += tax_order.filterDataTableToTax_orderType(ESearchOption.AND).Count;
            }
            return count;

        }

        public int UpdateOrder_item(List<Order_item> order_itemList)
        {
            int count = 0;
            foreach (var order_item in order_itemList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Order_items", DALHelper.getColumDictionary(order_item)));
                count += order_item.filterDataTableToOrder_itemType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateTax(List<Tax> taxList)
        {
            int count = 0;
            foreach (var tax in taxList)
            {
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Taxes", DALHelper.getColumDictionary(tax)));
                count += tax.filterDataTableToTaxType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateBill(List<Bill> billList)
        {
            int count = 0;
            foreach (var bill in billList)
            {
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Bills", DALHelper.getColumDictionary(bill)));
                count += bill.filterDataTableToBillType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateDelivery(List<Delivery> deliveryList)
        {
            int count = 0;
            foreach (var delivery in deliveryList)
            {
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Deliveries", DALHelper.getColumDictionary(delivery)));
                count += delivery.filterDataTableToDeliveryType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadOrder(Order order)
        {
            var dataFoundList = new Order { ID = order.ID }.filterDataTableToOrderType(ESearchOption.AND);
            if(dataFoundList.Count != 0)  
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Orders", DALHelper.getColumDictionary(order)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Orders", DALHelper.getColumDictionary(order)));

            return new Order { ID = order.ID }.filterDataTableToOrderType(ESearchOption.AND).Count;
        }

        public int LoadTax_order(Tax_order tax_order)
        {
            var dataFoundList = new Tax_order { ID = tax_order.ID }.filterDataTableToTax_orderType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Tax_orders", DALHelper.getColumDictionary(tax_order)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Tax_orders", DALHelper.getColumDictionary(tax_order)));
            return new Tax_order { ID = tax_order.ID }.filterDataTableToTax_orderType(ESearchOption.AND).Count;
        }

        public int LoadOrder_item(Order_item order_item)
        {
            var dataFoundList = new Order_item { ID = order_item.ID }.filterDataTableToOrder_itemType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Order_items", DALHelper.getColumDictionary(order_item)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Order_items", DALHelper.getColumDictionary(order_item)));
            return new Order_item { ID = order_item.ID }.filterDataTableToOrder_itemType(ESearchOption.AND).Count;
        }

        public int LoadTax(Tax tax)
        {
            var dataFoundList = new Tax { ID = tax.ID }.filterDataTableToTaxType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Taxes", DALHelper.getColumDictionary(tax)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Taxes", DALHelper.getColumDictionary(tax)));
            return new Tax { ID = tax.ID }.filterDataTableToTaxType(ESearchOption.AND).Count;
        }

        public int LoadBill(Bill bill)
        {
            var dataFoundList = new Bill { ID = bill.ID }.filterDataTableToBillType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Bills", DALHelper.getColumDictionary(bill)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Bills", DALHelper.getColumDictionary(bill)));
            return new Bill { ID = bill.ID }.filterDataTableToBillType(ESearchOption.AND).Count;
        }

        public int LoadDelivery(Delivery delivery)
        {
            var dataFoundList = new Delivery { ID = delivery.ID }.filterDataTableToDeliveryType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Deliveries", DALHelper.getColumDictionary(delivery)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Deliveries", DALHelper.getColumDictionary(delivery)));
            return new Delivery { ID = delivery.ID }.filterDataTableToDeliveryType(ESearchOption.AND).Count;
        }


        // getting

        public List<Order> GetOrderData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Orders", DALHelper.getColumDictionary(new Order()))).DataTableTypeToOrder();
        }

        public List<Order> GetOrderDataById(int id)
        {
            return new Order { ID = id }.filterDataTableToOrderType(ESearchOption.AND);
        }

        public List<Tax_order> GetTax_orderData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Tax_orders", DALHelper.getColumDictionary(new Tax_order()))).DataTableTypeToTax_order();
        }

        public List<Tax_order> GetTax_orderByOrderId(int orderId)
        {
            return new Tax_order { OrderId = orderId }.filterDataTableToTax_orderType(ESearchOption.AND);
        }

        public List<Tax_order> GetTax_orderDataById(int id)
        {
            return new Tax_order { ID = id }.filterDataTableToTax_orderType(ESearchOption.AND);
        }

        public List<Order_item> GetOrder_itemData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Order_items", DALHelper.getColumDictionary(new Order_item()))).DataTableTypeToOrder_item();
        }

        public List<Order_item> GetOrder_itemDataById(int id)
        {
            return new Order_item { ID = id }.filterDataTableToOrder_itemType(ESearchOption.AND);
        }

        public List<Order_item> GetOrder_itemDataByOrderId(int orderId)
        {
            return new Order_item { OrderId = orderId }.filterDataTableToOrder_itemType(ESearchOption.AND);
        }

        public List<Tax> GetTaxData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Taxes", DALHelper.getColumDictionary(new Tax()))).DataTableTypeToTax();
        }

        public List<Tax> GetTaxDataById(int id)
        {
            return new Tax { ID = id }.filterDataTableToTaxType(ESearchOption.AND);
        }

        public List<Bill> GetBillData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Bills", DALHelper.getColumDictionary(new Bill()))).DataTableTypeToBill();
        }

        public List<Bill> GetBillDataById(int id)
        {
            return new Bill { ID = id }.filterDataTableToBillType(ESearchOption.AND);
        }

        public List<Bill> GetBillDataByOrderId(int orderId)
        {
            return new Bill { OrderId = orderId }.filterDataTableToBillType(ESearchOption.AND);
        }

        public List<Delivery> GetDeliveryData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Deliveries", DALHelper.getColumDictionary(new Delivery()))).DataTableTypeToDelivery();
        }

        public List<Delivery> GetDeliveryDataById(int id)
        {
            return new Delivery { ID = id }.filterDataTableToDeliveryType(ESearchOption.AND);
        }

        public List<Delivery> GetDeliveryDataByOrderId(int orderId)
        {
            return new Delivery { OrderId = orderId }.filterDataTableToDeliveryType(ESearchOption.AND);
        }        

        // search

        public List<Order> searchOrder(Order order, ESearchOption filterOperator)
        {
            return order.filterDataTableToOrderType(filterOperator);
        }

        public List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator)
        {
            return Tax_order.filterDataTableToTax_orderType(filterOperator);
        }

        public List<Order_item> searchOrder_item(Order_item order_item, ESearchOption filterOperator)
        {
            return order_item.filterDataTableToOrder_itemType(filterOperator);
        }

        public List<Tax> searchTax(Tax Tax, ESearchOption filterOperator)
        {
            return Tax.filterDataTableToTaxType(filterOperator);
        }

        public List<Bill> searchBill(Bill Bill, ESearchOption filterOperator)
        {
            return Bill.filterDataTableToBillType(filterOperator);
        }

        public List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator)
        {
            return Delivery.filterDataTableToDeliveryType(filterOperator);
        }        

        #endregion

        #region [ Item Command ]

        // delete

        public int DeleteItem(int itemId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Items", DALHelper.getColumDictionary(new Item { ID = itemId })));
            return new Item { ID = itemId }.filterDataTableToItemType(ESearchOption.AND).Count;
        }

        public int DeleteProvider(int providerId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Providers", DALHelper.getColumDictionary(new Provider { ID = providerId })));
            return new Provider { ID = providerId }.filterDataTableToProviderType(ESearchOption.AND).Count;
        }

        public int DeleteProvider_item(int provider_itemId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Provider_items", DALHelper.getColumDictionary(new Provider_item { ID = provider_itemId })));
            return new Provider_item { ID = provider_itemId }.filterDataTableToProvider_itemType(ESearchOption.AND).Count;
        }

        public int DeleteItem_delivery(int item_deliveryId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Item_deliveries", DALHelper.getColumDictionary(new Item_delivery { ID = item_deliveryId })));
            return new Item_delivery { ID = item_deliveryId }.filterDataTableToItem_deliveryType(ESearchOption.AND).Count;
        }

        public int DeleteAuto_ref(int auto_refId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Auto_refs", DALHelper.getColumDictionary(new Auto_ref { ID = auto_refId })));
            return new Auto_ref { ID = auto_refId }.filterDataTableToAuto_refType(ESearchOption.AND).Count;
        }

        public int DeleteTax_item(int tax_itemId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Tax_items", DALHelper.getColumDictionary(new Tax_item { ID = tax_itemId })));
            return new Tax_item { ID = tax_itemId }.filterDataTableToTax_itemType(ESearchOption.AND).Count;
        }

        // update

        public int UpdateItem(List<Item> itemList)
        {
            int count = 0;
            foreach (var item in itemList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Items", DALHelper.getColumDictionary(item)));
                count += item.filterDataTableToItemType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateProvider(List<Provider> providerList)
        {
            int count = 0;
            foreach (var provider in providerList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Providers", DALHelper.getColumDictionary(provider)));
                count += provider.filterDataTableToProviderType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateProvider_item(List<Provider_item> provider_itemList)
        {
            int count = 0;
            foreach (var provider_item in provider_itemList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Provider_items", DALHelper.getColumDictionary(provider_item)));
                count += provider_item.filterDataTableToProvider_itemType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateItem_delivery(List<Item_delivery> item_deliveryList)
        {
            int count = 0;
            foreach (var item_delivery in item_deliveryList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Item_deliveries", DALHelper.getColumDictionary(item_delivery)));
                count += item_delivery.filterDataTableToItem_deliveryType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateAuto_ref(List<Auto_ref> auto_refList)
        {
            int count = 0;
            foreach (var auto_ref in auto_refList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Auto_refs", DALHelper.getColumDictionary(auto_ref)));
                count += auto_ref.filterDataTableToAuto_refType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateTax_item(List<Tax_item> tax_itemList)
        {
            int count = 0;
            foreach (var tax_item in tax_itemList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Tax_items", DALHelper.getColumDictionary(tax_item)));
                count += tax_item.filterDataTableToTax_itemType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadItem(Item item)
        {
            var dataFoundList = new Item { ID = item.ID }.filterDataTableToItemType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Items", DALHelper.getColumDictionary(item)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Items", DALHelper.getColumDictionary(item)));
            return new Item { ID = item.ID }.filterDataTableToItemType(ESearchOption.AND).Count;
        }

        public int LoadProvider(Provider provider)
        {
            var dataFoundList = new Provider { ID = provider.ID }.filterDataTableToProviderType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Providers", DALHelper.getColumDictionary(provider)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Providers", DALHelper.getColumDictionary(provider)));
            return new Provider { ID = provider.ID }.filterDataTableToProviderType(ESearchOption.AND).Count;
        }

        public int LoadProvider_item(Provider_item provider_item)
        {
            var dataFoundList = new Provider_item { ID = provider_item.ID }.filterDataTableToProvider_itemType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Provider_items", DALHelper.getColumDictionary(provider_item)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Provider_items", DALHelper.getColumDictionary(provider_item)));
            return new Provider_item { ID = provider_item.ID }.filterDataTableToProvider_itemType(ESearchOption.AND).Count;
        }

        public int LoadItem_delivery(Item_delivery item_delivery)
        {
            var dataFoundList = new Item_delivery { ID = item_delivery.ID }.filterDataTableToItem_deliveryType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Item_deliveries", DALHelper.getColumDictionary(item_delivery)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Item_deliveries", DALHelper.getColumDictionary(item_delivery)));
            return new Item_delivery { ID = item_delivery.ID }.filterDataTableToItem_deliveryType(ESearchOption.AND).Count;
        }

        public int LoadAuto_ref(Auto_ref auto_ref)
        {
            var dataFoundList = new Auto_ref { ID = auto_ref.ID }.filterDataTableToAuto_refType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Auto_refs", DALHelper.getColumDictionary(auto_ref)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Auto_refs", DALHelper.getColumDictionary(auto_ref)));
            return new Auto_ref { ID = auto_ref.ID }.filterDataTableToAuto_refType(ESearchOption.AND).Count;
        }

        public int LoadTax_item(Tax_item tax_item)
        {
            var dataFoundList = new Tax_item { ID = tax_item.ID }.filterDataTableToTax_itemType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Tax_items", DALHelper.getColumDictionary(tax_item)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Tax_items", DALHelper.getColumDictionary(tax_item)));
            return new Tax_item { ID = tax_item.ID }.filterDataTableToTax_itemType(ESearchOption.AND).Count;
        }

        // getting 

        public List<Item> GetItemData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Items", DALHelper.getColumDictionary(new Item()))).DataTableTypeToItem();
        }

        public List<Item> GetItemDataById(int id)
        {
            return new Item { ID = id }.filterDataTableToItemType(ESearchOption.AND);
        }

        public List<Provider> GetProviderData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Providers", DALHelper.getColumDictionary(new Provider()))).DataTableTypeToProvider();
        }

        public List<Provider> GetProviderDataById(int id)
        {
            return new Provider { ID = id }.filterDataTableToProviderType(ESearchOption.AND);
        }

        public List<Provider_item> GetProvider_itemData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Provider_items", DALHelper.getColumDictionary(new Provider_item()))).DataTableTypeToProvider_item();
        }

        public List<Provider_item> GetProvider_itemDataById(int id)
        {
            return new Provider_item { ID = id }.filterDataTableToProvider_itemType(ESearchOption.AND);
        }

        public List<Item_delivery> GetItem_deliveryData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Item_deliverys", DALHelper.getColumDictionary(new Item_delivery()))).DataTableTypeToItem_delivery();
        }

        public List<Item_delivery> GetItem_deliveryDataById(int id)
        {
            return new Item_delivery { ID = id }.filterDataTableToItem_deliveryType(ESearchOption.AND);
        }

        public List<Item_delivery> GetItem_deliveryDataByItemRefId(string itemRef)
        {
            return new Item_delivery { Item_ref = itemRef }.filterDataTableToItem_deliveryType(ESearchOption.AND);
        }

        public List<Item_delivery> GetItem_deliveryDataByDeliveryId(int deliveryId)
        {
            return new Item_delivery { DeliveryId = deliveryId }.filterDataTableToItem_deliveryType(ESearchOption.AND);
        }

        public List<Auto_ref> GetAuto_refData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Auto_refs", DALHelper.getColumDictionary(new Auto_ref()))).DataTableTypeToAuto_ref();
        }

        public List<Auto_ref> GetAuto_refDataById(int id)
        {
            return new Auto_ref { ID = id }.filterDataTableToAuto_refType(ESearchOption.AND);
        }

        public List<Tax_item> GetTax_itemData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Tax_items", DALHelper.getColumDictionary(new Tax_item()))).DataTableTypeToTax_item();
        }

        public List<Tax_item> GetTax_itemDataById(int id)
        {
            return new Tax_item { ID = id }.filterDataTableToTax_itemType(ESearchOption.AND);
        }


        // search

        public List<Item> searchItem(Item item, ESearchOption filterOperator)
        {
            return item.filterDataTableToItemType(filterOperator);
        }

        public List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator)
        {
            return Provider.filterDataTableToProviderType(filterOperator);
        }

        public List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator)
        {
            return Provider_item.filterDataTableToProvider_itemType(filterOperator);
        }

        public List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            return Item_delivery.filterDataTableToItem_deliveryType(filterOperator);
        }

        public List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            return Auto_ref.filterDataTableToAuto_refType(filterOperator);
        }

        public List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator)
        {
            return Tax_item.filterDataTableToTax_itemType(filterOperator);
        }

        #endregion

        #region [ Client Command ]

        // delete

        public int DeleteClient(int clientId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Clients", DALHelper.getColumDictionary(new Client { ID = clientId })));
            return new Client { ID = clientId }.filterDataTableToClientType(ESearchOption.AND).Count;
        }

        public int DeleteContact(int contactId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Contacts", DALHelper.getColumDictionary(new Contact { ID = contactId })));
            return new Contact { ID = contactId }.filterDataTableToContactType(ESearchOption.AND).Count;
        }

        public int DeleteAddress(int addressId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Addresses", DALHelper.getColumDictionary(new Address { ID = addressId })));
            return new Address { ID = addressId }.filterDataTableToAddressType(ESearchOption.AND).Count;
        }

        // update

        public int UpdateClient(List<Client> clientList)
        {
            int count = 0;
            foreach (var client in clientList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Clients", DALHelper.getColumDictionary(client)));
                count += client.filterDataTableToClientType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateContact(List<Contact> contactList)
        {
            int count = 0;
            foreach (var contact in contactList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Contacts", DALHelper.getColumDictionary(contact)));
                count += contact.filterDataTableToContactType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int UpdateAddress(List<Address> addressList)
        {
            int count = 0;
            foreach (var address in addressList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Addresses", DALHelper.getColumDictionary(address)));
                count += address.filterDataTableToAddressType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadClient(Client client)
        {
            var dataFoundList = new Client { ID = client.ID }.filterDataTableToClientType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Clients", DALHelper.getColumDictionary(client)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Clients", DALHelper.getColumDictionary(client)));
            return new Client { ID = client.ID }.filterDataTableToClientType(ESearchOption.AND).Count;
        }

        public int LoadContact(Contact contact)
        {
            var dataFoundList = new Contact { ID = contact.ID }.filterDataTableToContactType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Contacts", DALHelper.getColumDictionary(contact)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Contacts", DALHelper.getColumDictionary(contact)));
            return new Contact { ID = contact.ID }.filterDataTableToContactType(ESearchOption.AND).Count;
        }

        public int LoadAddress(Address address)
        {
            var dataFoundList = new Address { ID = address.ID }.filterDataTableToAddressType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Addresses", DALHelper.getColumDictionary(address)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Addresses", DALHelper.getColumDictionary(address)));
            return new Address { ID = address.ID }.filterDataTableToAddressType(ESearchOption.AND).Count;
        }

        // getting 

        public List<Client>  GetClientData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Clients", DALHelper.getColumDictionary(new Client()))).DataTableTypeToClient();
        }

        public List<Contact> GetContactData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Contacts", DALHelper.getColumDictionary(new Contact()))).DataTableTypeToContact();
        }

        public List<Address> GetAddressData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Addresses", DALHelper.getColumDictionary(new Address()))).DataTableTypeToAddress();
        }

        public List<Client> GetClientDataById(int id)
        {
            return new Client { ID = id }.filterDataTableToClientType(ESearchOption.AND);
        }

        public List<Contact> GetContactDataById(int id)
        {
            return new Contact { ID = id }.filterDataTableToContactType(ESearchOption.AND);
        }

        public List<Address> GetAddressDataById(int id)
        {
            return new Address { ID = id }.filterDataTableToAddressType(ESearchOption.AND);
        }

        // search

        public List<Client> searchClient(Client client, ESearchOption filterOperator)
        {
            return client.filterDataTableToClientType(filterOperator);
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            return Contact.filterDataTableToContactType(filterOperator);
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            return Address.filterDataTableToAddressType(filterOperator);
        }

        #endregion

        #region [ Agent Command ]

        // delete

        public int DeleteAgent(int agentId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Agents", DALHelper.getColumDictionary(new Agent { ID = agentId })));
            return new Agent { ID = agentId }.filterDataTableToAgentType(ESearchOption.AND).Count;
        }

        // update

        public int UpdateAgent(List<Agent> agentList)
        {
            int count = 0;
            foreach (var agent in agentList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Agents", DALHelper.getColumDictionary(agent)));
                count += agent.filterDataTableToAgentType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadAgent(Agent agent)
        {
            var dataFoundList = new Agent { ID = agent.ID }.filterDataTableToAgentType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Agents", DALHelper.getColumDictionary(agent)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Agents", DALHelper.getColumDictionary(agent)));
            return new Agent { ID = agent.ID }.filterDataTableToAgentType(ESearchOption.AND).Count;
        }

        // getting 

        public List<Agent> GetAgentData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Agents", DALHelper.getColumDictionary(new Agent()))).DataTableTypeToAgent();
        }

        public List<Agent> GetAgentDataById(int id)
        {
            return new Agent { ID = id }.filterDataTableToAgentType(ESearchOption.AND);
        }

        // search

        public List<Agent> searchAgent(Agent agent, ESearchOption filterOperator)
        {
            return agent.filterDataTableToAgentType(filterOperator);
        }

        #endregion

        #region [ Notification Command ]

        // delete

        public int DeleteNotification(int notificationId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Notifications", DALHelper.getColumDictionary(new Notification { ID = notificationId })));
            return new Notification { ID = notificationId }.filterDataTableToNotificationType(ESearchOption.AND).Count;
        }

        // update

        public int UpdateNotification(List<Notification> notificationList)
        {
            int count = 0;
            foreach (var notification in notificationList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Notifications", DALHelper.getColumDictionary(notification)));
                count += notification.filterDataTableToNotificationType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadNotification(Notification notification)
        {
            var dataFoundList = new Notification { ID = notification.ID }.filterDataTableToNotificationType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Notifications", DALHelper.getColumDictionary(notification)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Notifications", DALHelper.getColumDictionary(notification)));
            return new Notification { ID = notification.ID }.filterDataTableToNotificationType(ESearchOption.AND).Count;
        }

        // getting 

        public List<Notification> GetNotificationData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Notifications", DALHelper.getColumDictionary(new Notification()))).DataTableTypeToNotification();
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            return new Notification { ID = id }.filterDataTableToNotificationType(ESearchOption.AND);
        }

        // search

        public List<Notification> searchNotification(Notification notification, ESearchOption filterOperator)
        {
            return notification.filterDataTableToNotificationType(filterOperator);
        }

        #endregion

        #region [ Referential Command ]

        // delete

        public int DeleteInfo(int infoId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Infos", DALHelper.getColumDictionary(new Info { ID = infoId })));
            return new Info { ID = infoId }.filterDataTableToInfoType(ESearchOption.AND).Count;
        }

        // update

        public int UpdateInfo(List<Info> infoList)
        {
            int count = 0;
            foreach (var info in infoList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Infos", DALHelper.getColumDictionary(info)));
                count += info.filterDataTableToInfoType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadInfo(Info info)
        {
            var dataFoundList = new Info { ID = info.ID }.filterDataTableToInfoType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Infos", DALHelper.getColumDictionary(info)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Infos", DALHelper.getColumDictionary(info)));
            return new Info { ID = info.ID }.filterDataTableToInfoType(ESearchOption.AND).Count;
        }

        // getting 

        public List<Info> GetInfosData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Infos", DALHelper.getColumDictionary(new Info()))).DataTableTypeToInfos();
        }

        public List<Info> GetInfosDataById(int id)
        {
            return new Info { ID = id }.filterDataTableToInfoType(ESearchOption.AND);
        }   

        // search

        public List<Info> searchInfo(Info Infos, ESearchOption filterOperator)
        {
            return Infos.filterDataTableToInfoType(filterOperator);
        }

        #endregion

        #region [ Statistic Command ]

        // delete

        public int DeleteStatistic(int statisticId)
        {
            DALHelper.getDataTableFromSqlCEQuery(DALHelper.getDeleteSqlText("Statistics", DALHelper.getColumDictionary(new Statistic { ID = statisticId })));
            return new Statistic { ID = statisticId }.filterDataTableToStatisticType(ESearchOption.AND).Count;
        }

        // update

        public int UpdateStatistic(List<Statistic> statisticList)
        {
            int count = 0;
            foreach (var statistic in statisticList)
            {
                var dataTable = DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Statistics", DALHelper.getColumDictionary(statistic)));
                count += statistic.filterDataTableToStatisticType(ESearchOption.AND).Count;
            }
            return count;
        }

        public int LoadStatistic(Statistic statistic)
        {
            var dataFoundList = new Statistic { ID = statistic.ID }.filterDataTableToStatisticType(ESearchOption.AND);
            if (dataFoundList.Count != 0)
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getUpdateSqlText("Statistics", DALHelper.getColumDictionary(statistic)));
            else
                DALHelper.getDataTableFromSqlCEQuery(DALHelper.getInsertSqlText("Statistics", DALHelper.getColumDictionary(statistic)));
            return new Statistic { ID = statistic.ID }.filterDataTableToStatisticType(ESearchOption.AND).Count;
        }

        // getting 

        public List<Statistic> GetStatisticData()
        {
            return DALHelper.getDataTableFromSqlCEQuery(DALHelper.getAllDataSqlText("Statistics", DALHelper.getColumDictionary(new Statistic()))).DataTableTypeToStatistic();
        }

        public List<Statistic> GetStatisticDataById(int id)
        {
            return new Statistic { ID = id }.filterDataTableToStatisticType(ESearchOption.AND);
        }

        //search

        public List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator)
        {
            return statistic.filterDataTableToStatisticType(filterOperator);
        }

        #endregion

    }
}
