using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IAgent_roleManager
    {
        Task<List<Agent_role>> InsertAgent_roleAsync(List<Agent_role> listAgent_role);

        Task<List<Agent_role>> UpdateAgent_roleAsync(List<Agent_role> listAgent_role);

        Task<List<Agent_role>> DeleteAgent_roleAsync(List<Agent_role> listAgent_role);

        Task<List<Agent_role>> GetAgent_roleDataAsync(int nbLine);

        Task<List<Agent_role>> searchAgent_roleAsync(Agent_role Agent_role, ESearchOption filterOperator);

        Task<List<Agent_role>> GetAgent_roleDataByIdAsync(int id);
    }
}
