using QOBDBusiness;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Helper;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.ViewModel
{
    public class ItemDetailViewModel : BindBase
    {
        private string _ITEMREFERENCEPREFIX;
        private Func<object, object> _page;
        private string _title;
        private CurrencyModel _selectedCurrency;

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
            _selectedCurrency = new CurrencyModel();
            _title = ConfigurationManager.AppSettings["title_item_detail"];
            _ITEMREFERENCEPREFIX = ConfigurationManager.AppSettings["info_company_name"];          
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
        
        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
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
        
        //----------------------------[ Actions ]------------------

        private List<Provider> retrieveProviderFromProvider_item(List<Provider_item> provider_itemFoundList, int userSourceId)
        {
            List<Provider> returnResult = new List<Provider>();
            foreach (var provider_item in provider_itemFoundList)
            {
                var providerFoundList = Bl.BlItem.searchProvider(new Provider { Source = userSourceId, ID = provider_item.ProviderId }, ESearchOption.AND);
                if (providerFoundList.Count > 0)
                {
                    foreach (var provider in providerFoundList)
                        returnResult.Add(provider);
                }                    
            }
            return returnResult;
        }

        private async Task<List<ProviderModel>> getEntryNewProvider()
        {
            List<ProviderModel> returnResult = new List<ProviderModel>();
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewProvider))
            {
                // Check that the new Provider doesn't exist
                var providerFoundList = Bl.BlItem.searchProvider(new Provider { Name = SelectedItemModel.TxtNewProvider }, ESearchOption.AND);
                if (providerFoundList.Count == 0)
                {
                    var provider = new Provider();
                    provider.Name = SelectedItemModel.TxtNewProvider;
                    provider.Source = Bl.BlSecurity.GetAuthenticatedUser().ID;
                    
                    var providerSavedList = await Bl.BlItem.InsertProviderAsync(new List<Provider> { provider });
                    if(providerSavedList.Count > 0)
                    {
                        SelectedItemModel.SelectedProvider = new ProviderModel { Provider = providerSavedList.First() };
                        returnResult.Add(new ProviderModel { Provider = providerSavedList.First() });
                    }                        
                }
                else
                    returnResult.Add(new ProviderModel { Provider = providerFoundList.First() });
            }
            else if (SelectedItemModel.SelectedProvider.Provider.ID != 0)
                returnResult.Add(SelectedItemModel.SelectedProvider);

            return returnResult;
        }

        private async Task<List<Provider_itemModel>> updateProvider_itemTable()
        {
            // creating a new record in the table provider_item to link the item with its providers
            var returnResult = new List<Provider_itemModel>();

            var provider_itemFoundList = Bl.BlItem.searchProvider_item(new Provider_item { ProviderId = SelectedItemModel.SelectedProvider.Provider.ID }, ESearchOption.AND);
            var provider_itemModelFoundList = _main.ItemViewModel.loadProvider_itemInformation(provider_itemFoundList, SelectedItemModel.Item.Source);
            
            if (!string.IsNullOrEmpty(SelectedItemModel.SelectedProvider.TxtCompanyName) && !string.IsNullOrEmpty(SelectedItemModel.TxtRef))
            {
                // Processing in case of a new Item
                if (provider_itemModelFoundList.Count == 0 || provider_itemModelFoundList.Where(x => x.Provider_item.ItemId == SelectedItemModel.Item.ID).Count() == 0)
                {
                    returnResult = (await Bl.BlItem.InsertProvider_itemAsync(new List<Provider_item> { new Provider_item { ItemId = SelectedItemModel.Item.ID, ProviderId = SelectedItemModel.SelectedProvider.Provider.ID, CurrencyId = SelectedItemModel.CurrencyModel.Currency.ID } })).Select(x => new Provider_itemModel { Provider_item = x }).ToList();
                }

                //in case of an update
                else
                {
                    // retrieving and updating the current provider_item
                    returnResult = (from p_i in provider_itemModelFoundList
                                    where p_i.Provider_item.ItemId == SelectedItemModel.Item.ID
                                    select new Provider_itemModel
                                    {
                                        Provider_item = new Provider_item { ID = p_i.Provider_item.ID, ItemId = p_i.Provider_item.ItemId, CurrencyId = SelectedItemModel.CurrencyModel.Currency.ID, ProviderId = SelectedItemModel.SelectedProvider.Provider.ID },
                                        Provider = SelectedItemModel.SelectedProvider.Provider,
                                        CurrencyModel = SelectedItemModel.CurrencyModel
                                    }).ToList();

                    // saving into database
                    await Bl.BlItem.UpdateProvider_itemAsync(returnResult.Select(x => x.Provider_item).ToList());
                }
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
                var itemFamilyFoundList = Bl.BlItem.searchItem(new Item { Type_sub = SelectedItemModel.TxtNewFamily }, ESearchOption.AND);
                if (itemFamilyFoundList.Count == 0)
                {
                    SelectedItemModel.TxtType_sub = SelectedItemModel.TxtNewFamily;
                }
            }
        }

        public async void updateItemImage(List<Info> infoDataList)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
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

        private async void generateItemReference()
        {
            string newRef = "";
            if (string.IsNullOrEmpty(SelectedItemModel.TxtRef))
            {
                // creating a new reference via the automatic reference system
                var auto_reflist = await Bl.BlItem.GetAuto_refDataAsync(1);
                var auto_ref = (auto_reflist.Count > 0) ? auto_reflist[0] : new Auto_ref();
                newRef = _ITEMREFERENCEPREFIX + auto_ref.RefId;
                newRef += ":" + SelectedItemModel.TxtName;
                SelectedItemModel.TxtRef = newRef;
                auto_ref.RefId++;

                if (auto_ref.ID == 0)
                    await Bl.BlItem.InsertAuto_refAsync(new List<Auto_ref> { auto_ref });
                else
                    await Bl.BlItem.UpdateAuto_refAsync(new List<Auto_ref> { auto_ref });
            }
            else
            {
                var refs = SelectedItemModel.TxtRef.Split(':').ToList();
                SelectedItemModel.TxtRef = refs[0] + ":" + SelectedItemModel.TxtName;
            }
        }

        public override void Dispose()
        {
            if (SelectedItemModel != null)
            {
                if (SelectedItemModel.Image != null)
                    SelectedItemModel.Image.Dispose();
                SelectedItemModel.PropertyChanged -= onItemNameChange_generateReference;
            }                
        }

        //----------------------------[ Event Handler ]------------------
        
        /// <summary>
        /// generate reference from item name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onItemNameChange_generateReference(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtName"))
            {
                generateItemReference();
            }
        }

        //----------------------------[ Action Commands ]------------------
        
        private async void deleteItem(string obj)
        {
            if (await Dialog.showAsync("Do you confirm deleting the item [" + SelectedItemModel.TxtRef+"] ?"))
            {
                bool isErrorDetected = false;
                var itemFoundList = Bl.BlItem.searchItem(new Item { Ref = SelectedItemModel.TxtRef, ID = SelectedItemModel.Item.ID }, ESearchOption.AND);
                var provider_itemFoundList = Bl.BlItem.searchProvider_item(new Provider_item { ItemId = SelectedItemModel.Item.ID }, ESearchOption.AND);
                if (itemFoundList.Count > 0 && itemFoundList[0].Erasable == EItem.Yes.ToString())
                {
                    Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);
                    await Bl.BlItem.DeleteProvider_itemAsync(provider_itemFoundList);
                    var notSavedItemList = await Bl.BlItem.DeleteItemAsync(new List<Item> { SelectedItemModel.Item });
                    if (notSavedItemList.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(SelectedItemModel.TxtPicture))
                        {
                            var credentials = Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, ESearchOption.AND);
                            if (WPFHelper.deleteFileFromFtpServer(ConfigurationManager.AppSettings["ftp_catalogue_image_folder"], SelectedItemModel.TxtPicture, credentials))
                            {
                                if (_main.ItemViewModel.ItemModelList.Where(x => x.TxtID == SelectedItemModel.TxtID).Count() != 0)
                                {
                                    List<ItemModel> buffer = _main.ItemViewModel.ItemModelList;
                                    buffer.Remove(SelectedItemModel);

                                    // call the property change for UI update
                                    _main.ItemViewModel.ItemModelList = new List<ItemModel>(buffer);
                                }

                                await Dialog.showAsync("Item deleted successfully!");
                            }
                            else
                                isErrorDetected = true;
                        }
                        else
                            await Dialog.showAsync("Item deleted successfully!");
                    }
                    else
                        isErrorDetected = true;

                    if (isErrorDetected)
                    {
                        string errorMessage = "Error occurred while deleting the item [ref=" + SelectedItemModel.TxtRef + "].";
                        Log.error(errorMessage, EErrorFrom.ITEM);
                        await Dialog.showAsync(errorMessage);
                    }

                    Dialog.IsDialogOpen = false;
                    //_main.IsRefresh = true;
                    _page(_main.ItemViewModel);
                }
                else
                    await Dialog.showAsync("The Item " + SelectedItemModel.TxtRef + " is used in one or several order!");
            }            
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
            // check that the item doesn't already exist    
            if (!string.IsNullOrEmpty(SelectedItemModel.TxtRef))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["wait_message"]);

                var itemFoundList = await Bl.BlItem.searchItemAsync(new Item { Ref = SelectedItemModel.TxtRef }, ESearchOption.AND);
                if (itemFoundList.Count == 0)
                {
                    // Process the field New Brand
                    processEntryNewBrand();

                    // Process the field New Family
                    processEntryNewFamily();

                    // process the field New Provider
                    var providerFoundList = await getEntryNewProvider();
                    
                    SelectedItemModel.Item.Source = Bl.BlSecurity.GetAuthenticatedUser().ID;
                    SelectedItemModel.Item.Erasable = EItem.Yes.ToString();
                    var itemSavedList = await Bl.BlItem.InsertItemAsync(new List<Item> { SelectedItemModel.Item });

                    SelectedItemModel.SelectedProvider = ((providerFoundList.Count > 0) ? providerFoundList.First() : new ProviderModel());
                    SelectedItemModel.Item = itemSavedList[0];

                    SelectedItemModel.Provider_itemModelList = await updateProvider_itemTable();
                     
                    if (itemSavedList.Count > 0)
                    {
                        // update the catalogue
                        SelectedItemModel.Item = itemSavedList[0];
                        if (_main.ItemViewModel.ItemModelList.Where(x => x.TxtID == SelectedItemModel.TxtID).Count() == 0)
                        {
                            List<ItemModel> buffer = _main.ItemViewModel.ItemModelList;
                            buffer.Add(SelectedItemModel);

                            // call the property change for UI update
                            _main.ItemViewModel.ItemModelList = new List<ItemModel>(buffer); 
                        }                       

                        await Dialog.showAsync("Item has been created successfully!");                        
                    }
                }

                // Otherwise update the current item
                else
                {
                    var savedProviderList = await Bl.BlItem.UpdateProviderAsync((await getEntryNewProvider()).Select(x=>x.Provider).ToList());
                    var savedItemList = await Bl.BlItem.UpdateItemAsync(new List<Item> { SelectedItemModel.Item });

                    SelectedItemModel.SelectedProvider = ((savedProviderList.Count > 0) ? new ProviderModel { Provider = savedProviderList.First() } : new ProviderModel());
                    SelectedItemModel.Item = savedItemList[0];

                    // item providers updating
                    SelectedItemModel.Provider_itemModelList = await updateProvider_itemTable();
                    
                    if (savedItemList.Count > 0)
                        await Dialog.showAsync("Item has been updated successfully!");
                }

                // update the brand list
                if (_main.ItemViewModel.BrandList.Where(x => x == SelectedItemModel.TxtType).Count() == 0)
                {
                    List<string> buffer = _main.ItemViewModel.BrandList.ToList();
                    buffer.Add(SelectedItemModel.TxtType);

                    // call the property change for UI update
                    _main.ItemViewModel.BrandList = new HashSet<string>(buffer);
                }

                // update the family list
                if (_main.ItemViewModel.FamilyList.Where(x => x == SelectedItemModel.TxtType_sub).Count() == 0)
                {
                    List<string> buffer = _main.ItemViewModel.FamilyList.ToList();
                    buffer.Add(SelectedItemModel.TxtType_sub);

                    // call the property change for UI update
                    _main.ItemViewModel.FamilyList = new HashSet<string>(buffer);
                }

                // update the provider list
                if (_main.ItemViewModel.ProviderList.Where(x => x.Provider.ID == SelectedItemModel.SelectedProvider.Provider.ID).Count() == 0)
                {
                    List<ProviderModel> buffer = _main.ItemViewModel.ProviderList.ToList();
                    buffer.Add( SelectedItemModel.SelectedProvider);

                    // call the property change for UI update
                    _main.ItemViewModel.ProviderList = new List<ProviderModel>(buffer);
                }

                _main.ItemViewModel.checkBoxToCartCommand.raiseCanExecuteActionChanged();
                OpenFileExplorerCommand.raiseCanExecuteActionChanged();                
            }  
            else
                await Dialog.showAsync("Item's name cannot be empty!");

            Dialog.IsDialogOpen = false;
            
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
            string newFileFullPath = InfoManager.ExecuteOpenFileDialog("Select an image file", new List<string> { "png", "jpeg", "jpg" });
            if (!string.IsNullOrEmpty(newFileFullPath) && File.Exists(newFileFullPath))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["wait_message"]);
                var ftpCredentials = Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, ESearchOption.AND);

                // closing the image file if opened in the order detail
                if (_main.OrderViewModel != null
                    && _main.OrderViewModel.OrderDetailViewModel != null
                    && _main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList != null)
                {
                    var imageFound = _main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList.Where(x => x.TxtItem_ref == SelectedItemModel.TxtRef).Select(x => x.ItemModel.Image).SingleOrDefault();
                    if (imageFound != null)
                        imageFound.closeImageSource();
                }

                if (SelectedItemModel.Image != null)
                {
                    SelectedItemModel.Image.closeImageSource();
                    WPFHelper.deleteFileFromFtpServer(ConfigurationManager.AppSettings["ftp_catalogue_image_folder"], SelectedItemModel.TxtPicture, ftpCredentials);
                }
                else
                    SelectedItemModel.Image = await Task.Factory.StartNew(() => { return SelectedItemModel.Image.downloadPicture(ConfigurationManager.AppSettings["ftp_catalogue_image_folder"], ConfigurationManager.AppSettings["local_catalogue_image_folder"], SelectedItemModel.TxtPicture, SelectedItemModel.TxtRef.Replace(' ', '_').Replace(':', '_'), ftpCredentials); });

                // opening the file explorer for image file choosing and resizing
                SelectedItemModel.Image.TxtChosenFile = newFileFullPath.resizeImage();
                SelectedItemModel.TxtPicture = SelectedItemModel.Image.TxtFileName;
                
                // upload the image file to the FTP server
                SelectedItemModel.Image.uploadImage();

                // update item image
                var savedItemList = await Bl.BlItem.UpdateItemAsync(new List<Item> { SelectedItemModel.Item });

                if (savedItemList.Count > 0)
                    await Dialog.showAsync("The picture has been saved successfully!");
                else
                {
                    string errorMessage = "Error occured while updating the item [" + SelectedItemModel.TxtRef + "] picture";
                    Log.error(errorMessage, EErrorFrom.ITEM);
                    await Dialog.showAsync(errorMessage);
                } 

                Dialog.IsDialogOpen = false;
            }
        }

        private bool canGetFileFromLocal(object arg)
        {
            if (SelectedItemModel != null && !string.IsNullOrEmpty(SelectedItemModel.TxtRef) && SelectedItemModel.TxtRef.StartsWith(_ITEMREFERENCEPREFIX))
                return true;

            return false;
        }

    }
}
