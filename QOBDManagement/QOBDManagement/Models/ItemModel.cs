using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDCommon.Classes;
using QOBDManagement.Helper;

namespace QOBDManagement.Models
{
    public class ItemModel: BindBase
    {       
        private List<Item_deliveryModel> _item_deliveryModelList;
        private List<Provider> _providerList;
        private string _selectedBrand;
        private string _newBrand;
        private string _selectedFamily;
        private string _newFamily;
        private string _newProvider;
        private Provider _selectedProvider;
        private bool _isSelected;
        private bool _isModifyEnable;
        private bool _isSearchByItemName;
        private bool _isExactMatch;
        private bool _isDeepSearch;
        private Item _item;

        public ItemModel()
        {
            _item_deliveryModelList = new List<Item_deliveryModel>();
            _providerList = new List<Provider>();
            _selectedProvider = new Provider();
            _item = new Item();

            PropertyChanged += onItemChange;
        }

        private void onItemChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Item"))
            {
                foreach (var item_deliveryModel in Item_deliveryModelList)
                {
                    item_deliveryModel.ItemModel = new ItemModel { Item = Item };
                }
            }
        }

        public string SelectedBrand
        {
            get { return _selectedBrand; }
            set { setProperty(ref _selectedBrand, value); }
        }

        public string SelectedFamily
        {
            get { return _selectedFamily; }
            set { setProperty(ref _selectedFamily, value); }
        }
        public bool IsSearchByItemName
        {
            get { return _isSearchByItemName; }
            set { setProperty(ref _isSearchByItemName ,value); }
        }

        public bool IsExactMatch
        {
            get { return _isExactMatch; }
            set { setProperty(ref _isExactMatch, value); }
        }

        public bool IsDeepSearch
        {
            get { return _isDeepSearch; }
            set { setProperty(ref _isDeepSearch,value); }
        }

        public bool IsRefModifyEnable
        {
            get { return _isModifyEnable; }
            set { setProperty(ref _isModifyEnable, value); }
        }

        public List<Provider> ProviderList
        {
            get { return _providerList; }
            set { setProperty(ref _providerList, value); }
        }

        public Provider SelectedProvider
        {
            get { return _selectedProvider; }
            set { setProperty(ref _selectedProvider, value); }
        }

        public Item Item
        {
            get { return _item; }
            set { setProperty(ref _item ,value); }
        }

        public List<Item_deliveryModel> Item_deliveryModelList
        {
            get { return _item_deliveryModelList; }
            set { setProperty(ref _item_deliveryModelList, value); }
        }

        public string TxtNewProvider
        {
            get { return _newProvider; }
            set { setProperty(ref _newProvider, value); }
        }

        public string txtProvider
        {
            get { return _selectedProvider.Name; }
        }

        public string TxtNewBrand
        {
            get { return _newBrand; }
            set { setProperty(ref _newBrand, value); }
        }

        public string TxtNewFamily
        {
            get { return _newFamily; }
            set { setProperty(ref _newFamily, value); }
        }

        public string TxtID
        {
            get { return _item.ID.addPrefix(Enums.EPrefix.ITEM); }
            set { _item.ID = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
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

        public string TxtStock
        {
            get { return Item.Stock.ToString(); }
            set { Item.Stock = Utility.intTryParse(value); onPropertyChange(); }
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

        public bool IsItemSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; onPropertyChange(); }
        }        

    }
}
