using Microsoft.VisualStudio.TestTools.UnitTesting;
using QOBDCommon.Classes;
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
    public class ChatRoomUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;
        Discussion discussion;
        Message message;
        User_discussion user_discussion;

        [TestInitialize]
        public void startup()
        {
            discussion = new Discussion { ID = 1, Date = DateTime.Now };
            message = new Message { ID = 2, Date = Utility.convertToDateTime(DateTime.Now.ToShortDateString()), Content = "Content", DiscussionId = 1, Status = 0, UserId = 1 };
            user_discussion = new User_discussion { ID = 1, UserId = 1, DiscussionId = 1, Status = 0 };
        }

        #region [ Discussion ]
        [TestMethod]
        public async Task insertDiscussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedDiscussionList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.InsertDiscussionAsync(new List<Discussion> { discussion });

            // Assert
            Assert.AreEqual(insertedDiscussionList.Count, 1);
        }

        [TestMethod]
        public async Task deleteDiscussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var discussionNotDeletedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.DeleteDiscussionAsync(new List<Discussion> { discussion });

            // Assert
            Assert.AreEqual(discussionNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateDiscussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var discussionUpdatedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.UpdateDiscussionAsync(new List<Discussion> { discussion });

            // Assert
            Assert.AreEqual(discussionUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoDiscussionsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var discussionUpdatedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.GetDiscussionDataAsync(2);

            // Assert
            Assert.AreEqual(discussionUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchDiscussionFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var discussionfoundList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.searchDiscussionAsync(discussion, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(discussionfoundList.Count, 1);
        }

        [TestMethod]
        public async Task loadDiscussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            await _main.SecurityLoginViewModel.startAuthentication();

            // Act
            await _main.ChatRoomViewModel.DiscussionViewModel.retrieveUserDiscussions(_main.Startup.Bl.BlSecurity.GetAuthenticatedUser());

            // Assert
            Assert.AreEqual(_main.ChatRoomViewModel.DiscussionViewModel.DiscussionList.Count, 1);
        }
        #endregion

        #region [ Message ]
        [TestMethod]
        public async Task insertMessages()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedMessageList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.InsertMessageAsync(new List<Message> { message });

            // Assert
            Assert.AreEqual(insertedMessageList.Count, 1);
        }

        [TestMethod]
        public async Task deleteMessages()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var messageNotDeletedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.DeleteMessageAsync(new List<Message> { message });

            // Assert
            Assert.AreEqual(messageNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateMessages()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var messageUpdatedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.UpdateMessageAsync(new List<Message> { message });

            // Assert
            Assert.AreEqual(messageUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoMessagesFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var messageUpdatedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.GetMessageDataAsync(2);

            // Assert
            Assert.AreEqual(messageUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchMessageFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var messagefoundList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.searchMessageAsync(message, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(1, messagefoundList.Count);
        }

        [TestMethod]
        public async Task getGroupMessages()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            await _main.SecurityLoginViewModel.startAuthentication();
            _main.ChatRoomViewModel.DiscussionViewModel.ChatAgentModelList.Add(new QOBDManagement.Models.AgentModel { Agent = new Agent { ID = 2 } });

            // Act
            await _main.ChatRoomViewModel.MessageViewModel.loadAsync();

            // Assert
            Assert.AreEqual(0, _main.ChatRoomViewModel.MessageViewModel.MessageGroupHistoryList.Count);
        }

        [TestMethod]
        public async Task getIndividualMessages()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));
            await _main.SecurityLoginViewModel.startAuthentication();
            _main.ChatRoomViewModel.DiscussionViewModel.ChatAgentModelList.Add(new QOBDManagement.Models.AgentModel { Agent = new Agent { ID = 2 } });

            // Act
            await _main.ChatRoomViewModel.MessageViewModel.loadAsync();

            // Assert
            Assert.AreEqual(1, _main.ChatRoomViewModel.MessageViewModel.MessageIndividualHistoryList.Count);
        }
        #endregion

        #region [ User_discussion ]
        [TestMethod]
        public async Task insertUser_discussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedUser_discussionList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.InsertUser_discussionAsync(new List<User_discussion> { user_discussion });

            // Assert
            Assert.AreEqual(insertedUser_discussionList.Count, 1);
        }

        [TestMethod]
        public async Task deleteUser_discussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var user_discussionNotDeletedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.DeleteUser_discussionAsync(new List<User_discussion> { user_discussion });

            // Assert
            Assert.AreEqual(user_discussionNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateUser_discussions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var user_discussionUpdatedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.UpdateUser_discussionAsync(new List<User_discussion> { user_discussion });

            // Assert
            Assert.AreEqual(user_discussionUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoUser_discussionsFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var user_discussionUpdatedList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.GetUser_discussionDataAsync(2);

            // Assert
            Assert.AreEqual(user_discussionUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchUser_discussionFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var user_discussionfoundList = await _main.ChatRoomViewModel.Startup.Bl.BlChatRoom.searchUser_discussionAsync(user_discussion, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(1, user_discussionfoundList.Count);
        }
        #endregion
    }
}
