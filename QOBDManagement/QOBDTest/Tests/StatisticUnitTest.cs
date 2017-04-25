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
    public class StatisticUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;

        /*[TestMethod]
        public async Task insertStatistics()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Statistic statistic = new Statistic { ID = 1 };

            // Act
            var insertedStatisticList = await _main.StatisticViewModel.Bl.BlStatistic.InsertStatisticAsync(new List<Statistic> { statistic });

            // Assert
            Assert.AreEqual(insertedStatisticList.Count, 1);
        }

        [TestMethod]
        public async Task deleteStatistics()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Statistic statistic = new Statistic { ID = 1 };

            // Act
            var statisticNotDeletedList = await _main.StatisticViewModel.Bl.BlStatistic.DeleteStatisticAsync(new List<Statistic> { statistic });

            // Assert
            Assert.AreEqual(statisticNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateStatistics()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Statistic statistic = new Statistic { ID = 1 };

            // Act
            var statisticUpdatedList = await _main.StatisticViewModel.Bl.BlStatistic.UpdateStatisticAsync(new List<Statistic> { statistic });

            // Assert
            Assert.AreEqual(statisticUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoStatisticsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var statisticUpdatedList = _main.StatisticViewModel.Bl.BlStatistic.GetStatisticData(2);

            // Assert
            Assert.AreEqual(statisticUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoStatisticsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var statisticUpdatedList = await _main.StatisticViewModel.Bl.BlStatistic.GetStatisticDataAsync(2);

            // Assert
            Assert.AreEqual(statisticUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchStatisticFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var statisticfoundList = _main.StatisticViewModel.Bl.BlStatistic.searchStatistic(new Statistic { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(statisticfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchStatisticFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var statisticfoundList = await _main.StatisticViewModel.Bl.BlStatistic.searchStatisticAsync(new Statistic { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(statisticfoundList.Count, 1);
        }

        [TestMethod]
        public void loadStatistics()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.StatisticViewModel.loadStatistics();

            // Assert
            Assert.AreEqual(_main.StatisticViewModel.StatisticModelList.Count, 3);
        }

        [TestMethod]
        public void getActiveStatistics()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.StatisticViewModel.loadStatistics();

            // Assert
            Assert.AreEqual(_main.StatisticViewModel.ActiveStatisticModelList.Count, 2);
        }

        [TestMethod]
        public void getDeactivatedStatistics()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.StatisticViewModel.loadStatistics();

            // Assert
            Assert.AreEqual(_main.StatisticViewModel.DeactivatedStatisticModelList.Count, 1);
        }

        [TestMethod]
        public async Task moveStatisticCLientsToAnotherStatistic()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act 
            var movedClientList = await _main.StatisticViewModel.Bl.BlStatistic.MoveStatisticClient(new QOBDCommon.Entities.Statistic { ID = 1 }, new QOBDCommon.Entities.Statistic { ID = 50 });

            // Assert
            Assert.AreNotEqual(movedClientList.Count(), 0);
        }

        [TestMethod]
        public void selectStatisticForDetailsDisplaying()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act             
            _main.StatisticViewModel.selectStatistic(new QOBDManagement.Models.StatisticModel { Statistic = new QOBDCommon.Entities.Statistic { ID = 50 } });

            // Assert
            Assert.AreEqual(_main.StatisticViewModel.SelectedStatisticModel.Statistic.ID, 50);
        }*/
    }
}
