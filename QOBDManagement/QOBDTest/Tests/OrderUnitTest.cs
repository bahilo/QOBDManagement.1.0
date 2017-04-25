using Microsoft.VisualStudio.TestTools.UnitTesting;
using QOBDCommon.Entities;
using QOBDManagement;
using QOBDManagement.Interfaces;
using QOBDTest.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDTest.Tests
{
    [TestClass]
    public class OrderUnitTest
    {
        MockClient _mockProxy = new MockClient();
        IMainWindowViewModel _main;
        Order order;
        Order_item order_item;
        Bill bill;
        Tax tax;
        Tax_order tax_order;
        Delivery delivery;

        [TestInitialize]
        public void startup()
        {
            order = new Order { ID = 1, Tax = 20, ClientId = 1, AgentId = 1, Date = DateTime.Now, Status = "Order" };
            order_item = new Order_item { ID = 1, ItemId = 1, Item_ref = "item ref", OrderId = 1 };
            bill = new Bill { ID = 1, OrderId = 1, ClientId = 1, Date = DateTime.Now, DateLimit = DateTime.Now, DatePay = DateTime.Now };
            tax = new Tax { ID = 1, Type = "type", Date_insert = DateTime.Now, Tax_current = 1, Value = 20, Comment = "comment" };
            tax_order = new Tax_order { ID = 1, TaxId = 1, OrderId = 1, Date_insert = DateTime.Now, Tax_value = 20, Target = "" };
            delivery = new Delivery { ID = 1, BillId = 1, OrderId = 1, Date = DateTime.Now };
        }

        #region [ Order ]
        [TestMethod]
        public async Task insertOrders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedOrderList = await _main.OrderViewModel.Bl.BlOrder.InsertOrderAsync(new List<Order> { order });

            // Assert
            Assert.AreEqual(insertedOrderList.Count, 1);
        }

        [TestMethod]
        public async Task deleteOrders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var orderNotDeletedList = await _main.OrderViewModel.Bl.BlOrder.DeleteOrderAsync(new List<Order> { order });

            // Assert
            Assert.AreEqual(orderNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateOrders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var orderUpdatedList = await _main.OrderViewModel.Bl.BlOrder.UpdateOrderAsync(new List<Order> { order });

            // Assert
            Assert.AreEqual(orderUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoOrdersFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var orderUpdatedList = _main.OrderViewModel.Bl.BlOrder.GetOrderData(2);

            // Assert
            Assert.AreEqual(orderUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoOrdersFromWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var orderUpdatedList = await _main.OrderViewModel.Bl.BlOrder.GetOrderDataAsync(2);

            // Assert
            Assert.AreEqual(orderUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchOrderFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var orderfoundList = _main.OrderViewModel.Bl.BlOrder.searchOrder(order, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(orderfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchOrderFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var orderfoundList = await _main.OrderViewModel.Bl.BlOrder.searchOrderAsync(order, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(orderfoundList.Count, 1);
        }

        [TestMethod]
        public void loadOrders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.OrderViewModel.loadOrders();

            // Assert
            Assert.AreEqual(_main.OrderViewModel.OrderModelList.Count, 1);
        }

        [TestMethod]
        public void loadOrderOrder_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.OrderViewModel.OrderDetailViewModel.loadOrder_items();

            // Assert
            Assert.AreEqual(_main.OrderViewModel.OrderDetailViewModel.Order_ItemModelList.Count, 1);
        }

        [TestMethod]
        public void loadOrderDeliveryReceipts()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            _main.OrderViewModel.OrderDetailViewModel.loadInvoicesAndDeliveryReceipts();

            // Assert
            Assert.AreEqual(_main.OrderViewModel.OrderDetailViewModel.DeliveryModelList.Count, 1);
        }

        [TestMethod]
        public void selectOrderForDetailsDisplaying()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act             
            _main.OrderViewModel.saveSelectedOrder(new QOBDManagement.Models.OrderModel { Order = order });

            // Assert
            Assert.AreEqual(_main.OrderViewModel.SelectedOrderModel.Order.ID, 1);
        }
        #endregion

        #region [ Order_item ]
        [TestMethod]
        public async Task insertOrder_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedOrder_itemList = await _main.OrderViewModel.Bl.BlOrder.InsertOrder_itemAsync(new List<Order_item> { order_item });

            // Assert
            Assert.AreEqual(insertedOrder_itemList.Count, 1);
        }

        [TestMethod]
        public async Task deleteOrder_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var order_itemNotDeletedList = await _main.OrderViewModel.Bl.BlOrder.DeleteOrder_itemAsync(new List<Order_item> { order_item });

            // Assert
            Assert.AreEqual(order_itemNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateOrder_items()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var order_itemUpdatedList = await _main.OrderViewModel.Bl.BlOrder.UpdateOrder_itemAsync(new List<Order_item> { order_item });

            // Assert
            Assert.AreEqual(order_itemUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoOrder_itemsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var order_itemUpdatedList = _main.OrderViewModel.Bl.BlOrder.GetOrder_itemData(2);

            // Assert
            Assert.AreEqual(order_itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoOrder_itemsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var order_itemUpdatedList = await _main.OrderViewModel.Bl.BlOrder.GetOrder_itemDataAsync(2);

            // Assert
            Assert.AreEqual(order_itemUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchOrder_itemFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var order_itemfoundList = _main.OrderViewModel.Bl.BlOrder.searchOrder_item(order_item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(order_itemfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchOrder_itemFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var order_itemfoundList = await _main.OrderViewModel.Bl.BlOrder.searchOrder_itemAsync(order_item, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(order_itemfoundList.Count, 1);
        }
        #endregion

        #region [ Bill ]
        [TestMethod]
        public async Task insertBills()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedBillList = await _main.OrderViewModel.Bl.BlOrder.InsertBillAsync(new List<Bill> { bill });

            // Assert
            Assert.AreEqual(insertedBillList.Count, 1);
        }

        [TestMethod]
        public async Task deleteBills()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var billNotDeletedList = await _main.OrderViewModel.Bl.BlOrder.DeleteBillAsync(new List<Bill> { bill });

            // Assert
            Assert.AreEqual(billNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateBills()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var billUpdatedList = await _main.OrderViewModel.Bl.BlOrder.UpdateBillAsync(new List<Bill> { bill });

            // Assert
            Assert.AreEqual(billUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoBillsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var billUpdatedList = _main.OrderViewModel.Bl.BlOrder.GetBillData(2);

            // Assert
            Assert.AreEqual(billUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoBillsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var billUpdatedList = await _main.OrderViewModel.Bl.BlOrder.GetBillDataAsync(2);

            // Assert
            Assert.AreEqual(billUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchBillFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var billfoundList = _main.OrderViewModel.Bl.BlOrder.searchBill(bill, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(billfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchBillFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var billfoundList = await _main.OrderViewModel.Bl.BlOrder.searchBillAsync(bill, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(billfoundList.Count, 1);
        }
        #endregion

        #region [ Tax ]
        [TestMethod]
        public async Task insertTaxs()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedTaxList = await _main.OrderViewModel.Bl.BlOrder.InsertTaxAsync(new List<Tax> { tax });

            // Assert
            Assert.AreEqual(insertedTaxList.Count, 1);
        }

        [TestMethod]
        public async Task deleteTaxs()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var taxNotDeletedList = await _main.OrderViewModel.Bl.BlOrder.DeleteTaxAsync(new List<Tax> { tax });

            // Assert
            Assert.AreEqual(taxNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateTaxs()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var taxUpdatedList = await _main.OrderViewModel.Bl.BlOrder.UpdateTaxAsync(new List<Tax> { tax });

            // Assert
            Assert.AreEqual(taxUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoTaxsFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var taxUpdatedList = _main.OrderViewModel.Bl.BlOrder.GetTaxData(2);

            // Assert
            Assert.AreEqual(taxUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoTaxsFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var taxUpdatedList = await _main.OrderViewModel.Bl.BlOrder.GetTaxDataAsync(2);

            // Assert
            Assert.AreEqual(taxUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchTaxFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var taxfoundList = _main.OrderViewModel.Bl.BlOrder.searchTax(tax, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(taxfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchTaxFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var taxfoundList = await _main.OrderViewModel.Bl.BlOrder.searchTaxAsync(tax, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(taxfoundList.Count, 1);
        }
        #endregion

        #region [ Tax_order ]
        [TestMethod]
        public async Task insertTax_orders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedTax_orderList = await _main.OrderViewModel.Bl.BlOrder.InsertTax_orderAsync(new List<Tax_order> { tax_order });

            // Assert
            Assert.AreEqual(insertedTax_orderList.Count, 1);
        }

        [TestMethod]
        public async Task deleteTax_orders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_orderNotDeletedList = await _main.OrderViewModel.Bl.BlOrder.DeleteTax_orderAsync(new List<Tax_order> { tax_order });

            // Assert
            Assert.AreEqual(tax_orderNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateTax_orders()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_orderUpdatedList = await _main.OrderViewModel.Bl.BlOrder.UpdateTax_orderAsync(new List<Tax_order> { tax_order });

            // Assert
            Assert.AreEqual(tax_orderUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoTax_ordersFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_orderUpdatedList = _main.OrderViewModel.Bl.BlOrder.GetTax_orderData(2);

            // Assert
            Assert.AreEqual(tax_orderUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoTax_ordersFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_orderUpdatedList = await _main.OrderViewModel.Bl.BlOrder.GetTax_orderDataAsync(2);

            // Assert
            Assert.AreEqual(tax_orderUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchTax_orderFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_orderfoundList = _main.OrderViewModel.Bl.BlOrder.searchTax_order(tax_order, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(tax_orderfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchTax_orderFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var tax_orderfoundList = await _main.OrderViewModel.Bl.BlOrder.searchTax_orderAsync(tax_order, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(tax_orderfoundList.Count, 1);
        }
        #endregion

        #region [ Delivery ]
        [TestMethod]
        public async Task insertDeliverys()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var insertedDeliveryList = await _main.OrderViewModel.Bl.BlOrder.InsertDeliveryAsync(new List<Delivery> { delivery });

            // Assert
            Assert.AreEqual(insertedDeliveryList.Count, 1);
        }

        [TestMethod]
        public async Task deleteDeliverys()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var deliveryNotDeletedList = await _main.OrderViewModel.Bl.BlOrder.DeleteDeliveryAsync(new List<Delivery> { delivery });

            // Assert
            Assert.AreEqual(deliveryNotDeletedList.Count, 0);
        }

        [TestMethod]
        public async Task updateDeliverys()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var deliveryUpdatedList = await _main.OrderViewModel.Bl.BlOrder.UpdateDeliveryAsync(new List<Delivery> { delivery });

            // Assert
            Assert.AreEqual(deliveryUpdatedList.Count, 1);
        }

        [TestMethod]
        public void getTwoDeliverysFromLocalDataBase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var deliveryUpdatedList = _main.OrderViewModel.Bl.BlOrder.GetDeliveryData(2);

            // Assert
            Assert.AreEqual(deliveryUpdatedList.Count, 2);
        }

        [TestMethod]
        public async Task getTwoDeliverysFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var deliveryUpdatedList = await _main.OrderViewModel.Bl.BlOrder.GetDeliveryDataAsync(2);

            // Assert
            Assert.AreEqual(deliveryUpdatedList.Count, 2);
        }

        [TestMethod]
        public void searchDeliveryFromLocalDatabase()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var deliveryfoundList = _main.OrderViewModel.Bl.BlOrder.searchDelivery(delivery, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(deliveryfoundList.Count, 1);
        }

        [TestMethod]
        public async Task searchDeliveryFromLocalWebService()
        {
            // Arrange
            _mockProxy.initializer();
            _main = new MainWindowViewModel(new MockStartup(_mockProxy.Mock));

            // Act
            var deliveryfoundList = await _main.OrderViewModel.Bl.BlOrder.searchDeliveryAsync(delivery, QOBDCommon.Enum.ESearchOption.AND);

            // Assert
            Assert.AreEqual(deliveryfoundList.Count, 1);
        }
        #endregion
    }
}
