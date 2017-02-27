using QOBDBusiness;
using QOBDBusiness.Core;
using QOBDCommon.Entities;
using QOBDDAL.Core;
using QOBDGateway.Classes;
using QOBDGateway.QOBDServiceReference;
using QOBDManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Classes
{
    public class Startup: IStartup
    {
        private BusinessLogic _bl { get; set; }
        private DataAccess _dal { get; set; }
        private ClientProxy _proxyClient;
        private QOBDDAL.Interfaces.IQOBDSet _dataSet;

        public Startup() {
            initialize();
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

        public void initialize()
        {
            _dataSet = new QOBDDAL.Classes.QOBDDataSet();
            _proxyClient = new ClientProxy("QOBDWebServicePort");
            _dal = new DataAccess(
                                new DALAgent(_proxyClient, _dataSet),
                                new DALClient(_proxyClient, _dataSet),
                                new DALItem(_proxyClient, _dataSet),
                                new DALOrder(_proxyClient, _dataSet),
                                new DALSecurity(_proxyClient, _dataSet),
                                new DALStatisitc(_proxyClient, _dataSet),
                                new DALReferential(_proxyClient, _dataSet),
                                new DALNotification(_proxyClient, _dataSet));
            
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
