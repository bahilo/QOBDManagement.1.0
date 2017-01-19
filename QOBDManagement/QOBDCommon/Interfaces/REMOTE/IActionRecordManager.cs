using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IActionRecordManager
    {
        Task<List<ActionRecord>> InsertActionRecordAsync(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> UpdateActionRecordAsync(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> DeleteActionRecordAsync(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> GetActionRecordDataAsync(int nbLine);

        Task<List<ActionRecord>> searchActionRecordAsync(ActionRecord ActionRecord, ESearchOption filterOperator);

        Task<List<ActionRecord>> GetActionRecordDataByIdAsync(int id);
    }
}
