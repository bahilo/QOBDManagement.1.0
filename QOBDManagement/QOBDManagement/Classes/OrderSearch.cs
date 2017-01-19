using Entity = QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using QOBDCommon.Classes;
using QOBDCommon.Enum;
using System.ComponentModel;

namespace QOBDManagement.Classes
{
    public class OrderSearch : System.ComponentModel.INotifyPropertyChanged
    {
        private int _commandId;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderSearch()
        {
            _statusList = new List<string>
            {
                EStatusOrder.Quote.ToString(),                  //devis
                EStatusOrder.Pre_Order.ToString(),             //preco
                EStatusOrder.Order.ToString(),                //command
                EStatusOrder.Order_Close.ToString(),           // close
                EStatusOrder.Pre_Credit.ToString(),              // preavoir
                EStatusOrder.Credit.ToString(),                 // avoir
                EStatusOrder.Credit_CLose.ToString(),            // a_close
                EStatusOrder.Pre_Client_Validation.ToString(),    // revalid
                EStatusOrder.Bill_Order.ToString(),            // facture
                EStatusOrder.Bill_Credit.ToString(),              // a_facture
                EStatusOrder.Billed.ToString(),                    //f
                EStatusOrder.Not_Billed.ToString()               //nf}
            };
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
        }

        public int CommandId
        {
            get { return _commandId; }
            set { _commandId = value; onPropertyChange("CommandId"); }
        }

        public int BillId
        {
            get { return _billId; }
            set { _billId = value; onPropertyChange("BillId"); }
        }

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; onPropertyChange("SelectedStatus"); }
        }

        public List<string> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; onPropertyChange("StatusList"); }
        }

        public Entity.Agent SelectedAgent
        {
            get { return _selectedAgent; }
            set { _selectedAgent = value; onPropertyChange("SelectedAgent"); }
        }

        public List<Entity.Agent> AgentList
        {
            get { return _agents; }
            set { _agents = value; onPropertyChange("AgentList"); }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; onPropertyChange("CompanyName"); }
        }

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; onPropertyChange("ClientId"); }
        }

        public string StartDate
        {
            get { return _startDate.ToString(); }
            set {  _startDate = Utility.convertToDateTime(value, true); onPropertyChange("StartDate"); }
        }

        public string EndDate
        {
            get { return _endDate.ToString(); }
            set { _endDate = Utility.convertToDateTime(value, true); onPropertyChange("EndDate"); }
        }

        public bool IsDeepSearch
        {
            get { return _isDeepSearch; }
            set { _isDeepSearch = value; onPropertyChange("IsDeepSearch"); }
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
