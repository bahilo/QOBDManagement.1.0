using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Enums;
using QOBDManagement.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class DeliveryModel : BindBase
    {
        private Delivery _delivery;
        private bool _isConstructorRefVisible;

        public DeliveryModel()
        {
            _delivery = new Delivery();
            _delivery.Package = 1;
        }

        public bool IsConstructorRefVisible
        {
            get { return _isConstructorRefVisible; }
            set { setProperty(ref _isConstructorRefVisible, value, "IsConstructorRefVisible"); }
        }
        public Delivery Delivery
        {
            get { return _delivery; }
            set { setProperty(ref _delivery, value, "Delivery"); }
        }

        public string TxtID
        {
            get { return _delivery.ID.addPrefix(EPrefix.DELIVERY); }
            set { _delivery.ID = Convert.ToInt32(value.deletePrefix()); onPropertyChange("TxtID"); }
        }

        public string TxtOrderId
        {
            get { return _delivery.OrderId.ToString(); }
            set { _delivery.OrderId = Convert.ToInt32(value); onPropertyChange("TxtCommandId"); }
        }

        public string TxtBillId
        {
            get { return _delivery.BillId.addPrefix(EPrefix.INVOICE); }
            set { _delivery.BillId = Convert.ToInt32(value.deletePrefix()); onPropertyChange("TxtBillId"); }
        }

        public string TxtPackage
        {
            get { return _delivery.Package.ToString(); }
            set { _delivery.Package = Convert.ToInt32(value); onPropertyChange("TxtPackage"); }
        }

        public string TxtDate
        {
            get { return _delivery.Date.ToString(); }
            set { _delivery.Date = Convert.ToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public string TxtStatus
        {
            get { return _delivery.Status; }
            set { _delivery.Status = value; onPropertyChange("TxtStatus"); }
        }

        public List<DeliveryModel> DeliveryListToModelViewList(List<Delivery> list)
        {
            List<DeliveryModel> output = new List<DeliveryModel>();
            foreach (Delivery del in list)
            {
                DeliveryModel delivery = new DeliveryModel();
                delivery.Delivery = del;

                output.Add(delivery);
            }
            return output;
        }






    }
}
