using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface ITaxManager: REMOTE.ITaxManager
    {
        List<Tax> GetTaxData(int id);

        List<Tax> searchTax(Tax Tax, ESearchOption filterOperator);

        List<Tax> GetTaxDataById(int id);
        
    }
}
