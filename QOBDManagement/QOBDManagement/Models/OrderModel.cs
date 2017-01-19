using QOBDManagement.Classes;
using System;
using Entity = QOBDCommon.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDManagement.ViewModel;
using System.ComponentModel;
using QOBDCommon.Enum;
using System.Collections.ObjectModel;
using QOBDManagement.Helper;

namespace QOBDManagement.Models
{
    public class OrderModel : BindBase
    {
        private Entity.Order _order;
        private AgentModel _agentModel;
        private ClientModel _clientModel;
        private Entity.Address _deliveryAddress;
        private Entity.Address _billAddress;
        private List<Order_itemModel> _command_itemList;
        private List<Entity.Address> _addressList;
        private Entity.Tax_order _tax_command;
        private Entity.Tax _tax;
        private List<BillModel> _billModelList;
        private List<DeliveryModel> _deliveryModelList;


        public OrderModel()
        {
            _tax = new Entity.Tax();
            _tax_command = new Entity.Tax_order();
            _addressList = new List<Entity.Address>();
            _billModelList = new List<BillModel>();
            _deliveryModelList = new List<DeliveryModel>();
            _agentModel = new AgentModel();
            _clientModel = new ClientModel();
            _order = new Entity.Order();
            _command_itemList = new List<Order_itemModel>();

            PropertyChanged += onAddressListChange;
            PropertyChanged += onAgentModelChange;
            PropertyChanged += onClientModelChange;
        }


        private void onClientModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CLientModel"))
                TxtClientId = (CLientModel != null) ? CLientModel.TxtID : "0";
        }

        private void onAgentModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AgentModel"))
                TxtAgentId = AgentModel.TxtID;
        }

        private void onAddressListChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AddressList"))
            {
                var deliveryAddressFoundList = AddressList.Where(x => x.ID == Order.DeliveryAddress).ToList();
                DeliveryAddress = (deliveryAddressFoundList.Count() > 0) ? deliveryAddressFoundList[0] : new Entity.Address();
                var billAddressFoundList = AddressList.Where(x => x.ID == Order.BillAddress).ToList();
                BillAddress = (billAddressFoundList.Count() > 0) ? billAddressFoundList[0] : new Entity.Address();

            }
        }

        public Entity.Address BillAddress
        {
            get { return _billAddress; }
            set { setProperty(ref _billAddress, value); }
        }

        public Entity.Address DeliveryAddress
        {
            get { return _deliveryAddress; }
            set { setProperty(ref _deliveryAddress, value); }
        }
        
        public List<DeliveryModel> DeliveryModelList
        {
            get { return _deliveryModelList; }
            set { setProperty(ref _deliveryModelList, value); }
        }

        public List<BillModel> BillModelList
        {
            get { return _billModelList; }
            set { setProperty(ref _billModelList, value); }
        }

        public List<Order_itemModel> Order_ItemList
        {
            get { return _command_itemList; }
            set { _command_itemList = value; onPropertyChange("Command_ItemList"); }
        }

        public Entity.Order Order
        {
            get { return _order; }
            set { _order = value; onPropertyChange("Command"); }
        }

        public Entity.Tax_order Tax_order
        {
            get { return _tax_command; }
            set { _tax_command = value; onPropertyChange("Tax_command"); }
        }

        public Entity.Tax Tax
        {
            get { return _tax; }
            set { _tax = value; onPropertyChange("Tax"); }
        }

        public List<Entity.Address> AddressList
        {
            get
            { return _addressList; }
            set { setProperty(ref _addressList, value); }
        }

        public AgentModel AgentModel
        {
            get { return _agentModel;  }
            set { _agentModel = value; onPropertyChange("AgentModel"); }
        }

        public ClientModel CLientModel
        {
            get { return _clientModel;  }
            set { _clientModel = value; onPropertyChange("CLientModel"); }
        }

        public string TxtID
        {
            get {  return _order.ID.addPrefix(Enums.EPrefix.ORDER); }
            set { _order.ID = Convert.ToInt32(value.deletePrefix());  onPropertyChange("TxtID"); }
        }

        public string TxtAgentId
        {
            get {  return _order.AgentId.ToString(); }
            set { _order.AgentId = Convert.ToInt32(value); onPropertyChange("TxtAgentId"); }
        }

        public string TxtClientId
        {
            get { return _order.ClientId.addPrefix(Enums.EPrefix.CLIENT); }
            set { _order.ClientId = Convert.ToInt32(value.deletePrefix()); onPropertyChange("TxtClientId"); }
        }

        public string TxtPrivateComment
        {
            get { return _order.Comment1; }
            set {  _order.Comment1 = value; onPropertyChange("TxtComment1"); }
        }

        public string TxtPublicComment
        {
            get {  return _order.Comment2; }
            set { _order.Comment2 = value; onPropertyChange("TxtComment2"); }
        }

        public string TxtAdminComment
        {
            get { return _order.Comment3; }
            set { _order.Comment3 = value; onPropertyChange("TxtComment3"); }
        }

        public string TxtBillAddress
        {
            get { return _order.BillAddress.ToString(); }
            set { _order.BillAddress = Convert.ToInt32(value); onPropertyChange("TxtBillAddress"); }
        }

        public string TxtDeliveryAddress
        {
            get {  return _order.DeliveryAddress.ToString(); }
            set { _order.DeliveryAddress = Convert.ToInt32(value); onPropertyChange("TxtDeliveryAddress"); }
        }

        public string TxtStatus
        {
            get {  return _order.Status; }
            set { _order.Status = value; onPropertyChange("TxtStatus"); }
        }

        public string TxtDate
        {
            get { return _order.Date.ToString(); }
            set { _order.Date = Convert.ToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public string TxtTaxName
        {
            get { return _order.Tax.ToString(); }
            set { _order.Tax = Convert.ToDouble(value); onPropertyChange("TxtTaxName"); }
        }
        



    }
}
