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
    public class BLStatisitc : IStatisticManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC;

        public event PropertyChangedEventHandler PropertyChanged;

        public BLStatisitc(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }


        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALStatistic.progressBarManagement(progressBarFunc);
        }

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALStatistic.initializeCredential(user);
        }

        public void setServiceCredential(object channel)
        {
            DAC.DALStatistic.setServiceCredential(channel);
        }


        public async Task<List<Statistic>> InsertStatisticAsync(List<Statistic> statisticList)
        {
            if (statisticList == null || statisticList.Count == 0)
                return new List<Statistic>();

            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.InsertStatisticAsync(statisticList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Statistic>> DeleteStatisticAsync(List<Statistic> statisticList)
        {
            if (statisticList == null || statisticList.Count == 0)
                return new List<Statistic>();
            
            if (statisticList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting statistics(count = " + statisticList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.DeleteStatisticAsync(statisticList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Statistic>> UpdateStatisticAsync(List<Statistic> statisticList)
        {
            if (statisticList == null || statisticList.Count == 0)
                return new List<Statistic>();

            if (statisticList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating statistics(count = " + statisticList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.UpdateStatisticAsync(statisticList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Statistic> GetStatisticData(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = DAC.DALStatistic.GetStatisticData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Statistic>> GetStatisticDataAsync(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.GetStatisticDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public  List<Statistic> GetStatisticDataById(int id)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = DAC.DALStatistic.GetStatisticDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Statistic>> searchStatisticAsync(Statistic statistic, ESearchOption filterOperator)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.searchStatisticAsync(statistic, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public  List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = DAC.DALStatistic.searchStatistic(statistic, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void Dispose()
        {
            DAC.DALStatistic.Dispose();
        }
    } /* end class BLStatisitc */
}