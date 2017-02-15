using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using QOBDGateway.QOBDServiceReference;

namespace QOBDTest.Classes
{
    class QOBDClient : System.ServiceModel.ClientBase<QOBDGateway.QOBDServiceReference.QOBDWebServicePortType>, QOBDGateway.QOBDServiceReference.QOBDWebServicePortType
    {
        [return: MessageParameter(Name = "return")]
        public ActionQOBD[] delete_data_action(ActionQOBD[] action_array_list)
        {
            return new ActionQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionQOBD[]> delete_data_actionAsync(ActionQOBD[] action_array_list)
        {
            return Task.Factory.StartNew(()=> { return new ActionQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public ActionRecordQOBD[] delete_data_actionRecord(ActionRecordQOBD[] actionRecord_array_list)
        {
            return new ActionRecordQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionRecordQOBD[]> delete_data_actionRecordAsync(ActionRecordQOBD[] actionRecord_array_list)
        {
            return Task.Factory.StartNew(() => { return new ActionRecordQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] delete_data_address(AddressQOBD[] address_array_list)
        {
            return new AddressQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> delete_data_addressAsync(AddressQOBD[] address_array_list)
        {
            return Task.Factory.StartNew(() => { return new AddressQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] delete_data_agent(AgentQOBD[] agent_array_list)
        {
            return new AgentQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> delete_data_agentAsync(AgentQOBD[] agent_array_list)
        {
            return Task.Factory.StartNew(() => { return new AgentQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Agent_roleQOBD[] delete_data_agent_role(Agent_roleQOBD[] agent_role_array_list)
        {
            return new Agent_roleQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Agent_roleQOBD[]> delete_data_agent_roleAsync(Agent_roleQOBD[] agent_role_array_list)
        {
            return Task.Factory.StartNew(() => { return new Agent_roleQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Auto_refsQOBD[] delete_data_auto_ref(Auto_refsQOBD[] auto_ref_array_list)
        {
            return new Auto_refsQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Auto_refsQOBD[]> delete_data_auto_refAsync(Auto_refsQOBD[] auto_ref_array_list)
        {
            return Task.Factory.StartNew(() => { return new Auto_refsQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] delete_data_bill(BillQOBD[] bill_array_list)
        {
            return new BillQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> delete_data_billAsync(BillQOBD[] bill_array_list)
        {
            return Task.Factory.StartNew(() => { return new BillQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] delete_data_client(ClientQOBD[] client_array_list)
        {
            return new ClientQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> delete_data_clientAsync(ClientQOBD[] client_array_list)
        {
            return Task.Factory.StartNew(() => { return new ClientQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] delete_data_command(CommandsQOBD[] command_array_list)
        {
            return new CommandsQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> delete_data_commandAsync(CommandsQOBD[] command_array_list)
        {
            return Task.Factory.StartNew(() => { return new CommandsQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] delete_data_command_item(Command_itemQOBD[] command_item_array_list)
        {
            return new Command_itemQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> delete_data_command_itemAsync(Command_itemQOBD[] command_item_array_list)
        {
            return Task.Factory.StartNew(() => { return new Command_itemQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] delete_data_contact(ContactQOBD[] contact_array_list)
        {
            return new ContactQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> delete_data_contactAsync(ContactQOBD[] contact_array_list)
        {
            return Task.Factory.StartNew(() => { return new ContactQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] delete_data_delivery(DeliveryQOBD[] delivery_array_list)
        {
            return new DeliveryQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> delete_data_deliveryAsync(DeliveryQOBD[] delivery_array_list)
        {
            return Task.Factory.StartNew(() => { return new DeliveryQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public InfosQOBD[] delete_data_infos(InfosQOBD[] infos_array_list)
        {
            return new InfosQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<InfosQOBD[]> delete_data_infosAsync(InfosQOBD[] infos_array_list)
        {
            return Task.Factory.StartNew(() => { return new InfosQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] delete_data_item(ItemQOBD[] item_array_list)
        {
            return new ItemQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> delete_data_itemAsync(ItemQOBD[] item_array_list)
        {
            return Task.Factory.StartNew(() => { return new ItemQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] delete_data_item_delivery(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return new Item_deliveryQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> delete_data_item_deliveryAsync(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return Task.Factory.StartNew(() => { return new Item_deliveryQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] delete_data_notification(NotificationQOBD[] notification_array_list)
        {
            return new NotificationQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> delete_data_notificationAsync(NotificationQOBD[] notification_array_list)
        {
            return Task.Factory.StartNew(() => { return new NotificationQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public PrivilegeQOBD[] delete_data_privilege(PrivilegeQOBD[] privilege_array_list)
        {
            return new PrivilegeQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<PrivilegeQOBD[]> delete_data_privilegeAsync(PrivilegeQOBD[] privilege_array_list)
        {
            return Task.Factory.StartNew(() => { return new PrivilegeQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] delete_data_provider(ProviderQOBD[] provider_array_list)
        {
            return new ProviderQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> delete_data_providerAsync(ProviderQOBD[] provider_array_list)
        {
            return Task.Factory.StartNew(() => { return new ProviderQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] delete_data_provider_item(Provider_itemQOBD[] provider_item_array_list)
        {
            return new Provider_itemQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> delete_data_provider_itemAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return Task.Factory.StartNew(() => { return new Provider_itemQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public RoleQOBD[] delete_data_role(RoleQOBD[] role_array_list)
        {
            return new RoleQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<RoleQOBD[]> delete_data_roleAsync(RoleQOBD[] role_array_list)
        {
            return Task.Factory.StartNew(() => { return new RoleQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Role_actionQOBD[] delete_data_role_action(Role_actionQOBD[] role_action_array_list)
        {
            return new Role_actionQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Role_actionQOBD[]> delete_data_role_actionAsync(Role_actionQOBD[] role_action_array_list)
        {
            return Task.Factory.StartNew(() => { return new Role_actionQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public StatisticQOBD[] delete_data_statistic(StatisticQOBD[] statistic_array_list)
        {
            return new StatisticQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<StatisticQOBD[]> delete_data_statisticAsync(StatisticQOBD[] statistic_array_list)
        {
            return Task.Factory.StartNew(() => { return new StatisticQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public TaxQOBD[] delete_data_tax(TaxQOBD[] tax_array_list)
        {
            return new TaxQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<TaxQOBD[]> delete_data_taxAsync(TaxQOBD[] tax_array_list)
        {
            return Task.Factory.StartNew(() => { return new TaxQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] delete_data_tax_command(Tax_commandQOBD[] tax_command_array_list)
        {
            return new Tax_commandQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> delete_data_tax_commandAsync(Tax_commandQOBD[] tax_command_array_list)
        {
            return Task.Factory.StartNew(() => { return new Tax_commandQOBD[0]; });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] delete_data_tax_item(Tax_itemQOBD[] tax_item_array_list)
        {
            return new Tax_itemQOBD[0];
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> delete_data_tax_itemAsync(Tax_itemQOBD[] tax_item_array_list)
        {
            return Task.Factory.StartNew(() => { return new Tax_itemQOBD[0]; });
        }

        public void generate_pdf(PdfQOBD command_array)
        {
            
        }

        public Task generate_pdfAsync(PdfQOBD command_array)
        {
            return Task.Factory.StartNew(() => {  });
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD get_authenticate_user(string username, string password)
        {
            return new AgentQOBD { ID = 1, FirstName = "unitTestFirstname", LastName = "unitTestLastName" };
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD> get_authenticate_userAsync(string username, string password)
        {
            return Task.Factory.StartNew(() => { return new AgentQOBD { ID = 1, FirstName = "unitTestFirstname", LastName = "unitTestLastName" }; });
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] get_commands_client(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> get_commands_clientAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ActionQOBD[] get_data_action(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionQOBD[]> get_data_actionAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ActionRecordQOBD[] get_data_actionRecord(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionRecordQOBD[]> get_data_actionRecordAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ActionRecordQOBD[] get_data_actionRecord_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionRecordQOBD[]> get_data_actionRecord_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ActionQOBD[] get_data_action_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionQOBD[]> get_data_action_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] get_data_address(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> get_data_addressAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] get_data_address_by_client_list(ClientQOBD[] client_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> get_data_address_by_client_listAsync(ClientQOBD[] client_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] get_data_address_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> get_data_address_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] get_data_address_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> get_data_address_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] get_data_agent(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> get_data_agentAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] get_data_agent_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> get_data_agent_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] get_data_agent_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> get_data_agent_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] get_data_agent_credentail(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> get_data_agent_credentailAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Agent_roleQOBD[] get_data_agent_role(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Agent_roleQOBD[]> get_data_agent_roleAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Agent_roleQOBD[] get_data_agent_role_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Agent_roleQOBD[]> get_data_agent_role_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Auto_refsQOBD[] get_data_auto_ref(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Auto_refsQOBD[]> get_data_auto_refAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Auto_refsQOBD[] get_data_auto_ref_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Auto_refsQOBD[]> get_data_auto_ref_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] get_data_bill(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> get_data_billAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] get_data_bill_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> get_data_bill_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] get_data_bill_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> get_data_bill_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] get_data_bill_by_unpaid(int agent_id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> get_data_bill_by_unpaidAsync(int agent_id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] get_data_client(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> get_data_clientAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] get_data_client_by_bill_list(BillQOBD[] bill_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> get_data_client_by_bill_listAsync(BillQOBD[] bill_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] get_data_client_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> get_data_client_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] get_data_client_by_max_credit_over(int agent_id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> get_data_client_by_max_credit_overAsync(int agent_id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] get_data_command(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> get_data_commandAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] get_data_command_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> get_data_command_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] get_data_command_item(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> get_data_command_itemAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] get_data_command_item_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> get_data_command_item_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] get_data_command_item_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> get_data_command_item_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] get_data_contact(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> get_data_contactAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] get_data_contact_by_client_list(ClientQOBD[] client_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> get_data_contact_by_client_listAsync(ClientQOBD[] client_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] get_data_contact_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> get_data_contact_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] get_data_delivery(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> get_data_deliveryAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] get_data_delivery_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> get_data_delivery_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] get_data_delivery_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> get_data_delivery_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public InfosQOBD[] get_data_infos(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<InfosQOBD[]> get_data_infosAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public InfosQOBD[] get_data_infos_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<InfosQOBD[]> get_data_infos_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] get_data_item(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> get_data_itemAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] get_data_item_by_command_item_list(Command_itemQOBD[] command_item_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> get_data_item_by_command_item_listAsync(Command_itemQOBD[] command_item_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] get_data_item_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> get_data_item_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] get_data_item_delivery(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> get_data_item_deliveryAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] get_data_item_delivery_by_delivery_list(DeliveryQOBD[] delivery_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> get_data_item_delivery_by_delivery_listAsync(DeliveryQOBD[] delivery_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] get_data_item_delivery_by_id(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> get_data_item_delivery_by_idAsync(string id)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] get_data_notification(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> get_data_notificationAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] get_data_notification_by_client_list(ClientQOBD[] client_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> get_data_notification_by_client_listAsync(ClientQOBD[] client_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] get_data_notification_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> get_data_notification_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] get_data_notification_by_id(string id)
        {
            NotificationQOBD[] output = new NotificationQOBD[1];
            int i = 0;
            NotificationQOBD NotificationQOBD = new NotificationQOBD { ID = i, BillId = ++i, Date = "date" };
            output[i] = NotificationQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> get_data_notification_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                NotificationQOBD[] output = new NotificationQOBD[1];
                int i = 0;
                NotificationQOBD NotificationQOBD = new NotificationQOBD { ID = i, BillId = ++i, Date = "date" };
                output[i] = NotificationQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public PrivilegeQOBD[] get_data_privilege(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<PrivilegeQOBD[]> get_data_privilegeAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public PrivilegeQOBD[] get_data_privilege_by_id(string id)
        {
            PrivilegeQOBD[] output = new PrivilegeQOBD[1];
            int i = 0;
            PrivilegeQOBD PrivilegeQOBD = new PrivilegeQOBD { ID = i + "", Role_actionId = ++i + "", Date = "date" };
            output[i] = PrivilegeQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<PrivilegeQOBD[]> get_data_privilege_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                PrivilegeQOBD[] output = new PrivilegeQOBD[1];
                int i = 0;
                PrivilegeQOBD PrivilegeQOBD = new PrivilegeQOBD { ID = i+"", Role_actionId = ++i+"", Date = "date" };
                output[i] = PrivilegeQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] get_data_provider(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> get_data_providerAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] get_data_provider_by_id(string id)
        {
            ProviderQOBD[] output = new ProviderQOBD[1];
            int i = 0;
            ProviderQOBD ProviderQOBD = new ProviderQOBD { ID = i, Name = "name", Source = i };
            output[i] = ProviderQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> get_data_provider_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                ProviderQOBD[] output = new ProviderQOBD[1];
                int i = 0;
                ProviderQOBD ProviderQOBD = new ProviderQOBD { ID = i, Name = "name", Source = i };
                output[i] = ProviderQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] get_data_provider_by_provider_item_list(Provider_itemQOBD[] provider_item_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> get_data_provider_by_provider_item_listAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] get_data_provider_item(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> get_data_provider_itemAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] get_data_provider_item_by_id(string id)
        {
            Provider_itemQOBD[] output = new Provider_itemQOBD[1];
            int i = 0;
            Provider_itemQOBD Provider_itemQOBD = new Provider_itemQOBD { ID = i, Item_ref = "item ref", Provider_name = "provider name" };
            output[i] = Provider_itemQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> get_data_provider_item_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Provider_itemQOBD[] output = new Provider_itemQOBD[1];
                int i = 0;
                Provider_itemQOBD Provider_itemQOBD = new Provider_itemQOBD { ID = i, Item_ref = "item ref", Provider_name = "provider name" };
                output[i] = Provider_itemQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] get_data_provider_item_by_item_list(ItemQOBD[] item_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> get_data_provider_item_by_item_listAsync(ItemQOBD[] item_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public RoleQOBD[] get_data_role(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<RoleQOBD[]> get_data_roleAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Role_actionQOBD[] get_data_role_action(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Role_actionQOBD[]> get_data_role_actionAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Role_actionQOBD[] get_data_role_action_by_id(string id)
        {
            Role_actionQOBD[] output = new Role_actionQOBD[1];
            int i = 0;
            Role_actionQOBD Role_actionQOBD = new Role_actionQOBD { ID = i, ActionId = ++i, RoleId = i };
            output[i] = Role_actionQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<Role_actionQOBD[]> get_data_role_action_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Role_actionQOBD[] output = new Role_actionQOBD[1];
                int i = 0;
                Role_actionQOBD Role_actionQOBD = new Role_actionQOBD { ID = i, ActionId = ++i, RoleId = i  };
                output[i] = Role_actionQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public RoleQOBD[] get_data_role_by_id(string id)
        {
            RoleQOBD[] output = new RoleQOBD[1];
            int i = 0;
            RoleQOBD RoleQOBD = new RoleQOBD { ID = i, Actions = new ActionQOBD[10], Name = i + " name" };
            output[i] = RoleQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<RoleQOBD[]> get_data_role_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                RoleQOBD[] output = new RoleQOBD[1];
                int i = 0;
                RoleQOBD RoleQOBD = new RoleQOBD { ID = i, Actions = new ActionQOBD[10], Name = i + " name" };
                output[i] = RoleQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public StatisticQOBD[] get_data_statistic(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<StatisticQOBD[]> get_data_statisticAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public StatisticQOBD[] get_data_statistic_by_id(string id)
        {
            StatisticQOBD[] output = new StatisticQOBD[1];
            int i = 0;
            StatisticQOBD StatisticQOBD = new StatisticQOBD { ID = i, BillId = i + "", Company = i + " company", Bill_date = "date bill : " + i, Total = "total" };
            output[i] = StatisticQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<StatisticQOBD[]> get_data_statistic_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                StatisticQOBD[] output = new StatisticQOBD[1];
                int i = 0;
                StatisticQOBD StatisticQOBD = new StatisticQOBD { ID = i, BillId = i+"", Company = i+" company", Bill_date = "date bill : " + i, Total = "total" };
                output[i] = StatisticQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public TaxQOBD[] get_data_tax(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<TaxQOBD[]> get_data_taxAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public TaxQOBD[] get_data_tax_by_id(string id)
        {
            TaxQOBD[] output = new TaxQOBD[1];
            int i = 0;
            TaxQOBD TaxQOBD = new TaxQOBD { ID = i, Tax_current = i, Value = i, Type = "date insert : " + i, Comment = "comment" };
            output[i] = TaxQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<TaxQOBD[]> get_data_tax_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                TaxQOBD[] output = new TaxQOBD[1];
                int i = 0;
                TaxQOBD TaxQOBD = new TaxQOBD { ID = i, Tax_current = i, Value = i, Type = "date insert : " + i, Comment = "comment" };
                output[i] = TaxQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] get_data_tax_command(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> get_data_tax_commandAsync(string nbLine)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] get_data_tax_command_by_command_list(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> get_data_tax_command_by_command_listAsync(CommandsQOBD[] command_array_list)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] get_data_tax_command_by_id(string id)
        {
            Tax_commandQOBD[] output = new Tax_commandQOBD[1];
            int i = 0;
            Tax_commandQOBD Tax_commandQOBD = new Tax_commandQOBD { ID = i, CommandId = i, TaxId = i, Date_insert = "date insert : " + i, Tax_value = i };
            output[i] = Tax_commandQOBD;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> get_data_tax_command_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Tax_commandQOBD[] output = new Tax_commandQOBD[1];
                int i = 0;
                Tax_commandQOBD Tax_commandQOBD = new Tax_commandQOBD { ID = i, CommandId = i, TaxId = i, Date_insert = "date insert : " + i, Tax_value = i };
                output[i] = Tax_commandQOBD;

                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] get_data_tax_item(string nbLine)
        {
            Tax_itemQOBD[] output = new Tax_itemQOBD[Convert.ToInt32(nbLine)];
            for (int i = 0; i < Convert.ToInt32(nbLine); i++)
            {
                Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = "ref: " + i, TaxId = i, Tax_type = "tax type : " + i, Tax_value = i };
                output[i] = tax_item;
            }
            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> get_data_tax_itemAsync(string nbLine)
        {
            return Task.Factory.StartNew(() => {
                Tax_itemQOBD[] output = new Tax_itemQOBD[Convert.ToInt32(nbLine)];
                for (int i = 0; i < Convert.ToInt32(nbLine); i++)
                {
                    Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = "ref: " + i, TaxId = i, Tax_type = "tax type : " + i, Tax_value = i };
                    output[i] = tax_item;
                }
                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] get_data_tax_item_by_id(string id)
        {
            Tax_itemQOBD[] output = new Tax_itemQOBD[1];
            int i = 0;
            Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = "ref: " + i, TaxId = i, Tax_type = "tax type : " + i, Tax_value = i };
            output[i] = tax_item;

            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> get_data_tax_item_by_idAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                Tax_itemQOBD[] output = new Tax_itemQOBD[1];
                int i = 0;                
                Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = "ref: " + i, TaxId = i, Tax_type = "tax type : " + i, Tax_value = i };
                output[i] = tax_item;
                
                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] get_data_tax_item_by_item_list(ItemQOBD[] item_array_list)
        {
            Tax_itemQOBD[] output = new Tax_itemQOBD[item_array_list.Count()];
            for (int i = 0; i < item_array_list.Count(); i++)
            {
                Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = "ref: " + i, TaxId = i, Tax_type = "tax type : " + i, Tax_value = i };
                output[i] = tax_item;
            }
            return output;
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> get_data_tax_item_by_item_listAsync(ItemQOBD[] item_array_list)
        {
            return Task.Factory.StartNew(() => {
                Tax_itemQOBD[] output = new Tax_itemQOBD[Convert.ToInt32(item_array_list.Count())];
                for (int i = 0; i < item_array_list.Count(); i++)
                {
                    Tax_itemQOBD tax_item = new Tax_itemQOBD { ID = i, Item_ref = "ref: " + i, TaxId = i, Tax_type = "tax type : " + i, Tax_value = i };
                    output[i] = tax_item;
                }
                return output;
            });
        }

        [return: MessageParameter(Name = "return")]
        public ActionQOBD[] get_filter_action(ActionFilterQOBD action_array_list)
        {
            return new ActionQOBD[1] { new ActionQOBD { ID = action_array_list.ID, Name = action_array_list.Name, Right = new PrivilegeQOBD() } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionQOBD[]> get_filter_actionAsync(ActionFilterQOBD action_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new ActionQOBD[1] { new ActionQOBD { ID = action_array_list.ID, Name = action_array_list.Name, Right = new PrivilegeQOBD() } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public ActionRecordQOBD[] get_filter_actionRecord(ActionRecordFilterQOBD actionRecord_array_list)
        {
            return new ActionRecordQOBD[1] { new ActionRecordQOBD { ID = actionRecord_array_list.ID, AgentId = actionRecord_array_list.AgentId, TargetId = actionRecord_array_list.TargetId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionRecordQOBD[]> get_filter_actionRecordAsync(ActionRecordFilterQOBD actionRecord_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new ActionRecordQOBD[1] { new ActionRecordQOBD { ID = actionRecord_array_list.ID, AgentId = actionRecord_array_list.AgentId, TargetId = actionRecord_array_list.TargetId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] get_filter_address(AddressFilterQOBD address_array_list)
        {
            return new AddressQOBD[1] { new AddressQOBD { ID = address_array_list.ID, ClientId = address_array_list.ClientId, Name = address_array_list.Name } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> get_filter_addressAsync(AddressFilterQOBD address_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new AddressQOBD[1] { new AddressQOBD { ID = address_array_list.ID, ClientId = address_array_list.ClientId, Name = address_array_list.Name } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] get_filter_agent(AgentFilterQOBD agent_array_list_filter)
        {
            return new AgentQOBD[1] { new AgentQOBD { ID = agent_array_list_filter.ID, Login = agent_array_list_filter.Login, Password = agent_array_list_filter.Password } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> get_filter_agentAsync(AgentFilterQOBD agent_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new AgentQOBD[1] { new AgentQOBD { ID = agent_array_list_filter.ID, Login = agent_array_list_filter.Login, Password = agent_array_list_filter.Password } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Agent_roleQOBD[] get_filter_agent_role(Agent_roleFilterQOBD agent_role_array_list)
        {
            return new Agent_roleQOBD[1] { new Agent_roleQOBD { ID = agent_role_array_list.ID, AgentId = agent_role_array_list.AgentId, RoleId = agent_role_array_list.RoleId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Agent_roleQOBD[]> get_filter_agent_roleAsync(Agent_roleFilterQOBD agent_role_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new Agent_roleQOBD[1] { new Agent_roleQOBD { ID = agent_role_array_list.ID, AgentId = agent_role_array_list.AgentId, RoleId = agent_role_array_list.RoleId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Auto_refsQOBD[] get_filter_auto_ref(Auto_refsFilterQOBD auto_ref_array_list)
        {
            return new Auto_refsQOBD[1] { new Auto_refsQOBD { ID = auto_ref_array_list.ID, RefId = auto_ref_array_list.RefId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Auto_refsQOBD[]> get_filter_auto_refAsync(Auto_refsFilterQOBD auto_ref_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new Auto_refsQOBD[1] { new Auto_refsQOBD { ID = auto_ref_array_list.ID, RefId = auto_ref_array_list.RefId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] get_filter_bill(BillFilterQOBD bill_array_list)
        {
            return new BillQOBD[1] { new BillQOBD { ID = bill_array_list.ID, ClientId = bill_array_list.ClientId, CommandId = bill_array_list.CommandId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> get_filter_billAsync(BillFilterQOBD bill_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new BillQOBD[1] { new BillQOBD { ID = bill_array_list.ID, ClientId = bill_array_list.ClientId, CommandId = bill_array_list.CommandId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] get_filter_Client(ClientFilterQOBD client_array_list_filter)
        {
            return new ClientQOBD[1] { new ClientQOBD { ID = client_array_list_filter.ID, AgentId = client_array_list_filter.AgentId, Company = client_array_list_filter.Company } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> get_filter_ClientAsync(ClientFilterQOBD client_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ClientQOBD[1] { new ClientQOBD { ID = client_array_list_filter.ID, AgentId = client_array_list_filter.AgentId, Company = client_array_list_filter.Company } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] get_filter_command(CommandFilterQOBD command_array_list_filter)
        {
            return new CommandsQOBD[1] { new CommandsQOBD { ID = command_array_list_filter.ID, AgentId = command_array_list_filter.AgentId, ClientId = command_array_list_filter.ClientId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> get_filter_commandAsync(CommandFilterQOBD command_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new CommandsQOBD[1] { new CommandsQOBD { ID = command_array_list_filter.ID, AgentId = command_array_list_filter.AgentId, ClientId = command_array_list_filter.ClientId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] get_filter_command_item(Command_itemFilterQOBD command_item_array_list_filter)
        {
            return new Command_itemQOBD[1] { new Command_itemQOBD { ID = command_item_array_list_filter.ID, CommandId = command_item_array_list_filter.CommandId, Item_ref = command_item_array_list_filter.Item_ref } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> get_filter_command_itemAsync(Command_itemFilterQOBD command_item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Command_itemQOBD[1] { new Command_itemQOBD { ID = command_item_array_list_filter.ID, CommandId = command_item_array_list_filter.CommandId, Item_ref = command_item_array_list_filter.Item_ref } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] get_filter_contact(ContactFilterQOBD contact_array_list_filter)
        {
            return new ContactQOBD[1] { new ContactQOBD { ID = contact_array_list_filter.ID, ClientId = contact_array_list_filter.ClientId, Firstname = contact_array_list_filter.Firstname } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> get_filter_contactAsync(ContactFilterQOBD contact_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ContactQOBD[1] { new ContactQOBD { ID = contact_array_list_filter.ID, ClientId = contact_array_list_filter.ClientId, Firstname = contact_array_list_filter.Firstname } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] get_filter_delivery(DeliveryFilterQOBD delivery_array_list_filter)
        {
            return new DeliveryQOBD[1] { new DeliveryQOBD { ID = delivery_array_list_filter.ID, BillId = delivery_array_list_filter.BillId, CommandId = delivery_array_list_filter.CommandId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> get_filter_deliveryAsync(DeliveryFilterQOBD delivery_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new DeliveryQOBD[1] { new DeliveryQOBD { ID = delivery_array_list_filter.ID, BillId = delivery_array_list_filter.BillId, CommandId = delivery_array_list_filter.CommandId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public InfosQOBD[] get_filter_infos(InfosFilterQOBD infos_array_list_filter)
        {
            return new InfosQOBD[1] { new InfosQOBD { ID = infos_array_list_filter.ID, Name = infos_array_list_filter.Name, Value = infos_array_list_filter.Value } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<InfosQOBD[]> get_filter_infosAsync(InfosFilterQOBD infos_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new InfosQOBD[1] { new InfosQOBD { ID = infos_array_list_filter.ID, Name = infos_array_list_filter.Name, Value = infos_array_list_filter.Value } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] get_filter_item(ItemFilterQOBD item_array_list_filter)
        {
            return new ItemQOBD[1] { new ItemQOBD { ID = item_array_list_filter.ID, Ref = item_array_list_filter.Ref, Source = item_array_list_filter.Source } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> get_filter_itemAsync(ItemFilterQOBD item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ItemQOBD[1] { new ItemQOBD { ID = item_array_list_filter.ID, Ref = item_array_list_filter.Ref, Source = item_array_list_filter.Source } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] get_filter_item_delivery(Item_deliveryFilterQOBD item_delivery_array_list_filter)
        {
            return new Item_deliveryQOBD[1] { new Item_deliveryQOBD { ID = item_delivery_array_list_filter.ID, DeliveryId = item_delivery_array_list_filter.DeliveryId, Item_ref = item_delivery_array_list_filter.Item_ref } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> get_filter_item_deliveryAsync(Item_deliveryFilterQOBD item_delivery_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Item_deliveryQOBD[1] { new Item_deliveryQOBD { ID = item_delivery_array_list_filter.ID, DeliveryId = item_delivery_array_list_filter.DeliveryId, Item_ref = item_delivery_array_list_filter.Item_ref } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] get_filter_notification(NotificationFilterQOBD notification_array_list)
        {
            return new NotificationQOBD[1] { new NotificationQOBD { ID = notification_array_list.ID, BillId = notification_array_list.BillId, Date = notification_array_list.Date } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> get_filter_notificationAsync(NotificationFilterQOBD notification_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new NotificationQOBD[1] { new NotificationQOBD { ID = notification_array_list.ID, BillId = notification_array_list.BillId, Date = notification_array_list.Date } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public PrivilegeQOBD[] get_filter_privilege(PrivilegeFilterQOBD privilege_array_list_filter)
        {
            return new PrivilegeQOBD[1] { new PrivilegeQOBD { ID = privilege_array_list_filter.ID, Role_actionId = privilege_array_list_filter.Role_actionId, Date = privilege_array_list_filter.Date } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<PrivilegeQOBD[]> get_filter_privilegeAsync(PrivilegeFilterQOBD privilege_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new PrivilegeQOBD[1] { new PrivilegeQOBD { ID = privilege_array_list_filter.ID, Role_actionId = privilege_array_list_filter.Role_actionId, Date = privilege_array_list_filter.Date } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] get_filter_provider(ProviderFilterQOBD provider_array_list_filter)
        {
            return new ProviderQOBD[1] { new ProviderQOBD { ID = provider_array_list_filter.ID, Name = provider_array_list_filter.Name, Source = provider_array_list_filter.Source } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> get_filter_providerAsync(ProviderFilterQOBD provider_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new ProviderQOBD[1] { new ProviderQOBD { ID = provider_array_list_filter.ID, Name = provider_array_list_filter.Name, Source = provider_array_list_filter.Source } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] get_filter_provider_item(Provider_itemFilterQOBD provider_item_array_list_filter)
        {
            return new Provider_itemQOBD[1] { new Provider_itemQOBD { ID = provider_item_array_list_filter.ID, Item_ref = provider_item_array_list_filter.Item_ref, Provider_name = provider_item_array_list_filter.Provider_name } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> get_filter_provider_itemAsync(Provider_itemFilterQOBD provider_item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Provider_itemQOBD[1] { new Provider_itemQOBD { ID = provider_item_array_list_filter.ID, Item_ref = provider_item_array_list_filter.Item_ref, Provider_name = provider_item_array_list_filter.Provider_name } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public RoleQOBD[] get_filter_role(RoleFilterQOBD role_array_list)
        {
            return new RoleQOBD[1] { new RoleQOBD { ID = role_array_list.ID, Actions = new ActionQOBD[2], Name = role_array_list.Name } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<RoleQOBD[]> get_filter_roleAsync(RoleFilterQOBD role_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new RoleQOBD[1] { new RoleQOBD { ID = role_array_list.ID, Actions = new ActionQOBD[2], Name = role_array_list.Name } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Role_actionQOBD[] get_filter_role_action(Role_actionFilterQOBD role_action_array_list)
        {
            return new Role_actionQOBD[1] { new Role_actionQOBD { ID = role_action_array_list.ID, ActionId = role_action_array_list.ActionId, RoleId = role_action_array_list.RoleId } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Role_actionQOBD[]> get_filter_role_actionAsync(Role_actionFilterQOBD role_action_array_list)
        {
            return Task.Factory.StartNew(() => {
                return new Role_actionQOBD[1] { new Role_actionQOBD { ID = role_action_array_list.ID, ActionId = role_action_array_list.ActionId, RoleId = role_action_array_list.RoleId } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public StatisticQOBD[] get_filter_statistic(StatisticFilterQOBD statistic_array_list_filter)
        {
            return new StatisticQOBD[1] { new StatisticQOBD { ID = statistic_array_list_filter.ID, Total = statistic_array_list_filter.Total, BillId = statistic_array_list_filter.BillId, Company = statistic_array_list_filter.Company } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<StatisticQOBD[]> get_filter_statisticAsync(StatisticFilterQOBD statistic_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new StatisticQOBD[1] { new StatisticQOBD { ID = statistic_array_list_filter.ID, Total = statistic_array_list_filter.Total, BillId = statistic_array_list_filter.BillId, Company = statistic_array_list_filter.Company } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public TaxQOBD[] get_filter_tax(TaxFilterQOBD tax_array_list_filter)
        {
            return new TaxQOBD[1] { new TaxQOBD { ID = tax_array_list_filter.ID, Comment = tax_array_list_filter.Comment, Tax_current = tax_array_list_filter.Tax_current, Type = tax_array_list_filter.Type, Value = tax_array_list_filter.Value, Date_insert = tax_array_list_filter.Date_insert } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<TaxQOBD[]> get_filter_taxAsync(TaxFilterQOBD tax_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new TaxQOBD[1] { new TaxQOBD { ID = tax_array_list_filter.ID, Comment = tax_array_list_filter.Comment, Tax_current = tax_array_list_filter.Tax_current, Type = tax_array_list_filter.Type, Value = tax_array_list_filter.Value, Date_insert = tax_array_list_filter.Date_insert } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] get_filter_tax_command(Tax_commandFilterQOBD tax_command_array_list_filter)
        {
            return new Tax_commandQOBD[1] { new Tax_commandQOBD { ID = tax_command_array_list_filter.ID, CommandId = tax_command_array_list_filter.CommandId, TaxId = tax_command_array_list_filter.TaxId, Date_insert = tax_command_array_list_filter.Date_insert, Target = tax_command_array_list_filter.Target } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> get_filter_tax_commandAsync(Tax_commandFilterQOBD tax_command_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Tax_commandQOBD[1] { new Tax_commandQOBD { ID = tax_command_array_list_filter.ID, CommandId = tax_command_array_list_filter.CommandId, TaxId = tax_command_array_list_filter.TaxId, Date_insert = tax_command_array_list_filter.Date_insert, Target = tax_command_array_list_filter.Target } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] get_filter_tax_item(Tax_itemFilterQOBD tax_item_array_list_filter)
        {
            return new Tax_itemQOBD[1] { new Tax_itemQOBD { ID = tax_item_array_list_filter.ID, Item_ref = tax_item_array_list_filter.Item_ref, TaxId = tax_item_array_list_filter.TaxId, Tax_type = tax_item_array_list_filter.Tax_type, Tax_value = tax_item_array_list_filter.Tax_value } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> get_filter_tax_itemAsync(Tax_itemFilterQOBD tax_item_array_list_filter)
        {
            return Task.Factory.StartNew(() => {
                return new Tax_itemQOBD[1] { new Tax_itemQOBD { ID = tax_item_array_list_filter.ID, Item_ref = tax_item_array_list_filter.Item_ref, TaxId = tax_item_array_list_filter.TaxId, Tax_type = tax_item_array_list_filter.Tax_type, Tax_value = tax_item_array_list_filter.Tax_value } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] get_quotes_client(string id)
        {
            return new CommandsQOBD[1] { new CommandsQOBD { ClientId = Convert.ToInt32(id) } };
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> get_quotes_clientAsync(string id)
        {
            return Task.Factory.StartNew(() => {
                
                return new CommandsQOBD[1] { new CommandsQOBD { ClientId = Convert.ToInt32(id) } };
            });
        }

        [return: MessageParameter(Name = "return")]
        public ActionQOBD[] insert_data_action(ActionQOBD[] action_array_list)
        {
            return get_data_action(action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionQOBD[]> insert_data_actionAsync(ActionQOBD[] action_array_list)
        {
            return get_data_actionAsync(action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ActionRecordQOBD[] insert_data_actionRecord(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecord(actionRecord_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionRecordQOBD[]> insert_data_actionRecordAsync(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecordAsync(actionRecord_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] insert_data_address(AddressQOBD[] address_array_list)
        {
            return get_data_address(address_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> insert_data_addressAsync(AddressQOBD[] address_array_list)
        {
            return get_data_addressAsync(address_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] insert_data_agent(AgentQOBD[] agent_array_list)
        {
            return get_data_agent(agent_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> insert_data_agentAsync(AgentQOBD[] agent_array_list)
        {
            return get_data_agentAsync(agent_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Agent_roleQOBD[] insert_data_agent_role(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_role(agent_role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Agent_roleQOBD[]> insert_data_agent_roleAsync(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_roleAsync(agent_role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Auto_refsQOBD[] insert_data_auto_ref(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_ref(auto_ref_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Auto_refsQOBD[]> insert_data_auto_refAsync(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_refAsync(auto_ref_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] insert_data_bill(BillQOBD[] bill_array_list)
        {
            return get_data_bill(bill_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> insert_data_billAsync(BillQOBD[] bill_array_list)
        {
            return get_data_billAsync(bill_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] insert_data_client(ClientQOBD[] client_array_list)
        {
            return get_data_client(client_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> insert_data_clientAsync(ClientQOBD[] client_array_list)
        {
            return get_data_clientAsync(client_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] insert_data_command(CommandsQOBD[] command_array_list)
        {
            return get_data_command(command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> insert_data_commandAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_commandAsync(command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] insert_data_command_item(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_item(command_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> insert_data_command_itemAsync(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_itemAsync(command_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] insert_data_contact(ContactQOBD[] contact_array_list)
        {
            return get_data_contact(contact_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> insert_data_contactAsync(ContactQOBD[] contact_array_list)
        {
            return get_data_contactAsync(contact_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] insert_data_delivery(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_delivery(delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> insert_data_deliveryAsync(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_deliveryAsync(delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public InfosQOBD[] insert_data_infos(InfosQOBD[] infos_array_list)
        {
            return get_data_infos(infos_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<InfosQOBD[]> insert_data_infosAsync(InfosQOBD[] infos_array_list)
        {
            return get_data_infosAsync(infos_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] insert_data_item(ItemQOBD[] item_array_list)
        {
            return get_data_item(item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> insert_data_itemAsync(ItemQOBD[] item_array_list)
        {
            return get_data_itemAsync(item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] insert_data_item_delivery(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_delivery(item_delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> insert_data_item_deliveryAsync(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_deliveryAsync(item_delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] insert_data_notification(NotificationQOBD[] notification_array_list)
        {
            return get_data_notification(notification_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> insert_data_notificationAsync(NotificationQOBD[] notification_array_list)
        {
            return get_data_notificationAsync(notification_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public PrivilegeQOBD[] insert_data_privilege(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilege(privilege_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<PrivilegeQOBD[]> insert_data_privilegeAsync(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilegeAsync(privilege_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] insert_data_provider(ProviderQOBD[] provider_array_list)
        {
            return get_data_provider(provider_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> insert_data_providerAsync(ProviderQOBD[] provider_array_list)
        {
            return get_data_providerAsync(provider_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] insert_data_provider_item(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_item(provider_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> insert_data_provider_itemAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_itemAsync(provider_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public RoleQOBD[] insert_data_role(RoleQOBD[] role_array_list)
        {
            return get_data_role(role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<RoleQOBD[]> insert_data_roleAsync(RoleQOBD[] role_array_list)
        {
            return get_data_roleAsync(role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Role_actionQOBD[] insert_data_role_action(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_action(role_action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Role_actionQOBD[]> insert_data_role_actionAsync(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_actionAsync(role_action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public StatisticQOBD[] insert_data_statistic(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statistic(statistic_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<StatisticQOBD[]> insert_data_statisticAsync(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statisticAsync(statistic_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public TaxQOBD[] insert_data_tax(TaxQOBD[] tax_array_list)
        {
            return get_data_tax(tax_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<TaxQOBD[]> insert_data_taxAsync(TaxQOBD[] tax_array_list)
        {
            return get_data_taxAsync(tax_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] insert_data_tax_command(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_command(tax_command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> insert_data_tax_commandAsync(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_commandAsync(tax_command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] insert_data_tax_item(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_item(tax_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> insert_data_tax_itemAsync(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_itemAsync(tax_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public string send_email_to_client(EmailQOBD client_email)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public Task<string> send_email_to_clientAsync(EmailQOBD client_email)
        {
            throw new NotImplementedException();
        }

        [return: MessageParameter(Name = "return")]
        public ActionQOBD[] update_data_action(ActionQOBD[] action_array_list)
        {
            return get_data_action(action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionQOBD[]> update_data_actionAsync(ActionQOBD[] action_array_list)
        {
            return get_data_actionAsync(action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ActionRecordQOBD[] update_data_actionRecord(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecord(actionRecord_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ActionRecordQOBD[]> update_data_actionRecordAsync(ActionRecordQOBD[] actionRecord_array_list)
        {
            return get_data_actionRecordAsync(actionRecord_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public AddressQOBD[] update_data_address(AddressQOBD[] address_array_list)
        {
            return get_data_address(address_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<AddressQOBD[]> update_data_addressAsync(AddressQOBD[] address_array_list)
        {
            return get_data_addressAsync(address_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public AgentQOBD[] update_data_agent(AgentQOBD[] agent_array_list)
        {
            return get_data_agent(agent_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<AgentQOBD[]> update_data_agentAsync(AgentQOBD[] agent_array_list)
        {
            return get_data_agentAsync(agent_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Agent_roleQOBD[] update_data_agent_role(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_role(agent_role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Agent_roleQOBD[]> update_data_agent_roleAsync(Agent_roleQOBD[] agent_role_array_list)
        {
            return get_data_agent_roleAsync(agent_role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Auto_refsQOBD[] update_data_auto_ref(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_ref(auto_ref_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Auto_refsQOBD[]> update_data_auto_refAsync(Auto_refsQOBD[] auto_ref_array_list)
        {
            return get_data_auto_refAsync(auto_ref_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public BillQOBD[] update_data_bill(BillQOBD[] bill_array_list)
        {
            return get_data_bill(bill_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<BillQOBD[]> update_data_billAsync(BillQOBD[] bill_array_list)
        {
            return get_data_billAsync(bill_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ClientQOBD[] update_data_client(ClientQOBD[] client_array_list)
        {
            return get_data_client(client_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ClientQOBD[]> update_data_clientAsync(ClientQOBD[] client_array_list)
        {
            return get_data_clientAsync(client_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public CommandsQOBD[] update_data_command(CommandsQOBD[] command_array_list)
        {
            return get_data_command(command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<CommandsQOBD[]> update_data_commandAsync(CommandsQOBD[] command_array_list)
        {
            return get_data_commandAsync(command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Command_itemQOBD[] update_data_command_item(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_item(command_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Command_itemQOBD[]> update_data_command_itemAsync(Command_itemQOBD[] command_item_array_list)
        {
            return get_data_command_itemAsync(command_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ContactQOBD[] update_data_contact(ContactQOBD[] contact_array_list)
        {
            return get_data_contact(contact_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ContactQOBD[]> update_data_contactAsync(ContactQOBD[] contact_array_list)
        {
            return get_data_contactAsync(contact_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public DeliveryQOBD[] update_data_delivery(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_delivery(delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<DeliveryQOBD[]> update_data_deliveryAsync(DeliveryQOBD[] delivery_array_list)
        {
            return get_data_deliveryAsync(delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public InfosQOBD[] update_data_infos(InfosQOBD[] infos_array_list)
        {
            return get_data_infos(infos_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<InfosQOBD[]> update_data_infosAsync(InfosQOBD[] infos_array_list)
        {
            return get_data_infosAsync(infos_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ItemQOBD[] update_data_item(ItemQOBD[] item_array_list)
        {
            return get_data_item(item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ItemQOBD[]> update_data_itemAsync(ItemQOBD[] item_array_list)
        {
            return get_data_itemAsync(item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Item_deliveryQOBD[] update_data_item_delivery(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_delivery(item_delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Item_deliveryQOBD[]> update_data_item_deliveryAsync(Item_deliveryQOBD[] item_delivery_array_list)
        {
            return get_data_item_deliveryAsync(item_delivery_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public NotificationQOBD[] update_data_notification(NotificationQOBD[] notification_array_list)
        {
            return get_data_notification(notification_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<NotificationQOBD[]> update_data_notificationAsync(NotificationQOBD[] notification_array_list)
        {
            return get_data_notificationAsync(notification_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public PrivilegeQOBD[] update_data_privilege(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilege(privilege_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<PrivilegeQOBD[]> update_data_privilegeAsync(PrivilegeQOBD[] privilege_array_list)
        {
            return get_data_privilegeAsync(privilege_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public ProviderQOBD[] update_data_provider(ProviderQOBD[] provider_array_list)
        {
            return get_data_provider(provider_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<ProviderQOBD[]> update_data_providerAsync(ProviderQOBD[] provider_array_list)
        {
            return get_data_providerAsync(provider_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Provider_itemQOBD[] update_data_provider_item(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_item(provider_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Provider_itemQOBD[]> update_data_provider_itemAsync(Provider_itemQOBD[] provider_item_array_list)
        {
            return get_data_provider_itemAsync(provider_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public RoleQOBD[] update_data_role(RoleQOBD[] role_array_list)
        {
            return get_data_role(role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<RoleQOBD[]> update_data_roleAsync(RoleQOBD[] role_array_list)
        {
            return get_data_roleAsync(role_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Role_actionQOBD[] update_data_role_action(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_action(role_action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Role_actionQOBD[]> update_data_role_actionAsync(Role_actionQOBD[] role_action_array_list)
        {
            return get_data_role_actionAsync(role_action_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public StatisticQOBD[] update_data_statistic(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statistic(statistic_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<StatisticQOBD[]> update_data_statisticAsync(StatisticQOBD[] statistic_array_list)
        {
            return get_data_statisticAsync(statistic_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public TaxQOBD[] update_data_tax(TaxQOBD[] tax_array_list)
        {
            return get_data_tax(tax_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<TaxQOBD[]> update_data_taxAsync(TaxQOBD[] tax_array_list)
        {
            return get_data_taxAsync(tax_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Tax_commandQOBD[] update_data_tax_command(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_command(tax_command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_commandQOBD[]> update_data_tax_commandAsync(Tax_commandQOBD[] tax_command_array_list)
        {
            return get_data_tax_commandAsync(tax_command_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Tax_itemQOBD[] update_data_tax_item(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_item(tax_item_array_list.Count().ToString());
        }

        [return: MessageParameter(Name = "return")]
        public Task<Tax_itemQOBD[]> update_data_tax_itemAsync(Tax_itemQOBD[] tax_item_array_list)
        {
            return get_data_tax_itemAsync(tax_item_array_list.Count().ToString());
        }
    }
}
