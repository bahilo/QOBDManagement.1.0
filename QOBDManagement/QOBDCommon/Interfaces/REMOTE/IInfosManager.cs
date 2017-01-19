using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IInfosManager
    {
        Task<List<Info>> InsertInfoAsync(List<Info> listInfos);

        Task<List<Info>> UpdateInfoAsync(List<Info> listInfos);

        Task<List<Info>> DeleteInfoAsync(List<Info> listInfos);

        Task<List<Info>> GetInfoDataAsync(int nbLine);

        Task<List<Info>> searchInfosAsync(Info Infos, ESearchOption filterOperator);
    }
}
