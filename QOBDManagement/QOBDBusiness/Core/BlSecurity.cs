using QOBDBusiness.Helper;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceModel;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDBusiness.Core
{
    public class BlSecurity : ISecurityManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC { get; private set; }
        public Safe Safe { get; private set; }

        public BlSecurity(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
            Safe = new Safe();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<Agent> AuthenticateUserAsync(string username, string password, bool isClearPassword = true)
        {
            try
            {
                if(isClearPassword)
                    Safe.AuthenticatedUser = await DAC.DALSecurity.AuthenticateUserAsync(username, CalculateHash(password));  //DAC.DALSecurity.AuthenticateUser(username, CalculateHash(password));
                else
                    Safe.AuthenticatedUser = await DAC.DALSecurity.AuthenticateUserAsync(username, password);  //DAC.DALSecurity.AuthenticateUser(username, CalculateHash(password));

                if(Safe.AuthenticatedUser.ID != 0)
                    Safe.IsAuthenticated = true;
            }
            catch (CommunicationException ex)
            {
                Safe.IsAuthenticated = false;
                Log.warning(ex.Message);
                throw new ApplicationException("Remote communication error.");
            }
            catch (Exception ex)
            {                                
                Safe.IsAuthenticated = false;
                Log.error(ex.Message);              
            }      
            
            if(!Safe.IsAuthenticated)
            {
                return Safe.AuthenticatedUser;
            }

            return Safe.AuthenticatedUser;
        }

        public void setServiceCredential(object channel)
        {
            DAC.DALSecurity.setServiceCredential(channel);
        }

        public string CalculateHash(string clearTextPassword)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + (int)ESecurity.Salt);
            // Use the hash algorithm to calculate the hash
            MD5 algorithm = MD5.Create();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash string
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public Agent UseAgent(Agent inAgent)
        {
            //var result = DAC.DALAgent.UseAgent(inAgent);
            Agent result = new Agent();
            try
            {
                //result = BlSecurity;
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> DisableAgent(List<Agent> agentList)
        {
            List<Agent> result = new List<Agent>();
            if (agentList == null)
                return result;

            try
            {
                foreach (Agent agent in agentList)
                    agent.Status = EStatus.Deactivated.ToString();

                result = await DAC.DALAgent.UpdateAgentAsync(agentList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> EnableAgent(List<Agent> agentList)
        {
            List<Agent> result = new List<Agent>();
            if (agentList == null)
                return result;
            try
            {
                foreach (Agent agent in agentList)
                    agent.Status = EStatus.Active.ToString();

                result = await DAC.DALAgent.UpdateAgentAsync(agentList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALSecurity.initializeCredential(user);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALSecurity.progressBarManagement(progressBarFunc);
        }

        public Agent GetAuthenticatedUser()
        {
            return Safe.AuthenticatedUser;
        }

        public bool IsUserAuthenticated()
        {
            return Safe.IsAuthenticated;
        }

        public async Task DisconnectAuthenticatedUser()
        {
            Safe.AuthenticatedUser.IsOnline = false;
            await DAC.DALAgent.UpdateAgentAsync(new List<Agent> { Safe.AuthenticatedUser });
            Safe.AuthenticatedUser = new Agent();
            Safe.IsAuthenticated = false;
        }

        public async Task<List<ActionRecord>> InsertActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.InsertActionRecordAsync(listActionRecord);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role>> InsertRoleAsync(List<Role> Rolelist)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.InsertRoleAsync(Rolelist);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> InsertActionAsync(List<QOBDCommon.Entities.Action> listAction)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.InsertActionAsync(listAction);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent_role>> InsertAgent_roleAsync(List<Agent_role> Agent_rolelist)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.InsertAgent_roleAsync(Agent_rolelist);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role_action>> InsertRole_actionAsync(List<Role_action> listRole_action)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.InsertRole_actionAsync(listRole_action);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Privilege>> InsertPrivilegeAsync(List<Privilege> listPrivilege)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.InsertPrivilegeAsync(listPrivilege);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<ActionRecord>> DeleteActionRecordAsync(List<ActionRecord> listActionRecord)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.DeleteActionRecordAsync(listActionRecord);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role>> DeleteRoleAsync(List<Role> RoleList)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.DeleteRoleAsync(RoleList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> DeleteActionAsync(List<QOBDCommon.Entities.Action> ActionList)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.DeleteActionAsync(ActionList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent_role>> DeleteAgent_roleAsync(List<Agent_role> listAgent_role)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.DeleteAgent_roleAsync(listAgent_role);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role_action>> DeleteRole_actionAsync(List<Role_action> listRole_action)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.DeleteRole_actionAsync(listRole_action);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Privilege>> DeletePrivilegeAsync(List<Privilege> listPrivilege)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.DeletePrivilegeAsync(listPrivilege);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<ActionRecord>> UpdateActionRecordAsync(List<ActionRecord> ActionRecordList)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.UpdateActionRecordAsync(ActionRecordList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role>> UpdateRoleAsync(List<Role> RoleList)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.UpdateRoleAsync(RoleList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> UpdateActionAsync(List<QOBDCommon.Entities.Action> ActionList)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.UpdateActionAsync(ActionList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role_action>> UpdateRole_actionAsync(List<Role_action> Role_actionList)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.UpdateRole_actionAsync(Role_actionList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Privilege>> UpdatePrivilegeAsync(List<Privilege> PrivilegeList)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.UpdatePrivilegeAsync(PrivilegeList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent_role>> UpdateAgent_roleAsync(List<Agent_role> Agent_roleList)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.UpdateAgent_roleAsync(Agent_roleList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<ActionRecord>> GetActionRecordDataAsync(int nbLine)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.GetActionRecordDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<ActionRecord>> GetActionRecordDataByIdAsync(int id)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.GetActionRecordDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role>> GetRoleDataAsync(int nbLine)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.GetRoleDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role>> GetRoleDataByIdAsync(int id)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.GetRoleDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> GetActionDataAsync(int nbLine)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.GetActionDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> GetActionDataByIdAsync(int id)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.GetActionDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent_role>> GetAgent_roleDataAsync(int nbLine)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.GetAgent_roleDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent_role>> GetAgent_roleDataByIdAsync(int id)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.GetAgent_roleDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role_action>> GetRole_actionDataAsync(int nbLine)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.GetRole_actionDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role_action>> GetRole_actionDataByIdAsync(int id)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.GetRole_actionDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Privilege>> GetPrivilegeDataAsync(int nbLine)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.GetPrivilegeDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Privilege>> GetPrivilegeDataByIdAsync(int id)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.GetPrivilegeDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<ActionRecord>> searchActionRecordAsync(ActionRecord ActionRecord, ESearchOption filterOperator)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.searchActionRecordAsync(ActionRecord, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role>> searchRoleAsync(Role Role, ESearchOption filterOperator)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.searchRoleAsync(Role, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<QOBDCommon.Entities.Action>> searchActionAsync(QOBDCommon.Entities.Action Action, ESearchOption filterOperator)
        {
            List<QOBDCommon.Entities.Action> result = new List<QOBDCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.searchActionAsync(Action, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent_role>> searchAgent_roleAsync(Agent_role Agent_role, ESearchOption filterOperator)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.searchAgent_roleAsync(Agent_role, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Role_action>> searchRole_actionAsync(Role_action Role_action, ESearchOption filterOperator)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.searchRole_actionAsync(Role_action, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Privilege>> searchPrivilegeAsync(Privilege Privilege, ESearchOption filterOperator)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.searchPrivilegeAsync(Privilege, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALSecurity.Dispose();
        }
    }
}