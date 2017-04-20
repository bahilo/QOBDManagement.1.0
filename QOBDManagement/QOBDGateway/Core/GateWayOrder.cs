using Microsoft.Win32;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.REMOTE;
using QOBDCommon.Structures;
using QOBDGateway.Classes;
using QOBDGateway.Helper.ChannelHelper;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
/// <summary>
///  this class allows managing the orders web services
///  
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDGateway.Core
{
    public class GateWayOrder : IOrderManager, INotifyPropertyChanged
    {
        private ClientProxy _channel;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public GateWayOrder(ClientProxy servicePort)
        {
            _channel = servicePort;
        }

        public void setServiceCredential(object channel)
        {
            _channel = (ClientProxy)channel;
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task<List<Order>> InsertOrderAsync(List<Order> orderList)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = (await _channel.insert_data_orderAsync(orderList.OrderTypeToArray())).ArrayTypeToOrder();
            }
            catch (FaultException) {  Dispose(); throw; }
            catch (CommunicationException ) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_order>> InsertTax_orderAsync(List<Tax_order> tax_orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.insert_data_tax_orderAsync(tax_orderList.Tax_orderTypeToArray())).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> InsertOrder_itemAsync(List<Order_item> order_itemList)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.insert_data_order_itemAsync(order_itemList.order_itemTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax>> InsertTaxAsync(List<Tax> taxList)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = (await _channel.insert_data_taxAsync(taxList.TaxTypeToArray())).ArrayTypeToTax();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> InsertBillAsync(List<Bill> billList)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.insert_data_billAsync(billList.BillTypeToArray())).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> InsertDeliveryAsync(List<Delivery> deliveryList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.insert_data_deliveryAsync(deliveryList.DeliveryTypeToArray())).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Order>> DeleteOrderAsync(List<Order> orderList)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = (await _channel.delete_data_orderAsync(orderList.OrderTypeToArray())).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_order>> DeleteTax_orderAsync(List<Tax_order> tax_order)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.delete_data_tax_orderAsync(tax_order.Tax_orderTypeToArray())).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> DeleteOrder_itemAsync(List<Order_item> order_itemList)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.delete_data_order_itemAsync(order_itemList.order_itemTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax>> DeleteTaxAsync(List<Tax> taxList)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = (await _channel.delete_data_taxAsync(taxList.TaxTypeToArray())).ArrayTypeToTax();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> DeleteBillAsync(List<Bill> billList)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.delete_data_billAsync(billList.BillTypeToArray())).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> DeleteDeliveryAsync(List<Delivery> deliveryList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.delete_data_deliveryAsync(deliveryList.DeliveryTypeToArray())).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order>> UpdateOrderAsync(List<Order> orderList)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = (await _channel.update_data_orderAsync(orderList.OrderTypeToArray())).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_order>> UpdateTax_orderAsync(List<Tax_order> tax_orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.update_data_tax_orderAsync(tax_orderList.Tax_orderTypeToArray())).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> UpdateOrder_itemAsync(List<Order_item> order_itemList)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.update_data_order_itemAsync(order_itemList.order_itemTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax>> UpdateTaxAsync(List<Tax> taxList)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = (await _channel.update_data_taxAsync(taxList.TaxTypeToArray())).ArrayTypeToTax();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> UpdateBillAsync(List<Bill> listBill)
        {
            var formatListBillToArray = ServiceHelper.BillTypeToArray(listBill);
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.update_data_billAsync(formatListBillToArray)).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> UpdateDeliveryAsync(List<Delivery> listDelivery)
        {
            var formatListDeliveryToArray = ServiceHelper.DeliveryTypeToArray(listDelivery);
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.update_data_deliveryAsync(formatListDeliveryToArray)).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Order>> GetOrderDataAsync(int nbLine)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = (await _channel.get_data_orderAsync(nbLine.ToString())).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order>> GetOrderDataByIdAsync(int id)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = (await _channel.get_data_order_by_idAsync(id.ToString())).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Tax_order>> GetTax_orderDataAsync(int nbLine)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.get_data_tax_orderAsync(nbLine.ToString())).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_order>> GetTax_orderDataByOrderListAsync(List<Order> orderList)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.get_data_tax_order_by_order_listAsync(orderList.OrderTypeToArray())).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_order>> GetTax_orderDataByIdAsync(int id)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.get_data_tax_order_by_idAsync(id.ToString())).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemDataAsync(int nbLine)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.get_data_order_itemAsync(nbLine.ToString())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemDataByIdAsync(int id)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.get_data_order_item_by_idAsync(id.ToString())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> GetOrder_itemByOrderListAsync(List<Order> orderList)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.get_data_order_item_by_order_listAsync(orderList.OrderTypeToArray())).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax>> GetTaxDataAsync(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = (await _channel.get_data_taxAsync(nbLine.ToString())).ArrayTypeToTax();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax>> GetTaxDataByIdAsync(int id)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = (await _channel.get_data_tax_by_idAsync(id.ToString())).ArrayTypeToTax();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> GetBillDataAsync(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.get_data_billAsync(nbLine.ToString())).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> GetBillDataByOrderListAsync(List<Order> orderList)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.get_data_bill_by_order_listAsync(orderList.OrderTypeToArray())).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> GetBillDataByIdAsync(int id)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.get_data_bill_by_idAsync(id.ToString())).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<Bill> GetLastBillAsync()
        {
            List<Bill> result = new List<Bill>();
            result = await GetBillDataAsync(1);
            if (result.Count > 0)
                return result[0];

            return null;
        }

        public async Task<List<Bill>> GetUnpaidBillDataByAgentAsync(int agentId)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.get_data_bill_by_unpaidAsync(agentId.ToString())).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataAsync(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.get_data_deliveryAsync(nbLine.ToString())).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataByOrderListAsync(List<Order> orderList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.get_data_delivery_by_order_listAsync(orderList.OrderTypeToArray())).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataByIdAsync(int id)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.get_data_delivery_by_idAsync(id.ToString())).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }


        public async Task<List<Order>> searchOrderAsync(Order order, ESearchOption filterOperator)
        {
            List<Order> result = new List<Order>();
            try
            {
                result = (await _channel.get_filter_orderAsync(order.OrderTypeToFilterArray(filterOperator))).ArrayTypeToOrder();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax_order>> searchTax_orderAsync(Tax_order Tax_order, ESearchOption filterOperator)
        {
            List<Tax_order> result = new List<Tax_order>();
            try
            {
                result = (await _channel.get_filter_tax_orderAsync(Tax_order.Tax_orderTypeToFilterArray(filterOperator))).ArrayTypeToTax_order();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Order_item>> searchOrder_itemAsync(Order_item order_item, ESearchOption filterOperator)
        {
            List<Order_item> result = new List<Order_item>();
            try
            {
                result = (await _channel.get_filter_order_itemAsync(order_item.Order_itemTypeToFilterArray(filterOperator))).ArrayTypeToOrder_item();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Tax>> searchTaxAsync(Tax Tax, ESearchOption filterOperator)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = (await _channel.get_filter_taxAsync(Tax.TaxTypeToFilterArray(filterOperator))).ArrayTypeToTax();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Bill>> searchBillAsync(Bill Bill, ESearchOption filterOperator)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = (await _channel.get_filter_billAsync(Bill.BillTypeToFilterArray(filterOperator))).ArrayTypeToBill();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public async Task<List<Delivery>> searchDeliveryAsync(Delivery Delivery, ESearchOption filterOperator)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.get_filter_deliveryAsync(Delivery.DeliveryTypeToFilterArray(filterOperator))).ArrayTypeToDelivery();
            }
            catch (FaultException) { Dispose(); throw; }
            catch (CommunicationException) { _channel.Abort(); throw; }
            catch (TimeoutException) { _channel.Abort(); }
            return result;
        }

        public void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf)
        {
            WebClient client = new WebClient();
            try
            {
                client.Credentials = new NetworkCredential(_channel.ClientCredentials.UserName.UserName, _channel.ClientCredentials.UserName.Password);
                var uri = ConfigurationManager.AppSettings["remote_host"] + ConfigurationManager.AppSettings["remote_doc_lib_pdf_folder"] + "bin/BL_Codsimex.php?";
                uri += "num_dev=" + paramDeliveryToPdf.OrderId;
                uri += "&num_bl=" + paramDeliveryToPdf.DeliveryId;
                uri += "&lang=" + paramDeliveryToPdf.Lang;

                System.Diagnostics.Process.Start(uri);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.ORDER);
            }
            finally
            {
                client.Dispose();
            }
        }

        public void GeneratePdfOrder(ParamOrderToPdf paramOrderToPdf)
        {
            WebClient client = new WebClient();
            try
            {
                client.Credentials = new NetworkCredential(_channel.ClientCredentials.UserName.UserName, _channel.ClientCredentials.UserName.Password);
                string uri = ConfigurationManager.AppSettings["remote_host"] + ConfigurationManager.AppSettings["remote_doc_lib_pdf_folder"] +  "bin/Facture_Codsimex.php?";

                uri += "num_dev=" + paramOrderToPdf.OrderId;
                uri += "&num_fact=" + paramOrderToPdf.BillId;
                uri += "&currency=" + paramOrderToPdf.Currency;
                uri += "&lang=" + paramOrderToPdf.Lang;

                if (paramOrderToPdf.ParamEmail.IsSendEmail)
                {
                    uri += "&remind=" + paramOrderToPdf.ParamEmail.Reminder;
                    uri += "&mail=1";
                    uri += "&subject=" + paramOrderToPdf.ParamEmail.Subject;
                    if (paramOrderToPdf.ParamEmail.IsCopyToAgent)
                        uri += "&copyagent=1";
                }

                if (paramOrderToPdf.IsOrderConstructorReferencesVisible)
                    uri += "&refv=" + paramOrderToPdf.IsOrderConstructorReferencesVisible;

                System.Diagnostics.Process.Start(uri);
            }
            finally
            {
                client.Dispose();
            }
        }

        public void GeneratePdfQuote(ParamOrderToPdf paramOrderToPdf)
        {
            WebClient client = new WebClient();
            try
            {
                client.Credentials = new NetworkCredential(_channel.ClientCredentials.UserName.UserName, _channel.ClientCredentials.UserName.Password);
                string uri = ConfigurationManager.AppSettings["remote_host"] + ConfigurationManager.AppSettings["remote_doc_lib_pdf_folder"] + "bin/Devis_Codsimex.php?";

                uri += "num_dev=" + paramOrderToPdf.OrderId;
                uri += "&delay=" + paramOrderToPdf.ValidityDay;
                uri += "&quote=" + paramOrderToPdf.TypeQuoteOrProformat.ToString();
                uri += "&currency=" + paramOrderToPdf.Currency;
                uri += "&lang=" + paramOrderToPdf.Lang;

                if (paramOrderToPdf.ParamEmail.IsSendEmail)
                {
                    uri += "&mail=1";
                    uri += "&subject=" + paramOrderToPdf.ParamEmail.Subject;
                }

                if (paramOrderToPdf.IsQuoteConstructorReferencesVisible)
                    uri += "&refv=" + paramOrderToPdf.IsQuoteConstructorReferencesVisible;

                System.Diagnostics.Process.Start(uri);
            }
            finally
            {
                client.Dispose();
            }
        }


        public void Dispose()
        {
            if (_channel.State == CommunicationState.Opened)
                _channel.Close();
        }
    } /* end class BLOrdere */
}