using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
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


        public void setServiceCredential(string login, string password)
        {
            DAC.DALReferential.setServiceCredential(login, password);
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
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Info>> DeleteInfoAsync(List<Info> infosList)
        {
            if (infosList == null || infosList.Count == 0)
                return new List<Info>();

            if (infosList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting Infos(count = " + infosList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.DeleteInfoAsync(infosList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Info>> UpdateInfoAsync(List<Info> infosList)
        {
            if (infosList == null || infosList.Count == 0)
                return new List<Info>();

            if (infosList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating Infos(count = " + infosList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.UpdateInfoAsync(infosList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Info> GetInfosData(int nbLine)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = DAC.DALReferential.GetInfosData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Info>> GetInfoDataAsync(int nbLine)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.GetInfoDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Info> GetInfosDataById(int id)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = DAC.DALReferential.GetInfosDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Info>> searchInfosAsync(Info infos, ESearchOption filterOperator)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = await DAC.DALReferential.searchInfosAsync(infos, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Info> searchInfos(Info Infos, ESearchOption filterOperator)
        {
            List<Info> result = new List<Info>();
            try
            {
                result = DAC.DALReferential.searchInfos(Infos, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALReferential.Dispose();
        }
    } /* end class BlReferential */
}