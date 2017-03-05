using QOBDBusiness;
using QOBDBusiness.Core;
using QOBDCommon.Entities;
using QOBDDAL.Core;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;
using QOBDGateway.QOBDServiceReference;
using QOBDManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Classes
{
    public class Startup: IStartup, ICommunication
    {
        private BusinessLogic _bl { get; set; }
        private DataAccess _dal { get; set; }
        private ClientProxy _proxyClient;
        private QOBDDAL.Interfaces.IQOBDSet _dataSet;

        public Startup() {
            initialize();
        }

        public void initialize()
        {
            _dataSet = new QOBDDAL.Classes.QOBDDataSet();
            _proxyClient = new ClientProxy("QOBDWebServicePort");
            _dal = new DataAccess(
                                new DALAgent(_proxyClient, _dataSet, this),
                                new DALClient(_proxyClient, _dataSet, this),
                                new DALItem(_proxyClient, _dataSet, this),
                                new DALOrder(_proxyClient, _dataSet, this),
                                new DALSecurity(_proxyClient, _dataSet, this),
                                new DALStatisitc(_proxyClient, _dataSet, this),
                                new DALReferential(_proxyClient, _dataSet, this),
                                new DALNotification(_proxyClient, _dataSet, this),
                                new DALChatRoom(_proxyClient, _dataSet, this));

            _bl = new BusinessLogic(
                                    new BLAgent(_dal),
                                    new BlCLient(_dal),
                                    new BLItem(_dal),
                                    new BLOrder(_dal),
                                    new BlSecurity(_dal),
                                    new BLStatisitc(_dal),
                                    new BlReferential(_dal),
                                    new BlNotification(_dal),
                                    new BLChatRoom(_dal));
        }

        public QOBDDAL.Interfaces.IQOBDSet DataSet
        {
            get { return _dataSet; }
        }

        public ClientProxy ProxyClient
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
            set { _bl = value; }
        }

        public void resetCommunication()
        {
            var newProxyClient = new ClientProxy("QOBDWebServicePort");
            Bl.BlAgent.setServiceCredential(newProxyClient);
            Bl.BlChatRoom.setServiceCredential(newProxyClient);
            Bl.BlClient.setServiceCredential(newProxyClient);
            Bl.BlItem.setServiceCredential(newProxyClient);
            Bl.BlNotification.setServiceCredential(newProxyClient);
            Bl.BlOrder.setServiceCredential(newProxyClient);
            Bl.BlReferential.setServiceCredential(newProxyClient);
            Bl.BlSecurity.setServiceCredential(newProxyClient);
            Bl.BlStatisitc.setServiceCredential(newProxyClient);
        }
    }

}
