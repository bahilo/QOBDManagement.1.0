using QOBDCommon.Entities;
using QOBDCommon.Interfaces.REMOTE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using QOBDGateway.Helper.ChannelHelper;
using System.ServiceModel;
using System.Linq;
using QOBDGateway.QOBDServiceReference;
using QOBDCommon.Enum;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWayReferential : IReferentialManager, INotifyPropertyChanged
    {
        private QOBDWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayReferential()
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

        public async Task<List<Info>> DeleteInfoAsync(List<Info> listInfos)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = (await _channel.delete_data_infosAsync(listInfos.InfosTypeToArray())).ArrayTypeToInfos();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Info>> InsertInfoAsync(List<Info> listInfos)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = (await _channel.insert_data_infosAsync(listInfos.InfosTypeToArray())).ArrayTypeToInfos();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Info>> UpdateInfoAsync(List<Info> listInfos)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = (await _channel.update_data_infosAsync(listInfos.InfosTypeToArray())).ArrayTypeToInfos();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Info>> GetInfoDataAsync(int nbLine)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = (await _channel.get_data_infosAsync(nbLine.ToString())).ArrayTypeToInfos().OrderBy(x => x.ID).ToList();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Info>> GetInfosDataById(int id)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = (await _channel.get_data_infos_by_idAsync(id.ToString())).ArrayTypeToInfos();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Info>> searchInfosAsync(Info Infos, ESearchOption filterOperator)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = (await _channel.get_filter_infosAsync(Infos.InfosTypeToFilterArray(filterOperator))).ArrayTypeToInfos();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Info>> searchInfosFromWebServiceAsync(Info infos, ESearchOption filterOperator)
        {
            return await searchInfosAsync(infos, filterOperator);
        }

        public void Dispose()
        {
            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }
    } /* end class BlReferential */
}