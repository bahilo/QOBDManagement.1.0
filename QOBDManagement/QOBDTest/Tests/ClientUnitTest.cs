using Microsoft.VisualStudio.TestTools.UnitTesting;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
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
    public class ClientUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;
        Client client;
        Contact contact;
        Address address;

        [TestInitialize]
        public void startup()
        {
            client = new Client { ID = 1, MaxCredit = 200, FirstName = "Last name", LastName = "last name", Status = "Active" };
            contact = new Contact { ID = 1, ClientId = 1, Firstname = "First name", LastName = "Last Name" };
            address = new Address { ID = 1, Name = "Name", Name2 = "Name 2", LastName = "last name", FirstName = "first name", ClientId = 1 };
        }

        #region [ Client ]
        [TestMethod]
        public async Task insertClients()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedClientList = await _main.ClientViewModel.Bl.BlClient.InsertClientAsync(new List<Client> { client });

            // Assert
            Assert.AreEqual(insertedClientList.Count, 1);
        }

        [TestMethod]
        public async Task deleteClients()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientNotDeletedList = await _main.ClientViewModel.Bl.BlClient.DeleteClientAsync(new List<Client> { client });

            // Assert
            Assert.AreEqual(clientNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateClients()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientUpdatedList = await _main.ClientViewModel.Bl.BlClient.UpdateClientAsync(new List<Client> { client });

            // Assert
            Assert.AreEqual(clientUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoClientsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientUpdatedList = _main.ClientViewModel.Bl.BlClient.GetClientData(2);

            // Assert
            Assert.AreEqual(clientUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoClientsFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientUpdatedList = await _main.ClientViewModel.Bl.BlClient.GetClientDataAsync(2);

            // Assert
            Assert.AreEqual(clientUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchClientFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientfoundList = _main.ClientViewModel.Bl.BlClient.searchClient(client, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(clientfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchClientFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientfoundList = await _main.ClientViewModel.Bl.BlClient.searchClientAsync(client, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(clientfoundList.Count, 1);
        }

        [TestMethod]
        public void loadClients()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.ClientViewModel.loadClients();

            // Assert
            Assert.AreEqual(_main.ClientViewModel.ClientModelList.Count, 1);
        }

        [TestMethod]
        public void loadClientContacts()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientModel = _main.ClientViewModel.ClientDetailViewModel.loadContactsAndAddresses(new QOBDManagement.Models.ClientModel { Client = new QOBDCommon.Entities.Client { ID = 1 } });

            // Assert
            Assert.AreNotEqual(clientModel.ContactList.Count, 0);
        }

        [TestMethod]
        public void loadClientAddresses()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var clientModel = _main.ClientViewModel.ClientDetailViewModel.loadContactsAndAddresses(new QOBDManagement.Models.ClientModel { Client = new QOBDCommon.Entities.Client { ID = 1 } });

            // Assert
            Assert.AreNotEqual(clientModel.AddressList.Count, 0);
        }

        [TestMethod]
        public void selectClientForDetailsDisplaying()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.ClientViewModel.selectCurrentClient(new QOBDManagement.Models.ClientModel { Client = new QOBDCommon.Entities.Client { ID = 1 } });

            // Assert
            Assert.AreEqual(_main.ClientViewModel.SelectedCLientModel.Client.ID, 1);
        }

        [TestMethod]
        public async Task moveSelectedCLientListToAgent()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            List<Client> clientListToMove = new List<Client> {
                new Client { ID = 1, AgentId = 1, MaxCredit = 200, FirstName = "Last name", LastName = "last name", Status = "Active" },
                new Client { ID = 2, AgentId = 2, MaxCredit = 200, FirstName = "Last name", LastName = "last name", Status = "Active" },
                new Client { ID = 3, AgentId = 50, MaxCredit = 200, FirstName = "Last name", LastName = "last name", Status = "Active" },
            };

            // Act
            var movedClientList = await _main.ClientViewModel.Bl.BlClient.MoveClientAgentBySelection(clientListToMove, new Agent { ID = 1, Status = "Active" });

            // Assert
            Assert.AreEqual(movedClientList.Count, 3);
        }
        #endregion

        #region [ Contact ]
        [TestMethod]
        public async Task insertContacts()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedContactList = await _main.ClientViewModel.Bl.BlClient.InsertContactAsync(new List<Contact> { contact });

            // Assert
            Assert.AreEqual(insertedContactList.Count, 1);
        }

        [TestMethod]
        public async Task deleteContacts()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var contactNotDeletedList = await _main.ClientViewModel.Bl.BlClient.DeleteContactAsync(new List<Contact> { contact });

            // Assert
            Assert.AreEqual(contactNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateContacts()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var contactUpdatedList = await _main.ClientViewModel.Bl.BlClient.UpdateContactAsync(new List<Contact> { contact });

            // Assert
            Assert.AreEqual(contactUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoContactsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var contactUpdatedList = _main.ClientViewModel.Bl.BlClient.GetContactData(2);

            // Assert
            Assert.AreEqual(contactUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoContactsFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var contactUpdatedList = await _main.ClientViewModel.Bl.BlClient.GetContactDataAsync(2);

            // Assert
            Assert.AreEqual(contactUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchContactFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var contactfoundList = _main.ClientViewModel.Bl.BlClient.searchContact(new Contact { ID = 1, ClientId = 1, Firstname = "First name", LastName = "Last Name" }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(contactfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchContactFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var contactfoundList = await _main.ClientViewModel.Bl.BlClient.searchContactAsync(new Contact { ID = 1, ClientId = 1, Firstname = "First name", LastName = "Last Name" }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(contactfoundList.Count, 1);
        }
        #endregion

        #region [ Address ]
        [TestMethod]
        public async Task insertAddresses()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedAddressList = await _main.ClientViewModel.Bl.BlClient.InsertAddressAsync(new List<Address> { address });

            // Assert
            Assert.AreEqual(insertedAddressList.Count, 1);
        }

        [TestMethod]
        public async Task deleteAddresses()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var addressNotDeletedList = await _main.ClientViewModel.Bl.BlClient.DeleteAddressAsync(new List<Address> { address });

            // Assert
            Assert.AreEqual(addressNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateAddresses()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var addressUpdatedList = await _main.ClientViewModel.Bl.BlClient.UpdateAddressAsync(new List<Address> { address });

            // Assert
            Assert.AreEqual(addressUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoAddressesFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var addressUpdatedList = _main.ClientViewModel.Bl.BlClient.GetAddressData(2);

            // Assert
            Assert.AreEqual(addressUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoAddressesFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var addressUpdatedList = await _main.ClientViewModel.Bl.BlClient.GetAddressDataAsync(2);

            // Assert
            Assert.AreEqual(addressUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchAddressFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var addressfoundList = _main.ClientViewModel.Bl.BlClient.searchAddress(new Address { ID = 1, Name = "Name", Name2 = "Name 2", LastName = "last name", FirstName = "first name", ClientId = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(addressfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchAddressFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var addressfoundList = await _main.ClientViewModel.Bl.BlClient.searchAddressAsync(new Address { ID = 1, Name = "Name", Name2 = "Name 2", LastName = "last name", FirstName = "first name", ClientId = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(addressfoundList.Count, 1);
        }
        #endregion

    }
}
