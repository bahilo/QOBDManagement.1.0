using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
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
    public interface IClientManager : DAC.IClientManager
    {

        Task<List<Client>> MoveClientAgentBySelection(List<Client> clientList, Agent toAgent);

    } /* end interface IClientManager */
}