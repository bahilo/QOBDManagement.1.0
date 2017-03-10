using QOBDCommon.Entities;
using QOBDManagement.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IAgentViewModel
    {
        List<AgentModel> agentListToModelViewList(List<Agent> AgentList);
        Task loadAgents();
        void Dispose();
    }
}
