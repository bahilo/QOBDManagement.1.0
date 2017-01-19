using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IPrivilegeManager
    {
        Task<List<Privilege>> InsertPrivilegeAsync(List<Privilege> listPrivilege);

        Task<List<Privilege>> UpdatePrivilegeAsync(List<Privilege> listPrivilege);

        Task<List<Privilege>> DeletePrivilegeAsync(List<Privilege> listPrivilege);

        Task<List<Privilege>> GetPrivilegeDataAsync(int nbLine);

        Task<List<Privilege>> searchPrivilegeAsync(Privilege Privilege, ESearchOption filterOperator);

        Task<List<Privilege>> GetPrivilegeDataByIdAsync(int id);
    }
}
