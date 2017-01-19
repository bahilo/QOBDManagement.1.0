using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IRoleManager
    {
        Task<List<Role>> InsertRoleAsync(List<Role> listRole);

        Task<List<Role>> UpdateRoleAsync(List<Role> listRole);

        Task<List<Role>> DeleteRoleAsync(List<Role> listRole);

        Task<List<Role>> GetRoleDataAsync(int nbLine);

        Task<List<Role>> searchRoleAsync(Role Role, ESearchOption filterOperator);

        Task<List<Role>> GetRoleDataByIdAsync(int id);
    }
}
