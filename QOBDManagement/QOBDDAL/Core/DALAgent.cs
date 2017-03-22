using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using QOBDGateway.QOBDServiceReference;
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
using QOBDDAL.Classes;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;

namespace QOBDDAL.Core
{
    public class DALAgent : IAgentManager
    {
        private Func<double, double> _rogressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IAgentManager _gateWayAgent;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALAgent(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gateWayAgent = new GateWayAgent(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);

        }

        public DALAgent(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public DALAgent(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayAgent.setServiceCredential(_servicePortType);                
            }
        }

        public async void cacheWebServiceData()
        {
            await retrieveGateWayAgentDataAsync();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayAgent.setServiceCredential(_servicePortType);
        }

        public async Task retrieveGateWayAgentDataAsync()
        {
            try
            {
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                checkServiceCommunication();
                // getting agents without their credentials (_loadSize < 0)
                var agentList = await _gateWayAgent.GetAgentDataAsync(-1 * _loadSize);                

                if (agentList.Count > 0)
                    LoadAgent(agentList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.AGENT);
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

        private void checkServiceCommunication()
        {
            if (_servicePortType.State == System.ServiceModel.CommunicationState.Closed || _servicePortType.State == System.ServiceModel.CommunicationState.Faulted)
                _serviceCommunication.resetCommunication();
        }

        #region [ Actions ]
        //=================================[ Actions ]================================================

        public async Task<List<Agent>> InsertAgentAsync(List<Agent> listAgent)
        {
            checkServiceCommunication();
            List<Agent> gateWayResultList = await _gateWayAgent.InsertAgentAsync(listAgent);
            return LoadAgent(gateWayResultList);
        }

        public async Task<List<Agent>> DeleteAgentAsync(List<Agent> listAgent)
        {
            List<Agent> result = new List<Agent>();
            checkServiceCommunication();
            List<Agent> gateWayResultList = await _gateWayAgent.DeleteAgentAsync(listAgent);
            if (gateWayResultList.Count == 0)
                foreach (Agent agent in gateWayResultList)
                {
                    int returnResult = _dataSet.DeleteAgent(agent.ID);
                    if (returnResult == 0)
                        result.Add(agent);
                }

            return result;
        }

        public async Task<List<Agent>> UpdateAgentAsync(List<Agent> agentList)
        {
            List<Agent> result = new List<Agent>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Agent> gateWayResultList = await _gateWayAgent.UpdateAgentAsync(agentList);

            foreach (var agent in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillAgentDataTableById(dataSetLocal.agents, agent.ID);
                dataSet.agents.Merge(dataSetLocal.agents);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateAgent(gateWayResultList.AgentTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Agent> LoadAgent(List<Agent> agentsList)
        {
            List<Agent> result = new List<Agent>();

            foreach (var agent in agentsList)
            {
                int returnResult = _dataSet.LoadAgent(agent);
                if (returnResult > 0)
                    result.Add(agent);
            }

            return result;
        }

        public List<Agent> GetAgentData(int nbLine)
        {
            List<Agent> result = new List<Agent>();

            result = _dataSet.GetAgentData();

            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Agent>> GetAgentDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayAgent.GetAgentDataAsync(nbLine);
        }

        public List<Agent> GetAgentDataById(int id)
        {
            return _dataSet.GetAgentDataById(id);
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
            checkServiceCommunication();
            return await _gateWayAgent.GetAgentDataByOrderListAsync(orderList);
        }

        public List<Agent> searchAgent(Agent agent, ESearchOption filterOperator)
        {
            if(_servicePortType.State != System.ServiceModel.CommunicationState.Faulted)
            return _dataSet.searchAgent(agent, filterOperator);
            else
            {
                _serviceCommunication.resetCommunication();
                return searchAgent(agent, filterOperator);
            }
        }

        public async Task<List<Agent>> searchAgentAsync(Agent agent, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayAgent.searchAgentAsync(agent, filterOperator);
        }

        #endregion

        public void Dispose()
        {
            if (_gateWayAgent != null)
                _gateWayAgent.Dispose();
        }
    } /* end class BLAgent */
}