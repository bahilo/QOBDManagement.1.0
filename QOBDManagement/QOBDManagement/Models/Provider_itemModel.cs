using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class Provider_itemModel : BindBase
    {
        private Provider_item _provider_item;
        private Provider _provider;
        private CurrencyModel _currencyModel;

        public Provider_itemModel()
        {
            _provider = new Provider();
            _provider_item = new Provider_item();
            _currencyModel = new CurrencyModel();
        }

        public string TxtID
        {
            get { return _provider_item.ID.ToString(); }
            set { _provider_item.ID = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtItemId
        {
            get { return _provider_item.ItemId.ToString(); }
            set { _provider_item.ItemId = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtProviderId
        {
            get { return _provider_item.ProviderId.ToString(); }
            set { _provider_item.ProviderId = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtCurrencyId
        {
            get { return _provider_item.CurrencyId.ToString(); }
            set { _provider_item.CurrencyId = Utility.intTryParse(value); onPropertyChange(); }
        }

        public Provider_item Provider_item
        {
            get { return _provider_item; }
            set { _provider_item = value; onPropertyChange(); }
        }

        public Provider Provider
        {
            get { return _provider; }
            set
            {
                _provider = value;
                Provider_item.ProviderId = value.ID;
                onPropertyChange();
            }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _currencyModel; }
            set
            {
                if (value == null) return;

                _currencyModel = value;
                Provider_item.CurrencyId = value.Currency.ID;
                onPropertyChange();

            }
        }

        public void refresh()
        {
            onPropertyChange("CurrencyModel");
        }

    }
}
