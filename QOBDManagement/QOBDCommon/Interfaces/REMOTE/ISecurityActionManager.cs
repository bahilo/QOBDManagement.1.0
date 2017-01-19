using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface ISecurityActionManager
    {
        Task<List<Action>> InsertActionAsync(List<Action> listAction);

        Task<List<Action>> UpdateActionAsync(List<Action> listAction);

        Task<List<Action>> DeleteActionAsync(List<Action> listAction);

        Task<List<Action>> GetActionDataAsync(int nbLine);

        Task<List<Action>> searchActionAsync(Action Action, ESearchOption filterOperator);

        Task<List<Action>> GetActionDataByIdAsync(int id);
    }
}
