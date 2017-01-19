using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
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
        private Func<double, double> _rogressBarFunc;
        private GateWayItem _gateWayItem;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALItem()
        {
            _gateWayItem = new GateWayItem();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayItem.PropertyChanged += onCredentialChange_loadItemDataFromWebService;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsLodingDataFromWebServiceToLocal"); }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GateWayItem GateWayItem
        {
            get { return _gateWayItem; }
        }

        private void onCredentialChange_loadItemDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayDataItem);                
            }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayItem.initializeCredential(AuthenticatedUser);
            }
        }

        private void retrieveGateWayDataItem()
        {
            int loadUnit = 50;

            List<Provider> providerList = new List<Provider>();
            List<Provider_item> provider_itemList = new List<Provider_item>();

            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                var itemList = new NotifyTaskCompletion<List<Item>>(_gateWayItem.GetItemDataAsync(_loadSize)).Task.Result;
                if (itemList.Count > 0)
                {
                    List<Item> savedItemList = LoadItem(itemList);

                    for (int i = 0; i < (savedItemList.Count() / loadUnit) || loadUnit >= savedItemList.Count() && i == 0; i++)
                    {
                        lock (_lock)
                        {
                            List<Provider_item> savedProvider_itemList = LoadProvider_item(new NotifyTaskCompletion<List<Provider_item>>(_gateWayItem.GetProvider_itemDataByItemListAsync(savedItemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result);
                            List<Provider> savedProviderList = LoadProvider(new NotifyTaskCompletion<List<Provider>>(_gateWayItem.GetProviderDataByProvider_itemListAsync(savedProvider_itemList.OrderBy(x => x.Provider_name).Distinct().ToList())).Task.Result);
                        }
                    }
                }
                
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

        public async Task<List<Item>> InsertItemAsync(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            List<Item> gateWayResultList = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.InsertItemAsync(itemList);
                
                result = LoadItem(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Provider>> InsertProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            List<Provider> gateWayResultList = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.InsertProviderAsync(listProvider);
                
                result = LoadProvider(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Provider_item>> InsertProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.InsertProvider_itemAsync(listProvider_item);
                                
                result = LoadProvider_item(gateWayResultList);
            }
            return result;
        }


        public async Task<List<Item_delivery>> InsertItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.InsertItem_deliveryAsync(listItem_delivery);
                
                result = LoadItem_delivery(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Auto_ref>> InsertAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            List<Auto_ref> gateWayResultList = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.InsertAuto_refAsync(listAuto_ref);
                
                result = LoadAuto_ref(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Tax_item>> InsertTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            List<Tax_item> gateWayResultList = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.InsertTax_itemAsync(listTax_item);
                
                result = LoadTax_item(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Item>> DeleteItemAsync(List<Item> listItem)
        {
            List<Item> result = listItem;
            List<Item> gateWayResultList = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.DeleteItemAsync(listItem);
                if (gateWayResultList.Count == 0)
                    foreach (Item item in listItem)
                    {
                        int returnValue = _itemTableAdapter.Delete1(item.ID);
                        if (returnValue > 0)
                            result.Remove(item);
                    }
            }
            return result;
        }

        public async Task<List<Provider>> DeleteProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = listProvider;
            List<Provider> gateWayResultList = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.DeleteProviderAsync(listProvider);
                if (gateWayResultList.Count == 0)
                    foreach (Provider provider in listProvider)
                    {
                        int returnValue = _providersTableAdapter.Delete1(provider.ID);
                        if (returnValue > 0)
                            result.Remove(provider);
                    }
            }
            return result;
        }

        public async Task<List<Provider_item>> DeleteProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = listProvider_item;
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.DeleteProvider_itemAsync(listProvider_item);
                if (gateWayResultList.Count == 0)
                    foreach (Provider_item provider_item in listProvider_item)
                    {
                        int returnValue = _provider_itemsTableAdapter.Delete1(provider_item.ID);
                        if (returnValue > 0)
                            result.Remove(provider_item);
                    }
            }
            return result;
        }


        public async Task<List<Item_delivery>> DeleteItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = listItem_delivery;
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.DeleteItem_deliveryAsync(listItem_delivery);
                if (gateWayResultList.Count == 0)
                    foreach (Item_delivery item_delivery in gateWayResultList)
                    {
                        int returnValue = _item_deliveriesTableAdapter.Delete1(item_delivery.ID);
                        if (returnValue > 0)
                            result.Remove(item_delivery);
                    }
            }
            return result;
        }

        public async Task<List<Auto_ref>> DeleteAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = listAuto_ref;
            List<Auto_ref> gateWayResultList = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.DeleteAuto_refAsync(listAuto_ref);
                if (gateWayResultList.Count == 0)
                    foreach (Auto_ref Auto_ref in listAuto_ref)
                    {
                        int returnValue = _auto_refTableAdapter.Delete1(Auto_ref.ID);
                        if (returnValue > 0)
                            result.Remove(Auto_ref);
                    }
            }
            return result;
        }

        public async Task<List<Tax_item>> DeleteTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = listTax_item;
            List<Tax_item> gateWayResultList = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.DeleteTax_itemAsync(listTax_item);
                if (gateWayResultList.Count == 0)
                    foreach (Tax_item Tax_item in listTax_item)
                    {
                        int returnValue = _tax_itemTableAdapter.Delete1(Tax_item.ID);
                        if (returnValue > 0)
                            result.Remove(Tax_item);
                    }
            }
            return result;
        }


        public async Task<List<Item>> UpdateItemAsync(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            List<Item> gateWayResultList = new List<Item>();
            QOBDSet dataSet = new QOBDSet();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.UpdateItemAsync(itemList);

                foreach (var item in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _itemTableAdapter.FillById(dataSetLocal.items, item.ID);
                    dataSet.items.Merge(dataSetLocal.items);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _itemTableAdapter.Update(gateWayResultList.ItemTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }


        public List<Item> LoadItem(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            {
                foreach (var item in itemList)
                //Parallel.ForEach(gateWayResultList, (item) =>
                {
                    int returnResult = _itemTableAdapter
                                            .load_data_item(
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
                    if (returnResult > 0)
                        result.Add(item);
                }
                //);
            }
            return result;
        }

        public async Task<List<Provider>> UpdateProviderAsync(List<Provider> providerList)
        {
            List<Provider> result = new List<Provider>();
            List<Provider> gateWayResultList = new List<Provider>();
            QOBDSet dataSet = new QOBDSet();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.UpdateProviderAsync(providerList);

                foreach (var provider in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _providersTableAdapter.FillById(dataSetLocal.providers, provider.ID);
                    dataSet.providers.Merge(dataSetLocal.providers);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _providersTableAdapter.Update(gateWayResultList.ProviderTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Provider> LoadProvider(List<Provider> providersList)
        {
            List<Provider> result = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            {
                foreach (var provider in providersList)
                {
                    int returnResult = _providersTableAdapter
                                            .load_data_provider(
                                                provider.Name,
                                                provider.Source,
                                                provider.ID);
                    if (returnResult > 0)
                        result.Add(provider);
                }
            }
            return result;
        }

        public async Task<List<Provider_item>> UpdateProvider_itemAsync(List<Provider_item> provider_itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            QOBDSet dataSet = new QOBDSet();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.UpdateProvider_itemAsync(provider_itemList);

                foreach (var provider_item in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _provider_itemsTableAdapter.FillById(dataSetLocal.provider_items, provider_item.ID);
                    dataSet.provider_items.Merge(dataSetLocal.provider_items);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _provider_itemsTableAdapter.Update(gateWayResultList.Provider_itemTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Provider_item> LoadProvider_item(List<Provider_item> provider_itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            {
                foreach (var provider_item in provider_itemList)
                {
                    int returnResult = _provider_itemsTableAdapter
                                            .load_data_provider_item(
                                                provider_item.Provider_name,
                                                provider_item.Item_ref,
                                                provider_item.ID);

                    if (returnResult > 0)
                        result.Add(provider_item);
                }
            }
            return result;
        }

        public async Task<List<Item_delivery>> UpdateItem_deliveryAsync(List<Item_delivery> item_deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            QOBDSet dataSet = new QOBDSet();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.UpdateItem_deliveryAsync(item_deliveryList);

                foreach (var item_delivery in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _item_deliveriesTableAdapter.FillById(dataSetLocal.item_deliveries, item_delivery.ID);
                    dataSet.item_deliveries.Merge(dataSetLocal.item_deliveries);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _item_deliveriesTableAdapter.Update(gateWayResultList.Item_deliveryTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Item_delivery> LoadItem_delivery(List<Item_delivery> item_deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            {
                foreach (Item_delivery item_delivery in item_deliveryList)
                {
                    int returnResult = _item_deliveriesTableAdapter
                                    .load_data_item_delivery(
                                        item_delivery.DeliveryId,
                                        item_delivery.Item_ref,
                                        item_delivery.Quantity_delivery,
                                        item_delivery.ID);

                    if (returnResult > 0)
                        result.Add(item_delivery);
                }
            }
            return result;
        }

        public async Task<List<Auto_ref>> UpdateAuto_refAsync(List<Auto_ref> auto_refList)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            List<Auto_ref> gateWayResultList = new List<Auto_ref>();
            QOBDSet dataSet = new QOBDSet();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.UpdateAuto_refAsync(auto_refList);

                foreach (var auto_ref in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _auto_refTableAdapter.FillById(dataSetLocal.auto_refs, auto_ref.ID);
                    dataSet.auto_refs.Merge(dataSetLocal.auto_refs);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _auto_refTableAdapter.Update(gateWayResultList.Auto_refTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }

            return result;
        }

        public List<Auto_ref> LoadAuto_ref(List<Auto_ref> aut_refsList)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            {
                foreach (var Auto_ref in aut_refsList)
                {
                    int returnResult = _auto_refTableAdapter
                                    .load_auto_ref(
                                        Auto_ref.RefId,
                                        Auto_ref.ID);

                    if (returnResult > 0)
                        result.Add(Auto_ref);
                }
            }

            return result;
        }

        public async Task<List<Tax_item>> UpdateTax_itemAsync(List<Tax_item> tax_itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            List<Tax_item> gateWayResultList = new List<Tax_item>();
            QOBDSet dataSet = new QOBDSet();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            {
                _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayItem.UpdateTax_itemAsync(tax_itemList);

                foreach (var tax_item in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _tax_itemTableAdapter.FillById(dataSetLocal.tax_items, tax_item.ID);
                    dataSet.tax_items.Merge(dataSetLocal.tax_items);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _tax_itemTableAdapter.Update(gateWayResultList.Tax_itemTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }

            return result;
        }

        public List<Tax_item> LoadTax_item(List<Tax_item> tax_itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            {
                foreach (var Tax_item in tax_itemList)
                {
                    int returnResult = _tax_itemTableAdapter
                        .load_data_tax_item(
                            Tax_item.TaxId,
                            Tax_item.Item_ref,
                            Tax_item.Tax_value,
                            Tax_item.Tax_type,
                            Tax_item.ID);

                    if (returnResult > 0)
                        result.Add(Tax_item);
                }
            }

            return result;
        }

        public List<Item> GetItemData(int nbLine)
        {
            List<Item> result = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
                result =  _itemTableAdapter.GetData().DataTableTypeToItem();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Item>> GetItemDataAsync(int nbLine)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayItem.GetItemDataByOrder_itemListAsync(order_itemList);
        }

        public List<Item> GetItemDataById(int id)
        {
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
                return _itemTableAdapter.get_item_by_id(id).DataTableTypeToItem();
        }

        public List<Provider> GetProviderData(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
                result = _providersTableAdapter.GetData().DataTableTypeToProvider();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Provider>> GetProviderDataAsync(int nbLine)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetProviderDataAsync(nbLine);
        }

        public List<Provider> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            foreach (Provider_item provider_item in provider_itemList)
            {
                var providerList = searchProvider(new Provider { Name = provider_item.Provider_name }, ESearchOption.AND);
                if (providerList.Count() > 0)
                    result.Add(providerList.First());
            }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataByProvider_itemListAsync(List<Provider_item> provider_itemList)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetProviderDataByProvider_itemListAsync(provider_itemList);
        }

        public List<Provider> GetProviderDataById(int id)
        {
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
                return _providersTableAdapter.get_provider_by_id(id).DataTableTypeToProvider();

        }

        public List<Provider_item> GetProvider_itemData(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
                result = _provider_itemsTableAdapter.GetData().DataTableTypeToProvider_item();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Provider_item>> GetProvider_itemDataAsync(int nbLine)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetProvider_itemDataAsync(nbLine);
        }

        public List<Provider_item> GetProvider_itemDataByItemList(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            foreach (Item item in itemList)
            {
                var provider_itemList = searchProvider_item(new Provider_item { Item_ref = item.Ref }, ESearchOption.AND);
                if (provider_itemList.Count() > 0)
                    result.Add(provider_itemList.First());
            }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByItemListAsync(List<Item> itemList)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetProvider_itemDataByItemListAsync(itemList);
        }

        public List<Provider_item> GetProvider_itemDataById(int id)
        {
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
                return _provider_itemsTableAdapter.get_provider_item_by_id(id).DataTableTypeToProvider_item();
        }

        public List<Item_delivery> GetItem_deliveryData(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
                result = _item_deliveriesTableAdapter.GetData().DataTableTypeToItem_delivery();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataAsync(int nbLine)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetItem_deliveryDataByDeliveryListAsync(deliveryList);
        }

        public List<Item_delivery> GetItem_deliveryDataById(int id)
        {
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
                return _item_deliveriesTableAdapter.get_item_delivery_by_delivery_id(id).DataTableTypeToItem_delivery();
        }

        public List<Auto_ref> GetAuto_refData(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refDataTable = new auto_refsTableAdapter())
                result = _auto_refDataTable.GetData().DataTableTypeToAuto_ref();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);            
        }

        public async Task<List<Auto_ref>> GetAuto_refDataAsync(int nbLine)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetAuto_refDataAsync(nbLine);
        }

        public List<Auto_ref> GetAuto_refDataById(int id)
        {
            return searchAuto_ref(new Auto_ref { ID = id }, ESearchOption.AND);
        }

        public List<Tax_item> GetTax_itemData(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
                result = _tax_itemTableAdapter.GetData().DataTableTypeToTax_item();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax_item>> GetTax_itemDataAsync(int nbLine)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
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
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.GetTax_itemDataByItemListAsync(itemList);
        }

        public List<Tax_item> GetTax_itemDataById(int id)
        {
            return searchTax_item(new Tax_item { ID = id }, ESearchOption.AND);
        }

        public List<Item> searchItem(Item item, ESearchOption filterOperator)
        {
            return item.ItemTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Item>> searchItemAsync(Item item, ESearchOption filterOperator)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.searchItemAsync(item, filterOperator);
        }

        public List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator)
        {
            return Provider.ProviderTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Provider>> searchProviderAsync(Provider Provider, ESearchOption filterOperator)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.searchProviderAsync(Provider, filterOperator);
        }

        public List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator)
        {
            return Provider_item.Provider_itemTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Provider_item>> searchProvider_itemAsync(Provider_item Provider_item, ESearchOption filterOperator)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.searchProvider_itemAsync(Provider_item, filterOperator);
        }

        public List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            return Item_delivery.Item_deliveryTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Item_delivery>> searchItem_deliveryAsync(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.searchItem_deliveryAsync(Item_delivery, filterOperator);
        }

        public List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            return Auto_ref.Auto_refTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Auto_ref>> searchAuto_refAsync(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.searchAuto_refAsync(Auto_ref, filterOperator);
        }

        public List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator)
        {
            return Tax_item.Tax_itemTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Tax_item>> searchTax_itemAsync(Tax_item Tax_item, ESearchOption filterOperator)
        {
            _gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            return await _gateWayItem.searchTax_itemAsync(Tax_item, filterOperator);
        }        

        public void Dispose()
        {
            _gateWayItem.PropertyChanged -= onCredentialChange_loadItemDataFromWebService;
            _gateWayItem.Dispose();
        }
    } /* end class BLItem */
}