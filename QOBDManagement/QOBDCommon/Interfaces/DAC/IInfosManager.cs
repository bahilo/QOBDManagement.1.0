
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System.Collections.Generic;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IInfosManager: REMOTE.IInfosManager
    {
        List<Info> GetInfoData(int nbLine);

        List<Info> searchInfo(Info Infos, ESearchOption filterOperator);
        
        List<Info> GetInfosDataById(int id);
    }
}
