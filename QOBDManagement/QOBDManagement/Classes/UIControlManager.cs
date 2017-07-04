using QOBDCommon.Enum;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Classes
{
    public static class UIControlManager
    {
        
        /// <summary>
        /// disable IU item regarding the order status
        /// </summary>
        /// <param name="obj">the item to disable</param>
        /// <returns>boolean type expected by the IU</returns>
        public static bool disableUIElementByBoolean(OrderModel SelectedOrder, [CallerMemberName]string obj = "")
        {
            // Lock order when all invoices have been generated
            if ((SelectedOrder.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Bill_Credit.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            // Prevent updating information when the order has been closed
            if ((SelectedOrder.TxtStatus.Equals(EOrderStatus.Order_Close.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Credit_CLose.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            // prevent price, purchase, and quantity update outside quote
            if (!SelectedOrder.TxtStatus.Equals(EOrderStatus.Quote.ToString())
                && (obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            return true;
        }

        /// <summary>
        /// disable IU item regarding the order status
        /// </summary>
        /// <param name="obj">the item to disable</param>
        /// <returns>string type expected by the IU</returns>
        public static string disableUIElementByString(OrderModel SelectedOrder, List<Item_deliveryModel> Item_ModelDeliveryInProcess, List<Item_deliveryModel> Item_deliveryModelBillingInProcess, [CallerMemberName]string obj = "")
        {
            if (SelectedOrder.TxtStatus == null  && obj.Equals("BlockItemListDetailVisibility"))
                return "Collapsed";

            if (SelectedOrder.TxtStatus == null)
                return "Hidden";

            if ((!SelectedOrder.TxtStatus.Equals(EOrderStatus.Order.ToString()) && !SelectedOrder.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Collapsed";

            // Show order details when converted into order or credit
            else if ((SelectedOrder.TxtStatus.Equals(EOrderStatus.Order.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Visible";

            if ((!SelectedOrder.TxtStatus.Equals(EOrderStatus.Order.ToString()) && !SelectedOrder.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                && (obj.Equals("BlockDeliveryListToIncludeVisibility")
                || obj.Equals("BlockStepOneVisibility")
                || obj.Equals("BlockStepTwoVisibility")
                || obj.Equals("BlockStepThreeVisibility")))
                return "Hidden";

            if (SelectedOrder.TxtStatus.Equals(EOrderStatus.Quote.ToString())
                && (obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                || obj.Equals("BlockBillListBoxVisibility")
                ))
                return "Hidden";

            if ( (SelectedOrder.TxtStatus.Equals(EOrderStatus.Pre_Order.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Pre_Credit.ToString()))
                && (obj.Equals("BlockEmailVisibility")
                || obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                ))
                return "Hidden";

            if (Item_deliveryModelBillingInProcess == null || Item_ModelDeliveryInProcess == null || (SelectedOrder.TxtStatus.Equals(EOrderStatus.Order.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Credit.ToString()))
                && (obj.Equals("BlockStepOneVisibility") && Item_ModelDeliveryInProcess.Count == 0
                || obj.Equals("BlockStepTwoVisibility") && Item_deliveryModelBillingInProcess.Count == 0
                || obj.Equals("BlockStepThreeVisibility") && SelectedOrder.BillModelList.Count == 0
                ))
                return "Hidden";

            if ((SelectedOrder.TxtStatus.Equals(EOrderStatus.Bill_Order.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Bill_Credit.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            if ((SelectedOrder.TxtStatus.Equals(EOrderStatus.Order_Close.ToString()) || SelectedOrder.TxtStatus.Equals(EOrderStatus.Credit_CLose.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            return "Visible";
        }

        /// <summary>
        /// disable IU item regarding the user credentials
        /// </summary>
        /// <param name="obj">the item to disable</param>
        /// <returns>string type expected by the IU</returns>
        public static string disableUIElementByString(bool isVisible, [CallerMemberName]string obj = "")
        {
            if (isVisible && obj.Equals("BoxVisibility"))
                return "Visible";  
            if (isVisible && obj.Equals("BlockVisibility"))
                return "Visible";           

            return "Hidden";
        }

    }
}
