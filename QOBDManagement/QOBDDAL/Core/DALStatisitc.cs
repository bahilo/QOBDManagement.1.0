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
        private QOBDWebServicePortTypeClient _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private Func<double, double> _rogressBarFunc;
        private object _lock;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALStatisitc(QOBDWebServicePortTypeClient servicePort)
        {
            _lock = new object();
            _servicePortType = servicePort;
            _gateWayStatistic = new GateWayStatistic(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            _gateWayStatistic.setServiceCredential(_servicePortType);
            retrieveGateWayStatisticData();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (QOBDWebServicePortTypeClient)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.Login;
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

        public void retrieveGateWayStatisticData()
        {
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                var statisticList = new NotifyTaskCompletion<List<Statistic>>(_gateWayStatistic.GetStatisticDataAsync(_loadSize)).Task.Result;
                if (statisticList.Count > 0)
                    LoadStatistic(statisticList);
                //Log.debug("-- Statistics loaded --");
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
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

        public async Task<List<Statistic>> InsertStatisticAsync(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            List<Statistic> gateWayResultList = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            {
                gateWayResultList = await _gateWayStatistic.InsertStatisticAsync(statisticList);
                
                result = LoadStatistic(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Statistic>> UpdateStatisticAsync(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            List<Statistic> gateWayResultList = new List<Statistic>();
            QOBDSet dataSet = new QOBDSet();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            {
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await _gateWayStatistic.UpdateStatisticAsync(statisticList) : statisticList;

                foreach (var statistic in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _statisticsTableAdapter.FillById(dataSetLocal.statistics, statistic.ID);
                    dataSet.statistics.Merge(dataSetLocal.statistics);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _statisticsTableAdapter.Update(gateWayResultList.StatisticTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Statistic> LoadStatistic(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            {
                foreach (var statistic in statisticList)
                {
                    int returnResult = _statisticsTableAdapter
                                            .load_data_statistic(
                                                statistic.InvoiceDate,
                                                statistic.InvoiceId,
                                                statistic.Company,
                                                statistic.Price_purchase_total,
                                                statistic.Total,
                                                statistic.Total_tax_included,
                                                statistic.Income_percent,
                                                statistic.Income,
                                                statistic.Pay_received,
                                                statistic.Date_limit,
                                                statistic.Pay_date,
                                                statistic.Tax_value,
                                                statistic.ID);
                    if (returnResult > 0)
                        result.Add(statistic);
                }
            }
            return result;
        }

        public async Task<List<Statistic>> DeleteStatisticAsync(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            List<Statistic> gateWayResultList = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            {
                gateWayResultList = await _gateWayStatistic.DeleteStatisticAsync(statisticList);
                if (gateWayResultList.Count == 0)
                    foreach (Statistic statistic in gateWayResultList)
                    {
                        int returnResult = _statisticsTableAdapter.Delete1(statistic.ID);
                        if (returnResult == 0)
                            result.Add(statistic);
                    }
            }
            return result;
        }

        public List<Statistic> GetStatisticData(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
                result = _statisticsTableAdapter.GetData().DataTableTypeToStatistic();
            if (nbLine == 999 || result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Statistic>> GetStatisticDataAsync(int nbLine)
        {
            return await _gateWayStatistic.GetStatisticDataAsync(nbLine);
        }

        public List<Statistic> GetStatisticDataById(int id)
        {
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
                return _statisticsTableAdapter.get_statistic_by_id(id).DataTableTypeToStatistic();
        }

        public List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator)
        {
            return statistic.FilterDataTableToStatisticType(filterOperator);
        }

        public async Task<List<Statistic>> searchStatisticAsync(Statistic statistic, ESearchOption filterOperator)
        {
            return await _gateWayStatistic.searchStatisticAsync(statistic, filterOperator);
        }

        public void Dispose()
        {
            _gateWayStatistic.Dispose();
        }
    } /* end class BLStatisitc */
}