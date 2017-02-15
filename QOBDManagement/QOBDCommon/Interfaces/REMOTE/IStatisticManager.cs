// FILE: D:/Just IT Training/BillManagment/Classes//IStatisticManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000A26 begin
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000A26 end

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
namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IStatisticManager: IDisposable, INotifyPropertyChanged
    {
        // Operations

        void setServiceCredential(object channel);

        Task<List<Statistic>> InsertStatisticAsync(List<Statistic> statisticList);

        Task<List<Statistic>> UpdateStatisticAsync(List<Statistic> statisticList);

        Task<List<Statistic>> DeleteStatisticAsync(List<Statistic> statisticList);

        Task<List<Statistic>> GetStatisticDataAsync(int nbLine);

        Task<List<Statistic>> searchStatisticAsync(Statistic statistic, ESearchOption filterOperator);
    } /* end interface IStatisticManager */
}