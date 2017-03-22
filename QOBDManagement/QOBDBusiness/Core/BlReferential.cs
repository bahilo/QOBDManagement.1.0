using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
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
    public class BlReferential : IReferentialManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC;

        public event PropertyChangedEventHandler PropertyChanged;

        public BlReferential(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALReferential.initializeCredential(user);
        }

        public void cacheWebServiceData()
        {
            DAC.DALReferential.cacheWebServiceData();
        }


        public void setServiceCredential(object channel)
        {
            DAC.DALReferential.setServiceCredential(channel);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALReferential.progressBarManagement(progressBarFunc);
        }

        public async Task<List<Info>> InsertInfoAsync(List<Info> infosList)
        {
            if (infosList == null || infosList.Count == 0)
                return new List<Info>();

            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.InsertInfoAsync(infosList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public async Task<List<Info>> DeleteInfoAsync(List<Info> infosList)
        {
            List<Info> result = new List<Info>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(infosList.Where(x => x.ID == 0).Count()))
                infosList = infosList.Where(x => x.ID != 0).ToList();

            if (infosList == null || infosList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALReferential.DeleteInfoAsync(infosList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public async Task<List<Info>> UpdateInfoAsync(List<Info> infosList)
        {
            List<Info> result = new List<Info>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(infosList.Where(x => x.ID == 0).Count()))
                infosList = infosList.Where(x => x.ID != 0).ToList();

            if (infosList == null || infosList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALReferential.UpdateInfoAsync(infosList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public List<Info> GetInfoData(int nbLine)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = DAC.DALReferential.GetInfoData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public async Task<List<Info>> GetInfoDataAsync(int nbLine)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.GetInfoDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public List<Info> GetInfosDataById(int id)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = DAC.DALReferential.GetInfosDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public async Task<List<Info>> searchInfoAsync(Info infos, ESearchOption filterOperator)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.searchInfoAsync(infos, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public List<Info> searchInfo(Info Infos, ESearchOption filterOperator)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = DAC.DALReferential.searchInfo(Infos, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.REFERENTIAL); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALReferential.Dispose();
        }

        private bool checkIfUpdateOrDeleteParamRepectsRequirements(int IDValues, [CallerMemberName] string functionName = null)
        {
            bool isRequirementsRespected = true;
            if (IDValues > 0)
            {
                isRequirementsRespected = false;
                Log.warning(functionName + " params (count = " + IDValues + ") with ID = 0", EErrorFrom.REFERENTIAL);
            }
            return isRequirementsRespected;
        }
    } /* end class BlReferential */
}