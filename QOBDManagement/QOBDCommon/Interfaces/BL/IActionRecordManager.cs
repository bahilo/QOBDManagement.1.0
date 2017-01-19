using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.BL
{
    public interface IActionRecordManager: DAC.IActionRecordManager
    {
        Task<List<ActionRecord>> InsertActionRecord(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> UpdateActionRecord(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> DeleteActionRecord(List<ActionRecord> listActionRecord);

        
    }
}
