using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IAgentManager : REMOTE.IAgentManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);

        void cacheWebServiceData();

        void progressBarManagement(Func<double, double> progressBarFunc);

        List<Agent> GetAgentData(int nbLine);

        List<Agent> GetAgentDataById(int id);

        List<Agent> GetAgentDataByOrderList(List<Order> orderList);

        List<Agent> searchAgent(Agent agent, ESearchOption filterOperator);

    } /* end interface IAgentManager */
}