using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IAuto_refManagement
    {
        Task<List<Auto_ref>> InsertAuto_refAsync(List<Auto_ref> listAuto_ref);

        Task<List<Auto_ref>> UpdateAuto_refAsync(List<Auto_ref> listAuto_ref);

        Task<List<Auto_ref>> DeleteAuto_refAsync(List<Auto_ref> listAuto_ref);

        Task<List<Auto_ref>> GetAuto_refDataAsync(int nbLine);

        Task<List<Auto_ref>> searchAuto_refAsync(Auto_ref Auto_ref, ESearchOption filterOperator);
    }
}
