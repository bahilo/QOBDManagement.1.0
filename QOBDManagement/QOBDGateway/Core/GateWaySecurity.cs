using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.REMOTE;
using QOBDGateway.Helper.ChannelHelper;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWaySecurity : ISecurityManager
    {
        private QOBDWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public Agent _authenticatedUser;

        public GateWaySecurity()
        {
            _channel = new QOBDWebServicePortTypeClient("QOBDWebServicePort");// (binding, endPoint);
        }

        public void initializeCredential(Agent user)
        {
            Credential = user;
        }

        public Agent Credential
        {
            set
            {
                setServiceCredential(value.Login, value.HashedPassword);
                _authenticatedUser = value;
                onPropertyChange("Credential");
            }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QOBDWebServicePortTypeClient("QOBDWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        public async Task<Agent> AuthenticateUserAsync(string username, string password, bool isClearPassword = true)
        {
            Agent agentFound = new Agent();
            try
            {
                setServiceCredential(username, password);
                AgentQOBD agentArray = await _channel.get_authenticate_userAsync(username, password);
                agentFound = new AgentQOBD[] { agentArray}.ArrayTypeToAgent().FirstOrDefault();
                if (agentFound == null || agentFound.ID == 0)
                    throw new ApplicationException(string.Format("Your login {0} does not match any user in our database!", username));
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return agentFound;
        }

        public async Task<List<ActionRecord>> InsertActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.insert_data_actionRecordAsync(listActionRecord.ActionRecordTypeToArray())).ArrayTypeToActionRecord();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role>> InsertRoleAsync(List<Role> roleList)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.insert_data_roleAsync(roleList.RoleTypeToArray())).ArrayTypeToRole();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> InsertActionAsync(List<QOBDCommon.Entities.Action> actionList)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = (await _channel.insert_data_actionAsync(actionList.ActionTypeToArray())).ArrayTypeToAction();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role_action>> InsertRole_actionAsync(List<Role_action> role_actionList)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.insert_data_role_actionAsync(role_actionList.Role_actionTypeToArray())).ArrayTypeToRole_action();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent_role>> InsertAgent_roleAsync(List<Agent_role> agent_roleList)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.insert_data_agent_roleAsync(agent_roleList.Agent_roleTypeToArray())).ArrayTypeToAgent_role();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Privilege>> InsertPrivilegeAsync(List<Privilege> privilegeList)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.insert_data_privilegeAsync(privilegeList.PrivilegeTypeToArray())).ArrayTypeToPrivilege();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<ActionRecord>> DeleteActionRecordAsync(List<ActionRecord> actionRecordList)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.delete_data_actionRecordAsync(actionRecordList.ActionRecordTypeToArray())).ArrayTypeToActionRecord();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role>> DeleteRoleAsync(List<Role> roleList)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.delete_data_roleAsync(roleList.RoleTypeToArray())).ArrayTypeToRole();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> DeleteActionAsync(List<QOBDCommon.Entities.Action> actionList)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = (await _channel.delete_data_actionAsync(actionList.ActionTypeToArray())).ArrayTypeToAction();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent_role>> DeleteAgent_roleAsync(List<Agent_role> agent_roleList)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.delete_data_agent_roleAsync(agent_roleList.Agent_roleTypeToArray())).ArrayTypeToAgent_role();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role_action>> DeleteRole_actionAsync(List<Role_action> role_actionList)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.delete_data_role_actionAsync(role_actionList.Role_actionTypeToArray())).ArrayTypeToRole_action();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Privilege>> DeletePrivilegeAsync(List<Privilege> privilegeList)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.delete_data_privilegeAsync(privilegeList.PrivilegeTypeToArray())).ArrayTypeToPrivilege();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<ActionRecord>> UpdateActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.update_data_actionRecordAsync(listActionRecord.ActionRecordTypeToArray())).ArrayTypeToActionRecord();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role>> UpdateRoleAsync(List<Role> listRole)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.update_data_roleAsync(listRole.RoleTypeToArray())).ArrayTypeToRole();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> UpdateActionAsync(List<QOBDCommon.Entities.Action> listAction)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = (await _channel.update_data_actionAsync(listAction.ActionTypeToArray())).ArrayTypeToAction();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent_role>> UpdateAgent_roleAsync(List<Agent_role> agent_roleList)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.update_data_agent_roleAsync(agent_roleList.Agent_roleTypeToArray())).ArrayTypeToAgent_role();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role_action>> UpdateRole_actionAsync(List<Role_action> role_actionList)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.update_data_role_actionAsync(role_actionList.Role_actionTypeToArray())).ArrayTypeToRole_action();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Privilege>> UpdatePrivilegeAsync(List<Privilege> privilegeList)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.update_data_privilegeAsync(privilegeList.PrivilegeTypeToArray())).ArrayTypeToPrivilege();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<ActionRecord>> GetActionRecordDataAsync(int nbLine)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.get_data_actionRecordAsync(nbLine.ToString())).ArrayTypeToActionRecord();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<ActionRecord>> GetActionRecordDataByIdAsync(int id)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.get_data_actionRecord_by_idAsync(id.ToString())).ArrayTypeToActionRecord();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role>> GetRoleDataAsync(int nbLine)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.get_data_roleAsync(nbLine.ToString())).ArrayTypeToRole();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role>> GetRoleDataByIdAsync(int id)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.get_data_role_by_idAsync(id.ToString())).ArrayTypeToRole();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> GetActionDataAsync(int nbLine)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = (await _channel.get_data_actionAsync(nbLine.ToString())).ArrayTypeToAction();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> GetActionDataByIdAsync(int id)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = (await _channel.get_data_action_by_idAsync(id.ToString())).ArrayTypeToAction();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent_role>> GetAgent_roleDataAsync(int nbLine)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.get_data_agent_roleAsync(nbLine.ToString())).ArrayTypeToAgent_role();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent_role>> GetAgent_roleDataByIdAsync(int id)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.get_data_agent_role_by_idAsync(id.ToString())).ArrayTypeToAgent_role();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role_action>> GetRole_actionDataAsync(int nbLine)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.get_data_role_actionAsync(nbLine.ToString())).ArrayTypeToRole_action();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role_action>> GetRole_actionDataByIdAsync(int id)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.get_data_role_action_by_idAsync(id.ToString())).ArrayTypeToRole_action();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Privilege>> GetPrivilegeDataAsync(int nbLine)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.get_data_privilegeAsync(nbLine.ToString())).ArrayTypeToPrivilege();

            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Privilege>> GetPrivilegeDataByIdAsync(int id)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.get_data_privilege_by_idAsync(id.ToString())).ArrayTypeToPrivilege();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<ActionRecord>> searchActionRecordAsync(ActionRecord ActionRecord, ESearchOption filterOperator)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.get_filter_actionRecordAsync(ActionRecord.ActionRecordTypeToFilterArray(filterOperator))).ArrayTypeToActionRecord();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role>> searchRoleAsync(Role Role, ESearchOption filterOperator)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.get_filter_roleAsync(Role.RoleTypeToFilterArray(filterOperator))).ArrayTypeToRole();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> searchActionAsync(QOBDCommon.Entities.Action Action, ESearchOption filterOperator)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = (await _channel.get_filter_actionAsync(Action.ActionTypeToFilterArray(filterOperator))).ArrayTypeToAction();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Role_action>> searchRole_actionAsync(Role_action Role_action, ESearchOption filterOperator)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.get_filter_role_actionAsync(Role_action.Role_actionTypeToFilterArray(filterOperator))).ArrayTypeToRole_action();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Privilege>> searchPrivilegeAsync(Privilege Privilege, ESearchOption filterOperator)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.get_filter_privilegeAsync(Privilege.PrivilegeTypeToFilterArray(filterOperator))).ArrayTypeToPrivilege();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent_role>> searchAgent_roleAsync(Agent_role Agent_role, ESearchOption filterOperator)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.get_filter_agent_roleAsync(Agent_role.Agent_roleTypeToFilterArray(filterOperator))).ArrayTypeToAgent_role();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public void Dispose()
        {
            _channel.Close();
        }
    } /* end class BlSecurity */
}