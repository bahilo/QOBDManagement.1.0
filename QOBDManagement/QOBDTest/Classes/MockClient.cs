using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using QOBDGateway.QOBDServiceReference;
using Moq;
using QOBDGateway.Classes;
using QOBDCommon.Classes;

namespace QOBDTest.Classes
{
    public class MockClient:  QOBDGateway.QOBDServiceReference.QOBDWebServicePortType
    {
        Mock<ClientProxy> _mock;

        public MockClient()
        {
            _mock = new Mock<ClientProxy>();
            initializer();
        }

        public MockClient(Mock<ClientProxy> mock)
        {
            _mock = mock;
            initializer();
        }

        public Mock<ClientProxy> Mock { get { return _mock; } }
        public ClientProxy Proxy { get { return _mock.Object; } }

        public void initializer()
        {
            #region [ Mock Agent ]
            //================================[ Agent ]===================================================

            // get_authenticate_user
            _mock.Setup(s => s.get_authenticate_userAsync(It.IsAny<string>(), It.IsAny<string>()))
               .Returns((string username, string password)=>get_authenticate_userAsync(username, password));
            _mock.Setup(s => s.get_authenticate_user(It.IsAny<string>(), It.IsAny<string>()))
               .Returns((string username, string password) => get_authenticate_user(username, password));

            // get_data_agent
            _mock.Setup(s => s.get_data_agentAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_agentAsync(nbLine));
            _mock.Setup(s => s.get_data_agent(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_agent(nbLine));

            // get_data_agent_by_id
            _mock.Setup(s => s.get_data_agent_by_idAsync(It.IsAny<string>()))
               .Returns((string id)=> get_data_agent_by_idAsync(id));
            _mock.Setup(s => s.get_data_agent_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_agent_by_id(id));

            // get_data_agent_credential
            _mock.Setup(s => s.get_data_agent_credentailAsync(It.IsAny<string>()))
               .Returns((string nbLine)=> get_data_agent_credentailAsync(nbLine));
            _mock.Setup(s => s.get_data_agent_credentail(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_agent_credentail(nbLine));

            // get_data_agent_by_command_list
            _mock.Setup(s => s.get_data_agent_by_command_listAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orderList) => get_data_agent_by_command_listAsync(orderList));
            _mock.Setup(s => s.get_data_agent_by_command_list(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orderList) => get_data_agent_by_command_list(orderList));

            // update_data_agent
            _mock.Setup(s => s.update_data_agentAsync(It.IsAny<AgentQOBD[]>()))
               .Returns((AgentQOBD[] agentList) => update_data_agentAsync(agentList));
            _mock.Setup(s => s.update_data_agent(It.IsAny<AgentQOBD[]>()))
               .Returns((AgentQOBD[] agentList) => update_data_agent(agentList));

            // delete_data_agent
            _mock.Setup(s => s.delete_data_agentAsync(It.IsAny<AgentQOBD[]>()))
               .Returns((AgentQOBD[] agentList) => delete_data_agentAsync(agentList));
            _mock.Setup(s => s.delete_data_agent(It.IsAny<AgentQOBD[]>()))
               .Returns((AgentQOBD[] agentList) => delete_data_agent(agentList));

            // insert_data_agent
            _mock.Setup(s => s.insert_data_agentAsync(It.IsAny<AgentQOBD[]>()))
               .Returns((AgentQOBD[] agentList) => insert_data_agentAsync(agentList));
            _mock.Setup(s => s.insert_data_agent(It.IsAny<AgentQOBD[]>()))
               .Returns((AgentQOBD[] agentList) => insert_data_agent(agentList));

            // get_filter_agent
            _mock.Setup(s => s.get_filter_agentAsync(It.IsAny<AgentFilterQOBD>()))
               .Returns((AgentFilterQOBD agentFilter) => get_filter_agentAsync(agentFilter));
            _mock.Setup(s => s.get_filter_agent(It.IsAny<AgentFilterQOBD>()))
               .Returns((AgentFilterQOBD agentFilter) => get_filter_agent(agentFilter));

            #endregion

            #region [ Mock Client ]
            //================================[ Client ]===================================================

            // insert_data_agent
            _mock.Setup(s => s.insert_data_clientAsync(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clientList) => insert_data_clientAsync(clientList));
            _mock.Setup(s => s.insert_data_client(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clientList) => insert_data_client(clientList));

            // delete_data_client
            _mock.Setup(s => s.delete_data_clientAsync(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => delete_data_clientAsync(clients));
            _mock.Setup(s => s.delete_data_client(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => delete_data_client(clients));

            // get_data_client
            _mock.Setup(s => s.get_data_clientAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_clientAsync(nbLine));
            _mock.Setup(s => s.get_data_client(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_client(nbLine));

            // get_data_client_by_id
            _mock.Setup(s => s.get_data_client_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_client_by_idAsync(id));
            _mock.Setup(s => s.get_data_client_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_client_by_id(id));

            // get_data_client_by_bill_list
            _mock.Setup(s => s.get_data_client_by_bill_listAsync(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] bills) => get_data_client_by_bill_listAsync(bills));
            _mock.Setup(s => s.get_data_client_by_bill_list(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] bills) => get_data_client_by_bill_list(bills));

            // get_data_client_by_max_credit_over
            _mock.Setup(s => s.get_data_client_by_max_credit_overAsync(It.IsAny<int>()))
               .Returns((int agentId) => get_data_client_by_max_credit_overAsync(agentId));
            _mock.Setup(s => s.get_data_client_by_max_credit_over(It.IsAny<int>()))
               .Returns((int agentId) => get_data_client_by_max_credit_over(agentId));

            // get_filter_Client
            _mock.Setup(s => s.get_filter_ClientAsync(It.IsAny<ClientFilterQOBD>()))
               .Returns((ClientFilterQOBD clientFilter) => get_filter_ClientAsync(clientFilter));
            _mock.Setup(s => s.get_filter_Client(It.IsAny<ClientFilterQOBD>()))
               .Returns((ClientFilterQOBD agentFilter) => get_filter_Client(agentFilter));

            // update_data_client
            _mock.Setup(s => s.update_data_clientAsync(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => update_data_clientAsync(clients));
            _mock.Setup(s => s.update_data_client(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => update_data_client(clients));

            #region [ Contact ]
            //----------------[ Contact ]

            // insert_data_agent
            _mock.Setup(s => s.insert_data_contactAsync(It.IsAny<ContactQOBD[]>()))
               .Returns((ContactQOBD[] contactList) => insert_data_contactAsync(contactList));
            _mock.Setup(s => s.insert_data_contact(It.IsAny<ContactQOBD[]>()))
               .Returns((ContactQOBD[] contactList) => insert_data_contact(contactList));

            // delete_data_contact
            _mock.Setup(s => s.delete_data_contactAsync(It.IsAny<ContactQOBD[]>()))
               .Returns((ContactQOBD[] contacts) => delete_data_contactAsync(contacts));
            _mock.Setup(s => s.delete_data_contact(It.IsAny<ContactQOBD[]>()))
               .Returns((ContactQOBD[] contacts) => delete_data_contact(contacts));

            // get_data_contact
            _mock.Setup(s => s.get_data_contactAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_contactAsync(nbLine));
            _mock.Setup(s => s.get_data_contact(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_contact(nbLine));

            // get_data_contactt_by_id
            _mock.Setup(s => s.get_data_contact_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_contact_by_idAsync(id));
            _mock.Setup(s => s.get_data_contact_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_contact_by_id(id));

            // get_data_contact_by_client_list
            _mock.Setup(s => s.get_data_contact_by_client_listAsync(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => get_data_contact_by_client_listAsync(clients));
            _mock.Setup(s => s.get_data_contact_by_client_list(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => get_data_contact_by_client_list(clients));

            // get_filter_contact
            _mock.Setup(s => s.get_filter_contactAsync(It.IsAny<ContactFilterQOBD>()))
               .Returns((ContactFilterQOBD contact) => get_filter_contactAsync(contact));
            _mock.Setup(s => s.get_filter_contact(It.IsAny<ContactFilterQOBD>()))
               .Returns((ContactFilterQOBD contact) => get_filter_contact(contact));

            // update_data_contact
            _mock.Setup(s => s.update_data_contactAsync(It.IsAny<ContactQOBD[]>()))
               .Returns((ContactQOBD[] contacts) => update_data_contactAsync(contacts));
            _mock.Setup(s => s.update_data_contact(It.IsAny<ContactQOBD[]>()))
               .Returns((ContactQOBD[] contacts) => update_data_contact(contacts));
            #endregion
            #region [ Address ]
            //----------------[ Address ]

            // insert_data_agent
            _mock.Setup(s => s.insert_data_addressAsync(It.IsAny<AddressQOBD[]>()))
               .Returns((AddressQOBD[] addressList) => insert_data_addressAsync(addressList));
            _mock.Setup(s => s.insert_data_address(It.IsAny<AddressQOBD[]>()))
               .Returns((AddressQOBD[] addressList) => insert_data_address(addressList));

            // delete_data_address
            _mock.Setup(s => s.delete_data_addressAsync(It.IsAny<AddressQOBD[]>()))
               .Returns((AddressQOBD[] addresses) => delete_data_addressAsync(addresses));
            _mock.Setup(s => s.delete_data_address(It.IsAny<AddressQOBD[]>()))
               .Returns((AddressQOBD[] addresses) => delete_data_address(addresses));

            // get_data_address
            _mock.Setup(s => s.get_data_addressAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_addressAsync(nbLine));
            _mock.Setup(s => s.get_data_address(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_address(nbLine));

            // get_data_addresst_by_id
            _mock.Setup(s => s.get_data_address_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_address_by_idAsync(id));
            _mock.Setup(s => s.get_data_address_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_address_by_id(id));

            // get_data_address_by_client_list
            _mock.Setup(s => s.get_data_address_by_client_listAsync(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => get_data_address_by_client_listAsync(clients));
            _mock.Setup(s => s.get_data_address_by_client_list(It.IsAny<ClientQOBD[]>()))
               .Returns((ClientQOBD[] clients) => get_data_address_by_client_list(clients));

            // get_filter_address
            _mock.Setup(s => s.get_filter_addressAsync(It.IsAny<AddressFilterQOBD>()))
               .Returns((AddressFilterQOBD address) => get_filter_addressAsync(address));
            _mock.Setup(s => s.get_filter_address(It.IsAny<AddressFilterQOBD>()))
               .Returns((AddressFilterQOBD address) => get_filter_address(address));

            // update_data_address
            _mock.Setup(s => s.update_data_addressAsync(It.IsAny<AddressQOBD[]>()))
               .Returns((AddressQOBD[] addresses) => update_data_addressAsync(addresses));
            _mock.Setup(s => s.update_data_address(It.IsAny<AddressQOBD[]>()))
               .Returns((AddressQOBD[] addresses) => update_data_address(addresses));
            #endregion
            #endregion

            #region [ Mock Order ]
            //================================[ Order ]===================================================

            // insert_data_command
            _mock.Setup(s => s.insert_data_commandAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orderList) => insert_data_commandAsync(orderList));
            _mock.Setup(s => s.insert_data_command(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orderList) => insert_data_command(orderList));

            // delete_data_command
            _mock.Setup(s => s.delete_data_commandAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] commands) => delete_data_commandAsync(commands));
            _mock.Setup(s => s.delete_data_command(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] commands) => delete_data_command(commands));

            // get_data_command
            _mock.Setup(s => s.get_data_commandAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_commandAsync(nbLine));
            _mock.Setup(s => s.get_data_command(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_command(nbLine));

            // get_data_command_by_id
            _mock.Setup(s => s.get_data_command_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_command_by_idAsync(id));
            _mock.Setup(s => s.get_data_command_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_command_by_id(id));
            
            // get_filter_Command
            _mock.Setup(s => s.get_filter_commandAsync(It.IsAny<CommandFilterQOBD>()))
               .Returns((CommandFilterQOBD commandFilter) => get_filter_commandAsync(commandFilter));
            _mock.Setup(s => s.get_filter_command(It.IsAny<CommandFilterQOBD>()))
               .Returns((CommandFilterQOBD agentFilter) => get_filter_command(agentFilter));

            // update_data_command
            _mock.Setup(s => s.update_data_commandAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] commands) => update_data_commandAsync(commands));
            _mock.Setup(s => s.update_data_command(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] commands) => update_data_command(commands));
            
            #region [ Order_item ]
            //----------------[ Order_item ]

            // insert_data_command_item
            _mock.Setup(s => s.insert_data_command_itemAsync(It.IsAny<Command_itemQOBD[]>()))
               .Returns((Command_itemQOBD[] order_itemList) => insert_data_command_itemAsync(order_itemList));
            _mock.Setup(s => s.insert_data_command_item(It.IsAny<Command_itemQOBD[]>()))
               .Returns((Command_itemQOBD[] order_itemList) => insert_data_command_item(order_itemList));

            // delete_data_command_item
            _mock.Setup(s => s.delete_data_command_itemAsync(It.IsAny<Command_itemQOBD[]>()))
               .Returns((Command_itemQOBD[] command_items) => delete_data_command_itemAsync(command_items));
            _mock.Setup(s => s.delete_data_command_item(It.IsAny<Command_itemQOBD[]>()))
               .Returns((Command_itemQOBD[] command_items) => delete_data_command_item(command_items));

            // get_data_command_item
            _mock.Setup(s => s.get_data_command_itemAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_command_itemAsync(nbLine));
            _mock.Setup(s => s.get_data_command_item(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_command_item(nbLine));

            // get_data_command_itemt_by_id
            _mock.Setup(s => s.get_data_command_item_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_command_item_by_idAsync(id));
            _mock.Setup(s => s.get_data_command_item_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_command_item_by_id(id));
            
            // get_filter_command_item
            _mock.Setup(s => s.get_filter_command_itemAsync(It.IsAny<Command_itemFilterQOBD>()))
               .Returns((Command_itemFilterQOBD command_item) => get_filter_command_itemAsync(command_item));
            _mock.Setup(s => s.get_filter_command_item(It.IsAny<Command_itemFilterQOBD>()))
               .Returns((Command_itemFilterQOBD command_item) => get_filter_command_item(command_item));

            // update_data_command_item
            _mock.Setup(s => s.update_data_command_itemAsync(It.IsAny<Command_itemQOBD[]>()))
               .Returns((Command_itemQOBD[] command_items) => update_data_command_itemAsync(command_items));
            _mock.Setup(s => s.update_data_command_item(It.IsAny<Command_itemQOBD[]>()))
               .Returns((Command_itemQOBD[] command_items) => update_data_command_item(command_items));
            #endregion
            #region [ Bill ]
            //----------------[ Bill ]

            // insert_data_bill
            _mock.Setup(s => s.insert_data_billAsync(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] billList) => insert_data_billAsync(billList));
            _mock.Setup(s => s.insert_data_bill(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] billList) => insert_data_bill(billList));

            // delete_data_bill
            _mock.Setup(s => s.delete_data_billAsync(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] bills) => delete_data_billAsync(bills));
            _mock.Setup(s => s.delete_data_bill(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] bills) => delete_data_bill(bills));

            // get_data_bill
            _mock.Setup(s => s.get_data_billAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_billAsync(nbLine));
            _mock.Setup(s => s.get_data_bill(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_bill(nbLine));

            // get_data_billt_by_id
            _mock.Setup(s => s.get_data_bill_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_bill_by_idAsync(id));
            _mock.Setup(s => s.get_data_bill_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_bill_by_id(id));

            // get_data_bill_by_command_list
            _mock.Setup(s => s.get_data_bill_by_command_listAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orders) => get_data_bill_by_command_listAsync(orders));
            _mock.Setup(s => s.get_data_bill_by_command_list(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orders) => get_data_bill_by_command_list(orders));

            // get_data_bill_by_unpaid
            _mock.Setup(s => s.get_data_bill_by_unpaidAsync(It.IsAny<int>()))
               .Returns((int agentId) => get_data_bill_by_unpaidAsync(agentId));
            _mock.Setup(s => s.get_data_bill_by_unpaid(It.IsAny<int>()))
               .Returns((int agentId) => get_data_bill_by_unpaid(agentId));

            // get_filter_bill
            _mock.Setup(s => s.get_filter_billAsync(It.IsAny<BillFilterQOBD>()))
               .Returns((BillFilterQOBD bill) => get_filter_billAsync(bill));
            _mock.Setup(s => s.get_filter_bill(It.IsAny<BillFilterQOBD>()))
               .Returns((BillFilterQOBD bill) => get_filter_bill(bill));

            // update_data_bill
            _mock.Setup(s => s.update_data_billAsync(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] bills) => update_data_billAsync(bills));
            _mock.Setup(s => s.update_data_bill(It.IsAny<BillQOBD[]>()))
               .Returns((BillQOBD[] bills) => update_data_bill(bills));

            #endregion
            #region [ Tax ]
            //----------------[ Tax ]

            // insert_data_tax
            _mock.Setup(s => s.insert_data_taxAsync(It.IsAny<TaxQOBD[]>()))
               .Returns((TaxQOBD[] taxList) => insert_data_taxAsync(taxList));
            _mock.Setup(s => s.insert_data_tax(It.IsAny<TaxQOBD[]>()))
               .Returns((TaxQOBD[] taxList) => insert_data_tax(taxList));

            // delete_data_tax
            _mock.Setup(s => s.delete_data_taxAsync(It.IsAny<TaxQOBD[]>()))
               .Returns((TaxQOBD[] taxs) => delete_data_taxAsync(taxs));
            _mock.Setup(s => s.delete_data_tax(It.IsAny<TaxQOBD[]>()))
               .Returns((TaxQOBD[] taxs) => delete_data_tax(taxs));

            // get_data_tax
            _mock.Setup(s => s.get_data_taxAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_taxAsync(nbLine));
            _mock.Setup(s => s.get_data_tax(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_tax(nbLine));

            // get_data_taxt_by_id
            _mock.Setup(s => s.get_data_tax_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_tax_by_idAsync(id));
            _mock.Setup(s => s.get_data_tax_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_tax_by_id(id));
                                 
            // get_filter_tax
            _mock.Setup(s => s.get_filter_taxAsync(It.IsAny<TaxFilterQOBD>()))
               .Returns((TaxFilterQOBD tax) => get_filter_taxAsync(tax));
            _mock.Setup(s => s.get_filter_tax(It.IsAny<TaxFilterQOBD>()))
               .Returns((TaxFilterQOBD tax) => get_filter_tax(tax));

            // update_data_tax
            _mock.Setup(s => s.update_data_taxAsync(It.IsAny<TaxQOBD[]>()))
               .Returns((TaxQOBD[] taxs) => update_data_taxAsync(taxs));
            _mock.Setup(s => s.update_data_tax(It.IsAny<TaxQOBD[]>()))
               .Returns((TaxQOBD[] taxs) => update_data_tax(taxs));

            #endregion
            #region [ Tax_order ]
            //----------------[ Tax_order ]

            // insert_data_tax_command
            _mock.Setup(s => s.insert_data_tax_commandAsync(It.IsAny<Tax_commandQOBD[]>()))
               .Returns((Tax_commandQOBD[] tax_commandList) => insert_data_tax_commandAsync(tax_commandList));
            _mock.Setup(s => s.insert_data_tax_command(It.IsAny<Tax_commandQOBD[]>()))
               .Returns((Tax_commandQOBD[] tax_commandList) => insert_data_tax_command(tax_commandList));

            // delete_data_tax_command
            _mock.Setup(s => s.delete_data_tax_commandAsync(It.IsAny<Tax_commandQOBD[]>()))
               .Returns((Tax_commandQOBD[] tax_commands) => delete_data_tax_commandAsync(tax_commands));
            _mock.Setup(s => s.delete_data_tax_command(It.IsAny<Tax_commandQOBD[]>()))
               .Returns((Tax_commandQOBD[] tax_commands) => delete_data_tax_command(tax_commands));

            // get_data_tax_command
            _mock.Setup(s => s.get_data_tax_commandAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_tax_commandAsync(nbLine));
            _mock.Setup(s => s.get_data_tax_command(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_tax_command(nbLine));

            // get_data_tax_commandt_by_id
            _mock.Setup(s => s.get_data_tax_command_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_tax_command_by_idAsync(id));
            _mock.Setup(s => s.get_data_tax_command_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_tax_command_by_id(id));

            // get_data_tax_command_by_command_list
            _mock.Setup(s => s.get_data_tax_command_by_command_listAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orders) => get_data_tax_command_by_command_listAsync(orders));
            _mock.Setup(s => s.get_data_tax_command_by_command_list(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orders) => get_data_tax_command_by_command_list(orders));
            
            // get_filter_tax_command
            _mock.Setup(s => s.get_filter_tax_commandAsync(It.IsAny<Tax_commandFilterQOBD>()))
               .Returns((Tax_commandFilterQOBD tax_command) => get_filter_tax_commandAsync(tax_command));
            _mock.Setup(s => s.get_filter_tax_command(It.IsAny<Tax_commandFilterQOBD>()))
               .Returns((Tax_commandFilterQOBD tax_command) => get_filter_tax_command(tax_command));

            // update_data_tax_command
            _mock.Setup(s => s.update_data_tax_commandAsync(It.IsAny<Tax_commandQOBD[]>()))
               .Returns((Tax_commandQOBD[] tax_commands) => update_data_tax_commandAsync(tax_commands));
            _mock.Setup(s => s.update_data_tax_command(It.IsAny<Tax_commandQOBD[]>()))
               .Returns((Tax_commandQOBD[] tax_commands) => update_data_tax_command(tax_commands));

            #endregion
            #region [ Delivery ]
            //----------------[ Delivery ]

            // insert_data_delivery
            _mock.Setup(s => s.insert_data_deliveryAsync(It.IsAny<DeliveryQOBD[]>()))
               .Returns((DeliveryQOBD[] deliveryList) => insert_data_deliveryAsync(deliveryList));
            _mock.Setup(s => s.insert_data_delivery(It.IsAny<DeliveryQOBD[]>()))
               .Returns((DeliveryQOBD[] deliveryList) => insert_data_delivery(deliveryList));

            // delete_data_delivery
            _mock.Setup(s => s.delete_data_deliveryAsync(It.IsAny<DeliveryQOBD[]>()))
               .Returns((DeliveryQOBD[] deliverys) => delete_data_deliveryAsync(deliverys));
            _mock.Setup(s => s.delete_data_delivery(It.IsAny<DeliveryQOBD[]>()))
               .Returns((DeliveryQOBD[] deliverys) => delete_data_delivery(deliverys));

            // get_data_delivery
            _mock.Setup(s => s.get_data_deliveryAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_deliveryAsync(nbLine));
            _mock.Setup(s => s.get_data_delivery(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_delivery(nbLine));

            // get_data_deliveryt_by_id
            _mock.Setup(s => s.get_data_delivery_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_delivery_by_idAsync(id));
            _mock.Setup(s => s.get_data_delivery_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_delivery_by_id(id));

            // get_data_delivery_by_command_list
            _mock.Setup(s => s.get_data_delivery_by_command_listAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orders) => get_data_delivery_by_command_listAsync(orders));
            _mock.Setup(s => s.get_data_delivery_by_command_list(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orders) => get_data_delivery_by_command_list(orders));
            
            // get_filter_delivery
            _mock.Setup(s => s.get_filter_deliveryAsync(It.IsAny<DeliveryFilterQOBD>()))
               .Returns((DeliveryFilterQOBD delivery) => get_filter_deliveryAsync(delivery));
            _mock.Setup(s => s.get_filter_delivery(It.IsAny<DeliveryFilterQOBD>()))
               .Returns((DeliveryFilterQOBD delivery) => get_filter_delivery(delivery));

            // update_data_delivery
            _mock.Setup(s => s.update_data_deliveryAsync(It.IsAny<DeliveryQOBD[]>()))
               .Returns((DeliveryQOBD[] deliverys) => update_data_deliveryAsync(deliverys));
            _mock.Setup(s => s.update_data_delivery(It.IsAny<DeliveryQOBD[]>()))
               .Returns((DeliveryQOBD[] deliverys) => update_data_delivery(deliverys));

            #endregion

            #endregion

            #region [ Mock Item ]
            //================================[ Item ]===================================================
            
            // insert_data_item
            _mock.Setup(s => s.insert_data_itemAsync(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] itemList) => insert_data_itemAsync(itemList));
            _mock.Setup(s => s.insert_data_item(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] itemList) => insert_data_item(itemList));

            // delete_data_item
            _mock.Setup(s => s.delete_data_itemAsync(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => delete_data_itemAsync(items));
            _mock.Setup(s => s.delete_data_item(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => delete_data_item(items));

            // get_data_item
            _mock.Setup(s => s.get_data_itemAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_itemAsync(nbLine));
            _mock.Setup(s => s.get_data_item(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_item(nbLine));

            // get_data_itemt_by_id
            _mock.Setup(s => s.get_data_item_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_item_by_idAsync(id));
            _mock.Setup(s => s.get_data_item_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_item_by_id(id));

            // get_filter_item
            _mock.Setup(s => s.get_filter_itemAsync(It.IsAny<ItemFilterQOBD>()))
               .Returns((ItemFilterQOBD item) => get_filter_itemAsync(item));
            _mock.Setup(s => s.get_filter_item(It.IsAny<ItemFilterQOBD>()))
               .Returns((ItemFilterQOBD item) => get_filter_item(item));

            // update_data_item
            _mock.Setup(s => s.update_data_itemAsync(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => update_data_itemAsync(items));
            _mock.Setup(s => s.update_data_item(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => update_data_item(items));
            
            #region [ Provider ]

            // insert_data_provider
            _mock.Setup(s => s.insert_data_providerAsync(It.IsAny<ProviderQOBD[]>()))
               .Returns((ProviderQOBD[] providers) => insert_data_providerAsync(providers));
            _mock.Setup(s => s.insert_data_provider(It.IsAny<ProviderQOBD[]>()))
               .Returns((ProviderQOBD[] providers) => insert_data_provider(providers));

            // delete_data_provider
            _mock.Setup(s => s.delete_data_providerAsync(It.IsAny<ProviderQOBD[]>()))
               .Returns((ProviderQOBD[] providers) => delete_data_providerAsync(providers));
            _mock.Setup(s => s.delete_data_provider(It.IsAny<ProviderQOBD[]>()))
               .Returns((ProviderQOBD[] providers) => delete_data_provider(providers));

            // get_data_provider
            _mock.Setup(s => s.get_data_providerAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_providerAsync(nbLine));
            _mock.Setup(s => s.get_data_provider(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_provider(nbLine));

            // get_data_provider_by_id
            _mock.Setup(s => s.get_data_provider_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_provider_by_idAsync(id));
            _mock.Setup(s => s.get_data_provider_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_provider_by_id(id));

            // get_filter_provider
            _mock.Setup(s => s.get_filter_providerAsync(It.IsAny<ProviderFilterQOBD>()))
               .Returns((ProviderFilterQOBD provider) => get_filter_providerAsync(provider));
            _mock.Setup(s => s.get_filter_provider(It.IsAny<ProviderFilterQOBD>()))
               .Returns((ProviderFilterQOBD provider) => get_filter_provider(provider));

            // update_data_provider
            _mock.Setup(s => s.update_data_providerAsync(It.IsAny<ProviderQOBD[]>()))
               .Returns((ProviderQOBD[] providers) => update_data_providerAsync(providers));
            _mock.Setup(s => s.update_data_provider(It.IsAny<ProviderQOBD[]>()))
               .Returns((ProviderQOBD[] providers) => update_data_provider(providers));
            #endregion
            #region [ Provider_item ]
            //----------------[ Provider_item ]

            // insert_data_provider_item
            _mock.Setup(s => s.insert_data_provider_itemAsync(It.IsAny<Provider_itemQOBD[]>()))
               .Returns((Provider_itemQOBD[] provider_items) => insert_data_provider_itemAsync(provider_items));
            _mock.Setup(s => s.insert_data_provider_item(It.IsAny<Provider_itemQOBD[]>()))
               .Returns((Provider_itemQOBD[] provider_items) => insert_data_provider_item(provider_items));

            // delete_data_provider_item
            _mock.Setup(s => s.delete_data_provider_itemAsync(It.IsAny<Provider_itemQOBD[]>()))
               .Returns((Provider_itemQOBD[] provider_items) => delete_data_provider_itemAsync(provider_items));
            _mock.Setup(s => s.delete_data_provider_item(It.IsAny<Provider_itemQOBD[]>()))
               .Returns((Provider_itemQOBD[] provider_items) => delete_data_provider_item(provider_items));

            // get_data_provider_item
            _mock.Setup(s => s.get_data_provider_itemAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_provider_itemAsync(nbLine));
            _mock.Setup(s => s.get_data_provider_item(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_provider_item(nbLine));

            // get_data_provider_itemt_by_id
            _mock.Setup(s => s.get_data_provider_item_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_provider_item_by_idAsync(id));
            _mock.Setup(s => s.get_data_provider_item_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_provider_item_by_id(id));

            // get_data_provider_item_by_client_list
            _mock.Setup(s => s.get_data_provider_item_by_item_listAsync(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => get_data_provider_item_by_item_listAsync(items));
            _mock.Setup(s => s.get_data_provider_item_by_item_list(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => get_data_provider_item_by_item_list(items));

            // get_filter_provider_item
            _mock.Setup(s => s.get_filter_provider_itemAsync(It.IsAny<Provider_itemFilterQOBD>()))
               .Returns((Provider_itemFilterQOBD provider_item) => get_filter_provider_itemAsync(provider_item));
            _mock.Setup(s => s.get_filter_provider_item(It.IsAny<Provider_itemFilterQOBD>()))
               .Returns((Provider_itemFilterQOBD provider_item) => get_filter_provider_item(provider_item));

            // update_data_provider_item
            _mock.Setup(s => s.update_data_provider_itemAsync(It.IsAny<Provider_itemQOBD[]>()))
               .Returns((Provider_itemQOBD[] provider_items) => update_data_provider_itemAsync(provider_items));
            _mock.Setup(s => s.update_data_provider_item(It.IsAny<Provider_itemQOBD[]>()))
               .Returns((Provider_itemQOBD[] provider_items) => update_data_provider_item(provider_items));
            #endregion
            #region [ Tax_item ]
            //----------------[ Tax_item ]

            // insert_data_tax_item
            _mock.Setup(s => s.insert_data_tax_itemAsync(It.IsAny<Tax_itemQOBD[]>()))
               .Returns((Tax_itemQOBD[] tax_items) => insert_data_tax_itemAsync(tax_items));
            _mock.Setup(s => s.insert_data_tax_item(It.IsAny<Tax_itemQOBD[]>()))
               .Returns((Tax_itemQOBD[] tax_items) => insert_data_tax_item(tax_items));

            // delete_data_tax_item
            _mock.Setup(s => s.delete_data_tax_itemAsync(It.IsAny<Tax_itemQOBD[]>()))
               .Returns((Tax_itemQOBD[] tax_items) => delete_data_tax_itemAsync(tax_items));
            _mock.Setup(s => s.delete_data_tax_item(It.IsAny<Tax_itemQOBD[]>()))
               .Returns((Tax_itemQOBD[] tax_items) => delete_data_tax_item(tax_items));

            // get_data_tax_item
            _mock.Setup(s => s.get_data_tax_itemAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_tax_itemAsync(nbLine));
            _mock.Setup(s => s.get_data_tax_item(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_tax_item(nbLine));

            // get_data_tax_itemt_by_id
            _mock.Setup(s => s.get_data_tax_item_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_tax_item_by_idAsync(id));
            _mock.Setup(s => s.get_data_tax_item_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_tax_item_by_id(id));

            // get_data_tax_item_by_client_list
            _mock.Setup(s => s.get_data_tax_item_by_item_listAsync(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => get_data_tax_item_by_item_listAsync(items));
            _mock.Setup(s => s.get_data_tax_item_by_item_list(It.IsAny<ItemQOBD[]>()))
               .Returns((ItemQOBD[] items) => get_data_tax_item_by_item_list(items));

            // get_filter_tax_item
            _mock.Setup(s => s.get_filter_tax_itemAsync(It.IsAny<Tax_itemFilterQOBD>()))
               .Returns((Tax_itemFilterQOBD tax_item) => get_filter_tax_itemAsync(tax_item));
            _mock.Setup(s => s.get_filter_tax_item(It.IsAny<Tax_itemFilterQOBD>()))
               .Returns((Tax_itemFilterQOBD tax_item) => get_filter_tax_item(tax_item));

            // update_data_tax_item
            _mock.Setup(s => s.update_data_tax_itemAsync(It.IsAny<Tax_itemQOBD[]>()))
               .Returns((Tax_itemQOBD[] tax_items) => update_data_tax_itemAsync(tax_items));
            _mock.Setup(s => s.update_data_tax_item(It.IsAny<Tax_itemQOBD[]>()))
               .Returns((Tax_itemQOBD[] tax_items) => update_data_tax_item(tax_items));
            #endregion
            #region [ Auto_ref ]
            //----------------[ Auto_ref ]

            // insert_data_auto_ref
            _mock.Setup(s => s.insert_data_auto_refAsync(It.IsAny<Auto_refsQOBD[]>()))
               .Returns((Auto_refsQOBD[] auto_refs) => insert_data_auto_refAsync(auto_refs));
            _mock.Setup(s => s.insert_data_auto_ref(It.IsAny<Auto_refsQOBD[]>()))
               .Returns((Auto_refsQOBD[] auto_refs) => insert_data_auto_ref(auto_refs));

            // delete_data_auto_ref
            _mock.Setup(s => s.delete_data_auto_refAsync(It.IsAny<Auto_refsQOBD[]>()))
               .Returns((Auto_refsQOBD[] auto_refs) => delete_data_auto_refAsync(auto_refs));
            _mock.Setup(s => s.delete_data_auto_ref(It.IsAny<Auto_refsQOBD[]>()))
               .Returns((Auto_refsQOBD[] auto_refs) => delete_data_auto_ref(auto_refs));

            // get_data_auto_ref
            _mock.Setup(s => s.get_data_auto_refAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_auto_refAsync(nbLine));
            _mock.Setup(s => s.get_data_auto_ref(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_auto_ref(nbLine));

            // get_data_auto_reft_by_id
            _mock.Setup(s => s.get_data_auto_ref_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_auto_ref_by_idAsync(id));
            _mock.Setup(s => s.get_data_auto_ref_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_auto_ref_by_id(id));

            // get_filter_auto_ref
            _mock.Setup(s => s.get_filter_auto_refAsync(It.IsAny<Auto_refsFilterQOBD>()))
               .Returns((Auto_refsFilterQOBD auto_ref) => get_filter_auto_refAsync(auto_ref));
            _mock.Setup(s => s.get_filter_auto_ref(It.IsAny<Auto_refsFilterQOBD>()))
               .Returns((Auto_refsFilterQOBD auto_ref) => get_filter_auto_ref(auto_ref));

            // update_data_auto_ref
            _mock.Setup(s => s.update_data_auto_refAsync(It.IsAny<Auto_refsQOBD[]>()))
               .Returns((Auto_refsQOBD[] auto_refs) => update_data_auto_refAsync(auto_refs));
            _mock.Setup(s => s.update_data_auto_ref(It.IsAny<Auto_refsQOBD[]>()))
               .Returns((Auto_refsQOBD[] auto_refs) => update_data_auto_ref(auto_refs));
            #endregion

            #endregion

            #region [ Mock Notification ]
            //================================[ Notification ]===================================================
                        
            // get_data_notification
            _mock.Setup(s => s.get_data_notificationAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_notificationAsync(nbLine));
            _mock.Setup(s => s.get_data_notification(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_notification(nbLine));

            // get_data_notification_by_id
            _mock.Setup(s => s.get_data_notification_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_notification_by_idAsync(id));
            _mock.Setup(s => s.get_data_notification_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_notification_by_id(id));
            
            // get_data_notification_by_command_list
            _mock.Setup(s => s.get_data_notification_by_command_listAsync(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orderList) => get_data_notification_by_command_listAsync(orderList));
            _mock.Setup(s => s.get_data_notification_by_command_list(It.IsAny<CommandsQOBD[]>()))
               .Returns((CommandsQOBD[] orderList) => get_data_notification_by_command_list(orderList));

            // update_data_notification
            _mock.Setup(s => s.update_data_notificationAsync(It.IsAny<NotificationQOBD[]>()))
               .Returns((NotificationQOBD[] notificationList) => update_data_notificationAsync(notificationList));
            _mock.Setup(s => s.update_data_notification(It.IsAny<NotificationQOBD[]>()))
               .Returns((NotificationQOBD[] notificationList) => update_data_notification(notificationList));

            // delete_data_notification
            _mock.Setup(s => s.delete_data_notificationAsync(It.IsAny<NotificationQOBD[]>()))
               .Returns((NotificationQOBD[] notificationList) => delete_data_notificationAsync(notificationList));
            _mock.Setup(s => s.delete_data_notification(It.IsAny<NotificationQOBD[]>()))
               .Returns((NotificationQOBD[] notificationList) => delete_data_notification(notificationList));

            // insert_data_notification
            _mock.Setup(s => s.insert_data_notificationAsync(It.IsAny<NotificationQOBD[]>()))
               .Returns((NotificationQOBD[] notificationList) => insert_data_notificationAsync(notificationList));
            _mock.Setup(s => s.insert_data_notification(It.IsAny<NotificationQOBD[]>()))
               .Returns((NotificationQOBD[] notificationList) => insert_data_notification(notificationList));

            // get_filter_notification
            _mock.Setup(s => s.get_filter_notificationAsync(It.IsAny<NotificationFilterQOBD>()))
               .Returns((NotificationFilterQOBD notificationFilter) => get_filter_notificationAsync(notificationFilter));
            _mock.Setup(s => s.get_filter_notification(It.IsAny<NotificationFilterQOBD>()))
               .Returns((NotificationFilterQOBD notificationFilter) => get_filter_notification(notificationFilter));

            #endregion

            #region [ Mock Referential ]
            //================================[ Referential ]===================================================
                        
            // get_data_infos
            _mock.Setup(s => s.get_data_infosAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_infosAsync(nbLine));
            _mock.Setup(s => s.get_data_infos(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_infos(nbLine));

            // get_data_infos_by_id
            _mock.Setup(s => s.get_data_infos_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_infos_by_idAsync(id));
            _mock.Setup(s => s.get_data_infos_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_infos_by_id(id));
            
            // update_data_infos
            _mock.Setup(s => s.update_data_infosAsync(It.IsAny<InfosQOBD[]>()))
               .Returns((InfosQOBD[] infosList) => update_data_infosAsync(infosList));
            _mock.Setup(s => s.update_data_infos(It.IsAny<InfosQOBD[]>()))
               .Returns((InfosQOBD[] infosList) => update_data_infos(infosList));

            // delete_data_infos
            _mock.Setup(s => s.delete_data_infosAsync(It.IsAny<InfosQOBD[]>()))
               .Returns((InfosQOBD[] infosList) => delete_data_infosAsync(infosList));
            _mock.Setup(s => s.delete_data_infos(It.IsAny<InfosQOBD[]>()))
               .Returns((InfosQOBD[] infosList) => delete_data_infos(infosList));

            // insert_data_infos
            _mock.Setup(s => s.insert_data_infosAsync(It.IsAny<InfosQOBD[]>()))
               .Returns((InfosQOBD[] infosList) => insert_data_infosAsync(infosList));
            _mock.Setup(s => s.insert_data_infos(It.IsAny<InfosQOBD[]>()))
               .Returns((InfosQOBD[] infosList) => insert_data_infos(infosList));

            // get_filter_infos
            _mock.Setup(s => s.get_filter_infosAsync(It.IsAny<InfosFilterQOBD>()))
               .Returns((InfosFilterQOBD infosFilter) => get_filter_infosAsync(infosFilter));
            _mock.Setup(s => s.get_filter_infos(It.IsAny<InfosFilterQOBD>()))
               .Returns((InfosFilterQOBD infosFilter) => get_filter_infos(infosFilter));

            #endregion

            #region [ Mock Statistic ]
            //================================[ Statistic ]===================================================
            
            // get_data_statistic
            _mock.Setup(s => s.get_data_statisticAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_statisticAsync(nbLine));
            _mock.Setup(s => s.get_data_statistic(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_statistic(nbLine));

            // get_data_statistic_by_id
            _mock.Setup(s => s.get_data_statistic_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_statistic_by_idAsync(id));
            _mock.Setup(s => s.get_data_statistic_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_statistic_by_id(id));
                        
            // update_data_statistic
            _mock.Setup(s => s.update_data_statisticAsync(It.IsAny<StatisticQOBD[]>()))
               .Returns((StatisticQOBD[] statisticList) => update_data_statisticAsync(statisticList));
            _mock.Setup(s => s.update_data_statistic(It.IsAny<StatisticQOBD[]>()))
               .Returns((StatisticQOBD[] statisticList) => update_data_statistic(statisticList));

            // delete_data_statistic
            _mock.Setup(s => s.delete_data_statisticAsync(It.IsAny<StatisticQOBD[]>()))
               .Returns((StatisticQOBD[] statisticList) => delete_data_statisticAsync(statisticList));
            _mock.Setup(s => s.delete_data_statistic(It.IsAny<StatisticQOBD[]>()))
               .Returns((StatisticQOBD[] statisticList) => delete_data_statistic(statisticList));

            // insert_data_statistic
            _mock.Setup(s => s.insert_data_statisticAsync(It.IsAny<StatisticQOBD[]>()))
               .Returns((StatisticQOBD[] statisticList) => insert_data_statisticAsync(statisticList));
            _mock.Setup(s => s.insert_data_statistic(It.IsAny<StatisticQOBD[]>()))
               .Returns((StatisticQOBD[] statisticList) => insert_data_statistic(statisticList));

            // get_filter_statistic
            _mock.Setup(s => s.get_filter_statisticAsync(It.IsAny<StatisticFilterQOBD>()))
               .Returns((StatisticFilterQOBD statisticFilter) => get_filter_statisticAsync(statisticFilter));
            _mock.Setup(s => s.get_filter_statistic(It.IsAny<StatisticFilterQOBD>()))
               .Returns((StatisticFilterQOBD statisticFilter) => get_filter_statistic(statisticFilter));

            #endregion

            #region [ Mock Security ]
            //================================[ Security ]===================================================
            #region [ Action ]

            // insert_data_action
            _mock.Setup(s => s.insert_data_actionAsync(It.IsAny<ActionQOBD[]>()))
               .Returns((ActionQOBD[] actionList) => insert_data_actionAsync(actionList));
            _mock.Setup(s => s.insert_data_action(It.IsAny<ActionQOBD[]>()))
               .Returns((ActionQOBD[] actionList) => insert_data_action(actionList));

            // delete_data_action
            _mock.Setup(s => s.delete_data_actionAsync(It.IsAny<ActionQOBD[]>()))
               .Returns((ActionQOBD[] actions) => delete_data_actionAsync(actions));
            _mock.Setup(s => s.delete_data_action(It.IsAny<ActionQOBD[]>()))
               .Returns((ActionQOBD[] actions) => delete_data_action(actions));

            // get_data_action
            _mock.Setup(s => s.get_data_actionAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_actionAsync(nbLine));
            _mock.Setup(s => s.get_data_action(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_action(nbLine));

            // get_data_actiont_by_id
            _mock.Setup(s => s.get_data_action_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_action_by_idAsync(id));
            _mock.Setup(s => s.get_data_action_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_action_by_id(id));

            // get_filter_action
            _mock.Setup(s => s.get_filter_actionAsync(It.IsAny<ActionFilterQOBD>()))
               .Returns((ActionFilterQOBD action) => get_filter_actionAsync(action));
            _mock.Setup(s => s.get_filter_action(It.IsAny<ActionFilterQOBD>()))
               .Returns((ActionFilterQOBD action) => get_filter_action(action));

            // update_data_action
            _mock.Setup(s => s.update_data_actionAsync(It.IsAny<ActionQOBD[]>()))
               .Returns((ActionQOBD[] actions) => update_data_actionAsync(actions));
            _mock.Setup(s => s.update_data_action(It.IsAny<ActionQOBD[]>()))
               .Returns((ActionQOBD[] actions) => update_data_action(actions));
            #endregion
            #region [ ActionRecord ]

            // insert_data_actionRecord
            _mock.Setup(s => s.insert_data_actionRecordAsync(It.IsAny<ActionRecordQOBD[]>()))
               .Returns((ActionRecordQOBD[] actionRecords) => insert_data_actionRecordAsync(actionRecords));
            _mock.Setup(s => s.insert_data_actionRecord(It.IsAny<ActionRecordQOBD[]>()))
               .Returns((ActionRecordQOBD[] actionRecords) => insert_data_actionRecord(actionRecords));

            // delete_data_actionRecord
            _mock.Setup(s => s.delete_data_actionRecordAsync(It.IsAny<ActionRecordQOBD[]>()))
               .Returns((ActionRecordQOBD[] actionRecords) => delete_data_actionRecordAsync(actionRecords));
            _mock.Setup(s => s.delete_data_actionRecord(It.IsAny<ActionRecordQOBD[]>()))
               .Returns((ActionRecordQOBD[] actionRecords) => delete_data_actionRecord(actionRecords));

            // get_data_actionRecord
            _mock.Setup(s => s.get_data_actionRecordAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_actionRecordAsync(nbLine));
            _mock.Setup(s => s.get_data_actionRecord(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_actionRecord(nbLine));

            // get_data_actionRecord_by_id
            _mock.Setup(s => s.get_data_actionRecord_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_actionRecord_by_idAsync(id));
            _mock.Setup(s => s.get_data_actionRecord_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_actionRecord_by_id(id));

            // get_filter_actionRecord
            _mock.Setup(s => s.get_filter_actionRecordAsync(It.IsAny<ActionRecordFilterQOBD>()))
               .Returns((ActionRecordFilterQOBD actionRecord) => get_filter_actionRecordAsync(actionRecord));
            _mock.Setup(s => s.get_filter_actionRecord(It.IsAny<ActionRecordFilterQOBD>()))
               .Returns((ActionRecordFilterQOBD actionRecord) => get_filter_actionRecord(actionRecord));

            // update_data_actionRecord
            _mock.Setup(s => s.update_data_actionRecordAsync(It.IsAny<ActionRecordQOBD[]>()))
               .Returns((ActionRecordQOBD[] actionRecords) => update_data_actionRecordAsync(actionRecords));
            _mock.Setup(s => s.update_data_actionRecord(It.IsAny<ActionRecordQOBD[]>()))
               .Returns((ActionRecordQOBD[] actionRecords) => update_data_actionRecord(actionRecords));
            #endregion
            #region [ Agent_role ]
            //----------------[ Agent_role ]

            // insert_data_agent_role
            _mock.Setup(s => s.insert_data_agent_roleAsync(It.IsAny<Agent_roleQOBD[]>()))
               .Returns((Agent_roleQOBD[] agent_roles) => insert_data_agent_roleAsync(agent_roles));
            _mock.Setup(s => s.insert_data_agent_role(It.IsAny<Agent_roleQOBD[]>()))
               .Returns((Agent_roleQOBD[] agent_roles) => insert_data_agent_role(agent_roles));

            // delete_data_agent_role
            _mock.Setup(s => s.delete_data_agent_roleAsync(It.IsAny<Agent_roleQOBD[]>()))
               .Returns((Agent_roleQOBD[] agent_roles) => delete_data_agent_roleAsync(agent_roles));
            _mock.Setup(s => s.delete_data_agent_role(It.IsAny<Agent_roleQOBD[]>()))
               .Returns((Agent_roleQOBD[] agent_roles) => delete_data_agent_role(agent_roles));

            // get_data_agent_role
            _mock.Setup(s => s.get_data_agent_roleAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_agent_roleAsync(nbLine));
            _mock.Setup(s => s.get_data_agent_role(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_agent_role(nbLine));

            // get_data_agent_rolet_by_id
            _mock.Setup(s => s.get_data_agent_role_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_agent_role_by_idAsync(id));
            _mock.Setup(s => s.get_data_agent_role_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_agent_role_by_id(id));
            
            // get_filter_agent_role
            _mock.Setup(s => s.get_filter_agent_roleAsync(It.IsAny<Agent_roleFilterQOBD>()))
               .Returns((Agent_roleFilterQOBD agent_role) => get_filter_agent_roleAsync(agent_role));
            _mock.Setup(s => s.get_filter_agent_role(It.IsAny<Agent_roleFilterQOBD>()))
               .Returns((Agent_roleFilterQOBD agent_role) => get_filter_agent_role(agent_role));

            // update_data_agent_role
            _mock.Setup(s => s.update_data_agent_roleAsync(It.IsAny<Agent_roleQOBD[]>()))
               .Returns((Agent_roleQOBD[] agent_roles) => update_data_agent_roleAsync(agent_roles));
            _mock.Setup(s => s.update_data_agent_role(It.IsAny<Agent_roleQOBD[]>()))
               .Returns((Agent_roleQOBD[] agent_roles) => update_data_agent_role(agent_roles));
            #endregion
            #region [ Privilege ]
            //----------------[ Privilege ]

            // insert_data_privilege
            _mock.Setup(s => s.insert_data_privilegeAsync(It.IsAny<PrivilegeQOBD[]>()))
               .Returns((PrivilegeQOBD[] privileges) => insert_data_privilegeAsync(privileges));
            _mock.Setup(s => s.insert_data_privilege(It.IsAny<PrivilegeQOBD[]>()))
               .Returns((PrivilegeQOBD[] privileges) => insert_data_privilege(privileges));

            // delete_data_privilege
            _mock.Setup(s => s.delete_data_privilegeAsync(It.IsAny<PrivilegeQOBD[]>()))
               .Returns((PrivilegeQOBD[] privileges) => delete_data_privilegeAsync(privileges));
            _mock.Setup(s => s.delete_data_privilege(It.IsAny<PrivilegeQOBD[]>()))
               .Returns((PrivilegeQOBD[] privileges) => delete_data_privilege(privileges));

            // get_data_privilege
            _mock.Setup(s => s.get_data_privilegeAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_privilegeAsync(nbLine));
            _mock.Setup(s => s.get_data_privilege(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_privilege(nbLine));

            // get_data_privileget_by_id
            _mock.Setup(s => s.get_data_privilege_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_privilege_by_idAsync(id));
            _mock.Setup(s => s.get_data_privilege_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_privilege_by_id(id));
            
            // get_filter_privilege
            _mock.Setup(s => s.get_filter_privilegeAsync(It.IsAny<PrivilegeFilterQOBD>()))
               .Returns((PrivilegeFilterQOBD privilege) => get_filter_privilegeAsync(privilege));
            _mock.Setup(s => s.get_filter_privilege(It.IsAny<PrivilegeFilterQOBD>()))
               .Returns((PrivilegeFilterQOBD privilege) => get_filter_privilege(privilege));

            // update_data_privilege
            _mock.Setup(s => s.update_data_privilegeAsync(It.IsAny<PrivilegeQOBD[]>()))
               .Returns((PrivilegeQOBD[] privileges) => update_data_privilegeAsync(privileges));
            _mock.Setup(s => s.update_data_privilege(It.IsAny<PrivilegeQOBD[]>()))
               .Returns((PrivilegeQOBD[] privileges) => update_data_privilege(privileges));
            #endregion
            #region [ Role ]
            //----------------[ Role ]

            // insert_data_role
            _mock.Setup(s => s.insert_data_roleAsync(It.IsAny<RoleQOBD[]>()))
               .Returns((RoleQOBD[] roles) => insert_data_roleAsync(roles));
            _mock.Setup(s => s.insert_data_role(It.IsAny<RoleQOBD[]>()))
               .Returns((RoleQOBD[] roles) => insert_data_role(roles));

            // delete_data_role
            _mock.Setup(s => s.delete_data_roleAsync(It.IsAny<RoleQOBD[]>()))
               .Returns((RoleQOBD[] roles) => delete_data_roleAsync(roles));
            _mock.Setup(s => s.delete_data_role(It.IsAny<RoleQOBD[]>()))
               .Returns((RoleQOBD[] roles) => delete_data_role(roles));

            // get_data_role
            _mock.Setup(s => s.get_data_roleAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_roleAsync(nbLine));
            _mock.Setup(s => s.get_data_role(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_role(nbLine));

            // get_data_rolet_by_id
            _mock.Setup(s => s.get_data_role_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_role_by_idAsync(id));
            _mock.Setup(s => s.get_data_role_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_role_by_id(id));

            // get_filter_role
            _mock.Setup(s => s.get_filter_roleAsync(It.IsAny<RoleFilterQOBD>()))
               .Returns((RoleFilterQOBD role) => get_filter_roleAsync(role));
            _mock.Setup(s => s.get_filter_role(It.IsAny<RoleFilterQOBD>()))
               .Returns((RoleFilterQOBD role) => get_filter_role(role));

            // update_data_role
            _mock.Setup(s => s.update_data_roleAsync(It.IsAny<RoleQOBD[]>()))
               .Returns((RoleQOBD[] roles) => update_data_roleAsync(roles));
            _mock.Setup(s => s.update_data_role(It.IsAny<RoleQOBD[]>()))
               .Returns((RoleQOBD[] roles) => update_data_role(roles));
            #endregion
            #region [ Role_action ]
            //----------------[ Role_action ]

            // insert_data_role_action
            _mock.Setup(s => s.insert_data_role_actionAsync(It.IsAny<Role_actionQOBD[]>()))
               .Returns((Role_actionQOBD[] role_actions) => insert_data_role_actionAsync(role_actions));
            _mock.Setup(s => s.insert_data_role_action(It.IsAny<Role_actionQOBD[]>()))
               .Returns((Role_actionQOBD[] role_actions) => insert_data_role_action(role_actions));

            // delete_data_role_action
            _mock.Setup(s => s.delete_data_role_actionAsync(It.IsAny<Role_actionQOBD[]>()))
               .Returns((Role_actionQOBD[] role_actions) => delete_data_role_actionAsync(role_actions));
            _mock.Setup(s => s.delete_data_role_action(It.IsAny<Role_actionQOBD[]>()))
               .Returns((Role_actionQOBD[] role_actions) => delete_data_role_action(role_actions));

            // get_data_role_action
            _mock.Setup(s => s.get_data_role_actionAsync(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_role_actionAsync(nbLine));
            _mock.Setup(s => s.get_data_role_action(It.IsAny<string>()))
               .Returns((string nbLine) => get_data_role_action(nbLine));

            // get_data_role_actiont_by_id
            _mock.Setup(s => s.get_data_role_action_by_idAsync(It.IsAny<string>()))
               .Returns((string id) => get_data_role_action_by_idAsync(id));
            _mock.Setup(s => s.get_data_role_action_by_id(It.IsAny<string>()))
               .Returns((string id) => get_data_role_action_by_id(id));
            
            // get_filter_role_action
            _mock.Setup(s => s.get_filter_role_actionAsync(It.IsAny<Role_actionFilterQOBD>()))
               .Returns((Role_actionFilterQOBD role_action) => get_filter_role_actionAsync(role_action));
            _mock.Setup(s => s.get_filter_role_action(It.IsAny<Role_actionFilterQOBD>()))
               .Returns((Role_actionFilterQOBD role_action) => get_filter_role_action(role_action));

            // update_data_role_action
            _mock.Setup(s => s.update_data_role_actionAsync(It.IsAny<Role_actionQOBD[]>()))
               .Returns((Role_actionQOBD[] role_actions) => update_data_role_actionAsync(role_actions));
            _mock.Setup(s => s.update_data_role_action(It.IsAny<Role_actionQOBD[]>()))
               .Returns((Role_actionQOBD[] role_actions) => update_data_role_action(role_actions));
            #endregion

            #endregion

        }

        #region [ Mock Agent implementation ]
        //================================[ Agent ]===================================================

        
        public AgentQOBD get_authenticate_user(string username, string password)
        {
            return new AgentQOBD { ID = 1, FirstName = Utility.encodeStringToBase64("unitTestFirstname"), LastName = Utility.encodeStringToBase64("unitTestLastName"), Status = Utility.encodeStringToBase64("Active") };
        }

        
        public Task<AgentQOBD> get_authenticate_userAsync(string username, string password)
        {
            return Task.Factory.StartNew(() => { return new AgentQOBD { ID = 1, FirstName = Utility.encodeStringToBase64("unitTestFirstname"), LastName = Utility.encodeStringToBase64("unitTestLastName"), Status = Utility.encodeStringToBase64("Active") }; });
        }

        
        public AgentQOBD[] delete_data_agent(AgentQOBD[] agent_array_list)
        {
            return new AgentQOBD[0];
        }

        
        public Task<AgentQOBD[]> delete_data_agentAsync(AgentQOBD[] agent_array_list)
        {
            return Task.Factory.StartNew(() => { return new AgentQOBD[0]; });
        }

        
        public AgentQOBD[] get_data_agent(string nbLine)
        {
            AgentQOBD[] output = new AgentQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                AgentQOBD AgentQOBD = new AgentQOBD { ID = i, Status = Utility.encodeStringToBase64("status") };
                output[i-1] = AgentQOBD;
            }
            return output;
        }

        
        public Task<AgentQOBD[]> get_data_agentAsync(string nbLine)
        {
            nbLine = Math.Abs( Convert.ToInt32(nbLine)).ToString();
            if (Convert.ToInt32(nbLine) == 999)
                nbLine = 10.ToString();

            return Task.Factory.StartNew(() => 
            {
                AgentQOBD[] output = new AgentQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    AgentQOBD AgentQOBD = new AgentQOBD { ID = i, Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("First Name"), LastName = Utility.encodeStringToBase64("last name"), ListSize = 25, Login = Utility.encodeStringToBase64("user name"), Password = "password" };
                    output[i-1] = AgentQOBD;
                }
                return output;
            });
        }

        
        public AgentQOBD[] get_data_agent_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_agent(command_array_list.Count().ToString());
        }

        
        public Task<AgentQOBD[]> get_data_agent_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_agentAsync(command_array_list.Count().ToString());
        }

        
        public AgentQOBD[] get_data_agent_by_id(string id)
        {
            AgentQOBD[] output = new AgentQOBD[1];
            AgentQOBD AgentQOBD = new AgentQOBD { ID = Convert.ToInt32(id), Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("First Name"), LastName = Utility.encodeStringToBase64("last name"), ListSize = 25, Login = Utility.encodeStringToBase64("user name"), Password = "password" };
            output[0] = AgentQOBD;

            return output;
        }

        
        public Task<AgentQOBD[]> get_data_agent_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                AgentQOBD[] output = new AgentQOBD[1];
                AgentQOBD AgentQOBD = new AgentQOBD { ID = Convert.ToInt32(id), Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("First Name"), LastName = Utility.encodeStringToBase64("last name"), ListSize = 25, Login = Utility.encodeStringToBase64("user name"), Password = "password" };
                output[0] = AgentQOBD;

                return output;
            });
        }

        
        public AgentQOBD[] get_data_agent_credentail(string nbLine)
        {
            AgentQOBD[] output = new AgentQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                AgentQOBD AgentQOBD = new AgentQOBD { ID = i, Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("First Name"), LastName = Utility.encodeStringToBase64("last name"), ListSize = 25, Login = Utility.encodeStringToBase64("user name"), Password = "password" };
                output[i-1] = AgentQOBD;
            }
            return output;
        }

        
        public Task<AgentQOBD[]> get_data_agent_credentailAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                AgentQOBD[] output = new AgentQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    AgentQOBD AgentQOBD = new AgentQOBD { ID = i, Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("First Name"), LastName = Utility.encodeStringToBase64("last name"), ListSize = 25, Login = Utility.encodeStringToBase64("user name"), Password = "password" };
                    output[i-1] = AgentQOBD;
                }
                return output;
            });
        }

        
        public AgentQOBD[] get_filter_agent(AgentFilterQOBD agent_array_list_filter)
        {
            return new AgentQOBD[1] { new AgentQOBD { ID = agent_array_list_filter.ID, Login = agent_array_list_filter.Login, Password = agent_array_list_filter.Password, ListSize = agent_array_list_filter.ListSize, FirstName = agent_array_list_filter.FirstName, LastName = agent_array_list_filter.LastName } };
        }

        
        public Task<AgentQOBD[]> get_filter_agentAsync(AgentFilterQOBD agent_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new AgentQOBD[1] { new AgentQOBD { ID = agent_array_list_filter.ID, Login = agent_array_list_filter.Login, Password = agent_array_list_filter.Password, ListSize = agent_array_list_filter.ListSize, FirstName = agent_array_list_filter.FirstName, LastName = agent_array_list_filter.LastName } };
            });
        }

        
        public AgentQOBD[] insert_data_agent(AgentQOBD[] agent_array_list)
        {
            return get_data_agent(agent_array_list.Count().ToString());
        }

        
        public Task<AgentQOBD[]> insert_data_agentAsync(AgentQOBD[] agent_array_list)
        {
            return get_data_agentAsync(agent_array_list.Count().ToString());
        }

        
        public AgentQOBD[] update_data_agent(AgentQOBD[] agent_array_list)
        {
            return get_data_agent(agent_array_list.Count().ToString());
        }

        
        public Task<AgentQOBD[]> update_data_agentAsync(AgentQOBD[] agent_array_list)
        {
            return get_data_agentAsync(agent_array_list.Count().ToString());
        }

        #endregion

        #region [ Mock Client implementation ]
        //================================[ Client ]===================================================

        public ClientQOBD[] delete_data_client(ClientQOBD[] client_array_list)
        {
            return new ClientQOBD[0];
        }
        
        public Task<ClientQOBD[]> delete_data_clientAsync(ClientQOBD[] client_array_list)
        {
            return Task.Factory.StartNew(() => { return new ClientQOBD[0]; });
        }
        
        public AddressQOBD[] delete_data_address(AddressQOBD[] address_array_list)
        {
            return new AddressQOBD[0];
        }
        
        public Task<AddressQOBD[]> delete_data_addressAsync(AddressQOBD[] address_array_list)
        {
            return Task.Factory.StartNew(() => { return new AddressQOBD[0]; });
        }
        
        public ContactQOBD[] delete_data_contact(ContactQOBD[] contact_array_list)
        {
            return new ContactQOBD[0];
        }
        
        public Task<ContactQOBD[]> delete_data_contactAsync(ContactQOBD[] contact_array_list)
        {
            return Task.Factory.StartNew(() => { return new ContactQOBD[0]; });
        }
        
        public AddressQOBD[] get_data_address(string nbLine)
        {
            AddressQOBD[] output = new AddressQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                AddressQOBD AddressQOBD = new AddressQOBD { ID = i, ClientId = i + 1, FirstName = Utility.encodeStringToBase64("First name"), Name = Utility.encodeStringToBase64("name"), LastName = Utility.encodeStringToBase64("last name"), Name2 = Utility.encodeStringToBase64("name2") };
                output[i-1] = AddressQOBD;
            }
            return output;
        }
        
        public Task<AddressQOBD[]> get_data_addressAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                AddressQOBD[] output = new AddressQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    AddressQOBD AddressQOBD = new AddressQOBD { ID = i, ClientId = i + 1, FirstName = Utility.encodeStringToBase64("First name"), Name = Utility.encodeStringToBase64("name"), LastName = Utility.encodeStringToBase64("last name"), Name2 = Utility.encodeStringToBase64("name2") };
                    output[i-1] = AddressQOBD;
                }
                return output;
            });
        }
        
        public AddressQOBD[] get_data_address_by_client_list(ClientQOBD[] client_array_list)
        {
            return get_data_address(client_array_list.Count().ToString());
        }
        
        public Task<AddressQOBD[]> get_data_address_by_client_listAsync(ClientQOBD[] client_array_list)
        {
            return get_data_addressAsync(client_array_list.Count().ToString());
        }
        
        public AddressQOBD[] get_data_address_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_address(command_array_list.Count().ToString());
        }
        
        public Task<AddressQOBD[]> get_data_address_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_addressAsync(command_array_list.Count().ToString());
        }
        
        public AddressQOBD[] get_data_address_by_id(string id)
        {
            AddressQOBD[] output = new AddressQOBD[1];
            int i = 1;
            AddressQOBD AddressQOBD = new AddressQOBD { ID = Convert.ToInt32(id), ClientId = i + 1, FirstName = Utility.encodeStringToBase64("First name"), Name = Utility.encodeStringToBase64("name"), LastName = Utility.encodeStringToBase64("last name"), Name2 = Utility.encodeStringToBase64("name2") };
            output[i-1] = AddressQOBD;

            return output;
        }
        
        public Task<AddressQOBD[]> get_data_address_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                AddressQOBD[] output = new AddressQOBD[1];
                int i = 1;
                AddressQOBD AddressQOBD = new AddressQOBD { ID = Convert.ToInt32(id), ClientId = i +1, FirstName = Utility.encodeStringToBase64("First name"), Name = Utility.encodeStringToBase64("name"), LastName = Utility.encodeStringToBase64("last name"), Name2 = Utility.encodeStringToBase64("name2") };
                output[i-1] = AddressQOBD;

                return output;
            });
        }
        
        public ClientQOBD[] get_data_client(string nbLine)
        {
            ClientQOBD[] output = new ClientQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                ClientQOBD ClientQOBD = new ClientQOBD { ID = i, AgentId = i + 1, MaxCredit = i + 1 + "", Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("FirstName"), LastName = Utility.encodeStringToBase64("LastName") };
                output[i-1] = ClientQOBD;
            }
            return output;
        }
        
        public Task<ClientQOBD[]> get_data_clientAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                ClientQOBD[] output = new ClientQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    ClientQOBD ClientQOBD = new ClientQOBD { ID = i, AgentId = i + 1, MaxCredit = i + 1 + "", Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("FirstName"), LastName = Utility.encodeStringToBase64("LastName") };
                    output[i-1] = ClientQOBD;
                }
                return output;
            });
        }
        
        public ClientQOBD[] get_data_client_by_bill_list(BillQOBD[] bill_array_list)
        {
            return get_data_client(bill_array_list.Count().ToString());
        }
        
        public Task<ClientQOBD[]> get_data_client_by_bill_listAsync(BillQOBD[] bill_array_list)
        {
            return get_data_clientAsync(bill_array_list.Count().ToString());
        }
        
        public ClientQOBD[] get_data_client_by_id(string id)
        {
            ClientQOBD[] output = new ClientQOBD[1];
            int i = 1;
            ClientQOBD ClientQOBD = new ClientQOBD { ID = i, AgentId = i + 1, MaxCredit = i + 1 + "", Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("FirstName"), LastName = Utility.encodeStringToBase64("LastName") };
            output[i-1] = ClientQOBD;

            return output;
        }
        
        public Task<ClientQOBD[]> get_data_client_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ClientQOBD[] output = new ClientQOBD[1];
                int i = 1;
                ClientQOBD ClientQOBD = new ClientQOBD { ID = i, AgentId = i + 1, MaxCredit = i + 1 + "", Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("FirstName"), LastName = Utility.encodeStringToBase64("LastName") };
                output[i-1] = ClientQOBD;

                return output;
            });
        }
        
        public ClientQOBD[] get_data_client_by_max_credit_over(int agent_id)
        {
            ClientQOBD[] output = new ClientQOBD[1];
            int i = 1;
            ClientQOBD ClientQOBD = new ClientQOBD { ID = i, AgentId = agent_id, MaxCredit = i + 1 + "", Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("FirstName"), LastName = Utility.encodeStringToBase64("LastName") };
            output[i - 1] = ClientQOBD;

            return output;
        }
        
        public Task<ClientQOBD[]> get_data_client_by_max_credit_overAsync(int agent_id)
        {
            return Task.Factory.StartNew(() => {
                ClientQOBD[] output = new ClientQOBD[1];
                int i = 1;
                ClientQOBD ClientQOBD = new ClientQOBD { ID = i, AgentId = agent_id, MaxCredit = i + 1 + "", Status = Utility.encodeStringToBase64("status"), FirstName = Utility.encodeStringToBase64("FirstName"), LastName = Utility.encodeStringToBase64("LastName") };
                output[i - 1] = ClientQOBD;

                return output;
            });
        }
        
        public ContactQOBD[] get_data_contact(string nbLine)
        {
            ContactQOBD[] output = new ContactQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                ContactQOBD ContactQOBD = new ContactQOBD { ID = i, ClientId = i + 1, Firstname = Utility.encodeStringToBase64("first name"), LastName = Utility.encodeStringToBase64("last name") };
                output[i-1] = ContactQOBD;
            }
            return output;
        }
        
        public Task<ContactQOBD[]> get_data_contactAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                ContactQOBD[] output = new ContactQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    ContactQOBD ContactQOBD = new ContactQOBD { ID = i, ClientId = i + 1, Firstname = Utility.encodeStringToBase64("first name"), LastName = Utility.encodeStringToBase64("last name") };
                    output[i-1] = ContactQOBD;
                }
                return output;
            });
        }
        
        public ContactQOBD[] get_data_contact_by_client_list(ClientQOBD[] client_array_list)
        {
            return get_data_contact(client_array_list.Count().ToString());
        }
        
        public Task<ContactQOBD[]> get_data_contact_by_client_listAsync(ClientQOBD[] client_array_list)
        {
            return get_data_contactAsync(client_array_list.Count().ToString());
        }
        
        public ContactQOBD[] get_data_contact_by_id(string id)
        {
            ContactQOBD[] output = new ContactQOBD[1];
            int i = 1;
            ContactQOBD ContactQOBD = new ContactQOBD { ID = i, ClientId = i + 1, Firstname = Utility.encodeStringToBase64("first name"), LastName = Utility.encodeStringToBase64("last name") };
            output[i-1] = ContactQOBD;

            return output;
        }
        
        public Task<ContactQOBD[]> get_data_contact_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ContactQOBD[] output = new ContactQOBD[1];
                int i = 1;
                ContactQOBD ContactQOBD = new ContactQOBD { ID = i, ClientId = i + 1, Firstname = Utility.encodeStringToBase64("first name"), LastName = Utility.encodeStringToBase64("last name") };
                output[i-1] = ContactQOBD;

                return output;
            });
        }
        
        public AddressQOBD[] get_filter_address(AddressFilterQOBD address_array_list)
        {
            return new AddressQOBD[1] { new AddressQOBD { ID = address_array_list.ID, ClientId = address_array_list.ClientId, Name = address_array_list.Name } };
        }
        
        public Task<AddressQOBD[]> get_filter_addressAsync(AddressFilterQOBD address_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new AddressQOBD[1] { new AddressQOBD { ID = address_array_list.ID, ClientId = address_array_list.ClientId, Name = address_array_list.Name, Name2 = address_array_list.Name2 } };
            });
        }
        
        public ClientQOBD[] get_filter_Client(ClientFilterQOBD client_array_list_filter)
        {
            return new ClientQOBD[1] { new ClientQOBD { ID = client_array_list_filter.ID, AgentId = client_array_list_filter.AgentId, Company = client_array_list_filter.Company, MaxCredit = client_array_list_filter.MaxCredit } };
        }
        
        public Task<ClientQOBD[]> get_filter_ClientAsync(ClientFilterQOBD client_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ClientQOBD[1] { new ClientQOBD { ID = client_array_list_filter.ID, AgentId = client_array_list_filter.AgentId, Company = client_array_list_filter.Company, MaxCredit = client_array_list_filter.MaxCredit } };
            });
        }
        
        public ContactQOBD[] get_filter_contact(ContactFilterQOBD contact_array_list_filter)
        {
            return new ContactQOBD[1] { new ContactQOBD { ID = contact_array_list_filter.ID, ClientId = contact_array_list_filter.ClientId, Firstname = contact_array_list_filter.Firstname } };
        }
        
        public Task<ContactQOBD[]> get_filter_contactAsync(ContactFilterQOBD contact_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ContactQOBD[1] { new ContactQOBD { ID = contact_array_list_filter.ID, ClientId = contact_array_list_filter.ClientId, Firstname = contact_array_list_filter.Firstname } };
            });
        }
        
        public AddressQOBD[] insert_data_address(AddressQOBD[] address_array_list)
        {
            return get_data_address(address_array_list.Count().ToString());
        }
        
        public Task<AddressQOBD[]> insert_data_addressAsync(AddressQOBD[] address_array_list)
        {
            return get_data_addressAsync(address_array_list.Count().ToString());
        }
        
        public ClientQOBD[] insert_data_client(ClientQOBD[] client_array_list)
        {
            return get_data_client(client_array_list.Count().ToString());
        }
        
        public Task<ClientQOBD[]> insert_data_clientAsync(ClientQOBD[] client_array_list)
        {
            return get_data_clientAsync(client_array_list.Count().ToString());
        }
        
        public ContactQOBD[] insert_data_contact(ContactQOBD[] contact_array_list)
        {
            return get_data_contact(contact_array_list.Count().ToString());
        }
        
        public Task<ContactQOBD[]> insert_data_contactAsync(ContactQOBD[] contact_array_list)
        {
            return get_data_contactAsync(contact_array_list.Count().ToString());
        }
        
        public string send_email_to_client(EmailQOBD client_email)
        {
            return "";
        }
        
        public Task<string> send_email_to_clientAsync(EmailQOBD client_email)
        {
            return Task.Factory.StartNew(()=> { return ""; });
        }
        
        public AddressQOBD[] update_data_address(AddressQOBD[] address_array_list)
        {
            return get_data_address(address_array_list.Count().ToString());
        }
        
        public Task<AddressQOBD[]> update_data_addressAsync(AddressQOBD[] address_array_list)
        {
            return get_data_addressAsync(address_array_list.Count().ToString());
        }
        
        public ClientQOBD[] update_data_client(ClientQOBD[] client_array_list)
        {
            return get_data_client(client_array_list.Count().ToString());
        }
        
        public Task<ClientQOBD[]> update_data_clientAsync(ClientQOBD[] client_array_list)
        {
            return get_data_clientAsync(client_array_list.Count().ToString());
        }
        
        public ContactQOBD[] update_data_contact(ContactQOBD[] contact_array_list)
        {
            return get_data_contact(contact_array_list.Count().ToString());
        }
        
        public Task<ContactQOBD[]> update_data_contactAsync(ContactQOBD[] contact_array_list)
        {
            return get_data_contactAsync(contact_array_list.Count().ToString());
        }

        #endregion

        #region [ Mock Order implementation ]
        //================================[ Order ]===================================================

        
        public BillQOBD[] delete_data_bill(BillQOBD[] bill_array_list)
        {
            return new BillQOBD[0];
        }

        
        public Task<BillQOBD[]> delete_data_billAsync(BillQOBD[] bill_array_list)
        {
            return Task.Factory.StartNew(() => { return new BillQOBD[0]; });
        }

        
        public CommandsQOBD[] delete_data_command(CommandsQOBD[] command_array_list)
        {
            return new CommandsQOBD[0];
        }

        
        public Task<CommandsQOBD[]> delete_data_commandAsync(CommandsQOBD[] command_array_list)
        {
            return Task.Factory.StartNew(() => { return new CommandsQOBD[0]; });
        }

        
        public Command_itemQOBD[] delete_data_command_item(Command_itemQOBD[] command_item_array_list)
        {
            return new Command_itemQOBD[0];
        }

        
        public Task<Command_itemQOBD[]> delete_data_command_itemAsync(Command_itemQOBD[] command_item_array_list)
        {
            return Task.Factory.StartNew(() => { return new Command_itemQOBD[0]; });
        }

        
        public DeliveryQOBD[] delete_data_delivery(DeliveryQOBD[] delivery_array_list)
        {
            return new DeliveryQOBD[0];
        }

        
        public Task<DeliveryQOBD[]> delete_data_deliveryAsync(DeliveryQOBD[] delivery_array_list)
        {
            return Task.Factory.StartNew(() => { return new DeliveryQOBD[0]; });
        }

        
        public TaxQOBD[] delete_data_tax(TaxQOBD[] tax_array_list)
        {
            return new TaxQOBD[0];
        }

        
        public Task<TaxQOBD[]> delete_data_taxAsync(TaxQOBD[] tax_array_list)
        {
            return Task.Factory.StartNew(() => { return new TaxQOBD[0]; });
        }

        
        public Tax_commandQOBD[] delete_data_tax_command(Tax_commandQOBD[] tax_command_array_list)
        {
            return new Tax_commandQOBD[0];
        }

        
        public Task<Tax_commandQOBD[]> delete_data_tax_commandAsync(Tax_commandQOBD[] tax_command_array_list)
        {
            return Task.Factory.StartNew(() => { return new Tax_commandQOBD[0]; });
        }

        public void generate_pdf(PdfQOBD command_array)
        {

        }

        public Task generate_pdfAsync(PdfQOBD command_array)
        {
            return Task.Factory.StartNew(() => { });
        }

        
        public CommandsQOBD[] get_commands_client(string id)
        {
            CommandsQOBD[] output = new CommandsQOBD[1];
            int i = 1;
            CommandsQOBD CommandsQOBD = new CommandsQOBD { ID = Convert.ToInt32(id), AgentId = i, ClientId = i, Status = Utility.encodeStringToBase64("status"), Date = Utility.encodeStringToBase64(DateTime.Now+""), BillAddress = 1, DeliveryAddress = 1 };
            output[i-1] = CommandsQOBD;

            return output;
        }

        
        public Task<CommandsQOBD[]> get_commands_clientAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                CommandsQOBD[] output = new CommandsQOBD[1];
                int i = 1;
                CommandsQOBD CommandsQOBD = new CommandsQOBD { ID = Convert.ToInt32(id), AgentId = i, ClientId = i, Status = Utility.encodeStringToBase64("status"), Date = Utility.encodeStringToBase64(DateTime.Now + ""), BillAddress = 1, DeliveryAddress = 1 };
                output[i-1] = CommandsQOBD;

                return output;
            });
        }

        
        public BillQOBD[] get_data_bill(string nbLine)
        {
            BillQOBD[] output = new BillQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                BillQOBD BillQOBD = new BillQOBD { ID = i, ClientId = i, CommandId = i };
                output[i-1] = BillQOBD;
            }
            return output;
        }

        
        public Task<BillQOBD[]> get_data_billAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                BillQOBD[] output = new BillQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    BillQOBD BillQOBD = new BillQOBD { ID = i, ClientId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now+""), DateLimit = Utility.encodeStringToBase64(DateTime.Now + ""), DatePay = Utility.encodeStringToBase64(DateTime.Now + ""), PayReceived = 20, Pay = 50 };
                    output[i-1] = BillQOBD;
                }
                return output;
            });
        }

        
        public BillQOBD[] get_data_bill_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_bill(command_array_list.Count().ToString());
        }

        
        public Task<BillQOBD[]> get_data_bill_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_billAsync(command_array_list.Count().ToString());
        }

        
        public BillQOBD[] get_data_bill_by_id(string id)
        {
            BillQOBD[] output = new BillQOBD[1];
            int i = 1;
            BillQOBD BillQOBD = new BillQOBD { ID = Convert.ToInt32(id), ClientId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now + ""), DateLimit = Utility.encodeStringToBase64(DateTime.Now + ""), DatePay = Utility.encodeStringToBase64(DateTime.Now + ""), PayReceived = 20, Pay = 50 };
            output[i-1] = BillQOBD;

            return output;
        }

        
        public Task<BillQOBD[]> get_data_bill_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                BillQOBD[] output = new BillQOBD[1];
                int i = 1;
                BillQOBD BillQOBD = new BillQOBD { ID = Convert.ToInt32(id), ClientId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now + ""), DateLimit = Utility.encodeStringToBase64(DateTime.Now + ""), DatePay = Utility.encodeStringToBase64(DateTime.Now + ""), PayReceived = 20, Pay = 50 };
                output[i-1] = BillQOBD;

                return output;
            });
        }

        
        public BillQOBD[] get_data_bill_by_unpaid(int agent_id)
        {
            return get_data_bill(1.ToString());
        }

        
        public Task<BillQOBD[]> get_data_bill_by_unpaidAsync(int agent_id)
        {
            return get_data_billAsync(1.ToString());
        }

        
        public CommandsQOBD[] get_data_command(string nbLine)
        {
            CommandsQOBD[] output = new CommandsQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                CommandsQOBD CommandsQOBD = new CommandsQOBD { ID = i, AgentId = i, ClientId = i, Status = Utility.encodeStringToBase64("Order"), Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                output[i-1] = CommandsQOBD;
            }
            return output;
        }

        
        public Task<CommandsQOBD[]> get_data_commandAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                CommandsQOBD[] output = new CommandsQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    output[i-1] = new CommandsQOBD { ID = i, AgentId = i, ClientId = i, Status = Utility.encodeStringToBase64("Order"), Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                }
                return output;
            });
        }

        
        public CommandsQOBD[] get_data_command_by_id(string id)
        {
            CommandsQOBD[] output = new CommandsQOBD[1];
            int i = 1;
            CommandsQOBD CommandsQOBD = new CommandsQOBD { ID = Convert.ToInt32(id), AgentId = i, ClientId = i, Status = Utility.encodeStringToBase64("Order"), Date = Utility.encodeStringToBase64(DateTime.Now + "") };
            output[i-1] = CommandsQOBD;

            return output;
        }

        
        public Task<CommandsQOBD[]> get_data_command_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                CommandsQOBD[] output = new CommandsQOBD[1];
                int i = 1;
                CommandsQOBD CommandsQOBD = new CommandsQOBD { ID = Convert.ToInt32(id), AgentId = i, ClientId = i, Status = Utility.encodeStringToBase64("Order"), Date = Utility.encodeStringToBase64(DateTime.Now+"") };
                output[i-1] = CommandsQOBD;

                return output;
            });
        }

        
        public Command_itemQOBD[] get_data_command_item(string nbLine)
        {
            Command_itemQOBD[] output = new Command_itemQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Command_itemQOBD Command_itemQOBD = new Command_itemQOBD { ID = i, CommandId = i, Item_ref = Utility.encodeStringToBase64("item ref"), ItemId = 1, Price = 10, Price_purchase = 5 };
                output[i-1] = Command_itemQOBD;
            }
            return output;
        }

        
        public Task<Command_itemQOBD[]> get_data_command_itemAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Command_itemQOBD[] output = new Command_itemQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Command_itemQOBD Command_itemQOBD = new Command_itemQOBD { ID = i, CommandId = i, Item_ref = Utility.encodeStringToBase64("item ref"), ItemId = 1, Price = 10, Price_purchase = 5 };
                    output[i-1] = Command_itemQOBD;
                }
                return output;
            });
        }

        
        public Command_itemQOBD[] get_data_command_item_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_command_item(command_array_list.Count().ToString());
        }

        
        public Task<Command_itemQOBD[]> get_data_command_item_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_command_itemAsync(command_array_list.Count().ToString());
        }

        
        public Command_itemQOBD[] get_data_command_item_by_id(string id)
        {
            Command_itemQOBD[] output = new Command_itemQOBD[1];
            int i = 1;
            Command_itemQOBD Command_itemQOBD = new Command_itemQOBD { ID = Convert.ToInt32(id), CommandId = i, Item_ref = Utility.encodeStringToBase64("item ref"), ItemId = 1, Price = 10, Price_purchase = 5 };
            output[i-1] = Command_itemQOBD;

            return output;
        }

        
        public Task<Command_itemQOBD[]> get_data_command_item_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Command_itemQOBD[] output = new Command_itemQOBD[1];
                int i = 1;
                Command_itemQOBD Command_itemQOBD = new Command_itemQOBD { ID = Convert.ToInt32(id), CommandId = i, Item_ref = Utility.encodeStringToBase64("item ref"), ItemId = 1, Price = 10, Price_purchase = 5 };
                output[i-1] = Command_itemQOBD;

                return output;
            });
        }

        
        public DeliveryQOBD[] get_data_delivery(string nbLine)
        {
            DeliveryQOBD[] output = new DeliveryQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                DeliveryQOBD DeliveryQOBD = new DeliveryQOBD { ID = i, BillId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now+"") };
                output[i-1] = DeliveryQOBD;
            }
            return output;
        }

        
        public Task<DeliveryQOBD[]> get_data_deliveryAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                DeliveryQOBD[] output = new DeliveryQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    DeliveryQOBD DeliveryQOBD = new DeliveryQOBD { ID = i, BillId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                    output[i-1] = DeliveryQOBD;
                }
                return output;
            });
        }

        
        public DeliveryQOBD[] get_data_delivery_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_delivery(command_array_list.Count().ToString());
        }

        
        public Task<DeliveryQOBD[]> get_data_delivery_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_deliveryAsync(command_array_list.Count().ToString());
        }

        
        public DeliveryQOBD[] get_data_delivery_by_id(string id)
        {
            DeliveryQOBD[] output = new DeliveryQOBD[1];
            int i = 1;
            DeliveryQOBD DeliveryQOBD = new DeliveryQOBD { ID = Convert.ToInt32(id), BillId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now + "") };
            output[i-1] = DeliveryQOBD;

            return output;
        }

        
        public Task<DeliveryQOBD[]> get_data_delivery_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                DeliveryQOBD[] output = new DeliveryQOBD[1];
                int i = 1;
                DeliveryQOBD DeliveryQOBD = new DeliveryQOBD { ID = Convert.ToInt32(id), BillId = i, CommandId = i, Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                output[i-1] = DeliveryQOBD;

                return output;
            });
        }

        
        public TaxQOBD[] get_data_tax(string nbLine)
        {
            TaxQOBD[] output = new TaxQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                TaxQOBD TaxQOBD = new TaxQOBD { ID = i, Value = i, Type = Utility.encodeStringToBase64("type "), Date_insert = Utility.encodeStringToBase64(DateTime.Now+""), Tax_current = i };
                output[i-1] = TaxQOBD;
            }
            return output;
        }

        
        public Task<TaxQOBD[]> get_data_taxAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                TaxQOBD[] output = new TaxQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    TaxQOBD TaxQOBD = new TaxQOBD { ID = i, Value = i, Type = Utility.encodeStringToBase64("type "), Date_insert = Utility.encodeStringToBase64(DateTime.Now + ""), Tax_current = i };
                    output[i-1] = TaxQOBD;
                }
                return output;
            });
        }

        
        public TaxQOBD[] get_data_tax_by_id(string id)
        {
            TaxQOBD[] output = new TaxQOBD[1];
            int i = 1;
            TaxQOBD TaxQOBD = new TaxQOBD { ID = Convert.ToInt32(id), Value = i, Type = Utility.encodeStringToBase64("type "), Date_insert = Utility.encodeStringToBase64(DateTime.Now + ""), Tax_current = i };
            output[i-1] = TaxQOBD;

            return output;
        }

        
        public Task<TaxQOBD[]> get_data_tax_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                TaxQOBD[] output = new TaxQOBD[1];
                int i = 1;
                TaxQOBD TaxQOBD = new TaxQOBD { ID = Convert.ToInt32(id), Value = i, Type = Utility.encodeStringToBase64("type "), Date_insert = Utility.encodeStringToBase64(DateTime.Now + ""), Tax_current = i };
                output[i-1] = TaxQOBD;

                return output;
            });
        }

        
        public Tax_commandQOBD[] get_data_tax_command(string nbLine)
        {
            Tax_commandQOBD[] output = new Tax_commandQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Tax_commandQOBD Tax_commandQOBD = new Tax_commandQOBD { ID = i, CommandId = i, Target = Utility.encodeStringToBase64("target ") + i, TaxId = i, Date_insert = Utility.encodeStringToBase64(DateTime.Now+""), Tax_value = 20 };
                output[i-1] = Tax_commandQOBD;
            }
            return output;
        }

        
        public Task<Tax_commandQOBD[]> get_data_tax_commandAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Tax_commandQOBD[] output = new Tax_commandQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Tax_commandQOBD Tax_commandQOBD = new Tax_commandQOBD { ID = i, CommandId = i, Target = Utility.encodeStringToBase64("target ") + i, TaxId = i, Date_insert = Utility.encodeStringToBase64(DateTime.Now + ""), Tax_value = 20 };
                    output[i-1] = Tax_commandQOBD;
                }
                return output;
            });
        }

        
        public Tax_commandQOBD[] get_data_tax_command_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_tax_command(command_array_list.Count().ToString());
        }

        
        public Task<Tax_commandQOBD[]> get_data_tax_command_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_tax_commandAsync(command_array_list.Count().ToString());
        }

        
        public Tax_commandQOBD[] get_data_tax_command_by_id(string id)
        {
            Tax_commandQOBD[] output = new Tax_commandQOBD[1];
            int i = 1;
            Tax_commandQOBD Tax_commandQOBD = new Tax_commandQOBD { ID = Convert.ToInt32(id), CommandId = i, Target = Utility.encodeStringToBase64("target ") + i, TaxId = i, Date_insert = Utility.encodeStringToBase64(DateTime.Now + ""), Tax_value = 20 };
            output[i-1] = Tax_commandQOBD;

            return output;
        }

        
        public Task<Tax_commandQOBD[]> get_data_tax_command_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Tax_commandQOBD[] output = new Tax_commandQOBD[1];
                int i = 1;
                Tax_commandQOBD Tax_commandQOBD = new Tax_commandQOBD { ID = Convert.ToInt32(id), CommandId = i, Target = Utility.encodeStringToBase64("target ") + i, TaxId = i, Date_insert = Utility.encodeStringToBase64(DateTime.Now + ""), Tax_value = 20 };
                output[i-1] = Tax_commandQOBD;

                return output;
            });
        }

        
        public BillQOBD[] get_filter_bill(BillFilterQOBD bill_array_list)
        {
            return new BillQOBD[1] { new BillQOBD { ID = bill_array_list.ID, ClientId = bill_array_list.ClientId, CommandId = bill_array_list.CommandId, Date = bill_array_list.Date, DateLimit = bill_array_list.DateLimit, DatePay = bill_array_list.DatePay, Pay = bill_array_list.Pay } };
        }

        
        public Task<BillQOBD[]> get_filter_billAsync(BillFilterQOBD bill_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new BillQOBD[1] { new BillQOBD { ID = bill_array_list.ID, ClientId = bill_array_list.ClientId, CommandId = bill_array_list.CommandId, Date = bill_array_list.Date, DateLimit = bill_array_list.DateLimit, DatePay = bill_array_list.DatePay, Pay = bill_array_list.Pay } };
            });
        }

        
        public CommandsQOBD[] get_filter_command(CommandFilterQOBD command_array_list_filter)
        {
            return new CommandsQOBD[1] { new CommandsQOBD { ID = command_array_list_filter.ID, AgentId = command_array_list_filter.AgentId, ClientId = command_array_list_filter.ClientId, Date = command_array_list_filter.Date, Status = command_array_list_filter.Status, BillAddress = command_array_list_filter.BillAddress, DeliveryAddress = command_array_list_filter.DeliveryAddress } };
        }

        
        public Task<CommandsQOBD[]> get_filter_commandAsync(CommandFilterQOBD command_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new CommandsQOBD[1] { new CommandsQOBD { ID = command_array_list_filter.ID, AgentId = command_array_list_filter.AgentId, ClientId = command_array_list_filter.ClientId, Date = command_array_list_filter.Date, Status = command_array_list_filter.Status, BillAddress = command_array_list_filter.BillAddress, DeliveryAddress = command_array_list_filter.DeliveryAddress } };
            });
        }

        
        public Command_itemQOBD[] get_filter_command_item(Command_itemFilterQOBD command_item_array_list_filter)
        {
            return new Command_itemQOBD[1] { new Command_itemQOBD { ID = command_item_array_list_filter.ID, CommandId = command_item_array_list_filter.CommandId, Item_ref = command_item_array_list_filter.Item_ref, ItemId = command_item_array_list_filter.ItemId, Price = command_item_array_list_filter.Price, Price_purchase = command_item_array_list_filter.Price_purchase, Quantity = command_item_array_list_filter.Quantity } };
        }

        
        public Task<Command_itemQOBD[]> get_filter_command_itemAsync(Command_itemFilterQOBD command_item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Command_itemQOBD[1] { new Command_itemQOBD { ID = command_item_array_list_filter.ID, CommandId = command_item_array_list_filter.CommandId, Item_ref = command_item_array_list_filter.Item_ref, ItemId = command_item_array_list_filter.ItemId, Price = command_item_array_list_filter.Price, Price_purchase = command_item_array_list_filter.Price_purchase, Quantity = command_item_array_list_filter.Quantity } };
            });
        }

        
        public DeliveryQOBD[] get_filter_delivery(DeliveryFilterQOBD delivery_array_list_filter)
        {
            return new DeliveryQOBD[1] { new DeliveryQOBD { ID = delivery_array_list_filter.ID, BillId = delivery_array_list_filter.BillId, CommandId = delivery_array_list_filter.CommandId, Date = delivery_array_list_filter.Date } };
        }

        
        public Task<DeliveryQOBD[]> get_filter_deliveryAsync(DeliveryFilterQOBD delivery_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new DeliveryQOBD[1] { new DeliveryQOBD { ID = delivery_array_list_filter.ID, BillId = delivery_array_list_filter.BillId, CommandId = delivery_array_list_filter.CommandId, Date = delivery_array_list_filter.Date } };
            });
        }

        
        public TaxQOBD[] get_filter_tax(TaxFilterQOBD tax_array_list_filter)
        {
            return new TaxQOBD[1] { new TaxQOBD { ID = tax_array_list_filter.ID, Comment = tax_array_list_filter.Comment, Tax_current = tax_array_list_filter.Tax_current, Type = tax_array_list_filter.Type, Value = tax_array_list_filter.Value, Date_insert = tax_array_list_filter.Date_insert } };
        }

        
        public Task<TaxQOBD[]> get_filter_taxAsync(TaxFilterQOBD tax_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new TaxQOBD[1] { new TaxQOBD { ID = tax_array_list_filter.ID, Comment = tax_array_list_filter.Comment, Tax_current = tax_array_list_filter.Tax_current, Type = tax_array_list_filter.Type, Value = tax_array_list_filter.Value, Date_insert = tax_array_list_filter.Date_insert } };
            });
        }

        
        public Tax_commandQOBD[] get_filter_tax_command(Tax_commandFilterQOBD tax_command_array_list_filter)
        {
            return new Tax_commandQOBD[1] { new Tax_commandQOBD { ID = tax_command_array_list_filter.ID, CommandId = tax_command_array_list_filter.CommandId, TaxId = tax_command_array_list_filter.TaxId, Date_insert = tax_command_array_list_filter.Date_insert, Target = tax_command_array_list_filter.Target } };
        }

        
        public Task<Tax_commandQOBD[]> get_filter_tax_commandAsync(Tax_commandFilterQOBD tax_command_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Tax_commandQOBD[1] { new Tax_commandQOBD { ID = tax_command_array_list_filter.ID, CommandId = tax_command_array_list_filter.CommandId, TaxId = tax_command_array_list_filter.TaxId, Date_insert = tax_command_array_list_filter.Date_insert, Target = tax_command_array_list_filter.Target } };
            });
        }

        
        public CommandsQOBD[] get_quotes_client(string id)
        {
            return new CommandsQOBD[1] { new CommandsQOBD { ClientId = Convert.ToInt32(id), AgentId = 1, Status = Utility.encodeStringToBase64("Order"), Date = Utility.encodeStringToBase64(DateTime.Now + "") } };
        }

        
        public Task<CommandsQOBD[]> get_quotes_clientAsync(string id)
        {
            return Task.Factory.StartNew(() => {

                return new CommandsQOBD[1] { new CommandsQOBD { ClientId = Convert.ToInt32(id), AgentId = 1, Status = Utility.encodeStringToBase64("Order"), Date = Utility.encodeStringToBase64(DateTime.Now + "") } };
            });
        }

        
        public BillQOBD[] insert_data_bill(BillQOBD[] bill_array_list)
        {
            return get_data_bill(bill_array_list.Count().ToString());
        }

        
        public Task<BillQOBD[]> insert_data_billAsync(BillQOBD[] bill_array_list)
        {
            return get_data_billAsync(bill_array_list.Count().ToString());
        }

        
        public CommandsQOBD[] insert_data_command(CommandsQOBD[] command_array_list)
        {
            return get_data_command(command_array_list.Count().ToString());
        }

        
        public Task<CommandsQOBD[]> insert_data_commandAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_commandAsync(command_array_list.Count().ToString());
        }

        
        public Command_itemQOBD[] insert_data_command_item(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_item(command_item_array_list.Count().ToString());
        }

        
        public Task<Command_itemQOBD[]> insert_data_command_itemAsync(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_itemAsync(command_item_array_list.Count().ToString());
        }

        
        public DeliveryQOBD[] insert_data_delivery(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_delivery(delivery_array_list.Count().ToString());
        }

        
        public Task<DeliveryQOBD[]> insert_data_deliveryAsync(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_deliveryAsync(delivery_array_list.Count().ToString());
        }

        
        public TaxQOBD[] insert_data_tax(TaxQOBD[] tax_array_list)
        {
            return get_data_tax(tax_array_list.Count().ToString());
        }

        
        public Task<TaxQOBD[]> insert_data_taxAsync(TaxQOBD[] tax_array_list)
        {
            return get_data_taxAsync(tax_array_list.Count().ToString());
        }

        
        public Tax_commandQOBD[] insert_data_tax_command(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_command(tax_command_array_list.Count().ToString());
        }

        
        public Task<Tax_commandQOBD[]> insert_data_tax_commandAsync(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_commandAsync(tax_command_array_list.Count().ToString());
        }

        
        public BillQOBD[] update_data_bill(BillQOBD[] bill_array_list)
        {
            return get_data_bill(bill_array_list.Count().ToString());
        }

        
        public Task<BillQOBD[]> update_data_billAsync(BillQOBD[] bill_array_list)
        {
            return get_data_billAsync(bill_array_list.Count().ToString());
        }

        
        public CommandsQOBD[] update_data_command(CommandsQOBD[] command_array_list)
        {
            return get_data_command(command_array_list.Count().ToString());
        }

        
        public Task<CommandsQOBD[]> update_data_commandAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_commandAsync(command_array_list.Count().ToString());
        }

        
        public Command_itemQOBD[] update_data_command_item(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_item(command_item_array_list.Count().ToString());
        }

        
        public Task<Command_itemQOBD[]> update_data_command_itemAsync(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_itemAsync(command_item_array_list.Count().ToString());
        }

        
        public DeliveryQOBD[] update_data_delivery(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_delivery(delivery_array_list.Count().ToString());
        }

        
        public Task<DeliveryQOBD[]> update_data_deliveryAsync(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_deliveryAsync(delivery_array_list.Count().ToString());
        }

        
        public TaxQOBD[] update_data_tax(TaxQOBD[] tax_array_list)
        {
            return get_data_tax(tax_array_list.Count().ToString());
        }

        
        public Task<TaxQOBD[]> update_data_taxAsync(TaxQOBD[] tax_array_list)
        {
            return get_data_taxAsync(tax_array_list.Count().ToString());
        }

        
        public Tax_commandQOBD[] update_data_tax_command(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_command(tax_command_array_list.Count().ToString());
        }

        
        public Task<Tax_commandQOBD[]> update_data_tax_commandAsync(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_commandAsync(tax_command_array_list.Count().ToString());
        }

        #endregion

        #region [ Mock Item implementation ]
        //================================[ Item ]===================================================

        
        public Auto_refsQOBD[] delete_data_auto_ref(Auto_refsQOBD[] auto_ref_array_list)
        {
            return new Auto_refsQOBD[0];
        }

        
        public Task<Auto_refsQOBD[]> delete_data_auto_refAsync(Auto_refsQOBD[] auto_ref_array_list)
        {
            return Task.Factory.StartNew(() => { return new Auto_refsQOBD[0]; });
        }
        
        public ItemQOBD[] delete_data_item(ItemQOBD[] item_array_list)
        {
            return new ItemQOBD[0];
        }

        
        public Task<ItemQOBD[]> delete_data_itemAsync(ItemQOBD[] item_array_list)
        {
            return Task.Factory.StartNew(() => { return new ItemQOBD[0]; });
        }

        
        public Item_deliveryQOBD[] delete_data_item_delivery(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return new Item_deliveryQOBD[0];
        }

        
        public Task<Item_deliveryQOBD[]> delete_data_item_deliveryAsync(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return Task.Factory.StartNew(() => { return new Item_deliveryQOBD[0]; });
        }

        
        public ProviderQOBD[] delete_data_provider(ProviderQOBD[] provider_array_list)
        {
            return new ProviderQOBD[0];
        }

        
        public Task<ProviderQOBD[]> delete_data_providerAsync(ProviderQOBD[] provider_array_list)
        {
            return Task.Factory.StartNew(() => { return new ProviderQOBD[0]; });
        }

        
        public Provider_itemQOBD[] delete_data_provider_item(Provider_itemQOBD[] provider_item_array_list)
        {
            return new Provider_itemQOBD[0];
        }

        
        public Task<Provider_itemQOBD[]> delete_data_provider_itemAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return Task.Factory.StartNew(() => { return new Provider_itemQOBD[0]; });
        }

        
        public Tax_itemQOBD[] delete_data_tax_item(Tax_itemQOBD[] tax_item_array_list)
        {
            return new Tax_itemQOBD[0];
        }

        
        public Task<Tax_itemQOBD[]> delete_data_tax_itemAsync(Tax_itemQOBD[] tax_item_array_list)
        {
            return Task.Factory.StartNew(() => { return new Tax_itemQOBD[0]; });
        }

        
        public Auto_refsQOBD[] get_data_auto_ref(string nbLine)
        {
            Auto_refsQOBD[] output = new Auto_refsQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Auto_refsQOBD Auto_refsQOBD = new Auto_refsQOBD { ID = i, RefId = i };
                output[i-1] = Auto_refsQOBD;
            }
            return output;
        }

        
        public Task<Auto_refsQOBD[]> get_data_auto_refAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Auto_refsQOBD[] output = new Auto_refsQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Auto_refsQOBD Auto_refsQOBD = new Auto_refsQOBD { ID = i, RefId = i + 1 };
                    output[i-1] = Auto_refsQOBD;
                }
                return output;
            });
        }

        
        public Auto_refsQOBD[] get_data_auto_ref_by_id(string id)
        {
            Auto_refsQOBD[] output = new Auto_refsQOBD[1];
            int i = 1;
            Auto_refsQOBD Auto_refsQOBD = new Auto_refsQOBD { ID = Convert.ToInt32(id), RefId = i };
            output[i-1] = Auto_refsQOBD;

            return output;
        }

        
        public Task<Auto_refsQOBD[]> get_data_auto_ref_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Auto_refsQOBD[] output = new Auto_refsQOBD[1];
                int i = 1;
                Auto_refsQOBD Auto_refsQOBD = new Auto_refsQOBD { ID = Convert.ToInt32(id), RefId = i };
                output[i-1] = Auto_refsQOBD;

                return output;
            });
        }

        
        public ItemQOBD[] get_data_item(string nbLine)
        {
            ItemQOBD[] output = new ItemQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                ItemQOBD ItemQOBD = new ItemQOBD { ID = i, Source = i + "", Ref = Utility.encodeStringToBase64("ref"), Price_purchase = 10, Price_sell = 20, Type = Utility.encodeStringToBase64("type") };
                output[i-1] = ItemQOBD;
            }
            return output;
        }

        
        public Task<ItemQOBD[]> get_data_itemAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                ItemQOBD[] output = new ItemQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    ItemQOBD ItemQOBD = new ItemQOBD { ID = i, Source = i + "", Ref = Utility.encodeStringToBase64("ref"), Price_purchase = 10, Price_sell = 20, Type = Utility.encodeStringToBase64("type") };
                    output[i-1] = ItemQOBD;
                }
                return output;
            });
        }

        
        public ItemQOBD[] get_data_item_by_command_item_list(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_item(command_item_array_list.Count().ToString());
        }

        
        public Task<ItemQOBD[]> get_data_item_by_command_item_listAsync(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_itemAsync(command_item_array_list.Count().ToString());
        }

        
        public ItemQOBD[] get_data_item_by_id(string id)
        {
            ItemQOBD[] output = new ItemQOBD[1];
            int i = 1;
            ItemQOBD ItemQOBD = new ItemQOBD { ID = Convert.ToInt32(id), Source = i + "", Ref = Utility.encodeStringToBase64("ref"), Price_purchase = 10, Price_sell = 20, Type = Utility.encodeStringToBase64("type") };
            output[i-1] = ItemQOBD;

            return output;
        }

        
        public Task<ItemQOBD[]> get_data_item_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ItemQOBD[] output = new ItemQOBD[1];
                int i = 1;
                ItemQOBD ItemQOBD = new ItemQOBD { ID = Convert.ToInt32(id), Source = i + "", Ref = Utility.encodeStringToBase64("ref"), Price_purchase = 10, Price_sell = 20, Type = Utility.encodeStringToBase64("type") };
                output[i-1] = ItemQOBD;

                return output;
            });
        }

        
        public Item_deliveryQOBD[] get_data_item_delivery(string nbLine)
        {
            Item_deliveryQOBD[] output = new Item_deliveryQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Item_deliveryQOBD Item_deliveryQOBD = new Item_deliveryQOBD { ID = i, DeliveryId = i, Item_ref = Utility.encodeStringToBase64("ref"), Quantity_delivery = 10 };
                output[i-1] = Item_deliveryQOBD;
            }
            return output;
        }

        
        public Task<Item_deliveryQOBD[]> get_data_item_deliveryAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Item_deliveryQOBD[] output = new Item_deliveryQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Item_deliveryQOBD Item_deliveryQOBD = new Item_deliveryQOBD { ID = i, DeliveryId = i, Item_ref = Utility.encodeStringToBase64("ref"), Quantity_delivery = 10 };
                    output[i-1] = Item_deliveryQOBD;
                }
                return output;
            });
        }

        
        public Item_deliveryQOBD[] get_data_item_delivery_by_delivery_list(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_item_delivery(delivery_array_list.Count().ToString());
        }

        
        public Task<Item_deliveryQOBD[]> get_data_item_delivery_by_delivery_listAsync(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_item_deliveryAsync(delivery_array_list.Count().ToString());
        }

        
        public Item_deliveryQOBD[] get_data_item_delivery_by_id(string id)
        {
            Item_deliveryQOBD[] output = new Item_deliveryQOBD[1];
            int i = 1;
            Item_deliveryQOBD Item_deliveryQOBD = new Item_deliveryQOBD { ID = Convert.ToInt32(id), DeliveryId = i, Item_ref = Utility.encodeStringToBase64("ref"), Quantity_delivery = 10 };
            output[i-1] = Item_deliveryQOBD;

            return output;
        }

        
        public Task<Item_deliveryQOBD[]> get_data_item_delivery_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Item_deliveryQOBD[] output = new Item_deliveryQOBD[1];
                int i = 1;
                Item_deliveryQOBD Item_deliveryQOBD = new Item_deliveryQOBD { ID = Convert.ToInt32(id), DeliveryId = i, Item_ref = Utility.encodeStringToBase64("ref"), Quantity_delivery = 10 };
                output[i-1] = Item_deliveryQOBD;

                return output;
            });
        }

        
        public ProviderQOBD[] get_data_provider(string nbLine)
        {
            ProviderQOBD[] output = new ProviderQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                ProviderQOBD ProviderQOBD = new ProviderQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Source = i };
                output[i-1] = ProviderQOBD;
            }
            return output;
        }

        
        public Task<ProviderQOBD[]> get_data_providerAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                ProviderQOBD[] output = new ProviderQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    ProviderQOBD ProviderQOBD = new ProviderQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Source = i };
                    output[i-1] = ProviderQOBD;
                }
                return output;
            });
        }

        
        public ProviderQOBD[] get_data_provider_by_id(string id)
        {
            ProviderQOBD[] output = new ProviderQOBD[1];
            int i = 1;
            ProviderQOBD ProviderQOBD = new ProviderQOBD { ID = Convert.ToInt32(id), Name = Utility.encodeStringToBase64("name"), Source = i };
            output[i-1] = ProviderQOBD;

            return output;
        }

        
        public Task<ProviderQOBD[]> get_data_provider_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ProviderQOBD[] output = new ProviderQOBD[1];
                int i = 1;
                ProviderQOBD ProviderQOBD = new ProviderQOBD { ID = Convert.ToInt32(id), Name = Utility.encodeStringToBase64("name"), Source = i };
                output[i-1] = ProviderQOBD;

                return output;
            });
        }

        
        public ProviderQOBD[] get_data_provider_by_provider_item_list(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider(provider_item_array_list.Count().ToString());
        }

        
        public Task<ProviderQOBD[]> get_data_provider_by_provider_item_listAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_providerAsync(provider_item_array_list.Count().ToString());
        }

        
        public Provider_itemQOBD[] get_data_provider_item(string nbLine)
        {
            Provider_itemQOBD[] output = new Provider_itemQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Provider_itemQOBD Provider_itemQOBD = new Provider_itemQOBD { ID = i, Item_ref = Utility.encodeStringToBase64("item ref"), Provider_name = Utility.encodeStringToBase64("provider name") };
                output[i-1] = Provider_itemQOBD;
            }
            return output;
        }

        
        public Task<Provider_itemQOBD[]> get_data_provider_itemAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Provider_itemQOBD[] output = new Provider_itemQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Provider_itemQOBD Provider_itemQOBD = new Provider_itemQOBD { ID = i, Item_ref = Utility.encodeStringToBase64("item ref"), Provider_name = Utility.encodeStringToBase64("provider name") };
                    output[i-1] = Provider_itemQOBD;
                }
                return output;
            });
        }

        
        public Provider_itemQOBD[] get_data_provider_item_by_id(string id)
        {
            Provider_itemQOBD[] output = new Provider_itemQOBD[1];
            int i = 1;
            Provider_itemQOBD Provider_itemQOBD = new Provider_itemQOBD { ID = Convert.ToInt32(id), Item_ref = Utility.encodeStringToBase64("item ref"), Provider_name = Utility.encodeStringToBase64("provider name") };
            output[i-1] = Provider_itemQOBD;

            return output;
        }

        
        public Task<Provider_itemQOBD[]> get_data_provider_item_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Provider_itemQOBD[] output = new Provider_itemQOBD[1];
                int i = 1;
                Provider_itemQOBD Provider_itemQOBD = new Provider_itemQOBD { ID = Convert.ToInt32(id), Item_ref = Utility.encodeStringToBase64("item ref"), Provider_name = Utility.encodeStringToBase64("provider name") };
                output[i-1] = Provider_itemQOBD;

                return output;
            });
        }

        
        public Provider_itemQOBD[] get_data_provider_item_by_item_list(ItemQOBD[] item_array_list)
        {
            return get_data_provider_item(item_array_list.Count().ToString());
        }

        
        public Task<Provider_itemQOBD[]> get_data_provider_item_by_item_listAsync(ItemQOBD[] item_array_list)
        {
            return get_data_provider_itemAsync(item_array_list.Count().ToString());
        }

        
        public Tax_itemQOBD[] get_data_tax_item(string nbLine)
        {
            Tax_itemQOBD[] output = new Tax_itemQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = Utility.encodeStringToBase64("ref") + i, TaxId = i, Tax_type = Utility.encodeStringToBase64("tax type : " + i), Tax_value = i };
                output[i-1] = tax_item;
            }
            return output;
        }

        
        public Task<Tax_itemQOBD[]> get_data_tax_itemAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Tax_itemQOBD[] output = new Tax_itemQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = Utility.encodeStringToBase64("ref") + i, TaxId = i, Tax_type = Utility.encodeStringToBase64("tax type : " + i), Tax_value = i };
                    output[i-1] = tax_item;
                }
                return output;
            });
        }

        
        public Tax_itemQOBD[] get_data_tax_item_by_id(string id)
        {
            Tax_itemQOBD[] output = new Tax_itemQOBD[1];
            int i = 1;
            Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = Convert.ToInt32(id), Item_ref = Utility.encodeStringToBase64("ref") + i, TaxId = i, Tax_type = Utility.encodeStringToBase64("tax type : " + i), Tax_value = i };
            output[i-1] = tax_item;

            return output;
        }

        
        public Task<Tax_itemQOBD[]> get_data_tax_item_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Tax_itemQOBD[] output = new Tax_itemQOBD[1];
                int i = 1;
                Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = Convert.ToInt32(id), Item_ref = Utility.encodeStringToBase64("ref") + i, TaxId = i, Tax_type = Utility.encodeStringToBase64("tax type : " + i), Tax_value = i };
                output[i-1] = tax_item;

                return output;
            });
        }

        
        public Tax_itemQOBD[] get_data_tax_item_by_item_list(ItemQOBD[] item_array_list)
        {
            return get_data_tax_item(item_array_list.Count().ToString());
        }

        
        public Task<Tax_itemQOBD[]> get_data_tax_item_by_item_listAsync(ItemQOBD[] item_array_list)
        {
            return get_data_tax_itemAsync(item_array_list.Count().ToString());
        }

        
        public Auto_refsQOBD[] get_filter_auto_ref(Auto_refsFilterQOBD auto_ref_array_list)
        {
            return new Auto_refsQOBD[1] { new Auto_refsQOBD { ID = auto_ref_array_list.ID, RefId = auto_ref_array_list.RefId } };
        }

        
        public Task<Auto_refsQOBD[]> get_filter_auto_refAsync(Auto_refsFilterQOBD auto_ref_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new Auto_refsQOBD[1] { new Auto_refsQOBD { ID = auto_ref_array_list.ID, RefId = auto_ref_array_list.RefId } };
            });
        }

        
        public ItemQOBD[] get_filter_item(ItemFilterQOBD item_array_list_filter)
        {
            return new ItemQOBD[1] { new ItemQOBD { ID = item_array_list_filter.ID, Ref = item_array_list_filter.Ref, Source = item_array_list_filter.Source, Type = item_array_list_filter.Type, Name = item_array_list_filter.Name } };
        }

        
        public Task<ItemQOBD[]> get_filter_itemAsync(ItemFilterQOBD item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ItemQOBD[1] { new ItemQOBD { ID = item_array_list_filter.ID, Ref = item_array_list_filter.Ref, Source = item_array_list_filter.Source, Type = item_array_list_filter.Type, Name = item_array_list_filter.Name } };
            });
        }

        
        public Item_deliveryQOBD[] get_filter_item_delivery(Item_deliveryFilterQOBD item_delivery_array_list_filter)
        {
            return new Item_deliveryQOBD[1] { new Item_deliveryQOBD { ID = item_delivery_array_list_filter.ID, DeliveryId = item_delivery_array_list_filter.DeliveryId, Item_ref = item_delivery_array_list_filter.Item_ref } };
        }

        
        public Task<Item_deliveryQOBD[]> get_filter_item_deliveryAsync(Item_deliveryFilterQOBD item_delivery_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Item_deliveryQOBD[1] { new Item_deliveryQOBD { ID = item_delivery_array_list_filter.ID, DeliveryId = item_delivery_array_list_filter.DeliveryId, Item_ref = item_delivery_array_list_filter.Item_ref } };
            });
        }

        
        public ProviderQOBD[] get_filter_provider(ProviderFilterQOBD provider_array_list_filter)
        {
            return new ProviderQOBD[1] { new ProviderQOBD { ID = provider_array_list_filter.ID, Name = provider_array_list_filter.Name, Source = provider_array_list_filter.Source } };
        }

        
        public Task<ProviderQOBD[]> get_filter_providerAsync(ProviderFilterQOBD provider_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ProviderQOBD[1] { new ProviderQOBD { ID = provider_array_list_filter.ID, Name = provider_array_list_filter.Name, Source = provider_array_list_filter.Source } };
            });
        }

        
        public Provider_itemQOBD[] get_filter_provider_item(Provider_itemFilterQOBD provider_item_array_list_filter)
        {
            return new Provider_itemQOBD[1] { new Provider_itemQOBD { ID = provider_item_array_list_filter.ID, Item_ref = provider_item_array_list_filter.Item_ref, Provider_name = provider_item_array_list_filter.Provider_name } };
        }

        
        public Task<Provider_itemQOBD[]> get_filter_provider_itemAsync(Provider_itemFilterQOBD provider_item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Provider_itemQOBD[1] { new Provider_itemQOBD { ID = provider_item_array_list_filter.ID, Item_ref = provider_item_array_list_filter.Item_ref, Provider_name = provider_item_array_list_filter.Provider_name } };
            });
        }

        
        public Tax_itemQOBD[] get_filter_tax_item(Tax_itemFilterQOBD tax_item_array_list_filter)
        {
            return new Tax_itemQOBD[1] { new Tax_itemQOBD { ID = tax_item_array_list_filter.ID, Item_ref = tax_item_array_list_filter.Item_ref, TaxId = tax_item_array_list_filter.TaxId, Tax_type = tax_item_array_list_filter.Tax_type, Tax_value = tax_item_array_list_filter.Tax_value } };
        }

        
        public Task<Tax_itemQOBD[]> get_filter_tax_itemAsync(Tax_itemFilterQOBD tax_item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Tax_itemQOBD[1] { new Tax_itemQOBD { ID = tax_item_array_list_filter.ID, Item_ref = tax_item_array_list_filter.Item_ref, TaxId = tax_item_array_list_filter.TaxId, Tax_type = tax_item_array_list_filter.Tax_type, Tax_value = tax_item_array_list_filter.Tax_value } };
            });
        }

        
        public Auto_refsQOBD[] insert_data_auto_ref(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_ref(auto_ref_array_list.Count().ToString());
        }

        
        public Task<Auto_refsQOBD[]> insert_data_auto_refAsync(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_refAsync(auto_ref_array_list.Count().ToString());
        }

        
        public ItemQOBD[] insert_data_item(ItemQOBD[] item_array_list)
        {
            return get_data_item(item_array_list.Count().ToString());
        }

        
        public Task<ItemQOBD[]> insert_data_itemAsync(ItemQOBD[] item_array_list)
        {
            return get_data_itemAsync(item_array_list.Count().ToString());
        }

        
        public Item_deliveryQOBD[] insert_data_item_delivery(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_delivery(item_delivery_array_list.Count().ToString());
        }

        
        public Task<Item_deliveryQOBD[]> insert_data_item_deliveryAsync(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_deliveryAsync(item_delivery_array_list.Count().ToString());
        }

        
        public ProviderQOBD[] insert_data_provider(ProviderQOBD[] provider_array_list)
        {
            return get_data_provider(provider_array_list.Count().ToString());
        }

        
        public Task<ProviderQOBD[]> insert_data_providerAsync(ProviderQOBD[] provider_array_list)
        {
            return get_data_providerAsync(provider_array_list.Count().ToString());
        }

        
        public Provider_itemQOBD[] insert_data_provider_item(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_item(provider_item_array_list.Count().ToString());
        }

        
        public Task<Provider_itemQOBD[]> insert_data_provider_itemAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_itemAsync(provider_item_array_list.Count().ToString());
        }

        
        public Tax_itemQOBD[] insert_data_tax_item(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_item(tax_item_array_list.Count().ToString());
        }

        
        public Task<Tax_itemQOBD[]> insert_data_tax_itemAsync(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_itemAsync(tax_item_array_list.Count().ToString());
        }

        
        public Auto_refsQOBD[] update_data_auto_ref(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_ref(auto_ref_array_list.Count().ToString());
        }

        
        public Task<Auto_refsQOBD[]> update_data_auto_refAsync(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_refAsync(auto_ref_array_list.Count().ToString());
        }

        
        public ItemQOBD[] update_data_item(ItemQOBD[] item_array_list)
        {
            return get_data_item(item_array_list.Count().ToString());
        }

        
        public Task<ItemQOBD[]> update_data_itemAsync(ItemQOBD[] item_array_list)
        {
            return get_data_itemAsync(item_array_list.Count().ToString());
        }

        
        public Item_deliveryQOBD[] update_data_item_delivery(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_delivery(item_delivery_array_list.Count().ToString());
        }

        
        public Task<Item_deliveryQOBD[]> update_data_item_deliveryAsync(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_deliveryAsync(item_delivery_array_list.Count().ToString());
        }

        
        public ProviderQOBD[] update_data_provider(ProviderQOBD[] provider_array_list)
        {
            return get_data_provider(provider_array_list.Count().ToString());
        }

        
        public Task<ProviderQOBD[]> update_data_providerAsync(ProviderQOBD[] provider_array_list)
        {
            return get_data_providerAsync(provider_array_list.Count().ToString());
        }

        
        public Provider_itemQOBD[] update_data_provider_item(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_item(provider_item_array_list.Count().ToString());
        }

        
        public Task<Provider_itemQOBD[]> update_data_provider_itemAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_itemAsync(provider_item_array_list.Count().ToString());
        }

        public Tax_itemQOBD[] update_data_tax_item(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_item(tax_item_array_list.Count().ToString());
        }

        public Task<Tax_itemQOBD[]> update_data_tax_itemAsync(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_itemAsync(tax_item_array_list.Count().ToString());
        }

        #endregion

        #region [ Mock Notification implementation ]
        //================================[ Notification ]===================================================

        
        public NotificationQOBD[] delete_data_notification(NotificationQOBD[] notification_array_list)
        {
            return new NotificationQOBD[0];
        }

        
        public Task<NotificationQOBD[]> delete_data_notificationAsync(NotificationQOBD[] notification_array_list)
        {
            return Task.Factory.StartNew(() => { return new NotificationQOBD[0]; });
        }

        
        public NotificationQOBD[] get_data_notification(string nbLine)
        {
            NotificationQOBD[] output = new NotificationQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                NotificationQOBD NotificationQOBD = new NotificationQOBD { ID = i, BillId = i, Date = "Date" };
                output[i-1] = NotificationQOBD;
            }
            return output;
        }

        
        public Task<NotificationQOBD[]> get_data_notificationAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                NotificationQOBD[] output = new NotificationQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    NotificationQOBD NotificationQOBD = new NotificationQOBD { ID = i, BillId = i, Date = "Date" };
                    output[i-1] = NotificationQOBD;
                }
                return output;
            });
        }

        
        public NotificationQOBD[] get_data_notification_by_client_list(ClientQOBD[] client_array_list)
        {
            return get_data_notification(client_array_list.Count().ToString());
        }

        
        public Task<NotificationQOBD[]> get_data_notification_by_client_listAsync(ClientQOBD[] client_array_list)
        {
            return get_data_notificationAsync(client_array_list.Count().ToString());
        }

        
        public NotificationQOBD[] get_data_notification_by_command_list(CommandsQOBD[] command_array_list)
        {
            return get_data_notification(command_array_list.Count().ToString());
        }

        
        public Task<NotificationQOBD[]> get_data_notification_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_notificationAsync(command_array_list.Count().ToString());
        }

        
        public NotificationQOBD[] get_data_notification_by_id(string id)
        {
            NotificationQOBD[] output = new NotificationQOBD[1];
            int i = 1;
            NotificationQOBD NotificationQOBD = new NotificationQOBD { ID = Convert.ToInt32(id), BillId = i, Date = "date" };
            output[i-1] = NotificationQOBD;

            return output;
        }

        
        public Task<NotificationQOBD[]> get_data_notification_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                NotificationQOBD[] output = new NotificationQOBD[1];
                int i = 1;
                NotificationQOBD NotificationQOBD = new NotificationQOBD { ID = Convert.ToInt32(id), BillId = i, Date = "date" };
                output[i-1] = NotificationQOBD;

                return output;
            });
        }

        
        public NotificationQOBD[] get_filter_notification(NotificationFilterQOBD notification_array_list)
        {
            return new NotificationQOBD[1] { new NotificationQOBD { ID = notification_array_list.ID, BillId = notification_array_list.BillId, Date = notification_array_list.Date } };
        }

        
        public Task<NotificationQOBD[]> get_filter_notificationAsync(NotificationFilterQOBD notification_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new NotificationQOBD[1] { new NotificationQOBD { ID = notification_array_list.ID, BillId = notification_array_list.BillId, Date = notification_array_list.Date } };
            });
        }

        
        public NotificationQOBD[] insert_data_notification(NotificationQOBD[] notification_array_list)
        {
            return get_data_notification(notification_array_list.Count().ToString());
        }

        
        public Task<NotificationQOBD[]> insert_data_notificationAsync(NotificationQOBD[] notification_array_list)
        {
            return get_data_notificationAsync(notification_array_list.Count().ToString());
        }

        
        public NotificationQOBD[] update_data_notification(NotificationQOBD[] notification_array_list)
        {
            return get_data_notification(notification_array_list.Count().ToString());
        }

        
        public Task<NotificationQOBD[]> update_data_notificationAsync(NotificationQOBD[] notification_array_list)
        {
            return get_data_notificationAsync(notification_array_list.Count().ToString());
        }


        #endregion

        #region [ Mock Referential implementation ]
        //================================[ Referential ]===================================================

        
        public InfosQOBD[] delete_data_infos(InfosQOBD[] infos_array_list)
        {
            return new InfosQOBD[0];
        }

        
        public Task<InfosQOBD[]> delete_data_infosAsync(InfosQOBD[] infos_array_list)
        {
            return Task.Factory.StartNew(() => { return new InfosQOBD[0]; });
        }

        
        public InfosQOBD[] get_data_infos(string nbLine)
        {
            InfosQOBD[] output = new InfosQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                InfosQOBD InfosQOBD = new InfosQOBD { ID = i, Name = "name", Value = "value" };
                output[i-1] = InfosQOBD;
            }
            return output;
        }

        
        public Task<InfosQOBD[]> get_data_infosAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                InfosQOBD[] output = new InfosQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    InfosQOBD InfosQOBD = new InfosQOBD { ID = i, Name = "name", Value = "value" };
                    output[i-1] = InfosQOBD;
                }
                return output;
            });
        }

        
        public InfosQOBD[] get_data_infos_by_id(string id)
        {
            InfosQOBD[] output = new InfosQOBD[1];
            int i = 1;
            InfosQOBD InfosQOBD = new InfosQOBD { ID = Convert.ToInt32(id), Value = i + "", Name = "Name" };
            output[i-1] = InfosQOBD;

            return output;
        }

        
        public Task<InfosQOBD[]> get_data_infos_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                InfosQOBD[] output = new InfosQOBD[1];
                int i = 1;
                InfosQOBD InfosQOBD = new InfosQOBD { ID = Convert.ToInt32(id), Value = i + "", Name = "Name" };
                output[i-1] = InfosQOBD;

                return output;
            });
        }

        
        public InfosQOBD[] get_filter_infos(InfosFilterQOBD infos_array_list_filter)
        {
            return new InfosQOBD[1] { new InfosQOBD { ID = infos_array_list_filter.ID, Name = infos_array_list_filter.Name, Value = infos_array_list_filter.Value } };
        }

        
        public Task<InfosQOBD[]> get_filter_infosAsync(InfosFilterQOBD infos_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new InfosQOBD[1] { new InfosQOBD { ID = infos_array_list_filter.ID, Name = infos_array_list_filter.Name, Value = infos_array_list_filter.Value } };
            });
        }

        
        public InfosQOBD[] insert_data_infos(InfosQOBD[] infos_array_list)
        {
            return get_data_infos(infos_array_list.Count().ToString());
        }

        
        public Task<InfosQOBD[]> insert_data_infosAsync(InfosQOBD[] infos_array_list)
        {
            return get_data_infosAsync(infos_array_list.Count().ToString());
        }

        
        public InfosQOBD[] update_data_infos(InfosQOBD[] infos_array_list)
        {
            return get_data_infos(infos_array_list.Count().ToString());
        }

        
        public Task<InfosQOBD[]> update_data_infosAsync(InfosQOBD[] infos_array_list)
        {
            return get_data_infosAsync(infos_array_list.Count().ToString());
        }

        #endregion

        #region [ Mock Security implementation ]
        //================================[ Security ]===================================================

        
        public ActionQOBD[] delete_data_action(ActionQOBD[] action_array_list)
        {
            return new ActionQOBD[0];
        }

        
        public Task<ActionQOBD[]> delete_data_actionAsync(ActionQOBD[] action_array_list)
        {
            return Task.Factory.StartNew(() => { return new ActionQOBD[0]; });
        }

        
        public ActionRecordQOBD[] delete_data_actionRecord(ActionRecordQOBD[] actionRecord_array_list)
        {
            return new ActionRecordQOBD[0];
        }

        
        public Task<ActionRecordQOBD[]> delete_data_actionRecordAsync(ActionRecordQOBD[] actionRecord_array_list)
        {
            return Task.Factory.StartNew(() => { return new ActionRecordQOBD[0]; });
        }

        
        public Agent_roleQOBD[] delete_data_agent_role(Agent_roleQOBD[] agent_role_array_list)
        {
            return new Agent_roleQOBD[0];
        }

        
        public Task<Agent_roleQOBD[]> delete_data_agent_roleAsync(Agent_roleQOBD[] agent_role_array_list)
        {
            return Task.Factory.StartNew(() => { return new Agent_roleQOBD[0]; });
        }

        
        public PrivilegeQOBD[] delete_data_privilege(PrivilegeQOBD[] privilege_array_list)
        {
            return new PrivilegeQOBD[0];
        }

        
        public Task<PrivilegeQOBD[]> delete_data_privilegeAsync(PrivilegeQOBD[] privilege_array_list)
        {
            return Task.Factory.StartNew(() => { return new PrivilegeQOBD[0]; });
        }

        
        public RoleQOBD[] delete_data_role(RoleQOBD[] role_array_list)
        {
            return new RoleQOBD[0];
        }

        
        public Task<RoleQOBD[]> delete_data_roleAsync(RoleQOBD[] role_array_list)
        {
            return Task.Factory.StartNew(() => { return new RoleQOBD[0]; });
        }

        
        public Role_actionQOBD[] delete_data_role_action(Role_actionQOBD[] role_action_array_list)
        {
            return new Role_actionQOBD[0];
        }

        
        public Task<Role_actionQOBD[]> delete_data_role_actionAsync(Role_actionQOBD[] role_action_array_list)
        {
            return Task.Factory.StartNew(() => { return new Role_actionQOBD[0]; });
        }

        
        public ActionQOBD[] get_data_action(string nbLine)
        {
            ActionQOBD[] output = new ActionQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                ActionQOBD ActionQOBD = new ActionQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = 1 + "", Date = DateTime.Now + "", Role_actionId = 1 + "" } };
                output[i-1] = ActionQOBD;
            }
            return output;
        }

        
        public Task<ActionQOBD[]> get_data_actionAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                ActionQOBD[] output = new ActionQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    ActionQOBD ActionQOBD = new ActionQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = 1+"", Date = DateTime.Now +"", Role_actionId = 1+"" } };
                    output[i-1] = ActionQOBD;
                }
                return output;
            });
        }

        
        public ActionRecordQOBD[] get_data_actionRecord(string nbLine)
        {
            ActionRecordQOBD[] output = new ActionRecordQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                ActionRecordQOBD ActionRecordQOBD = new ActionRecordQOBD { ID = i, AgentId = i, TargetId = i + 1, Date = Utility.encodeStringToBase64(DateTime.Now +"") };
                output[i-1] = ActionRecordQOBD;
            }
            return output;
        }

        
        public Task<ActionRecordQOBD[]> get_data_actionRecordAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                ActionRecordQOBD[] output = new ActionRecordQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    ActionRecordQOBD ActionRecordQOBD = new ActionRecordQOBD { ID = i, AgentId = i, TargetId = i +1, Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                    output[i-1] = ActionRecordQOBD;
                }
                return output;
            });
        }

        
        public ActionRecordQOBD[] get_data_actionRecord_by_id(string id)
        {
            ActionRecordQOBD[] output = new ActionRecordQOBD[1];
            int i = 1;
            ActionRecordQOBD ActionRecordQOBD = new ActionRecordQOBD { ID = Convert.ToInt32(id), AgentId = i, TargetId = i, TargetName = Utility.encodeStringToBase64("target name"), Date = Utility.encodeStringToBase64(DateTime.Now + "") };
            output[i-1] = ActionRecordQOBD;

            return output;
        }

        
        public Task<ActionRecordQOBD[]> get_data_actionRecord_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ActionRecordQOBD[] output = new ActionRecordQOBD[1];
                int i = 1;
                ActionRecordQOBD ActionRecordQOBD = new ActionRecordQOBD { ID = Convert.ToInt32(id), AgentId = i, TargetId = i, TargetName = Utility.encodeStringToBase64("target name"), Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                output[i-1] = ActionRecordQOBD;

                return output;
            });
        }

        
        public ActionQOBD[] get_data_action_by_id(string id)
        {
            ActionQOBD[] output = new ActionQOBD[1];
            int i = 1;
            ActionQOBD ActionQOBD = new ActionQOBD { ID = Convert.ToInt32(id), Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = 1+"", Date = Utility.encodeStringToBase64(DateTime.Now +"") } };
            output[i-1] = ActionQOBD;

            return output;
        }

        
        public Task<ActionQOBD[]> get_data_action_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ActionQOBD[] output = new ActionQOBD[1];
                int i = 1;
                ActionQOBD ActionQOBD = new ActionQOBD { ID = Convert.ToInt32(id), Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = 1 + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") } };
                output[i-1] = ActionQOBD;

                return output;
            });
        }

        
        public Agent_roleQOBD[] get_data_agent_role(string nbLine)
        {
            Agent_roleQOBD[] output = new Agent_roleQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Agent_roleQOBD Agent_roleQOBD = new Agent_roleQOBD { ID = i, AgentId = i, Date = Utility.encodeStringToBase64(DateTime.Now + ""), RoleId = 1 };
                output[i-1] = Agent_roleQOBD;
            }
            return output;
        }

        
        public Task<Agent_roleQOBD[]> get_data_agent_roleAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Agent_roleQOBD[] output = new Agent_roleQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Agent_roleQOBD Agent_roleQOBD = new Agent_roleQOBD { ID = i, AgentId = i, Date = Utility.encodeStringToBase64(DateTime.Now + ""), RoleId = 1 };
                    output[i-1] = Agent_roleQOBD;
                }
                return output;
            });
        }

        
        public Agent_roleQOBD[] get_data_agent_role_by_id(string id)
        {
            Agent_roleQOBD[] output = new Agent_roleQOBD[1];
            int i = 1;
            Agent_roleQOBD Agent_roleQOBD = new Agent_roleQOBD { ID = Convert.ToInt32(id), AgentId = i, RoleId = i, Date = Utility.encodeStringToBase64(DateTime.Now + "") };
            output[i-1] = Agent_roleQOBD;

            return output;
        }

        
        public Task<Agent_roleQOBD[]> get_data_agent_role_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Agent_roleQOBD[] output = new Agent_roleQOBD[1];
                int i = 1;
                Agent_roleQOBD Agent_roleQOBD = new Agent_roleQOBD { ID = Convert.ToInt32(id), AgentId = i , Date = Utility.encodeStringToBase64(DateTime.Now + ""), RoleId = i };
                output[i-1] = Agent_roleQOBD;

                return output;
            });
        }

        
        public PrivilegeQOBD[] get_data_privilege(string nbLine)
        {
            PrivilegeQOBD[] output = new PrivilegeQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                PrivilegeQOBD PrivilegeQOBD = new PrivilegeQOBD { ID = i + "", Role_actionId = i + "", Date = Utility.encodeStringToBase64(DateTime.Now+"") };
                output[i-1] = PrivilegeQOBD;
            }
            return output;
        }

        
        public Task<PrivilegeQOBD[]> get_data_privilegeAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                PrivilegeQOBD[] output = new PrivilegeQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    PrivilegeQOBD PrivilegeQOBD = new PrivilegeQOBD { ID = i + "", Role_actionId = i + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                    output[i-1] = PrivilegeQOBD;
                }
                return output;
            });
        }

        
        public PrivilegeQOBD[] get_data_privilege_by_id(string id)
        {
            PrivilegeQOBD[] output = new PrivilegeQOBD[1];
            int i = 1;
            PrivilegeQOBD PrivilegeQOBD = new PrivilegeQOBD { ID = Convert.ToInt32(id) + "", Role_actionId = i + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") };
            output[i-1] = PrivilegeQOBD;

            return output;
        }

        
        public Task<PrivilegeQOBD[]> get_data_privilege_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                PrivilegeQOBD[] output = new PrivilegeQOBD[1];
                int i = 1;
                PrivilegeQOBD PrivilegeQOBD = new PrivilegeQOBD { ID = id, Role_actionId = i + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") };
                output[i-1] = PrivilegeQOBD;

                return output;
            });
        }

        
        public RoleQOBD[] get_data_role(string nbLine)
        {
            RoleQOBD[] output = new RoleQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                RoleQOBD Role_actionQOBD = new RoleQOBD { ID = i, Name = Utility.encodeStringToBase64("Name"), Actions = new ActionQOBD[] { new ActionQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = i+"", Date = Utility.encodeStringToBase64(DateTime.Now+"") } } } };
                output[i-1] = Role_actionQOBD;
            }
            return output;
        }

        
        public Task<RoleQOBD[]> get_data_roleAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                RoleQOBD[] output = new RoleQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    RoleQOBD Role_actionQOBD = new RoleQOBD { ID = i, Name = Utility.encodeStringToBase64("Name"), Actions = new ActionQOBD[] { new ActionQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = i + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") } } } };
                    output[i-1] = Role_actionQOBD;
                }
                return output;
            });
        }

        
        public Role_actionQOBD[] get_data_role_action(string nbLine)
        {
            Role_actionQOBD[] output = new Role_actionQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                Role_actionQOBD Role_actionQOBD = new Role_actionQOBD { ID = i, ActionId = i, RoleId = i };
                output[i-1] = Role_actionQOBD;
            }
            return output;
        }

        
        public Task<Role_actionQOBD[]> get_data_role_actionAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Role_actionQOBD[] output = new Role_actionQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    Role_actionQOBD Role_actionQOBD = new Role_actionQOBD { ID = i, ActionId = i, RoleId = i };
                    output[i-1] = Role_actionQOBD;
                }
                return output;
            });
        }

        
        public Role_actionQOBD[] get_data_role_action_by_id(string id)
        {
            Role_actionQOBD[] output = new Role_actionQOBD[1];
            int i = 1;
            Role_actionQOBD Role_actionQOBD = new Role_actionQOBD { ID = Convert.ToInt32(id), ActionId = i, RoleId = i };
            output[i-1] = Role_actionQOBD;

            return output;
        }

        
        public Task<Role_actionQOBD[]> get_data_role_action_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Role_actionQOBD[] output = new Role_actionQOBD[1];
                int i = 1;
                Role_actionQOBD Role_actionQOBD = new Role_actionQOBD { ID = Convert.ToInt32(id), ActionId = i, RoleId = i };
                output[i-1] = Role_actionQOBD;

                return output;
            });
        }

        
        public RoleQOBD[] get_data_role_by_id(string id)
        {
            RoleQOBD[] output = new RoleQOBD[1];
            int i = 1;
            RoleQOBD RoleQOBD = new RoleQOBD { ID = Convert.ToInt32(id), Name =  Utility.encodeStringToBase64("Name"), Actions = new ActionQOBD[] { new ActionQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = i + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") } } } };
            output[i-1] = RoleQOBD;

            return output;
        }

        
        public Task<RoleQOBD[]> get_data_role_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                RoleQOBD[] output = new RoleQOBD[1];
                int i = 1;
                RoleQOBD RoleQOBD = new RoleQOBD { ID = Convert.ToInt32(id), Name = Utility.encodeStringToBase64("Name"), Actions = new ActionQOBD[] { new ActionQOBD { ID = i, Name = Utility.encodeStringToBase64("name"), Right = new PrivilegeQOBD { ID = i + "", Date = Utility.encodeStringToBase64(DateTime.Now + "") } } } };
                output[i-1] = RoleQOBD;

                return output;
            });
        }

        
        public ActionQOBD[] get_filter_action(ActionFilterQOBD action_array_list)
        {
            return new ActionQOBD[1] { new ActionQOBD { ID = action_array_list.ID, Name = action_array_list.Name, Right = new PrivilegeQOBD() } };
        }

        
        public Task<ActionQOBD[]> get_filter_actionAsync(ActionFilterQOBD action_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new ActionQOBD[1] { new ActionQOBD { ID = action_array_list.ID, Name = action_array_list.Name, Right = new PrivilegeQOBD() } };
            });
        }

        
        public ActionRecordQOBD[] get_filter_actionRecord(ActionRecordFilterQOBD actionRecord_array_list)
        {
            return new ActionRecordQOBD[1] { new ActionRecordQOBD { ID = actionRecord_array_list.ID, AgentId = actionRecord_array_list.AgentId, TargetId = actionRecord_array_list.TargetId } };
        }

        
        public Task<ActionRecordQOBD[]> get_filter_actionRecordAsync(ActionRecordFilterQOBD actionRecord_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new ActionRecordQOBD[1] { new ActionRecordQOBD { ID = actionRecord_array_list.ID, AgentId = actionRecord_array_list.AgentId, TargetId = actionRecord_array_list.TargetId } };
            });
        }

        
        public Agent_roleQOBD[] get_filter_agent_role(Agent_roleFilterQOBD agent_role_array_list)
        {
            return new Agent_roleQOBD[1] { new Agent_roleQOBD { ID = agent_role_array_list.ID, AgentId = agent_role_array_list.AgentId, RoleId = agent_role_array_list.RoleId } };
        }

        
        public Task<Agent_roleQOBD[]> get_filter_agent_roleAsync(Agent_roleFilterQOBD agent_role_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new Agent_roleQOBD[1] { new Agent_roleQOBD { ID = agent_role_array_list.ID, AgentId = agent_role_array_list.AgentId, RoleId = agent_role_array_list.RoleId } };
            });
        }

        
        public PrivilegeQOBD[] get_filter_privilege(PrivilegeFilterQOBD privilege_array_list_filter)
        {
            return new PrivilegeQOBD[1] { new PrivilegeQOBD { ID = privilege_array_list_filter.ID, Role_actionId = privilege_array_list_filter.Role_actionId, Date = privilege_array_list_filter.Date } };
        }

        
        public Task<PrivilegeQOBD[]> get_filter_privilegeAsync(PrivilegeFilterQOBD privilege_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new PrivilegeQOBD[1] { new PrivilegeQOBD { ID = privilege_array_list_filter.ID, Role_actionId = privilege_array_list_filter.Role_actionId, Date = privilege_array_list_filter.Date } };
            });
        }

        
        public RoleQOBD[] get_filter_role(RoleFilterQOBD role_array_list)
        {
            return new RoleQOBD[1] { new RoleQOBD { ID = role_array_list.ID, Name = role_array_list.Name } };
        }

        
        public Task<RoleQOBD[]> get_filter_roleAsync(RoleFilterQOBD role_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new RoleQOBD[1] { new RoleQOBD { ID = role_array_list.ID, Name = role_array_list.Name } };
            });
        }

        
        public Role_actionQOBD[] get_filter_role_action(Role_actionFilterQOBD role_action_array_list)
        {
            return new Role_actionQOBD[1] { new Role_actionQOBD { ID = role_action_array_list.ID, ActionId = role_action_array_list.ActionId, RoleId = role_action_array_list.RoleId } };
        }

        
        public Task<Role_actionQOBD[]> get_filter_role_actionAsync(Role_actionFilterQOBD role_action_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new Role_actionQOBD[1] { new Role_actionQOBD { ID = role_action_array_list.ID, ActionId = role_action_array_list.ActionId, RoleId = role_action_array_list.RoleId } };
            });
        }
        
        public ActionQOBD[] insert_data_action(ActionQOBD[] action_array_list)
        {
            return get_data_action(action_array_list.Count().ToString());
        }
        
        public Task<ActionQOBD[]> insert_data_actionAsync(ActionQOBD[] action_array_list)
        {
            return get_data_actionAsync(action_array_list.Count().ToString());
        }
        
        public ActionRecordQOBD[] insert_data_actionRecord(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecord(actionRecord_array_list.Count().ToString());
        }
        
        public Task<ActionRecordQOBD[]> insert_data_actionRecordAsync(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecordAsync(actionRecord_array_list.Count().ToString());
        }

        
        public Agent_roleQOBD[] insert_data_agent_role(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_role(agent_role_array_list.Count().ToString());
        }

        
        public Task<Agent_roleQOBD[]> insert_data_agent_roleAsync(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_roleAsync(agent_role_array_list.Count().ToString());
        }

        
        public PrivilegeQOBD[] insert_data_privilege(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilege(privilege_array_list.Count().ToString());
        }

        
        public Task<PrivilegeQOBD[]> insert_data_privilegeAsync(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilegeAsync(privilege_array_list.Count().ToString());
        }

        
        public RoleQOBD[] insert_data_role(RoleQOBD[] role_array_list)
        {
            return get_data_role(role_array_list.Count().ToString());
        }

        
        public Task<RoleQOBD[]> insert_data_roleAsync(RoleQOBD[] role_array_list)
        {
            return get_data_roleAsync(role_array_list.Count().ToString());
        }

        
        public Role_actionQOBD[] insert_data_role_action(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_action(role_action_array_list.Count().ToString());
        }

        
        public Task<Role_actionQOBD[]> insert_data_role_actionAsync(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_actionAsync(role_action_array_list.Count().ToString());
        }

        
        public ActionQOBD[] update_data_action(ActionQOBD[] action_array_list)
        {
            return get_data_action(action_array_list.Count().ToString());
        }

        
        public Task<ActionQOBD[]> update_data_actionAsync(ActionQOBD[] action_array_list)
        {
            return get_data_actionAsync(action_array_list.Count().ToString());
        }

        
        public ActionRecordQOBD[] update_data_actionRecord(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecord(actionRecord_array_list.Count().ToString());
        }

        
        public Task<ActionRecordQOBD[]> update_data_actionRecordAsync(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecordAsync(actionRecord_array_list.Count().ToString());
        }

        
        public Agent_roleQOBD[] update_data_agent_role(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_role(agent_role_array_list.Count().ToString());
        }

        
        public Task<Agent_roleQOBD[]> update_data_agent_roleAsync(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_roleAsync(agent_role_array_list.Count().ToString());
        }

        
        public PrivilegeQOBD[] update_data_privilege(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilege(privilege_array_list.Count().ToString());
        }

        
        public Task<PrivilegeQOBD[]> update_data_privilegeAsync(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilegeAsync(privilege_array_list.Count().ToString());
        }

        
        public RoleQOBD[] update_data_role(RoleQOBD[] role_array_list)
        {
            return get_data_role(role_array_list.Count().ToString());
        }

        
        public Task<RoleQOBD[]> update_data_roleAsync(RoleQOBD[] role_array_list)
        {
            return get_data_roleAsync(role_array_list.Count().ToString());
        }

        
        public Role_actionQOBD[] update_data_role_action(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_action(role_action_array_list.Count().ToString());
        }

        
        public Task<Role_actionQOBD[]> update_data_role_actionAsync(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_actionAsync(role_action_array_list.Count().ToString());
        }


        #endregion

        #region [ Mock Statistic implementation ]
        //================================[ Statistic ]===================================================

        
        public StatisticQOBD[] delete_data_statistic(StatisticQOBD[] statistic_array_list)
        {
            return new StatisticQOBD[0];
        }

        
        public Task<StatisticQOBD[]> delete_data_statisticAsync(StatisticQOBD[] statistic_array_list)
        {
            return Task.Factory.StartNew(() => { return new StatisticQOBD[0]; });
        }

        
        public StatisticQOBD[] get_data_statistic(string nbLine)
        {
            StatisticQOBD[] output = new StatisticQOBD[Convert.ToInt32(nbLine)];
            for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
            {
                StatisticQOBD StatisticQOBD = new StatisticQOBD { ID = i, BillId = i + "", Bill_date = Utility.encodeStringToBase64(DateTime.Now +""), Total = i + "", Income = i +"", Pay_received = i + 10 +"", Pay_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total_tax_included = i+"", Price_purchase_total = i+"", Tax_value = i, Date_limit = Utility.encodeStringToBase64(DateTime.Now + "") };
                output[i-1] = StatisticQOBD;
            }
            return output;
        }

        
        public Task<StatisticQOBD[]> get_data_statisticAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                StatisticQOBD[] output = new StatisticQOBD[Convert.ToInt32(nbLine)];
                for (int i = 1; i <= Convert.ToInt32(nbLine); i++)
                {
                    StatisticQOBD StatisticQOBD = new StatisticQOBD { ID = i, BillId = i + "", Bill_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total = i + "", Income = i + "", Pay_received = i + 10 + "", Pay_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total_tax_included = i + "", Price_purchase_total = i + "", Tax_value = i, Date_limit = Utility.encodeStringToBase64(DateTime.Now + "") };
                    output[i-1] = StatisticQOBD;
                }
                return output;
            });
        }

        
        public StatisticQOBD[] get_data_statistic_by_id(string id)
        {
            StatisticQOBD[] output = new StatisticQOBD[1];
            int i = 1;
            StatisticQOBD StatisticQOBD = new StatisticQOBD { ID = i, BillId = i + "", Bill_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total = i + "", Income = i + "", Pay_received = i + 10 + "", Pay_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total_tax_included = i + "", Price_purchase_total = i + "", Tax_value = i, Date_limit = Utility.encodeStringToBase64(DateTime.Now + "") };
            output[i-1] = StatisticQOBD;

            return output;
        }

        
        public Task<StatisticQOBD[]> get_data_statistic_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                StatisticQOBD[] output = new StatisticQOBD[1];
                int i = 1;
                StatisticQOBD StatisticQOBD = new StatisticQOBD { ID = i, BillId = i + "", Bill_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total = i + "", Income = i + "", Pay_received = i + 10 + "", Pay_date = Utility.encodeStringToBase64(DateTime.Now + ""), Total_tax_included = i + "", Price_purchase_total = i + "", Tax_value = i, Date_limit = Utility.encodeStringToBase64(DateTime.Now + "") };
                output[i-1] = StatisticQOBD;

                return output;
            });
        }

        
        public StatisticQOBD[] get_filter_statistic(StatisticFilterQOBD statistic_array_list_filter)
        {
            return new StatisticQOBD[1] { new StatisticQOBD { ID = statistic_array_list_filter.ID, Total = statistic_array_list_filter.Total, BillId = statistic_array_list_filter.BillId, Company = statistic_array_list_filter.Company, Date_limit = statistic_array_list_filter.Date_limit, Pay_date = statistic_array_list_filter.Pay_date, Bill_date = statistic_array_list_filter.Bill_date, Income = statistic_array_list_filter.Income, Pay_received = statistic_array_list_filter.Pay_received, Income_percent = statistic_array_list_filter.Income_percent, Total_tax_included = statistic_array_list_filter.Total_tax_included, Price_purchase_total = statistic_array_list_filter.Price_purchase_total } };
        }

        
        public Task<StatisticQOBD[]> get_filter_statisticAsync(StatisticFilterQOBD statistic_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new StatisticQOBD[1] { new StatisticQOBD { ID = statistic_array_list_filter.ID, Total = statistic_array_list_filter.Total, BillId = statistic_array_list_filter.BillId, Company = statistic_array_list_filter.Company, Date_limit = statistic_array_list_filter.Date_limit, Pay_date = statistic_array_list_filter.Pay_date, Bill_date = statistic_array_list_filter.Bill_date, Income = statistic_array_list_filter.Income, Pay_received = statistic_array_list_filter.Pay_received, Income_percent = statistic_array_list_filter.Income_percent, Total_tax_included = statistic_array_list_filter.Total_tax_included, Price_purchase_total = statistic_array_list_filter.Price_purchase_total } };
            });
        }

        
        public StatisticQOBD[] insert_data_statistic(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statistic(statistic_array_list.Count().ToString());
        }

        
        public Task<StatisticQOBD[]> insert_data_statisticAsync(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statisticAsync(statistic_array_list.Count().ToString());
        }

        
        public StatisticQOBD[] update_data_statistic(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statistic(statistic_array_list.Count().ToString());
        }

        
        public Task<StatisticQOBD[]> update_data_statisticAsync(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statisticAsync(statistic_array_list.Count().ToString());
        }

        #endregion
        
    }
}
