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
namespace QOBDCommon.Interfaces.BL
{
    public interface IAgentManager : DAC.IAgentManager
    {        

        Task<List<Client>> MoveAgentClient(Agent fromAgent, Agent toAgent);

    } /* end interface IAgentManager */
}