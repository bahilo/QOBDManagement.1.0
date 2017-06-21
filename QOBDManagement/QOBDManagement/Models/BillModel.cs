using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDBusiness;
using System.Xml.Serialization;
using QOBDCommon.Classes;
using QOBDManagement.Helper;

namespace QOBDManagement.Models
{
    public class BillModel : BindBase
    {
        private Bill _bill;
        private bool _isConstructorRefVisible;
        
        private BusinessLogic _bl;
        private ClientModel _clientModel;
        private OrderModel _orderModel;
        private NotificationModel _notificaionModel;
        private StatisticModel _statisticModel;
        private CurrencyModel _currencyModel;
        private string _outputStringFormat;

        public BillModel()
        {
            _bill = new Bill();
            _currencyModel = new CurrencyModel();
            _clientModel = new ClientModel();
            _orderModel = new OrderModel();
            _notificaionModel = new NotificationModel();
            _statisticModel = new StatisticModel();

            PropertyChanged += onTaxValueChange;
        }


        public BusinessLogic Bl
        {
            get { return _bl; }
            set { setProperty(ref _bl, value); }
        }


        public ClientModel ClientModel
        {
            get { return _clientModel; }
            set { setProperty(ref _clientModel, value); }
        }

        public OrderModel OrderModel
        {
            get { return _orderModel; }
            set { setProperty(ref _orderModel, value); }
        }

        public bool IsConstructorRefVisible
        {
            get { return _isConstructorRefVisible; }
            set { setProperty(ref _isConstructorRefVisible, value); }
        }        

        public Bill Bill
        {
            get { return _bill; }
            set { setProperty(ref _bill, value); }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _currencyModel; }
            set { _currencyModel = value; onPropertyChange(); }
        }

        public string TxtID
        {
            get { return _bill.ID.addPrefix(Enums.EPrefix.INVOICE); }
            set { int converted; if (int.TryParse(value.deletePrefix(), out converted)) { _bill.ID = converted; } else _bill.ID = 0; onPropertyChange();}
        }

        public string TxtClientId
        {
            get { return _bill.ClientId.addPrefix(Enums.EPrefix.CLIENT); }
            set { int converted; if (int.TryParse(value.deletePrefix(), out converted)) { _bill.ClientId = converted; } else _bill.ClientId = 0; onPropertyChange();}
        }

        public string TxtOrderId
        {
            get { return _bill.OrderId.addPrefix(Enums.EPrefix.ORDER); }
            set { int converted; if (int.TryParse(value.deletePrefix(), out converted)) { _bill.OrderId = converted; } else _bill.OrderId = 0; onPropertyChange(); }
        }

        public string TxtPayMod
        {
            get { return _bill.PayMod; }
            set { _bill.PayMod = value; onPropertyChange(); }
        }

        public string TxtPay
        {
            get { return (_bill.Pay * (CurrencyModel.Currency.Rate != 0 ? CurrencyModel.Currency.Rate : 1m)).ToString(_outputStringFormat); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _bill.Pay = converted; } else _bill.Pay = 0; onPropertyChange(); }
        }

        public string TxtPayReceived
        {
            get { return (_bill.PayReceived / (CurrencyModel.Currency.Rate != 0 ? CurrencyModel.Currency.Rate : 1m)).ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _bill.PayReceived = converted; } else _bill.PayReceived = 0; onPropertyChange(); }
        }

        public string TxtPrivateComment
        {
            get { return _bill.Comment1; }
            set { _bill.Comment1 = value; onPropertyChange(); }
        }

        public string TxtPublicComment
        {
            get { return _bill.Comment2; }
            set { _bill.Comment2 = value; onPropertyChange(); }
        }

        public string TxtDate
        {
            get { return _bill.Date.ToString(); }
            set { _bill.Date = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public string TxtDateLimit
        {
            get { return _bill.DateLimit.ToString(); }
            set { _bill.DateLimit = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public string TxtPayDate
        {
            get { return _bill.DatePay.ToString("MM/dd/yyyy"); }
            set { _bill.DatePay = Utility.convertToDateTime(value, true); onPropertyChange(); }
        }

        public string TxtOutputStringFormat
        {
            get { return _outputStringFormat; }
            set { _outputStringFormat = value; onPropertyChange(); }
        }

        //=====================[ Events ]========================

        private void onTaxValueChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtTaxValue"))
            {
                StatisticModel.Statistic.Total_tax_included = StatisticModel.Statistic.Total + (decimal)_statisticModel.Statistic.Tax_value * StatisticModel.Statistic.Total;
            }
        }


        //=====================[ Statistic ]========================

        public StatisticModel StatisticModel
        {
            get { return _statisticModel; }
            set { setProperty(ref _statisticModel, value); }
        }

        public string TxtDaysLate
        {
            get { return _statisticModel.TxtDaysLate; }
            set { _statisticModel.TxtDaysLate = value; onPropertyChange(); }
        }

        public string TxtTaxValue
        {
            get { return _statisticModel.TxtTaxValue; }
            set { _statisticModel.TxtTaxValue = value; onPropertyChange(); }
        }

        public string TxtTotalIncome
        {
            get { return _statisticModel.TxtTotalIncome; }
            set { _statisticModel.TxtTotalIncome = value; onPropertyChange(); }
        }

        public string TxtTotalIncomePercent
        {
            get { return _statisticModel.TxtTotalIncomePercent; }
            set { _statisticModel.TxtTotalIncomePercent = value; onPropertyChange(); }
        }

        public string TxtTotalPurchase
        {
            get { return _statisticModel.TxtTotalPurchase; }
            set { _statisticModel.TxtTotalPurchase = value; onPropertyChange(); }
        }

        public string TxtTotalTaxAmount
        {
            get { return _statisticModel.TxtTotalTaxAmount; }
            set { _statisticModel.TxtTotalTaxAmount = value; onPropertyChange(); }
        }

        public string TxtTotalTaxExcluded
        {
            get { return _statisticModel.TxtTotalTaxExcluded; }
            set { _statisticModel.TxtTotalTaxExcluded = value; onPropertyChange(); }
        }

        public string TxtTotalTaxIncluded
        {
            get { return _statisticModel.TxtTotalTaxIncluded; }
            set { _statisticModel.TxtTotalTaxIncluded = value; onPropertyChange(); }
        }

        //=====================[ Notification ]========================
        
        public NotificationModel NotificationModel
        {
            get { return _notificaionModel; }
            set { setProperty(ref _notificaionModel, value); }
        }

        public string TxtDateFirstReminder
        {
            get { return (_notificaionModel.Notification.Reminder1 > Utility.DateTimeMinValueInSQL2005) ? _notificaionModel.TxtReminder1 : " - "; }
            set { _notificaionModel.TxtReminder1 = value; onPropertyChange(); }
        }

        public string TxtDateSecondReminder
        {
            get { return (_notificaionModel.Notification.Reminder2 > Utility.DateTimeMinValueInSQL2005) ?_notificaionModel.TxtReminder2 : " - "; }
            set { _notificaionModel.TxtReminder2 = value; onPropertyChange(); }
        }
        

    }
}
