using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDManagement.Helper;
using QOBDManagement.Enums;
using QOBDCommon.Classes;

namespace QOBDManagement.Models
{
    public class ClientModel : BindBase
    {
        private Client _client;
        private Address _address;
        private List<Address> _Addresses;
        private Contact _contact;
        private List<Contact> _contacts;
        private AgentModel _agent;        
        private bool _isSearchCompanyChecked;
        private bool _isSearchContactChecked;
        private bool _isSearchClientChecked;
        private bool _isSearchProspectChecked;
        private bool _isSelectForQuote;
        private decimal _usedCredit;

        public ClientModel()
        {
            _client = new Client();
            _agent = new AgentModel();
            _Addresses = new List<Address>();
            _contacts = new List<Contact>();
            _contact = new Contact();
            _address = new Address();

            PropertyChanged += onClientChange_updateAllClientObjects;
        }

        public Contact Contact
        {
            get { return (_contact != null)? _contact : new Contact(); }
            set { setProperty(ref _contact, value); }
        }

        public Address Address
        {
            get { return (_address != null)?_address:new Address(); }
            set { setProperty(ref _address, value); }
        }

        public List<Contact> ContactList
        {
            get { return _contacts; }
            set { setProperty(ref _contacts, value); }
        }

        public AgentModel Agent
        {
            get { return _agent; }
            set { setProperty(ref _agent, value); }
        }

        public string TxtID
        {
            get { return _client.ID.addPrefix(EPrefix.CLIENT); }
            set { _client.ID = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtAgentId
        {
            get {  return _client.AgentId.ToString(); }
            set { _client.AgentId = Utility.intTryParse(value); onPropertyChange();  }
        }

        public string TxtAgentName
        {
            get { return _agent.TxtLastName; }
            set { _agent.TxtLastName = value; onPropertyChange(); }
        }

        public string TxtAgentFirstName
        {
            get { return _agent.TxtFirstName; }
            set { _agent.TxtFirstName = value; onPropertyChange(); }
        }

        public string TxtFirstName
        {
            get { return _client.FirstName; }
            set { _client.FirstName = value; onPropertyChange(); }
        }

        public string TxtLastName
        {
            get { return _client.LastName; }
            set { _client.LastName = value; onPropertyChange(); }
        }

        public string TxtCompany
        {
            get { return (!string.IsNullOrEmpty(_client.Company)) ? _client.Company : _client.CompanyName; }
            set { _client.Company = value; onPropertyChange(); }
        }

        public string TxtEmail
        {
            get { return _client.Email; }
            set { _client.Email = value; onPropertyChange(); }
        }

        public string TxtPhone
        {
            get { return _client.Phone; }
            set { _client.Phone = value; onPropertyChange(); }
        }

        public string TxtFax
        {
            get { return _client.Fax; }
            set { _client.Fax = value; onPropertyChange(); }
        }

        public string TxtRib
        {
            get { return _client.Rib; }
            set { _client.Rib = value; onPropertyChange(); }
        }

        public string TxtCRN
        {
            get { return _client.CRN; }
            set { _client.CRN = value; onPropertyChange(); }
        }

        public string TxtPayDelay
        {
            get { return _client.PayDelay.ToString(); }
            set { _client.PayDelay = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtComment
        {
            get { return _client.Comment; }
            set { _client.Comment = value; onPropertyChange(); }
        }

        public string TxtMaxCredit
        {
            get { return _client.MaxCredit.ToString(); }
            set { _client.MaxCredit = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtUsedCredit
        {
            get { return _usedCredit.ToString(); }
            set { setProperty(ref _usedCredit, Utility.decimalTryParse(value)); }
        }

        public bool IsProspect
        {
            get
            {
                if (_client.Status == EStatus.Prospect.ToString())
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _client.Status = EStatus.Prospect.ToString();
                    onPropertyChange();
                }
            }
        }

        public bool IsClient
        {
            get
            {
                if (_client.Status == EStatus.Client.ToString())
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _client.Status = EStatus.Client.ToString();
                    onPropertyChange();
                }
            }
        }

        public string TxtStatus
        {
            get { return _client.Status; }
            set { _client.Status = value; onPropertyChange(); }
        }

        public string TxtCompanyName
        {
            get { return _client.CompanyName; }
            set { _client.CompanyName = value; onPropertyChange(); }
        }

        public bool IsSearchCompanyChecked
        {
            get { return _isSearchCompanyChecked; }
            set { _isSearchCompanyChecked = value; onPropertyChange(); }
        }

        public bool IsSearchContactCheked
        {
            get { return _isSearchContactChecked; }
            set { _isSearchContactChecked = value; onPropertyChange(); }
        }

        public bool IsSearchClientChecked
        {
            get { return _isSearchClientChecked; }
            set { _isSearchClientChecked = value; onPropertyChange(); }
        }

        public bool IsSearchProspectChecked
        {
            get { return _isSearchProspectChecked; }
            set { _isSearchProspectChecked = value; onPropertyChange(); }
        } 

        public bool IsSelectForQuote
        {
            get { return _isSelectForQuote; }
            set { _isSelectForQuote = value; onPropertyChange(); }
        }                    

        public Client Client
        {
            get { return _client; }
            set { _client = value; onPropertyChange(); }
        }     
        
        public List<Address> AddressList
        {
            get { return _Addresses; }
            set { setProperty(ref _Addresses, value); }
        }

        private void onClientChange_updateAllClientObjects(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Client"))
            {
                onPropertyChange("TxtID");
                onPropertyChange("TxtAgentId");
                onPropertyChange("TxtFirstName");
                onPropertyChange("TxtLastName");
                onPropertyChange("TxtCompany");
                onPropertyChange("TxtEmail");
                onPropertyChange("TxtPhone");
                onPropertyChange("TxtFax");
                onPropertyChange("TxtRib");
                onPropertyChange("TxtCRN");
                onPropertyChange("TxtPayDelay");
                onPropertyChange("TxtComment");
                onPropertyChange("TxtMaxCredit");
                onPropertyChange("IsProspect");
                onPropertyChange("IsClient");
                onPropertyChange("TxtStatus");
                onPropertyChange("TxtCompanyName");
            }
        }

    }
}
