using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class ProviderModel : BindBase
    {
        private Provider _provider;
        private ObservableCollection<Address> _addressList;
        private Address _selectedAddress;

        public ProviderModel()
        {
            _provider = new Provider();
            _addressList = new ObservableCollection<Address>();
            _selectedAddress = new Address();
        }

        public Provider Provider
        {
            get { return _provider; }
            set { setProperty(ref _provider, value); }
        }

        public ObservableCollection<Address> AddressList
        {
            get { return _addressList; }
            set { setProperty(ref _addressList, value); }
        }

        public Address SelectedAddress
        {
            get { return _selectedAddress; }
            set { setProperty(ref _selectedAddress, value); }
        }

        public string TxtID
        {
            get { return _provider.ID.ToString(); }
            set { _provider.ID = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtAddressId
        {
            get { return _provider.AddressId.ToString(); }
            set { _provider.AddressId = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtSource
        {
            get { return _provider.Source.ToString(); }
            set { _provider.Source = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtPhone
        {
            get { return _provider.Phone; }
            set { _provider.Phone = value; onPropertyChange(); }
        }

        public string TxtFax
        {
            get { return _provider.Fax; }
            set { _provider.Fax = value; onPropertyChange(); }
        }

        public string TxtEmail
        {
            get { return _provider.Email; }
            set { _provider.Email = value; onPropertyChange(); }
        }

        public string TxtRIB
        {
            get { return _provider.RIB; }
            set { _provider.RIB = value; onPropertyChange(); }
        }

        public string TxtComment
        {
            get { return _provider.Comment; }
            set { _provider.Comment = value; onPropertyChange(); }
        }

        public string TxtCompanyName
        {
            get { return _provider.Name; }
            set { _provider.Name = value; onPropertyChange(); }
        }

    }
}
