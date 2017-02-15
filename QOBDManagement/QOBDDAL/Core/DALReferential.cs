using QOBDCommon.Entities;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.Helper.ChannelHelper;
using QOBDGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;
using QOBDCommon.Classes;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDCommon.Enum;
using QOBDGateway.QOBDServiceReference;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDDAL.Core
{
    public class DALReferential : IReferentialManager
    {
        private QOBDCommon.Interfaces.REMOTE.IReferentialManager _gateWayReferential;
        private QOBDWebServicePortTypeClient _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private object _lock = new object();
        private int _progressStep;
        private Func<double, double> _rogressBarFunc;

        public event PropertyChangedEventHandler PropertyChanged;

        public Agent AuthenticatedUser { get; set; }


        public DALReferential(QOBDWebServicePortTypeClient servicePort)
        {
            _servicePortType = servicePort;
            _gateWayReferential = new GateWayReferential(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);            
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsLodingDataFromWebServiceToLocal"); }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayReferential.setServiceCredential(_servicePortType);
                retrieveGateWayReferentialData();
            }
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (QOBDWebServicePortTypeClient)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.Login;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayReferential.setServiceCredential(_servicePortType);
        }

        private void retrieveGateWayReferentialData()
        {
            object _lock = new object();

            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                ConcurrentBag<Info> infosList = new ConcurrentBag<Info>(new NotifyTaskCompletion<List<Info>>(_gateWayReferential.GetInfoDataAsync(_loadSize)).Task.Result);
                List<Info> savedInfosList = LoadInfos(infosList.ToList());

                //Log.debug("-- Referentials loaded --");
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }
            finally
            {
                lock (_lock)
                {
                    IsLodingDataFromWebServiceToLocal = false;
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                }
            }
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Info>> InsertInfoAsync(List<Info> listInfos)
        {
            List<Info> result = new List<Info>();
            List<Info> gateWayResultList = new List<Info>();
            using (infosTableAdapter _infossTableAdapter = new infosTableAdapter())
            {
                gateWayResultList = await _gateWayReferential.InsertInfoAsync(listInfos);
                
                result = LoadInfos(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Info>> DeleteInfoAsync(List<Info> listInfos)
        {
            List<Info> result = new List<Info>();
            List<Info> gateWayResultList = new List<Info>();
            using (infosTableAdapter _infosTableAdapter = new infosTableAdapter())
            {
                gateWayResultList = await _gateWayReferential.DeleteInfoAsync(listInfos);
                if (gateWayResultList.Count == 0)
                {
                    foreach (Info infos in listInfos)
                    {
                        int returnValue = _infosTableAdapter.Delete1(infos.ID);
                        if (returnValue == 0)
                            result.Add(infos);
                    }
                }

            }
            return result;
        }

        public List<Language> DeleteLanguageInfos(List<Language> languageList)
        {
            List<Language> result = new List<Language>();
            using (LanguagesTableAdapter _languagesTableAdapter = new LanguagesTableAdapter())
            {
                foreach (Language lang in languageList)
                {
                    int returnValue = _languagesTableAdapter.Delete1(lang.ID);
                    if (returnValue == 0)
                        result.Add(lang);
                }
            }
            return result;
        }

        public async Task<List<Info>> UpdateInfoAsync(List<Info> infoList)
        {
            List<Info> result = new List<Info>();
            List<Info> gateWayResultList = new List<Info>();
            QOBDSet dataSet = new QOBDSet();
            using (infosTableAdapter _InfosTableAdapter = new infosTableAdapter())
            {
                gateWayResultList = await _gateWayReferential.UpdateInfoAsync(infoList);

                foreach (var info in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _InfosTableAdapter.FillById(dataSetLocal.infos, info.ID);
                    dataSet.infos.Merge(dataSetLocal.infos);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _InfosTableAdapter.Update(gateWayResultList.InfosTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Info> LoadInfos(List<Info> infoList)
        {
            List<Info> result = new List<Info>();
            using (infosTableAdapter _InfosTableAdapter = new infosTableAdapter())
            {
                foreach (var Infos in infoList)
                {
                    int returnValue = _InfosTableAdapter
                            .load_data_infos(
                                Infos.Name,
                                Infos.Value,
                                Infos.ID);

                    if (returnValue > 0)
                        result.Add(Infos);
                }
            }
            return result;
        }

        public List<Language> UpdateLanguageInfos(List<Language> languageList)
        {
            List<Language> result = new List<Language>();
            using (LanguagesTableAdapter _languagesTableAdapter = new LanguagesTableAdapter())
            {
                int returnValue = _languagesTableAdapter.Update(languageList.LanguageTypeToDataTable());
                if (returnValue == languageList.Count)
                    result = languageList;
            }
            return result;
        }

        public List<Info> GetInfosData(int nbLine)
        {
            List<Info> result = new List<Info>();
            using (infosTableAdapter _infosTableAdapter = new infosTableAdapter())
                result = _infosTableAdapter.GetData().DataTableTypeToInfos();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Info>> GetInfoDataAsync(int nbLine)
        {
            return await _gateWayReferential.GetInfoDataAsync(nbLine);
        }

        public List<Info> GetInfosDataById(int id)
        {
            using (infosTableAdapter _infosTableAdapter = new infosTableAdapter())
                return _infosTableAdapter.get_infos_by_id(id).DataTableTypeToInfos();
        }

        public List<Info> searchInfos(Info Infos, ESearchOption filterOperator)
        {
            return Infos.FilterDataTableToInfoType(filterOperator);
        }

        public async Task<List<Info>> searchInfosAsync(Info Infos, ESearchOption filterOperator)
        {
            return await _gateWayReferential.searchInfosAsync(Infos, filterOperator);
        }

        public List<Language> searchLanguageInfos(Language language, ESearchOption filterOperator)
        {
            return language.langauageTypeToFilterDataTable(filterOperator);
        }

        public void Dispose()
        {
            _gateWayReferential.Dispose();
        }
    } /* end class BlReferential */
}