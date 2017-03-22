using QOBDBusiness.Helper;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public class BLItem : IItemManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC { get; set; }

        public BLItem(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALItem.initializeCredential(user);
        }

        public void cacheWebServiceData()
        {
            DAC.DALItem.cacheWebServiceData();
        }


        public void setServiceCredential(object channel)
        {
            DAC.DALItem.setServiceCredential(channel);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALItem.progressBarManagement(progressBarFunc);
        }

        public async Task UpdateItemDependenciesAsync(List<Item> itemList, bool isActiveProgress = false)
        {
            if (itemList.Count > 0)
                await DAC.DALItem.UpdateItemDependenciesAsync(itemList);
        }

        #region [ Item ]

        public async Task<List<Item>> InsertItemAsync(List<Item> itemList)
        {
            if (itemList == null  || itemList.Count == 0)
                return new List<Item>();

            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.InsertItemAsync(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item>> DeleteItemAsync(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(itemList.Where(x => x.ID == 0).Count()))
                itemList = itemList.Where(x => x.ID != 0).ToList();

            if (itemList == null || itemList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.DeleteItemAsync(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item>> UpdateItemAsync(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(itemList.Where(x => x.ID == 0).Count()))
                itemList = itemList.Where(x => x.ID != 0).ToList();

            if (itemList == null || itemList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.UpdateItemAsync(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item> GetItemData(int nbLine)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = DAC.DALItem.GetItemData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item>> GetItemDataAsync(int nbLine)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.GetItemDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item> GetItemDataByOrder_itemList(List<Order_item> order_itemList)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = DAC.DALItem.GetItemDataByOrder_itemList(order_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item>> GetItemDataByOrder_itemListAsync(List<Order_item> order_itemList)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.GetItemDataByOrder_itemListAsync(order_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item> GetItemDataById(int id)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = DAC.DALItem.GetItemDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item> searchItem(Item item, ESearchOption filterOperator)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = DAC.DALItem.searchItem(item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item>> searchItemAsync(Item item, ESearchOption filterOperator)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.searchItemAsync(item, filterOperator);
                await UpdateItemDependenciesAsync(result);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        #endregion

        #region [ Provider ]

        public async Task<List<Provider>> InsertProviderAsync(List<Provider> listProvider)
        {
            if (listProvider == null || listProvider.Count == 0)
                return new List<Provider>();

            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.InsertProviderAsync(listProvider);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider>> DeleteProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listProvider.Where(x => x.ID == 0).Count()))
                listProvider = listProvider.Where(x => x.ID != 0).ToList();

            if (listProvider == null || listProvider.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.DeleteProviderAsync(listProvider);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider>> UpdateProviderAsync(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listProvider.Where(x => x.ID == 0).Count()))
                listProvider = listProvider.Where(x => x.ID != 0).ToList();

            if (listProvider == null || listProvider.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.UpdateProviderAsync(listProvider);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider> GetProviderData(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = DAC.DALItem.GetProviderData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataAsync(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.GetProviderDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = DAC.DALItem.GetProviderDataByProvider_itemList(provider_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataByProvider_itemListAsync(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.GetProviderDataByProvider_itemListAsync(provider_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider> GetProviderDataById(int id)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = DAC.DALItem.GetProviderDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider> searchProvider(Provider Provider, ESearchOption filterOperator)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = DAC.DALItem.searchProvider(Provider, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider>> searchProviderAsync(Provider Provider, ESearchOption filterOperator)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.searchProviderAsync(Provider, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        #endregion

        #region [ Provider_item ]

        public async Task<List<Provider_item>> InsertProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            if (listProvider_item == null || listProvider_item.Count == 0)
                return new List<Provider_item>();

            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.InsertProvider_itemAsync(listProvider_item);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider_item>> DeleteProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listProvider_item.Where(x => x.ID == 0).Count()))
                listProvider_item = listProvider_item.Where(x => x.ID != 0).ToList();

            if (listProvider_item == null || listProvider_item.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.DeleteProvider_itemAsync(listProvider_item);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider_item>> UpdateProvider_itemAsync(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listProvider_item.Where(x => x.ID == 0).Count()))
                listProvider_item = listProvider_item.Where(x => x.ID != 0).ToList();

            if (listProvider_item == null || listProvider_item.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.UpdateProvider_itemAsync(listProvider_item);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider_item> GetProvider_itemData(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = DAC.DALItem.GetProvider_itemData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataAsync(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.GetProvider_itemDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider_item> GetProvider_itemDataByItemList(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = DAC.DALItem.GetProvider_itemDataByItemList(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByItemListAsync(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.GetProvider_itemDataByItemListAsync(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider_item> GetProvider_itemDataById(int id)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = DAC.DALItem.GetProvider_itemDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Provider_item> searchProvider_item(Provider_item Provider_item, ESearchOption filterOperator)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = DAC.DALItem.searchProvider_item(Provider_item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Provider_item>> searchProvider_itemAsync(Provider_item Provider_item, ESearchOption filterOperator)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.searchProvider_itemAsync(Provider_item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        #endregion

        #region [ Item_delivery ]

        public async Task<List<Item_delivery>> InsertItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            if (listItem_delivery == null || listItem_delivery.Count == 0)
                return new List<Item_delivery>();

            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.InsertItem_deliveryAsync(listItem_delivery);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item_delivery>> DeleteItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listItem_delivery.Where(x => x.ID == 0).Count()))
                listItem_delivery = listItem_delivery.Where(x => x.ID != 0).ToList();

            if (listItem_delivery == null || listItem_delivery.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.DeleteItem_deliveryAsync(listItem_delivery);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item_delivery>> UpdateItem_deliveryAsync(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listItem_delivery.Where(x => x.ID == 0).Count()))
                listItem_delivery = listItem_delivery.Where(x => x.ID != 0).ToList();

            if (listItem_delivery == null || listItem_delivery.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.UpdateItem_deliveryAsync(listItem_delivery);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item_delivery> GetItem_deliveryData(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = DAC.DALItem.GetItem_deliveryData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataAsync(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.GetItem_deliveryDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item_delivery> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = DAC.DALItem.GetItem_deliveryDataByDeliveryList(deliveryList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryListAsync(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.GetItem_deliveryDataByDeliveryListAsync(deliveryList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item_delivery> GetItem_deliveryDataById(int id)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = DAC.DALItem.GetItem_deliveryDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Item_delivery> searchItem_delivery(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = DAC.DALItem.searchItem_delivery(Item_delivery, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Item_delivery>> searchItem_deliveryAsync(Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.searchItem_deliveryAsync(Item_delivery, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        #endregion

        #region [ Auto_ref ]

        public async Task<List<Auto_ref>> InsertAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            if (listAuto_ref == null || listAuto_ref.Count == 0)
                return new List<Auto_ref>();

            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.InsertAuto_refAsync(listAuto_ref);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Auto_ref>> DeleteAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listAuto_ref.Where(x => x.ID == 0).Count()))
                listAuto_ref = listAuto_ref.Where(x => x.ID != 0).ToList();

            if (listAuto_ref == null || listAuto_ref.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.DeleteAuto_refAsync(listAuto_ref);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Auto_ref>> UpdateAuto_refAsync(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listAuto_ref.Where(x => x.ID == 0).Count()))
                listAuto_ref = listAuto_ref.Where(x => x.ID != 0).ToList();

            if (listAuto_ref == null || listAuto_ref.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.UpdateAuto_refAsync(listAuto_ref);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Auto_ref> GetAuto_refData(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = DAC.DALItem.GetAuto_refData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Auto_ref>> GetAuto_refDataAsync(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.GetAuto_refDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Auto_ref> GetAuto_refDataById(int id)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = DAC.DALItem.GetAuto_refDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = DAC.DALItem.searchAuto_ref(Auto_ref, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Auto_ref>> searchAuto_refAsync(Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.searchAuto_refAsync(Auto_ref, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        #endregion

        #region [ Tax_item ]

        public async Task<List<Tax_item>> InsertTax_itemAsync(List<Tax_item> listTax_item)
        {
            if (listTax_item == null || listTax_item.Count == 0)
                return new List<Tax_item>();

            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.InsertTax_itemAsync(listTax_item);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Tax_item>> DeleteTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listTax_item.Where(x => x.ID == 0).Count()))
                listTax_item = listTax_item.Where(x => x.ID != 0).ToList();

            if (listTax_item == null || listTax_item.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.DeleteTax_itemAsync(listTax_item);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Tax_item>> UpdateTax_itemAsync(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(listTax_item.Where(x => x.ID == 0).Count()))
                listTax_item = listTax_item.Where(x => x.ID != 0).ToList();

            if (listTax_item == null || listTax_item.Count == 0)
                return result;

            try
            {
                result = await DAC.DALItem.UpdateTax_itemAsync(listTax_item);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Tax_item> GetTax_itemData(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = DAC.DALItem.GetTax_itemData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataAsync(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.GetTax_itemDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Tax_item> GetTax_itemDataByItemList(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = DAC.DALItem.GetTax_itemDataByItemList(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataByItemListAsync(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.GetTax_itemDataByItemListAsync(itemList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Tax_item> GetTax_itemDataById(int id)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = DAC.DALItem.GetTax_itemDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public List<Tax_item> searchTax_item(Tax_item Tax_item, ESearchOption filterOperator)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = DAC.DALItem.searchTax_item(Tax_item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        public async Task<List<Tax_item>> searchTax_itemAsync(Tax_item Tax_item, ESearchOption filterOperator)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.searchTax_itemAsync(Tax_item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ITEM); }
            return result;
        }

        #endregion

        public void Dispose()
        {
            DAC.DALItem.Dispose();
        }

        private bool checkIfUpdateOrDeleteParamRepectsRequirements(int IDValues, [CallerMemberName] string functionName = null)
        {
            bool isRequirementsRespected = true;
            if (IDValues > 0)
            {
                isRequirementsRespected = false;
                Log.warning(functionName + " params (count = " + IDValues + ") with ID = 0", EErrorFrom.ITEM);
            }
            return isRequirementsRespected;
        }
    } /* end class BLItem */
}