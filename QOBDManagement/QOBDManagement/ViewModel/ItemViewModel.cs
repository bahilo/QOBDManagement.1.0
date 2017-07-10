using QOBDCommon.Entities;
using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using QOBDManagement.Classes;
using QOBDCommon.Enum;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using QOBDManagement.Models;
using System.Threading;
using System.Diagnostics;
using QOBDManagement.Interfaces;
using System.Windows.Threading;
using System.Globalization;
using QOBDCommon.Classes;
using System.Configuration;
using QOBDManagement.Helper;
using System.Windows;
using System.IO;

namespace QOBDManagement.ViewModel
{
    public class ItemViewModel : BindBase, IItemViewModel
    {
        private HashSet<string> _itemFamilyList;
        private HashSet<string> _itemBrandList;
        private List<ProviderModel> _providerList;
        private List<string> _cbSearchCriteriaList;
        private Func<Object, Object> _page;
        private List<Item> _items;
        private string _searchItemName;
        private string _title;
        private string _providerTitle;
        private bool _isSearchResult;
        private ProviderModel _selectedProviderModel;

        //----------------------------[ Models ]------------------

        private ItemModel _itemModel;
        private List<ItemModel> _itemsModel;
        private ItemDetailViewModel _itemDetailViewModel;
        private ItemSideBarViewModel _itemSideBarViewModel;
        private IMainWindowViewModel _main;


        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> checkBoxSearchCommand { get; set; }
        public ButtonCommand<ItemModel> checkBoxToCartCommand { get; set; }
        public ButtonCommand<string> btnSearchCommand { get; set; }
        public ButtonCommand<Cart_itemModel> DeleteFromCartCommand { get; set; }
        public ButtonCommand<string> NavigCommand { get; set; }
        public ButtonCommand<ItemModel> GetCurrentItemCommand { get; set; }
        public ButtonCommand<object> GoToQuoteCommand { get; set; }
        public ButtonCommand<object> ClearCartCommand { get; set; }
        public ButtonCommand<ProviderModel> BtnProviderSearchCommand { get; set; }
        public ButtonCommand<object> BtnAddProviderAddressCommand { get; set; }
        public ButtonCommand<object> BtnDeleteProviderAddressCommand { get; set; }
        public ButtonCommand<Address> SelectedAddressDetailCommand { get; set; }
        public ButtonCommand<object> btnDeleteProviderCommand { get; set; }
        public ButtonCommand<object> btnValidProviderCommand { get; set; }
        public ButtonCommand<object> btnClearSelectedProviderCommand { get; set; }


        public ItemViewModel()
        {
            instances();
            instancesCommand();
        }

        public ItemViewModel(IMainWindowViewModel mainWindowViewModel) : this()
        {
            this._main = mainWindowViewModel;
            _page = _main.navigation;
            instancesModel(mainWindowViewModel);
            initEvents();
        }

        public ItemViewModel(IMainWindowViewModel mainWindowViewModel, IStartup startup, IConfirmationViewModel dialog) : this(mainWindowViewModel)
        {
            this.Startup = startup;
            this.Dialog = dialog;

            _itemDetailViewModel.Dialog = Dialog;
            _itemSideBarViewModel.Dialog = Dialog;
            _itemDetailViewModel.Startup = Startup;
            _itemSideBarViewModel.Startup = Startup;
        }



        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
        }

        private void instances()
        {
            _items = new List<Item>();
            _cbSearchCriteriaList = new List<string>();
            _title = ConfigurationManager.AppSettings["title_catalogue"];
            _providerTitle = ConfigurationManager.AppSettings["title_catalogue_provider"];
        }

        private void instancesModel(IMainWindowViewModel main)
        {
            _itemModel = new ItemModel();
            _itemsModel = new List<ItemModel>();
            _selectedProviderModel = new ProviderModel();
            _itemDetailViewModel = new ItemDetailViewModel(main);
            _itemSideBarViewModel = new ItemSideBarViewModel(main);
        }

        private void instancesCommand()
        {
            checkBoxSearchCommand = new ButtonCommand<string>(saveSearchChecks, canSaveSearchChecks);
            checkBoxToCartCommand = new ButtonCommand<ItemModel>(saveCartChecks, canSaveCartChecks);
            btnSearchCommand = new ButtonCommand<string>(filterItem, canFilterItem);
            DeleteFromCartCommand = new ButtonCommand<Cart_itemModel>(deleteItemFromCart, canDeleteItemFromCart);
            NavigCommand = new ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentItemCommand = new ButtonCommand<ItemModel>(showSelectedItem, canShowSelectedItem);
            GoToQuoteCommand = new ButtonCommand<object>(gotoQuote, canGoToQuote);
            ClearCartCommand = new ButtonCommand<object>(clearCart, canClearTheCart);
            BtnProviderSearchCommand = new ButtonCommand<ProviderModel>(searchProvider, canSearchProvider);
            BtnAddProviderAddressCommand = new ButtonCommand<object>(addProviderAddress, canAddProviderAddress);
            BtnDeleteProviderAddressCommand = new ButtonCommand<object>(deleteProviderAddress, canDeleteProviderAddress);
            SelectedAddressDetailCommand = new ButtonCommand<Address>(showSelectedAddressDetail, canShowSelectedAddressDetail);
            btnDeleteProviderCommand = new ButtonCommand<object>(deleteProvider, canDeleteProvider);
            btnValidProviderCommand = new ButtonCommand<object>(saveProvider, canSaveProvider);
            btnClearSelectedProviderCommand = new ButtonCommand<object>(clearSelectedProvider, canClearProvider);
        }

        //----------------------------[ Properties ]------------------

        public string SearchItemName
        {
            get { return _searchItemName; }
            set { setProperty(ref _searchItemName, value); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public string ProviderTitle
        {
            get { return _providerTitle; }
            set { setProperty(ref _providerTitle, value); }
        }

        public ItemDetailViewModel ItemDetailViewModel
        {
            get { return _itemDetailViewModel; }
            set { setProperty(ref _itemDetailViewModel, value); }
        }

        public ProviderModel SelectedProviderModel
        {
            get { return _selectedProviderModel; }
            set { setProperty(ref _selectedProviderModel, value); }
        }

        public ItemSideBarViewModel ItemSideBarViewModel
        {
            get { return _itemSideBarViewModel; }
            set { setProperty(ref _itemSideBarViewModel, value); }
        }

        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value); }
        }

        public List<Item> Items
        {
            get { return _items; }
            set { setProperty(ref _items, value); }
        }

        public List<ItemModel> ItemModelList
        {
            get { return _itemsModel; }
            set
            {
                if (Application.Current != null && !Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _itemsModel = value;
                        onPropertyChange("ItemModelList");
                    });
                }
                else
                    setProperty(ref _itemsModel, value);
            }
        }

        public List<ProviderModel> ProviderList
        {
            get { return _providerList; }
            set
            {
                if (Application.Current != null && !Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _providerList = value;
                        onPropertyChange("ProviderList");
                    });
                }
                else { _providerList = value; onPropertyChange(); }
            }
        }

        public List<ProviderModel> ItemProviderList
        {
            get { return _providerList; }
            set
            {
                if (Application.Current != null && !Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _providerList = value;
                        onPropertyChange("ItemProviderList");
                    });
                }
                else { _providerList = value; onPropertyChange(); }
            }
        }

        public HashSet<string> FamilyList
        {
            get { return _itemFamilyList; }
            set
            {
                if (Application.Current != null && !Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _itemFamilyList = value;
                        onPropertyChange("FamilyList");
                    });
                }
                else { _itemFamilyList = value; onPropertyChange(); }
            }
        }

        public HashSet<string> BrandList
        {
            get { return _itemBrandList; }
            set
            {
                if (Application.Current != null && !Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _itemBrandList = value;
                        onPropertyChange();
                    });
                }
                else { _itemBrandList = value; onPropertyChange("BrandList"); }
            }
        }

        public Cart Cart
        {
            get { return (_main != null) ? _main.Cart : new Cart(); }
        }

        public ItemModel SelectedItemModel
        {
            get { return ItemDetailViewModel.SelectedItemModel; }
            set { ItemDetailViewModel.SelectedItemModel = value; onPropertyChange(); }
        }

        public List<CurrencyModel> CurrenciesList
        {
            get { return _main.OrderViewModel.CurrenciesList; }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _main.OrderViewModel.CurrenciesList.Where(x => x.IsDefault).FirstOrDefault() ?? new CurrencyModel(); }
        }

        public List<CurrencyModel> ItemCurrenciesList
        {
            get { return _main.OrderViewModel.CurrenciesList; }
        }

        public string BoxVisibility
        {
            get { return _main.OrderViewModel.OrderDetailViewModel.BoxVisibility; }
        }


        //----------------------------[ Actions ]------------------

        /// <summary>
        /// loading the catalogue's items from cache
        /// </summary>
        public async void loadItems()
        {
            await Task.Factory.StartNew(() =>
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["load_message"]);

                // if not in searching mode
                if (!_isSearchResult)
                {
                    var itemFoundList = Bl.BlItem.GetItemData(999);
                    ProviderList = providerListToModelList(Bl.BlItem.GetProviderData(999));
                    FamilyList = new HashSet<string>(itemFoundList.Select(x => x.Type_sub).ToList());
                    BrandList = new HashSet<string>(itemFoundList.Select(x => x.Type).ToList());

                    // close items picture file before reloading
                    foreach (var itemModel in ItemModelList)
                    {
                        if (itemModel.Image != null)
                            itemModel.Image.closeImageSource();
                    }

                    // loading items
                    ItemModelList = itemListToModelViewList(itemFoundList);

                    // update the selected item in case of a refresh
                    if (SelectedItemModel != null && SelectedItemModel.Item.ID != 0)
                    {
                        SelectedItemModel = ItemModelList.Where(x => x.TxtID == SelectedItemModel.TxtID).SingleOrDefault();
                        if (SelectedItemModel != null)
                        {
                            SelectedItemModel.PropertyChanged -= ItemDetailViewModel.onItemNameChange_generateReference;
                            SelectedItemModel.PropertyChanged += ItemDetailViewModel.onItemNameChange_generateReference;
                            //onPropertyChange("TxtType");
                            //onPropertyChange("TxtType_sub");
                            //onPropertyChange("SelectedProvider");                            
                        }
                    }
                    onPropertyChange("CurrencyModel");
                    _cbSearchCriteriaList = new List<string>();
                }
                _isSearchResult = false;
                Dialog.IsDialogOpen = false;
            });
        }

        private List<ProviderModel> providerListToModelList(List<Provider> providerList)
        {
            object _lock = new object();
            lock (_lock)
            {
                List<ProviderModel> output = new List<ProviderModel>();
                foreach (Provider provider in providerList)
                {
                    ProviderModel providerModel = new ProviderModel();
                    providerModel.Provider = provider;

                    var addressesFound = Bl.BlClient.searchAddress(new Address { ProviderId = provider.ID }, ESearchOption.AND);
                    if (addressesFound.Count > 0)
                    {
                        providerModel.AddressList = new ObservableCollection<Address>(addressesFound);
                        providerModel.SelectedAddress = addressesFound.First();
                    }

                    output.Add(providerModel);
                }
                return output;
            }
        }

        public List<Item_deliveryModel> item_deliveryListToModelList(List<Item_delivery> item_deliveryList)
        {
            object _lock = new object();
            lock (_lock)
            {
                List<Item_deliveryModel> output = new List<Item_deliveryModel>();
                foreach (var item_delivery in item_deliveryList)
                {
                    Item_deliveryModel idm = new Item_deliveryModel();
                    idm.Item_delivery = item_delivery;
                    var deliveryList = new DeliveryModel().DeliveryListToModelViewList(Bl.BlOrder.searchDelivery(new Delivery { ID = item_delivery.DeliveryId }, ESearchOption.AND));
                    idm.DeliveryModel = (deliveryList.Count > 0) ? deliveryList[0] : new DeliveryModel();
                    output.Add(idm);
                }
                return output;
            }
        }

        public List<ItemModel> itemListToModelViewList(List<Item> itemtList)
        {
            object _lock = new object();
            lock (_lock)
            {
                List<ItemModel> output = new List<ItemModel>();
                var ftpCredentials = Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, ESearchOption.AND);

                foreach (var item in itemtList)
                {
                    ItemModel ivm = new ItemModel();

                    ivm.Item = item;

                    var provider_itemFoundList = Bl.BlItem.searchProvider_item(new Provider_item { ItemId = item.ID }, ESearchOption.AND);

                    // getting all providers for each item
                    ivm.Provider_itemModelList = loadProvider_itemInformation(provider_itemFoundList, item.Source);

                    // selecting one provider among the item providers
                    var providerFoundList = ProviderList.Where(x => ivm.Provider_itemModelList.Where(y => y.Provider.ID == x.Provider.ID).Count() > 0).ToList();
                    {
                        ivm.SelectedProvider = providerFoundList.First();
                        ivm.SelectedProvider_itemModel = ivm.Provider_itemModelList.Where(x => x.Provider.ID == ivm.SelectedProvider.Provider.ID).First();
                    }

                    // select the items appearing in the cart
                    if (Cart.CartItemList.Where(x => x.Item.ID == ivm.Item.ID).Count() > 0)
                        ivm.IsItemSelected = true;

                    // loading the item's picture
                    downloadImage(ivm, ftpCredentials);

                    //ivm.Image = await Task.Factory.StartNew(() => { return ivm.Image.downloadPicture(ConfigurationManager.AppSettings["ftp_catalogue_image_folder"], ConfigurationManager.AppSettings["local_catalogue_image_folder"], ivm.TxtPicture, ivm.TxtRef.Replace(' ', '_').Replace(':', '_'), ftpCredentials); });

                    output.Add(ivm);
                }

                return output;
            }
        }

        private async void downloadImage(ItemModel itemModel, List<Info> ftpCredentials)
        {
            object _lock = new object();
            var image = await Task.Factory.StartNew(() => { return itemModel.Image.downloadPicture(ConfigurationManager.AppSettings["ftp_catalogue_image_folder"], ConfigurationManager.AppSettings["local_catalogue_image_folder"], itemModel.TxtPicture, itemModel.TxtRef.Replace(' ', '_').Replace(':', '_'), ftpCredentials); });
            lock (_lock)
            {
                itemModel.Image = image;
            }
        }

        public List<Provider_itemModel> loadProvider_itemInformation(List<Provider_item> provider_itemFoundList, int userSourceId)
        {
            object _lock = new object();
            lock (_lock)
            {
                List<Provider_itemModel> returnResult = new List<Provider_itemModel>();
                _main.OrderViewModel.loadCurrencies();

                foreach (var provider_item in provider_itemFoundList)
                {
                    var provider_itemModel = new Provider_itemModel();
                    provider_itemModel.Provider_item = provider_item;

                    // getting the item provider information
                    var providerFoundList = Bl.BlItem.searchProvider(new Provider { ID = provider_item.ProviderId, Source = userSourceId }, ESearchOption.AND);
                    if (providerFoundList.Count > 0)
                        provider_itemModel.Provider = providerFoundList[0];

                    // getting the item currency information
                    //var currencyFoundList = Bl.BlOrder.searchCurrency(new Currency { ID = provider_item.CurrencyId }, ESearchOption.AND);
                    var currencyFoundList = _main.OrderViewModel.CurrenciesList.Where(x => x.Currency.ID == provider_item.CurrencyId).ToList();
                    if (currencyFoundList.Count > 0)
                        provider_itemModel.CurrencyModel = currencyFoundList.First();

                    //if (_main.OrderViewModel.CurrenciesList.Where(x => x.Currency.ID == provider_item.CurrencyId).Count() > 0)
                    //    provider_itemModel.CurrencyModel = _main.OrderViewModel.CurrenciesList.Where(x => x.Currency.ID == provider_item.CurrencyId).First();

                    returnResult.Add(provider_itemModel);
                }
                return returnResult;
            }
        }

        public async Task increaseStockAsync(List<Order_itemModel> order_itemList)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
                await increaseStockAsync(new List<Order_itemModel> { order_itemModel }, order_itemModel.Order_Item.Quantity);
        }

        public async Task increaseStockAsync(List<Order_itemModel> order_itemList, int quantity = 0)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
            {
                var itemFound = (Bl.BlItem.searchItem(new Item { Ref = order_itemModel.TxtItem_ref }, ESearchOption.AND)).SingleOrDefault();
                if (itemFound != null)
                {
                    if (quantity > 0)
                        order_itemModel.ItemModel.Item.Stock = itemFound.Stock + quantity;
                    else
                        order_itemModel.ItemModel.Item.Stock = itemFound.Stock + order_itemModel.Order_Item.Quantity;
                }
            }

            await Bl.BlItem.UpdateItemAsync(order_itemList.Select(x => x.ItemModel.Item).ToList());
        }

        public async Task decreaseStockAsync(List<Order_itemModel> order_itemList)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
                await decreaseStockAsync(new List<Order_itemModel> { order_itemModel }, order_itemModel.Order_Item.Quantity);
        }

        public async Task decreaseStockAsync(List<Order_itemModel> order_itemList, int quantity = 0)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
            {
                var itemFound = (await Bl.BlItem.searchItemAsync(new Item { Ref = order_itemModel.TxtItem_ref }, ESearchOption.AND)).FirstOrDefault();
                if (itemFound != null)
                {
                    if ((itemFound.Stock - order_itemModel.Order_Item.Quantity) > 0)
                    {
                        if (quantity > 0)
                            order_itemModel.ItemModel.Item.Stock = itemFound.Stock - quantity;
                        else
                            order_itemModel.ItemModel.Item.Stock = itemFound.Stock - order_itemModel.Order_Item.Quantity;
                    }
                }
            }

            await Bl.BlItem.UpdateItemAsync(order_itemList.Select(x => x.ItemModel.Item).ToList());
        }

        public async Task updateStockAsync(List<Order_itemModel> order_itemModelList, bool isStockReset = false)
        {
            foreach (Order_itemModel order_itemModel in order_itemModelList)
            {
                if (order_itemModel.ItemModel.Item.Stock > 0)
                {
                    if (isStockReset)
                        await increaseStockAsync(new List<Order_itemModel> { order_itemModel }, Utility.intTryParse(order_itemModel.TxtQuantity_delivery));
                    else
                    {
                        if (order_itemModel.Order_Item.Quantity < Utility.intTryParse(order_itemModel.TxtOldQuantity))
                            await increaseStockAsync(new List<Order_itemModel> { order_itemModel }, Utility.intTryParse(order_itemModel.TxtQuantity_current));
                        else if (order_itemModel.Order_Item.Quantity > Utility.intTryParse(order_itemModel.TxtOldQuantity))
                            await decreaseStockAsync(new List<Order_itemModel> { order_itemModel }, Utility.intTryParse(order_itemModel.TxtQuantity_current));
                    }
                }
            }
        }

        public async Task updateStockAsync(List<Cart_itemModel> cart_itemModelList, bool isResetStock = false)
        {
            foreach (Cart_itemModel cart_itemModel in cart_itemModelList)
            {
                if (cart_itemModel.Item.Stock > 0)
                {
                    if (isResetStock)
                        await increaseStockAsync(new List<Order_itemModel> { new Order_itemModel { ItemModel = new ItemModel { Item = cart_itemModel.Item }, TxtQuantity = cart_itemModel.TxtQuantity } }, Utility.intTryParse(cart_itemModel.TxtOldQuantity));
                    else
                    {
                        if (Utility.intTryParse(cart_itemModel.TxtQuantity) < Utility.intTryParse(cart_itemModel.TxtOldQuantity))
                            await increaseStockAsync(new List<Order_itemModel> { new Order_itemModel { ItemModel = new ItemModel { Item = cart_itemModel.Item }, TxtQuantity = cart_itemModel.TxtQuantity } }, Utility.intTryParse(cart_itemModel.TxtOldQuantity) - Utility.intTryParse(cart_itemModel.TxtQuantity));
                        else if (Utility.intTryParse(cart_itemModel.TxtQuantity) > Utility.intTryParse(cart_itemModel.TxtOldQuantity))
                            await decreaseStockAsync(new List<Order_itemModel> { new Order_itemModel { ItemModel = new ItemModel { Item = cart_itemModel.Item }, TxtQuantity = cart_itemModel.TxtQuantity } }, Utility.intTryParse(cart_itemModel.TxtQuantity) - Utility.intTryParse(cart_itemModel.TxtOldQuantity));
                    }
                }
            }
        }

        public async Task<bool> checkIfStockAvailable(Order_itemModel order_itemModel)
        {
            bool isStockAvailable = false;
            var itemFound = (await Bl.BlItem.searchItemAsync(new Item { Ref = order_itemModel.TxtItem_ref }, ESearchOption.AND)).FirstOrDefault();
            if (itemFound != null && itemFound.Stock >= order_itemModel.Order_Item.Quantity - Utility.intTryParse(order_itemModel.TxtOldQuantity))
                isStockAvailable = true;

            return isStockAvailable;
        }

        public async Task<bool> checkIfStockAvailable(Cart_itemModel cart_itemModel)
        {
            bool isStockAvailable = false;
            var itemFound = (await Bl.BlItem.searchItemAsync(new Item { Ref = cart_itemModel.TxtRef }, ESearchOption.AND)).FirstOrDefault();
            if (itemFound != null && itemFound.Stock >= Utility.intTryParse(cart_itemModel.TxtQuantity) - Utility.intTryParse(cart_itemModel.TxtOldQuantity))
                isStockAvailable = true;

            return isStockAvailable;
        }

        public override void Dispose()
        {
            try
            {
                ItemDetailViewModel.Dispose();
                ItemSideBarViewModel.Dispose();
                foreach (var itemModel in ItemModelList)
                {
                    if (itemModel.Image != null)
                        itemModel.Image.Dispose();
                }
            }
            catch (Exception)
            { }
        }

        //----------------------------[ Action Commands ]------------------

        private async void filterItem(string obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["wait_message"]);
            ItemModel itemModel = new ItemModel();
            List<Item> results = new List<Item>();
            ESearchOption filterOperator;
            itemModel.TxtID = obj;
            itemModel.TxtRef = obj;
            itemModel.TxtName = ItemModel.TxtName;
            itemModel.TxtType = ItemModel.SelectedBrand;
            itemModel.TxtType_sub = ItemModel.SelectedFamily;

            if (ItemModel.IsExactMatch) { filterOperator = ESearchOption.AND; }
            else { filterOperator = ESearchOption.OR; }

            if (ItemModel.IsDeepSearch) { results = await Bl.BlItem.searchItemAsync(itemModel.Item, filterOperator); }
            else { results = Bl.BlItem.searchItem(itemModel.Item, filterOperator); }

            if (ItemModel.IsSearchByItemName) { results = results.Where(x => x.Name.IndexOf(obj, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList(); }

            ItemModelList = itemListToModelViewList(results);
            _isSearchResult = true;

            ItemModel.SelectedBrand = null;
            ItemModel.SelectedFamily = null;
            ItemModel.IsExactMatch = false;
            ItemModel.IsDeepSearch = false;
            ItemModel.IsSearchByItemName = false;
            ItemModel.TxtName = "";
            Dialog.IsDialogOpen = false;
            //_main.IsRefresh = true;
            _page(this);
        }

        public void saveSearchChecks(string obj)
        {
            if (!_cbSearchCriteriaList.Contains(obj))
            { _cbSearchCriteriaList.Add(obj); }
            else { _cbSearchCriteriaList.Remove(obj); }
        }

        private bool canSaveSearchChecks(string arg)
        {
            return true;
        }

        public void saveCartChecks(ItemModel obj)
        {
            // add new item to the cart
            if (Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID).Count() == 0)
            {
                var cart_itemModel = new Cart_itemModel();
                obj.IsItemSelected = true;
                cart_itemModel.Item = obj.Item;
                cart_itemModel.TxtQuantity = 1.ToString();
                Cart.AddItem(cart_itemModel);
            }

            // delete item from the cart
            else
            {
                // unselect item
                var itemFound = ItemModelList.Where(x => x.Item.ID == obj.Item.ID).FirstOrDefault();
                if (itemFound != null)
                    itemFound.IsItemSelected = false;

                foreach (var cart_itemModel in Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID && x.TxtRef == obj.TxtRef).ToList())
                    Cart.RemoveItem(cart_itemModel);
            }
            GoToQuoteCommand.raiseCanExecuteActionChanged();
        }

        private bool canSaveCartChecks(ItemModel arg)
        {
            return true;
        }

        private bool canFilterItem(string arg)
        {
            return true;
        }


        private bool canDeleteItemFromCart(Cart_itemModel arg)
        {
            return true;
        }

        private void deleteItemFromCart(Cart_itemModel obj)
        {
            saveCartChecks(new ItemModel { Item = obj.Item });
        }

        public void showSelectedItem(ItemModel obj)
        {
            obj.PropertyChanged -= _itemDetailViewModel.onItemNameChange_generateReference;
            obj.PropertyChanged += _itemDetailViewModel.onItemNameChange_generateReference;
            SelectedItemModel = obj;
            ItemSideBarViewModel.SelectedItem = SelectedItemModel;
            executeNavig("item-detail");
        }

        private bool canShowSelectedItem(ItemModel arg)
        {
            bool canUpdate = _main.AgentViewModel.IsAuthenticatedAgentAdmin;
            if (canUpdate)
                return true;

            return false;
        }


        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "item":
                    _page(this);
                    break;
                case "item-detail":
                    _page(ItemDetailViewModel);
                    break;
                default:
                    goto case "item";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        private void gotoQuote(object obj)
        {
            _page(new QuoteViewModel());
        }

        private bool canGoToQuote(object arg)
        {
            bool isRead = _main.securityCheck(EAction.Quote, ESecurity._Read);
            if (isRead && Cart.CartItemList.Count > 0)
                return true;
            return false;
        }

        private void clearCart(object obj)
        {
            // add item to the cart and create an event on quantity change
            foreach (var itemModel in Cart.CartItemList.Select(x => new ItemModel { Item = x.Item }).ToList())
                saveCartChecks(itemModel);

            Cart.Client.Client = new QOBDCommon.Entities.Client();
        }

        private bool canClearTheCart(object obj)
        {
            return true;
        }

        private async void saveProvider(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["wait_message"]);

            // saving the provider
            List<Provider> savedProviders = new List<Provider>();

            bool isProviderMandatoryFieldNotEmpty = !string.IsNullOrEmpty(SelectedProviderModel.TxtCompanyName)
                                                           && !string.IsNullOrEmpty(SelectedProviderModel.TxtPhone)
                                                               && !string.IsNullOrEmpty(SelectedProviderModel.TxtEmail);

            bool isAddressMandatoryFieldNotEmpty = !string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.Name)
                                                           && !string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.AddressName)
                                                               && !string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.CityName)
                                                                   && !string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.Postcode)
                                                                       && !string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.Country);

            if (SelectedProviderModel.Provider.ID == 0)
            {

                if (isProviderMandatoryFieldNotEmpty)
                {
                    savedProviders = await Bl.BlItem.InsertProviderAsync(new List<Provider> { SelectedProviderModel.Provider });
                    if (savedProviders.Count() > 0)
                        ProviderList = ProviderList.Concat(providerListToModelList(savedProviders)).ToList();
                }
                else
                    await Dialog.showAsync("[Main detail area]: Please fill up mandatory fields!");
            }
            else
                savedProviders = await Bl.BlItem.UpdateProviderAsync(new List<Provider> { SelectedProviderModel.Provider });

            // saving the provider address
            if (savedProviders.Count() > 0)
            {
                List<Address> savedAddresses = new List<Address>();

                SelectedProviderModel.SelectedAddress.ProviderId = SelectedProviderModel.Provider.ID;

                if (SelectedProviderModel.SelectedAddress.ID == 0)
                {
                    if (isAddressMandatoryFieldNotEmpty)
                    {
                        savedAddresses = await Bl.BlClient.InsertAddressAsync(new List<Address> { SelectedProviderModel.SelectedAddress });
                        if (savedAddresses.Count() > 0)
                        {
                            SelectedProviderModel.SelectedAddress = savedAddresses.First();
                            SelectedProviderModel.AddressList.Add(savedAddresses.First());
                        }
                    }
                    else if (!string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.Name))
                        await Dialog.showAsync("[Address detail area]: Please fill up mandatory fields!");
                }
                else
                    savedAddresses = await Bl.BlClient.UpdateAddressAsync(new List<Address> { SelectedProviderModel.SelectedAddress });

                if (savedAddresses.Count() > 0)
                {
                    SelectedProviderModel.Provider.AddressId = savedAddresses.First().ID;
                    await Bl.BlItem.UpdateProviderAsync(new List<Provider> { SelectedProviderModel.Provider });
                }

                if (savedAddresses.Count() > 0 || string.IsNullOrEmpty(SelectedProviderModel.SelectedAddress.Name))
                    await Dialog.showAsync("The provider has been successfully updated!");
            }

            Dialog.IsDialogOpen = false;
        }

        private bool canSaveProvider(object arg)
        {
            bool canUpdate = _main.securityCheck(EAction.Item, ESecurity._Update)
                                && _main.securityCheck(EAction.Item, ESecurity._Write);
            if (canUpdate)
                return true;
            return false;
        }

        private async void deleteProvider(object obj)
        {
            if (await Dialog.showAsync("Do you confirm the provider [" + SelectedProviderModel.TxtCompanyName + "] deletion?"))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);
                var notDeletedProviders = await Bl.BlItem.DeleteProviderAsync(new List<Provider> { SelectedProviderModel.Provider });

                var providerFound = ProviderList.Where(x => x.Provider.ID == SelectedProviderModel.Provider.ID).SingleOrDefault();
                if (notDeletedProviders.Count() == 0 && providerFound != null)
                    ProviderList.Remove(providerFound);

                Dialog.IsDialogOpen = false;
            }
        }

        private bool canDeleteProvider(object arg)
        {
            bool canDelete = _main.securityCheck(EAction.Item, ESecurity._Delete);
            if (canDelete)
                return true;
            return false;
        }

        private void showSelectedAddressDetail(Address obj)
        {
            SelectedProviderModel.SelectedAddress = obj;
        }

        private bool canShowSelectedAddressDetail(Address arg)
        {
            return true;
        }

        private async void deleteProviderAddress(object obj)
        {
            if (await Dialog.showAsync("Do you confirm the address [" + SelectedProviderModel.SelectedAddress.Name2 + "] deletion?"))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);
                await Bl.BlClient.DeleteAddressAsync(new List<Address> { SelectedProviderModel.SelectedAddress });
                var addressFound = SelectedProviderModel.AddressList.Where(x => x.ID == SelectedProviderModel.SelectedAddress.ID).SingleOrDefault();
                if (addressFound != null)
                {
                    SelectedProviderModel.AddressList.Remove(addressFound);

                    if (SelectedProviderModel.AddressList.Count() > 0)
                        SelectedProviderModel.SelectedAddress = SelectedProviderModel.AddressList.First();
                    else
                        SelectedProviderModel.SelectedAddress = new Address();
                }
                Dialog.IsDialogOpen = false;
            }

        }

        private bool canDeleteProviderAddress(object arg)
        {
            bool canDelete = _main.securityCheck(EAction.Item, ESecurity._Delete);
            if (canDelete)
                return true;
            return false;
        }

        private void addProviderAddress(object obj)
        {
            SelectedProviderModel.SelectedAddress = new Address();
        }

        private bool canAddProviderAddress(object arg)
        {
            bool canUpdate = _main.securityCheck(EAction.Item, ESecurity._Update)
                                && _main.securityCheck(EAction.Item, ESecurity._Write);
            if (canUpdate)
                return true;
            return false;
        }

        private void searchProvider(ProviderModel obj)
        {
            SelectedProviderModel = obj;
        }

        private bool canSearchProvider(ProviderModel arg)
        {
            return true;
        }

        private void clearSelectedProvider(object obj)
        {
            SelectedProviderModel = new ProviderModel();
        }

        private bool canClearProvider(object arg)
        {
            bool canUpdate = _main.securityCheck(EAction.Item, ESecurity._Update)
                                && _main.securityCheck(EAction.Item, ESecurity._Write);
            if (canUpdate)
                return true;
            return false;
        }



    }
}
