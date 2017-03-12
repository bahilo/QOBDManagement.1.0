using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Enum
{
    public enum EStatus
    {
        //-----[ Agent Status ]
        Active,
        Deactivated,
        Order,

        //-----[ Order Status ]
        Quote,
        Close,
        Bill,

        //-----[ Delivery Status ]
        LinkedToBill,
        NoLinkedToBill,

        //-----[ Client Status ]
        Client,
        Prospect
    }
}
