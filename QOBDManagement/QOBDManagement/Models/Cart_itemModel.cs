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
    public class Cart_itemModel : BindBase
    {
        //private List<Item_deliveryModel> _item_deliveryModelList;
        private decimal _totalPurchasePrice;
        private decimal _totalSellingPrice; // PT
        private decimal _total;
        private decimal _cartTotalPurchasePrice; // PAT
        private decimal _cartTotalSellingPrice; //PTT
        private bool _isSelected;
        //private bool _isModifyEnable;
        private Item _item;
        private int _quantity;
        private int _oldQuantity;

        public Cart_itemModel()
        {
            _item = new Item();
            PropertyChanged += onItemOrQuantityChange;
        }

        public Item Item
        {
            get { return _item; }
            set { _item = value; onPropertyChange(); }
        }

        public string TxtCartTotalPurchasePrice
        {
            get { return _cartTotalPurchasePrice.ToString(); }
            set { _cartTotalPurchasePrice = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtCartTotalSellingPrice
        {
            get { return _cartTotalSellingPrice.ToString(); }
            set { _cartTotalSellingPrice = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtID
        {
            get { return _item.ID.ToString(); }
            set { _item.ID = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtRef
        {
            get { return Item.Ref; }
            set { Item.Ref = value; onPropertyChange(); }
        }

        public string TxtName
        {
            get { return Item.Name; }
            set { Item.Name = value; onPropertyChange(); }
        }

        public string TxtType
        {
            get { return Item.Type; }
            set { Item.Type = value; onPropertyChange(); }
        }

        public string TxtType_sub
        {
            get { return Item.Type_sub; }
            set { Item.Type_sub = value; onPropertyChange(); }
        }

        public string TxtPrice_purchase
        {
            get { return Item.Price_purchase.ToString(); }
            set { Item.Price_purchase = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtPrice_sell
        {
            get { return Item.Price_sell.ToString(); }
            set { Item.Price_sell = Utility.decimalTryParse(value); onPropertyChange(); }
        }

        public string TxtSource
        {
            get { return Item.Source.ToString(); }
            set { Item.Source = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtComment
        {
            get { return Item.Comment; }
            set { Item.Comment = value; onPropertyChange(); }
        }

        public string TxtErasable
        {
            get { return Item.Erasable; }
            set { Item.Erasable = value; onPropertyChange(); }
        }

        public string TxtTotalPurchasePrice
        {
            get { return _totalPurchasePrice.ToString(); }
            set { setProperty(ref _totalPurchasePrice, Utility.decimalTryParse(value)); }
        }

        public string TxtTotalSellingPrice
        {
            get { return _totalSellingPrice.ToString(); }
            set { setProperty(ref _totalSellingPrice, Utility.decimalTryParse(value)); }
        }

        public string TxtTotal
        {
            get { return _total.ToString(); }
            set { setProperty(ref _total, Utility.decimalTryParse(value)); }
        }

        public bool IsItemSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; onPropertyChange(); }
        }

        public string TxtQuantity
        {
            get { return _quantity.ToString(); }
            set { _oldQuantity = _quantity ; _quantity = Utility.intTryParse(value); onPropertyChange(); }
        }

        public string TxtOldQuantity
        {
            get { return _oldQuantity.ToString(); }
            set { _oldQuantity = Utility.intTryParse(value); onPropertyChange(); }
        }

        private void onItemOrQuantityChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "TxtQuantity"))
            {
                TxtTotalSellingPrice = (_quantity * Item.Price_sell).ToString();
                TxtTotalPurchasePrice = (_quantity * Item.Price_purchase).ToString();
            }
        }
    }
}
