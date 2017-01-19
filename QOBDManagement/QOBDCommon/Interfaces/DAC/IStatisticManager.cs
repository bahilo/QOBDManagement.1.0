using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.DAC
{
    public interface IStatisticManager: REMOTE.IStatisticManager, INotifyPropertyChanged, IDisposable
    {
        void progressBarManagement(Func<double, double> progressBarFunc);

        void initializeCredential(Agent user);

        List<Statistic> GetStatisticData(int nbLine);

        List<Statistic> searchStatistic(Statistic statistic, ESearchOption filterOperator);

        List<Statistic> GetStatisticDataById(int id);
    } /* end interface IStatisticManager */
}