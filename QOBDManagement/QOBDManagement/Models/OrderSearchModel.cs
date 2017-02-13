using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class OrderSearchModel : System.ComponentModel.INotifyPropertyChanged
    {
        OrderSearch _orderSearch;
        public event PropertyChangedEventHandler PropertyChanged;

        public OrderSearchModel()
        {
            _orderSearch = new OrderSearch();
            _orderSearch.StatusList = new List<string>
            {
                EOrderStatus.Quote.ToString(),                  //devis
                EOrderStatus.Pre_Order.ToString(),             //preco
                EOrderStatus.Order.ToString(),                //command
                EOrderStatus.Order_Close.ToString(),           // close
                EOrderStatus.Pre_Credit.ToString(),              // preavoir
                EOrderStatus.Credit.ToString(),                 // avoir
                EOrderStatus.Credit_CLose.ToString(),            // a_close
                EOrderStatus.Pre_Client_Validation.ToString(),    // revalid
                EOrderStatus.Bill_Order.ToString(),            // facture
                EOrderStatus.Bill_Credit.ToString(),              // a_facture
                EOrderStatus.Billed.ToString(),                    //f
                EOrderStatus.Not_Billed.ToString()               //nf}
            };
            TxtStartDate = "";// DateTime.Now;
            TxtEndDate = "";// DateTime.Now;
        }


        public OrderSearch OrderSearch
        {
            get { return _orderSearch; }
            set { _orderSearch = value; onPropertyChange(); }
        }


        public string TxtOrderId
        {
            get { return _orderSearch.OrderId.addPrefix(Enums.EPrefix.ORDER); }
            set { _orderSearch.OrderId = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtBillId
        {
            get { return _orderSearch.BillId.addPrefix(Enums.EPrefix.INVOICE); }
            set { _orderSearch.BillId = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtSelectedStatus
        {
            get { return _orderSearch.SelectedStatus; }
            set { _orderSearch.SelectedStatus = value; onPropertyChange(); }
        }

        public List<string> StatusList
        {
            get { return _orderSearch.StatusList; }
            set { _orderSearch.StatusList = value; onPropertyChange(); }
        }

        public Agent SelectedAgent
        {
            get { return _orderSearch.SelectedAgent; }
            set { _orderSearch.SelectedAgent = value; onPropertyChange(); }
        }

        public List<Agent> AgentList
        {
            get { return _orderSearch.AgentList; }
            set { _orderSearch.AgentList = value; onPropertyChange(); }
        }

        public string TxtCompanyName
        {
            get { return _orderSearch.CompanyName; }
            set { _orderSearch.CompanyName = value; onPropertyChange(); }
        }

        public string TxtClientId
        {
            get { return _orderSearch.ClientId.addPrefix(Enums.EPrefix.CLIENT); }
            set { _orderSearch.ClientId = Utility.intTryParse(value.deletePrefix()); onPropertyChange(); }
        }

        public string TxtStartDate
        {
            get { return _orderSearch.StartDate.ToString(); }
            set { _orderSearch.StartDate = Utility.convertToDateTime(value, true); onPropertyChange(); }
        }

        public string TxtEndDate
        {
            get { return _orderSearch.EndDate.ToString(); }
            set { _orderSearch.EndDate = Utility.convertToDateTime(value, true); onPropertyChange(); }
        }

        public bool IsDeepSearch
        {
            get { return _orderSearch.IsDeepSearch; }
            set { _orderSearch.IsDeepSearch = value; onPropertyChange(); }
        }


        public void onPropertyChange([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
