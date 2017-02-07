using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class StatisticModel : BindBase
    {
        private Statistic _statistic;
        private Decimal _totalTaxAmount;
        private int _daysLate;

        public StatisticModel()
        {
            _statistic = new Statistic();
        }

        public Statistic Statistic
        {
            get { return _statistic; }
            set { setProperty(ref _statistic, value); }
        }

        public string TxtTotalTaxAmount
        {
            get { return _totalTaxAmount.ToString(); }
            set { setProperty(ref _totalTaxAmount, Convert.ToDecimal(value)); }
        }

        public string TxtPayReceived
        {
            get { return _statistic.Pay_received.ToString(); }
            set { _statistic.Pay_received = Convert.ToDecimal(value); onPropertyChange(); }
        }

        public string TxtTotalIncome
        {
            get { return _statistic.Income.ToString(); }
            set { _statistic.Income = Convert.ToDecimal(value); onPropertyChange(); }
        }

        public string TxtTotalTaxIncluded
        {
            get { return _statistic.Total_tax_included.ToString(); }
            set { _statistic.Total_tax_included = Convert.ToDecimal(value); onPropertyChange(); }
        }

        public string TxtTotalTaxExcluded
        {
            get { return _statistic.Total.ToString(); }
            set { _statistic.Total = Convert.ToDecimal(value); onPropertyChange(); }
        }

        public string TxtTotalIncomePercent
        {
            get { return _statistic.Income_percent.ToString(); }
            set { _statistic.Income_percent = Convert.ToDouble(value); onPropertyChange(); }
        }

        public string TxtTotalPurchase
        {
            get { return _statistic.Price_purchase_total.ToString(); }
            set { _statistic.Price_purchase_total = Convert.ToDecimal(value); onPropertyChange(); }
        }

        public string TxtLimitDate
        {
            get { return _statistic.Date_limit.ToString(); }
            set { _statistic.Date_limit = Convert.ToDateTime(value); onPropertyChange(); }
        }

        public string TxtPaymentDate
        {
            get { return _statistic.Pay_date.ToString(); }
            set { _statistic.Pay_date = Convert.ToDateTime(value); onPropertyChange(); }
        }

        public string TxtTaxValue
        {
            get { return _statistic.Tax_value.ToString(); }
            set { _statistic.Tax_value = Convert.ToDouble(value); onPropertyChange(); }
        }

        public string TxtInvoiceDate
        {
            get { return _statistic.InvoiceDate.ToString(); }
            set { _statistic.InvoiceDate = Convert.ToDateTime(value); onPropertyChange(); }
        }

        public string TxtInvoiceId
        {
            get { return _statistic.InvoiceId.addPrefix(Enums.EPrefix.INVOICE); }
            set { _statistic.InvoiceId = Convert.ToInt32(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtCompanyName
        {
            get { return _statistic.Company; }
            set { _statistic.Company = value; onPropertyChange(); }
        }

        public string TxtDaysLate
        {
            get { return _daysLate.ToString(); }
            set { setProperty(ref _daysLate, Convert.ToInt32(value)); }
        }
    }
}
