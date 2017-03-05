using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDDAL.App_Data;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDDAL.Interfaces
{
    public interface IQOBDSet
    {
        
        //----------------------------[ Actions ]------------------

        #region[ order commands ]

        // delete

        int DeleteOrder(int orderId);
        int DeleteTax_order(int tax_orderId);
        int DeleteOrder_item(int order_itemId);
        int DeleteTax(int taxId);
        int DeleteBill(int billId);
        int DeleteDelivery(int deliveryId);

        // update

        int UpdateOrder(QOBDSet.commandsDataTable orderDataTable);
        int LoadOrder(Order order);
        int UpdateTax_order(QOBDSet.tax_commandsDataTable tax_orderDataTable);
        int LoadTax_order(Tax_order tax_order);
        int UpdateOrder_item(QOBDSet.command_itemsDataTable order_itemDataTable);
        int LoadOrder_item(Order_item order_item);
        int UpdateTax(QOBDSet.taxesDataTable taxDataTable);
        int LoadTax(Tax tax);
        int UpdateBill(QOBDSet.billsDataTable billDataTable);
        int LoadBill(Bill bill);
        int UpdateDelivery(QOBDSet.deliveriesDataTable deliveryDataTable);
        int UpdateDelivery(Delivery delivery);
        int LoadDelivery(Delivery delivery);

        // getting

        List<Order> GetOrderData();
        List<Order> GetOrderDataById(int id);
        void FillOrderDataTableById(QOBDSet.commandsDataTable irderDataTable, int id);
        List<Tax_order> GetTax_orderData();
        List<Tax_order> GetTax_orderByOrderId(int orderId);
        List<Tax_order> GetTax_orderDataById(int id);
        void FillTax_orderDataTableById(QOBDSet.tax_commandsDataTable taxOrderDataTable, int id);
        List<Order_item> GetOrder_itemData();
        List<Order_item> GetOrder_itemDataById(int id);
        void FillOrder_itemDataTableById(QOBDSet.command_itemsDataTable orderItemDataTable, int id);
        List<Order_item> GetOrder_itemDataByOrderId(int orderId);
        List<Tax> GetTaxData();
        List<Tax> GetTaxDataById(int id);
        void FillTaxDataTableById(QOBDSet.taxesDataTable taxDataTable, int id);
        List<Bill> GetBillData();
        List<Bill> GetBillDataById(int id);
        void FillBillDataTableById(QOBDSet.billsDataTable billDataTable, int id);
        List<Bill> GetBillDataByOrderId(int orderId);
        List<Delivery> GetDeliveryData();
        List<Delivery> GetDeliveryDataById(int id);
        void FillDeliveryDataTableById(QOBDSet.deliveriesDataTable deliveryDataTable, int id);
        List<Delivery> GetDeliveryDataByOrderId(int orderId);

        // search

        List<Order>  searchOrder(Order order, ESearchOption filterOperator);
        List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator);
        List<Order_item> searchOrder_item(Order_item order_item, ESearchOption filterOperator);
        List<Tax> searchTax(Tax Tax, ESearchOption filterOperator);
        List<Bill> searchBill(Bill Bill, ESearchOption filterOperator);
        List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator);

        #endregion

        #region [ Item Command ]

        // delete

        int DeleteItem(int itemId);
        int DeleteProvider(int providerId);
        int DeleteProvider_item(int provider_itemId);
        int DeleteItem_delivery(int item_deliveryId);
        int DeleteAuto_ref(int auto_refId);
        int DeleteTax_item(int tax_itemId);

        // update

        int UpdateItem(QOBDSet.itemsDataTable itemDataTable);
        int LoadItem(Item item);
        int UpdateProvider(QOBDSet.providersDataTable providerDataTable);
        int LoadProvider(Provider provider);
        int UpdateProvider_item(QOBDSet.provider_itemsDataTable provider_itemDataTable);
        int LoadProvider_item(Provider_item provider_item);
        int UpdateItem_delivery(QOBDSet.item_deliveriesDataTable item_deliveryDataTable);
        int LoadItem_delivery(Item_delivery item_delivery);
        int UpdateAuto_ref(QOBDSet.auto_refsDataTable auto_refDataTable);
        int LoadAuto_ref(Auto_ref auto_ref);
        int UpdateTax_item(QOBDSet.tax_itemsDataTable tax_itemDataTable);
        int LoadTax_item(Tax_item tax_item);

        // getting 

        List<Item> GetItemData();
        List<Item> GetItemDataById(int id);
        void FillItemDataTableById(QOBDSet.itemsDataTable itemDataTable, int id);
        List<Provider> GetProviderData();
        List<Provider> GetProviderDataById(int id);
        void FillProviderDataTableById(QOBDSet.providersDataTable providerDataTable, int id);
        List<Provider_item> GetProvider_itemData();
        List<Provider_item> GetProvider_itemDataById(int id);
        void FillProvider_itemDataTableById(QOBDSet.provider_itemsDataTable provider_itemDataTable, int id);
        List<Item_delivery> GetItem_deliveryData();
        List<Item_delivery> GetItem_deliveryDataById(int id);
        void FillItem_deliveryDataTableById(QOBDSet.item_deliveriesDataTable item_deliveryDataTable, int id);
        List<Item_delivery> GetItem_deliveryDataByItemRefId(string itemRef);
        List<Item_delivery> GetItem_deliveryDataByDeliveryId(int deliveryId);
        List<Auto_ref> GetAuto_refData();
        List<Auto_ref> GetAuto_refDataById(int id);
        void FillAuto_refDataTableById(QOBDSet.auto_refsDataTable auto_refDataTable, int id);
        List<Tax_item> GetTax_itemData();
        List<Tax_item> GetTax_itemDataById(int id);
        void FillTax_itemDataTableById(QOBDSet.tax_itemsDataTable tax_itemDataTable, int id);

        // search 

        List<Item> searchItem(Item item, ESearchOption filterOperator);
        List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator);
        List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator);
        List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator);
        List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator);
        List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator);

        #endregion

        #region [ Client Command ]

        // delete

        int DeleteClient(int clientId);
        int DeleteContact(int contactId);
        int DeleteAddress(int addressId);

        // update

        int UpdateClient(QOBDSet.clientsDataTable clientDataTable);
        int LoadClient(Client client);
        int UpdateContact(QOBDSet.contactsDataTable contactDataTable);
        int LoadContact(Contact contact);
        int UpdateAddress(QOBDSet.addressesDataTable addressDataTable);
        int LoadAddress(Address address);

        // getting 

        List<Client> GetClientData();
        List<Contact> GetContactData();
        List<Address> GetAddressData();
        List<Client> GetClientDataById(int id);
        void FillClientDataTableById(QOBDSet.clientsDataTable clientDataTable, int id);
        List<Contact> GetContactDataById(int id);
        void FillContactDataTableById(QOBDSet.contactsDataTable contactDataTable, int id);
        List<Address> GetAddressDataById(int id);
        void FilladdressDataTableById(QOBDSet.addressesDataTable addressDataTable, int id);

        // search

        List<Client> searchClient(Client client, ESearchOption filterOperator);
        List<Contact> searchContact(Contact Contact, ESearchOption filterOperator);
        List<Address> searchAddress(Address Address, ESearchOption filterOperator);

        #endregion

        #region [ Agent Command ]

        // delete

        int DeleteAgent(int agentId);

        // update

        int UpdateAgent(QOBDSet.agentsDataTable agentDataTable);
        int LoadAgent(Agent agent);

        // getting 

        List<Agent> GetAgentData();
        List<Agent> GetAgentDataById(int id);
        void FillAgentDataTableById(QOBDSet.agentsDataTable agentDataTable, int id);

        // search 

        List<Agent> searchAgent(Agent agent, ESearchOption filterOperator);

        #endregion

        #region [ Notification Command ]

        // delete

        int DeleteNotification(int notificationId);

        // update

        int UpdateNotification(QOBDSet.notificationsDataTable notificationDataTable);
        int LoadNotification(Notification notification);

        // getting 

        List<Notification> GetNotificationData();
        List<Notification> GetNotificationDataById(int id);
        void FillNotificationDataTableById(QOBDSet.notificationsDataTable notificationDataTable, int id);

        // search

        List<Notification> searchNotification(Notification notification, ESearchOption filterOperator);

        #endregion

        #region [ Referential Command ]

        // delete

        int DeleteInfo(int infoId);

        // update

        int UpdateInfo(QOBDSet.infosDataTable infoDataTable);
        int LoadInfo(Info info);
        // getting 

        List<Info> GetInfosData();
        List<Info> GetInfosDataById(int id);
        void FillInfoDataTableById(QOBDSet.infosDataTable infoDataTable, int id);

        // search

        List<Info> searchInfo(Info Infos, ESearchOption filterOperator);

        #endregion

        #region [ Statistic Command ]

        // delete

        int DeleteStatistic(int statisticId);

        // update

        int UpdateStatistic(QOBDSet.statisticsDataTable statisticDataTable);
        int LoadStatistic(Statistic statistic);        

        // getting 

        List<Statistic> GetStatisticData();
        List<Statistic> GetStatisticDataById(int id);
        void FillStatisticDataTableById(QOBDSet.statisticsDataTable statisticDataTable, int id);

        // search

        List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator);

        #endregion
    }
}
