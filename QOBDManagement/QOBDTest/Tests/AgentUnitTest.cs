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
    public class AgentUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;
        Agent agent;
        Agent agent2;

        [TestInitialize]
        public void startup()
        {
            agent = new Agent { ID = 1, LastName = "Last name", FirstName = "First name", ListSize = 20, UserName = "user name", Status = "Active", HashedPassword = "password" };
            agent2 = new Agent { ID = 2, LastName = "Last name", FirstName = "First name", ListSize = 20, UserName = "user name", Status = "Active", HashedPassword = "password" };
        }

        [TestMethod]
        public async Task insertAgents()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedAgentList = await _main.AgentViewModel.Bl.BlAgent.InsertAgentAsync(new List<Agent> { agent });

            // Assert
            Assert.AreEqual(insertedAgentList.Count, 1);
        }

        [TestMethod]
        public async Task deleteAgents()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agentNotDeletedList = await _main.AgentViewModel.Bl.BlAgent.DeleteAgentAsync(new List<Agent> { agent });

            // Assert
            Assert.AreEqual(agentNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateAgents()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agentUpdatedList = await _main.AgentViewModel.Bl.BlAgent.UpdateAgentAsync(new List<Agent> { agent });

            // Assert
            Assert.AreEqual(agentUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoAgentsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agentUpdatedList = _main.AgentViewModel.Bl.BlAgent.GetAgentData(2);

            // Assert
            Assert.AreEqual(agentUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoAgentsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agentUpdatedList = await _main.AgentViewModel.Bl.BlAgent.GetAgentDataAsync(2);

            // Assert
            Assert.AreEqual(agentUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchAgentFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agentfoundList = _main.AgentViewModel.Bl.BlAgent.searchAgent(agent, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(agentfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchAgentFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agentfoundList = await _main.AgentViewModel.Bl.BlAgent.searchAgentAsync(agent, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(agentfoundList.Count, 1);
        }

        [TestMethod]
        public async Task loadAgents()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            await _main.AgentViewModel.loadAgents();

            // Assert
            Assert.AreEqual(_main.AgentViewModel.AgentModelList.Count, 10);
        }

        [TestMethod]
        public async Task getActiveAgents()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            await _main.AgentViewModel.loadAgents();

            // Assert
            Assert.AreNotEqual(_main.AgentViewModel.ActiveAgentModelList.Count, 0);
        }

        [TestMethod]
        public async Task getDeactivatedAgents()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            await _main.AgentViewModel.loadAgents();

            // Assert
            Assert.AreEqual(_main.AgentViewModel.DeactivatedAgentModelList.Count, 1);
        }

        [TestMethod]
        public async Task moveAgentCLientsToAnotherAgent()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act 
            var movedClientList = await _main.AgentViewModel.Bl.BlAgent.MoveAgentClient(agent, agent2);
            
            // Assert
            Assert.AreNotEqual(movedClientList.Count(), 0);
        }

        [TestMethod]
        public void selectAgentForDetailsDisplaying()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act             
            _main.AgentViewModel.selectAgent(new QOBDManagement.Models.AgentModel { Agent = new QOBDCommon.Entities.Agent { ID = 50 } });
            
            // Assert
            Assert.AreEqual(_main.AgentViewModel.SelectedAgentModel.Agent.ID, 50);
        }
    }
}
