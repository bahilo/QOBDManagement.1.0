
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IInfosManager: REMOTE.IInfosManager
    {
        List<Info> GetInfosData(int nbLine);

        List<Info> searchInfos(Info Infos, ESearchOption filterOperator);
        
        List<Info> GetInfosDataById(int id);
    }
}
