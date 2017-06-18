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
using QOBDCommon.Classes;

namespace QOBDManagement.Models
{
    public class OrderModel : BindBase
    {
        private AgentModel _agentModel;
        private ClientModel _clientModel;
        private Entity.Address _deliveryAddress;
        private Entity.Address _billAddress;
        private List<Order_itemModel> _command_itemList;
        private List<Entity.Address> _addressList;
        private Tax_orderModel _tax_commandModel;
        private List<BillModel> _billModelList;
        private List<DeliveryModel> _deliveryModelList;
        private CurrencyModel _currencyModel;

        public OrderModel()
        {
            _currencyModel = new CurrencyModel();
            _tax_commandModel = new Tax_orderModel();
            _addressList = new List<Entity.Address>();
            _billModelList = new List<BillModel>();
            _deliveryModelList = new List<DeliveryModel>();
            _agentModel = new AgentModel();
            _clientModel = new ClientModel();
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
            set { _command_itemList = value; onPropertyChange(); }
        }

        public Entity.Order Order
        {
            get { return _tax_commandModel.Order; }
            set { _tax_commandModel.Order = value; onPropertyChange(); }
        }

        public Tax_orderModel Tax_orderModel
        {
            get { return _tax_commandModel; }
            set { _tax_commandModel = value; onPropertyChange(); }
        }

        public Entity.Tax Tax
        {
            get { return _tax_commandModel.Tax; }
            set { _tax_commandModel.Tax = value; onPropertyChange(); }
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
            set { _agentModel = value; onPropertyChange(); }
        }

        public ClientModel CLientModel
        {
            get { return _clientModel;  }
            set { _clientModel = value; onPropertyChange(); }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _currencyModel; }
            set { _currencyModel = value; onPropertyChange(); }
        }

        public string TxtID
        {
            get {  return _tax_commandModel.Order.ID.addPrefix(Enums.EPrefix.ORDER); }
            set { _tax_commandModel.Order.ID = Utility.intTryParse(value.deletePrefix());  onPropertyChange(); }
        }

        public string TxtAgentId
        {
            get {  return _tax_commandModel.Order.AgentId.ToString(); }
            set { _tax_commandModel.Order.AgentId = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtClientId
        {
            get { return _tax_commandModel.Order.ClientId.addPrefix(Enums.EPrefix.CLIENT); }
            set { _tax_commandModel.Order.ClientId = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtPrivateComment
        {
            get { return _tax_commandModel.Order.Comment1; }
            set {  _tax_commandModel.Order.Comment1 = value; onPropertyChange(); }
        }

        public string TxtPublicComment
        {
            get {  return _tax_commandModel.Order.Comment2; }
            set { _tax_commandModel.Order.Comment2 = value; onPropertyChange(); }
        }

        public string TxtAdminComment
        {
            get { return _tax_commandModel.Order.Comment3; }
            set { _tax_commandModel.Order.Comment3 = value; onPropertyChange(); }
        }

        public string TxtBillAddress
        {
            get { return _tax_commandModel.Order.BillAddress.ToString(); }
            set { _tax_commandModel.Order.BillAddress = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtDeliveryAddress
        {
            get {  return _tax_commandModel.Order.DeliveryAddress.ToString(); }
            set { _tax_commandModel.Order.DeliveryAddress = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtStatus
        {
            get {  return _tax_commandModel.Order.Status; }
            set { _tax_commandModel.Order.Status = value; onPropertyChange(); }
        }

        public string TxtDate
        {
            get { return _tax_commandModel.Order.Date.ToString(); }
            set { _tax_commandModel.Order.Date = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public string TxtTaxName
        {
            get { return _tax_commandModel.Order.Tax.ToString(); }
            set { _tax_commandModel.Order.Tax = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public override void Dispose()
        {
            PropertyChanged -= onAddressListChange;
            PropertyChanged -= onAgentModelChange;
            PropertyChanged -= onClientModelChange;
        }


    }
}
