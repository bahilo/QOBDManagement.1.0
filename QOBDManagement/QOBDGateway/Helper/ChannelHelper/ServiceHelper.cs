using QOBDCommon.Entities;
using QOBDCommon.Structures;
using QOBDGateway.QOBDServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using QOBDCommon.Classes;
using System.Threading.Tasks;
using QOBDCommon.Enum;
using System.Diagnostics;

namespace QOBDGateway.Helper.ChannelHelper
{
    public static class ServiceHelper
    {
        
        //====================================================================================
        //===============================[ Agent ]===========================================
        //====================================================================================

        public static List<Agent> ArrayTypeToAgent(this AgentQOBD[] agentQOBDList)
        {
            List<Agent> outputList = agentQOBDList.AsParallel().Select(x => new Agent
            {
                ID = x.ID,
                FirstName = Utility.decodeBase64ToString(x.FirstName),
                LastName = Utility.decodeBase64ToString(x.LastName),
                Login = Utility.decodeBase64ToString(x.Login),
                HashedPassword = Utility.decodeBase64ToString(x.Password),
                Phone = Utility.decodeBase64ToString(x.Phone),
                Status = Utility.decodeBase64ToString(x.Status),
                ListSize = x.ListSize,
                Email = Utility.decodeBase64ToString(x.Email),
                Fax = Utility.decodeBase64ToString(x.Fax),
                RoleList = x.Roles.ArrayTypeToRole(),
            }).ToList();
            
            return outputList;
        }

        public static AgentQOBD[] AgentTypeToArray(this List<Agent> agentList)
        {
            AgentQOBD[] outputArray = agentList.AsParallel().Select(x => new AgentQOBD
            {
                ID = x.ID,
                FirstName = Utility.encodeStringToBase64(x.FirstName),
                LastName = Utility.encodeStringToBase64(x.LastName),
                Login = Utility.encodeStringToBase64(x.Login),
                Password = Utility.encodeStringToBase64(x.HashedPassword),
                Phone = Utility.encodeStringToBase64(x.Phone),
                Status = Utility.encodeStringToBase64(x.Status),
                ListSize = x.ListSize,
                Email = Utility.encodeStringToBase64(x.Email),
                Fax = Utility.encodeStringToBase64(x.Fax),
            }).ToArray();
            
            return outputArray;
        }

        public static AgentFilterQOBD AgentTypeToFilterArray(this Agent agent, ESearchOption filterOperator)
        {
            AgentFilterQOBD agentQCBD = new AgentFilterQOBD();
            if (agent != null)
            {
                agentQCBD.ID = agent.ID;
                agentQCBD.FirstName = Utility.encodeStringToBase64(agent.FirstName);
                agentQCBD.LastName = Utility.encodeStringToBase64(agent.LastName);
                agentQCBD.Login = Utility.encodeStringToBase64(agent.Login);
                agentQCBD.Password = Utility.encodeStringToBase64(agent.HashedPassword);
                agentQCBD.Phone = Utility.encodeStringToBase64(agent.Phone);
                agentQCBD.Status = Utility.encodeStringToBase64(agent.Status);
                agentQCBD.ListSize = agent.ListSize;
                agentQCBD.Email = Utility.encodeStringToBase64(agent.Email);
                agentQCBD.Fax = Utility.encodeStringToBase64(agent.Fax);
                agentQCBD.Operator = filterOperator.ToString();
            }
            return agentQCBD;
        }

        //====================================================================================
        //===============================[ Statistic ]===========================================
        //====================================================================================

        public static List<Statistic> ArrayTypeToStatistic(this StatisticQOBD[] statisticQOBDList)
        {
            List<Statistic> outputList = statisticQOBDList.AsParallel().Select(x => new Statistic
            {
                ID = x.ID,
                BillId = Convert.ToInt32(x.BillId),
                Bill_date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Bill_date)),
                Company = Utility.decodeBase64ToString(x.Company),
                Date_limit = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date_limit)),
                Income = Convert.ToDecimal(Utility.decodeBase64ToString(x.Income)),
                Income_percent =  Convert.ToDouble(Utility.decodeBase64ToString(x.Income_percent).Replace("%", "")),
                Pay_date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Pay_date)),
                Pay_received = Convert.ToDecimal(Utility.decodeBase64ToString(x.Pay_received).Split(new char[] { ' ' }).FirstOrDefault()),
                Price_purchase_total = Convert.ToDecimal(Utility.decodeBase64ToString(x.Price_purchase_total)),
                Tax_value = x.Tax_value,
                Total = Convert.ToDecimal(Utility.decodeBase64ToString(x.Total)),
                Total_tax_included = Convert.ToDecimal(Utility.decodeBase64ToString(x.Total_tax_included)),
            }).ToList();

            return outputList;
        }

        public static StatisticQOBD[] StatisticTypeToArray(this List<Statistic> statisticList)
        {
            StatisticQOBD[] outputArray = statisticList.AsParallel().Select(x => new StatisticQOBD
            {
                ID = x.ID,
                BillId = Utility.encodeStringToBase64(x.BillId.ToString()),
                Bill_date = Utility.encodeStringToBase64(x.Bill_date.ToString()),
                Company = Utility.encodeStringToBase64(x.Company),
                Date_limit = Utility.encodeStringToBase64(x.Date_limit.ToString()),
                Income = Utility.encodeStringToBase64(x.Income.ToString()),
                Income_percent = Utility.encodeStringToBase64(x.Income_percent.ToString()),
                Pay_date = Utility.encodeStringToBase64(x.Pay_date.ToString()),
                Pay_received = Utility.encodeStringToBase64(x.Pay_received.ToString()),
                Price_purchase_total = Utility.encodeStringToBase64(x.Price_purchase_total.ToString()),
                Tax_value = x.Tax_value,
                Total = Utility.encodeStringToBase64(x.Total.ToString()),
                Total_tax_included = Utility.encodeStringToBase64(x.Total_tax_included.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static StatisticFilterQOBD StatisticTypeToFilterArray(this Statistic statistic, ESearchOption filterOperator)
        {
            StatisticFilterQOBD statisticQCBD = new StatisticFilterQOBD();
            if (statistic != null)
            {
                statisticQCBD.ID = statistic.ID;
                statisticQCBD.Option = statistic.Option;
                statisticQCBD.BillId = Utility.encodeStringToBase64(statistic.BillId.ToString());
                statisticQCBD.Bill_date = Utility.encodeStringToBase64(statistic.Bill_date.ToString());
                statisticQCBD.Company = Utility.encodeStringToBase64(statistic.Company);
                statisticQCBD.Date_limit = Utility.encodeStringToBase64(statistic.Date_limit.ToString());
                statisticQCBD.Income = Utility.encodeStringToBase64(statistic.Income.ToString());
                statisticQCBD.Income_percent = Utility.encodeStringToBase64(statistic.Income_percent.ToString());
                statisticQCBD.Pay_date = Utility.encodeStringToBase64(statistic.Pay_date.ToString());
                statisticQCBD.Pay_received = Utility.encodeStringToBase64(statistic.Pay_received.ToString());
                statisticQCBD.Price_purchase_total = Utility.encodeStringToBase64(statistic.Price_purchase_total.ToString());
                statisticQCBD.Tax_value = statistic.Tax_value;
                statisticQCBD.Total = Utility.encodeStringToBase64(statistic.Total.ToString());
                statisticQCBD.Total_tax_included = Utility.encodeStringToBase64(statistic.Total_tax_included.ToString());
                statisticQCBD.Operator = filterOperator.ToString();
            }
            return statisticQCBD;
        }


        //====================================================================================
        //===============================[ Infos ]===========================================
        //====================================================================================

        public static List<Info> ArrayTypeToInfos(this InfosQOBD[] infosQOBDList)
        {
            List<Info> outputList = infosQOBDList.AsParallel().Select(x => new Info
            {
                ID = x.ID,
                Name = Utility.decodeBase64ToString(x.Name),
                Value = Utility.decodeBase64ToString(x.Value),
            }).ToList();

            return outputList;
        }

        public static InfosQOBD[] InfosTypeToArray(this List<Info> infosList)
        {
            InfosQOBD[] outputArray = infosList.AsParallel().Select(x => new InfosQOBD
            {
                ID = x.ID,
                Name = Utility.encodeStringToBase64(x.Name),
                Value = Utility.encodeStringToBase64(x.Value),
            }).ToArray();
            
            return outputArray;
        }

        public static InfosFilterQOBD InfosTypeToFilterArray(this Info infos, ESearchOption filterOperator)
        {
            InfosFilterQOBD infosQCBD = new InfosFilterQOBD();
            if (infos != null)
            {
                infosQCBD.ID = infos.ID;
                infosQCBD.Option = infos.Option;
                infosQCBD.Name = Utility.encodeStringToBase64(infos.Name);
                infosQCBD.Value = Utility.encodeStringToBase64(infos.Value);
                infosQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return infosQCBD;
        }
        
        //====================================================================================
        //===============================[ ActionRecord ]===========================================
        //====================================================================================

        public static List<ActionRecord> ArrayTypeToActionRecord(this ActionRecordQOBD[] actionRecordQOBDList)
        {
            List<ActionRecord> outputList = actionRecordQOBDList.AsParallel().Select(x => new ActionRecord
            {
                ID = x.ID,
                AgentId = x.AgentId,
                Action = Utility.decodeBase64ToString(x.Action),
                TargetId = x.TargetId,
                TargetName = Utility.decodeBase64ToString(x.TargetName),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
            }).ToList();
          
            return outputList;
        }

        public static ActionRecordQOBD[] ActionRecordTypeToArray(this List<ActionRecord> actionRecordList)
        {
            ActionRecordQOBD[] outputArray = actionRecordList.AsParallel().Select(x => new ActionRecordQOBD
            {
                ID = x.ID,
                AgentId = x.AgentId,
                Action = Utility.encodeStringToBase64(x.Action),
                TargetId = x.TargetId,
                TargetName = Utility.encodeStringToBase64(x.TargetName),
                Date = Utility.encodeStringToBase64(x.Date.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static ActionRecordFilterQOBD ActionRecordTypeToFilterArray(this ActionRecord actionRecord, ESearchOption filterOperator)
        {
            ActionRecordFilterQOBD actionRecordQCBD = new ActionRecordFilterQOBD();
            if (actionRecord != null)
            {
                actionRecordQCBD.ID = actionRecord.ID;
                actionRecordQCBD.AgentId = actionRecord.AgentId;
                actionRecordQCBD.Action = Utility.encodeStringToBase64(actionRecord.Action);
                actionRecordQCBD.TargetId = actionRecord.TargetId;
                actionRecordQCBD.TargetName = Utility.encodeStringToBase64(actionRecord.TargetName);
                actionRecordQCBD.Date = Utility.encodeStringToBase64(actionRecord.Date.ToString());
                actionRecordQCBD.Operator = filterOperator.ToString();
            }
            return actionRecordQCBD;
        }

        //====================================================================================
        //===============================[ Role ]===========================================
        //====================================================================================
        
        public static List<Role> ArrayTypeToRole(this RoleQOBD[] roleQOBDList)
        {
            List<Role> outputList = new List<Role>();
            if (roleQOBDList != null)
            {
                outputList = roleQOBDList.AsParallel().Select(x => new Role
                {
                    ID = x.ID,
                    Name = Utility.decodeBase64ToString(x.Name),
                    ActionList = x.Actions.ArrayTypeToAction(),
                }).ToList();
            }            

            return outputList;
        }

        public static RoleQOBD[] RoleTypeToArray(this List<Role> roleList)
        {
            RoleQOBD[] outputArray = roleList.AsParallel().Select(x => new RoleQOBD
            {
                ID = x.ID,
                Name = Utility.encodeStringToBase64(x.Name),
                Actions = x.ActionList.ActionTypeToArray(),
            }).ToArray();
            
            return outputArray;
        }

        public static RoleFilterQOBD RoleTypeToFilterArray(this Role role, ESearchOption filterOperator)
        {
            RoleFilterQOBD roleQCBD = new RoleFilterQOBD();
            if (role != null)
            {
                roleQCBD.ID = role.ID;
                roleQCBD.Name = Utility.encodeStringToBase64(role.Name);
                roleQCBD.Operator = filterOperator.ToString();
            }
            return roleQCBD;
        }



        //====================================================================================
        //===============================[ Role_action ]===========================================
        //====================================================================================

        public static List<Role_action> ArrayTypeToRole_action(this Role_actionQOBD[] role_actionQOBDList)
        {
            List<Role_action> outputList = role_actionQOBDList.AsParallel().Select(x => new Role_action
            {
                ID = x.ID,
                ActionId = x.ActionId,
                RoleId = x.RoleId,
            }).ToList();
            
            return outputList;
        }

        public static Role_actionQOBD[] Role_actionTypeToArray(this List<Role_action> role_actionList)
        {
            Role_actionQOBD[] outputArray = role_actionList.AsParallel().Select(x => new Role_actionQOBD
            {
                ID = x.ID,
                ActionId = x.ActionId,
                RoleId = x.RoleId,
            }).ToArray();
            
            return outputArray;
        }

        public static Role_actionFilterQOBD Role_actionTypeToFilterArray(this Role_action role_action, ESearchOption filterOperator)
        {
            Role_actionFilterQOBD role_actionQCBD = new Role_actionFilterQOBD();
            if (role_action != null)
            {
                role_actionQCBD.ID = role_action.ID;
                role_actionQCBD.ActionId = role_action.ActionId;
                role_actionQCBD.RoleId = role_action.RoleId;
                role_actionQCBD.Operator = filterOperator.ToString();
            }
            return role_actionQCBD;
        }


        //====================================================================================
        //===============================[ SecurityAction ]===========================================
        //====================================================================================

        public static List<QOBDCommon.Entities.Action> ArrayTypeToAction(this ActionQOBD[] actionQOBDList)
        {
            List<QOBDCommon.Entities.Action> outputList = actionQOBDList.AsParallel().Select(x => new QOBDCommon.Entities.Action
            {
                ID = x.ID,
                Name = Utility.decodeBase64ToString(x.Name),
                Right = (new PrivilegeQOBD[] { x.Right }.ArrayTypeToPrivilege().Count > 0) ? new PrivilegeQOBD[] { x.Right }.ArrayTypeToPrivilege().First() : new Privilege(),
            }).ToList();
            
            return outputList;
        }

        public static ActionQOBD[] ActionTypeToArray(this List<QOBDCommon.Entities.Action> actionList)
        {
            ActionQOBD[] outputArray = actionList.AsParallel().Select(x => new ActionQOBD
            {
                ID = x.ID,
                Name = Utility.encodeStringToBase64(x.Name),
                Right = (new List<Privilege> { { x.Right } }.PrivilegeTypeToArray()).FirstOrDefault(),
            }).ToArray();
            
            return outputArray;
        }

        public static ActionFilterQOBD ActionTypeToFilterArray(this QOBDCommon.Entities.Action action, ESearchOption filterOperator)
        {
            ActionFilterQOBD actionQCBD = new ActionFilterQOBD();
            if (action != null)
            {
                actionQCBD.ID = action.ID;
                actionQCBD.Name = Utility.encodeStringToBase64(action.Name);
                actionQCBD.Operator = filterOperator.ToString();
            }
            return actionQCBD;
        }


        //====================================================================================
        //===============================[ Agent_role ]===========================================
        //====================================================================================

        public static List<Agent_role> ArrayTypeToAgent_role(this Agent_roleQOBD[] agent_roleQOBDList)
        {
            List<Agent_role> outputList = agent_roleQOBDList.AsParallel().Select(x => new Agent_role
            {
                ID = x.ID,
                AgentId = x.AgentId,
                RoleId = x.RoleId,
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
            }).ToList();
            
            return outputList;
        }

        public static Agent_roleQOBD[] Agent_roleTypeToArray(this List<Agent_role> agent_roleList)
        {
            Agent_roleQOBD[] outputArray = agent_roleList.AsParallel().Select(x => new Agent_roleQOBD
            {
                ID = x.ID,
                AgentId = x.AgentId,
                RoleId = x.RoleId,
                Date = Utility.encodeStringToBase64(x.Date.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static Agent_roleFilterQOBD Agent_roleTypeToFilterArray(this Agent_role agent_role, ESearchOption filterOperator)
        {
            Agent_roleFilterQOBD agent_roleQCBD = new Agent_roleFilterQOBD();
            if (agent_role != null)
            {
                agent_roleQCBD.ID = agent_role.ID;
                agent_roleQCBD.AgentId = agent_role.AgentId;
                agent_roleQCBD.RoleId = agent_role.RoleId;
                agent_roleQCBD.Date = Utility.encodeStringToBase64(agent_role.Date.ToString());
                agent_roleQCBD.Operator = filterOperator.ToString();
            }
            return agent_roleQCBD;
        }

        //====================================================================================
        //===============================[ Privilege ]===========================================
        //====================================================================================

        public static List<Privilege> ArrayTypeToPrivilege(this PrivilegeQOBD[] privilegeQOBDList)
        {
            List<Privilege> outputList = privilegeQOBDList.AsParallel().Select(x => new Privilege
            {
                ID = x.ID,
                Role_actionId = x.Role_actionId,
                IsWrite = Utility.convertToBoolean(Utility.decodeBase64ToString(x._Write)),
                IsRead = Utility.convertToBoolean(Utility.decodeBase64ToString(x._Read)),
                IsDelete = Utility.convertToBoolean(Utility.decodeBase64ToString(x._Delete)),
                IsUpdate = Utility.convertToBoolean(Utility.decodeBase64ToString(x._Update)),
                IsSendMail = Utility.convertToBoolean(Utility.decodeBase64ToString(x.SendEmail)),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
            }).ToList();
            
            return outputList;
        }

        public static PrivilegeQOBD[] PrivilegeTypeToArray(this List<Privilege> privilegeList)
        {
            PrivilegeQOBD[] outputArray = privilegeList.AsParallel().Select(x => new PrivilegeQOBD
            {
                ID = x.ID,
                Role_actionId = x.Role_actionId,
                _Write = Utility.encodeStringToBase64((x.IsWrite) ? "1" : "0"),
                _Read = Utility.encodeStringToBase64((x.IsRead) ? "1" : "0"),
                _Delete = Utility.encodeStringToBase64((x.IsDelete) ? "1" : "0"),
                _Update = Utility.encodeStringToBase64((x.IsUpdate) ? "1" : "0"),
                SendEmail = Utility.encodeStringToBase64((x.IsSendMail) ? "1" : "0"),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
            }).ToArray();
            
            return outputArray;
        }

        public static PrivilegeFilterQOBD PrivilegeTypeToFilterArray(this Privilege privilege, ESearchOption filterOperator)
        {
            PrivilegeFilterQOBD privilegeQCBD = new PrivilegeFilterQOBD();
            if (privilege != null)
            {
                privilegeQCBD.ID = privilege.ID;
                privilegeQCBD.Role_actionId = privilege.Role_actionId;
                privilegeQCBD._Write = Utility.encodeStringToBase64((privilege.IsWrite) ? "1" : "0");
                privilegeQCBD._Read = Utility.encodeStringToBase64((privilege.IsRead) ? "1" : "0");
                privilegeQCBD._Delete = Utility.encodeStringToBase64((privilege.IsDelete) ? "1" : "0");
                privilegeQCBD._Update = Utility.encodeStringToBase64((privilege.IsUpdate) ? "1" : "0");
                privilegeQCBD.SendEmail = Utility.encodeStringToBase64((privilege.IsSendMail) ? "1" : "0");
                privilegeQCBD.Date = Utility.encodeStringToBase64(privilege.Date.ToString("yyyy-MM-dd H:mm:ss"));
                privilegeQCBD.Operator = filterOperator.ToString();
            }
            return privilegeQCBD;
        }


        //====================================================================================
        //===============================[ Order ]===========================================
        //====================================================================================

        public static List<Order> ArrayTypeToOrder(this CommandsQOBD[] OrderQOBDList)
        {
            List<Order> outputList = OrderQOBDList.AsParallel().Select(x => new Order
            {
                ID = x.ID,
                AgentId = x.AgentId,
                BillAddress = x.BillAddress,
                ClientId = x.ClientId,
                Comment1 = Utility.decodeBase64ToString(x.Comment1),
                Comment2 = Utility.decodeBase64ToString(x.Comment2),
                Comment3 = Utility.decodeBase64ToString(x.Comment3),
                Status = Utility.decodeBase64ToString(x.Status),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
                DeliveryAddress = x.DeliveryAddress,
                Tax = Convert.ToDouble(x.Tax),
            }).ToList();
            
            return outputList;
        }

        public static CommandsQOBD[] OrderTypeToArray(this List<Order> orderList)
        {
            CommandsQOBD[] outputArray = orderList.AsParallel().Select(x => new CommandsQOBD
            {
                ID = x.ID,
                AgentId = x.AgentId,
                BillAddress = x.BillAddress,
                ClientId = x.ClientId,
                Comment1 = Utility.encodeStringToBase64(x.Comment1),
                Comment2 = Utility.encodeStringToBase64(x.Comment2),
                Comment3 = Utility.encodeStringToBase64(x.Comment3),
                Status = Utility.encodeStringToBase64(x.Status),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
                DeliveryAddress = x.DeliveryAddress,
                Tax = x.Tax,
            }).ToArray();

            return outputArray;
        }

        public static CommandFilterQOBD CommandTypeToFilterArray(this Order order, ESearchOption filterOperator)
        {
            CommandFilterQOBD orderQCBD = new CommandFilterQOBD();
            if (order != null)
            {
                orderQCBD.ID = order.ID;
                orderQCBD.AgentId = order.AgentId;
                orderQCBD.BillAddress = order.BillAddress;
                orderQCBD.ClientId = order.ClientId;
                orderQCBD.Comment1 = Utility.encodeStringToBase64(order.Comment1);
                orderQCBD.Comment2 = Utility.encodeStringToBase64(order.Comment2);
                orderQCBD.Comment3 = Utility.encodeStringToBase64(order.Comment3);
                orderQCBD.Status = Utility.encodeStringToBase64(order.Status);
                orderQCBD.DeliveryAddress = order.DeliveryAddress;
                orderQCBD.Tax = order.Tax;
                orderQCBD.Operator = filterOperator.ToString();
            }
            return orderQCBD;
        }

        public static PdfQOBD ParamCommandPdfTypeToArray(this ParamOrderToPdf paramOrderToPdf)
        {
            PdfQOBD outputArray = new PdfQOBD();
            if (!paramOrderToPdf.Equals(null))
            {
                outputArray.BillId = paramOrderToPdf.BillId;
                outputArray.CommandId = paramOrderToPdf.OrderId;                
            }
            return outputArray;
        }

        //====================================================================================
        //===============================[ Tax_order ]======================================
        //====================================================================================

        public static List<Tax_order> ArrayTypeToTax_order(this Tax_commandQOBD[] Tax_orderQOBDList)
        {
            List<Tax_order> outputList = Tax_orderQOBDList.AsParallel().Select(x => new Tax_order
            {
                ID = x.ID,
                OrderId = x.CommandId,
                Date_insert = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date_insert)),
                Target = Utility.decodeBase64ToString(x.Target),
                Tax_value = Convert.ToDouble(x.Tax_value),
                TaxId = x.TaxId,
            }).ToList();
            
            return outputList;
        }

        public static Tax_commandQOBD[] Tax_orderTypeToArray(this List<Tax_order> Tax_orderList)
        {
            Tax_commandQOBD[] outputArray = Tax_orderList.AsParallel().Select(x => new Tax_commandQOBD
            {
                ID = x.ID,
                CommandId = x.OrderId,
                Date_insert = Utility.encodeStringToBase64(x.Date_insert.ToString("yyy-MM-dd H:mm:ss")),
                Target = Utility.encodeStringToBase64(x.Target),
                Tax_value = x.Tax_value,
                TaxId = x.TaxId,
            }).ToArray();
            
            return outputArray;
        }

        public static Tax_commandFilterQOBD Tax_commandTypeToFilterArray(this Tax_order Tax_command, ESearchOption filterOperator)
        {
            Tax_commandFilterQOBD Tax_commandQCBD = new Tax_commandFilterQOBD();
            if (Tax_command != null)
            {
                Tax_commandQCBD.ID = Tax_command.ID;
                Tax_commandQCBD.CommandId = Tax_command.OrderId;
                Tax_commandQCBD.Date_insert = Utility.encodeStringToBase64(Tax_command.Date_insert.ToString("yyy-MM-dd H:mm:ss"));
                Tax_commandQCBD.Target = Utility.encodeStringToBase64(Tax_command.Target);
                Tax_commandQCBD.Tax_value = Tax_command.Tax_value;
                Tax_commandQCBD.TaxId = Tax_command.TaxId;
                Tax_commandQCBD.Operator = filterOperator.ToString();
            }
            return Tax_commandQCBD;
        }

        //====================================================================================
        //===============================[ Client ]===========================================
        //====================================================================================

        public static List<Client> ArrayTypeToClient(this ClientQOBD[] ClientQOBD)
        {
            List<Client> outputList = ClientQOBD.AsParallel().Select(x => new Client
            {
                ID = x.ID,
                FirstName = Utility.decodeBase64ToString(x.FirstName),
                LastName = Utility.decodeBase64ToString(x.LastName),
                AgentId = x.AgentId,
                Comment = Utility.decodeBase64ToString(x.Comment),
                Phone = Utility.decodeBase64ToString(x.Phone),
                Status = Utility.decodeBase64ToString(x.Status),
                Company = Utility.decodeBase64ToString(x.Company),
                Email = Utility.decodeBase64ToString(x.Email),
                Fax = Utility.decodeBase64ToString(x.Fax),
                CompanyName = Utility.decodeBase64ToString(x.CompanyName),
                CRN = Utility.decodeBase64ToString(x.CRN),
                MaxCredit = Convert.ToDecimal(x.MaxCredit),
                Rib = Utility.decodeBase64ToString(x.Rib),
                PayDelay = x.PayDelay,
            }).ToList();

            return outputList;
        }

        public static ClientQOBD[] ClientTypeToArray(this List<Client> ClientList)
        {
            ClientQOBD[] outputArray = ClientList.AsParallel().Select(x=> new ClientQOBD {
                ID = x.ID,
                FirstName = Utility.encodeStringToBase64(x.FirstName),
                LastName = Utility.encodeStringToBase64(x.LastName),
                AgentId = x.AgentId,
                Comment = Utility.encodeStringToBase64(x.Comment),
                Phone = Utility.encodeStringToBase64(x.Phone),
                Status = Utility.encodeStringToBase64(x.Status),
                Company = Utility.encodeStringToBase64(x.Company),
                Email = Utility.encodeStringToBase64(x.Email),
                Fax = Utility.encodeStringToBase64(x.Fax),
                CompanyName = Utility.encodeStringToBase64(x.CompanyName),
                CRN = Utility.encodeStringToBase64(x.CRN),
                MaxCredit = Utility.encodeStringToBase64(x.MaxCredit.ToString()),
                Rib = Utility.encodeStringToBase64(x.Rib),
                PayDelay = x.PayDelay
        }).ToArray();
            
            return outputArray;
        }
        public static ClientFilterQOBD ClientTypeToFilterArray(this Client Client, ESearchOption filterOperator)
        {
            ClientFilterQOBD ClientQCBD = new ClientFilterQOBD();
            if (Client != null)
            {
                ClientQCBD.ID = Client.ID;
                ClientQCBD.FirstName = Utility.encodeStringToBase64(Client.FirstName);
                ClientQCBD.LastName = Utility.encodeStringToBase64(Client.LastName);
                ClientQCBD.AgentId = Client.AgentId;
                ClientQCBD.Comment = Utility.encodeStringToBase64(Client.Comment);
                ClientQCBD.Phone = Utility.encodeStringToBase64(Client.Phone);
                ClientQCBD.Status = Utility.encodeStringToBase64(Client.Status);
                ClientQCBD.Company = Utility.encodeStringToBase64(Client.Company);
                ClientQCBD.Email = Utility.encodeStringToBase64(Client.Email);
                ClientQCBD.Fax = Utility.encodeStringToBase64(Client.Fax);
                ClientQCBD.CompanyName = Utility.encodeStringToBase64(Client.CompanyName);
                ClientQCBD.CRN = Utility.encodeStringToBase64(Client.CRN);
                ClientQCBD.MaxCredit = Utility.encodeStringToBase64(Client.MaxCredit.ToString());
                ClientQCBD.Rib = Utility.encodeStringToBase64(Client.Rib);
                ClientQCBD.PayDelay = Client.PayDelay;
                ClientQCBD.Operator = filterOperator.ToString();
            }
            return ClientQCBD;
        }

        //====================================================================================
        //===============================[ Contact ]===========================================
        //====================================================================================

        public static List<Contact> ArrayTypeToContact(this ContactQOBD[] ContactQOBDList)
        {
            List<Contact> outputList = ContactQOBDList.AsParallel().Select(x => new Contact
            {
                ID = x.ID,
                Cellphone = Utility.decodeBase64ToString(x.Cellphone),
                ClientId = x.ClientId,
                Comment = Utility.decodeBase64ToString(x.Comment),
                Email = Utility.decodeBase64ToString(x.Email),
                Phone = Utility.decodeBase64ToString(x.Phone),
                Fax = Utility.decodeBase64ToString(x.Fax),
                Firstname = Utility.decodeBase64ToString(x.Firstname),
                LastName = Utility.decodeBase64ToString(x.LastName),
                Position = Utility.decodeBase64ToString(x.Position),
            }).ToList();
            
            return outputList;
        }

        public static ContactQOBD[] ContactTypeToArray(this List<Contact> ContactList)
        {
            ContactQOBD[] outputArray = ContactList.AsParallel().Select(x => new ContactQOBD
            {
                ID = x.ID,
                Position = Utility.encodeStringToBase64(x.Position),
                LastName = Utility.encodeStringToBase64(x.LastName),
                Firstname = Utility.encodeStringToBase64(x.Firstname),
                Comment = Utility.encodeStringToBase64(x.Comment),
                Phone = Utility.encodeStringToBase64(x.Phone),
                ClientId = x.ClientId,
                Cellphone = Utility.encodeStringToBase64(x.Cellphone),
                Email = Utility.encodeStringToBase64(x.Email),
                Fax = Utility.encodeStringToBase64(x.Fax),
            }).ToArray();
            
            return outputArray;
        }

        public static ContactFilterQOBD ContactTypeToFilterArray(this Contact Contact, ESearchOption filterOperator)
        {
            ContactFilterQOBD ContactQCBD = new ContactFilterQOBD();
            if (Contact != null)
            {
                ContactQCBD.ID = Contact.ID;
                ContactQCBD.Position = Utility.encodeStringToBase64(Contact.Position);
                ContactQCBD.LastName = Utility.encodeStringToBase64(Contact.LastName);
                ContactQCBD.Firstname = Utility.encodeStringToBase64(Contact.Firstname);
                ContactQCBD.Comment = Utility.encodeStringToBase64(Contact.Comment);
                ContactQCBD.Phone = Utility.encodeStringToBase64(Contact.Phone);
                ContactQCBD.ClientId = Contact.ClientId;
                ContactQCBD.Cellphone = Utility.encodeStringToBase64(Contact.Cellphone);
                ContactQCBD.Email = Utility.encodeStringToBase64(Contact.Email);
                ContactQCBD.Fax = Utility.encodeStringToBase64(Contact.Fax);
                ContactQCBD.Operator = filterOperator.ToString();
            }
            return ContactQCBD;
        }



        //====================================================================================
        //===============================[ Address ]===========================================
        //====================================================================================

        public static List<Address> ArrayTypeToAddress(this AddressQOBD[] AddressQOBDList)
        {
            List<Address> outputList = AddressQOBDList.AsParallel().Select(x => new Address
            {
                ID = x.ID,
                AddressName = Utility.decodeBase64ToString(x.Address),
                ClientId = x.ClientId,
                Comment = Utility.decodeBase64ToString(x.Comment),
                Email = Utility.decodeBase64ToString(x.Email),
                Phone = Utility.decodeBase64ToString(x.Phone),
                CityName = Utility.decodeBase64ToString(x.CityName),
                Country = Utility.decodeBase64ToString(x.Country),
                LastName = Utility.decodeBase64ToString(x.LastName),
                FirstName = Utility.decodeBase64ToString(x.FirstName),
                Name = Utility.decodeBase64ToString(x.Name),
                Name2 = Utility.decodeBase64ToString(x.Name2),
                Postcode = Utility.decodeBase64ToString(x.Postcode),
            }).ToList();
            
            return outputList;
        }

        public static AddressQOBD[] AddressTypeToArray(this List<Address> AddressList)
        {
            AddressQOBD[] outputArray = AddressList.AsParallel().Select(x => new AddressQOBD
            {
                ID = x.ID,
                Address = Utility.encodeStringToBase64(x.AddressName),
                ClientId = x.ClientId,
                Comment = Utility.encodeStringToBase64(x.Comment),
                Email = Utility.encodeStringToBase64(x.Email),
                Phone = Utility.encodeStringToBase64(x.Phone),
                CityName = Utility.encodeStringToBase64(x.CityName),
                Country = Utility.encodeStringToBase64(x.Country),
                LastName = Utility.encodeStringToBase64(x.LastName),
                FirstName = Utility.encodeStringToBase64(x.FirstName),
                Name = Utility.encodeStringToBase64(x.Name),
                Name2 = Utility.encodeStringToBase64(x.Name2),
                Postcode = Utility.encodeStringToBase64(x.Postcode),
            }).ToArray();
            
            return outputArray;
        }

        public static AddressFilterQOBD AddressTypeToFilterArray(this Address Address, ESearchOption filterOperator)
        {
            AddressFilterQOBD AddressQCBD = new AddressFilterQOBD();
            if (Address != null)
            {
                AddressQCBD.ID = Address.ID;
                AddressQCBD.Address = Utility.encodeStringToBase64(Address.AddressName);
                AddressQCBD.ClientId = Address.ClientId;
                AddressQCBD.Comment = Utility.encodeStringToBase64(Address.Comment);
                AddressQCBD.Email = Utility.encodeStringToBase64(Address.Email);
                AddressQCBD.Phone = Utility.encodeStringToBase64(Address.Phone);
                AddressQCBD.CityName = Utility.encodeStringToBase64(Address.CityName);
                AddressQCBD.Country = Utility.encodeStringToBase64(Address.Country);
                AddressQCBD.LastName = Utility.encodeStringToBase64(Address.LastName);
                AddressQCBD.FirstName = Utility.encodeStringToBase64(Address.FirstName);
                AddressQCBD.Name = Utility.encodeStringToBase64(Address.Name);
                AddressQCBD.Name2 = Utility.encodeStringToBase64(Address.Name2);
                AddressQCBD.Postcode = Utility.encodeStringToBase64(Address.Postcode);
                AddressQCBD.Operator = filterOperator.ToString();
            }
            return AddressQCBD;
        }


        //====================================================================================
        //===============================[ Bill ]===========================================
        //====================================================================================

        public static List<Bill> ArrayTypeToBill(this BillQOBD[] BillQOBDList)
        {
            List<Bill> outputList = BillQOBDList.AsParallel().Select(x => new Bill
            {
                ID = x.ID,
                ClientId = x.ClientId,
                OrderId = x.CommandId,
                Comment1 = Utility.decodeBase64ToString(x.Comment1),
                Comment2 = Utility.decodeBase64ToString(x.Comment2),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
                DateLimit = Utility.convertToDateTime(Utility.decodeBase64ToString(x.DateLimit)),
                Pay = x.Pay,
                PayDate = Utility.convertToDateTime(Utility.decodeBase64ToString(x.DatePay)),
                PayMod = Utility.decodeBase64ToString(x.PayMod),
                PayReceived = x.PayReceived,
            }).ToList();
            
            return outputList;
        }

        public static BillQOBD[] BillTypeToArray(this List<Bill> BillList)
        {
            BillQOBD[] outputArray = BillList.AsParallel().Select(x => new BillQOBD
            {
                ID = x.ID,
                ClientId = x.ClientId,
                CommandId = x.OrderId,
                Comment1 = Utility.encodeStringToBase64(x.Comment1),
                Comment2 = Utility.encodeStringToBase64(x.Comment2),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
                DateLimit = Utility.encodeStringToBase64(x.DateLimit.ToString("yyyy-MM-dd H:mm:ss")),
                Pay = x.Pay,
                DatePay = Utility.encodeStringToBase64(x.PayDate.ToString("yyyy-MM-dd H:mm:ss")),
                PayMod = Utility.encodeStringToBase64(x.PayMod),
                PayReceived = x.PayReceived,
            }).ToArray();
            
            return outputArray;
        }

        public static BillFilterQOBD BillTypeToFilterArray(this Bill Bill, ESearchOption filterOperator)
        {
            BillFilterQOBD BillQCBD = new BillFilterQOBD();
            if (Bill != null)
            {
                BillQCBD.ID = Bill.ID;
                BillQCBD.ClientId = Bill.ClientId;
                BillQCBD.CommandId = Bill.OrderId;
                BillQCBD.Comment1 = Utility.encodeStringToBase64(Bill.Comment1);
                BillQCBD.Comment2 = Utility.encodeStringToBase64(Bill.Comment2);
                BillQCBD.Date = Utility.encodeStringToBase64(Bill.Date.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.DateLimit = Utility.encodeStringToBase64(Bill.DateLimit.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.Pay = Bill.Pay;
                BillQCBD.DatePay = Utility.encodeStringToBase64(Bill.PayDate.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.PayMod = Utility.encodeStringToBase64(Bill.PayMod);
                BillQCBD.PayReceived = Bill.PayReceived;
                BillQCBD.Operator = filterOperator.ToString();
            }
            return BillQCBD;
        }

        //====================================================================================
        //===============================[ Delivery ]===========================================
        //====================================================================================

        public static List<Delivery> ArrayTypeToDelivery(this DeliveryQOBD[] DeliveryQOBDList)
        {
            List<Delivery> outputList = DeliveryQOBDList.AsParallel().Select(x => new Delivery
            {
                ID = x.ID,
                BillId = x.BillId,
                OrderId = x.CommandId,
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
                Package = x.Package,
                Status = Utility.decodeBase64ToString(x.Status),
            }).ToList();
            
            return outputList;
        }

        public static DeliveryQOBD[] DeliveryTypeToArray(this List<Delivery> DeliveryList)
        {
            DeliveryQOBD[] outputArray = DeliveryList.AsParallel().Select(x => new DeliveryQOBD
            {
                ID = x.ID,
                BillId = x.BillId,
                CommandId = x.OrderId,
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
                Package = x.Package,
                Status = Utility.encodeStringToBase64(x.Status),
            }).ToArray();
            
            return outputArray;
        }

        public static DeliveryFilterQOBD DeliveryTypeToFilterArray(this Delivery Delivery, ESearchOption filterOperator)
        {
            DeliveryFilterQOBD DeliveryQCBD = new DeliveryFilterQOBD();
            if (Delivery != null)
            {
                DeliveryQCBD.ID = Delivery.ID;
                DeliveryQCBD.BillId = Delivery.BillId;
                DeliveryQCBD.CommandId = Delivery.OrderId;
                DeliveryQCBD.Date = Utility.encodeStringToBase64(Delivery.Date.ToString("yyyy-MM-dd H:mm:ss"));
                DeliveryQCBD.Package = Delivery.Package;
                DeliveryQCBD.Status = Utility.encodeStringToBase64(Delivery.Status);
                DeliveryQCBD.Operator = filterOperator.ToString();
            }
            return DeliveryQCBD;
        }

        //====================================================================================
        //================================[ order_item ]====================================
        //====================================================================================

        public static List<Order_item> ArrayTypeToOrder_item(this Command_itemQOBD[] order_itemQOBDList)
        {
            List<Order_item> outputList = order_itemQOBDList.AsParallel().Select(x => new Order_item
            {
                ID = x.ID,
                OrderId = x.CommandId,
                Comment_Purchase_Price = Utility.decodeBase64ToString(x.Comment_Purchase_Price),
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Order = x.Order,
                Price = x.Price,
                Price_purchase = x.Price_purchase,
                Quantity = x.Quantity,
                Quantity_current = x.Quantity_current,
                Quantity_delivery = x.Quantity_delivery,
            }).ToList();
            
            return outputList;
        }

        public static Command_itemQOBD[] order_itemTypeToArray(this List<Order_item> Command_itemList)
        {
            Command_itemQOBD[] outputArray = Command_itemList.AsParallel().Select(x => new Command_itemQOBD
            {
                ID = x.ID,
                CommandId = x.OrderId,
                Comment_Purchase_Price = Utility.encodeStringToBase64(x.Comment_Purchase_Price),
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Order = x.Order,
                Price = x.Price,
                Price_purchase = x.Price_purchase,
                Quantity = x.Quantity,
                Quantity_current = x.Quantity_current,
                Quantity_delivery = x.Quantity_delivery,
            }).ToArray();
            
            return outputArray;
        }

        public static Command_itemFilterQOBD Order_itemTypeToFilterArray(this Order_item order_item, ESearchOption filterOperator)
        {
            Command_itemFilterQOBD order_itemQCBD = new Command_itemFilterQOBD();
            if (order_item != null)
            {
                order_itemQCBD.ID = order_item.ID;
                order_itemQCBD.CommandId = order_item.OrderId;
                order_itemQCBD.Comment_Purchase_Price = Utility.encodeStringToBase64(order_item.Comment_Purchase_Price);
                order_itemQCBD.Item_ref = Utility.encodeStringToBase64(order_item.Item_ref);
                order_itemQCBD.Order = order_item.Order;
                order_itemQCBD.Price = order_item.Price;
                order_itemQCBD.Price_purchase = order_item.Price_purchase;
                order_itemQCBD.Quantity = order_item.Quantity;
                order_itemQCBD.Quantity_current = order_item.Quantity_current;
                order_itemQCBD.Quantity_delivery = order_item.Quantity_delivery;
                order_itemQCBD.Operator = filterOperator.ToString();
            }
            return order_itemQCBD;
        }
        

        //====================================================================================
        //==================================[ Tax ]===========================================
        //====================================================================================

        public static List<Tax> ArrayTypeToTax(this TaxQOBD[] TaxQOBDList)
        {
            List<Tax> outputList = TaxQOBDList.AsParallel().Select(x => new Tax
            {
                ID = x.ID,
                Tax_current = x.Tax_current,
                Type = Utility.decodeBase64ToString(x.Type),
                Value = Convert.ToDouble(x.Value),
                Date_insert = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date_insert)),
                Comment = Utility.decodeBase64ToString(x.Comment),
            }).ToList();
            
            return outputList;
        }

        public static TaxQOBD[] TaxTypeToArray(this List<Tax> TaxList)
        {
            TaxQOBD[] outputArray = TaxList.AsParallel().Select(x => new TaxQOBD
            {
                ID = x.ID,
                Tax_current = x.Tax_current,
                Type = Utility.encodeStringToBase64(x.Type),
                Value = x.Value,
                Date_insert = Utility.encodeStringToBase64(x.Date_insert.ToString("yyyy-MM-dd H:mm:ss")),
                Comment = Utility.encodeStringToBase64(x.Comment),
            }).ToArray();
            
            return outputArray;
        }

        public static TaxFilterQOBD TaxTypeToFilterArray(this Tax Tax, ESearchOption filterOperator)
        {
            TaxFilterQOBD TaxQCBD = new TaxFilterQOBD();
            if (Tax != null)
            {
                TaxQCBD.ID = Tax.ID;
                TaxQCBD.Tax_current = Tax.Tax_current;
                TaxQCBD.Type = Utility.encodeStringToBase64(Tax.Type);
                TaxQCBD.Value = Tax.Value;
                TaxQCBD.Date_insert = Utility.encodeStringToBase64(Tax.Date_insert.ToString("yyyy-MM-dd H:mm:ss"));
                TaxQCBD.Comment = Utility.encodeStringToBase64(Tax.Comment);
                TaxQCBD.Operator = filterOperator.ToString();
            }
            return TaxQCBD;
        }



        //====================================================================================
        //===============================[ Provider_item ]===========================================
        //====================================================================================

        public static List<Provider_item> ArrayTypeToProvider_item(this Provider_itemQOBD[] Provider_itemQOBDList)
        {
            List<Provider_item> outputList = Provider_itemQOBDList.AsParallel().Select(x => new Provider_item
            {
                ID = x.ID,
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Provider_name = Utility.decodeBase64ToString(x.Provider_name),
            }).ToList();
            
            return outputList;
        }

        public static Provider_itemQOBD[] Provider_itemTypeToArray(this List<Provider_item> Provider_itemList)
        {
            Provider_itemQOBD[] outputArray = Provider_itemList.AsParallel().Select(x => new Provider_itemQOBD
            {
                ID = x.ID,
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Provider_name = Utility.encodeStringToBase64(x.Provider_name),
            }).ToArray();
            
            return outputArray;
        }

        public static Provider_itemFilterQOBD Provider_itemTypeToFilterArray(this Provider_item Provider_item, ESearchOption filterOperator)
        {
            Provider_itemFilterQOBD Provider_itemQCBD = new Provider_itemFilterQOBD();
            if (Provider_item != null)
            {
                Provider_itemQCBD.ID = Provider_item.ID;
                Provider_itemQCBD.Item_ref = Utility.encodeStringToBase64(Provider_item.Item_ref);
                Provider_itemQCBD.Provider_name = Utility.encodeStringToBase64(Provider_item.Provider_name);
                Provider_itemQCBD.Operator = filterOperator.ToString();
            }
            return Provider_itemQCBD;
        }

        //====================================================================================
        //===============================[ Provider ]===========================================
        //====================================================================================

        public static List<Provider> ArrayTypeToProvider(this ProviderQOBD[] ProviderQOBDList)
        {
            List<Provider> outputList = ProviderQOBDList.AsParallel().Select(x => new Provider
            {
                ID = x.ID,
                Name = Utility.decodeBase64ToString(x.Name),
                Source = x.Source,
            }).ToList();
            
            return outputList;
        }

        public static ProviderQOBD[] ProviderTypeToArray(this List<Provider> ProviderList)
        {
            ProviderQOBD[] outputArray = ProviderList.AsParallel().Select(x => new ProviderQOBD
            {
                ID = x.ID,
                Name = Utility.encodeStringToBase64(x.Name),
                Source = x.Source,
            }).ToArray();
            
            return outputArray;
        }

        public static ProviderFilterQOBD ProviderTypeToFilterArray(this Provider Provider, ESearchOption filterOperator)
        {
            ProviderFilterQOBD ProviderQCBD = new ProviderFilterQOBD();
            if (Provider != null)
            {
                ProviderQCBD.ID = Provider.ID;
                ProviderQCBD.Name = Utility.encodeStringToBase64(Provider.Name);
                ProviderQCBD.Source = Provider.Source;
                ProviderQCBD.Operator = filterOperator.ToString();
            }
            return ProviderQCBD;
        }

        //====================================================================================
        //===============================[ Item ]===========================================
        //====================================================================================

        public static List<Item> ArrayTypeToItem(this ItemQOBD[] ItemQOBDList)
        {
            List<Item> outputList = ItemQOBDList.AsParallel().Select(x => new Item
            {
                ID = x.ID,
                Comment = Utility.decodeBase64ToString(x.Comment),
                Erasable = Utility.decodeBase64ToString(x.Erasable),
                Name = Utility.decodeBase64ToString(x.Name),
                Price_purchase = x.Price_purchase,
                Number_of_sale = x.Number_of_sale,
                Price_sell = x.Price_sell,
                Ref = Utility.decodeBase64ToString(x.Ref),
                Type_sub = Utility.decodeBase64ToString(x.Type_sub),
                Source = Convert.ToInt32(Utility.decodeBase64ToString(x.Source.ToString())),
                Type = Utility.decodeBase64ToString(x.Type),
            }).ToList();
            
            return outputList;
        }

        public static ItemQOBD[] ItemTypeToArray(this List<Item> ItemList)
        {
            ItemQOBD[] outputArray = ItemList.AsParallel().Select(x => new ItemQOBD
            {
                ID = x.ID,
                Comment = Utility.encodeStringToBase64(x.Comment),
                Erasable = Utility.encodeStringToBase64(x.Erasable),
                Name = Utility.encodeStringToBase64(x.Name),
                Price_purchase = x.Price_purchase,
                Number_of_sale = x.Number_of_sale,
                Price_sell = x.Price_sell,
                Ref = Utility.encodeStringToBase64(x.Ref),
                Type_sub = Utility.encodeStringToBase64(x.Type_sub),
                Source = Utility.encodeStringToBase64(x.Source.ToString()),
                Type = Utility.encodeStringToBase64(x.Type),
            }).ToArray();
            
            return outputArray;
        }

        public static ItemFilterQOBD ItemTypeToFilterArray(this Item Item, ESearchOption filterOperator)
        {
            ItemFilterQOBD ItemQCBD = new ItemFilterQOBD();
            if (Item != null)
            {
                ItemQCBD.ID = Item.ID;
                ItemQCBD.Comment = Utility.encodeStringToBase64(Item.Comment);
                ItemQCBD.Erasable = Utility.encodeStringToBase64(Item.Erasable);
                ItemQCBD.Name = Utility.encodeStringToBase64(Item.Name);
                ItemQCBD.Price_purchase = Item.Price_purchase;
                ItemQCBD.Price_sell = Item.Price_sell;
                ItemQCBD.Number_of_sale = Item.Number_of_sale;
                ItemQCBD.Option = Item.Option;
                ItemQCBD.Ref = Utility.encodeStringToBase64(Item.Ref);
                ItemQCBD.Type_sub = Utility.encodeStringToBase64(Item.Type_sub);
                ItemQCBD.Source = Utility.encodeStringToBase64(Item.Source.ToString());
                ItemQCBD.Type = Utility.encodeStringToBase64(Item.Type);
                ItemQCBD.Operator = filterOperator.ToString();
            }
            return ItemQCBD;
        }




        //====================================================================================
        //===============================[ Item_delivery ]===========================================
        //====================================================================================
        

        public static List<Item_delivery> ArrayTypeToItem_delivery(this Item_deliveryQOBD[] Item_deliveryQOBDList)
        {
            List<Item_delivery> outputList = Item_deliveryQOBDList.AsParallel().Select(x => new Item_delivery
            {
                ID = x.ID,
                DeliveryId = x.DeliveryId,
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Quantity_delivery = x.Quantity_delivery,
            }).ToList();
            
            return outputList;
        }

        public static Item_deliveryQOBD[] Item_deliveryTypeToArray(this List<Item_delivery> Item_deliveryList)
        {
            Item_deliveryQOBD[] outputArray = Item_deliveryList.AsParallel().Select(x => new Item_deliveryQOBD
            {
                ID = x.ID,
                DeliveryId = x.DeliveryId,
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Quantity_delivery = x.Quantity_delivery,
            }).ToArray();
            
            return outputArray;
        }

        public static Item_deliveryFilterQOBD Item_deliveryTypeToFilterArray(this Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            Item_deliveryFilterQOBD Item_deliveryQCBD = new Item_deliveryFilterQOBD();
            if (Item_delivery != null)
            {
                Item_deliveryQCBD.ID = Item_delivery.ID;
                Item_deliveryQCBD.DeliveryId = Item_delivery.DeliveryId;
                Item_deliveryQCBD.Item_ref = Utility.encodeStringToBase64(Item_delivery.Item_ref);
                Item_deliveryQCBD.Quantity_delivery = Item_delivery.Quantity_delivery;
                Item_deliveryQCBD.Operator = filterOperator.ToString();
            }
            return Item_deliveryQCBD;
        }



        //====================================================================================
        //===============================[ Tax_item ]===========================================
        //====================================================================================


        public static List<Tax_item> ArrayTypeToTax_item(this Tax_itemQOBD[] Tax_itemQOBDList)
        {
            List<Tax_item> outputList = Tax_itemQOBDList.AsParallel().Select(x => new Tax_item
            {
                ID = x.ID,
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Tax_type = Utility.decodeBase64ToString(x.Tax_type),
                TaxId = x.TaxId,
                Tax_value = x.Tax_value,
            }).ToList();
            
            return outputList;
        }

        public static Tax_itemQOBD[] Tax_itemTypeToArray(this List<Tax_item> Tax_itemList)
        {
            Tax_itemQOBD[] outputArray = Tax_itemList.AsParallel().Select(x => new Tax_itemQOBD
            {
                ID = x.ID,
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Tax_type = Utility.encodeStringToBase64(x.Tax_type),
                TaxId = x.TaxId,
                Tax_value = x.Tax_value,
            }).ToArray();
            
            return outputArray;
        }

        public static Tax_itemFilterQOBD Tax_itemTypeToFilterArray(this Tax_item Tax_item, ESearchOption filterOperator)
        {
            Tax_itemFilterQOBD Tax_itemQCBD = new Tax_itemFilterQOBD();
            if (Tax_item != null)
            {
                Tax_itemQCBD.ID = Tax_item.ID;
                Tax_itemQCBD.Item_ref = Utility.encodeStringToBase64(Tax_item.Item_ref);
                Tax_itemQCBD.Tax_type = Utility.encodeStringToBase64(Tax_item.Tax_type);
                Tax_itemQCBD.TaxId = Tax_item.TaxId;
                Tax_itemQCBD.Tax_value = Tax_item.Tax_value;
                Tax_itemQCBD.Operator = filterOperator.ToString();
            }
            return Tax_itemQCBD;
        }


        //====================================================================================
        //===============================[ Auto_ref ]===========================================
        //====================================================================================

        public static List<Auto_ref> ArrayTypeToAuto_ref(this Auto_refsQOBD[] Auto_refQOBDList)
        {
            List<Auto_ref> outputList = Auto_refQOBDList.AsParallel().Select(x => new Auto_ref
            {
                ID = x.ID,
                RefId = x.RefId,
            }).ToList();
            
            return outputList;
        }

        public static Auto_refsQOBD[] Auto_refTypeToArray(this List<Auto_ref> Auto_refList)
        {
            Auto_refsQOBD[] outputArray = Auto_refList.AsParallel().Select(x => new Auto_refsQOBD
            {
                ID = x.ID,
                RefId = x.RefId,
            }).ToArray();
            
            return outputArray;
        }

        public static Auto_refsFilterQOBD Auto_refTypeToFilterArray(this Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            Auto_refsFilterQOBD Auto_refQCBD = new Auto_refsFilterQOBD();
            if (Auto_ref != null)
            {
                Auto_refQCBD.ID = Auto_ref.ID;
                Auto_refQCBD.RefId = Auto_ref.RefId;
                Auto_refQCBD.Operator = filterOperator.ToString();
            }
            return Auto_refQCBD;
        }
    }


}
