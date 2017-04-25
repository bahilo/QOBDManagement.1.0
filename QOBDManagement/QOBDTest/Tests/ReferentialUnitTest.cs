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
    public class ReferentialUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;

        [TestMethod]
        public async Task insertInfos()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Info info = new Info { ID = 1 };

            // Act
            var insertedInfoList = await _main.ReferentialViewModel.Bl.BlReferential.InsertInfoAsync(new List<Info> { info });

            // Assert
            Assert.AreEqual(insertedInfoList.Count, 1);
        }

        [TestMethod]
        public async Task deleteInfos()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Info info = new Info { ID = 1 };

            // Act
            var infoNotDeletedList = await _main.ReferentialViewModel.Bl.BlReferential.DeleteInfoAsync(new List<Info> { info });

            // Assert
            Assert.AreEqual(infoNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateInfos()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Info info = new Info { ID = 1 };

            // Act
            var infoUpdatedList = await _main.ReferentialViewModel.Bl.BlReferential.UpdateInfoAsync(new List<Info> { info });

            // Assert
            Assert.AreEqual(infoUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoInfosFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var infoUpdatedList = _main.ReferentialViewModel.Bl.BlReferential.GetInfoData(2);

            // Assert
            Assert.AreEqual(infoUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoInfosFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var infoUpdatedList = await _main.ReferentialViewModel.Bl.BlReferential.GetInfoDataAsync(2);

            // Assert
            Assert.AreEqual(infoUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchInfoFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var infofoundList = _main.ReferentialViewModel.Bl.BlReferential.searchInfo(new Info { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(infofoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchInfoFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var infofoundList = await _main.ReferentialViewModel.Bl.BlReferential.searchInfoAsync(new Info { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(infofoundList.Count, 1);
        }
    }
}
