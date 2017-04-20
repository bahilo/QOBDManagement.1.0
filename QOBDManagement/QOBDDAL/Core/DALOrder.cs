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
using QOBDDAL.Classes;
using QOBDDAL.Interfaces;
using QOBDGateway.Classes;
using QOBDGateway.Interfaces;

namespace QOBDDAL.Core
{
    public class DALOrder : IOrderManager
    {
        private Func<double, double> _progressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IOrderManager _gatewayOrder;
        private ClientProxy _servicePortType;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALOrder(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gatewayOrder = new GateWayOrder(_servicePortType);
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public DALOrder(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet) : this(servicePort)
        {
            this._dataSet = _dataSet;
        }

        public DALOrder(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gatewayOrder.setServiceCredential(_servicePortType);
            }
        }

        public async void cacheWebServiceData()
        {
            await retrieveGateWayOrderDataAsync();
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gatewayOrder.setServiceCredential(_servicePortType);
        }

        private async Task retrieveGateWayOrderDataAsync()
        {
            try
            {
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                await UpdateOrderDependenciesAsync((await _gatewayOrder.searchOrderAsync(new Order { AgentId = AuthenticatedUser.ID }, ESearchOption.AND)).Take(_loadSize).ToList(), true);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.ORDER); }
            finally { lock (_lock) IsLodingDataFromWebServiceToLocal = true; }

        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }

        private void checkServiceCommunication()
        {
            if (_servicePortType.State == System.ServiceModel.CommunicationState.Closed || _servicePortType.State == System.ServiceModel.CommunicationState.Faulted)
                _serviceCommunication.resetCommunication();
        }

        #region [ Actions ]
        //=================================[ Actions ]================================================

        public async Task<List<Order>> InsertOrderAsync(List<Order> listOrder)
        {
            checkServiceCommunication();
            List<Order> gateWayResultList = await _gatewayOrder.InsertOrderAsync(listOrder);
            return LoadOrder(gateWayResultList);
        }

        public async Task<List<Tax_order>> InsertTax_orderAsync(List<Tax_order> listTax_order)
        {
            checkServiceCommunication();
            List<Tax_order> gateWayResultList = await _gatewayOrder.InsertTax_orderAsync(listTax_order);
            return LoadTax_order(gateWayResultList);
        }

        public async Task<List<Order_item>> InsertOrder_itemAsync(List<Order_item> listOrder_item)
        {
            checkServiceCommunication();
            List<Order_item> gateWayResultList = await _gatewayOrder.InsertOrder_itemAsync(listOrder_item);
            return LoadOrder_item(gateWayResultList);
        }

        public async Task<List<Tax>> InsertTaxAsync(List<Tax> listTax)
        {
            checkServiceCommunication();
            List<Tax> gateWayResultList = await _gatewayOrder.InsertTaxAsync(listTax);
            return LoadTax(gateWayResultList);
        }

        public async Task<List<Bill>> InsertBillAsync(List<Bill> listBill)
        {
            checkServiceCommunication();
            List<Bill> gateWayResultList = await _gatewayOrder.InsertBillAsync(listBill);
            return LoadBill(gateWayResultList);
        }

        public async Task<List<Delivery>> InsertDeliveryAsync(List<Delivery> listDelivery)
        {
            checkServiceCommunication();
            List<Delivery> gateWayResultList = await _gatewayOrder.InsertDeliveryAsync(listDelivery);
            return LoadDelivery(gateWayResultList);
        }


        public async Task<List<Order>> DeleteOrderAsync(List<Order> listOrder)
        {
            List<Order> result = new List<Order>();
            checkServiceCommunication();
            List<Order> gateWayResultList = await _gatewayOrder.DeleteOrderAsync(listOrder);
            if (gateWayResultList.Count == 0)
                foreach (Order order in listOrder)
                {
                    int returnValue = _dataSet.DeleteOrder(order.ID);
                    if (returnValue == 0)
                        result.Add(order);
                }
            return result;
        }

        public async Task<List<Tax_order>> DeleteTax_orderAsync(List<Tax_order> listTax_order)
        {
            List<Tax_order> result = new List<Tax_order>();
            checkServiceCommunication();
            List<Tax_order> gateWayResultList = await _gatewayOrder.DeleteTax_orderAsync(listTax_order);
            if (gateWayResultList.Count == 0)
                foreach (Tax_order tax_order in listTax_order)
                {
                    int returnValue = _dataSet.DeleteTax_order(tax_order.ID);
                    if (returnValue == 0)
                        result.Add(tax_order);
                }
            return result;
        }

        public async Task<List<Order_item>> DeleteOrder_itemAsync(List<Order_item> listOrder_item)
        {
            List<Order_item> result = new List<Order_item>();
            checkServiceCommunication();
            List<Order_item> gateWayResultList = await _gatewayOrder.DeleteOrder_itemAsync(listOrder_item);
            if (gateWayResultList.Count == 0)
                foreach (Order_item order_item in listOrder_item)
                {
                    int returnValue = _dataSet.DeleteOrder_item(order_item.ID);
                    if (returnValue == 0)
                        result.Add(order_item);
                }
            return result;
        }

        public async Task<List<Tax>> DeleteTaxAsync(List<Tax> listTax)
        {
            List<Tax> result = new List<Tax>();
            checkServiceCommunication();
            List<Tax> gateWayResultList = await _gatewayOrder.DeleteTaxAsync(listTax);
            if (gateWayResultList.Count == 0)
                foreach (Tax tax in listTax)
                {
                    int returnValue = _dataSet.DeleteTax(tax.ID);
                    if (returnValue == 0)
                        result.Add(tax);
                }
            return result;
        }

        public async Task<List<Bill>> DeleteBillAsync(List<Bill> listBill)
        {
            List<Bill> result = new List<Bill>();
            checkServiceCommunication();
            List<Bill> gateWayResultList = await _gatewayOrder.DeleteBillAsync(listBill);
            if (gateWayResultList.Count == 0)
                foreach (Bill bill in listBill)
                {
                    int returnValue = _dataSet.DeleteBill(bill.ID);
                    if (returnValue == 0)
                        result.Add(bill);
                }
            return result;
        }

        public async Task<List<Delivery>> DeleteDeliveryAsync(List<Delivery> listDelivery)
        {
            List<Delivery> result = new List<Delivery>();
            checkServiceCommunication();
            List<Delivery> gateWayResultList = await _gatewayOrder.DeleteDeliveryAsync(listDelivery);
            if (gateWayResultList.Count == 0)
                foreach (Delivery delivery in listDelivery)
                {
                    int returnValue = _dataSet.DeleteDelivery(delivery.ID);
                    if (returnValue == 0)
                        result.Add(delivery);
                }
            return result;
        }

        public async Task<List<Order>> UpdateOrderAsync(List<Order> ordersList)
        {
            List<Order> result = new List<Order>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Order> gateWayResultList = await _gatewayOrder.UpdateOrderAsync(ordersList);

            foreach (var order in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillOrderDataTableById(dataSetLocal.commands, order.ID);
                dataSet.commands.Merge(dataSetLocal.commands);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateOrder(gateWayResultList.OrderTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Order> LoadOrder(List<Order> ordersList)
        {
            List<Order> result = new List<Order>();
            foreach (var order in ordersList)
            {
                int returnResult = _dataSet.LoadOrder(order);
                if (returnResult > 0)
                    result.Add(order);
            }
            return result;
        }

        public async Task<List<Tax_order>> UpdateTax_orderAsync(List<Tax_order> tax_orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Tax_order> gateWayResultList = await _gatewayOrder.UpdateTax_orderAsync(tax_orderList);
            foreach (var tax_order in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillTax_orderDataTableById(dataSetLocal.tax_commands, tax_order.ID);
                dataSet.tax_commands.Merge(dataSetLocal.tax_commands);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateTax_order(gateWayResultList.Tax_orderTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Tax_order> LoadTax_order(List<Tax_order> tax_ordersList)
        {
            List<Tax_order> result = new List<Tax_order>();
            foreach (var tax_order in tax_ordersList)
            {
                int returnResult = _dataSet.LoadTax_order(tax_order);
                if (returnResult > 0)
                    result.Add(tax_order);
            }
            return result;
        }

        public async Task<List<Order_item>> UpdateOrder_itemAsync(List<Order_item> order_itemList)
        {
            List<Order_item> result = new List<Order_item>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Order_item> gateWayResultList = await _gatewayOrder.UpdateOrder_itemAsync(order_itemList);
            foreach (var order_item in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillOrder_itemDataTableById(dataSetLocal.command_items, order_item.ID);
                dataSet.command_items.Merge(dataSetLocal.command_items);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateOrder_item(gateWayResultList.Order_itemTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Order_item> LoadOrder_item(List<Order_item> order_itemsList)
        {
            List<Order_item> result = new List<Order_item>();
            foreach (var order_item in order_itemsList)
            {
                int returnResult = _dataSet.LoadOrder_item(order_item);
                if (returnResult > 0)
                    result.Add(order_item);
            }
            return result;
        }

        public async Task<List<Tax>> UpdateTaxAsync(List<Tax> taxesList)
        {
            List<Tax> result = new List<Tax>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Tax> gateWayResultList = await _gatewayOrder.UpdateTaxAsync(taxesList);
            foreach (var tax in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillTaxDataTableById(dataSetLocal.taxes, tax.ID);
                dataSet.taxes.Merge(dataSetLocal.taxes);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateTax(gateWayResultList.TaxTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Tax> LoadTax(List<Tax> taxesList)
        {
            List<Tax> result = new List<Tax>();
            foreach (var tax in taxesList)
            {
                int returnResult = _dataSet.LoadTax(tax);
                if (returnResult > 0)
                    result.Add(tax);
            }
            return result;
        }

        public async Task<List<Bill>> UpdateBillAsync(List<Bill> billList)
        {
            List<Bill> result = new List<Bill>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Bill> gateWayResultList = await _gatewayOrder.UpdateBillAsync(billList);

            foreach (var bill in gateWayResultList)
            {
                QOBDSet dataSetLocal = new QOBDSet();
                _dataSet.FillBillDataTableById(dataSetLocal.bills, bill.ID);
                dataSet.bills.Merge(dataSetLocal.bills);
            }

            if (gateWayResultList.Count > 0)
            {
                int returnValue = _dataSet.UpdateBill(gateWayResultList.BillTypeToDataTable(dataSet));
                if (returnValue == gateWayResultList.Count)
                    result = gateWayResultList;
            }
            return result;
        }

        public List<Bill> LoadBill(List<Bill> billList)
        {
            List<Bill> result = new List<Bill>();
            foreach (var bill in billList)
            {
                int returnResult = _dataSet.LoadBill(bill);
                if (returnResult > 0)
                    result.Add(bill);
            }
            return result;
        }

        public async Task<List<Delivery>> UpdateDeliveryAsync(List<Delivery> deliveryList)
        {
            List<Delivery> result = new List<Delivery>();
            QOBDSet dataSet = new QOBDSet();
            checkServiceCommunication();
            List<Delivery> gateWayResultList = await _gatewayOrder.UpdateDeliveryAsync(deliveryList);
            foreach (var delivery in gateWayResultList)
            {
                int returnValue = _dataSet.UpdateDelivery(delivery);
                if (returnValue > 0)
                    result.Add(delivery);
            }
            return result;
        }

        public List<Delivery> LoadDelivery(List<Delivery> deliveryList)
        {
            List<Delivery> result = new List<Delivery>();
            foreach (var delivery in deliveryList)
            {
                int returnResult = _dataSet.LoadDelivery(delivery);
                if (returnResult > 0)
                    result.Add(delivery);
            }
            return result;
        }


        public List<Order> GetOrderData(int nbLine)
        {
            List<Order> result = _dataSet.GetOrderData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }
        
        public async Task<List<Order>> GetOrderDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gatewayOrder.GetOrderDataAsync(nbLine);
        }

        public List<Order> GetOrderDataById(int id)
        {
            return _dataSet.GetOrderDataById(id);
        }


        public List<Tax_order> GetTax_orderData(int nbLine)
        {
            List<Tax_order> result = _dataSet.GetTax_orderData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }


        public async Task<List<Tax_order>> GetTax_orderDataAsync(int nbLine)
        {
            checkServiceCommunication();
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
            checkServiceCommunication();
            return await _gatewayOrder.GetTax_orderDataByOrderListAsync(orderList);
        }

        public List<Tax_order> GetTax_orderByOrderId(int orderId)
        {
            return _dataSet.GetTax_orderByOrderId(orderId);
        }

        public List<Tax_order> GetTax_orderDataById(int id)
        {
            return _dataSet.GetTax_orderDataById(id);
        }

        public List<Order_item> GetOrder_itemData(int nbLine)
        {
            List<Order_item> result = _dataSet.GetOrder_itemData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Order_item>> GetOrder_itemDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gatewayOrder.GetOrder_itemDataAsync(nbLine);
        }

        public async Task<List<Order_item>> GetOrder_itemByOrderListAsync(List<Order> ordersList)
        {
            checkServiceCommunication();
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
            return _dataSet.GetOrder_itemDataById(id);
        }

        public List<Order_item> GetOrder_itemDataByOrderId(int orderId)
        {
            return _dataSet.GetOrder_itemDataByOrderId(orderId);
        }

        public List<Tax> GetTaxData(int nbLine)
        {
            List<Tax> result = _dataSet.GetTaxData();

            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax>> GetTaxDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gatewayOrder.GetTaxDataAsync(nbLine);
        }

        public List<Tax> GetTaxDataById(int id)
        {
            return _dataSet.GetTaxDataById(id);
        }

        public List<Bill> GetBillData(int nbLine)
        {
            List<Bill> result = _dataSet.GetBillData();

            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Bill>> GetUnpaidBillDataByAgentAsync(int agentId)
        {
            checkServiceCommunication();
            return await _gatewayOrder.GetUnpaidBillDataByAgentAsync(agentId);
        }

        public async Task<List<Bill>> GetBillDataAsync(int nbLine)
        {
            checkServiceCommunication();
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
            checkServiceCommunication();
            return await _gatewayOrder.GetBillDataByOrderListAsync(orderList);
        }

        public List<Bill> GetBillDataByOrderId(int orderId)
        {
            return _dataSet.GetBillDataByOrderId(orderId);
        }

        public List<Bill> GetBillDataById(int id)
        {
            return _dataSet.GetBillDataById(id);
        }

        public async Task<Bill> GetLastBillAsync()
        {
            checkServiceCommunication();
            return await _gatewayOrder.GetLastBillAsync();
        }

        public List<Delivery> GetDeliveryData(int nbLine)
        {
            List<Delivery> result = _dataSet.GetDeliveryData();
            if (nbLine.Equals(999) || result.Count == 0|| result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Delivery>> GetDeliveryDataAsync(int nbLine)
        {
            checkServiceCommunication();
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
            checkServiceCommunication();
            return await _gatewayOrder.GetDeliveryDataByOrderListAsync(orderList);
        }

        public List<Delivery> GetDeliveryDataByOrderId(int orderId)
        {
            return _dataSet.GetDeliveryDataByOrderId(orderId);
        }

        public List<Delivery> GetDeliveryDataById(int id)
        {
            return _dataSet.GetDeliveryDataById(id);
        }

        public List<Order> searchOrder(Order order, ESearchOption filterOperator)
        {
            return _dataSet.searchOrder(order, filterOperator);
        }

        public async Task<List<Order>> searchOrderAsync(Order order, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gatewayOrder.searchOrderAsync(order, filterOperator);
        }

        public List<Tax_order> searchTax_order(Tax_order Tax_order, ESearchOption filterOperator)
        {
            return _dataSet.searchTax_order(Tax_order, filterOperator);
        }

        public async Task<List<Tax_order>> searchTax_orderAsync(Tax_order Tax_order, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gatewayOrder.searchTax_orderAsync(Tax_order, filterOperator);
        }

        public List<Order_item> searchOrder_item(Order_item order_item, ESearchOption filterOperator)
        {
            return _dataSet.searchOrder_item(order_item, filterOperator);
        }

        public async Task<List<Order_item>> searchOrder_itemAsync(Order_item order_item, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gatewayOrder.searchOrder_itemAsync(order_item, filterOperator);
        }

        public List<Tax> searchTax(Tax Tax, ESearchOption filterOperator)
        {
            return _dataSet.searchTax(Tax, filterOperator);
        }

        public async Task<List<Tax>> searchTaxAsync(Tax Tax, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gatewayOrder.searchTaxAsync(Tax, filterOperator);
        }

        public List<Bill> searchBill(Bill Bill, ESearchOption filterOperator)
        {
            return _dataSet.searchBill(Bill, filterOperator);
        }

        public async Task<List<Bill>> searchBillAsync(Bill Bill, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gatewayOrder.searchBillAsync(Bill, filterOperator);
        }

        public List<Delivery> searchDelivery(Delivery Delivery, ESearchOption filterOperator)
        {
            return _dataSet.searchDelivery(Delivery, filterOperator);
        }

        public async Task<List<Delivery>> searchDeliveryAsync(Delivery Delivery, ESearchOption filterOperator)
        {
            checkServiceCommunication();
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
        #endregion

        public void Dispose()
        {
            if (_gatewayOrder != null)
                _gatewayOrder.Dispose();
        }


        //----------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------//

        public async Task UpdateOrderDependenciesAsync(List<Order> orders, bool isActiveProgress = false)
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

            DALItem dalItem = new DALItem(_servicePortType, _dataSet);
            dalItem.AuthenticatedUser = AuthenticatedUser;
            dalItem.GateWayItem.setServiceCredential(_servicePortType);
            dalItem.IsLodingDataFromWebServiceToLocal = true;

            DALClient dalClient = new DALClient(_servicePortType, _dataSet);
            dalClient.AuthenticatedUser = AuthenticatedUser;
            dalClient.setServiceCredential(_servicePortType);
            dalClient.IsLodingDataFromWebServiceToLocal = true;

            orderList = new ConcurrentBag<Order>(orders);

            // Address loading
            if (orderList.Count > 0)
            {
                checkServiceCommunication();
                var addressfoundList = await dalClient.GateWayClient.GetAddressDataByOrderListAsync(orderList.ToList());
                addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressfoundList)));
                var savedAddressList = dalClient.LoadAddress(addressList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Tax_order Loading
            if (orderList.Count > 0)
            {
                checkServiceCommunication();
                var tax_orderfoundList = await _gatewayOrder.GetTax_orderDataByOrderListAsync(orderList.ToList());
                tax_orderList = new ConcurrentBag<Tax_order>(tax_orderList.Concat(new ConcurrentBag<Tax_order>(tax_orderfoundList)));
                List<Tax_order> savedTax_orderList = LoadTax_order(tax_orderList.ToList());
            }

            // Tax Loading
            var taxFoundList = await _gatewayOrder.GetTaxDataAsync(999); 
            taxList = new ConcurrentBag<Tax>(new ConcurrentBag<Tax>(taxFoundList));
            List<Tax> savedTaxList = LoadTax(taxList.ToList());
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Bills Loading
            if (orderList.Count > 0)
            {
                checkServiceCommunication();
                List<Bill> billfoundList = await _gatewayOrder.GetBillDataByOrderListAsync(orderList.ToList());
                billList = new ConcurrentBag<Bill>(billList.Concat(new ConcurrentBag<Bill>(billfoundList)));
                List<Bill> savedBillList = LoadBill(billList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Delivery Loading
            if (orderList.Count > 0)
            {
                checkServiceCommunication();
                List<Delivery> deliveryfoundList = await _gatewayOrder.GetDeliveryDataByOrderListAsync(orderList.ToList());
                deliveryList = new ConcurrentBag<Delivery>(deliveryList.Concat(new ConcurrentBag<Delivery>(deliveryfoundList)));
                List<Delivery> savedDeliveryList = LoadDelivery(deliveryList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Order_item Loading
            if (orderList.Count > 0)
            {
                checkServiceCommunication();
                for (int i = 0; i < (orderList.Count() / loadUnit) || loadUnit > orderList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Order_item> order_itemFoundList = new ConcurrentBag<Order_item>(await _gatewayOrder.GetOrder_itemByOrderListAsync(orderList.Skip(i * loadUnit).Take(loadUnit).ToList()));
                    order_itemList = new ConcurrentBag<Order_item>(order_itemList.Concat(new ConcurrentBag<Order_item>(order_itemFoundList)));
                }
                var savedOrder_itemList = new ConcurrentBag<Order_item>(LoadOrder_item(order_itemList.ToList()));
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Item Loading
            if (order_itemList.Count > 0)
            {
                checkServiceCommunication();
                for (int i = 0; i < (order_itemList.Count() / loadUnit) || loadUnit > order_itemList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Item> itemFoundList = new ConcurrentBag<Item>(await dalItem.GateWayItem.GetItemDataByOrder_itemListAsync(order_itemList.Skip(i * loadUnit).Take(loadUnit).ToList()));
                    itemList = new ConcurrentBag<Item>(itemList.Concat(new ConcurrentBag<Item>(itemFoundList)));
                }
                var savedItemList = new ConcurrentBag<Item>(dalItem.LoadItem(itemList.ToList()));
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Provider_item Loading
            if (itemList.Count > 0)
            {
                checkServiceCommunication();
                for (int i = 0; i < (itemList.Count() / loadUnit) || loadUnit > itemList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Provider_item> provider_itemFoundList = new ConcurrentBag<Provider_item>(await dalItem.GateWayItem.GetProvider_itemDataByItemListAsync(itemList.Skip(i * loadUnit).Take(loadUnit).ToList()));
                    provider_itemList = new ConcurrentBag<Provider_item>(provider_itemList.Concat(new ConcurrentBag<Provider_item>(provider_itemFoundList)).OrderBy(x => x.Provider_name).Distinct());
                }
                var savedProvider_itemList = new ConcurrentBag<Provider_item>(dalItem.LoadProvider_item(provider_itemList.ToList()));
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Provider Loading
            if (provider_itemList.Count > 0)
            {
                checkServiceCommunication();
                List<Provider> providerFoundList = await dalItem.GateWayItem.GetProviderDataByProvider_itemListAsync(provider_itemList.ToList());
                providerList = new ConcurrentBag<Provider>(providerList.Concat(new ConcurrentBag<Provider>(providerFoundList)).OrderBy(x => x.Name).Distinct());
                List<Provider> savedProviderList = dalItem.LoadProvider(providerList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);


            // Item_delivery Loading
            if (deliveryList.Count > 0)
            {
                checkServiceCommunication();
                List<Item_delivery> item_deliveryFoundList = await dalItem.GateWayItem.GetItem_deliveryDataByDeliveryListAsync(deliveryList.ToList());
                item_deliveryList = new ConcurrentBag<Item_delivery>(item_deliveryList.Concat(new ConcurrentBag<Item_delivery>(item_deliveryFoundList)));
                List<Item_delivery> savedItem_deliveryList = dalItem.LoadItem_delivery(item_deliveryList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Tax_item Loading
            if (itemList.Count > 0)
            {
                checkServiceCommunication();
                for (int i = 0; i < (itemList.Count() / loadUnit) || loadUnit > itemList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Tax_item> tax_itemFoundList = new ConcurrentBag<Tax_item>(await dalItem.GateWayItem.GetTax_itemDataByItemListAsync(itemList.Skip(i * loadUnit).Take(loadUnit).ToList())); // await dalItem.GateWayItem.GetTax_itemDataByItemList(new List<Item>(itemList.Skip(i * loadUnit).Take(loadUnit)));
                    tax_itemList = new ConcurrentBag<Tax_item>(tax_itemList.Concat(new ConcurrentBag<Tax_item>(tax_itemFoundList)));
                }
                var savedTax_itemList = new ConcurrentBag<Tax_item>(dalItem.LoadTax_item(tax_itemList.ToList()));

            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);
            
            // Client Loading
            if (orderList.Count > 0)
            {
                checkServiceCommunication();
                for (int i = 0; i < (orderList.Count() / loadUnit) || loadUnit > orderList.Count() && i == 0; i++)
                {
                    ConcurrentBag<Client> clientFoundList = new ConcurrentBag<Client>(await dalClient.GateWayClient.GetClientDataByOrderListAsync(orderList.Skip(i * loadUnit).Take(loadUnit).ToList()));
                    clientList = new ConcurrentBag<Client>(clientList.Concat(new ConcurrentBag<Client>(clientFoundList)));
                }
                List<Client> savedClientList = dalClient.LoadClient(clientList.ToList());
            }
            if (isActiveProgress) _progressBarFunc(_progressBarFunc(0) + step);

            // Contacts Loading
            if (clientList.Count > 0)
            {
                checkServiceCommunication();
                List<Contact> contactFoundList = await dalClient.GateWayClient.GetContactDataByClientListAsync(clientList.ToList()); // await dalClient.GateWayClient.GetContactDataByClientList(new List<Client>(clientList.Skip(i * loadUnit).Take(loadUnit)));
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