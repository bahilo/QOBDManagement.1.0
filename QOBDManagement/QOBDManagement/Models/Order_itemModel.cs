using QOBDBusiness;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using QOBDManagement.Helper;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDCommon.Classes;

namespace QOBDManagement.Models
{
    public class Order_itemModel : BindBase
    {
        private string _outputStringFormat;
        private Order_item _order_item;
        private Order _order;
        private ItemModel _itemModel;
        private double _unitIncomPercent;
        private decimal _unitIncome;
        private decimal _totalPurchase;
        private decimal _totalSelling;
        private int _quantityPending;
        private int _quantityReceived;
        private int _nbPackages;
        private bool _isRowColored;
        private decimal _totalTaxAmount;
        private decimal _totalIncome;
        private double _totalIncomePercent;
        private decimal _totalTaxIncluded;
        private int _oldQuantity;
        private CurrencyModel _currencyModel;

        public Order_itemModel()
        {
            _order_item = new Order_item();
            _currencyModel = new CurrencyModel();
            _itemModel = new ItemModel();
            _nbPackages = 1;
        }

        public Order_itemModel(string outputStringFormat) : this()
        {
            _outputStringFormat = outputStringFormat;
        }

        public string TxtItemId
        {
            get { return _order_item.ItemId.ToString(); }
            set { _order_item.ItemId = Utility.intTryParse(value); onPropertyChange(); }
        }

        public Order_item Order_Item
        {
            get { return _order_item; }
            set { setProperty(ref _order_item, value); calcul(); }
        }

        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value); }
        }

        public Order Order
        {
            get { return _order; }
            set { setProperty(ref _order, value); }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _currencyModel; }
            set { if (value == null) return; _currencyModel = value; onPropertyChange(); }
        }

        public string TxtTotalIncome
        {
            get { return _totalIncome.ToString(_outputStringFormat); }
            set { setProperty(ref _totalIncome, Utility.decimalTryParse(value)); }
        }

        public string TxtTotalIncomePercent
        {
            get { return _totalIncomePercent.ToString(_outputStringFormat); }
            set { setProperty(ref _totalIncomePercent, (double)Utility.decimalTryParse(value)); }
        }

        public string TxtTotalTaxAmount
        {
            get { return _totalTaxAmount.ToString(_outputStringFormat); }
            set { setProperty(ref _totalTaxAmount, Utility.decimalTryParse(value)); }
        }

        public string TxtTotalTaxIncluded
        {
            get { return _totalTaxIncluded.ToString(_outputStringFormat); }
            set { setProperty(ref _totalTaxIncluded, Utility.decimalTryParse(value)); }
        }

        public string TxtTotalPurchase
        {
            get { return _totalPurchase.ToString(_outputStringFormat); }
            set { setProperty(ref _totalPurchase, Utility.decimalTryParse(value)); }
        }

        public string TxtTotalSelling
        {
            get { return _totalSelling.ToString(_outputStringFormat); }
            set { setProperty(ref _totalSelling, Utility.decimalTryParse(value)); }
        }

        public string TxtProfit
        {
            get { return _unitIncome.ToString(_outputStringFormat); }
            set { setProperty(ref _unitIncome, Utility.decimalTryParse(value)); }
        }

        public string TxtPercentProfit
        {
            get { return _unitIncomPercent.ToString(_outputStringFormat); }
            set { setProperty(ref _unitIncomPercent, (double)Utility.decimalTryParse(value)); }
        }

        public string TxtID
        {
            get { return _order_item.ID.ToString(); }
            set { _order_item.ID = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtOrderId
        {
            get { return _order_item.OrderId.addPrefix(Enums.EPrefix.ORDER); }
            set { _order_item.OrderId = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtItem_ref
        {
            get { return _order_item.Item_ref; }
            set { _order_item.Item_ref = value; onPropertyChange(); }
        }

        public string TxtQuantity
        {
            get { return _order_item.Quantity.ToString(); }
            set { _oldQuantity = _order_item.Quantity; _order_item.Quantity = Utility.intTryParse(value); onPropertyChange(); calcul(); }
        }

        public string TxtOldQuantity
        {
            get { return _oldQuantity.ToString(); }
            set { _oldQuantity = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtQuantity_delivery
        {
            get { return _order_item.Quantity_delivery.ToString(); }
            set { _order_item.Quantity_delivery = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtQuantity_current
        {
            get { return _order_item.Quantity_current.ToString(); }
            set { _order_item.Quantity_current = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtQuantity_received
        {
            get { return _quantityReceived.ToString(); }
            set { setProperty(ref _quantityReceived, Utility.intTryParse(value)); }
        }

        public bool IsQuantityReceivedEnabled
        {
            get
            {
                if (ItemModel.Item.Stock > 0)
                    return true;
                else
                    return false;
            }
        }

        public string TxtQuantity_pending
        {
            get { return (_quantityPending = _order_item.Quantity - Order_Item.Quantity_delivery).ToString(); }
            set { setProperty(ref _quantityPending, Utility.intTryParse(value)); }
        }

        public string TxtPrice
        {
            get { return decimal.Multiply(_order_item.Price, (CurrencyModel.Currency.Rate != 0 ? CurrencyModel.Currency.Rate : 1m)).ToString(_outputStringFormat); }
            set { _order_item.Price = Utility.decimalTryParse(value); onPropertyChange(); calcul(); }
        }

        public bool IsPriceChangeEnabled
        {
            get
            {
                if (Order != null && Order.Status.Equals(QOBDCommon.Enum.EOrderStatus.Quote.ToString()))
                    return true;
                else
                    return false;
            }
        }

        public string TxtPrice_purchase
        {
            get { return decimal.Multiply(Utility.decimalTryParse(TxtDefaultCurrencyPurchase_price), (CurrencyModel.Currency.Rate != 0 ? CurrencyModel.Currency.Rate : 1m)).ToString(_outputStringFormat); }
            set { _order_item.Price_purchase = Utility.decimalTryParse(value); onPropertyChange(); calcul(); }
        }

        public string TxtDefaultCurrencyPurchase_price
        {
            get { return decimal.Divide(_order_item.Price_purchase, (ItemModel.CurrencyModel.Currency.Rate != 0 ? ItemModel.CurrencyModel.Currency.Rate : 1m)).ToString(); }
        }

        public bool IsPrice_purchaseChangeEnabled
        {
            get
            {
                if (Order != null && Order.Status.Equals(QOBDCommon.Enum.EOrderStatus.Quote.ToString()))
                    return true;
                else
                    return false;
            }
        }

        public string TxtComment_Purchase_Price
        {
            get { return _order_item.Comment_Purchase_Price; }
            set { _order_item.Comment_Purchase_Price = value; onPropertyChange("TxtComment_Purchase_Price"); }
        }

        public string TxtSort
        {
            get { return _order_item.Rank.ToString(); }
            set { _order_item.Rank = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtPackage
        {
            get { return _nbPackages.ToString(); }
            set { setProperty(ref _nbPackages, Utility.intTryParse(value)); }
        }

        public bool IsRowColored
        {
            get { return _isRowColored; }
            set { setProperty(ref _isRowColored, value); }
        }

        //----------------------------[ Actions ]------------------
        
        public void calcul()
        {
            if (Order != null)
            {
                // convert price into credit if order status is credit
                _order_item.Price = (decimal)ConvertIfOrderCreditStatus(_order_item.Price);

                // convert purchase price into credit if order status is credit
                _order_item.Price_purchase = (decimal)ConvertIfOrderCreditStatus(_order_item.Price_purchase);

                // income percentage per unit calculation
                try
                {
                    _unitIncomPercent = (double)(decimal)ConvertIfOrderCreditStatus(((Math.Abs(Utility.decimalTryParse(TxtPrice)) - Math.Abs(Utility.decimalTryParse(TxtPrice_purchase))) / Math.Abs(Utility.decimalTryParse(TxtPrice))) * 100);
                }
                catch (DivideByZeroException)
                {
                    _unitIncomPercent = 0;
                }
                onPropertyChange("TxtPercentProfit");

                // income per unit calculation
                _unitIncome = (decimal)ConvertIfOrderCreditStatus((Math.Abs(Utility.decimalTryParse(TxtPrice)) - Math.Abs(Utility.decimalTryParse(TxtPrice_purchase))) * _order_item.Quantity);
                onPropertyChange("TxtProfit");

                // total purchase calculation
                _totalPurchase = (decimal)ConvertIfOrderCreditStatus(_order_item.Quantity * Math.Abs(Utility.decimalTryParse(TxtPrice_purchase)));
                onPropertyChange("TxtTotalPurchase");

                // total sales calculations
                _totalSelling = (decimal)ConvertIfOrderCreditStatus(_order_item.Quantity * Math.Abs(Utility.decimalTryParse(TxtPrice)));
                onPropertyChange("TxtTotalSelling");

                // tax amount calculation
                _totalTaxAmount = (decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalSelling) * (decimal)(Order.Tax / 100));
                onPropertyChange("TxtTotalTaxAmount");

                // income calculation
                _totalIncome = (decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalSelling) - Math.Abs(_totalPurchase));
                onPropertyChange("TxtTotalIncome");

                // percent income
                try
                {
                    _totalIncomePercent = (double)(decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalIncome) / Math.Abs(_totalSelling) * 100);
                }
                catch (DivideByZeroException)
                {
                    _totalIncomePercent = 0;
                }
                onPropertyChange("TxtTotalIncomePercent");

                // total tax included calculation
                _totalTaxIncluded = (decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalSelling) + Math.Abs(_totalTaxAmount));
                onPropertyChange("TxtTotalTaxIncluded");
            }            
        }

        public void calculWithDefaultCurrency()
        {
            if (Order != null)
            {
                // convert price into credit if order status is credit
                _order_item.Price = (decimal)ConvertIfOrderCreditStatus(_order_item.Price);

                // convert purchase price into credit if order status is credit
                _order_item.Price_purchase = (decimal)ConvertIfOrderCreditStatus(_order_item.Price_purchase);

                // income percentage per unit calculation
                try
                {
                    _unitIncomPercent = (double)(decimal)ConvertIfOrderCreditStatus(((Math.Abs(_order_item.Price) - Math.Abs(Utility.decimalTryParse(TxtDefaultCurrencyPurchase_price))) / Math.Abs(_order_item.Price)) * 100);
                }
                catch (DivideByZeroException)
                {
                    _unitIncomPercent = 0;
                }
                onPropertyChange("TxtPercentProfit");

                // income per unit calculation
                _unitIncome = (decimal)ConvertIfOrderCreditStatus((Math.Abs(_order_item.Price) - Math.Abs(Utility.decimalTryParse(TxtDefaultCurrencyPurchase_price))) * _order_item.Quantity);
                onPropertyChange("TxtProfit");

                // total purchase calculation
                _totalPurchase = (decimal)ConvertIfOrderCreditStatus(_order_item.Quantity * Math.Abs(Utility.decimalTryParse(TxtDefaultCurrencyPurchase_price)));
                onPropertyChange("TxtTotalPurchase");

                // total sales calculations
                _totalSelling = (decimal)ConvertIfOrderCreditStatus(_order_item.Quantity * Math.Abs(_order_item.Price));
                onPropertyChange("TxtTotalSelling");

                // tax amount calculation
                _totalTaxAmount = (decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalSelling) * (decimal)(Order.Tax / 100));
                onPropertyChange("TxtTotalTaxAmount");

                // income calculation
                _totalIncome = (decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalSelling) - Math.Abs(_totalPurchase));
                onPropertyChange("TxtTotalIncome");

                // percent income
                try
                {
                    _totalIncomePercent = (double)(decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalIncome) / Math.Abs(_totalSelling) * 100);
                }
                catch (DivideByZeroException)
                {
                    _totalIncomePercent = 0;
                }
                onPropertyChange("TxtTotalIncomePercent");

                // total tax included calculation
                _totalTaxIncluded = (decimal)ConvertIfOrderCreditStatus(Math.Abs(_totalSelling) + Math.Abs(_totalTaxAmount));
                onPropertyChange("TxtTotalTaxIncluded");
            }            
        }

        private object ConvertIfOrderCreditStatus(object value)
        {
            decimal convertedValue = (decimal)value;

            if(Order != null && (Order.Status.Equals(QOBDCommon.Enum.EOrderStatus.Credit.ToString()) || Order.Status.Equals(QOBDCommon.Enum.EOrderStatus.Pre_Credit.ToString())))
            {
                convertedValue *= -1;
            }
            return convertedValue;
        }

    }
}
