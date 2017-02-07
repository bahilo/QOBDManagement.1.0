using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class AddressModel: BindBase
    {
        private Address _address;

        public AddressModel()
        {
            _address = new Address();
        }

        public Address Address
        {
            get { return _address; }
            set { setProperty(ref _address, value); }
        }

        public string TxtID
        {
            get { return _address.ID.ToString(); }
            set { _address.ID = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtClientId
        {
            get { return _address.ClientId.ToString(); }
            set { _address.ClientId = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtFirstName
        {
            get { return _address.FirstName; }
            set { _address.FirstName = value; onPropertyChange(); }
        }

        public string TxtLastName
        {
            get { return _address.LastName; }
            set { _address.LastName = value; onPropertyChange(); }
        }

        public string TxtType // delivery or Billing address
        {
            get { return _address.Name; }
            set { _address.Name = value; onPropertyChange(); }
        }

        public string TxtName
        {
            get { return _address.Name2; }
            set { _address.Name2 = value; onPropertyChange(); }
        }

        public string TxtPhone
        {
            get { return _address.Phone; }
            set { _address.Phone = value; onPropertyChange(); }
        }

        public string TxtEmail
        {
            get { return _address.Email; }
            set { _address.Email = value; onPropertyChange(); }
        }

        public string TxtPostcode
        {
            get { return _address.Postcode; }
            set { _address.Postcode = value; onPropertyChange(); }
        }

        public string TxtAddressName
        {
            get { return _address.AddressName; }
            set { _address.AddressName = value; onPropertyChange(); }
        }

        public string TxtCityName
        {
            get { return _address.CityName; }
            set { _address.CityName = value; onPropertyChange(); }
        }

        public string TxtCountry
        {
            get { return _address.Country; }
            set { _address.Country = value; onPropertyChange(); }
        }

        public string TxtComment
        {
            get { return _address.Comment; }
            set { _address.Comment = value; onPropertyChange(); }
        }

    }
}
