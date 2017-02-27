using QOBDCommon.Entities;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface INotificationViewModel
    {
        Task loadNotifications();
        Task<List<BillModel>> billListToModelViewList(List<Bill> billList);
        void Dispose();
    }
}
