using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.REMOTE;
using QOBDGateway.Classes;
using QOBDGateway.Helper.ChannelHelper;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWayItem : IItemManager, INotifyPropertyChanged
    {
        private ClientProxy _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayItem(ClientProxy servicePort)
        {
            _channel = servicePort;
        }

        public void setServiceCredential(object channel)
        {
            _channel = (ClientProxy)channel;
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task<List<Item>> InsertItemAsync(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            try
            {                
                result = (await _channel.insert_data_itemAsync(itemList.ItemTypeToArray())).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> InsertProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            try
            {                
                result = (await _channel.insert_data_providerAsync(listProvider.ProviderTypeToArray())).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> InsertProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {                
                result = (await _channel.insert_data_provider_itemAsync(listProvider_item.Provider_itemTypeToArray())).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> InsertOrder_itemAsync(List<Order_item> listOrder_item)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {                
                result = (await _channel.insert_data_order_itemAsync(listOrder_item.order_itemTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Item_delivery>> InsertItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {                
                result = (await _channel.insert_data_item_deliveryAsync(listItem_delivery.Item_deliveryTypeToArray())).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Auto_ref>> InsertAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.insert_data_auto_refAsync(listAuto_ref.Auto_refTypeToArray())).ArrayTypeToAuto_ref();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> InsertTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.insert_data_tax_itemAsync(listTax_item.Tax_itemTypeToArray())).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item>> DeleteItemAsync(List<Item> listItem)
        {
            List<Item> result = new List<Item>();
            try
            {                
                result = (await _channel.delete_data_itemAsync(listItem.ItemTypeToArray())).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> DeleteProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            try
            {                
                result = (await _channel.delete_data_providerAsync(listProvider.ProviderTypeToArray())).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> DeleteProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {                
                result = (await _channel.delete_data_provider_itemAsync(listProvider_item.Provider_itemTypeToArray())).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> DeleteOrder_itemAsync(List<Order_item> listOrder_item)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {                
                result = (await _channel.delete_data_order_itemAsync(listOrder_item.order_itemTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item_delivery>> DeleteItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {                
                result = (await _channel.delete_data_item_deliveryAsync(listItem_delivery.Item_deliveryTypeToArray())).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Auto_ref>> DeleteAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.delete_data_auto_refAsync(listAuto_ref.Auto_refTypeToArray())).ArrayTypeToAuto_ref();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> DeleteTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.delete_data_tax_itemAsync(listTax_item.Tax_itemTypeToArray())).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Item>> UpdateItemAsync(List<Item> listItem)
        {
            List<Item> result = new List<Item>();
            try
            {                
                result = (await _channel.update_data_itemAsync(listItem.ItemTypeToArray())).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> UpdateProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            try
            {                
                result = (await _channel.update_data_providerAsync(listProvider.ProviderTypeToArray())).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> UpdateProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {                
                result = (await _channel.update_data_provider_itemAsync(listProvider_item.Provider_itemTypeToArray())).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> UpdateOrder_itemAsync(List<Order_item> listOrder_item)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {                
                result = (await _channel.update_data_order_itemAsync(listOrder_item.order_itemTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item_delivery>> UpdateItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {                
                result = (await _channel.update_data_item_deliveryAsync(listItem_delivery.Item_deliveryTypeToArray())).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Auto_ref>> UpdateAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.update_data_auto_refAsync(listAuto_ref.Auto_refTypeToArray())).ArrayTypeToAuto_ref();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> UpdateTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.update_data_tax_itemAsync(listTax_item.Tax_itemTypeToArray())).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item>> GetItemDataAsync(int nbLine)
        {
            List<Item> result = new List<Item>();
            try
            {                
                result = (await _channel.get_data_itemAsync(nbLine.ToString())).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item>> GetItemDataByOrder_itemListAsync(List<Order_item> order_itemList)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = (await _channel.get_data_item_by_order_item_listAsync(order_itemList.order_itemTypeToArray())).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item>> GetItemDataByIdAsync(int id)
        {
            List<Item> result = new List<Item>();
            try
            {                
                result = (await _channel.get_data_item_by_idAsync(id.ToString())).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataAsync(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.get_data_providerAsync(nbLine.ToString())).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataByProvider_itemListAsync(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = (await _channel.get_data_provider_by_provider_item_listAsync(provider_itemList.Provider_itemTypeToArray())).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataByIdAsync(int id)
        {
            List<Provider> result = new List<Provider>();
            try
            {                
                result = (await _channel.get_data_provider_by_idAsync(id.ToString())).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataAsync(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {                
                result = (await _channel.get_data_provider_itemAsync(nbLine.ToString())).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByItemListAsync(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = (await _channel.get_data_provider_item_by_item_listAsync(itemList.ItemTypeToArray())).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByIdAsync(int id)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {                
                result = (await _channel.get_data_provider_item_by_idAsync(id.ToString())).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemDataAsync(int nbLine)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {                
                result = (await _channel.get_data_order_itemAsync(nbLine.ToString())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemDataByIdAsync(int id)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {                
                result = (await _channel.get_data_order_item_by_idAsync(id.ToString())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataAsync(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {                
                result = (await _channel.get_data_item_deliveryAsync(nbLine.ToString())).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryListAsync(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = (await _channel.get_data_item_delivery_by_delivery_listAsync(deliveryList.DeliveryTypeToArray())).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataByIdAsync(int id)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {                
                result = (await _channel.get_data_item_delivery_by_idAsync(id.ToString())).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Auto_ref>> GetAuto_refDataAsync(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.get_data_auto_refAsync(nbLine.ToString())).ArrayTypeToAuto_ref();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Auto_ref>> GetAuto_refDataByIdAsync(int id)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.get_data_auto_ref_by_idAsync(id.ToString())).ArrayTypeToAuto_ref();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataAsync(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_data_tax_itemAsync(nbLine.ToString())).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataByItemListAsync(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_data_tax_item_by_item_listAsync(itemList.ItemTypeToArray())).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataByIdAsync(int id)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_data_tax_item_by_idAsync(id.ToString())).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Item>> searchItemAsync(Item item, ESearchOption filterOperator)
        {
            List<Item> result = new List<Item>();
            try
            {                
                result = (await _channel.get_filter_itemAsync(item.ItemTypeToFilterArray(filterOperator))).ArrayTypeToItem();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider>> searchProviderAsync(Provider Provider, ESearchOption filterOperator)
        {
            List<Provider> result = new List<Provider>();
            try
            {                
                result = (await _channel.get_filter_providerAsync(Provider.ProviderTypeToFilterArray(filterOperator))).ArrayTypeToProvider();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Provider_item>> searchProvider_itemAsync(Provider_item Provider_item, ESearchOption filterOperator)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {                
                result = (await _channel.get_filter_provider_itemAsync(Provider_item.Provider_itemTypeToFilterArray(filterOperator))).ArrayTypeToProvider_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> searchOrder_itemAsync(Order_item Order_item, ESearchOption filterOperator)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {                
                result = (await _channel.get_filter_order_itemAsync(Order_item.Order_itemTypeToFilterArray(filterOperator))).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item_delivery>> searchItem_deliveryAsync(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {                
                result = (await _channel.get_filter_item_deliveryAsync(Item_delivery.Item_deliveryTypeToFilterArray(filterOperator))).ArrayTypeToItem_delivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Auto_ref>> searchAuto_refAsync(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.get_filter_auto_refAsync(Auto_ref.Auto_refTypeToFilterArray(filterOperator))).ArrayTypeToAuto_ref();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_item>> searchTax_itemAsync(Tax_item Tax_item, ESearchOption filterOperator)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_filter_tax_itemAsync(Tax_item.Tax_itemTypeToFilterArray(filterOperator))).ArrayTypeToTax_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Item>> searchItemFromWebServiceAsync(Item item, ESearchOption filterOperator)
        {
            return await searchItemAsync(item, filterOperator);
        }

        public async Task<List<Provider>> searchProviderFromWebServiceAsync(Provider Provider, ESearchOption filterOperator)
        {
            return await searchProviderAsync(Provider, filterOperator);
        }

        public async Task<List<Provider_item>> searchProvider_itemFromWebServiceAsync(Provider_item Provider_item, ESearchOption filterOperator)
        {
            return await searchProvider_itemAsync(Provider_item, filterOperator);
        }

        public async Task<List<Item_delivery>> searchItem_deliveryFromWebServiceAsync(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            return await searchItem_deliveryAsync(Item_delivery, filterOperator);
        }

        public void Dispose()
        {
            if (_channel.State == CommunicationState.Opened)
                _channel.Close();
        }
    } /* end class BLItem */
}