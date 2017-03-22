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
        private decimal _totalSelling; // PT
        private int _quantityPending;
        private int _quantityReceived;
        private int _nbPackages;
        private bool _isRowColored;
        private decimal _totalTaxAmount;
        private decimal _totalIncome;
        private double _totalIncomePercent;
        private decimal _totalTaxIncluded;

        public Order_itemModel()
        {
            _order_item = new Order_item();
            _itemModel = new ItemModel();
            _nbPackages = 1;
            PropertyChanged += onAmountOrQuantityOrObjectChange;
            //PropertyChanged += onOrder_itemChange;
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
            set { setProperty(ref _order_item, value); }
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
            set { _order_item.Quantity = Utility.intTryParse(value); onPropertyChange(); }
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

        public string TxtQuantity_pending
        {
            get { return (_quantityPending = _order_item.Quantity - Order_Item.Quantity_delivery).ToString(); }
            set { setProperty(ref _quantityPending, Utility.intTryParse(value)); }
        }

        public string TxtPrice
        {
            get { return _order_item.Price.ToString(_outputStringFormat); }
            set { _order_item.Price = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtPrice_purchase
        {
            get { return _order_item.Price_purchase.ToString(_outputStringFormat); }
            set { _order_item.Price_purchase = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtComment_Purchase_Price
        {
            get { return _order_item.Comment_Purchase_Price; }
            set { _order_item.Comment_Purchase_Price = value; onPropertyChange("TxtComment_Purchase_Price"); }
        }

        public string TxtSort
        {
            get { return _order_item.Order.ToString(); }
            set { _order_item.Order = Utility.intTryParse(value); onPropertyChange(); }
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
            // convert price into credit if order status is credit
            _order_item.Price = (decimal)ConvertIfOrderCreditStatus(_order_item.Price);
            //onPropertyChange("TxtPrice");

            // convert purchase price into credit if order status is credit
            _order_item.Price_purchase = (decimal)ConvertIfOrderCreditStatus(_order_item.Price_purchase);
            //onPropertyChange("TxtPrice_purchase");

            // income percentage per unit calculation
            try
            {
                _unitIncomPercent = (double)(decimal)ConvertIfOrderCreditStatus(((Math.Abs(_order_item.Price) - Math.Abs(_order_item.Price_purchase)) / Math.Abs(_order_item.Price)) * 100);                
            }
            catch (DivideByZeroException)
            {
                _unitIncomPercent = 0;
            }
            onPropertyChange("TxtPercentProfit");

            // income per unit calculation
            _unitIncome = (decimal)ConvertIfOrderCreditStatus((Math.Abs(_order_item.Price) - Math.Abs(_order_item.Price_purchase)) * _order_item.Quantity);
            onPropertyChange("TxtProfit");

            // total purchase calculation
            _totalPurchase = (decimal)ConvertIfOrderCreditStatus(_order_item.Quantity * Math.Abs(_order_item.Price_purchase));
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

        private object ConvertIfOrderCreditStatus(object value)
        {
            decimal convertedValue = (decimal)value;

            if(Order.Status.Equals(QOBDCommon.Enum.EOrderStatus.Credit.ToString()) || Order.Status.Equals(QOBDCommon.Enum.EOrderStatus.Pre_Credit.ToString()))
            {
                convertedValue *= -1;
            }
            return convertedValue;
        }

        private void profitCalcul()
        {
            try
            {
                if(_order_item.Price_purchase >= 0)
                    TxtPercentProfit = string.Format("{0:0.00}", (((_order_item.Price - _order_item.Price_purchase) / _order_item.Price) * 100));
                else
                    // In case of order converted into credit
                    TxtPercentProfit = string.Format("{0:0.00}", ( -1 * ((_order_item.Price - _order_item.Price_purchase) / _order_item.Price) * 100));
            }
            catch (DivideByZeroException)
            {
                TxtPercentProfit = 0.ToString();
            }

            if (_order_item.Price_purchase >= 0)
                TxtProfit = string.Format("{0:0.00}", (_order_item.Price - _order_item.Price_purchase) * _order_item.Quantity);
            else
                // In case of order converted into credit
                TxtProfit = string.Format("{0:0.00}", -1 * (_order_item.Price - _order_item.Price_purchase) * _order_item.Quantity);
        }

        private void resetSellingAndPurchasePrice()
        {
            _totalPurchase = _order_item.Quantity * _order_item.Price_purchase;
            _totalPurchase *= (_order_item.Price_purchase < 0) ? -1 : 1 ;
            onPropertyChange("TxtTotalPurchase");

            _totalSelling = _order_item.Quantity * _order_item.Price;
            _totalSelling *= (_order_item.Price < 0) ? -1 : 1;
            onPropertyChange("TxtTotalSelling");
        }

        //----------------------------[ Event Handler ]------------------

        //private void onQuantityChange(object sender, PropertyChangedEventArgs e)
        //{
        //    if (string.Equals(e.PropertyName, "TxtQuantity") || string.Equals(e.PropertyName, "Order_Item"))
        //    {

        //    }
        //        profitCalcul();
        //}

        //private void onOrder_itemChange(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName.Equals("Order_Item"))
        //    {
        //        resetSellingAndPurchasePrice();
        //        profitCalcul();
        //    }
        //}
        

        private void onAmountOrQuantityOrObjectChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtPrice") 
                || e.PropertyName.Equals("TxtPrice_purchase")
                || e.PropertyName.Equals("TxtQuantity")
                || e.PropertyName.Equals("Order_Item"))
            {
                if ( _order_item.Price_purchase != 0 && _order_item.Price != 0)
                {
                    /*resetSellingAndPurchasePrice();
                    profitCalcul();*/
                    calcul();
                }                    
            }            
        }

        public override void Dispose()
        {
            PropertyChanged -= onAmountOrQuantityOrObjectChange;
            //PropertyChanged -= onOrder_itemChange;
        }

    }
}
