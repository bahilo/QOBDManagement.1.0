using QOBDBusiness;
using Entity = QOBDCommon.Entities;
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
        private Entity.Order_item _order_item;
        private ItemModel _itemModel;
        private decimal _percentProfit;
        private decimal _profit;
        private decimal _totalPurchase;
        private decimal _totalSelling; // PT
        private int _quantityPending;
        private int _quantityReceived;
        private int _nbPackages;

        public Order_itemModel()
        {
            _order_item = new Entity.Order_item();
            _itemModel = new ItemModel();
            _nbPackages = 1;
            PropertyChanged += onAmountOrQuantityOrObjectChange;
            //PropertyChanged += onOrder_itemChange;
        }

        public string TxtItemId
        {
            get { return _order_item.ItemId.ToString(); }
            set { _order_item.ItemId = Convert.ToInt32(value); onPropertyChange("TxtItemId"); }
        }

        public Entity.Order_item Order_Item
        {
            get { return _order_item; }
            set { setProperty(ref _order_item, value); }
        }

        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value); }
        }

        public string TxtTotalPurchase
        {
            get { return _totalPurchase.ToString(); }
            set { setProperty(ref _totalPurchase, Convert.ToDecimal(value)); }
        }

        public string TxtTotalSelling
        {
            get { return _totalSelling.ToString(); }
            set { setProperty(ref _totalSelling, Convert.ToDecimal(value)); }
        }

        public string TxtProfit
        {
            get { return _profit.ToString(); }
            set { setProperty(ref _profit, Convert.ToDecimal(value)); }
        }

        public string TxtPercentProfit
        {
            get { return _percentProfit.ToString(); }
            set { setProperty(ref _percentProfit, Convert.ToDecimal(value)); }
        }

        public string TxtID
        {
            get { return _order_item.ID.ToString(); }
            set { _order_item.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtOrderId
        {
            get { return _order_item.OrderId.addPrefix(Enums.EPrefix.ORDER); }
            set { _order_item.OrderId = Convert.ToInt32(value.deletePrefix()); onPropertyChange("TxtOrderId"); }
        }

        public string TxtItem_ref
        {
            get { return _order_item.Item_ref; }
            set { _order_item.Item_ref = value; onPropertyChange("TxtItems_ref"); }
        }

        public string TxtQuantity
        {
            get { return _order_item.Quantity.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _order_item.Quantity = converted; onPropertyChange("TxtQuantity"); } }
        }

        public string TxtQuantity_delivery
        {
            get { return _order_item.Quantity_delivery.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _order_item.Quantity_delivery = converted; onPropertyChange("TxtQuantity_delivery"); } }
        }

        public string TxtQuantity_current
        {
            get { return _order_item.Quantity_current.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _order_item.Quantity_current = converted; onPropertyChange("TxtQuantity_current"); } }
        }

        public string TxtQuantity_received
        {
            get { return _quantityReceived.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) setProperty(ref _quantityReceived, converted, "TxtQuantity_received"); }
        }

        public string TxtQuantity_pending
        {
            get { return (_quantityPending = _order_item.Quantity - Order_Item.Quantity_delivery).ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) setProperty(ref _quantityPending, converted); }
        }

        public string TxtPrice
        {
            get { return _order_item.Price.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _order_item.Price = converted; onPropertyChange(); } }
        }

        public string TxtPrice_purchase
        {
            get { return _order_item.Price_purchase.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _order_item.Price_purchase = converted; onPropertyChange("TxtPrice_purchase"); } }
        }

        public string TxtComment_Purchase_Price
        {
            get { return _order_item.Comment_Purchase_Price; }
            set { _order_item.Comment_Purchase_Price = value; onPropertyChange("TxtComment_Purchase_Price"); }
        }

        public string TxtOrder
        {
            get { return _order_item.Order.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _order_item.Order = converted; onPropertyChange("TxtOrder"); } }
        }

        public string TxtPackage
        {
            get { return _nbPackages.ToString(); }
            set { int converted; if(int.TryParse(value, out converted))setProperty(ref _nbPackages, converted, "TxtPackage"); }
        }

        //----------------------------[ Actions ]------------------
        
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
            TxtTotalSelling = Convert.ToString(_order_item.Quantity * _order_item.Price);
            TxtTotalPurchase = Convert.ToString(_order_item.Quantity * _order_item.Price_purchase);
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
                    resetSellingAndPurchasePrice();
                    profitCalcul();
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
