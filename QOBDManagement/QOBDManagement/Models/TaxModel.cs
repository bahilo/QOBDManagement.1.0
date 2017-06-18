using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class TaxModel : BindBase
    {
        private Tax _tax;

        public TaxModel()
        {
            _tax = new Tax();
        }

        public Tax Tax
        {
            get { return _tax; }
            set { setProperty(ref _tax, value); }
        }

        public string TxtTaxValue
        {
            get { return _tax.Value.ToString(); }
            set { _tax.Value = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtTaxType
        {
            get { return _tax.Type.ToString(); }
            set { _tax.Type = value; onPropertyChange(); }
        }

        public bool IsDefault
        {
            get { return _tax.Tax_current == 1 ? true : false; }
            set { _tax.Tax_current = value ? 1 : 0; onPropertyChange(); }
        }

        public string TxtDate
        {
            get { return _tax.Date_insert.ToString("dd/MM/yyyy"); }
            set { _tax.Date_insert = (Utility.convertToDateTime(value).Equals(Utility.DateTimeMinValueInSQL2005))? DateTime.Now: Utility.convertToDateTime(value) ; onPropertyChange(); }
        }

        public string TxtComment
        {
            get { return _tax.Comment; }
            set { _tax.Comment = value; onPropertyChange(); }
        }

    }
}
