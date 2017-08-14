using Microsoft.VisualStudio.TestTools.UnitTesting;
using QOBDCommon.Entities;
using QOBDManagement;
using QOBDManagement.Interfaces;
using QOBDTest.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDTest.Tests
{
    [TestClass]
    public class ItemUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;
        Item item;
        Provider provider;
        Provider_item provider_item;
        Tax_item tax_item;
        Auto_ref auto_ref;

        [TestInitialize]
        public void startup()
        {
            item = new Item { ID = 1, Name = "name", Ref = "ref", Type = "brand", Type_sub = "family", Source = 1, Price_purchase = 10, Price_sell = 20 };
            provider = new Provider { ID = 1, Source = 1, Name = "Name" };
            provider_item = new Provider_item { ID = 1, ItemId = 1, ProviderId = 1 };
            tax_item = new Tax_item { ID = 1, itemId = 1, Item_ref = "item ref", TaxId = 1, Tax_type = "tax type", Tax_value = 20 };
            auto_ref = new Auto_ref { ID = 1, RefId = 1 };
        }


        #region [ Item ]
        [TestMethod]
        public async Task insertItems()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedItemList = await _main.ItemViewModel.Bl.BlItem.InsertItemAsync(new List<Item> { item });

            // Assert
            Assert.AreEqual(insertedItemList.Count, 1);
        }

        [TestMethod]
        public async Task deleteItems()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var itemNotDeletedList = await _main.ItemViewModel.Bl.BlItem.DeleteItemAsync(new List<Item> { item });

            // Assert
            Assert.AreEqual(itemNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateItems()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var itemUpdatedList = await _main.ItemViewModel.Bl.BlItem.UpdateItemAsync(new List<Item> { item });

            // Assert
            Assert.AreEqual(itemUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoItemsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var itemUpdatedList = _main.ItemViewModel.Bl.BlItem.GetItemData(2);

            // Assert
            Assert.AreEqual(itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoItemsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var itemUpdatedList = await _main.ItemViewModel.Bl.BlItem.GetItemDataAsync(2);

            // Assert
            Assert.AreEqual(itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchItemFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var itemfoundList = _main.ItemViewModel.Bl.BlItem.searchItem(item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(itemfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchItemFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var itemfoundList = await _main.ItemViewModel.Bl.BlItem.searchItemAsync(item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(itemfoundList.Count, 1);
        }

        [TestMethod]
        public async Task loadItems()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            await _main.ItemViewModel.loadItemsAsync();

            // Assert
            Assert.AreEqual(2, _main.ItemViewModel.ItemModelList.Count);
        }

        [TestMethod]
        public void selectItemForDetailsDisplaying()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act             
            _main.ItemViewModel.showSelectedItem(new QOBDManagement.Models.ItemModel { Item = item });

            // Assert
            Assert.AreEqual(_main.ItemViewModel.SelectedItemModel.Item.ID, 1);
        }        
        #endregion

        #region[ Provider ]
        [TestMethod]
        public async Task insertProviders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedProviderList = await _main.ItemViewModel.Bl.BlItem.InsertProviderAsync(new List<Provider> { provider });

            // Assert
            Assert.AreEqual(insertedProviderList.Count, 1);
        }

        [TestMethod]
        public async Task deleteProviders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var providerNotDeletedList = await _main.ItemViewModel.Bl.BlItem.DeleteProviderAsync(new List<Provider> { provider });

            // Assert
            Assert.AreEqual(providerNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateProviders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var providerUpdatedList = await _main.ItemViewModel.Bl.BlItem.UpdateProviderAsync(new List<Provider> { provider });

            // Assert
            Assert.AreEqual(providerUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoProvidersFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var providerUpdatedList = _main.ItemViewModel.Bl.BlItem.GetProviderData(2);

            // Assert
            Assert.AreEqual(providerUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoProvidersFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var providerUpdatedList = await _main.ItemViewModel.Bl.BlItem.GetProviderDataAsync(2);

            // Assert
            Assert.AreEqual(providerUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchProviderFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var providerfoundList = _main.ItemViewModel.Bl.BlItem.searchProvider(provider, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(providerfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchProviderFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var providerfoundList = await _main.ItemViewModel.Bl.BlItem.searchProviderAsync(provider, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(providerfoundList.Count, 1);
        }
        #endregion

        #region[ Provider_item ]
        [TestMethod]
        public async Task insertProvider_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedProvider_itemList = await _main.ItemViewModel.Bl.BlItem.InsertProvider_itemAsync(new List<Provider_item> { provider_item });

            // Assert
            Assert.AreEqual(insertedProvider_itemList.Count, 1);
        }

        [TestMethod]
        public async Task deleteProvider_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var provider_itemNotDeletedList = await _main.ItemViewModel.Bl.BlItem.DeleteProvider_itemAsync(new List<Provider_item> { provider_item });

            // Assert
            Assert.AreEqual(provider_itemNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateProvider_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var provider_itemUpdatedList = await _main.ItemViewModel.Bl.BlItem.UpdateProvider_itemAsync(new List<Provider_item> { provider_item });

            // Assert
            Assert.AreEqual(provider_itemUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoProvider_itemsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var provider_itemUpdatedList = _main.ItemViewModel.Bl.BlItem.GetProvider_itemData(2);

            // Assert
            Assert.AreEqual(provider_itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoProvider_itemsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var provider_itemUpdatedList = await _main.ItemViewModel.Bl.BlItem.GetProvider_itemDataAsync(2);

            // Assert
            Assert.AreEqual(provider_itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchProvider_itemFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var provider_itemfoundList = _main.ItemViewModel.Bl.BlItem.searchProvider_item(provider_item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(provider_itemfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchProvider_itemFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var provider_itemfoundList = await _main.ItemViewModel.Bl.BlItem.searchProvider_itemAsync(provider_item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(provider_itemfoundList.Count, 1);
        }
        #endregion

        #region[ Tax_item ]
        [TestMethod]
        public async Task insertTax_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedTax_itemList = await _main.ItemViewModel.Bl.BlItem.InsertTax_itemAsync(new List<Tax_item> { tax_item });

            // Assert
            Assert.AreEqual(insertedTax_itemList.Count, 1);
        }

        [TestMethod]
        public async Task deleteTax_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_itemNotDeletedList = await _main.ItemViewModel.Bl.BlItem.DeleteTax_itemAsync(new List<Tax_item> { tax_item });

            // Assert
            Assert.AreEqual(tax_itemNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateTax_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_itemUpdatedList = await _main.ItemViewModel.Bl.BlItem.UpdateTax_itemAsync(new List<Tax_item> { tax_item });

            // Assert
            Assert.AreEqual(tax_itemUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoTax_itemsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_itemUpdatedList = _main.ItemViewModel.Bl.BlItem.GetTax_itemData(2);

            // Assert
            Assert.AreEqual(tax_itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoTax_itemsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_itemUpdatedList = await _main.ItemViewModel.Bl.BlItem.GetTax_itemDataAsync(2);

            // Assert
            Assert.AreEqual(tax_itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchTax_itemFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_itemfoundList = _main.ItemViewModel.Bl.BlItem.searchTax_item(tax_item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(tax_itemfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchTax_itemFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_itemfoundList = await _main.ItemViewModel.Bl.BlItem.searchTax_itemAsync(tax_item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(tax_itemfoundList.Count, 1);
        }
        #endregion

        #region[ Auto_ref ]
        [TestMethod]
        public async Task insertAuto_refs()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedAuto_refList = await _main.ItemViewModel.Bl.BlItem.InsertAuto_refAsync(new List<Auto_ref> { auto_ref });

            // Assert
            Assert.AreEqual(insertedAuto_refList.Count, 1);
        }

        [TestMethod]
        public async Task deleteAuto_refs()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var auto_refNotDeletedList = await _main.ItemViewModel.Bl.BlItem.DeleteAuto_refAsync(new List<Auto_ref> { auto_ref });

            // Assert
            Assert.AreEqual(auto_refNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateAuto_refs()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var auto_refUpdatedList = await _main.ItemViewModel.Bl.BlItem.UpdateAuto_refAsync(new List<Auto_ref> { auto_ref });

            // Assert
            Assert.AreEqual(auto_refUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoAuto_refsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var auto_refUpdatedList = _main.ItemViewModel.Bl.BlItem.GetAuto_refData(2);

            // Assert
            Assert.AreEqual(auto_refUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoAuto_refsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var auto_refUpdatedList = await _main.ItemViewModel.Bl.BlItem.GetAuto_refDataAsync(2);

            // Assert
            Assert.AreEqual(auto_refUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchAuto_refFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var auto_reffoundList = _main.ItemViewModel.Bl.BlItem.searchAuto_ref(auto_ref, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(auto_reffoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchAuto_refFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var auto_reffoundList = await _main.ItemViewModel.Bl.BlItem.searchAuto_refAsync(auto_ref, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(auto_reffoundList.Count, 1);
        }
        #endregion
    }
}
