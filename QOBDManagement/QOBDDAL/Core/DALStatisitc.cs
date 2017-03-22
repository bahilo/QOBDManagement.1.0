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
using System.Data.SqlClient;
using System.Threading.Tasks;
using QOBDDAL.Classes;
using QOBDDAL.Interfaces;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDDAL.Core
{
    public class DALStatisitc : IStatisticManager
    {
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IStatisticManager _gateWayStatistic;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private Func<double, double> _rogressBarFunc;
        private object _lock;
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALStatisitc(ClientProxy servicePort)
        {
            _lock = new object();
            _servicePortType = servicePort;
            _gateWayStatistic = new GateWayStatistic(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public DALStatisitc(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public DALStatisitc(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            _gateWayStatistic.setServiceCredential(_servicePortType);
        }

        public async void cacheWebServiceData()
        {
            await retrieveGateWayStatisticDataAsync();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayStatistic.setServiceCredential(_servicePortType);
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsLodingDataFromWebServiceToLocal"); }
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task retrieveGateWayStatisticDataAsync()
        {
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                checkServiceCommunication();
                var statisticList = await _gateWayStatistic.GetStatisticDataAsync(_loadSize);
                if (statisticList.Count > 0)
                    LoadStatistic(statisticList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.STATISTIC);
            }
            finally
            {
                lock (_lock)
                {
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    IsLodingDataFromWebServiceToLocal = false;
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
        public async Task<List<Statistic>> InsertStatisticAsync(List<Statistic> statisticList)
        {
            List<Statistic> gateWayResultList = new List<Statistic>();
            checkServiceCommunication();
            gateWayResultList = await _gateWayStatistic.InsertStatisticAsync(statisticList);
            List<Statistic> result = LoadStatistic(gateWayResultList);
            return result;
        }

        public async Task<List<Statistic>> UpdateStatisticAsync(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Statistic> gateWayResultList = await _gateWayStatistic.UpdateStatisticAsync(statisticList);

            foreach (var statistic in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillStatisticDataTableById(dataSetLocal.statistics, statistic.ID);
                dataSet.statistics.Merge(dataSetLocal.statistics);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateStatistic(gateWayResultList.StatisticTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Statistic> LoadStatistic(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            foreach (var statistic in statisticList)
            {
                int returnResult = _dataSet.LoadStatistic(statistic);
                if (returnResult > 0)
                    result.Add(statistic);
            }
            return result;
        }

        public async Task<List<Statistic>> DeleteStatisticAsync(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            checkServiceCommunication();
            List<Statistic> gateWayResultList = await _gateWayStatistic.DeleteStatisticAsync(statisticList);
            if (gateWayResultList.Count == 0)
                foreach (Statistic statistic in gateWayResultList)
                {
                    int returnResult = _dataSet.DeleteStatistic(statistic.ID);
                    if (returnResult == 0)
                        result.Add(statistic);
                }
            return result;
        }

        public List<Statistic> GetStatisticData(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            result = _dataSet.GetStatisticData();
            if (nbLine == 999 || result.Count == 0  || result.Count < nbLine)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Statistic>> GetStatisticDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayStatistic.GetStatisticDataAsync(nbLine);
        }

        public List<Statistic> GetStatisticDataById(int id)
        {
            return _dataSet.GetStatisticDataById(id);
        }

        public List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator)
        {
            return _dataSet.searchStatistic(statistic, filterOperator);
        }

        public async Task<List<Statistic>> searchStatisticAsync(Statistic statistic, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayStatistic.searchStatisticAsync(statistic, filterOperator);
        }
        #endregion

        public void Dispose()
        {
            if (_gateWayStatistic != null)
                _gateWayStatistic.Dispose();
        }
    } /* end class BLStatisitc */
}