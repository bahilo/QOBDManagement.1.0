using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Interfaces.DAC;
using QOBDDAL.App_Data;
using QOBDDAL.App_Data.QOBDSetTableAdapters;
using QOBDDAL.Helper.ChannelHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ComponentModel;
using QOBDCommon.Structures;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading;
using System.Collections.Concurrent;
using QOBDCommon.Classes;
using QOBDGateway.Core;
using QOBDCommon.Enum;
using QOBDGateway.QOBDServiceReference;

namespace QOBDDAL.Core
{
    public class DALOrder : IOrderManager
    {
        private Func<double, double> _progressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IOrderManager _gatewayOrder;
        private QOBDWebServicePortTypeClient _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALOrder(QOBDWebServicePortTypeClient servicePort)
        {
            _servicePortType = servicePort;
            _gatewayOrder = new GateWayOrder(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gatewayOrder.setServiceCredential(_servicePortType);
                retrieveGateWayOrderData();
            }
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (QOBDWebServicePortTypeClient)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.Login;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gatewayOrder.setServiceCredential(_servicePortType);
        }

        private void retrieveGateWayOrderData()
        {
            try
            {
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                UpdateOrderDependencies(new NotifyTaskCompletion<List<Order>>(_gatewayOrder.searchOrderAsync(new Order { AgentId = AuthenticatedUser.ID }, ESearchOption.AND)).Task.Result.Take(_loadSize).ToList(), true);
                //Log.debug("-- Orders loaded --");
            }
            catch (Exception ex) { Log.error(ex.Message); }
            finally { lock (_lock) IsLodingDataFromWebServiceToLocal = true; }

        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }


        public async Task<List<Order>> InsertOrderAsync(List<Order> listOrder)
        {
            List<Order> result = new List<Order>();
            List<Order> gateWayResultList = new List<Order>();
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.InsertOrderAsync(listOrder);

                result = LoadOrder(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Tax_order>> InsertTax_orderAsync(List<Tax_order> listTax_order)
        {
            List<Tax_order> result = new List<Tax_order>();
            List<Tax_order> gateWayResultList = new List<Tax_order>();
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.InsertTax_orderAsync(listTax_order);

                result = LoadTax_order(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Order_item>> InsertOrder_itemAsync(List<Order_item> listOrder_item)
        {
            List<Order_item> result = new List<Order_item>();
            List<Order_item> gateWayResultList = new List<Order_item>();
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.InsertOrder_itemAsync(listOrder_item);

                result = LoadOrder_item(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Tax>> InsertTaxAsync(List<Tax> listTax)
        {
            List<Tax> result = new List<Tax>();
            List<Tax> gateWayResultList = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.InsertTaxAsync(listTax);

                result = LoadTax(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Bill>> InsertBillAsync(List<Bill> listBill)
        {
            List<Bill> result = new List<Bill>();
            List<Bill> gateWayResultList = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.InsertBillAsync(listBill);

                result = LoadBill(gateWayResultList);
            }
            return result;
        }

        public async Task<List<Delivery>> InsertDeliveryAsync(List<Delivery> listDelivery)
        {
            List<Delivery> result = new List<Delivery>();
            List<Delivery> gateWayResultList = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.InsertDeliveryAsync(listDelivery);

                result = LoadDelivery(gateWayResultList);
            }
            return result;
        }


        public async Task<List<Order>> DeleteOrderAsync(List<Order> listOrder)
        {
            List<Order> result = new List<Order>();
            List<Order> gateWayResultList = new List<Order>();
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.DeleteOrderAsync(listOrder);
                if (gateWayResultList.Count == 0)
                    foreach (Order order in listOrder)
                    {
                        int returnValue = _ordersTableAdapter.Delete1(order.ID);
                        if (returnValue == 0)
                            result.Add(order);
                    }
            }
            return result;
        }

        public async Task<List<Tax_order>> DeleteTax_orderAsync(List<Tax_order> listTax_order)
        {
            List<Tax_order> result = new List<Tax_order>();
            List<Tax_order> gateWayResultList = new List<Tax_order>();
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.DeleteTax_orderAsync(listTax_order);
                if (gateWayResultList.Count == 0)
                    foreach (Tax_order tax_order in listTax_order)
                    {
                        int returnValue = _tax_ordersTableAdapter.Delete1(tax_order.ID);
                        if (returnValue == 0)
                            result.Add(tax_order);
                    }
            }
            return result;
        }

        public async Task<List<Order_item>> DeleteOrder_itemAsync(List<Order_item> listOrder_item)
        {
            List<Order_item> result = new List<Order_item>();
            List<Order_item> gateWayResultList = new List<Order_item>();
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.DeleteOrder_itemAsync(listOrder_item);
                if (gateWayResultList.Count == 0)
                    foreach (Order_item order_item in listOrder_item)
                    {
                        int returnValue = _order_itemsTableAdapter.Delete1(order_item.ID);
                        if (returnValue == 0)
                            result.Add(order_item);
                    }
            }
            return result;
        }

        public async Task<List<Tax>> DeleteTaxAsync(List<Tax> listTax)
        {
            List<Tax> result = new List<Tax>();
            List<Tax> gateWayResultList = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.DeleteTaxAsync(listTax);
                if (gateWayResultList.Count == 0)
                    foreach (Tax tax in listTax)
                    {
                        int returnValue = _taxesTableAdapter.Delete1(tax.ID);
                        if (returnValue == 0)
                            result.Add(tax);
                    }
            }
            return result;
        }

        public async Task<List<Bill>> DeleteBillAsync(List<Bill> listBill)
        {
            List<Bill> result = new List<Bill>();
            List<Bill> gateWayResultList = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.DeleteBillAsync(listBill);
                if (gateWayResultList.Count == 0)
                    foreach (Bill bill in listBill)
                    {
                        int returnValue = _billsTableAdapter.Delete1(bill.ID);
                        if (returnValue == 0)
                            result.Add(bill);
                    }
            }
            return result;
        }

        public async Task<List<Delivery>> DeleteDeliveryAsync(List<Delivery> listDelivery)
        {
            List<Delivery> result = new List<Delivery>();
            List<Delivery> gateWayResultList = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.DeleteDeliveryAsync(listDelivery);
                if (gateWayResultList.Count == 0)
                    foreach (Delivery delivery in listDelivery)
                    {
                        int returnValue = _deliveriesTableAdapter.Delete1(delivery.ID);
                        if (returnValue == 0)
                            result.Add(delivery);
                    }
            }
            return result;
        }

        public async Task<List<Order>> UpdateOrderAsync(List<Order> ordersList)
        {
            List<Order> result = new List<Order>();
            List<Order> gateWayResultList = new List<Order>();
            QOBDSet dataSet = new QOBDSet();
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.UpdateOrderAsync(ordersList);

                foreach (var order in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _ordersTableAdapter.FillById(dataSetLocal.commands, order.ID);
                    dataSet.commands.Merge(dataSetLocal.commands);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _ordersTableAdapter.Update(gateWayResultList.OrderTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Order> LoadOrder(List<Order> ordersList)
        {
            List<Order> result = new List<Order>();
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
            {
                foreach (var order in ordersList)
                {
                    int returnResult = _ordersTableAdapter
                                            .load_data_order(
                                                    order.AgentId,
                                                    order.ClientId,
                                                    order.Comment1,
                                                    order.Comment2,
                                                    order.Comment3,
                                                    order.BillAddress,
                                                    order.DeliveryAddress,
                                                    order.Status,
                                                    order.Date,
                                                    order.Tax,
                                                    order.ID);
                    if (returnResult > 0)
                        result.Add(order);
                }
            }
            return result;
        }

        public async Task<List<Tax_order>> UpdateTax_orderAsync(List<Tax_order> tax_orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            List<Tax_order> gateWayResultList = new List<Tax_order>();
            QOBDSet dataSet = new QOBDSet();
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.UpdateTax_orderAsync(tax_orderList);

                foreach (var tax_order in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _tax_ordersTableAdapter.FillById(dataSetLocal.tax_commands, tax_order.ID);
                    dataSet.tax_commands.Merge(dataSetLocal.tax_commands);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _tax_ordersTableAdapter.Update(gateWayResultList.Tax_orderTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Tax_order> LoadTax_order(List<Tax_order> tax_ordersList)
        {
            List<Tax_order> result = new List<Tax_order>();
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
            {
                foreach (var tax_order in tax_ordersList)
                {
                    int returnResult = _tax_ordersTableAdapter
                                            .load_data_tax_order(
                                                    tax_order.OrderId,
                                                    tax_order.TaxId,
                                                    tax_order.Date_insert,
                                                    tax_order.Tax_value.ToString(),
                                                    tax_order.Target,
                                                    tax_order.ID);
                    if (returnResult > 0)
                        result.Add(tax_order);
                }
            }
            return result;
        }

        public async Task<List<Order_item>> UpdateOrder_itemAsync(List<Order_item> order_itemList)
        {
            List<Order_item> result = new List<Order_item>();
            List<Order_item> gateWayResultList = new List<Order_item>();
            QOBDSet dataSet = new QOBDSet();
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.UpdateOrder_itemAsync(order_itemList);

                foreach (var order_item in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _order_itemsTableAdapter.FillById(dataSetLocal.command_items, order_item.ID);
                    dataSet.command_items.Merge(dataSetLocal.command_items);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _order_itemsTableAdapter.Update(gateWayResultList.Order_itemTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Order_item> LoadOrder_item(List<Order_item> order_itemsList)
        {
            List<Order_item> result = new List<Order_item>();
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
            {
                foreach (var order_item in order_itemsList)
                {
                    int returnResult = _order_itemsTableAdapter
                                            .load_order_item(
                                                    order_item.OrderId,
                                                    order_item.ItemId,
                                                    order_item.Item_ref,
                                                    order_item.Quantity,
                                                    order_item.Quantity_delivery,
                                                    order_item.Quantity_current,
                                                    order_item.Comment_Purchase_Price,
                                                    order_item.Price,
                                                    order_item.Price_purchase,
                                                    order_item.Order,
                                                    order_item.ID);
                    if (returnResult > 0)
                        result.Add(order_item);
                }
            }
            return result;
        }

        public async Task<List<Tax>> UpdateTaxAsync(List<Tax> taxesList)
        {
            List<Tax> result = new List<Tax>();
            List<Tax> gateWayResultList = new List<Tax>();
            QOBDSet dataSet = new QOBDSet();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.UpdateTaxAsync(taxesList);

                foreach (var tax in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _taxesTableAdapter.FillById(dataSetLocal.taxes, tax.ID);
                    dataSet.taxes.Merge(dataSetLocal.taxes);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _taxesTableAdapter.Update(gateWayResultList.TaxTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Tax> LoadTax(List<Tax> taxesList)
        {
            List<Tax> result = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            {
                foreach (var tax in taxesList)
                {
                    int returnResult = _taxesTableAdapter
                                            .load_data_tax(
                                                tax.Type,
                                                tax.Date_insert,
                                                tax.Value.ToString(),
                                                tax.Comment,
                                                tax.Tax_current,
                                                tax.ID);
                    if (returnResult > 0)
                        result.Add(tax);
                }
            }
            return result;
        }

        public async Task<List<Bill>> UpdateBillAsync(List<Bill> billList)
        {
            List<Bill> result = new List<Bill>();
            List<Bill> gateWayResultList = new List<Bill>();
            QOBDSet dataSet = new QOBDSet();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.UpdateBillAsync(billList);

                foreach (var bill in gateWayResultList)
                {
                    QOBDSet dataSetLocal = new QOBDSet();
                    _billsTableAdapter.FillById(dataSetLocal.bills, bill.ID);
                    dataSet.bills.Merge(dataSetLocal.bills);
                }

                if (gateWayResultList.Count > 0)
                {
                    int returnValue = _billsTableAdapter.Update(gateWayResultList.BillTypeToDataTable(dataSet));
                    if (returnValue == gateWayResultList.Count)
                        result = gateWayResultList;
                }
            }
            return result;
        }

        public List<Bill> LoadBill(List<Bill> billList)
        {
            List<Bill> result = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            {
                foreach (var bill in billList)
                {
                    int returnResult = _billsTableAdapter
                                            .load_data_bill(
                                                bill.ClientId,
                                                bill.OrderId,
                                                bill.PayMod,
                                                bill.Pay,
                                                bill.PayReceived,
                                                bill.Comment1,
                                                bill.Comment2,
                                                bill.Date,
                                                bill.DateLimit,
                                                bill.PayDate,
                                                bill.ID);
                    if (returnResult > 0)
                        result.Add(bill);
                }
            }
            return result;
        }

        public async Task<List<Delivery>> UpdateDeliveryAsync(List<Delivery> deliveryList)
        {
            List<Delivery> result = new List<Delivery>();
            List<Delivery> gateWayResultList = new List<Delivery>();
            QOBDSet dataSet = new QOBDSet();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            {
                gateWayResultList = await _gatewayOrder.UpdateDeliveryAsync(deliveryList);

                foreach (var delivery in gateWayResultList)
                {
                    int returnValue = _deliveriesTableAdapter.Update1(delivery.OrderId,
                                                                        delivery.BillId,
                                                                        delivery.Package,
                                                                        delivery.Date,
                                                                        delivery.Status,
                                                                        delivery.ID);
                    if (returnValue >= gateWayResultList.Count)
                        result.Add(delivery);
                }
            }
            return result;
        }

        public List<Delivery> LoadDelivery(List<Delivery> deliveryList)
        {
            List<Delivery> result = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            {
                foreach (var delivery in deliveryList)
                {
                    int returnResult = _deliveriesTableAdapter
                                            .load_data_delivery(
                                                delivery.OrderId,
                                                delivery.BillId,
                                                delivery.Package,
                                                delivery.Date,
                                                delivery.Status,
                                                delivery.ID);
                    if (returnResult > 0)
                        result.Add(delivery);
                }
            }
            return result;
        }


        public List<Order> GetOrderData(int nbLine)
        {
            List<Order> result = new List<Order>();
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
                result = _ordersTableAdapter.GetData().DataTableTypeToOrder();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }


        public async Task<List<Order>> GetOrderDataAsync(int nbLine)
        {
            List<Order> result = new List<Order>();
            return await _gatewayOrder.GetOrderDataAsync(nbLine);

        }

        public List<Order> GetOrderDataById(int id)
        {
            using (ordersTableAdapter _ordersTableAdapter = new ordersTableAdapter())
                return _ordersTableAdapter.get_order_by_id(id).DataTableTypeToOrder();
        }


        public List<Tax_order> GetTax_orderData(int nbLine)
        {
            List<Tax_order> result = new List<Tax_order>();
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
                result = _tax_ordersTableAdapter.GetData().DataTableTypeToTax_order();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }


        public async Task<List<Tax_order>> GetTax_orderDataAsync(int nbLine)
        {
            return await _gatewayOrder.GetTax_orderDataAsync(nbLine);

        }

        public List<Tax_order> GetTax_orderDataByOrderList(List<Order> orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            foreach (Order order in orderList)
            {
                var tax_orderFoundList = GetTax_orderByOrderId(order.ID);
                if (tax_orderFoundList.Count() > 0)
                    result.Add(tax_orderFoundList.First());
            }
            return result;
        }

        public async Task<List<Tax_order>> GetTax_orderDataByOrderListAsync(List<Order> orderList)
        {
            return await _gatewayOrder.GetTax_orderDataByOrderListAsync(orderList);
        }

        public List<Tax_order> GetTax_orderByOrderId(int orderId)
        {
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
                return _tax_ordersTableAdapter.get_tax_order_by_id(orderId).DataTableTypeToTax_order();
        }

        public List<Tax_order> GetTax_orderDataById(int id)
        {
            using (tax_ordersTableAdapter _tax_ordersTableAdapter = new tax_ordersTableAdapter())
                return _tax_ordersTableAdapter.get_tax_order_by_id(id).DataTableTypeToTax_order();
        }

        public List<Order_item> GetOrder_itemData(int nbLine)
        {
            List<Order_item> result = new List<Order_item>();
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
                result = _order_itemsTableAdapter.GetData().DataTableTypeToOrder_item();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Order_item>> GetOrder_itemDataAsync(int nbLine)
        {
            return await _gatewayOrder.GetOrder_itemDataAsync(nbLine);
        }

        public async Task<List<Order_item>> GetOrder_itemByOrderListAsync(List<Order> ordersList)
        {
            return await _gatewayOrder.GetOrder_itemByOrderListAsync(ordersList);
        }

        public List<Order_item> GetOrder_itemByOrderList(List<Order> ordersList)
        {
            List<Order_item> result = new List<Order_item>();
            foreach (Order order in ordersList)
            {
                var order_itemFoundList = GetOrder_itemDataByOrderId(order.ID);
                if (order_itemFoundList.Count() > 0)
                    result.Add(order_itemFoundList.First());
            }

            return result;
        }

        public List<Order_item> GetOrder_itemDataById(int id)
        {
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
                return _order_itemsTableAdapter.get_order_item_by_id(id).DataTableTypeToOrder_item();
        }

        public List<Order_item> GetOrder_itemDataByOrderId(int orderId)
        {
            using (order_itemsTableAdapter _order_itemsTableAdapter = new order_itemsTableAdapter())
                return _order_itemsTableAdapter.get_order_item_by_order_id(orderId).DataTableTypeToOrder_item();
        }

        public List<Tax> GetTaxData(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
                result = _taxesTableAdapter.GetData().DataTableTypeToTax();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax>> GetTaxDataAsync(int nbLine)
        {
            return await _gatewayOrder.GetTaxDataAsync(nbLine);
        }

        public List<Tax> GetTaxDataById(int id)
        {
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
                return _taxesTableAdapter.get_tax_by_id(id).DataTableTypeToTax();
        }

        public List<Bill> GetBillData(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
                result = _billsTableAdapter.GetData().DataTableTypeToBill();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Bill>> GetUnpaidBillDataByAgentAsync(int agentId)
        {
            return await _gatewayOrder.GetUnpaidBillDataByAgentAsync(agentId);
        }

        public async Task<List<Bill>> GetBillDataAsync(int nbLine)
        {
            return await _gatewayOrder.GetBillDataAsync(nbLine);
        }

        public List<Bill> GetBillDataByOrderList(List<Order> orderList)
        {
            List<Bill> result = new List<Bill>();
            foreach (Order order in orderList)
            {
                var billFoundList = GetBillDataByOrderId(order.ID);
                if (billFoundList.Count() > 0)
                    result.Add(billFoundList.First());
            }
            return result;
        }

        public async Task<List<Bill>> GetBillDataByOrderListAsync(List<Order> orderList)
        {
            return await _gatewayOrder.GetBillDataByOrderListAsync(orderList);

        }

        public List<Bill> GetBillDataByOrderId(int orderId)
        {
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
                return _billsTableAdapter.get_bill_by_order_id(orderId).DataTableTypeToBill();
        }

        public List<Bill> GetBillDataById(int id)
        {
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
                return _billsTableAdapter.get_bill_by_id(id).DataTableTypeToBill();
        }

        public async Task<Bill> GetLastBillAsync()
        {
            return await _gatewayOrder.GetLastBillAsync();
        }

        public List<Delivery> GetDeliveryData(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
                result = _deliveriesTableAdapter.GetData().DataTableTypeToDelivery();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Delivery>> GetDeliveryDataAsync(int nbLine)
        {
            return await _gatewayOrder.GetDeliveryDataAsync(nbLine);
        }

        public List<Delivery> GetDeliveryDataByOrderList(List<Order> orderList)
        {
            List<Delivery> result = new List<Delivery>();
            foreach (Order order in orderList)
            {
                var deliveryFoundList = GetDeliveryDataByOrderId(order.ID);
                if (deliveryFoundList.Count() > 0)
                    result.Add(deliveryFoundList.First());
            }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataByOrderListAsync(List<Order> orderList)
        {
            return await _gatewayOrder.GetDeliveryDataByOrderListAsync(orderList);
        }

        public List<Delivery> GetDeliveryDataByOrderId(int orderId)
        {
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
                return _deliveriesTableAdapter.get_delivery_by_order_id(orderId).DataTableTypeToDelivery();
        }

        public List<Delivery> GetDeliveryDataById(int id)
        {
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
                return _deliveriesTableAdapter.get_delivery_by_id(id).DataTableTypeToDelivery();
        }

        public List<Order> searchOrder(Order order, ESearchOption filterOperator)
        {
            return order.orderTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Order>> searchOrderAsync(Order order, ESearchOption filterOperator)
        {
            return await _gatewayOrder.searchOrderAsync(order, filterOperator);
        }

        public List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator)
        {
            return Tax_order.Tax_orderTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Tax_order>> searchTax_orderAsync(Tax_order Tax_order, ESearchOption filterOperator)
        {
            return await _gatewayOrder.searchTax_orderAsync(Tax_order, filterOperator);
        }

        public List<Order_item> searchOrder_item(Order_item order_item, ESearchOption filterOperator)
        {
            return order_item.order_itemTypeToFilterDataTable(filterOperator);

        }

        public async Task<List<Order_item>> searchOrder_itemAsync(Order_item order_item, ESearchOption filterOperator)
        {
            return await _gatewayOrder.searchOrder_itemAsync(order_item, filterOperator);
        }

        public List<Tax> searchTax(Tax Tax, ESearchOption filterOperator)
        {
            return Tax.TaxTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Tax>> searchTaxAsync(Tax Tax, ESearchOption filterOperator)
        {
            return await _gatewayOrder.searchTaxAsync(Tax, filterOperator);
        }

        public List<Bill> searchBill(Bill Bill, ESearchOption filterOperator)
        {
            return Bill.BillTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Bill>> searchBillAsync(Bill Bill, ESearchOption filterOperator)
        {
            return await _gatewayOrder.searchBillAsync(Bill, filterOperator);
        }

        public List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator)
        {
            return Delivery.DeliveryTypeToFilterDataTable(filterOperator);
        }

        public async Task<List<Delivery>> searchDeliveryAsync(Delivery Delivery, ESearchOption filterOperator)
        {
            return await _gatewayOrder.searchDeliveryAsync(Delivery, filterOperator);
        }

        public void GeneratePdfOrder(ParamOrderToPdf paramOrderToPdf)
        {
            _gatewayOrder.GeneratePdfOrder(paramOrderToPdf);
        }

        public void GeneratePdfQuote(ParamOrderToPdf paramOrderToPdf)
        {
            _gatewayOrder.GeneratePdfQuote(paramOrderToPdf);
        }

        public void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf)
        {
            _gatewayOrder.GeneratePdfDelivery(paramDeliveryToPdf);
        }

        public void Dispose()
        {
            _gatewayOrder.Dispose();
        }


        //----------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------//

        public void UpdateOrderDependencies(List<Order> orders, bool isActiveProgress = false)
        {
            int loadUnit = 25;

            ConcurrentBag<Order> orderList;
            ConcurrentBag<Order_item> order_itemList = new ConcurrentBag<Order_item>();
            ConcurrentBag<Item> itemList = new ConcurrentBag<Item>();
            ConcurrentBag<Provider_item> provider_itemList = new ConcurrentBag<Provider_item>();
            ConcurrentBag<Provider> providerList = new ConcurrentBag<Provider>();
            ConcurrentBag<Item_delivery> item_deliveryList = new ConcurrentBag<Item_delivery>();
            ConcurrentBag<Delivery> deliveryList = new ConcurrentBag<Delivery>();
            ConcurrentBag<Tax_item> tax_itemList = new ConcurrentBag<Tax_item>();
            ConcurrentBag<Tax> taxList = new ConcurrentBag<Tax>();
            ConcurrentBag<Bill> billList = new ConcurrentBag<Bill>();
            ConcurrentBag<Client> clientList = new ConcurrentBag<Client>();
            ConcurrentBag<Contact> contactList = new ConcurrentBag<Contact>();
            ConcurrentBag<Address> addressList = new ConcurrentBag<Address>();
            ConcurrentBag<Tax_order> tax_orderList = new ConcurrentBag<Tax_order>();

            int step = 100 / _progressStep;

            DALItem dalItem = new DALItem(_servicePortType);
            dalItem.AuthenticatedUser = AuthenticatedUser;
            dalItem.GateWayItem.setServiceCredential(_servicePortType);
            dalItem.IsLodingDataFromWebServiceToLocal = true;
            
            DALClient dalClient = new DALClient(_servicePortType);
            dalClient.AuthenticatedUser = AuthenticatedUser;
            dalClient.setServiceCredential(_servicePortType);
            dalClient.IsLodingDataFromWebServiceToLocal = true;

            orderList = new ConcurrentBag<Order>(orders);

            // Address loading
            if (orderList.Count > 0)
            {
                var addressfoundList = new NotifyTaskCompletion<List<Address>>(dalClient.GateWayClient.GetAddressDataByOrderListAsync(orderList.ToList())).Task.Result;
                addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressfoundList)));
                var savedAddressList = dalClient.LoadAddress(addressList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Tax_order Loading
            if (orderList.Count > 0)
            {
                var tax_orderfoundList = new NotifyTaskCompletion<List<Tax_order>>(_gatewayOrder.GetTax_orderDataByOrderListAsync(orderList.ToList())).Task.Result; // await GateWayOrder.GetTax_orderDataByOrderList(new List<Order>(orderList.Skip(i * loadUnit).Take(loadUnit)));
                tax_orderList = new ConcurrentBag<Tax_order>(tax_orderList.Concat(new ConcurrentBag<Tax_order>(tax_orderfoundList)));
                List<Tax_order> savedTax_orderList = LoadTax_order(tax_orderList.ToList());
            }

            // Tax Loading
            var taxFoundList = new NotifyTaskCompletion<List<Tax>>(_gatewayOrder.GetTaxDataAsync(999)).Task.Result; // await GateWayOrder.GetTax_orderDataByOrderList(new List<Order>(orderList.Skip(i * loadUnit).Take(loadUnit)));
            taxList = new ConcurrentBag<Tax>(new ConcurrentBag<Tax>(taxFoundList));
            List<Tax> savedTaxList = LoadTax(taxList.ToList());
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Bills Loading
            if (orderList.Count > 0)
            {
                List<Bill> billfoundList = new NotifyTaskCompletion<List<Bill>>(_gatewayOrder.GetBillDataByOrderListAsync(orderList.ToList())).Task.Result; // await GateWayOrder.GetBillDataByOrderList(new List<Order>(orderList.Skip(i * loadUnit).Take(loadUnit)));
                billList = new ConcurrentBag<Bill>(billList.Concat(new ConcurrentBag<Bill>(billfoundList)));
                List<Bill> savedBillList = LoadBill(billList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Delivery Loading
            if (orderList.Count > 0)
            {
                List<Delivery> deliveryfoundList = new NotifyTaskCompletion<List<Delivery>>(_gatewayOrder.GetDeliveryDataByOrderListAsync(orderList.ToList())).Task.Result; //await GateWayOrder.GetDeliveryDataByOrderList(new List<Order>(orderList.Skip(i * loadUnit).Take(loadUnit)));
                deliveryList = new ConcurrentBag<Delivery>(deliveryList.Concat(new ConcurrentBag<Delivery>(deliveryfoundList)));
                List<Delivery> savedDeliveryList = LoadDelivery(deliveryList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Order_item Loading
            if (orderList.Count > 0)
            {
                for (int i = 0; i < (orderList.Count() / loadUnit) || loadUnit > orderList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Order_item> order_itemFoundList = new ConcurrentBag<Order_item>(new NotifyTaskCompletion<List<Order_item>>(_gatewayOrder.GetOrder_itemByOrderListAsync(orderList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await GateWayOrder.GetOrder_itemByOrderList(new List<Order>(orderList.Skip(i * loadUnit).Take(loadUnit)));
                    order_itemList = new ConcurrentBag<Order_item>(order_itemList.Concat(new ConcurrentBag<Order_item>(order_itemFoundList)));
                }
                var savedOrder_itemList = new ConcurrentBag<Order_item>(LoadOrder_item(order_itemList.ToList()));
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Item Loading
            if (order_itemList.Count > 0)
            {
                for (int i = 0; i < (order_itemList.Count() / loadUnit) || loadUnit > order_itemList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Item> itemFoundList = new ConcurrentBag<Item>(new NotifyTaskCompletion<List<Item>>(dalItem.GateWayItem.GetItemDataByOrder_itemListAsync(order_itemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByOrder_itemList(new List<Order_item>(order_itemList.Skip(i * loadUnit).Take(loadUnit)));
                    itemList = new ConcurrentBag<Item>(itemList.Concat(new ConcurrentBag<Item>(itemFoundList)));
                }
                var savedItemList = new ConcurrentBag<Item>(dalItem.LoadItem(itemList.ToList()));
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Provider_item Loading
            if (itemList.Count > 0)
            {
                for (int i = 0; i < (itemList.Count() / loadUnit) || loadUnit > itemList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Provider_item> provider_itemFoundList = new ConcurrentBag<Provider_item>(new NotifyTaskCompletion<List<Provider_item>>(dalItem.GateWayItem.GetProvider_itemDataByItemListAsync(itemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetProvider_itemDataByItemList(new List<Item>(itemList.Skip(i * loadUnit).Take(loadUnit)));
                    provider_itemList = new ConcurrentBag<Provider_item>(provider_itemList.Concat(new ConcurrentBag<Provider_item>(provider_itemFoundList)).OrderBy(x => x.Provider_name).Distinct());
                }
                var savedProvider_itemList = new ConcurrentBag<Provider_item>(dalItem.LoadProvider_item(provider_itemList.ToList()));
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Provider Loading
            if (provider_itemList.Count > 0)
            {
                List<Provider> providerFoundList = new NotifyTaskCompletion<List<Provider>>(dalItem.GateWayItem.GetProviderDataByProvider_itemListAsync(provider_itemList.ToList())).Task.Result; // await dalItem.GateWayItem.GetProviderDataByProvider_itemList(new List<Provider_item>(provider_itemList.Skip(i * loadUnit).Take(loadUnit)));
                providerList = new ConcurrentBag<Provider>(providerList.Concat(new ConcurrentBag<Provider>(providerFoundList)).OrderBy(x => x.Name).Distinct());
                List<Provider> savedProviderList = dalItem.LoadProvider(providerList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Item_delivery Loading
            if (deliveryList.Count > 0)
            {
                List<Item_delivery> item_deliveryFoundList = new NotifyTaskCompletion<List<Item_delivery>>(dalItem.GateWayItem.GetItem_deliveryDataByDeliveryListAsync(deliveryList.ToList())).Task.Result; // await dalItem.GateWayItem.GetItem_deliveryDataByDeliveryList(new List<Delivery>(deliveryList.Skip(i * loadUnit).Take(loadUnit)));
                item_deliveryList = new ConcurrentBag<Item_delivery>(item_deliveryList.Concat(new ConcurrentBag<Item_delivery>(item_deliveryFoundList)));
                List<Item_delivery> savedItem_deliveryList = dalItem.LoadItem_delivery(item_deliveryList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Tax_item Loading
            if (itemList.Count > 0)
            {
                for (int i = 0; i < (itemList.Count() / loadUnit) || loadUnit > itemList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Tax_item> tax_itemFoundList = new ConcurrentBag<Tax_item>(new NotifyTaskCompletion<List<Tax_item>>(dalItem.GateWayItem.GetTax_itemDataByItemListAsync(itemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetTax_itemDataByItemList(new List<Item>(itemList.Skip(i * loadUnit).Take(loadUnit)));
                    tax_itemList = new ConcurrentBag<Tax_item>(tax_itemList.Concat(new ConcurrentBag<Tax_item>(tax_itemFoundList)));
                }
                var savedTax_itemList = new ConcurrentBag<Tax_item>(dalItem.LoadTax_item(tax_itemList.ToList()));

            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Client Loading
            if (billList.Count > 0)
            {
                List<Client> clientFoundList = new NotifyTaskCompletion<List<Client>>(dalClient.GateWayClient.GetClientDataByBillListAsync(billList.ToList())).Task.Result; // await dalClient.GateWayClient.GetClientDataByBillList(new List<Bill>(billList.Skip(i * loadUnit).Take(loadUnit)));
                clientList = new ConcurrentBag<Client>(clientList.Concat(new ConcurrentBag<Client>(clientFoundList)));
                List<Client> savedClientList = dalClient.LoadClient(clientList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Contacts Loading
            if (clientList.Count > 0)
            {
                List<Contact> contactFoundList = new NotifyTaskCompletion<List<Contact>>(dalClient.GateWayClient.GetContactDataByClientListAsync(clientList.ToList())).Task.Result; // await dalClient.GateWayClient.GetContactDataByClientList(new List<Client>(clientList.Skip(i * loadUnit).Take(loadUnit)));
                contactList = new ConcurrentBag<Contact>(contactList.Concat(new ConcurrentBag<Contact>(contactFoundList)));
                List<Contact> savedContactList = dalClient.LoadContact(contactList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // saving orders
            if (orderList.Count > 0)
            {
                var savedOrderList = LoadOrder(orderList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);
        }
    } /* end class BLOrdere */
}