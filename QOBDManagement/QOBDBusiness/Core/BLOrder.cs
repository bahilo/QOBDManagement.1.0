using QOBDBusiness.Helper;
using QOBDCommon;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.BL;
using QOBDCommon.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDBusiness.Core
{
    public class BLOrder : IOrderManager
    {
        // Attributes

        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC;

        public event PropertyChangedEventHandler PropertyChanged;

        public BLOrder(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALOrder.initializeCredential(user);
        }


        public void setServiceCredential(string login, string password)
        {
            DAC.DALOrder.setServiceCredential(login, password);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            if (progressBarFunc != null)
                DAC.DALOrder.progressBarManagement(progressBarFunc);
        }

        public void UpdateOrderDependencies(List<Order> orderList, bool isActiveProgress = false)
        {
            if (orderList != null)
                DAC.DALOrder.UpdateOrderDependencies(orderList, isActiveProgress);
        }


        public async Task<List<Order>> InsertOrderAsync(List<Order> orderList)
        {
            List<Order> result = new List<Order>();
            if (orderList == null || orderList.Count == 0)
                return result;
            try
            {
                result = await DAC.DALOrder.InsertOrderAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> InsertOrderBillAsync(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.InsertBillAsync(billList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> InsertOrderDeliveryAsync(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();

            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.InsertDeliveryAsync(deliveryList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax>> InsertOrderTaxAsync(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALOrder.InsertTaxAsync(taxList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax_order>> InsertTax_orderAsync(List<Tax_order> tax_orderList)
        {
            if (tax_orderList == null || tax_orderList.Count == 0)
                return new List<Tax_order>();

            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = await DAC.DALOrder.InsertTax_orderAsync(tax_orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order_item>> InsertOrder_itemAsync(List<Order_item> order_itemList)
        {
            if (order_itemList == null || order_itemList.Count == 0)
                return new List<Order_item>();

            List<Order_item> result = new List<Order_item>();
            try
            {
                result = await DAC.DALOrder.InsertOrder_itemAsync(order_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax>> InsertTaxAsync(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALOrder.InsertTaxAsync(taxList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> InsertBillAsync(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.InsertBillAsync(billList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> InsertDeliveryAsync(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();


            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.InsertDeliveryAsync(deliveryList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order>> UpdateOrderAsync(List<Order> orderList)
        {
            if (orderList == null || orderList.Count == 0)
                return new List<Order>();

            if (orderList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating commands(count = " + orderList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Order> result = new List<Order>();
            try
            {
                result = await DAC.DALOrder.UpdateOrderAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }
        

        public async Task<List<Tax_order>> UpdateTax_orderAsync(List<Tax_order> tax_orderList)
        {
            if (tax_orderList == null || tax_orderList.Count == 0)
                return new List<Tax_order>();

            if (tax_orderList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating tax_commands(count = " + tax_orderList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = await DAC.DALOrder.UpdateTax_orderAsync(tax_orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order_item>> UpdateOrder_itemAsync(List<Order_item> order_itemList)
        {
            if (order_itemList == null || order_itemList.Count == 0)
                return new List<Order_item>();

            if (order_itemList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating order_items(count = " + order_itemList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = await DAC.DALOrder.UpdateOrder_itemAsync(order_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax>> UpdateTaxAsync(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            if (taxList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating Taxes(count = " + taxList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALOrder.UpdateTaxAsync(taxList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> UpdateBillAsync(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            if (billList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating bills(count = " + billList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.UpdateBillAsync(billList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> UpdateDeliveryAsync(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();

            if (deliveryList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating deliveries(count = " + deliveryList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.UpdateDeliveryAsync(deliveryList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order>> DeleteOrderAsync(List<Order> orderList)
        {
            if (orderList == null || orderList.Count == 0)
                return new List<Order>();

            if (orderList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting commands(count = " + orderList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Order> result = new List<Order>();
            try
            {
                result = await DAC.DALOrder.DeleteOrderAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        
        public async Task<List<Tax_order>> DeleteTax_orderAsync(List<Tax_order> tax_orderList)
        {
            if (tax_orderList == null || tax_orderList.Count == 0)
                return new List<Tax_order>();

            if (tax_orderList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting tax_commands(count = " + tax_orderList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = await DAC.DALOrder.DeleteTax_orderAsync(tax_orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order_item>> DeleteOrder_itemAsync(List<Order_item> order_itemList)
        {
            if (order_itemList == null || order_itemList.Count == 0)
                return new List<Order_item>();

            if (order_itemList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting command_items(count = " + order_itemList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = await DAC.DALOrder.DeleteOrder_itemAsync(order_itemList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax>> DeleteTaxAsync(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            if (taxList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting Taxes(count = " + taxList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALOrder.DeleteTaxAsync(taxList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> DeleteBillAsync(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            if (billList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting bills(count = " + billList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.DeleteBillAsync(billList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> DeleteDeliveryAsync(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();

            if (deliveryList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting deliveries(count = " + deliveryList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.DeleteDeliveryAsync(deliveryList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order> GetOrderData(int nbLine)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = DAC.DALOrder.GetOrderData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order>> GetOrderDataAsync(int nbLine)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = await DAC.DALOrder.GetOrderDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order> GetOrderDataById(int id)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = DAC.DALOrder.GetOrderDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Tax_order> GetTax_orderData(int nbLine)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = DAC.DALOrder.GetTax_orderData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax_order>> GetTax_orderDataAsync(int nbLine)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = await DAC.DALOrder.GetTax_orderDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Tax_order> GetTax_orderDataByOrderList(List<Order> orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = DAC.DALOrder.GetTax_orderDataByOrderList(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax_order>> GetTax_orderDataByOrderListAsync(List<Order> orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = await DAC.DALOrder.GetTax_orderDataByOrderListAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Tax_order> GetTax_orderDataById(int id)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = DAC.DALOrder.GetTax_orderDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order_item> GetOrder_itemData(int nbLine)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = DAC.DALOrder.GetOrder_itemData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemDataAsync(int nbLine)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = await DAC.DALOrder.GetOrder_itemDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order_item> GetOrder_itemByOrderList(List<Order> orderList)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = DAC.DALOrder.GetOrder_itemByOrderList(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemByOrderListAsync(List<Order> orderList)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = await DAC.DALOrder.GetOrder_itemByOrderListAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order_item> GetOrder_itemDataById(int id)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = DAC.DALOrder.GetOrder_itemDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Tax> GetTaxData(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = DAC.DALOrder.GetTaxData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax>> GetTaxDataAsync(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALOrder.GetTaxDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Tax> GetTaxDataById(int id)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = DAC.DALOrder.GetTaxDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Bill> GetBillData(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = DAC.DALOrder.GetBillData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> GetBillDataAsync(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.GetBillDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> GetUnpaidBillDataByAgentAsync(int agentId)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.GetUnpaidBillDataByAgentAsync(agentId);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Bill> GetBillDataByOrderList(List<Order> orderList)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = DAC.DALOrder.GetBillDataByOrderList(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> GetBillDataByOrderListAsync(List<Order> orderList)
        {
            List<Bill> result = new List<Bill>();
            if (orderList == null || orderList.Count == 0)
                return result;
            try
            {
                result = await DAC.DALOrder.GetBillDataByOrderListAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public  List<Bill> GetBillDataById(int id)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = DAC.DALOrder.GetBillDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<Bill> GetLastBillAsync()
        {
            Bill result = new Bill();
            try
            {
                result = await DAC.DALOrder.GetLastBillAsync();
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Delivery> GetDeliveryData(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = DAC.DALOrder.GetDeliveryData(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataAsync(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.GetDeliveryDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Delivery> GetDeliveryDataByOrderList(List<Order> orderList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = DAC.DALOrder.GetDeliveryDataByOrderList(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataByOrderListAsync(List<Order> orderList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.GetDeliveryDataByOrderListAsync(orderList);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Delivery> GetDeliveryDataById(int id)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = DAC.DALOrder.GetDeliveryDataById(id);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order> searchOrder(Order order, ESearchOption filterOperator)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = DAC.DALOrder.searchOrder(order, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order>> searchOrderAsync(Order order, ESearchOption filterOperator)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = await DAC.DALOrder.searchOrderAsync(order, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }


        public List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = DAC.DALOrder.searchTax_order(Tax_order, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }


        public async Task<List<Tax_order>> searchTax_orderAsync(Tax_order Tax_order, ESearchOption filterOperator)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = await DAC.DALOrder.searchTax_orderAsync(Tax_order, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Order_item> searchOrder_item(Order_item Order_item, ESearchOption filterOperator)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = DAC.DALOrder.searchOrder_item(Order_item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Order_item>> searchOrder_itemAsync(Order_item Order_item, ESearchOption filterOperator)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = await DAC.DALOrder.searchOrder_itemAsync(Order_item, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Tax> searchTax(Tax Tax, ESearchOption filterOperator)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = DAC.DALOrder.searchTax(Tax, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Tax>> searchTaxAsync(Tax Tax, ESearchOption filterOperator)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALOrder.searchTaxAsync(Tax, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Bill> searchBill(Bill Bill, ESearchOption filterOperator)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = DAC.DALOrder.searchBill(Bill, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Bill>> searchBillAsync(Bill Bill, ESearchOption filterOperator)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALOrder.searchBillAsync(Bill, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = DAC.DALOrder.searchDelivery(Delivery, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public async Task<List<Delivery>> searchDeliveryAsync(Delivery Delivery, ESearchOption filterOperator)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALOrder.searchDeliveryAsync(Delivery, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message); }
            return result;
        }

        public void GeneratePdfOrder(ParamOrderToPdf paramCommandToPdf)
        {
            try
            {
                DAC.DALOrder.GeneratePdfOrder(paramCommandToPdf);
            }
            catch (Exception ex) { Log.error(ex.Message); }
        }

        public void GeneratePdfQuote(ParamOrderToPdf paramCommandToPdf)
        {
            try
            {
                DAC.DALOrder.GeneratePdfQuote(paramCommandToPdf);
            }
            catch (Exception ex) { Log.error(ex.Message); }
        }

        public void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf)
        {
            try
            {
                DAC.DALOrder.GeneratePdfDelivery(paramDeliveryToPdf);
            }
            catch (Exception ex) { Log.error(ex.Message); }
        }

        public void Dispose()
        {
            DAC.DALOrder.Dispose();
        }
    } /* end class BLCommande */
}