using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IActionRecordManager: REMOTE.IActionRecordManager
    {
        List<ActionRecord> GetActionRecordData(int nbLine);

        List<ActionRecord> searchActionRecord(ActionRecord ActionRecord, ESearchOption filterOperator);

        List<ActionRecord> GetActionRecordDataById(int id);
    }
}
