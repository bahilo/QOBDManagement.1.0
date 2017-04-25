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
    public class NotificationUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;

        [TestMethod]
        public async Task insertNotifications()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Notification notification = new Notification { ID = 1 };

            // Act
            var insertedNotificationList = await _main.NotificationViewModel.Bl.BlNotification.InsertNotificationAsync(new List<Notification> { notification });

            // Assert
            Assert.AreEqual(insertedNotificationList.Count, 1);
        }

        [TestMethod]
        public async Task deleteNotifications()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Notification notification = new Notification { ID = 1 };

            // Act
            var notificationNotDeletedList = await _main.NotificationViewModel.Bl.BlNotification.DeleteNotificationAsync(new List<Notification> { notification });

            // Assert
            Assert.AreEqual(notificationNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateNotifications()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            Notification notification = new Notification { ID = 1 };

            // Act
            var notificationUpdatedList = await _main.NotificationViewModel.Bl.BlNotification.UpdateNotificationAsync(new List<Notification> { notification });

            // Assert
            Assert.AreEqual(notificationUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoNotificationsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var notificationUpdatedList = _main.NotificationViewModel.Bl.BlNotification.GetNotificationData(2);

            // Assert
            Assert.AreEqual(notificationUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoNotificationsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var notificationUpdatedList = await _main.NotificationViewModel.Bl.BlNotification.GetNotificationDataAsync(2);

            // Assert
            Assert.AreEqual(notificationUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchNotificationFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var notificationfoundList = _main.NotificationViewModel.Bl.BlNotification.searchNotification(new Notification { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(notificationfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchNotificationFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var notificationfoundList = await _main.NotificationViewModel.Bl.BlNotification.searchNotificationAsync(new Notification { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(notificationfoundList.Count, 1);
        }

        [TestMethod]
        public async Task loadNotifications()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            await _main.NotificationViewModel.loadNotifications();

            // Assert
            Assert.AreEqual(_main.NotificationViewModel.BillNotPaidList.Count, 1);
        }
    }
}
