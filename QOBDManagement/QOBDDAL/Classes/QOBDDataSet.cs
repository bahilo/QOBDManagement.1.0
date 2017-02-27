using QOBDDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDDAL.App_Data;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDDAL.Helper.ChannelHelper;

namespace QOBDDAL.Classes
{
    public class QOBDDataSet : IQOBDSet
    {
        private QOBDSet _dataSet;
        private agentsTableAdapter _agentsTableAdapter;
        private clientsTableAdapter _clientsTableAdapter;
        private itemsTableAdapter _itemsTableAdapter;
        private billsTableAdapter _billsTableAdapter;
        private actionRecordsTableAdapter _actionRecordsTableAdapter;
        private addressesTableAdapter _addressesTableAdapter;
        private auto_refsTableAdapter _auto_refsTableAdapter;
        private contactsTableAdapter _contactsTableAdapter;
        private deliveriesTableAdapter _deliveriesTableAdapter;
        private infosTableAdapter _infosTableAdapter;
        private item_deliveriesTableAdapter _item_deliveriesTableAdapter;
        private notificationsTableAdapter _notificationsTableAdapter;
        private provider_itemsTableAdapter _provider_itemsTableAdapter;
        private providersTableAdapter _providersTableAdapter;
        private statisticsTableAdapter _statisticsTableAdapter;
        private order_itemsTableAdapter _order_itemsTableAdapter;
        private taxesTableAdapter _taxesTableAdapter;
        private ordersTableAdapter _ordersTableAdapter;
        private tax_ordersTableAdapter _tax_ordersTableAdapter;
        private tax_itemsTableAdapter _tax_itemsTableAdapter;

        public QOBDDataSet()
        {
            _dataSet = new QOBDSet();
            _agentsTableAdapter = new agentsTableAdapter();
            _clientsTableAdapter = new clientsTableAdapter();
            _itemsTableAdapter = new itemsTableAdapter();
            _billsTableAdapter = new billsTableAdapter();
            _actionRecordsTableAdapter = new actionRecordsTableAdapter();
            _addressesTableAdapter = new addressesTableAdapter();
            _auto_refsTableAdapter = new auto_refsTableAdapter();
            _contactsTableAdapter = new contactsTableAdapter();
            _deliveriesTableAdapter = new deliveriesTableAdapter();
            _infosTableAdapter = new infosTableAdapter();
            _item_deliveriesTableAdapter = new item_deliveriesTableAdapter();
            _notificationsTableAdapter = new notificationsTableAdapter();
            _provider_itemsTableAdapter = new provider_itemsTableAdapter();
            _providersTableAdapter = new providersTableAdapter();
            _statisticsTableAdapter = new statisticsTableAdapter();
            _order_itemsTableAdapter = new order_itemsTableAdapter();
            _taxesTableAdapter = new taxesTableAdapter();
            _ordersTableAdapter = new ordersTableAdapter();
            _tax_ordersTableAdapter = new tax_ordersTableAdapter();
            _tax_itemsTableAdapter = new tax_itemsTableAdapter();
        }
        

        //----------------------------[ Properties ]------------------

        #region[ Properties ]

        public actionRecordsTableAdapter actionRecordsTableAdapter
        {
            get
            {
                return _actionRecordsTableAdapter;
            }

            set
            {
                _actionRecordsTableAdapter = value;
            }
        }

        public addressesTableAdapter addressesTableAdapter
        {
            get
            {
                return _addressesTableAdapter;
            }

            set
            {
                _addressesTableAdapter = value;
            }
        }

        public agentsTableAdapter agentsTableAdapter
        {
            get
            {
                return _agentsTableAdapter;
            }

            set
            {
                _agentsTableAdapter = value;
            }
        }

        public auto_refsTableAdapter auto_refsTableAdapter
        {
            get
            {
                return _auto_refsTableAdapter;
            }

            set
            {
                _auto_refsTableAdapter = value;
            }
        }

        public billsTableAdapter billsTableAdapter
        {
            get
            {
                return _billsTableAdapter;
            }

            set
            {
                _billsTableAdapter = value;
            }
        }

        public clientsTableAdapter clientsTableAdapter
        {
            get
            {
                return _clientsTableAdapter;
            }

            set
            {
                _clientsTableAdapter = value;
            }
        }

        public contactsTableAdapter contactsTableAdapter
        {
            get
            {
                return _contactsTableAdapter;
            }

            set
            {
                _contactsTableAdapter = value;
            }
        }

        public deliveriesTableAdapter deliveriesTableAdapter
        {
            get
            {
                return _deliveriesTableAdapter;
            }

            set
            {
                _deliveriesTableAdapter = value;
            }
        }

        public infosTableAdapter infosTableAdapter
        {
            get
            {
                return _infosTableAdapter;
            }

            set
            {
                _infosTableAdapter = value;
            }
        }

        public itemsTableAdapter itemsTableAdapter
        {
            get
            {
                return _itemsTableAdapter;
            }

            set
            {
                _itemsTableAdapter = value;
            }
        }

        public item_deliveriesTableAdapter item_deliveriesTableAdapter
        {
            get
            {
                return _item_deliveriesTableAdapter;
            }

            set
            {
                _item_deliveriesTableAdapter = value;
            }
        }

        public notificationsTableAdapter notificationsTableAdapter
        {
            get
            {
                return _notificationsTableAdapter;
            }

            set
            {
                _notificationsTableAdapter = value;
            }
        }

        public ordersTableAdapter ordersTableAdapter
        {
            get
            {
                return _ordersTableAdapter;
            }

            set
            {
                _ordersTableAdapter = value;
            }
        }

        public order_itemsTableAdapter order_itemsTableAdapter
        {
            get
            {
                return _order_itemsTableAdapter;
            }

            set
            {
                _order_itemsTableAdapter = value;
            }
        }

        public providersTableAdapter providersTableAdapter
        {
            get
            {
                return _providersTableAdapter;
            }

            set
            {
                _providersTableAdapter = value;
            }
        }

        public provider_itemsTableAdapter provider_itemsTableAdapter
        {
            get
            {
                return _provider_itemsTableAdapter;
            }

            set
            {
                _provider_itemsTableAdapter = value;
            }
        }

        public statisticsTableAdapter statisticsTableAdapter
        {
            get
            {
                return _statisticsTableAdapter;
            }

            set
            {
                _statisticsTableAdapter = value;
            }
        }

        public taxesTableAdapter taxesTableAdapter
        {
            get
            {
                return _taxesTableAdapter;
            }

            set
            {
                _taxesTableAdapter = value;
            }
        }

        public tax_itemsTableAdapter tax_itemsTableAdapter
        {
            get
            {
                return _tax_itemsTableAdapter;
            }

            set
            {
                _tax_itemsTableAdapter = value;
            }
        }

        public tax_ordersTableAdapter tax_ordersTableAdapter
        {
            get
            {
                return _tax_ordersTableAdapter;
            }

            set
            {
                _tax_ordersTableAdapter = value;
            }
        }

        #endregion

        //----------------------------[ Actions ]------------------

        #region[ Order Commands ]

        // delete

        public int DeleteOrder(int orderId)
        {
            return _ordersTableAdapter.Delete1(orderId);
        }

        public int DeleteTax_order(int tax_orderId)
        {
            return tax_ordersTableAdapter.Delete1(tax_orderId);
        }

        public int DeleteOrder_item(int order_itemId)
        {
            return order_itemsTableAdapter.Delete1(order_itemId);
        }

        public int DeleteTax(int taxId)
        {
            return taxesTableAdapter.Delete1(taxId);
        }

        public int DeleteBill(int billId)
        {
            return billsTableAdapter.Delete1(billId);
        }

        public int DeleteDelivery(int deliveryId)
        {
            return deliveriesTableAdapter.Delete1(deliveryId);
        }


        // update

        public int UpdateOrder(QOBDSet.commandsDataTable orderDataTable)
        {
            return _ordersTableAdapter.Update(orderDataTable);
        }

        public int UpdateTax_order(QOBDSet.tax_commandsDataTable tax_orderDataTable)
        {
            return tax_ordersTableAdapter.Update(tax_orderDataTable);
        }

        public int UpdateOrder_item(QOBDSet.command_itemsDataTable order_itemDataTable)
        {
            return order_itemsTableAdapter.Update(order_itemDataTable);
        }

        public int UpdateTax(QOBDSet.taxesDataTable taxDataTable)
        {
            return taxesTableAdapter.Update(taxDataTable);
        }

        public int UpdateBill(QOBDSet.billsDataTable billDataTable)
        {
            return billsTableAdapter.Update(billDataTable);
        }

        public int UpdateDelivery(QOBDSet.deliveriesDataTable deliveryDataTable)
        {
            return deliveriesTableAdapter.Update(deliveryDataTable);
        }

        public int UpdateDelivery(Delivery delivery)
        {
            return deliveriesTableAdapter.Update1(
                                                delivery.OrderId,
                                                delivery.BillId,
                                                delivery.Package,
                                                delivery.Date,
                                                delivery.Status,
                                                delivery.ID);
        }

        public int LoadOrder(Order order)
        {
            return ordersTableAdapter.load_data_order(
                                                order.AgentId,
                                                order.ClientId,
                                                order.Comment1,
                                                order.Comment2,
                                                order.Comment3,
                                                order.BillAddress,
                                                order.DeliveryAddress,
                                                order.Status,
                                                order.Date,
                                                order.Tax,
                                                order.ID); ;
        }

        public int LoadTax_order(Tax_order tax_order)
        {
            return tax_ordersTableAdapter.load_data_tax_order(
                                                tax_order.OrderId,
                                                tax_order.TaxId,
                                                tax_order.Date_insert,
                                                tax_order.Tax_value.ToString(),
                                                tax_order.Target,
                                                tax_order.ID);
        }

        public int LoadOrder_item(Order_item order_item)
        {
            return order_itemsTableAdapter.load_order_item(
                                                order_item.OrderId,
                                                order_item.ItemId,
                                                order_item.Item_ref,
                                                order_item.Quantity,
                                                order_item.Quantity_delivery,
                                                order_item.Quantity_current,
                                                order_item.Comment_Purchase_Price,
                                                order_item.Price,
                                                order_item.Price_purchase,
                                                order_item.Order,
                                                order_item.ID);
        }

        public int LoadTax(Tax tax)
        {
            return taxesTableAdapter.load_data_tax(
                                            tax.Type,
                                            tax.Date_insert,
                                            tax.Value.ToString(),
                                            tax.Comment,
                                            tax.Tax_current,
                                            tax.ID);
        }

        public int LoadBill(Bill bill)
        {
            return billsTableAdapter.load_data_bill(
                                            bill.ClientId,
                                            bill.OrderId,
                                            bill.PayMod,
                                            bill.Pay,
                                            bill.PayReceived,
                                            bill.Comment1,
                                            bill.Comment2,
                                            bill.Date,
                                            bill.DateLimit,
                                            bill.PayDate,
                                            bill.ID);
        }

        public int LoadDelivery(Delivery delivery)
        {
            return deliveriesTableAdapter.load_data_delivery(
                                            delivery.OrderId,
                                            delivery.BillId,
                                            delivery.Package,
                                            delivery.Date,
                                            delivery.Status,
                                            delivery.ID);
        }


        // getting

        public List<Order> GetOrderData()
        {
            return _ordersTableAdapter.GetData().DataTableTypeToOrder();
        }

        public List<Order> GetOrderDataById(int id)
        {
            return _ordersTableAdapter.get_order_by_id(id).DataTableTypeToOrder();
        }

        public List<Tax_order> GetTax_orderData()
        {
            return tax_ordersTableAdapter.GetData().DataTableTypeToTax_order();
        }

        public List<Tax_order> GetTax_orderByOrderId(int orderId)
        {
            return tax_ordersTableAdapter.get_tax_order_by_order_id(orderId).DataTableTypeToTax_order();
        }

        public List<Tax_order> GetTax_orderDataById(int id)
        {
            return tax_ordersTableAdapter.get_tax_order_by_id(id).DataTableTypeToTax_order();
        }

        public List<Order_item> GetOrder_itemData()
        {
            return order_itemsTableAdapter.GetData().DataTableTypeToOrder_item();
        }

        public List<Order_item> GetOrder_itemDataById(int id)
        {
            return order_itemsTableAdapter.get_order_item_by_id(id).DataTableTypeToOrder_item();
        }

        public List<Order_item> GetOrder_itemDataByOrderId(int orderId)
        {
            return order_itemsTableAdapter.get_order_item_by_order_id(orderId).DataTableTypeToOrder_item();
        }

        public List<Tax> GetTaxData()
        {
            return taxesTableAdapter.GetData().DataTableTypeToTax();
        }

        public List<Tax> GetTaxDataById(int id)
        {
            return taxesTableAdapter.get_tax_by_id(id).DataTableTypeToTax();
        }

        public List<Bill> GetBillData()
        {
            return billsTableAdapter.GetData().DataTableTypeToBill();
        }

        public List<Bill> GetBillDataById(int id)
        {
            return billsTableAdapter.get_bill_by_id(id).DataTableTypeToBill();
        }

        public List<Bill> GetBillDataByOrderId(int orderId)
        {
            return billsTableAdapter.get_bill_by_order_id(orderId).DataTableTypeToBill();
        }

        public List<Delivery> GetDeliveryData()
        {
            return deliveriesTableAdapter.GetData().DataTableTypeToDelivery();
        }

        public List<Delivery> GetDeliveryDataById(int id)
        {
            return deliveriesTableAdapter.get_delivery_by_id(id).DataTableTypeToDelivery();
        }

        public List<Delivery> GetDeliveryDataByOrderId(int orderId)
        {
            return _deliveriesTableAdapter.get_delivery_by_order_id(orderId).DataTableTypeToDelivery();
        }

        public void FillOrderDataTableById(QOBDSet.commandsDataTable irderDataTable, int id)
        {
            throw new NotImplementedException();
        }

        public void FillTax_orderDataTableById(QOBDSet.tax_commandsDataTable taxOrderDataTable, int id)
        {
            tax_ordersTableAdapter.FillById(taxOrderDataTable, id);
        }

        public void FillOrder_itemDataTableById(QOBDSet.command_itemsDataTable orderItemDataTable, int id)
        {
            order_itemsTableAdapter.FillById(orderItemDataTable, id);
        }

        public void FillTaxDataTableById(QOBDSet.taxesDataTable taxDataTable, int id)
        {
            taxesTableAdapter.FillById(taxDataTable, id);
        }

        public void FillBillDataTableById(QOBDSet.billsDataTable billDataTable, int id)
        {
            billsTableAdapter.FillById(billDataTable, id);
        }

        public void FillDeliveryDataTableById(QOBDSet.deliveriesDataTable deliveryDataTable, int id)
        {
            throw new NotImplementedException();
        }

        // search

        public List<Order> searchOrder(Order order, ESearchOption filterOperator)
        {
            return order.orderTypeToFilterDataTable(filterOperator);
        }

        public List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator)
        {
            return Tax_order.Tax_orderTypeToFilterDataTable(filterOperator);
        }

        public List<Order_item> searchOrder_item(Order_item order_item, ESearchOption filterOperator)
        {
            return order_item.order_itemTypeToFilterDataTable(filterOperator);
        }

        public List<Tax> searchTax(Tax Tax, ESearchOption filterOperator)
        {
            return Tax.TaxTypeToFilterDataTable(filterOperator);
        }

        public List<Bill> searchBill(Bill Bill, ESearchOption filterOperator)
        {
            return Bill.BillTypeToFilterDataTable(filterOperator);
        }

        public List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator)
        {
            return Delivery.DeliveryTypeToFilterDataTable(filterOperator);
        }        

        #endregion

        #region [ Item Command ]

        // delete

        public int DeleteItem(int itemId)
        {
            return _itemsTableAdapter.Delete1(itemId);
        }

        public int DeleteProvider(int providerId)
        {
            return _providersTableAdapter.Delete1(providerId);
        }

        public int DeleteProvider_item(int provider_itemId)
        {
            return _provider_itemsTableAdapter.Delete1(provider_itemId);
        }

        public int DeleteItem_delivery(int item_deliveryId)
        {
            return _item_deliveriesTableAdapter.Delete1(item_deliveryId);
        }

        public int DeleteAuto_ref(int auto_refId)
        {
            return _auto_refsTableAdapter.Delete1(auto_refId);
        }

        public int DeleteTax_item(int tax_itemId)
        {
            return _tax_itemsTableAdapter.Delete1(tax_itemId);
        }

        // update

        public int UpdateItem(QOBDSet.itemsDataTable itemDataTable)
        {
            return _itemsTableAdapter.Update(itemDataTable);
        }

        public int UpdateProvider(QOBDSet.providersDataTable providerDataTable)
        {
            return _providersTableAdapter.Update(providerDataTable);
        }

        public int UpdateProvider_item(QOBDSet.provider_itemsDataTable provider_itemDataTable)
        {
            return _provider_itemsTableAdapter.Update(provider_itemDataTable);
        }

        public int UpdateItem_delivery(QOBDSet.item_deliveriesDataTable item_deliveryDataTable)
        {
            return item_deliveriesTableAdapter.Update(item_deliveryDataTable);
        }

        public int UpdateAuto_ref(QOBDSet.auto_refsDataTable auto_refDataTable)
        {
            return _auto_refsTableAdapter.Update(auto_refDataTable);
        }

        public int UpdateTax_item(QOBDSet.tax_itemsDataTable tax_itemDataTable)
        {
            return _tax_itemsTableAdapter.Update(tax_itemDataTable);
        }

        public int LoadItem(Item item)
        {
            return itemsTableAdapter.load_data_item(
                                                item.Ref,
                                                item.Name,
                                                item.Type,
                                                item.Type_sub,
                                                item.Price_purchase,
                                                item.Price_sell,
                                                item.Source,
                                                item.Comment,
                                                item.Erasable,
                                                item.ID);
        }

        public int LoadProvider(Provider provider)
        {
            return providersTableAdapter.load_data_provider(
                                                provider.Name,
                                                provider.Source,
                                                provider.ID);
        }

        public int LoadProvider_item(Provider_item provider_item)
        {
            return provider_itemsTableAdapter.load_data_provider_item(
                                                provider_item.Provider_name,
                                                provider_item.Item_ref,
                                                provider_item.ID);
        }

        public int LoadItem_delivery(Item_delivery item_delivery)
        {
            return item_deliveriesTableAdapter.load_data_item_delivery(
                                        item_delivery.DeliveryId,
                                        item_delivery.Item_ref,
                                        item_delivery.Quantity_delivery,
                                        item_delivery.ID);
        }

        public int LoadAuto_ref(Auto_ref auto_ref)
        {
            return auto_refsTableAdapter.load_auto_ref(
                                                        auto_ref.RefId,
                                                        auto_ref.ID);
        }

        public int LoadTax_item(Tax_item tax_item)
        {
            return tax_itemsTableAdapter.load_data_tax_item(
                                                            tax_item.TaxId,
                                                            tax_item.Item_ref,
                                                            tax_item.Tax_value,
                                                            tax_item.Tax_type,
                                                            tax_item.ID);
        }

        // getting 

        public List<Item> GetItemData()
        {
            return _itemsTableAdapter.GetData().DataTableTypeToItem();
        }

        public List<Item> GetItemDataById(int id)
        {
            return _itemsTableAdapter.get_item_by_id(id).DataTableTypeToItem();
        }

        public List<Provider> GetProviderData()
        {
            return _providersTableAdapter.GetData().DataTableTypeToProvider();
        }

        public List<Provider> GetProviderDataById(int id)
        {
            return _providersTableAdapter.get_provider_by_id(id).DataTableTypeToProvider();
        }

        public List<Provider_item> GetProvider_itemData()
        {
            return _provider_itemsTableAdapter.GetData().DataTableTypeToProvider_item();
        }

        public List<Provider_item> GetProvider_itemDataById(int id)
        {
            return _provider_itemsTableAdapter.get_provider_item_by_id(id).DataTableTypeToProvider_item();
        }

        public List<Item_delivery> GetItem_deliveryData()
        {
            return _item_deliveriesTableAdapter.GetData().DataTableTypeToItem_delivery();
        }

        public List<Item_delivery> GetItem_deliveryDataById(int id)
        {
            return _item_deliveriesTableAdapter.get_item_delivery_by_id(id).DataTableTypeToItem_delivery();
        }

        public List<Item_delivery> GetItem_deliveryDataByItemRefId(string itemRef)
        {
            return _item_deliveriesTableAdapter.get_item_delivery_by_item_ref(itemRef).DataTableTypeToItem_delivery();
        }

        public List<Item_delivery> GetItem_deliveryDataByDeliveryId(int deliveryId)
        {
            return _item_deliveriesTableAdapter.get_item_delivery_by_delivery_id(deliveryId).DataTableTypeToItem_delivery();
        }

        public List<Auto_ref> GetAuto_refData()
        {
            return _auto_refsTableAdapter.GetData().DataTableTypeToAuto_ref();
        }

        public List<Auto_ref> GetAuto_refDataById(int id)
        {
            return _auto_refsTableAdapter.get_auto_ref_by_id(id).DataTableTypeToAuto_ref();
        }

        public List<Tax_item> GetTax_itemData()
        {
            return _tax_itemsTableAdapter.GetData().DataTableTypeToTax_item();
        }

        public List<Tax_item> GetTax_itemDataById(int id)
        {
            return _tax_itemsTableAdapter.get_tax_item_by_id(id).DataTableTypeToTax_item();
        }

        public void FillItemDataTableById(QOBDSet.itemsDataTable itemDataTable, int id)
        {
            itemsTableAdapter.FillById(itemDataTable, id);
        }

        public void FillProviderDataTableById(QOBDSet.providersDataTable providerDataTable, int id)
        {
            providersTableAdapter.FillById(providerDataTable, id);
        }

        public void FillProvider_itemDataTableById(QOBDSet.provider_itemsDataTable provider_itemDataTable, int id)
        {
            provider_itemsTableAdapter.FillById(provider_itemDataTable, id);
        }

        public void FillItem_deliveryDataTableById(QOBDSet.item_deliveriesDataTable item_deliveryDataTable, int id)
        {
            item_deliveriesTableAdapter.FillById(item_deliveryDataTable, id);
        }

        public void FillAuto_refDataTableById(QOBDSet.auto_refsDataTable auto_refDataTable, int id)
        {
            auto_refsTableAdapter.FillById(auto_refDataTable, id);
        }
        
        public void FillTax_itemDataTableById(QOBDSet.tax_itemsDataTable tax_itemDataTable, int id)
        {
            tax_itemsTableAdapter.FillById(tax_itemDataTable, id);
        }


        // search

        public List<Item> searchItem(Item item, ESearchOption filterOperator)
        {
            return item.ItemTypeToFilterDataTable(filterOperator);
        }

        public List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator)
        {
            return Provider.ProviderTypeToFilterDataTable(filterOperator);
        }

        public List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator)
        {
            return Provider_item.Provider_itemTypeToFilterDataTable(filterOperator);
        }

        public List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            return Item_delivery.Item_deliveryTypeToFilterDataTable(filterOperator);
        }

        public List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            return Auto_ref.Auto_refTypeToFilterDataTable(filterOperator);
        }

        public List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator)
        {
            return Tax_item.Tax_itemTypeToFilterDataTable(filterOperator);
        }

        #endregion

        #region [ Client Command ]

        // delete

        public int DeleteClient(int clientId)
        {
            return _clientsTableAdapter.Delete1(clientId);
        }

        public int DeleteContact(int contactId)
        {
            return _contactsTableAdapter.Delete1(contactId);
        }

        public int DeleteAddress(int addressId)
        {
            return _addressesTableAdapter.Delete1(addressId);
        }

        // update

        public int UpdateClient(QOBDSet.clientsDataTable clientDataTable)
        {
            return _clientsTableAdapter.Update(clientDataTable);
        }

        public int UpdateContact(QOBDSet.contactsDataTable contactDataTable)
        {
            return _contactsTableAdapter.Update(contactDataTable);
        }

        public int UpdateAddress(QOBDSet.addressesDataTable addressDataTable)
        {
            return _addressesTableAdapter.Update(addressDataTable);
        }

        public int LoadClient(Client client)
        {
            return clientsTableAdapter.load_data_client(
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
        }

        public int LoadContact(Contact contact)
        {
            return contactsTableAdapter.load_data_contact(
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
        }

        public int LoadAddress(Address address)
        {
            return addressesTableAdapter.load_data_address(
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
        }

        // getting 

        public List<Client>  GetClientData()
        {
            return _clientsTableAdapter.GetData().DataTableTypeToClient();
        }

        public List<Contact> GetContactData()
        {
            return _contactsTableAdapter.GetData().DataTableTypeToContact();
        }

        public List<Address> GetAddressData()
        {
            return _addressesTableAdapter.GetData().DataTableTypeToAddress();
        }

        public List<Client> GetClientDataById(int id)
        {
            return _clientsTableAdapter.get_client_by_id(id).DataTableTypeToClient();
        }

        public List<Contact> GetContactDataById(int id)
        {
            return _contactsTableAdapter.get_contact_by_id(id).DataTableTypeToContact();
        }

        public List<Address> GetAddressDataById(int id)
        {
            return _addressesTableAdapter.get_address_by_id(id).DataTableTypeToAddress();
        }

        public void FillClientDataTableById(QOBDSet.clientsDataTable clientDataTable, int id)
        {
            clientsTableAdapter.FillById(clientDataTable, id);
        }

        public void FillContactDataTableById(QOBDSet.contactsDataTable contactDataTable, int id)
        {
            contactsTableAdapter.FillById(contactDataTable, id);
        }

        public void FilladdressDataTableById(QOBDSet.addressesDataTable addressDataTable, int id)
        {
            addressesTableAdapter.FillById(addressDataTable, id);
        }


        // search

        public List<Client> searchClient(Client client, ESearchOption filterOperator)
        {
            return client.ClientTypeToFilterDataTable(filterOperator);
        }

        public List<Contact> searchContact(Contact Contact, ESearchOption filterOperator)
        {
            return Contact.ContactTypeToFilterDataTable(filterOperator);
        }

        public List<Address> searchAddress(Address Address, ESearchOption filterOperator)
        {
            return Address.AddressTypeToFilterDataTable(filterOperator);
        }

        #endregion

        #region [ Agent Command ]

        // delete

        public int DeleteAgent(int agentId)
        {
            return _ordersTableAdapter.Delete1(agentId);
        }

        // update

        public int UpdateAgent(QOBDSet.agentsDataTable agentDataTable)
        {
            return _agentsTableAdapter.Update(agentDataTable);
        }

        public int LoadAgent(Agent agent)
        {
            return agentsTableAdapter.load_data_agent(
                                            agent.LastName,
                                            agent.FirstName,
                                            agent.Phone,
                                            agent.Fax,
                                            agent.Email,
                                            agent.Login,
                                            agent.HashedPassword,
                                            agent.Admin,
                                            agent.Status,
                                            agent.ListSize,
                                            agent.ID);
        }

        // getting 

        public List<Agent> GetAgentData()
        {
            return _agentsTableAdapter.GetData().DataTableTypeToAgent();
        }

        public List<Agent> GetAgentDataById(int id)
        {
            return _agentsTableAdapter.get_agent_by_id(id).DataTableTypeToAgent();
        }

        public void FillAgentDataTableById(QOBDSet.agentsDataTable agentDataTable, int id)
        {
            agentsTableAdapter.FillById(agentDataTable, id);
        }

        // search

        public List<Agent> searchAgent(Agent agent, ESearchOption filterOperator)
        {
            return agent.AgentTypeToFilterDataTable(filterOperator);
        }

        #endregion

        #region [ Notification Command ]

        // delete

        public int DeleteNotification(int notificationId)
        {
            return _notificationsTableAdapter.Delete1(notificationId);
        }

        // update

        public int UpdateNotification(QOBDSet.notificationsDataTable notificationDataTable)
        {
            return _notificationsTableAdapter.Update(notificationDataTable);
        }

        public int LoadNotification(Notification notification)
        {
            return notificationsTableAdapter.load_data_notification(
                                            notification.Reminder1,
                                            notification.Reminder2,
                                            notification.BillId,
                                            notification.Date,
                                            notification.ID);
        }

        // getting 

        public List<Notification> GetNotificationData()
        {
            return _notificationsTableAdapter.GetData().DataTableTypeToNotification();
        }

        public List<Notification> GetNotificationDataById(int id)
        {
            return _notificationsTableAdapter.get_notification_by_id(id).DataTableTypeToNotification();
        }

        public void FillNotificationDataTableById(QOBDSet.notificationsDataTable notificationDataTable, int id)
        {
            _notificationsTableAdapter.FillById(notificationDataTable,id);
        }

        // search

        public List<Notification> searchNotification(Notification notification, ESearchOption filterOperator)
        {
            return notification.NotificationTypeToFilterDataTable(filterOperator);
        }

        #endregion

        #region [ Referential Command ]

        // delete

        public int DeleteInfo(int infoId)
        {
            return _infosTableAdapter.Delete1(infoId);
        }

        // update

        public int UpdateInfo(QOBDSet.infosDataTable infoDataTable)
        {
            return _infosTableAdapter.Update(infoDataTable);
        }

        public int LoadInfo(Info info)
        {
            return _infosTableAdapter.load_data_infos(
                                                        info.Name,
                                                        info.Value,
                                                        info.ID);
        }

        // getting 

        public List<Info> GetInfosData()
        {
            return _infosTableAdapter.GetData().DataTableTypeToInfos();
        }

        public List<Info> GetInfosDataById(int id)
        {
            return _infosTableAdapter.get_infos_by_id(id).DataTableTypeToInfos();
        }   
        
        public void FillInfoDataTableById(QOBDSet.infosDataTable infoDataTable, int id)
        {
            _infosTableAdapter.FillById(infoDataTable, id);
        }

        // search

        public List<Info> searchInfo(Info Infos, ESearchOption filterOperator)
        {
            return Infos.FilterDataTableToInfoType(filterOperator);
        }

        #endregion

        #region [ Statistic Command ]

        // delete

        public int DeleteStatistic(int statisticId)
        {
            return _statisticsTableAdapter.Delete1(statisticId);
        }

        // update

        public int UpdateStatistic(QOBDSet.statisticsDataTable statisticDataTable)
        {
            return _statisticsTableAdapter.Update(statisticDataTable);
        }

        public int LoadStatistic(Statistic statistic)
        {
            return statisticsTableAdapter
                                        .load_data_statistic(
                                            statistic.InvoiceDate,
                                            statistic.InvoiceId,
                                            statistic.Company,
                                            statistic.Price_purchase_total,
                                            statistic.Total,
                                            statistic.Total_tax_included,
                                            statistic.Income_percent,
                                            statistic.Income,
                                            statistic.Pay_received,
                                            statistic.Date_limit,
                                            statistic.Pay_date,
                                            statistic.Tax_value,
                                            statistic.ID);
        }

        // getting 

        public List<Statistic> GetStatisticData()
        {
            return statisticsTableAdapter.GetData().DataTableTypeToStatistic(); ;
        }

        public void FillStatisticDataTableById(QOBDSet.statisticsDataTable statisticDataTable, int id)
        {
            statisticsTableAdapter.FillById(statisticDataTable, id);
        }

        public List<Statistic> GetStatisticDataById(int id)
        {
            return _statisticsTableAdapter.get_statistic_by_id(id).DataTableTypeToStatistic();
        }

        //search

        public List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator)
        {
            return statistic.FilterDataTableToStatisticType(filterOperator);
        }

        #endregion



        public void Dispose()
        {
            _agentsTableAdapter.Dispose();
            _clientsTableAdapter.Dispose();
            _itemsTableAdapter.Dispose();
            _billsTableAdapter.Dispose();
            _actionRecordsTableAdapter.Dispose();
            _addressesTableAdapter.Dispose();
            _auto_refsTableAdapter.Dispose();
            _contactsTableAdapter.Dispose();
            _deliveriesTableAdapter.Dispose();
            _infosTableAdapter.Dispose();
            _item_deliveriesTableAdapter.Dispose();
            _notificationsTableAdapter.Dispose();
            _provider_itemsTableAdapter.Dispose();
            _providersTableAdapter.Dispose();
            _statisticsTableAdapter.Dispose();
            _order_itemsTableAdapter.Dispose();
            _taxesTableAdapter.Dispose();
            _ordersTableAdapter.Dispose();
            _tax_ordersTableAdapter.Dispose();
            _tax_itemsTableAdapter.Dispose();
        }
    }
}
