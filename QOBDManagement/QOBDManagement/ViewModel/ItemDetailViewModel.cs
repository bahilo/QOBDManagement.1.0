using QOBDBusiness;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.ViewModel
{
    public class ItemDetailViewModel : BindBase
    {
        private readonly string _ITEMREFERENCEPREFIX = "QOBD";
        private HashSet<string> _itemFamilyList;
        private HashSet<string> _itemBrandList;
        private HashSet<string> _itemRefList;
        private HashSet<Provider> _providerList;
        private Func<object, object> _page;
        private string _title;

        //----------------------------[ Models ]------------------

        private ItemModel _selectedItemModel;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> btnValidCommand { get; set; }
        public ButtonCommand<string> btnDeleteCommand { get; set; }
        public ButtonCommand<object> SearchCommand { get; set; }
        public ButtonCommand<object> OpenFileExplorerCommand { get; set; }


        public ItemDetailViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
        }

        public ItemDetailViewModel(IMainWindowViewModel main): this()
        {
            _main = main;
            _page = _main.navigation;
            
        }



        //----------------------------[ Initialization ]------------------
        
        private void instances()
        {
            _title = "Item Description";
            _itemFamilyList = new HashSet<string>();
            _itemBrandList = new HashSet<string>();
            _itemRefList = new HashSet<string>();
            _providerList = new HashSet<Provider>();
        }

        private void instancesModel()
        {
            _selectedItemModel = new ItemModel();
        }

        private void instancesCommand()
        {
            btnValidCommand = new ButtonCommand<string>(saveItem, canSaveItem);
            btnDeleteCommand = new ButtonCommand<string>(deleteItem, canDeleteItem);
            SearchCommand = new ButtonCommand<object>(searchItem, canSearchItem);
            OpenFileExplorerCommand = new ButtonCommand<object>(getFileFromLocal, canGetFileFromLocal);
        }


        //----------------------------[ Properties ]------------------

        public HashSet<Provider> AllProviderList
        {
            get { return _providerList; }
            set { setProperty(ref _providerList, value); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public HashSet<string> ItemRefList
        {
            get { return _itemRefList; }
            set { setProperty(ref _itemRefList, value); }
        }

        public HashSet<string> FamilyList
        {
            get { return _itemFamilyList; }
            set { setProperty(ref _itemFamilyList, value); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public ItemModel SelectedItemModel
        {
            get { return _selectedItemModel; }
            set { setProperty(ref _selectedItemModel, value); }
        }

        public HashSet<string> BrandList
        {
            get { return _itemBrandList; }
            set { setProperty(ref _itemBrandList, value); }
        }



        //----------------------------[ Actions ]------------------

        private List<Provider> retrieveProviderFromProvider_item(List<Provider_item> provider_itemFoundList, int userSourceId)
        {
            List<Provider> returnResult = new List<Provider>();
            foreach (var provider_item in provider_itemFoundList)
            {
                var providerFoundList = Bl.BlItem.searchProvider(new Provider { Source = userSourceId, Name = provider_item.Provider_name }, ESearchOption.AND);
                if (providerFoundList.Count > 0)
                    foreach (var provider in providerFoundList)
                        returnResult.Add(provider);
            }
            return returnResult;
        }

        private async Task<List<Provider>> getEntryNewProvider()
        {
            List<Provider> returnResult = new List<Provider>();
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewProvider))
            {
                // Check that the new Provider doesn't exist
                var providerFoundList = Bl.BlItem.searchProvider(new Provider { Name = SelectedItemModel.TxtNewProvider }, ESearchOption.AND);
                if (providerFoundList.Count == 0)
                {
                    var provider = new Provider();
                    provider.Name = SelectedItemModel.TxtNewProvider;
                    provider.Source = Bl.BlSecurity.GetAuthenticatedUser().ID;
                    SelectedItemModel.SelectedProvider = provider;
                    var providerSavedList = await Bl.BlItem.InsertProviderAsync(new List<Provider> { provider });
                    if(providerSavedList.Count > 0)
                        returnResult.Add(providerSavedList[0]);
                }
                else
                    returnResult.Add(providerFoundList[0]);
            }
            else if (SelectedItemModel.SelectedProvider.ID != 0)
                returnResult.Add(SelectedItemModel.SelectedProvider);

            return returnResult;
        }

        private async Task<List<Provider_item>> updateProvider_itemTable(Item item, Provider provider)
        {
            // creating a new record in the table provider_item to link the item with its providers
            var provider_itemToSaveList = new List<Provider_item>();
            var provider_item = new Provider_item();
            var returnResult = new List<Provider_item>();

            provider_item.Provider_name = provider.Name;
            var provider_itemFoundList = Bl.BlItem.searchProvider_item(provider_item, ESearchOption.AND);
            provider_item.Item_ref = item.Ref;
            provider_itemToSaveList.Add(provider_item);

            if (!string.IsNullOrEmpty(provider.Name) && !string.IsNullOrEmpty(item.Ref))
                
                // Processing in case of a new Item
                if (provider_itemFoundList.Count == 0)
                    returnResult = await Bl.BlItem.InsertProvider_itemAsync(provider_itemToSaveList);

                //in case of an update
                else
                {
                    // retrieving and updating the current provider_item to update
                    var provider_itemToUpdateList =
                            from p_i in provider_itemToSaveList
                            where p_i.Provider_name == SelectedItemModel.SelectedProvider.Name
                            select new Provider_item { ID = p_i.ID, Item_ref = p_i.Item_ref, Provider_name = provider.Name };

                    // saving into database
                    returnResult = await Bl.BlItem.UpdateProvider_itemAsync(provider_itemToUpdateList.ToList());
                }

            return returnResult;
        }

        private void processEntryNewBrand()
        {
            // Checking that the new Brand doesn't exist
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewBrand))
            {
                var searchItemBrand = new Item();
                searchItemBrand.Type = SelectedItemModel.TxtNewBrand;
                var itemBrandFoundList = Bl.BlItem.searchItem(searchItemBrand, ESearchOption.AND);
                if (itemBrandFoundList.Count == 0)
                {
                    SelectedItemModel.TxtType = SelectedItemModel.TxtNewBrand;
                }
            }
        }

        private void processEntryNewFamily()
        {
            // Checking that the The new family doesn't exist
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewFamily))
            {
                var searchItemFamily = new Item();
                searchItemFamily.Type_sub = SelectedItemModel.TxtNewFamily;
                var itemFamilyFoundList = Bl.BlItem.searchItem(searchItemFamily, ESearchOption.AND);
                if (itemFamilyFoundList.Count == 0)
                {
                    SelectedItemModel.TxtType_sub = SelectedItemModel.TxtNewFamily;
                }
            }
        }

        public async void updateItemImage(List<Info> infoDataList)
        {
            Dialog.showSearch("File saving...");
            var infosToUpdateList = infoDataList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = infoDataList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfoAsync(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfoAsync(infosToCreateList);

            if (infosUpdatedList.Count == 0 && infosCreatedList.Count == 0)
            {
                string errorMessage = "Error occurred while saving the information!";
                Log.error(errorMessage, EErrorFrom.REFERENTIAL);
                await Dialog.showAsync(errorMessage);
            }

            Dialog.IsDialogOpen = false;
        }


        //----------------------------[ Event Handler ]------------------


        //----------------------------[ Action Commands ]------------------

        private async void deleteItem(string obj)
        {
            var itemFoundList = Bl.BlItem.searchItem(new Item { Ref = SelectedItemModel.TxtRef, ID = SelectedItemModel.Item.ID }, ESearchOption.AND);
            if (itemFoundList.Count > 0 && itemFoundList[0].Erasable == EItem.Yes.ToString())
            {
                Dialog.showSearch("Deleting...");
                await Bl.BlItem.DeleteProviderAsync(SelectedItemModel.ProviderList);
                var notSavedList = await Bl.BlItem.DeleteItemAsync(new List<Item> { SelectedItemModel.Item });
                if (notSavedList.Count > 0)
                    await Dialog.showAsync("Item deleted successfully!");
                Dialog.IsDialogOpen = false;
                _page(new ItemViewModel());
            }
            else
                await Dialog.showAsync("The Item "+SelectedItemModel.TxtRef+" is used in one or several order!");
        }

        private bool canDeleteItem(string arg)
        {
            bool isDelete = _main.securityCheck(QOBDCommon.Enum.EAction.Item, QOBDCommon.Enum.ESecurity._Delete);
            if (!isDelete)
                return false;

            if (SelectedItemModel == null || string.IsNullOrEmpty(SelectedItemModel.TxtRef))
                return false;

            if (SelectedItemModel.TxtErasable.Equals(EItem.No.ToString()))
                return false;            
           
            return true;
        }

        private async void saveItem(string obj)
        {
            Dialog.showSearch("Please wait while we are dealing with your request...");

            // check that the item doesn't already exist
            string newRef = "";
            var itemFoundList = await Bl.BlItem.searchItemAsync(new Item { Ref = SelectedItemModel.Item.Ref }, ESearchOption.AND);
            if (itemFoundList.Count == 0)
            {
                // creating a new reference via the automatic reference system
                var auto_reflist = await Bl.BlItem.GetAuto_refDataAsync(1);
                var auto_ref = (auto_reflist.Count > 0) ? auto_reflist[0] : new Auto_ref();
                newRef = _ITEMREFERENCEPREFIX + auto_ref.RefId;
                newRef += " : " + SelectedItemModel.TxtRef;
                auto_ref.RefId++;

                // Process the field New Brand
                processEntryNewBrand();

                // Process the field New Family
                processEntryNewFamily();

                // process the field New Provider
                var providerFoundList = await getEntryNewProvider();

                SelectedItemModel.Item.Name = SelectedItemModel.TxtName;
                SelectedItemModel.Item.Ref = newRef;
                SelectedItemModel.Item.Source = Bl.BlSecurity.GetAuthenticatedUser().ID;
                SelectedItemModel.Item.Erasable = EItem.Yes.ToString();
                var itemSavedList = await Bl.BlItem.InsertItemAsync(new List<Item> { SelectedItemModel.Item });
                                
                if (auto_ref.ID == 0)
                        await Bl.BlItem.InsertAuto_refAsync(new List<Auto_ref> { auto_ref });
                else
                    await Bl.BlItem.UpdateAuto_refAsync(new List<Auto_ref> { auto_ref }); 

                var provider_itemResultList = await updateProvider_itemTable(itemSavedList[0], ((providerFoundList.Count > 0) ? providerFoundList[0] : new Provider()));
                SelectedItemModel.ProviderList = retrieveProviderFromProvider_item(provider_itemResultList, SelectedItemModel.Item.Source);

                if (itemSavedList.Count > 0)
                {
                    SelectedItemModel.Item = itemSavedList[0];
                    await Dialog.showAsync("Item has been created successfully!");
                }                    
            }

            // Otherwise update the current item
            else
            {
                var savedProviderList = await Bl.BlItem.UpdateProviderAsync(await getEntryNewProvider());
                var savedItemList = await Bl.BlItem.UpdateItemAsync(new List<Item> { SelectedItemModel.Item });
                var savedProvider_itemList = await updateProvider_itemTable(savedItemList[0], ((savedProviderList.Count > 0) ? savedProviderList[0] : new Provider()));
                
                // update of the item providers of the selected item
                SelectedItemModel.ProviderList = retrieveProviderFromProvider_item(savedProvider_itemList, SelectedItemModel.Item.Source);

                if (savedItemList.Count > 0)
                    await Dialog.showAsync("Item has been updated successfully!");
            }

            Dialog.IsDialogOpen = false;
            _main.ItemViewModel.checkBoxToCartCommand.raiseCanExecuteActionChanged();
            OpenFileExplorerCommand.raiseCanExecuteActionChanged();
        }

        private bool canSaveItem(string arg)
        {
            bool isUpdate = _main.securityCheck(QOBDCommon.Enum.EAction.Item, QOBDCommon.Enum.ESecurity._Update);
            bool isWrite = _main.securityCheck(QOBDCommon.Enum.EAction.Item, QOBDCommon.Enum.ESecurity._Write);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        private void searchItem(object obj)
        {
            var itemFoundList = Bl.BlItem.searchItem(new Item { Ref = SelectedItemModel.Item.Ref }, ESearchOption.AND);
            if (itemFoundList.Count > 0)
            {
                ItemModel itemModel = new ItemModel();
                itemModel.Item = itemFoundList[0];
                SelectedItemModel = itemModel;
            }
        }

        private bool canSearchItem(object arg)
        {
            return true;
        }

        private async void getFileFromLocal(object obj)
        {
            Dialog.showSearch("Picture updating...");

            // closing the image file if opened in the order detail
            if (_main.OrderViewModel != null
                && _main.OrderViewModel.OrderDetailViewModel != null
                && _main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList != null)
            {
                var imageFound = _main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList.Where(x => x.TxtItem_ref == SelectedItemModel.TxtRef).Select(x => x.ItemModel.Image).SingleOrDefault();
                if (imageFound != null)
                    imageFound.closeImageSource();
            }

            // generating new image
            SelectedItemModel.Image = _main.ItemViewModel.loadPicture(SelectedItemModel, Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, ESearchOption.AND));
            
            // opening the file explorer for image file choosing
            SelectedItemModel.Image.TxtChosenFile = InfoManager.ExecuteOpenFileDialog("Select an image file", new List<string> { "png", "jpeg", "jpg" });
            SelectedItemModel.TxtPicture = SelectedItemModel.Image.TxtFileName;

            // upload the image file to the FTP server
            SelectedItemModel.Image.uploadImage();

            // update item image
            var savedItemList = await Bl.BlItem.UpdateItemAsync(new List<Item> { SelectedItemModel.Item });

            if (savedItemList.Count > 0)
                await Dialog.showAsync("The picture has been saved successfully!");
            else
            {
                string errorMessage = "Error occured while updating the item ["+SelectedItemModel.TxtRef+"] picture";
                Log.error(errorMessage, EErrorFrom.ITEM);
                await Dialog.showAsync(errorMessage);
            }               

            Dialog.IsDialogOpen = false;
        }

        private bool canGetFileFromLocal(object arg)
        {
            if (SelectedItemModel != null && !string.IsNullOrEmpty(SelectedItemModel.TxtRef) && SelectedItemModel.TxtRef.StartsWith(_ITEMREFERENCEPREFIX))
                return true;

            return false;
        }

    }
}
