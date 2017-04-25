using Moq;
using QOBDBusiness;
using QOBDBusiness.Core;
using QOBDDAL.Core;
using QOBDGateway.Classes;
using QOBDManagement.Interfaces;
using QOBDTest.DAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDTest.Classes
{
    public class MockStartup : IStartup
    {
        private BusinessLogic _bl { get; set; }
        private DataAccess _dal { get; set; }
        private ClientProxy _proxy;
        private QOBDDAL.Interfaces.IQOBDSet _dataSet;

        public MockStartup()
        {
            initialize();
        }
        public MockStartup(Mock<ClientProxy> mock)
        {
            initialize(mock);
        }

        public QOBDDAL.Interfaces.IQOBDSet DataSet
        {
            get { return _dataSet; }
        }

        public ClientProxy ProxyClient
        {
            get { return _proxy; }
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

        public void initialize() { }
        public void initialize(Mock<ClientProxy> mock)
        {

            _dataSet = new MockDataSet();
            _proxy = mock.Object;// new MockClient("QOBDWebServicePort") ;
            _dal = new DataAccess(
                                new DALAgent(_proxy, _dataSet),
                                new DALClient(_proxy, _dataSet),
                                new DALItem(_proxy, _dataSet),
                                new DALOrder(_proxy, _dataSet),
                                new DALSecurity(_proxy, _dataSet),
                                new DALStatisitc(_proxy, _dataSet),
                                new DALReferential(_proxy, _dataSet),
                                new DALNotification(_proxy, _dataSet),
                                new DALChatRoom(_proxy, _dataSet));

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

        public void resetCommunication()
        {
            
        }
    }
}
