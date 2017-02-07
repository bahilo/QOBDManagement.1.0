using QOBDBusiness.Helper;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.ComponentModel;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary> 
namespace QOBDBusiness.Core
{
    public class BLAgent : IAgentManager
    {
        // Attributes
        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC {get; set;}

        public BLAgent(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALAgent.initializeCredential(user);
        }


        public void setServiceCredential(string login, string password)
        {
            DAC.DALAgent.setServiceCredential(login, password);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALAgent.progressBarManagement(progressBarFunc);
        }

        public async Task<List<Agent>> InsertAgentAsync(List<Agent> agentList)
        {
            if (agentList == null || agentList.Count == 0)
                return new List<Agent>();
            
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.InsertAgentAsync(agentList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> UpdateAgentAsync(List<Agent> agentList)
        {
            if (agentList == null || agentList.Count == 0)
                return new List<Agent>();

            if (agentList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating agents(count = " + agentList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.UpdateAgentAsync(agentList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> DeleteAgentAsync(List<Agent> agentList)
        {
            if (agentList == null || agentList.Count == 0)
                return new List<Agent>();

            if (agentList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting agents(count = " + agentList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
                            
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.DeleteAgentAsync(agentList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Agent> GetAgentData(int nbLine)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = DAC.DALAgent.GetAgentData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataAsync(int nbLine)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.GetAgentDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Agent> GetAgentDataById(int id)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = DAC.DALAgent.GetAgentDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Agent> GetAgentDataByOrderList(List<Order> commandList)
        {
            List<Agent> result = new List<Agent>();
            if (commandList.Count == 0)
                return result;
            try
            {
                result = DAC.DALAgent.GetAgentDataByOrderList(commandList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataByOrderListAsync(List<Order> commandList)
        {
            List<Agent> result = new List<Agent>();
            if (commandList.Count == 0)
                return result;
            try
            {
                result = await DAC.DALAgent.GetAgentDataByOrderListAsync(commandList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }        

        public async Task<List<Client>> MoveAgentClient(Agent fromAgent, Agent toAgent)
        {
            List<Client> fromAgentClientListBefore = new List<Client>();
            List<Client> result = new List<Client>();
            
            try
            {
                fromAgentClientListBefore = await DAC.DALClient.searchClientAsync(new Client { AgentId=fromAgent.ID }, ESearchOption.AND);
                foreach (var client in fromAgentClientListBefore)
                {
                    client.AgentId = toAgent.ID;
                }
                if(fromAgentClientListBefore.Count > 0)
                    result = await DAC.DALClient.UpdateClientAsync(fromAgentClientListBefore);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALAgent.Dispose();
        }

        public List<Agent> searchAgent(Agent agent, ESearchOption filterOperator)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = DAC.DALAgent.searchAgent(agent, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Agent>> searchAgentAsync(Agent agent, ESearchOption filterOperator)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.searchAgentAsync(agent, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }
    } /* end class BLAgent */
}