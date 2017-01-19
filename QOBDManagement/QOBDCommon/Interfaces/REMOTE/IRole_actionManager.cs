using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IRole_actionManager
    {
        Task<List<Role_action>> InsertRole_actionAsync(List<Role_action> listRole_action);

        Task<List<Role_action>> UpdateRole_actionAsync(List<Role_action> listRole_action);

        Task<List<Role_action>> DeleteRole_actionAsync(List<Role_action> listRole_action);

        Task<List<Role_action>> GetRole_actionDataAsync(int nbLine);

        Task<List<Role_action>> searchRole_actionAsync(Role_action Role_action, ESearchOption filterOperator);

        Task<List<Role_action>> GetRole_actionDataByIdAsync(int id);
    }
}
