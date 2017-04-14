using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class Tax_orderModel: BindBase
    {
        private Tax _tax;
        private Order _order;
        private Tax_order _tax_order;

        public Tax_orderModel()
        {
            _tax = new Tax();
            _order = new Order();
            _tax_order = new Tax_order();
        }


        public Tax Tax
        {
            get { return _tax; }
            set
            {
                if (value != null)
                {
                    _tax = value;
                    TxtTaxId = value.ID.ToString();
                    TxtTax_value = value.Value.ToString();
                    TxtTarget = EOrderStatus.Order.ToString();
                    TxtDate_insert = DateTime.Now.ToString();
                    _order.Tax = value.Value;
                    onPropertyChange();
                }                
            }
        }

        public Order Order
        {
            get { return _order; }
            set { _order = value; TxtOrderId = value.ID.ToString(); onPropertyChange(); }
        }

        public Tax_order Tax_order
        {
            get { return _tax_order; }
            set { _tax_order = value; onPropertyChange(); }
        }

        public string TxtID
        {
            get { return _tax_order.ID.ToString(); }
            set { _tax_order.ID = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtOrderId
        {
            get { return _tax_order.OrderId.addPrefix(Enums.EPrefix.ORDER); }
            set { _tax_order.OrderId = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtTaxId
        {
            get { return _tax_order.TaxId.ToString(); }
            set { _tax_order.TaxId = Utility.intTryParse(value); ; onPropertyChange(); }
        }

        public string TxtDate_insert
        {
            get { return _tax_order.Date_insert.ToString(); }
            set { _tax_order.Date_insert = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public string TxtTax_value
        {
            get { return _tax_order.Tax_value.ToString(); }
            set { _tax_order.Tax_value = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtTarget
        {
            get { return _tax_order.Target; }
            set { _tax_order.Target = value; onPropertyChange(); }
        }

    }
}
