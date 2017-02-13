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

namespace QOBDManagement.ViewModel
{
    public class ItemViewModel : BindBase, IItemViewModel
    {
        private Cart _cart;
        private List<string> _cbSearchCriteriaList;
        private Func<Object, Object> _page;
        private List<Item> _items;
        private string _searchItemName;
        private string _title;
        private bool _isSearchResult;

        //----------------------------[ Models ]------------------

        private ItemModel _itemModel;
        private IEnumerable<ItemModel> _itemsModel;
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
            Cart = _main.Cart;
            initEvents();
        }



        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            ItemDetailViewModel.PropertyChanged += onSelectedItemChange;
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }

        private void instances()
        {
            _title = "Catalog Management";
            _cart = new Cart();
            _cbSearchCriteriaList = new List<string>();
            _items = new List<Item>();
        }

        private void instancesModel(IMainWindowViewModel main)
        {
            _itemModel = new ItemModel();
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
            GetCurrentItemCommand = new ButtonCommand<ItemModel>(saveSelectedItem, canSaveSelectedItem);
            GoToQuoteCommand = new ButtonCommand<object>(gotoQuote, canGoToQuote);
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
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public ItemDetailViewModel ItemDetailViewModel
        {
            get { return _itemDetailViewModel; }
            set { setProperty(ref _itemDetailViewModel, value); }
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

        public IEnumerable<ItemModel> ItemModelList
        {
            get { return _itemsModel; }
            set { setProperty(ref _itemsModel, value); }
        }

        public HashSet<string> FamilyList
        {
            get { return ItemDetailViewModel.FamilyList; }
            set { ItemDetailViewModel.FamilyList = value; onPropertyChange("FamilyList"); }
        }

        public HashSet<string> BrandList
        {
            get { return ItemDetailViewModel.BrandList; }
            set { ItemDetailViewModel.BrandList = value; onPropertyChange("BrandList"); }
        }

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value); }
        }

        public ItemModel SelectedItemModel
        {
            get { return ItemDetailViewModel.SelectedItemModel; }
            set { ItemDetailViewModel.SelectedItemModel = value; onPropertyChange("SelectedItemModel"); }
        }


        //----------------------------[ Actions ]------------------

        public void loadItems()
        {
            Dialog.showSearch("Loading...");

            // check if search mode to display only search result
            if (!_isSearchResult)
            {
                ItemDetailViewModel.AllProviderList = new HashSet<Provider>(Bl.BlItem.GetProviderData(999));
                ItemModelList = itemListToModelViewList(Bl.BlItem.GetItemData(999));
                _cbSearchCriteriaList = new List<string>();
            }
            _isSearchResult = false;
            Dialog.IsDialogOpen = false;
        }

        public List<Item_deliveryModel> item_deliveryListToModelList(List<Item_delivery> item_deliveryList)
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

        private List<ItemModel> itemListToModelViewList(List<Item> itemtList)
        {
            List<ItemModel> output = new List<ItemModel>();
            ItemDetailViewModel.ItemRefList = new HashSet<string>();
            var familyList = new HashSet<string>();
            var brandList = new HashSet<string>();

            foreach (var item in itemtList)
            {
                ItemModel ivm = new ItemModel();

                ivm.Item = item;
                Provider_item searchProvider_item = new Provider_item();

                searchProvider_item.Item_ref = item.Ref;
                var provider_itemFoundList = Bl.BlItem.searchProvider_item(searchProvider_item, ESearchOption.AND);

                // getting all providers for each item
                ivm.ProviderList = loadProviderFromProvider_item(provider_itemFoundList, item.Source);

                if (ivm.ProviderList.Count > 0)
                    ivm.SelectedProvider = ItemDetailViewModel.AllProviderList.Where(x=> x.ID == ivm.ProviderList.OrderByDescending(y => y.ID).First().ID).First();
                
                familyList.Add(item.Type_sub);
                ItemDetailViewModel.ItemRefList.Add(item.Ref);
                brandList.Add(item.Type);
                output.Add(ivm);
            }
            FamilyList = familyList;
            BrandList = brandList;

            return output;
        }

        private List<Provider> loadProviderFromProvider_item(List<Provider_item> provider_itemFoundList, int userSourceId)
        {
            List<Provider> returnResult = new List<Provider>();
            foreach (var provider_item in provider_itemFoundList)
            {
                Provider searchProvider = new Provider();
                searchProvider.Source = userSourceId;
                searchProvider.Name = provider_item.Provider_name;
                var providerFoundList = Bl.BlItem.searchProvider(searchProvider, ESearchOption.AND);
                if (providerFoundList.Count > 0)
                    returnResult = returnResult.Concat(providerFoundList).ToList();
            }
            return returnResult;
        }

        public override void Dispose()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }
            ItemDetailViewModel.PropertyChanged -= onSelectedItemChange;
            ItemDetailViewModel.Dispose();
            ItemSideBarViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------

        private void onSelectedItemChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedItemModel"))
            {
                executeNavig("item-detail");
                ItemSideBarViewModel.SelectedItem = SelectedItemModel;
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                _startup = (_main.getObject("main") as BindBase).Startup;
                _itemDetailViewModel.Startup = Startup;
                _itemSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                _itemDetailViewModel.Dialog = Dialog;
                _itemSideBarViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------

        private async void filterItem(string obj)
        {
            Dialog.showSearch("Searching...");
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

        private void saveCartChecks(ItemModel obj)
        {
            if (Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID).Count() == 0)
            {
                var cart_itemModel = new Cart_itemModel();
                cart_itemModel.Item = obj.Item;
                cart_itemModel.TxtQuantity = 1.ToString();
                Cart.AddItem(cart_itemModel);
            }
            else
            {
                foreach (var cart_itemModel in Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID).ToList())
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
            Cart.CartItemList.Remove(obj);
            ItemModelList.Where(x => x.TxtID == obj.TxtID).Single().IsItemSelected = false;
            GoToQuoteCommand.raiseCanExecuteActionChanged();
        }

        private void saveSelectedItem(ItemModel obj)
        {
            SelectedItemModel = obj;
        }

        private bool canSaveSelectedItem(ItemModel arg)
        {
            return true;
        }


        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "item":
                    _page(this);
                    break;
                case "item-detail":
                    SelectedItemModel.IsRefModifyEnable = false;
                    _page(ItemDetailViewModel);
                    break;
                case "item-update":
                    SelectedItemModel.IsRefModifyEnable = false;
                    _page(_itemDetailViewModel);
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



    }
}
