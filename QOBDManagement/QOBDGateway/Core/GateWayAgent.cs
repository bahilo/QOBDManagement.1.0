using QOBDCommon.Entities;
using QOBDCommon.Interfaces.REMOTE;
using QOBDGateway.Helper.ChannelHelper;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Linq;
using QOBDCommon.Enum;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWayAgent : IAgentManager
    {
        private QOBDWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;
        

        public GateWayAgent()
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
                onPropertyChange("Credential");
            }
        }


        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QOBDWebServicePortTypeClient("QOBDWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public QOBDWebServicePortTypeClient AgentGatWayChannel
        {
            get
            {
                return _channel;
            }
        }

        public async Task<List<Agent>> DeleteAgentAsync(List<Agent> listAgent)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.delete_data_agentAsync(listAgent.AgentTypeToArray())).ArrayTypeToAgent();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataAsync(int nbLine)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.get_data_agentAsync(nbLine.ToString())).ArrayTypeToAgent().OrderBy(x=>x.ID).ToList();
             }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataByIdAsync(int id)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.get_data_agent_by_idAsync(id.ToString())).ArrayTypeToAgent();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataByOrderListAsync(List<Order> commandList)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = (await _channel.get_data_agent_by_command_listAsync(commandList.OrderTypeToArray())).ArrayTypeToAgent();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Agent>> InsertAgentAsync(List<Agent> listAgent)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.insert_data_agentAsync(listAgent.AgentTypeToArray())).ArrayTypeToAgent();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent>> UpdateAgentAsync(List<Agent> listAgent)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.update_data_agentAsync(listAgent.AgentTypeToArray())).ArrayTypeToAgent();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Agent>> searchAgentAsync(Agent agent, ESearchOption filterOperator)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.get_filter_agentAsync(agent.AgentTypeToFilterArray(filterOperator))).ArrayTypeToAgent();
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
    } /* end class BLAgent */
}