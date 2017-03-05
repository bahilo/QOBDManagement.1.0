using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using QOBDGateway.QOBDServiceReference;

namespace QOBDGateway.Classes
{
    public class ClientProxy : System.ServiceModel.ClientBase<QOBDGateway.QOBDServiceReference.QOBDWebServicePortType>, QOBDGateway.QOBDServiceReference.QOBDWebServicePortType
    {
        public ClientProxy()
        {
        }

        public ClientProxy(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }

        public ClientProxy(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }

        public ClientProxy(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }

        public ClientProxy(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD get_authenticate_user(string username, string password)
        {
            return base.Channel.get_authenticate_user(username, password);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD> get_authenticate_userAsync(string username, string password)
        {
            return base.Channel.get_authenticate_userAsync(username, password);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionQOBD[] get_data_action(string nbLine)
        {
            return base.Channel.get_data_action(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionQOBD[]> get_data_actionAsync(string nbLine)
        {
            return base.Channel.get_data_actionAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionQOBD[] delete_data_action(QOBDGateway.QOBDServiceReference.ActionQOBD[] action_array_list)
        {
            return base.Channel.delete_data_action(action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionQOBD[]> delete_data_actionAsync(QOBDGateway.QOBDServiceReference.ActionQOBD[] action_array_list)
        {
            return base.Channel.delete_data_actionAsync(action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionQOBD[] insert_data_action(QOBDGateway.QOBDServiceReference.ActionQOBD[] action_array_list)
        {
            return base.Channel.insert_data_action(action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionQOBD[]> insert_data_actionAsync(QOBDGateway.QOBDServiceReference.ActionQOBD[] action_array_list)
        {
            return base.Channel.insert_data_actionAsync(action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionQOBD[] update_data_action(QOBDGateway.QOBDServiceReference.ActionQOBD[] action_array_list)
        {
            return base.Channel.update_data_action(action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionQOBD[]> update_data_actionAsync(QOBDGateway.QOBDServiceReference.ActionQOBD[] action_array_list)
        {
            return base.Channel.update_data_actionAsync(action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionQOBD[] get_data_action_by_id(string id)
        {
            return base.Channel.get_data_action_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionQOBD[]> get_data_action_by_idAsync(string id)
        {
            return base.Channel.get_data_action_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionQOBD[] get_filter_action(QOBDGateway.QOBDServiceReference.ActionFilterQOBD action_array_list)
        {
            return base.Channel.get_filter_action(action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionQOBD[]> get_filter_actionAsync(QOBDGateway.QOBDServiceReference.ActionFilterQOBD action_array_list)
        {
            return base.Channel.get_filter_actionAsync(action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] get_data_agent_role(string nbLine)
        {
            return base.Channel.get_data_agent_role(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Agent_roleQOBD[]> get_data_agent_roleAsync(string nbLine)
        {
            return base.Channel.get_data_agent_roleAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] delete_data_agent_role(QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] agent_role_array_list)
        {
            return base.Channel.delete_data_agent_role(agent_role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Agent_roleQOBD[]> delete_data_agent_roleAsync(QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] agent_role_array_list)
        {
            return base.Channel.delete_data_agent_roleAsync(agent_role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] insert_data_agent_role(QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] agent_role_array_list)
        {
            return base.Channel.insert_data_agent_role(agent_role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Agent_roleQOBD[]> insert_data_agent_roleAsync(QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] agent_role_array_list)
        {
            return base.Channel.insert_data_agent_roleAsync(agent_role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] update_data_agent_role(QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] agent_role_array_list)
        {
            return base.Channel.update_data_agent_role(agent_role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Agent_roleQOBD[]> update_data_agent_roleAsync(QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] agent_role_array_list)
        {
            return base.Channel.update_data_agent_roleAsync(agent_role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] get_data_agent_role_by_id(string id)
        {
            return base.Channel.get_data_agent_role_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Agent_roleQOBD[]> get_data_agent_role_by_idAsync(string id)
        {
            return base.Channel.get_data_agent_role_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Agent_roleQOBD[] get_filter_agent_role(QOBDGateway.QOBDServiceReference.Agent_roleFilterQOBD agent_role_array_list)
        {
            return base.Channel.get_filter_agent_role(agent_role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Agent_roleQOBD[]> get_filter_agent_roleAsync(QOBDGateway.QOBDServiceReference.Agent_roleFilterQOBD agent_role_array_list)
        {
            return base.Channel.get_filter_agent_roleAsync(agent_role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.RoleQOBD[] get_data_role(string nbLine)
        {
            return base.Channel.get_data_role(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.RoleQOBD[]> get_data_roleAsync(string nbLine)
        {
            return base.Channel.get_data_roleAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.RoleQOBD[] delete_data_role(QOBDGateway.QOBDServiceReference.RoleQOBD[] role_array_list)
        {
            return base.Channel.delete_data_role(role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.RoleQOBD[]> delete_data_roleAsync(QOBDGateway.QOBDServiceReference.RoleQOBD[] role_array_list)
        {
            return base.Channel.delete_data_roleAsync(role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.RoleQOBD[] insert_data_role(QOBDGateway.QOBDServiceReference.RoleQOBD[] role_array_list)
        {
            return base.Channel.insert_data_role(role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.RoleQOBD[]> insert_data_roleAsync(QOBDGateway.QOBDServiceReference.RoleQOBD[] role_array_list)
        {
            return base.Channel.insert_data_roleAsync(role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.RoleQOBD[] update_data_role(QOBDGateway.QOBDServiceReference.RoleQOBD[] role_array_list)
        {
            return base.Channel.update_data_role(role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.RoleQOBD[]> update_data_roleAsync(QOBDGateway.QOBDServiceReference.RoleQOBD[] role_array_list)
        {
            return base.Channel.update_data_roleAsync(role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.RoleQOBD[] get_data_role_by_id(string id)
        {
            return base.Channel.get_data_role_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.RoleQOBD[]> get_data_role_by_idAsync(string id)
        {
            return base.Channel.get_data_role_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.RoleQOBD[] get_filter_role(QOBDGateway.QOBDServiceReference.RoleFilterQOBD role_array_list)
        {
            return base.Channel.get_filter_role(role_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.RoleQOBD[]> get_filter_roleAsync(QOBDGateway.QOBDServiceReference.RoleFilterQOBD role_array_list)
        {
            return base.Channel.get_filter_roleAsync(role_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Role_actionQOBD[] get_data_role_action(string nbLine)
        {
            return base.Channel.get_data_role_action(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Role_actionQOBD[]> get_data_role_actionAsync(string nbLine)
        {
            return base.Channel.get_data_role_actionAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Role_actionQOBD[] delete_data_role_action(QOBDGateway.QOBDServiceReference.Role_actionQOBD[] role_action_array_list)
        {
            return base.Channel.delete_data_role_action(role_action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Role_actionQOBD[]> delete_data_role_actionAsync(QOBDGateway.QOBDServiceReference.Role_actionQOBD[] role_action_array_list)
        {
            return base.Channel.delete_data_role_actionAsync(role_action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Role_actionQOBD[] insert_data_role_action(QOBDGateway.QOBDServiceReference.Role_actionQOBD[] role_action_array_list)
        {
            return base.Channel.insert_data_role_action(role_action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Role_actionQOBD[]> insert_data_role_actionAsync(QOBDGateway.QOBDServiceReference.Role_actionQOBD[] role_action_array_list)
        {
            return base.Channel.insert_data_role_actionAsync(role_action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Role_actionQOBD[] update_data_role_action(QOBDGateway.QOBDServiceReference.Role_actionQOBD[] role_action_array_list)
        {
            return base.Channel.update_data_role_action(role_action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Role_actionQOBD[]> update_data_role_actionAsync(QOBDGateway.QOBDServiceReference.Role_actionQOBD[] role_action_array_list)
        {
            return base.Channel.update_data_role_actionAsync(role_action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Role_actionQOBD[] get_data_role_action_by_id(string id)
        {
            return base.Channel.get_data_role_action_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Role_actionQOBD[]> get_data_role_action_by_idAsync(string id)
        {
            return base.Channel.get_data_role_action_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Role_actionQOBD[] get_filter_role_action(QOBDGateway.QOBDServiceReference.Role_actionFilterQOBD role_action_array_list)
        {
            return base.Channel.get_filter_role_action(role_action_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Role_actionQOBD[]> get_filter_role_actionAsync(QOBDGateway.QOBDServiceReference.Role_actionFilterQOBD role_action_array_list)
        {
            return base.Channel.get_filter_role_actionAsync(role_action_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] get_data_bill(string nbLine)
        {
            return base.Channel.get_data_bill(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> get_data_billAsync(string nbLine)
        {
            return base.Channel.get_data_billAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] delete_data_bill(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.delete_data_bill(bill_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> delete_data_billAsync(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.delete_data_billAsync(bill_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] insert_data_bill(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.insert_data_bill(bill_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> insert_data_billAsync(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.insert_data_billAsync(bill_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] update_data_bill(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.update_data_bill(bill_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> update_data_billAsync(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.update_data_billAsync(bill_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] get_data_bill_by_id(string id)
        {
            return base.Channel.get_data_bill_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> get_data_bill_by_idAsync(string id)
        {
            return base.Channel.get_data_bill_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] get_filter_bill(QOBDGateway.QOBDServiceReference.BillFilterQOBD bill_array_list)
        {
            return base.Channel.get_filter_bill(bill_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> get_filter_billAsync(QOBDGateway.QOBDServiceReference.BillFilterQOBD bill_array_list)
        {
            return base.Channel.get_filter_billAsync(bill_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] get_data_bill_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_bill_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> get_data_bill_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_bill_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.BillQOBD[] get_data_bill_by_unpaid(int agent_id)
        {
            return base.Channel.get_data_bill_by_unpaid(agent_id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.BillQOBD[]> get_data_bill_by_unpaidAsync(int agent_id)
        {
            return base.Channel.get_data_bill_by_unpaidAsync(agent_id);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] get_data_order(string nbLine)
        {
            return base.Channel.get_data_order(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> get_data_orderAsync(string nbLine)
        {
            return base.Channel.get_data_orderAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] delete_data_order(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.delete_data_order(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> delete_data_orderAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.delete_data_orderAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] insert_data_order(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.insert_data_order(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> insert_data_orderAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.insert_data_orderAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] update_data_order(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.update_data_order(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> update_data_orderAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.update_data_orderAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] get_data_order_by_id(string id)
        {
            return base.Channel.get_data_order_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> get_data_order_by_idAsync(string id)
        {
            return base.Channel.get_data_order_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] get_filter_order(QOBDGateway.QOBDServiceReference.OrderFilterQOBD order_array_list_filter)
        {
            return base.Channel.get_filter_order(order_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> get_filter_orderAsync(QOBDGateway.QOBDServiceReference.OrderFilterQOBD order_array_list_filter)
        {
            return base.Channel.get_filter_orderAsync(order_array_list_filter);
        }

        public virtual void generate_pdf(QOBDGateway.QOBDServiceReference.PdfQOBD order_array)
        {
            base.Channel.generate_pdf(order_array);
        }

        public virtual System.Threading.Tasks.Task generate_pdfAsync(QOBDGateway.QOBDServiceReference.PdfQOBD order_array)
        {
            return base.Channel.generate_pdfAsync(order_array);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] get_data_agent(string nbLine)
        {
            return base.Channel.get_data_agent(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> get_data_agentAsync(string nbLine)
        {
            return base.Channel.get_data_agentAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] get_data_agent_credentail(string nbLine)
        {
            return base.Channel.get_data_agent_credentail(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> get_data_agent_credentailAsync(string nbLine)
        {
            return base.Channel.get_data_agent_credentailAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] delete_data_agent(QOBDGateway.QOBDServiceReference.AgentQOBD[] agent_array_list)
        {
            return base.Channel.delete_data_agent(agent_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> delete_data_agentAsync(QOBDGateway.QOBDServiceReference.AgentQOBD[] agent_array_list)
        {
            return base.Channel.delete_data_agentAsync(agent_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] insert_data_agent(QOBDGateway.QOBDServiceReference.AgentQOBD[] agent_array_list)
        {
            return base.Channel.insert_data_agent(agent_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> insert_data_agentAsync(QOBDGateway.QOBDServiceReference.AgentQOBD[] agent_array_list)
        {
            return base.Channel.insert_data_agentAsync(agent_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] update_data_agent(QOBDGateway.QOBDServiceReference.AgentQOBD[] agent_array_list)
        {
            return base.Channel.update_data_agent(agent_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> update_data_agentAsync(QOBDGateway.QOBDServiceReference.AgentQOBD[] agent_array_list)
        {
            return base.Channel.update_data_agentAsync(agent_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] get_data_agent_by_id(string id)
        {
            return base.Channel.get_data_agent_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> get_data_agent_by_idAsync(string id)
        {
            return base.Channel.get_data_agent_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] get_filter_agent(QOBDGateway.QOBDServiceReference.AgentFilterQOBD agent_array_list_filter)
        {
            return base.Channel.get_filter_agent(agent_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> get_filter_agentAsync(QOBDGateway.QOBDServiceReference.AgentFilterQOBD agent_array_list_filter)
        {
            return base.Channel.get_filter_agentAsync(agent_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.AgentQOBD[] get_data_agent_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_agent_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AgentQOBD[]> get_data_agent_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_agent_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.StatisticQOBD[] get_data_statistic(string nbLine)
        {
            return base.Channel.get_data_statistic(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.StatisticQOBD[]> get_data_statisticAsync(string nbLine)
        {
            return base.Channel.get_data_statisticAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.StatisticQOBD[] delete_data_statistic(QOBDGateway.QOBDServiceReference.StatisticQOBD[] statistic_array_list)
        {
            return base.Channel.delete_data_statistic(statistic_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.StatisticQOBD[]> delete_data_statisticAsync(QOBDGateway.QOBDServiceReference.StatisticQOBD[] statistic_array_list)
        {
            return base.Channel.delete_data_statisticAsync(statistic_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.StatisticQOBD[] insert_data_statistic(QOBDGateway.QOBDServiceReference.StatisticQOBD[] statistic_array_list)
        {
            return base.Channel.insert_data_statistic(statistic_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.StatisticQOBD[]> insert_data_statisticAsync(QOBDGateway.QOBDServiceReference.StatisticQOBD[] statistic_array_list)
        {
            return base.Channel.insert_data_statisticAsync(statistic_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.StatisticQOBD[] update_data_statistic(QOBDGateway.QOBDServiceReference.StatisticQOBD[] statistic_array_list)
        {
            return base.Channel.update_data_statistic(statistic_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.StatisticQOBD[]> update_data_statisticAsync(QOBDGateway.QOBDServiceReference.StatisticQOBD[] statistic_array_list)
        {
            return base.Channel.update_data_statisticAsync(statistic_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.StatisticQOBD[] get_data_statistic_by_id(string id)
        {
            return base.Channel.get_data_statistic_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.StatisticQOBD[]> get_data_statistic_by_idAsync(string id)
        {
            return base.Channel.get_data_statistic_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.StatisticQOBD[] get_filter_statistic(QOBDGateway.QOBDServiceReference.StatisticFilterQOBD statistic_array_list_filter)
        {
            return base.Channel.get_filter_statistic(statistic_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.StatisticQOBD[]> get_filter_statisticAsync(QOBDGateway.QOBDServiceReference.StatisticFilterQOBD statistic_array_list_filter)
        {
            return base.Channel.get_filter_statisticAsync(statistic_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] get_data_item(string nbLine)
        {
            return base.Channel.get_data_item(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> get_data_itemAsync(string nbLine)
        {
            return base.Channel.get_data_itemAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] delete_data_item(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.delete_data_item(item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> delete_data_itemAsync(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.delete_data_itemAsync(item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] insert_data_item(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.insert_data_item(item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> insert_data_itemAsync(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.insert_data_itemAsync(item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] update_data_item(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.update_data_item(item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> update_data_itemAsync(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.update_data_itemAsync(item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] get_data_item_by_id(string id)
        {
            return base.Channel.get_data_item_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> get_data_item_by_idAsync(string id)
        {
            return base.Channel.get_data_item_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] get_filter_item(QOBDGateway.QOBDServiceReference.ItemFilterQOBD item_array_list_filter)
        {
            return base.Channel.get_filter_item(item_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> get_filter_itemAsync(QOBDGateway.QOBDServiceReference.ItemFilterQOBD item_array_list_filter)
        {
            return base.Channel.get_filter_itemAsync(item_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.ItemQOBD[] get_data_item_by_order_item_list(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.get_data_item_by_order_item_list(order_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ItemQOBD[]> get_data_item_by_order_item_listAsync(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.get_data_item_by_order_item_listAsync(order_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] get_data_client(string nbLine)
        {
            return base.Channel.get_data_client(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> get_data_clientAsync(string nbLine)
        {
            return base.Channel.get_data_clientAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] delete_data_client(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.delete_data_client(client_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> delete_data_clientAsync(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.delete_data_clientAsync(client_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] insert_data_client(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.insert_data_client(client_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> insert_data_clientAsync(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.insert_data_clientAsync(client_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] update_data_client(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.update_data_client(client_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> update_data_clientAsync(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.update_data_clientAsync(client_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] get_data_client_by_id(string id)
        {
            return base.Channel.get_data_client_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> get_data_client_by_idAsync(string id)
        {
            return base.Channel.get_data_client_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] get_filter_Client(QOBDGateway.QOBDServiceReference.ClientFilterQOBD client_array_list_filter)
        {
            return base.Channel.get_filter_Client(client_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> get_filter_ClientAsync(QOBDGateway.QOBDServiceReference.ClientFilterQOBD client_array_list_filter)
        {
            return base.Channel.get_filter_ClientAsync(client_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] get_quotes_client(string id)
        {
            return base.Channel.get_quotes_client(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> get_quotes_clientAsync(string id)
        {
            return base.Channel.get_quotes_clientAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.OrdersQOBD[] get_orders_client(string id)
        {
            return base.Channel.get_orders_client(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.OrdersQOBD[]> get_orders_clientAsync(string id)
        {
            return base.Channel.get_orders_clientAsync(id);
        }

        public virtual string send_email_to_client(QOBDGateway.QOBDServiceReference.EmailQOBD client_email)
        {
            return base.Channel.send_email_to_client(client_email);
        }

        public virtual System.Threading.Tasks.Task<string> send_email_to_clientAsync(QOBDGateway.QOBDServiceReference.EmailQOBD client_email)
        {
            return base.Channel.send_email_to_clientAsync(client_email);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] get_data_client_by_bill_list(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.get_data_client_by_bill_list(bill_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> get_data_client_by_bill_listAsync(QOBDGateway.QOBDServiceReference.BillQOBD[] bill_array_list)
        {
            return base.Channel.get_data_client_by_bill_listAsync(bill_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ClientQOBD[] get_data_client_by_max_credit_over(int agent_id)
        {
            return base.Channel.get_data_client_by_max_credit_over(agent_id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ClientQOBD[]> get_data_client_by_max_credit_overAsync(int agent_id)
        {
            return base.Channel.get_data_client_by_max_credit_overAsync(agent_id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] get_data_actionRecord(string nbLine)
        {
            return base.Channel.get_data_actionRecord(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionRecordQOBD[]> get_data_actionRecordAsync(string nbLine)
        {
            return base.Channel.get_data_actionRecordAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] delete_data_actionRecord(QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] actionRecord_array_list)
        {
            return base.Channel.delete_data_actionRecord(actionRecord_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionRecordQOBD[]> delete_data_actionRecordAsync(QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] actionRecord_array_list)
        {
            return base.Channel.delete_data_actionRecordAsync(actionRecord_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] insert_data_actionRecord(QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] actionRecord_array_list)
        {
            return base.Channel.insert_data_actionRecord(actionRecord_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionRecordQOBD[]> insert_data_actionRecordAsync(QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] actionRecord_array_list)
        {
            return base.Channel.insert_data_actionRecordAsync(actionRecord_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] update_data_actionRecord(QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] actionRecord_array_list)
        {
            return base.Channel.update_data_actionRecord(actionRecord_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionRecordQOBD[]> update_data_actionRecordAsync(QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] actionRecord_array_list)
        {
            return base.Channel.update_data_actionRecordAsync(actionRecord_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] get_data_actionRecord_by_id(string id)
        {
            return base.Channel.get_data_actionRecord_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionRecordQOBD[]> get_data_actionRecord_by_idAsync(string id)
        {
            return base.Channel.get_data_actionRecord_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ActionRecordQOBD[] get_filter_actionRecord(QOBDGateway.QOBDServiceReference.ActionRecordFilterQOBD actionRecord_array_list)
        {
            return base.Channel.get_filter_actionRecord(actionRecord_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ActionRecordQOBD[]> get_filter_actionRecordAsync(QOBDGateway.QOBDServiceReference.ActionRecordFilterQOBD actionRecord_array_list)
        {
            return base.Channel.get_filter_actionRecordAsync(actionRecord_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] get_data_address(string nbLine)
        {
            return base.Channel.get_data_address(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> get_data_addressAsync(string nbLine)
        {
            return base.Channel.get_data_addressAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] delete_data_address(QOBDGateway.QOBDServiceReference.AddressQOBD[] address_array_list)
        {
            return base.Channel.delete_data_address(address_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> delete_data_addressAsync(QOBDGateway.QOBDServiceReference.AddressQOBD[] address_array_list)
        {
            return base.Channel.delete_data_addressAsync(address_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] insert_data_address(QOBDGateway.QOBDServiceReference.AddressQOBD[] address_array_list)
        {
            return base.Channel.insert_data_address(address_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> insert_data_addressAsync(QOBDGateway.QOBDServiceReference.AddressQOBD[] address_array_list)
        {
            return base.Channel.insert_data_addressAsync(address_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] update_data_address(QOBDGateway.QOBDServiceReference.AddressQOBD[] address_array_list)
        {
            return base.Channel.update_data_address(address_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> update_data_addressAsync(QOBDGateway.QOBDServiceReference.AddressQOBD[] address_array_list)
        {
            return base.Channel.update_data_addressAsync(address_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] get_data_address_by_id(string id)
        {
            return base.Channel.get_data_address_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> get_data_address_by_idAsync(string id)
        {
            return base.Channel.get_data_address_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] get_filter_address(QOBDGateway.QOBDServiceReference.AddressFilterQOBD address_array_list)
        {
            return base.Channel.get_filter_address(address_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> get_filter_addressAsync(QOBDGateway.QOBDServiceReference.AddressFilterQOBD address_array_list)
        {
            return base.Channel.get_filter_addressAsync(address_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] get_data_address_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_address_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> get_data_address_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_address_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.AddressQOBD[] get_data_address_by_client_list(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.get_data_address_by_client_list(client_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.AddressQOBD[]> get_data_address_by_client_listAsync(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.get_data_address_by_client_listAsync(client_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] get_data_order_item(string nbLine)
        {
            return base.Channel.get_data_order_item(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> get_data_order_itemAsync(string nbLine)
        {
            return base.Channel.get_data_order_itemAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] delete_data_order_item(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.delete_data_order_item(order_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> delete_data_order_itemAsync(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.delete_data_order_itemAsync(order_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] insert_data_order_item(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.insert_data_order_item(order_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> insert_data_order_itemAsync(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.insert_data_order_itemAsync(order_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] update_data_order_item(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.update_data_order_item(order_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> update_data_order_itemAsync(QOBDGateway.QOBDServiceReference.Order_itemQOBD[] order_item_array_list)
        {
            return base.Channel.update_data_order_itemAsync(order_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] get_data_order_item_by_id(string id)
        {
            return base.Channel.get_data_order_item_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> get_data_order_item_by_idAsync(string id)
        {
            return base.Channel.get_data_order_item_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] get_filter_order_item(QOBDGateway.QOBDServiceReference.Order_itemFilterQOBD order_item_array_list_filter)
        {
            return base.Channel.get_filter_order_item(order_item_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> get_filter_order_itemAsync(QOBDGateway.QOBDServiceReference.Order_itemFilterQOBD order_item_array_list_filter)
        {
            return base.Channel.get_filter_order_itemAsync(order_item_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.Order_itemQOBD[] get_data_order_item_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_order_item_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Order_itemQOBD[]> get_data_order_item_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_order_item_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] get_data_contact(string nbLine)
        {
            return base.Channel.get_data_contact(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> get_data_contactAsync(string nbLine)
        {
            return base.Channel.get_data_contactAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] delete_data_contact(QOBDGateway.QOBDServiceReference.ContactQOBD[] contact_array_list)
        {
            return base.Channel.delete_data_contact(contact_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> delete_data_contactAsync(QOBDGateway.QOBDServiceReference.ContactQOBD[] contact_array_list)
        {
            return base.Channel.delete_data_contactAsync(contact_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] insert_data_contact(QOBDGateway.QOBDServiceReference.ContactQOBD[] contact_array_list)
        {
            return base.Channel.insert_data_contact(contact_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> insert_data_contactAsync(QOBDGateway.QOBDServiceReference.ContactQOBD[] contact_array_list)
        {
            return base.Channel.insert_data_contactAsync(contact_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] update_data_contact(QOBDGateway.QOBDServiceReference.ContactQOBD[] contact_array_list)
        {
            return base.Channel.update_data_contact(contact_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> update_data_contactAsync(QOBDGateway.QOBDServiceReference.ContactQOBD[] contact_array_list)
        {
            return base.Channel.update_data_contactAsync(contact_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] get_data_contact_by_id(string id)
        {
            return base.Channel.get_data_contact_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> get_data_contact_by_idAsync(string id)
        {
            return base.Channel.get_data_contact_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] get_filter_contact(QOBDGateway.QOBDServiceReference.ContactFilterQOBD contact_array_list_filter)
        {
            return base.Channel.get_filter_contact(contact_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> get_filter_contactAsync(QOBDGateway.QOBDServiceReference.ContactFilterQOBD contact_array_list_filter)
        {
            return base.Channel.get_filter_contactAsync(contact_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.ContactQOBD[] get_data_contact_by_client_list(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.get_data_contact_by_client_list(client_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ContactQOBD[]> get_data_contact_by_client_listAsync(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.get_data_contact_by_client_listAsync(client_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] get_data_delivery(string nbLine)
        {
            return base.Channel.get_data_delivery(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> get_data_deliveryAsync(string nbLine)
        {
            return base.Channel.get_data_deliveryAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delete_data_delivery(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.delete_data_delivery(delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> delete_data_deliveryAsync(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.delete_data_deliveryAsync(delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] insert_data_delivery(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.insert_data_delivery(delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> insert_data_deliveryAsync(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.insert_data_deliveryAsync(delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] update_data_delivery(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.update_data_delivery(delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> update_data_deliveryAsync(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.update_data_deliveryAsync(delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] get_data_delivery_by_id(string id)
        {
            return base.Channel.get_data_delivery_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> get_data_delivery_by_idAsync(string id)
        {
            return base.Channel.get_data_delivery_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] get_filter_delivery(QOBDGateway.QOBDServiceReference.DeliveryFilterQOBD delivery_array_list_filter)
        {
            return base.Channel.get_filter_delivery(delivery_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> get_filter_deliveryAsync(QOBDGateway.QOBDServiceReference.DeliveryFilterQOBD delivery_array_list_filter)
        {
            return base.Channel.get_filter_deliveryAsync(delivery_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.DeliveryQOBD[] get_data_delivery_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_delivery_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DeliveryQOBD[]> get_data_delivery_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_delivery_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.InfosQOBD[] get_data_infos(string nbLine)
        {
            return base.Channel.get_data_infos(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.InfosQOBD[]> get_data_infosAsync(string nbLine)
        {
            return base.Channel.get_data_infosAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.InfosQOBD[] delete_data_infos(QOBDGateway.QOBDServiceReference.InfosQOBD[] infos_array_list)
        {
            return base.Channel.delete_data_infos(infos_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.InfosQOBD[]> delete_data_infosAsync(QOBDGateway.QOBDServiceReference.InfosQOBD[] infos_array_list)
        {
            return base.Channel.delete_data_infosAsync(infos_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.InfosQOBD[] insert_data_infos(QOBDGateway.QOBDServiceReference.InfosQOBD[] infos_array_list)
        {
            return base.Channel.insert_data_infos(infos_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.InfosQOBD[]> insert_data_infosAsync(QOBDGateway.QOBDServiceReference.InfosQOBD[] infos_array_list)
        {
            return base.Channel.insert_data_infosAsync(infos_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.InfosQOBD[] update_data_infos(QOBDGateway.QOBDServiceReference.InfosQOBD[] infos_array_list)
        {
            return base.Channel.update_data_infos(infos_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.InfosQOBD[]> update_data_infosAsync(QOBDGateway.QOBDServiceReference.InfosQOBD[] infos_array_list)
        {
            return base.Channel.update_data_infosAsync(infos_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.InfosQOBD[] get_data_infos_by_id(string id)
        {
            return base.Channel.get_data_infos_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.InfosQOBD[]> get_data_infos_by_idAsync(string id)
        {
            return base.Channel.get_data_infos_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.InfosQOBD[] get_filter_infos(QOBDGateway.QOBDServiceReference.InfosFilterQOBD infos_array_list_filter)
        {
            return base.Channel.get_filter_infos(infos_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.InfosQOBD[]> get_filter_infosAsync(QOBDGateway.QOBDServiceReference.InfosFilterQOBD infos_array_list_filter)
        {
            return base.Channel.get_filter_infosAsync(infos_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] get_data_auto_ref(string nbLine)
        {
            return base.Channel.get_data_auto_ref(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Auto_refsQOBD[]> get_data_auto_refAsync(string nbLine)
        {
            return base.Channel.get_data_auto_refAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] delete_data_auto_ref(QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] auto_ref_array_list)
        {
            return base.Channel.delete_data_auto_ref(auto_ref_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Auto_refsQOBD[]> delete_data_auto_refAsync(QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] auto_ref_array_list)
        {
            return base.Channel.delete_data_auto_refAsync(auto_ref_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] insert_data_auto_ref(QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] auto_ref_array_list)
        {
            return base.Channel.insert_data_auto_ref(auto_ref_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Auto_refsQOBD[]> insert_data_auto_refAsync(QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] auto_ref_array_list)
        {
            return base.Channel.insert_data_auto_refAsync(auto_ref_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] update_data_auto_ref(QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] auto_ref_array_list)
        {
            return base.Channel.update_data_auto_ref(auto_ref_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Auto_refsQOBD[]> update_data_auto_refAsync(QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] auto_ref_array_list)
        {
            return base.Channel.update_data_auto_refAsync(auto_ref_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] get_data_auto_ref_by_id(string id)
        {
            return base.Channel.get_data_auto_ref_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Auto_refsQOBD[]> get_data_auto_ref_by_idAsync(string id)
        {
            return base.Channel.get_data_auto_ref_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Auto_refsQOBD[] get_filter_auto_ref(QOBDGateway.QOBDServiceReference.Auto_refsFilterQOBD auto_ref_array_list)
        {
            return base.Channel.get_filter_auto_ref(auto_ref_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Auto_refsQOBD[]> get_filter_auto_refAsync(QOBDGateway.QOBDServiceReference.Auto_refsFilterQOBD auto_ref_array_list)
        {
            return base.Channel.get_filter_auto_refAsync(auto_ref_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] get_data_item_delivery(string nbLine)
        {
            return base.Channel.get_data_item_delivery(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> get_data_item_deliveryAsync(string nbLine)
        {
            return base.Channel.get_data_item_deliveryAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] delete_data_item_delivery(QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] item_delivery_array_list)
        {
            return base.Channel.delete_data_item_delivery(item_delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> delete_data_item_deliveryAsync(QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] item_delivery_array_list)
        {
            return base.Channel.delete_data_item_deliveryAsync(item_delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] insert_data_item_delivery(QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] item_delivery_array_list)
        {
            return base.Channel.insert_data_item_delivery(item_delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> insert_data_item_deliveryAsync(QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] item_delivery_array_list)
        {
            return base.Channel.insert_data_item_deliveryAsync(item_delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] update_data_item_delivery(QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] item_delivery_array_list)
        {
            return base.Channel.update_data_item_delivery(item_delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> update_data_item_deliveryAsync(QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] item_delivery_array_list)
        {
            return base.Channel.update_data_item_deliveryAsync(item_delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] get_data_item_delivery_by_id(string id)
        {
            return base.Channel.get_data_item_delivery_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> get_data_item_delivery_by_idAsync(string id)
        {
            return base.Channel.get_data_item_delivery_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] get_filter_item_delivery(QOBDGateway.QOBDServiceReference.Item_deliveryFilterQOBD item_delivery_array_list_filter)
        {
            return base.Channel.get_filter_item_delivery(item_delivery_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> get_filter_item_deliveryAsync(QOBDGateway.QOBDServiceReference.Item_deliveryFilterQOBD item_delivery_array_list_filter)
        {
            return base.Channel.get_filter_item_deliveryAsync(item_delivery_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[] get_data_item_delivery_by_delivery_list(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.get_data_item_delivery_by_delivery_list(delivery_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Item_deliveryQOBD[]> get_data_item_delivery_by_delivery_listAsync(QOBDGateway.QOBDServiceReference.DeliveryQOBD[] delivery_array_list)
        {
            return base.Channel.get_data_item_delivery_by_delivery_listAsync(delivery_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] get_data_provider_item(string nbLine)
        {
            return base.Channel.get_data_provider_item(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> get_data_provider_itemAsync(string nbLine)
        {
            return base.Channel.get_data_provider_itemAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] delete_data_provider_item(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.delete_data_provider_item(provider_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> delete_data_provider_itemAsync(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.delete_data_provider_itemAsync(provider_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] insert_data_provider_item(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.insert_data_provider_item(provider_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> insert_data_provider_itemAsync(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.insert_data_provider_itemAsync(provider_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] update_data_provider_item(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.update_data_provider_item(provider_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> update_data_provider_itemAsync(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.update_data_provider_itemAsync(provider_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] get_data_provider_item_by_id(string id)
        {
            return base.Channel.get_data_provider_item_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> get_data_provider_item_by_idAsync(string id)
        {
            return base.Channel.get_data_provider_item_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] get_filter_provider_item(QOBDGateway.QOBDServiceReference.Provider_itemFilterQOBD provider_item_array_list_filter)
        {
            return base.Channel.get_filter_provider_item(provider_item_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> get_filter_provider_itemAsync(QOBDGateway.QOBDServiceReference.Provider_itemFilterQOBD provider_item_array_list_filter)
        {
            return base.Channel.get_filter_provider_itemAsync(provider_item_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] get_data_provider_item_by_item_list(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.get_data_provider_item_by_item_list(item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Provider_itemQOBD[]> get_data_provider_item_by_item_listAsync(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.get_data_provider_item_by_item_listAsync(item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] get_data_provider(string nbLine)
        {
            return base.Channel.get_data_provider(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> get_data_providerAsync(string nbLine)
        {
            return base.Channel.get_data_providerAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] delete_data_provider(QOBDGateway.QOBDServiceReference.ProviderQOBD[] provider_array_list)
        {
            return base.Channel.delete_data_provider(provider_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> delete_data_providerAsync(QOBDGateway.QOBDServiceReference.ProviderQOBD[] provider_array_list)
        {
            return base.Channel.delete_data_providerAsync(provider_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] insert_data_provider(QOBDGateway.QOBDServiceReference.ProviderQOBD[] provider_array_list)
        {
            return base.Channel.insert_data_provider(provider_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> insert_data_providerAsync(QOBDGateway.QOBDServiceReference.ProviderQOBD[] provider_array_list)
        {
            return base.Channel.insert_data_providerAsync(provider_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] update_data_provider(QOBDGateway.QOBDServiceReference.ProviderQOBD[] provider_array_list)
        {
            return base.Channel.update_data_provider(provider_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> update_data_providerAsync(QOBDGateway.QOBDServiceReference.ProviderQOBD[] provider_array_list)
        {
            return base.Channel.update_data_providerAsync(provider_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] get_data_provider_by_id(string id)
        {
            return base.Channel.get_data_provider_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> get_data_provider_by_idAsync(string id)
        {
            return base.Channel.get_data_provider_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] get_filter_provider(QOBDGateway.QOBDServiceReference.ProviderFilterQOBD provider_array_list_filter)
        {
            return base.Channel.get_filter_provider(provider_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> get_filter_providerAsync(QOBDGateway.QOBDServiceReference.ProviderFilterQOBD provider_array_list_filter)
        {
            return base.Channel.get_filter_providerAsync(provider_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.ProviderQOBD[] get_data_provider_by_provider_item_list(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.get_data_provider_by_provider_item_list(provider_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.ProviderQOBD[]> get_data_provider_by_provider_item_listAsync(QOBDGateway.QOBDServiceReference.Provider_itemQOBD[] provider_item_array_list)
        {
            return base.Channel.get_data_provider_by_provider_item_listAsync(provider_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] get_data_tax_order(string nbLine)
        {
            return base.Channel.get_data_tax_order(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> get_data_tax_orderAsync(string nbLine)
        {
            return base.Channel.get_data_tax_orderAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] delete_data_tax_order(QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] tax_order_array_list)
        {
            return base.Channel.delete_data_tax_order(tax_order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> delete_data_tax_orderAsync(QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] tax_order_array_list)
        {
            return base.Channel.delete_data_tax_orderAsync(tax_order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] insert_data_tax_order(QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] tax_order_array_list)
        {
            return base.Channel.insert_data_tax_order(tax_order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> insert_data_tax_orderAsync(QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] tax_order_array_list)
        {
            return base.Channel.insert_data_tax_orderAsync(tax_order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] update_data_tax_order(QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] tax_order_array_list)
        {
            return base.Channel.update_data_tax_order(tax_order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> update_data_tax_orderAsync(QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] tax_order_array_list)
        {
            return base.Channel.update_data_tax_orderAsync(tax_order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] get_data_tax_order_by_id(string id)
        {
            return base.Channel.get_data_tax_order_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> get_data_tax_order_by_idAsync(string id)
        {
            return base.Channel.get_data_tax_order_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] get_filter_tax_order(QOBDGateway.QOBDServiceReference.Tax_orderFilterQOBD tax_order_array_list_filter)
        {
            return base.Channel.get_filter_tax_order(tax_order_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> get_filter_tax_orderAsync(QOBDGateway.QOBDServiceReference.Tax_orderFilterQOBD tax_order_array_list_filter)
        {
            return base.Channel.get_filter_tax_orderAsync(tax_order_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_orderQOBD[] get_data_tax_order_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_tax_order_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_orderQOBD[]> get_data_tax_order_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_tax_order_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] get_data_tax_item(string nbLine)
        {
            return base.Channel.get_data_tax_item(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> get_data_tax_itemAsync(string nbLine)
        {
            return base.Channel.get_data_tax_itemAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] delete_data_tax_item(QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] tax_item_array_list)
        {
            return base.Channel.delete_data_tax_item(tax_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> delete_data_tax_itemAsync(QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] tax_item_array_list)
        {
            return base.Channel.delete_data_tax_itemAsync(tax_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] insert_data_tax_item(QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] tax_item_array_list)
        {
            return base.Channel.insert_data_tax_item(tax_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> insert_data_tax_itemAsync(QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] tax_item_array_list)
        {
            return base.Channel.insert_data_tax_itemAsync(tax_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] update_data_tax_item(QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] tax_item_array_list)
        {
            return base.Channel.update_data_tax_item(tax_item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> update_data_tax_itemAsync(QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] tax_item_array_list)
        {
            return base.Channel.update_data_tax_itemAsync(tax_item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] get_data_tax_item_by_id(string id)
        {
            return base.Channel.get_data_tax_item_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> get_data_tax_item_by_idAsync(string id)
        {
            return base.Channel.get_data_tax_item_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] get_filter_tax_item(QOBDGateway.QOBDServiceReference.Tax_itemFilterQOBD tax_item_array_list_filter)
        {
            return base.Channel.get_filter_tax_item(tax_item_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> get_filter_tax_itemAsync(QOBDGateway.QOBDServiceReference.Tax_itemFilterQOBD tax_item_array_list_filter)
        {
            return base.Channel.get_filter_tax_itemAsync(tax_item_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.Tax_itemQOBD[] get_data_tax_item_by_item_list(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.get_data_tax_item_by_item_list(item_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.Tax_itemQOBD[]> get_data_tax_item_by_item_listAsync(QOBDGateway.QOBDServiceReference.ItemQOBD[] item_array_list)
        {
            return base.Channel.get_data_tax_item_by_item_listAsync(item_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.TaxQOBD[] get_data_tax(string nbLine)
        {
            return base.Channel.get_data_tax(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.TaxQOBD[]> get_data_taxAsync(string nbLine)
        {
            return base.Channel.get_data_taxAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.TaxQOBD[] delete_data_tax(QOBDGateway.QOBDServiceReference.TaxQOBD[] tax_array_list)
        {
            return base.Channel.delete_data_tax(tax_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.TaxQOBD[]> delete_data_taxAsync(QOBDGateway.QOBDServiceReference.TaxQOBD[] tax_array_list)
        {
            return base.Channel.delete_data_taxAsync(tax_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.TaxQOBD[] insert_data_tax(QOBDGateway.QOBDServiceReference.TaxQOBD[] tax_array_list)
        {
            return base.Channel.insert_data_tax(tax_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.TaxQOBD[]> insert_data_taxAsync(QOBDGateway.QOBDServiceReference.TaxQOBD[] tax_array_list)
        {
            return base.Channel.insert_data_taxAsync(tax_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.TaxQOBD[] update_data_tax(QOBDGateway.QOBDServiceReference.TaxQOBD[] tax_array_list)
        {
            return base.Channel.update_data_tax(tax_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.TaxQOBD[]> update_data_taxAsync(QOBDGateway.QOBDServiceReference.TaxQOBD[] tax_array_list)
        {
            return base.Channel.update_data_taxAsync(tax_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.TaxQOBD[] get_data_tax_by_id(string id)
        {
            return base.Channel.get_data_tax_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.TaxQOBD[]> get_data_tax_by_idAsync(string id)
        {
            return base.Channel.get_data_tax_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.TaxQOBD[] get_filter_tax(QOBDGateway.QOBDServiceReference.TaxFilterQOBD tax_array_list_filter)
        {
            return base.Channel.get_filter_tax(tax_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.TaxQOBD[]> get_filter_taxAsync(QOBDGateway.QOBDServiceReference.TaxFilterQOBD tax_array_list_filter)
        {
            return base.Channel.get_filter_taxAsync(tax_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] get_data_privilege(string nbLine)
        {
            return base.Channel.get_data_privilege(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.PrivilegeQOBD[]> get_data_privilegeAsync(string nbLine)
        {
            return base.Channel.get_data_privilegeAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] delete_data_privilege(QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] privilege_array_list)
        {
            return base.Channel.delete_data_privilege(privilege_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.PrivilegeQOBD[]> delete_data_privilegeAsync(QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] privilege_array_list)
        {
            return base.Channel.delete_data_privilegeAsync(privilege_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] insert_data_privilege(QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] privilege_array_list)
        {
            return base.Channel.insert_data_privilege(privilege_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.PrivilegeQOBD[]> insert_data_privilegeAsync(QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] privilege_array_list)
        {
            return base.Channel.insert_data_privilegeAsync(privilege_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] update_data_privilege(QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] privilege_array_list)
        {
            return base.Channel.update_data_privilege(privilege_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.PrivilegeQOBD[]> update_data_privilegeAsync(QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] privilege_array_list)
        {
            return base.Channel.update_data_privilegeAsync(privilege_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] get_data_privilege_by_id(string id)
        {
            return base.Channel.get_data_privilege_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.PrivilegeQOBD[]> get_data_privilege_by_idAsync(string id)
        {
            return base.Channel.get_data_privilege_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.PrivilegeQOBD[] get_filter_privilege(QOBDGateway.QOBDServiceReference.PrivilegeFilterQOBD privilege_array_list_filter)
        {
            return base.Channel.get_filter_privilege(privilege_array_list_filter);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.PrivilegeQOBD[]> get_filter_privilegeAsync(QOBDGateway.QOBDServiceReference.PrivilegeFilterQOBD privilege_array_list_filter)
        {
            return base.Channel.get_filter_privilegeAsync(privilege_array_list_filter);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] get_data_notification(string nbLine)
        {
            return base.Channel.get_data_notification(nbLine);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> get_data_notificationAsync(string nbLine)
        {
            return base.Channel.get_data_notificationAsync(nbLine);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] delete_data_notification(QOBDGateway.QOBDServiceReference.NotificationQOBD[] notification_array_list)
        {
            return base.Channel.delete_data_notification(notification_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> delete_data_notificationAsync(QOBDGateway.QOBDServiceReference.NotificationQOBD[] notification_array_list)
        {
            return base.Channel.delete_data_notificationAsync(notification_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] insert_data_notification(QOBDGateway.QOBDServiceReference.NotificationQOBD[] notification_array_list)
        {
            return base.Channel.insert_data_notification(notification_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> insert_data_notificationAsync(QOBDGateway.QOBDServiceReference.NotificationQOBD[] notification_array_list)
        {
            return base.Channel.insert_data_notificationAsync(notification_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] update_data_notification(QOBDGateway.QOBDServiceReference.NotificationQOBD[] notification_array_list)
        {
            return base.Channel.update_data_notification(notification_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> update_data_notificationAsync(QOBDGateway.QOBDServiceReference.NotificationQOBD[] notification_array_list)
        {
            return base.Channel.update_data_notificationAsync(notification_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] get_data_notification_by_id(string id)
        {
            return base.Channel.get_data_notification_by_id(id);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> get_data_notification_by_idAsync(string id)
        {
            return base.Channel.get_data_notification_by_idAsync(id);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] get_filter_notification(QOBDGateway.QOBDServiceReference.NotificationFilterQOBD notification_array_list)
        {
            return base.Channel.get_filter_notification(notification_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> get_filter_notificationAsync(QOBDGateway.QOBDServiceReference.NotificationFilterQOBD notification_array_list)
        {
            return base.Channel.get_filter_notificationAsync(notification_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] get_data_notification_by_order_list(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_notification_by_order_list(order_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> get_data_notification_by_order_listAsync(QOBDGateway.QOBDServiceReference.OrdersQOBD[] order_array_list)
        {
            return base.Channel.get_data_notification_by_order_listAsync(order_array_list);
        }

        public virtual QOBDGateway.QOBDServiceReference.NotificationQOBD[] get_data_notification_by_client_list(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.get_data_notification_by_client_list(client_array_list);
        }

        public virtual System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.NotificationQOBD[]> get_data_notification_by_client_listAsync(QOBDGateway.QOBDServiceReference.ClientQOBD[] client_array_list)
        {
            return base.Channel.get_data_notification_by_client_listAsync(client_array_list);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] get_data_user(string nbLine)
        {
            return base.Channel.get_data_user(nbLine);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> get_data_userAsync(string nbLine)
        {
            return base.Channel.get_data_userAsync(nbLine);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] delete_data_user(QOBDGateway.QOBDServiceReference.UserChatRoom[] user_array_list)
        {
            return base.Channel.delete_data_user(user_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> delete_data_userAsync(QOBDGateway.QOBDServiceReference.UserChatRoom[] user_array_list)
        {
            return base.Channel.delete_data_userAsync(user_array_list);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] insert_data_user(QOBDGateway.QOBDServiceReference.UserChatRoom[] user_array_list)
        {
            return base.Channel.insert_data_user(user_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> insert_data_userAsync(QOBDGateway.QOBDServiceReference.UserChatRoom[] user_array_list)
        {
            return base.Channel.insert_data_userAsync(user_array_list);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] update_data_user(QOBDGateway.QOBDServiceReference.UserChatRoom[] user_array_list)
        {
            return base.Channel.update_data_user(user_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> update_data_userAsync(QOBDGateway.QOBDServiceReference.UserChatRoom[] user_array_list)
        {
            return base.Channel.update_data_userAsync(user_array_list);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] get_data_user_by_id(string id)
        {
            return base.Channel.get_data_user_by_id(id);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> get_data_user_by_idAsync(string id)
        {
            return base.Channel.get_data_user_by_idAsync(id);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] get_filter_user(QOBDGateway.QOBDServiceReference.UserFilterChatRoom user_array_list_filter)
        {
            return base.Channel.get_filter_user(user_array_list_filter);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> get_filter_userAsync(QOBDGateway.QOBDServiceReference.UserFilterChatRoom user_array_list_filter)
        {
            return base.Channel.get_filter_userAsync(user_array_list_filter);
        }

        public QOBDGateway.QOBDServiceReference.UserChatRoom[] get_data_user_by_user_discussion_list(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.get_data_user_by_user_discussion_list(user_discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.UserChatRoom[]> get_data_user_by_user_discussion_listAsync(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.get_data_user_by_user_discussion_listAsync(user_discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.MessageChatRoom[] get_data_message(string nbLine)
        {
            return base.Channel.get_data_message(nbLine);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.MessageChatRoom[]> get_data_messageAsync(string nbLine)
        {
            return base.Channel.get_data_messageAsync(nbLine);
        }

        public QOBDGateway.QOBDServiceReference.MessageChatRoom[] delete_data_message(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.delete_data_message(message_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.MessageChatRoom[]> delete_data_messageAsync(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.delete_data_messageAsync(message_array_list);
        }

        public QOBDGateway.QOBDServiceReference.MessageChatRoom[] insert_data_message(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.insert_data_message(message_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.MessageChatRoom[]> insert_data_messageAsync(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.insert_data_messageAsync(message_array_list);
        }

        public QOBDGateway.QOBDServiceReference.MessageChatRoom[] update_data_message(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.update_data_message(message_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.MessageChatRoom[]> update_data_messageAsync(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.update_data_messageAsync(message_array_list);
        }

        public QOBDGateway.QOBDServiceReference.MessageChatRoom[] get_data_message_by_id(string id)
        {
            return base.Channel.get_data_message_by_id(id);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.MessageChatRoom[]> get_data_message_by_idAsync(string id)
        {
            return base.Channel.get_data_message_by_idAsync(id);
        }

        public QOBDGateway.QOBDServiceReference.MessageChatRoom[] get_filter_message(QOBDGateway.QOBDServiceReference.MessageFilterChatRoom message_array_list_filter)
        {
            return base.Channel.get_filter_message(message_array_list_filter);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.MessageChatRoom[]> get_filter_messageAsync(QOBDGateway.QOBDServiceReference.MessageFilterChatRoom message_array_list_filter)
        {
            return base.Channel.get_filter_messageAsync(message_array_list_filter);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] get_data_discussion(string nbLine)
        {
            return base.Channel.get_data_discussion(nbLine);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> get_data_discussionAsync(string nbLine)
        {
            return base.Channel.get_data_discussionAsync(nbLine);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] delete_data_discussion(QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] discussion_array_list)
        {
            return base.Channel.delete_data_discussion(discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> delete_data_discussionAsync(QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] discussion_array_list)
        {
            return base.Channel.delete_data_discussionAsync(discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] insert_data_discussion(QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] discussion_array_list)
        {
            return base.Channel.insert_data_discussion(discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> insert_data_discussionAsync(QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] discussion_array_list)
        {
            return base.Channel.insert_data_discussionAsync(discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] update_data_discussion(QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] discussion_array_list)
        {
            return base.Channel.update_data_discussion(discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> update_data_discussionAsync(QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] discussion_array_list)
        {
            return base.Channel.update_data_discussionAsync(discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] get_data_discussion_by_id(string id)
        {
            return base.Channel.get_data_discussion_by_id(id);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> get_data_discussion_by_idAsync(string id)
        {
            return base.Channel.get_data_discussion_by_idAsync(id);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] get_filter_discussion(QOBDGateway.QOBDServiceReference.DiscussionFilterChatRoom discussion_array_list_filter)
        {
            return base.Channel.get_filter_discussion(discussion_array_list_filter);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> get_filter_discussionAsync(QOBDGateway.QOBDServiceReference.DiscussionFilterChatRoom discussion_array_list_filter)
        {
            return base.Channel.get_filter_discussionAsync(discussion_array_list_filter);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] get_data_discussion_by_user_discussion_list(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.get_data_discussion_by_user_discussion_list(user_discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> get_data_discussion_by_user_discussion_listAsync(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.get_data_discussion_by_user_discussion_listAsync(user_discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.DiscussionChatRoom[] get_data_discussion_by_message_list(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.get_data_discussion_by_message_list(message_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.DiscussionChatRoom[]> get_data_discussion_by_message_listAsync(QOBDGateway.QOBDServiceReference.MessageChatRoom[] message_array_list)
        {
            return base.Channel.get_data_discussion_by_message_listAsync(message_array_list);
        }

        public QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] get_data_user_discussion(string nbLine)
        {
            return base.Channel.get_data_user_discussion(nbLine);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.User_discussionChatRoom[]> get_data_user_discussionAsync(string nbLine)
        {
            return base.Channel.get_data_user_discussionAsync(nbLine);
        }

        public QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] delete_data_user_discussion(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.delete_data_user_discussion(user_discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.User_discussionChatRoom[]> delete_data_user_discussionAsync(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.delete_data_user_discussionAsync(user_discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] insert_data_user_discussion(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.insert_data_user_discussion(user_discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.User_discussionChatRoom[]> insert_data_user_discussionAsync(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.insert_data_user_discussionAsync(user_discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] update_data_user_discussion(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.update_data_user_discussion(user_discussion_array_list);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.User_discussionChatRoom[]> update_data_user_discussionAsync(QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] user_discussion_array_list)
        {
            return base.Channel.update_data_user_discussionAsync(user_discussion_array_list);
        }

        public QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] get_data_user_discussion_by_id(string id)
        {
            return base.Channel.get_data_user_discussion_by_id(id);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.User_discussionChatRoom[]> get_data_user_discussion_by_idAsync(string id)
        {
            return base.Channel.get_data_user_discussion_by_idAsync(id);
        }

        public QOBDGateway.QOBDServiceReference.User_discussionChatRoom[] get_filter_user_discussion(QOBDGateway.QOBDServiceReference.User_discussionFilterChatRoom user_discussion_array_list_filter)
        {
            return base.Channel.get_filter_user_discussion(user_discussion_array_list_filter);
        }

        public System.Threading.Tasks.Task<QOBDGateway.QOBDServiceReference.User_discussionChatRoom[]> get_filter_user_discussionAsync(QOBDGateway.QOBDServiceReference.User_discussionFilterChatRoom user_discussion_array_list_filter)
        {
            return base.Channel.get_filter_user_discussionAsync(user_discussion_array_list_filter);
        }
    }
}
