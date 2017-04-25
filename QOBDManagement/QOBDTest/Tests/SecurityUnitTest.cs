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
    public class SecurityUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;
        QOBDCommon.Entities.Action action;
        ActionRecord actionRecord;
        Agent_role agent_role;
        Privilege privilege;
        Role role;
        Role_action role_action;

        [TestInitialize]
        public void startup()
        {
            action = new QOBDCommon.Entities.Action { ID = 1, Right = new Privilege { ID = 1, Date = DateTime.Now, Role_actionId = 1 } };
            actionRecord = new ActionRecord { ID = 1, Action = "Action name", TargetId = 1, AgentId = 1, Date = DateTime.Now, TargetName = "Target name" };
            agent_role  = new Agent_role { ID = 1, AgentId = 1, RoleId = 1, Date = DateTime.Now };
            privilege = new Privilege { ID = 1, Date = DateTime.Now };
            role = new Role { ID = 1, Name = "role name", ActionList = new List<QOBDCommon.Entities.Action>() };
            role_action = new Role_action { ID = 1, ActionId = 1, RoleId = 1 };
        }

        #region [ Action ]
        [TestMethod]
        public async Task insertActions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedActionList = await _main.SecurityLoginViewModel.Bl.BlSecurity.InsertActionAsync(new List<QOBDCommon.Entities.Action> { action });

            // Assert
            Assert.AreEqual(insertedActionList.Count, 1);
        }

        [TestMethod]
        public async Task deleteActions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionNotDeletedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.DeleteActionAsync(new List<QOBDCommon.Entities.Action> { action });

            // Assert
            Assert.AreEqual(actionNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateActions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.UpdateActionAsync(new List<QOBDCommon.Entities.Action> { action });

            // Assert
            Assert.AreEqual(actionUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoActionsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetActionDataAsync(2);

            // Assert
            Assert.AreEqual(actionUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoActionsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetActionDataAsync(2);

            // Assert
            Assert.AreEqual(actionUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchActionFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionfoundList = await _main.SecurityLoginViewModel.Bl.BlSecurity.searchActionAsync(new QOBDCommon.Entities.Action { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(actionfoundList.Count, 1);
        }
        #endregion

        #region [ ActionRecord ]
        [TestMethod]
        public async Task insertActionRecords()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedActionRecordList = await _main.SecurityLoginViewModel.Bl.BlSecurity.InsertActionRecordAsync(new List<ActionRecord> { actionRecord });

            // Assert
            Assert.AreEqual(insertedActionRecordList.Count, 1);
        }

        [TestMethod]
        public async Task deleteActionRecords()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionRecordNotDeletedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.DeleteActionRecordAsync(new List<ActionRecord> { actionRecord });

            // Assert
            Assert.AreEqual(actionRecordNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateActionRecords()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionRecordUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.UpdateActionRecordAsync(new List<ActionRecord> { actionRecord });

            // Assert
            Assert.AreEqual(actionRecordUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoActionRecordsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionRecordUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetActionRecordDataAsync(2);

            // Assert
            Assert.AreEqual(actionRecordUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoActionRecordsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionRecordUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetActionRecordDataAsync(2);

            // Assert
            Assert.AreEqual(actionRecordUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchActionRecordFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var actionRecordfoundList = await _main.SecurityLoginViewModel.Bl.BlSecurity.searchActionRecordAsync(new ActionRecord { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(actionRecordfoundList.Count, 1);
        }
        #endregion

        #region [ Agent_role ]
        [TestMethod]
        public async Task insertAgent_roles()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));             

            // Act
            var insertedAgent_roleList = await _main.SecurityLoginViewModel.Bl.BlSecurity.InsertAgent_roleAsync(new List<Agent_role> { agent_role });

            // Assert
            Assert.AreEqual(insertedAgent_roleList.Count, 1);
        }

        [TestMethod]
        public async Task deleteAgent_roles()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agent_roleNotDeletedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.DeleteAgent_roleAsync(new List<Agent_role> { agent_role });

            // Assert
            Assert.AreEqual(agent_roleNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateAgent_roles()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agent_roleUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.UpdateAgent_roleAsync(new List<Agent_role> { agent_role });

            // Assert
            Assert.AreEqual(agent_roleUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoAgent_rolesFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agent_roleUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetAgent_roleDataAsync(2);

            // Assert
            Assert.AreEqual(agent_roleUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoAgent_rolesFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agent_roleUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetAgent_roleDataAsync(2);

            // Assert
            Assert.AreEqual(agent_roleUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchAgent_roleFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var agent_rolefoundList = await _main.SecurityLoginViewModel.Bl.BlSecurity.searchAgent_roleAsync(new Agent_role { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(agent_rolefoundList.Count, 1);
        }
        #endregion

        #region [ Privilege ]
        [TestMethod]
        public async Task insertPrivileges()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));            

            // Act
            var insertedPrivilegeList = await _main.SecurityLoginViewModel.Bl.BlSecurity.InsertPrivilegeAsync(new List<Privilege> { privilege });

            // Assert
            Assert.AreEqual(insertedPrivilegeList.Count, 1);
        }

        [TestMethod]
        public async Task deletePrivileges()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var privilegeNotDeletedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.DeletePrivilegeAsync(new List<Privilege> { privilege });

            // Assert
            Assert.AreEqual(privilegeNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updatePrivileges()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var privilegeUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.UpdatePrivilegeAsync(new List<Privilege> { privilege });

            // Assert
            Assert.AreEqual(privilegeUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoPrivilegesFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var privilegeUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetPrivilegeDataAsync(2);

            // Assert
            Assert.AreEqual(privilegeUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoPrivilegesFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var privilegeUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetPrivilegeDataAsync(2);

            // Assert
            Assert.AreEqual(privilegeUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchPrivilegeFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var privilegefoundList = await _main.SecurityLoginViewModel.Bl.BlSecurity.searchPrivilegeAsync(new Privilege { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(privilegefoundList.Count, 1);
        }
        #endregion

        #region [ Role ]
        [TestMethod]
        public async Task insertRoles()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));            

            // Act
            var insertedRoleList = await _main.SecurityLoginViewModel.Bl.BlSecurity.InsertRoleAsync(new List<Role> { role });

            // Assert
            Assert.AreEqual(insertedRoleList.Count, 1);
        }

        [TestMethod]
        public async Task deleteRoles()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var roleNotDeletedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.DeleteRoleAsync(new List<Role> { role });

            // Assert
            Assert.AreEqual(roleNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateRoles()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var roleUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.UpdateRoleAsync(new List<Role> { role });

            // Assert
            Assert.AreEqual(roleUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoRolesFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var roleUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetRoleDataAsync(2);

            // Assert
            Assert.AreEqual(roleUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoRolesFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var roleUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetRoleDataAsync(2);

            // Assert
            Assert.AreEqual(roleUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchRoleFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var rolefoundList = await _main.SecurityLoginViewModel.Bl.BlSecurity.searchRoleAsync(new Role { ID = 1, Name = "name"}, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(rolefoundList.Count, 1);
        }
        #endregion

        #region [ Role_action ]
        [TestMethod]
        public async Task insertRole_actions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedRole_actionList = await _main.SecurityLoginViewModel.Bl.BlSecurity.InsertRole_actionAsync(new List<Role_action> { role_action });

            // Assert
            Assert.AreEqual(insertedRole_actionList.Count, 1);
        }

        [TestMethod]
        public async Task deleteRole_actions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var role_actionNotDeletedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.DeleteRole_actionAsync(new List<Role_action> { role_action });

            // Assert
            Assert.AreEqual(role_actionNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateRole_actions()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var role_actionUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.UpdateRole_actionAsync(new List<Role_action> { role_action });

            // Assert
            Assert.AreEqual(role_actionUpdatedList.Count, 1);
        }

        [TestMethod]
        public async Task getTwoRole_actionsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var role_actionUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetRole_actionDataAsync(2);

            // Assert
            Assert.AreEqual(role_actionUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoRole_actionsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var role_actionUpdatedList = await _main.SecurityLoginViewModel.Bl.BlSecurity.GetRole_actionDataAsync(2);

            // Assert
            Assert.AreEqual(role_actionUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task searchRole_actionFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var role_actionfoundList = await _main.SecurityLoginViewModel.Bl.BlSecurity.searchRole_actionAsync(new Role_action { ID = 1 }, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(role_actionfoundList.Count, 1);
        }
        #endregion
    }
}
