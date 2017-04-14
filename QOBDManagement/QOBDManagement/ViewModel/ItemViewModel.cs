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
            _title = "Catalog Management";
            _cart = new Cart();
            _cbSearchCriteriaList = new List<string>();
            _items = new List<Item>();
        }

        private void instancesModel(IMainWindowViewModel main)
        {
            _itemModel = new ItemModel();
            _itemsModel = new List<ItemModel>();
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

        public List<ItemModel> ItemModelList
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

        /// <summary>
        /// loading the catalogue's items from cache
        /// </summary>
        public void loadItems()
        {
            Dialog.showSearch("Loading...");

            // if not in searching mode
            if (!_isSearchResult)
            {
                var itemFoundList = Bl.BlItem.GetItemData(999);
                ItemDetailViewModel.AllProviderList = new HashSet<Provider>(Bl.BlItem.GetProviderData(999));

                // close items picture file before reloading
                foreach (var itemModel in ItemModelList)
                    itemModel.Image.closeImageSource();

                // loading items
                ItemModelList = itemListToModelViewList(itemFoundList);

                FamilyList = new HashSet<string>(itemFoundList.Select(x=> x.Type_sub).ToList());
                BrandList = new HashSet<string>(itemFoundList.Select(x=>x.Type).ToList());
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

        public List<ItemModel> itemListToModelViewList(List<Item> itemtList)
        {
            List<ItemModel> output = new List<ItemModel>();
            ItemDetailViewModel.ItemRefList = new HashSet<string>();
            var familyList = new HashSet<string>();
            var brandList = new HashSet<string>();

            var infoList = Bl.BlReferential.searchInfo(new Info { Name = "ftp_"}, ESearchOption.AND);
            Info usernameInfo = infoList.Where(x => x.Name == "ftp_login").FirstOrDefault() ?? new Info();
            Info passwordInfo = infoList.Where(x => x.Name == "ftp_password").FirstOrDefault() ?? new Info();

            foreach (var item in itemtList)
            {
                ItemModel ivm = new ItemModel();

                ivm.Item = item;
                Provider_item searchProvider_item = new Provider_item();

                searchProvider_item.Item_ref = item.Ref;
                var provider_itemFoundList = Bl.BlItem.searchProvider_item(searchProvider_item, ESearchOption.AND);

                // getting all providers for each item
                ivm.ProviderList = loadProviderFromProvider_item(provider_itemFoundList, item.Source);

                if (ivm.ProviderList.Count > 0 && ivm.ProviderList.Count > 0 && ItemDetailViewModel.AllProviderList.Where(x => x.ID == ivm.ProviderList.OrderByDescending(y => y.ID).First().ID).Count() > 0)
                    ivm.SelectedProvider = ItemDetailViewModel.AllProviderList.Where(x => x.ID == ivm.ProviderList.OrderByDescending(y => y.ID).First().ID).First();

                // select the items appearing in the cart
                if (Cart.CartItemList.Where(x => x.Item.ID == ivm.Item.ID).Count() > 0)
                    ivm.IsItemSelected = true;

                // loading the item's picture
                ivm.Image = loadPicture(ivm, infoList);
                
                output.Add(ivm);
            }

            return output;
        }

        /// <summary>
        /// loading the item's picture from ftp server
        /// </summary>
        /// <param name="imageFileName">picture filename</param>
        /// <param name="infoList">ftp credential</param>
        public InfoManager.Display loadPicture(ItemModel itemModel, List<Info> infoList)
        {           
            Info usernameInfo = infoList.Where(x => x.Name == "ftp_login").FirstOrDefault() ?? new Info();
            Info passwordInfo = infoList.Where(x => x.Name == "ftp_password").FirstOrDefault() ?? new Info();

            if (infoList.Count > 0 && ItemModel != null)
            {
                string fileName = itemModel.TxtRef.Replace(' ', '_').Replace(':', '_');

                // closing item picture file before update
                if (itemModel.Image != null)
                    itemModel.Image.closeImageSource();
                else
                    itemModel.Image = new InfoManager.Display(ConfigurationManager.AppSettings["ftp_catalogue_image_folder"], ConfigurationManager.AppSettings["local_catalogue_image_folder"], usernameInfo.Value, passwordInfo.Value);

                if (!string.IsNullOrEmpty(itemModel.TxtPicture) && itemModel.TxtPicture.Split('.').Count() > 1 && !string.IsNullOrEmpty(itemModel.TxtPicture.Split('.')[0]))
                    fileName = itemModel.TxtPicture.Split('.')[0].Replace(' ', '_').Replace(':', '_');

                itemModel.Image.TxtFileNameWithoutExtension = fileName;
                itemModel.Image.FilterList = new List<string> { fileName };
                itemModel.Image.InfoDataList = new List<Info> { new Info { Name = fileName, Value = itemModel.TxtPicture } };
                itemModel.Image.downloadFile();                
            }
            return itemModel.Image;
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

        public void increaseStock(List<Order_itemModel> order_itemList)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
                increaseStock(new List<Order_itemModel> { order_itemModel }, order_itemModel.Order_Item.Quantity);
        }

        public async void increaseStock(List<Order_itemModel> order_itemList, int quantity = 0)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
            {
                var itemFound = (await Bl.BlItem.searchItemAsync(new Item { Ref = order_itemModel.TxtItem_ref }, ESearchOption.AND)).FirstOrDefault();
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

        public void decreaseStock(List<Order_itemModel> order_itemList)
        {
            foreach (Order_itemModel order_itemModel in order_itemList)
                decreaseStock(new List<Order_itemModel> { order_itemModel }, order_itemModel.Order_Item.Quantity);
        }

        public async void decreaseStock(List<Order_itemModel> order_itemList, int quantity = 0)
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

        public void updateStock(List<Order_itemModel> order_itemModelList, bool isStockReset = false)
        {
            foreach (Order_itemModel order_itemModel in order_itemModelList)
            {
                if (order_itemModel.ItemModel.Item.Stock > 0)
                {
                    if (isStockReset)
                        increaseStock(new List<Order_itemModel> { order_itemModel }, order_itemModel.Order_Item.Quantity);
                    else
                    {
                        if (order_itemModel.Order_Item.Quantity < Utility.intTryParse(order_itemModel.TxtOldQuantity))
                            increaseStock(new List<Order_itemModel> { order_itemModel }, Utility.intTryParse(order_itemModel.TxtOldQuantity) - order_itemModel.Order_Item.Quantity);
                        else if (order_itemModel.Order_Item.Quantity > Utility.intTryParse(order_itemModel.TxtOldQuantity))
                            decreaseStock(new List<Order_itemModel> { order_itemModel }, order_itemModel.Order_Item.Quantity - Utility.intTryParse(order_itemModel.TxtOldQuantity));
                    }
                }
            }
        }

        public void updateStock(List<Cart_itemModel> cart_itemModelList, bool isResetStock = false)
        {
            foreach (Cart_itemModel cart_itemModel in cart_itemModelList)
            {
                if (cart_itemModel.Item.Stock > 0)
                {
                    if (isResetStock)
                        increaseStock(new List<Order_itemModel> { new Order_itemModel { ItemModel = new ItemModel { Item = cart_itemModel.Item }, TxtQuantity = cart_itemModel.TxtQuantity } }, Utility.intTryParse(cart_itemModel.TxtOldQuantity));
                    else
                    {
                        if (Utility.intTryParse(cart_itemModel.TxtQuantity) < Utility.intTryParse(cart_itemModel.TxtOldQuantity))
                            increaseStock(new List<Order_itemModel> { new Order_itemModel { ItemModel = new ItemModel { Item = cart_itemModel.Item }, TxtQuantity = cart_itemModel.TxtQuantity } }, Utility.intTryParse(cart_itemModel.TxtOldQuantity) - Utility.intTryParse(cart_itemModel.TxtQuantity));
                        else if (Utility.intTryParse(cart_itemModel.TxtQuantity) > Utility.intTryParse(cart_itemModel.TxtOldQuantity))
                            decreaseStock(new List<Order_itemModel> { new Order_itemModel { ItemModel = new ItemModel { Item = cart_itemModel.Item }, TxtQuantity = cart_itemModel.TxtQuantity } }, Utility.intTryParse(cart_itemModel.TxtQuantity) - Utility.intTryParse(cart_itemModel.TxtOldQuantity));
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
            ItemDetailViewModel.Dispose();
            ItemSideBarViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------


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

        public void saveCartChecks(ItemModel obj)
        {
            // add new item to the cart
            if (Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID).Count() == 0)
            {
                var cart_itemModel = new Cart_itemModel();
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
            //if (arg != null && arg.Item.Stock > 0)
            return true;

            //return false;
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

        public void saveSelectedItem(ItemModel obj)
        {
            // closing the old picture file
            /*if(SelectedItemModel != null && SelectedItemModel.Image != null)
                SelectedItemModel.Image.closeImageSource();

            if (_main.OrderViewModel != null 
                && _main.OrderViewModel.OrderDetailViewModel != null 
                &&_main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList != null)
            {
                var imageFound = _main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList.Where(x=>x.TxtItem_ref == obj.TxtRef).Select(x=> x.ItemModel.Image).SingleOrDefault();
                if (imageFound != null)
                    imageFound.closeImageSource();
            }*/
            
            SelectedItemModel = obj;
            ItemSideBarViewModel.SelectedItem = SelectedItemModel;
            executeNavig("item-detail");
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
