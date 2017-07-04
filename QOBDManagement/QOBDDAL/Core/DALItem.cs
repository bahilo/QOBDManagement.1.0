using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class DALItem : IItemManager
    {
        public Agent AuthenticatedUser { get; set; }
        private Func<double, double> _progressBarFunc;
        private QOBDCommon.Interfaces.REMOTE.IItemManager _gateWayItem;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALItem(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gateWayItem = new GateWayItem(_servicePortType);
            _loadSize = Utility.intTryParse(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Utility.intTryParse(ConfigurationManager.AppSettings["progress_step"]);
        }

        public DALItem(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public DALItem(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
        }

        public bool IsDataDownloading
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsDataDownloading"); }
        }

        public QOBDCommon.Interfaces.REMOTE.IItemManager GateWayItem
        {
            get { return _gateWayItem; }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayItem.setServiceCredential(_servicePortType);
            }
        }

        public async void cacheWebServiceData()
        {
            await retrieveGateWayItemDataAsync();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayItem.setServiceCredential(_servicePortType);
        }

        private async Task retrieveGateWayItemDataAsync()
        {
            lock (_lock) IsDataDownloading = true;
            try
            {
                checkServiceCommunication();
                var itemList = await _gateWayItem.GetItemDataAsync(_loadSize);
                if (itemList.Count > 0)
                    await UpdateItemDependenciesAsync(itemList);

                var providerList = await _gateWayItem.GetProviderDataAsync(_loadSize);
                if (providerList.Count > 0)
                    LoadProvider(providerList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.ITEM);
                checkServiceCommunication();
            }
            finally
            {
                lock (_lock) IsDataDownloading = false;
                try
                {
                    _progressBarFunc((double)100 / _progressStep);
                }
                catch (DivideByZeroException ex) { Log.error(ex.Message, EErrorFrom.STATISTIC); }
                // Log.debug("Loaded[" + _progressBarFunc(0) + "%]!", EErrorFrom.ITEM);
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

        public async Task<List<Item>> InsertItemAsync(List<Item> itemList)
        {
            checkServiceCommunication();
            List<Item> gateWayResultList = await _gateWayItem.InsertItemAsync(itemList);
            List<Item> result = LoadItem(gateWayResultList);
            return result;
        }

        public async Task<List<Provider>> InsertProviderAsync(List<Provider> listProvider)
        {
            checkServiceCommunication();
            List<Provider> gateWayResultList = await _gateWayItem.InsertProviderAsync(listProvider);
            List<Provider> result = LoadProvider(gateWayResultList);
            return result;
        }

        public async Task<List<Provider_item>> InsertProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            checkServiceCommunication();
            List<Provider_item> gateWayResultList = await _gateWayItem.InsertProvider_itemAsync(listProvider_item);
            List<Provider_item> result = LoadProvider_item(gateWayResultList);
            return result;
        }


        public async Task<List<Item_delivery>> InsertItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            checkServiceCommunication();
            List<Item_delivery> gateWayResultList = await _gateWayItem.InsertItem_deliveryAsync(listItem_delivery);
            List<Item_delivery> result = LoadItem_delivery(gateWayResultList);
            return result;
        }

        public async Task<List<Auto_ref>> InsertAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            checkServiceCommunication();
            List<Auto_ref> gateWayResultList = await _gateWayItem.InsertAuto_refAsync(listAuto_ref);
            List<Auto_ref> result = LoadAuto_ref(gateWayResultList);
            return result;
        }

        public async Task<List<Tax_item>> InsertTax_itemAsync(List<Tax_item> listTax_item)
        {
            checkServiceCommunication();
            List<Tax_item> gateWayResultList = await _gateWayItem.InsertTax_itemAsync(listTax_item);
            List<Tax_item> result = LoadTax_item(gateWayResultList);
            return result;
        }

        public async Task<List<Item>> DeleteItemAsync(List<Item> listItem)
        {
            List<Item> result = new List<Item>();
            checkServiceCommunication();
            List<Item> gateWayResultList = await _gateWayItem.DeleteItemAsync(listItem);
            if (gateWayResultList.Count == 0)
                foreach (Item item in listItem)
                {
                    int returnValue = _dataSet.DeleteItem(item.ID);
                    if (returnValue > 0)
                        result.Add(item);
                }
            return result;
        }

        public async Task<List<Provider>> DeleteProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            checkServiceCommunication();
            List<Provider> gateWayResultList = await _gateWayItem.DeleteProviderAsync(listProvider);
            if (gateWayResultList.Count == 0)
                foreach (Provider provider in listProvider)
                {
                    int returnValue = _dataSet.DeleteProvider(provider.ID);
                    if (returnValue > 0)
                        result.Add(provider);
                }
            return result;
        }

        public async Task<List<Provider_item>> DeleteProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            checkServiceCommunication();
            List<Provider_item> gateWayResultList = await _gateWayItem.DeleteProvider_itemAsync(listProvider_item);
            if (gateWayResultList.Count == 0)
                foreach (Provider_item provider_item in listProvider_item)
                {
                    int returnValue = _dataSet.DeleteProvider_item(provider_item.ID);
                    if (returnValue > 0)
                        result.Add(provider_item);
                }
            return result;
        }


        public async Task<List<Item_delivery>> DeleteItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            checkServiceCommunication();
            List<Item_delivery> gateWayResultList = await _gateWayItem.DeleteItem_deliveryAsync(listItem_delivery);
            if (gateWayResultList.Count == 0)
                foreach (Item_delivery item_delivery in listItem_delivery)
                {
                    int returnValue = _dataSet.DeleteItem_delivery(item_delivery.ID);
                    if (returnValue > 0)
                        result.Add(item_delivery);
                }
            return result;
        }

        public async Task<List<Auto_ref>> DeleteAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            checkServiceCommunication();
            List<Auto_ref> gateWayResultList = await _gateWayItem.DeleteAuto_refAsync(listAuto_ref);
            if (gateWayResultList.Count == 0)
                foreach (Auto_ref Auto_ref in listAuto_ref)
                {
                    int returnValue = _dataSet.DeleteAuto_ref(Auto_ref.ID);
                    if (returnValue > 0)
                        result.Add(Auto_ref);
                }
            return result;
        }

        public async Task<List<Tax_item>> DeleteTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            checkServiceCommunication();
            List<Tax_item> gateWayResultList = await _gateWayItem.DeleteTax_itemAsync(listTax_item);
            if (gateWayResultList.Count == 0)
                foreach (Tax_item Tax_item in listTax_item)
                {
                    int returnValue = _dataSet.DeleteTax_item(Tax_item.ID);
                    if (returnValue > 0)
                        result.Add(Tax_item);
                }
            return result;
        }


        public async Task<List<Item>> UpdateItemAsync(List<Item> itemList)
        {            
            checkServiceCommunication();
            List<Item> gateWayResultList = await _gateWayItem.UpdateItemAsync(itemList);
            List<Item> result = LoadItem(gateWayResultList);
            return result;
        }


        public List<Item> LoadItem(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            foreach (var item in itemList)
            {
                int returnResult = _dataSet.LoadItem(item);
                if (returnResult > 0)
                    result.Add(item);
            }
            return result;
        }

        public async Task<List<Provider>> UpdateProviderAsync(List<Provider> providerList)
        {
            checkServiceCommunication();
            List<Provider> gateWayResultList = await _gateWayItem.UpdateProviderAsync(providerList);
            List<Provider> result = LoadProvider(gateWayResultList);
            return result;
        }

        public List<Provider> LoadProvider(List<Provider> providersList)
        {
            List<Provider> result = new List<Provider>();
            foreach (var provider in providersList)
            {
                int returnResult = _dataSet.LoadProvider(provider);
                if (returnResult > 0)
                    result.Add(provider);
            }
            return result;
        }

        public async Task<List<Provider_item>> UpdateProvider_itemAsync(List<Provider_item> provider_itemList)
        {
            checkServiceCommunication();
            List<Provider_item> gateWayResultList = await _gateWayItem.UpdateProvider_itemAsync(provider_itemList);
            List<Provider_item> result = LoadProvider_item(gateWayResultList);

            return result;
        }

        public List<Provider_item> LoadProvider_item(List<Provider_item> provider_itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            foreach (var provider_item in provider_itemList)
            {
                int returnResult = _dataSet.LoadProvider_item(provider_item);
                if (returnResult > 0)
                    result.Add(provider_item);
            }
            return result;
        }

        public async Task<List<Item_delivery>> UpdateItem_deliveryAsync(List<Item_delivery> item_deliveryList)
        {
            checkServiceCommunication();
            List<Item_delivery> gateWayResultList = await _gateWayItem.UpdateItem_deliveryAsync(item_deliveryList);
            List<Item_delivery> result = LoadItem_delivery(gateWayResultList);
            return result;
        }

        public List<Item_delivery> LoadItem_delivery(List<Item_delivery> item_deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            foreach (Item_delivery item_delivery in item_deliveryList)
            {
                int returnResult = _dataSet.LoadItem_delivery(item_delivery);
                if (returnResult > 0)
                    result.Add(item_delivery);
            }
            return result;
        }

        public async Task<List<Auto_ref>> UpdateAuto_refAsync(List<Auto_ref> auto_refList)
        {
            checkServiceCommunication();
            List<Auto_ref> gateWayResultList = await _gateWayItem.UpdateAuto_refAsync(auto_refList);
            List<Auto_ref> result = LoadAuto_ref(gateWayResultList);
            return result;
        }

        public List<Auto_ref> LoadAuto_ref(List<Auto_ref> aut_refsList)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            foreach (var auto_ref in aut_refsList)
            {
                int returnResult = _dataSet.LoadAuto_ref(auto_ref);
                if (returnResult > 0)
                    result.Add(auto_ref);
            }
            return result;
        }

        public async Task<List<Tax_item>> UpdateTax_itemAsync(List<Tax_item> tax_itemList)
        {
            checkServiceCommunication();
            List<Tax_item> gateWayResultList = await _gateWayItem.UpdateTax_itemAsync(tax_itemList);
            List<Tax_item> result = LoadTax_item(gateWayResultList);
            return result;
        }

        public List<Tax_item> LoadTax_item(List<Tax_item> tax_itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            foreach (var tax_item in tax_itemList)
            {
                int returnResult = _dataSet.LoadTax_item(tax_item);
                if (returnResult > 0)
                    result.Add(tax_item);
            }
            return result;
        }

        public List<Item> GetItemData(int nbLine)
        {
            List<Item> result = _dataSet.GetItemData();
            if (nbLine.Equals(999) || result.Count == 0 || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Item>> GetItemDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetItemDataAsync(nbLine);
        }

        public List<Item> GetItemDataByOrder_itemList(List<Order_item> order_itemList)
        {
            List<Item> result = new List<Item>();
            foreach (Order_item order_item in order_itemList)
            {
                var itemList = searchItem(new Item { Ref = order_item.Item_ref }, ESearchOption.AND);
                if (itemList.Count() > 0)
                    result.Add(itemList.First());
            }
            return result;
        }

        public async Task<List<Item>> GetItemDataByOrder_itemListAsync(List<Order_item> order_itemList)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetItemDataByOrder_itemListAsync(order_itemList);
        }

        public List<Item> GetItemDataById(int id)
        {
            return _dataSet.GetItemDataById(id);
        }

        public List<Provider> GetProviderData(int nbLine)
        {
            List<Provider> result = _dataSet.GetProviderData();
            if (nbLine.Equals(999) || result.Count == 0 || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Provider>> GetProviderDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetProviderDataAsync(nbLine);
        }

        public List<Provider> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            foreach (Provider_item provider_item in provider_itemList)
            {
                var providerList = searchProvider(new Provider { ID = provider_item.ProviderId }, ESearchOption.AND);
                if (providerList.Count() > 0)
                    result.Add(providerList.First());
            }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataByProvider_itemListAsync(List<Provider_item> provider_itemList)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetProviderDataByProvider_itemListAsync(provider_itemList);
        }

        public List<Provider> GetProviderDataById(int id)
        {
            return _dataSet.GetProviderDataById(id);
        }

        public List<Provider_item> GetProvider_itemData(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            result = _dataSet.GetProvider_itemData();
            if (nbLine.Equals(999) || result.Count == 0 || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Provider_item>> GetProvider_itemDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetProvider_itemDataAsync(nbLine);
        }

        public List<Provider_item> GetProvider_itemDataByItemList(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            foreach (Item item in itemList)
            {
                var provider_itemList = searchProvider_item(new Provider_item { ItemId = item.ID }, ESearchOption.AND);
                if (provider_itemList.Count() > 0)
                    result.Add(provider_itemList.First());
            }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByItemListAsync(List<Item> itemList)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetProvider_itemDataByItemListAsync(itemList);
        }

        public List<Provider_item> GetProvider_itemDataById(int id)
        {
            return _dataSet.GetProvider_itemDataById(id);
        }

        public List<Item_delivery> GetItem_deliveryData(int nbLine)
        {
            List<Item_delivery> result = _dataSet.GetItem_deliveryData();
            if (nbLine.Equals(999) || result.Count == 0 || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetItem_deliveryDataAsync(nbLine);
        }

        public List<Item_delivery> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            foreach (Delivery delivery in deliveryList)
            {
                var item_deliveryList = searchItem_delivery(new Item_delivery { DeliveryId = delivery.ID }, ESearchOption.AND);
                if (item_deliveryList.Count() > 0)
                    result.Add(item_deliveryList.First());
            }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryListAsync(List<Delivery> deliveryList)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetItem_deliveryDataByDeliveryListAsync(deliveryList);
        }

        public List<Item_delivery> GetItem_deliveryDataById(int id)
        {
            return _dataSet.GetItem_deliveryDataByDeliveryId(id);
        }

        public List<Auto_ref> GetAuto_refData(int nbLine)
        {
            List<Auto_ref> result = _dataSet.GetAuto_refData();
            if (nbLine.Equals(999) || result.Count == 0 || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Auto_ref>> GetAuto_refDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetAuto_refDataAsync(nbLine);
        }

        public List<Auto_ref> GetAuto_refDataById(int id)
        {
            return searchAuto_ref(new Auto_ref { ID = id }, ESearchOption.AND);
        }

        public List<Tax_item> GetTax_itemData(int nbLine)
        {
            List<Tax_item> result = _dataSet.GetTax_itemData();
            if (nbLine.Equals(999) || result.Count == 0 || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax_item>> GetTax_itemDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetTax_itemDataAsync(nbLine);
        }

        public List<Tax_item> GetTax_itemDataByItemList(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            foreach (Item item in itemList)
            {
                var tax_itemList = searchTax_item(new Tax_item { Item_ref = item.Ref, itemId = item.ID }, ESearchOption.OR);
                if (tax_itemList.Count() > 0)
                    result.Add(tax_itemList.First());
            }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataByItemListAsync(List<Item> itemList)
        {
            checkServiceCommunication();
            return await _gateWayItem.GetTax_itemDataByItemListAsync(itemList);
        }

        public List<Tax_item> GetTax_itemDataById(int id)
        {
            return searchTax_item(new Tax_item { ID = id }, ESearchOption.AND);
        }

        public List<Item> searchItem(Item item, ESearchOption filterOperator)
        {
            return _dataSet.searchItem(item, filterOperator);
        }

        public async Task<List<Item>> searchItemAsync(Item item, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayItem.searchItemAsync(item, filterOperator);
        }

        public List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator)
        {
            return _dataSet.searchProvider(Provider, filterOperator);
        }

        public async Task<List<Provider>> searchProviderAsync(Provider Provider, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayItem.searchProviderAsync(Provider, filterOperator);
        }

        public List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator)
        {
            return _dataSet.searchProvider_item(Provider_item, filterOperator);
        }

        public async Task<List<Provider_item>> searchProvider_itemAsync(Provider_item Provider_item, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayItem.searchProvider_itemAsync(Provider_item, filterOperator);
        }

        public List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            return _dataSet.searchItem_delivery(Item_delivery, filterOperator);
        }

        public async Task<List<Item_delivery>> searchItem_deliveryAsync(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayItem.searchItem_deliveryAsync(Item_delivery, filterOperator);
        }

        public List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            return _dataSet.searchAuto_ref(Auto_ref, filterOperator);
        }

        public async Task<List<Auto_ref>> searchAuto_refAsync(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayItem.searchAuto_refAsync(Auto_ref, filterOperator);
        }

        public List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator)
        {
            return _dataSet.searchTax_item(Tax_item, filterOperator);
        }

        public async Task<List<Tax_item>> searchTax_itemAsync(Tax_item Tax_item, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayItem.searchTax_itemAsync(Tax_item, filterOperator);
        }
        #endregion

        public void Dispose()
        {
            if(_gateWayItem != null)
                _gateWayItem.Dispose();
        }

        public async Task UpdateItemDependenciesAsync(List<Item> itemList, bool isActiveProgress = false)
        {
            int loadUnit = 50;
            List<Provider> providerList = new List<Provider>();
            List<Provider_item> savedProvider_itemList = new List<Provider_item>();
            List<Item> savedItemList = LoadItem(itemList);
            for (int i = 0; i < (savedItemList.Count() / loadUnit) || loadUnit >= savedItemList.Count() && i == 0; i++)
            {
                savedProvider_itemList = LoadProvider_item(await _gateWayItem.GetProvider_itemDataByItemListAsync(savedItemList.Skip(i * loadUnit).Take(loadUnit).ToList()));
                if(savedProvider_itemList.Count > 0)
                    LoadProvider(await _gateWayItem.GetProviderDataByProvider_itemListAsync(savedProvider_itemList.OrderBy(x => x.ProviderId).Distinct().ToList()));
            }

        }
    } /* end class BLItem */
}