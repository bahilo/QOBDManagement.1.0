using Entity = QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using QOBDCommon.Classes;
using QOBDCommon.Enum;
using System.ComponentModel;

namespace QOBDManagement.Classes
{
    public class OrderSearch 
    {
        private int _orderId;
        private int _billId;
        private int _clientId;
        private List<string> _statusList;
        private string _selectedStatus;
        private List<Entity.Agent> _agents;
        private Entity.Agent _selectedAgent;
        private string _companyName;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isDeepSearch;
        

        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public int BillId
        {
            get { return _billId; }
            set { _billId = value;}
        }

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; }
        }

        public List<string> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; }
        }

        public Entity.Agent SelectedAgent
        {
            get { return _selectedAgent; }
            set { _selectedAgent = value; }
        }

        public List<Entity.Agent> AgentList
        {
            get { return _agents; }
            set { _agents = value; }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set {  _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public bool IsDeepSearch
        {
            get { return _isDeepSearch; }
            set { _isDeepSearch = value;  }
        }
        

    }
}
