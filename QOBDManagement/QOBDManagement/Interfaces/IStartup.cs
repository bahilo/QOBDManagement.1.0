using QOBDBusiness;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IStartup
    {
        QOBDDAL.Interfaces.IQOBDSet DataSet { get; }
        ClientProxy ProxyClient { get; }
        DataAccess Dal { get; }
        BusinessLogic Bl { get; }

        void initialize();
        
    }
}
