using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDDAL.Core
{
    public class DALSecurity : ISecurityManager
    {
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.ISecurityManager _gateWaySecurity;
        private QOBDWebServicePortTypeClient _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private Func<double, double> _progressBarFunc;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALSecurity(QOBDWebServicePortTypeClient servicePort)
        {
            _servicePortType = servicePort;
            _gateWaySecurity = new GateWaySecurity(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            _loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
            _gateWaySecurity.setServiceCredential(_servicePortType);

        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (QOBDWebServicePortTypeClient)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.Login;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWaySecurity.setServiceCredential(_servicePortType);
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }
        

        public async Task<Agent> AuthenticateUserAsync(string username, string password , bool isClearPassword = true)
        {
            _servicePortType.ClientCredentials.UserName.UserName = username;
            _servicePortType.ClientCredentials.UserName.Password = password;
            return await _gateWaySecurity.AuthenticateUserAsync(username, password);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }

        public async Task<List<ActionRecord>> InsertActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            return await _gateWaySecurity.InsertActionRecordAsync(listActionRecord);
        }

        public async Task<List<Role>> InsertRoleAsync(List<Role> listRole)
        {
            return await _gateWaySecurity.InsertRoleAsync(listRole);
        }

        public async Task<List<QOBDCommon.Entities.Action>> InsertActionAsync(List<QOBDCommon.Entities.Action> listAction)
        {
            return await _gateWaySecurity.InsertActionAsync(listAction);
        }

        public async Task<List<Agent_role>> InsertAgent_roleAsync(List<Agent_role> listAgent_role)
        {
            return await _gateWaySecurity.InsertAgent_roleAsync(listAgent_role);
        }

        public async Task<List<Role_action>> InsertRole_actionAsync(List<Role_action> listRole_action)
        {
            return await _gateWaySecurity.InsertRole_actionAsync(listRole_action);
        }

        public async Task<List<Privilege>> InsertPrivilegeAsync(List<Privilege> listPrivilege)
        {
            return await _gateWaySecurity.InsertPrivilegeAsync(listPrivilege);
        }

        public async Task<List<ActionRecord>> DeleteActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            return await _gateWaySecurity.DeleteActionRecordAsync(listActionRecord);
        }

        public async Task<List<Role>> DeleteRoleAsync(List<Role> listRole)
        {
            return await _gateWaySecurity.DeleteRoleAsync(listRole);
        }

        public async Task<List<QOBDCommon.Entities.Action>> DeleteActionAsync(List<QOBDCommon.Entities.Action> listAction)
        {
            return await _gateWaySecurity.DeleteActionAsync(listAction);
        }

        public async Task<List<Agent_role>> DeleteAgent_roleAsync(List<Agent_role> listAgent_role)
        {
            return await _gateWaySecurity.DeleteAgent_roleAsync(listAgent_role);
        }

        public async Task<List<Role_action>> DeleteRole_actionAsync(List<Role_action> listRole_action)
        {
            return await _gateWaySecurity.DeleteRole_actionAsync(listRole_action);
        }

        public async Task<List<Privilege>> DeletePrivilegeAsync(List<Privilege> listPrivilege)
        {
            return await _gateWaySecurity.DeletePrivilegeAsync(listPrivilege);
        }

        public async Task<List<ActionRecord>> UpdateActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            return await _gateWaySecurity.UpdateActionRecordAsync(listActionRecord);
        }

        public async Task<List<Role>> UpdateRoleAsync(List<Role> listRole)
        {
            return await _gateWaySecurity.UpdateRoleAsync(listRole);
        }

        public async Task<List<QOBDCommon.Entities.Action>> UpdateActionAsync(List<QOBDCommon.Entities.Action> listAction)
        {
            return await _gateWaySecurity.UpdateActionAsync(listAction);
        }

        public async Task<List<Agent_role>> UpdateAgent_roleAsync(List<Agent_role> listAgent_role)
        {
            return await _gateWaySecurity.UpdateAgent_roleAsync(listAgent_role);
        }

        public async Task<List<Role_action>> UpdateRole_actionAsync(List<Role_action> listRole_action)
        {
            return await _gateWaySecurity.UpdateRole_actionAsync(listRole_action);
        }

        public async Task<List<Privilege>> UpdatePrivilegeAsync(List<Privilege> listPrivilege)
        {
            return await _gateWaySecurity.UpdatePrivilegeAsync(listPrivilege);
        }

        public async Task<List<ActionRecord>> GetActionRecordDataAsync(int nbLine)
        {
            return await _gateWaySecurity.GetActionRecordDataAsync(nbLine);
        }

        public async Task<List<ActionRecord>> GetActionRecordDataByIdAsync(int id)
        {
            return await _gateWaySecurity.GetActionRecordDataByIdAsync(id);
        }

        public async Task<List<Role>> GetRoleDataAsync(int nbLine)
        {
            return await _gateWaySecurity.GetRoleDataAsync(nbLine);
        }

        public async Task<List<Role>> GetRoleDataByIdAsync(int id)
        {
            return await _gateWaySecurity.GetRoleDataByIdAsync(id);
        }

        public async Task<List<QOBDCommon.Entities.Action>> GetActionDataAsync(int nbLine)
        {
            return await _gateWaySecurity.GetActionDataAsync(nbLine);
        }

        public async Task<List<QOBDCommon.Entities.Action>> GetActionDataByIdAsync(int id)
        {
            return await _gateWaySecurity.GetActionDataByIdAsync(id);
        }

        public async Task<List<Agent_role>> GetAgent_roleDataAsync(int nbLine)
        {
            return await _gateWaySecurity.GetAgent_roleDataAsync(nbLine);
        }

        public async Task<List<Agent_role>> GetAgent_roleDataByIdAsync(int id)
        {
            return await _gateWaySecurity.GetAgent_roleDataByIdAsync(id);
        }

        public async Task<List<Role_action>> GetRole_actionDataAsync(int nbLine)
        {
            return await _gateWaySecurity.GetRole_actionDataAsync(nbLine);
        }

        public async Task<List<Role_action>> GetRole_actionDataByIdAsync(int id)
        {
            return await _gateWaySecurity.GetRole_actionDataByIdAsync(id);
        }

        public async Task<List<Privilege>> GetPrivilegeDataAsync(int nbLine)
        {
            return await _gateWaySecurity.GetPrivilegeDataAsync(nbLine);
        }

        public async Task<List<Privilege>> GetPrivilegeDataByIdAsync(int id)
        {
            return await _gateWaySecurity.GetPrivilegeDataByIdAsync(id);
        }

        public Agent GetAuthenticatedUser()
        {
            return AuthenticatedUser;
        }

        public async Task<List<ActionRecord>> searchActionRecordAsync(ActionRecord ActionRecord, ESearchOption filterOperator)
        {
            return await _gateWaySecurity.searchActionRecordAsync(ActionRecord, filterOperator);
        }

        public async Task<List<Role>> searchRoleAsync(Role Role, ESearchOption filterOperator)
        {
            return await _gateWaySecurity.searchRoleAsync(Role, filterOperator);
        }

        public async Task<List<QOBDCommon.Entities.Action>> searchActionAsync(QOBDCommon.Entities.Action Action, ESearchOption filterOperator)
        {
            return await _gateWaySecurity.searchActionAsync(Action, filterOperator);
        }

        public async Task<List<Agent_role>> searchAgent_roleAsync(Agent_role Agent_role, ESearchOption filterOperator)
        {
            return await _gateWaySecurity.searchAgent_roleAsync(Agent_role, filterOperator);
        }

        public async Task<List<Role_action>> searchRole_actionAsync(Role_action Role_action, ESearchOption filterOperator)
        {
            return await _gateWaySecurity.searchRole_actionAsync(Role_action, filterOperator);
        }

        public async Task<List<Privilege>> searchPrivilegeAsync(Privilege Privilege, ESearchOption filterOperator)
        {
            return await _gateWaySecurity.searchPrivilegeAsync(Privilege, filterOperator);
        }

        public void Dispose()
        {
            _gateWaySecurity.Dispose();
        }


    } /* end class BlSecurity */
}