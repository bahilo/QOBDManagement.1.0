using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QOBDDAL.Core
{
    public class DALAgent : IAgentManager
    {
        private Func<double, double> _rogressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private GateWayAgent _gateWayAgent;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALAgent()
        {
            _gateWayAgent = new GateWayAgent();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayAgent.PropertyChanged += onCredentialChange_loadAgentDataFromWebService;

        }


        public GateWayAgent GateWayAgent
        {
            get { return _gateWayAgent; }
            set { _gateWayAgent = value; }
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        private void onCredentialChange_loadAgentDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayData);                
            }
        }        
        

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                //_loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gateWayAgent.initializeCredential(user);
            }                       
        }

        public void retrieveGateWayData() 
        {
            try
            { 
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                var agentList = new NotifyTaskCompletion<List<Agent>>(_gateWayAgent.GetAgentDataAsync(_loadSize)).Task.Result;
                if (agentList.Count > 0)
                {
                    var savedAgentList = LoadAgent(agentList);
                }
            }
            finally
            {
                lock (_lock)
                {
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    _isLodingDataFromWebServiceToLocal = false;
                }
            }          
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public async Task<List<Agent>> DeleteAgentAsync(List<Agent> listAgent)
        {
            List<Agent> result = listAgent;
            List<Agent> gateWayResultList = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            {
                _gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayAgent.DeleteAgentAsync(listAgent);
                if (gateWayResultList.Count == 0)
                    foreach (Agent agent in gateWayResultList)
                    {
                        int returnResult = _agentsTableAdapter.Delete1(agent.ID);
                        if (returnResult > 0)
                            result.Remove(agent);
                    }
            }
            return result;
        }

        public List<Agent> GetAgentData(int nbLine)
        {
            List<Agent> result = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
                result = _agentsTableAdapter.GetData().DataTableTypeToAgent();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Agent>> GetAgentDataAsync(int nbLine)
        {
            _gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayAgent.GetAgentDataAsync(nbLine);
        }

        public List<Agent> GetAgentDataById(int id)
        {
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
                return _agentsTableAdapter.get_agent_by_id(id).DataTableTypeToAgent();
        }

        public List<Agent> GetAgentDataByOrderList(List<Order> orderList)
        {
            List<Agent> result = new List<Agent>();
            foreach (Order order in orderList)
            {
                var agentList = searchAgent(new Agent { ID = order.AgentId }, ESearchOption.OR);
                if (agentList.Count() > 0)
                    result.Add(agentList.First());
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataByOrderListAsync(List<Order> orderList)
        {
            _gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayAgent.GetAgentDataByOrderListAsync(orderList);
        }


        public async Task<List<Agent>> InsertAgentAsync(List<Agent> listAgent)
        {
           List<Agent> result = new List<Agent>();
            List<Agent> gateWayResultList = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            {
                _gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayAgent.InsertAgentAsync(listAgent);
                
                result = LoadAgent(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Agent>> UpdateAgentAsync(List<Agent> agentList)
        {
            List<Agent> result = new List<Agent>();
            List<Agent> gateWayResultList = new List<Agent>();
            QOBDSet dataSet = new QOBDSet();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            {
                _gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await _gateWayAgent.UpdateAgentAsync(agentList);

                foreach (var agent in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _agentsTableAdapter.FillById(dataSetLocal.agents, agent.ID);
                    dataSet.agents.Merge(dataSetLocal.agents);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _agentsTableAdapter.Update(gateWayResultList.AgentTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Agent> LoadAgent(List<Agent> agentsList)
        {
            List<Agent> result = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            {
                foreach (var agent in agentsList)
                {
                    int returnResult = _agentsTableAdapter
                                            .load_data_agent(
                                                agent.LastName,
                                                agent.FirstName,
                                                agent.Phone,
                                                agent.Fax,
                                                agent.Email,
                                                agent.Login,
                                                agent.HashedPassword,
                                                agent.Admin,
                                                agent.Status,
                                                agent.ListSize,
                                                agent.ID);
                    if (returnResult > 0)
                        result.Add(agent);
                }
            }
            return result;
        }

        public List<Agent> searchAgent(Agent agent, ESearchOption filterOperator)
        {
            return agent.AgentTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Agent>> searchAgentAsync(Agent agent, ESearchOption filterOperator)
        {
            _gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _gateWayAgent.searchAgentAsync(agent, filterOperator);
        }

        public void Dispose()
        {
            _gateWayAgent.PropertyChanged -= onCredentialChange_loadAgentDataFromWebService;
            _gateWayAgent.Dispose();
        }

    } /* end class BLAgent */
}