using QOBDBusiness;
using QOBDBusiness.Core;
using QOBDCommon.Entities;
using QOBDDAL.Core;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Classes
{
    public class Startup
    {
        private BusinessLogic _bl { get; set; }
        private DataAccess _dal { get; set; }
        private QOBDWebServicePortTypeClient _proxyClient;

        public Startup() {
            initialize();
        }

        public QOBDWebServicePortTypeClient ProxyClient
        {
            get { return _proxyClient; }
        }

        public DataAccess Dal
        {
            get { return _dal; }
        }

        public BusinessLogic Bl
        {
            get { return _bl; }
            set { Bl = value; }
        }

        public void initialize()
        {
            _proxyClient = new QOBDWebServicePortTypeClient("QOBDWebServicePort"); ;
            _dal = new DataAccess(
                                new DALAgent(_proxyClient),
                                new DALClient(_proxyClient),
                                new DALItem(_proxyClient),
                                new DALOrder(_proxyClient),
                                new DALSecurity(_proxyClient),
                                new DALStatisitc(_proxyClient),
                                new DALReferential(_proxyClient),
                                new DALNotification(_proxyClient));
            
            _bl = new BusinessLogic(
                                    new BLAgent(_dal),
                                    new BlCLient(_dal),
                                    new BLItem(_dal),
                                    new BLOrder(_dal),
                                    new BlSecurity(_dal),
                                    new BLStatisitc(_dal),
                                    new BlReferential(_dal),
                                    new BlNotification(_dal));
        }



    }

}
