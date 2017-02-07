using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface ISecurityActionManager: IActionRecordManager, IRoleManager, IAgent_roleManager, IRole_actionManager, IPrivilegeManager, IDisposable
    {
        void setServiceCredential(string login, string password);

        Task<Agent> AuthenticateUserAsync(string username, string password, bool isClearPassword = true);

        Task<List<Entities.Action>> InsertActionAsync(List<Entities.Action> listAction);

        Task<List<Entities.Action>> UpdateActionAsync(List<Entities.Action> listAction);

        Task<List<Entities.Action>> DeleteActionAsync(List<Entities.Action> listAction);

        Task<List<Entities.Action>> GetActionDataAsync(int nbLine);

        Task<List<Entities.Action>> searchActionAsync(Entities.Action Action, ESearchOption filterOperator);

        Task<List<Entities.Action>> GetActionDataByIdAsync(int id);
    }
}
