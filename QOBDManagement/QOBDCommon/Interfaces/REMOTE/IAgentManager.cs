using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IAgentManager : IDisposable
    {
        // Operations

        Task<List<Agent>> InsertAgentAsync(List<Agent> listAgent);

        Task<List<Agent>> UpdateAgentAsync(List<Agent> listAgent);

        Task<List<Agent>> DeleteAgentAsync(List<Agent> listAgent);

        Task<List<Agent>> GetAgentDataAsync(int nbLine);

        Task<List<Agent>> GetAgentDataByOrderListAsync(List<Order> orderList);

        Task<List<Agent>> searchAgentAsync(Agent agent, ESearchOption filterOperator);

    } /* end interface IAgentManager */
}