using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IAuto_refManagement: REMOTE.IAuto_refManagement
    {
        List<Auto_ref> GetAuto_refData(int nbLine);

        List<Auto_ref> searchAuto_ref(Auto_ref Auto_ref, ESearchOption filterOperator);

        List<Auto_ref> GetAuto_refDataById(int id);
    }
}
