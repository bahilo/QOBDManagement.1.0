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

        #region [ ChatRoom ]
        //====================================================================================
        //===============================[ Discussion ]===========================================
        //====================================================================================

        public static List<Discussion> ArrayTypeToDiscussion(this DiscussionChatRoom[] discussionChatRoomList)
        {
            object _lock = new object(); List<Discussion> returnList = new List<Discussion>();
            if (discussionChatRoomList != null)
            {
                //Parallel.ForEach(discussionChatRoomList, (discussionChat) =>
                foreach (var discussionChat in discussionChatRoomList)
                {
                    Discussion discussion = new Discussion();
                    discussion.ID = Utility.intTryParse(Utility.decodeBase64ToString(discussionChat.ID));
                    discussion.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(discussionChat.Date));

                    lock (_lock) returnList.Add(discussion);
                }
                //);
            }
            return returnList;
        }

        public static DiscussionChatRoom[] DiscussionTypeToArray(this List<Discussion> discussionList)
        {
            int i = 0;
            DiscussionChatRoom[] returnChatArray = new DiscussionChatRoom[discussionList.Count];
            if (discussionList != null)
            {
                Parallel.ForEach(discussionList, (discussion) =>
                {
                    DiscussionChatRoom discussionChat = new DiscussionChatRoom();
                    discussionChat.ID = Utility.encodeStringToBase64(discussion.ID.ToString());
                    discussionChat.Date = Utility.encodeStringToBase64(discussion.Date.ToString("yyyy-MM-dd H:mm:ss"));

                    returnChatArray[i] = discussionChat;
                    i++;
                });
            }
            return returnChatArray;
        }

        public static DiscussionFilterChatRoom DiscussionTypeToFilterArray(this Discussion discussion, string filterOperator)
        {
            DiscussionFilterChatRoom discussionChat = new DiscussionFilterChatRoom();
            if (discussion != null)
            {
                discussionChat.ID = Utility.encodeStringToBase64(discussion.ID.ToString());
                discussionChat.Date = Utility.encodeStringToBase64(discussion.Date.ToString("yyyy-MM-dd H:mm:ss"));
                discussionChat.Operator = Utility.encodeStringToBase64(filterOperator);
            }
            return discussionChat;
        }

        //====================================================================================
        //===============================[ Message ]===========================================
        //====================================================================================

        public static List<Message> ArrayTypeToMessage(this MessageChatRoom[] messageChatRoomList)
        {
            object _lock = new object(); List<Message> returnList = new List<Message>();
            if (messageChatRoomList != null)
            {
                //Parallel.ForEach(messageChatRoomList, (messageChat) =>
                foreach (var messageChat in messageChatRoomList)
                {
                    Message message = new Message();
                    message.ID = Utility.intTryParse(Utility.decodeBase64ToString(messageChat.ID));
                    message.Status = Utility.intTryParse(Utility.decodeBase64ToString(messageChat.Status));
                    message.UserId = Utility.intTryParse(Utility.decodeBase64ToString(messageChat.UserId));
                    message.DiscussionId = Utility.intTryParse(Utility.decodeBase64ToString(messageChat.DiscussionId));
                    message.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(messageChat.Date));
                    message.Content = Utility.decodeBase64ToString(messageChat.Content);

                    lock (_lock) returnList.Add(message);
                }
                //);
            }
            return returnList;
        }

        public static MessageChatRoom[] MessageTypeToArray(this List<Message> messageList)
        {
            int i = 0;
            MessageChatRoom[] returnChatArray = new MessageChatRoom[messageList.Count];
            if (messageList != null)
            {
                Parallel.ForEach(messageList, (message) =>
                {
                    MessageChatRoom messageChat = new MessageChatRoom();
                    messageChat.ID = Utility.encodeStringToBase64(message.ID.ToString());
                    messageChat.Status = Utility.encodeStringToBase64(message.Status.ToString());
                    messageChat.UserId = Utility.encodeStringToBase64(message.UserId.ToString());
                    messageChat.DiscussionId = Utility.encodeStringToBase64(message.DiscussionId.ToString());
                    messageChat.Date = Utility.encodeStringToBase64(message.Date.ToString("yyyy-MM-dd H:mm:ss"));
                    messageChat.Content = Utility.encodeStringToBase64(message.Content);

                    returnChatArray[i] = messageChat;
                    i++;
                });
            }
            return returnChatArray;
        }

        public static MessageFilterChatRoom MessageTypeToFilterArray(this Message message, string filterOperator)
        {
            MessageFilterChatRoom messageChat = new MessageFilterChatRoom();
            if (message != null)
            {
                messageChat.ID = Utility.encodeStringToBase64(message.ID.ToString());
                messageChat.Status = Utility.encodeStringToBase64(message.Status.ToString());
                messageChat.UserId = Utility.encodeStringToBase64(message.UserId.ToString());
                messageChat.DiscussionId = Utility.encodeStringToBase64(message.DiscussionId.ToString());
                messageChat.Date = Utility.encodeStringToBase64(message.Date.ToString("yyyy-MM-dd H:mm:ss"));
                messageChat.Content = Utility.encodeStringToBase64(message.Content);
                messageChat.Operator = Utility.encodeStringToBase64(filterOperator);
            }
            return messageChat;
        }

        //====================================================================================
        //===============================[ User_discussion ]===========================================
        //====================================================================================

        public static List<User_discussion> ArrayTypeToUser_discussion(this User_discussionChatRoom[] user_discussionChatRoomList)
        {
            object _lock = new object(); List<User_discussion> returnList = new List<User_discussion>();
            if (user_discussionChatRoomList != null)
            {
                //Parallel.ForEach(user_discussionChatRoomList, (user_discussionChat) =>
                foreach (var user_discussionChat in user_discussionChatRoomList)
                {
                    User_discussion user_discussion = new User_discussion();
                    user_discussion.ID = Utility.intTryParse(Utility.decodeBase64ToString(user_discussionChat.ID));
                    user_discussion.UserId = Utility.intTryParse(Utility.decodeBase64ToString(user_discussionChat.UserId));
                    user_discussion.DiscussionId = Utility.intTryParse(Utility.decodeBase64ToString(user_discussionChat.DiscussionId));
                    user_discussion.Status = Utility.intTryParse(Utility.decodeBase64ToString(user_discussionChat.Status));

                    lock (_lock) returnList.Add(user_discussion);
                }
                //);
            }
            return returnList;
        }

        public static User_discussionChatRoom[] User_discussionTypeToArray(this List<User_discussion> user_discussionList)
        {
            int i = 0;
            User_discussionChatRoom[] returnChatArray = new User_discussionChatRoom[user_discussionList.Count];
            if (user_discussionList != null)
            {
                Parallel.ForEach(user_discussionList, (user_discussion) =>
                {
                    User_discussionChatRoom user_discussionChat = new User_discussionChatRoom();
                    user_discussionChat.ID = Utility.encodeStringToBase64(user_discussion.ID.ToString());
                    user_discussionChat.UserId = Utility.encodeStringToBase64(user_discussion.UserId.ToString());
                    user_discussionChat.DiscussionId = Utility.encodeStringToBase64(user_discussion.DiscussionId.ToString());
                    user_discussionChat.Status = Utility.encodeStringToBase64(user_discussion.Status.ToString());

                    returnChatArray[i] = user_discussionChat;
                    i++;
                });
            }
            return returnChatArray;
        }

        public static User_discussionFilterChatRoom User_discussionTypeToFilterArray(this User_discussion user_discussion, string filterOperator)
        {
            User_discussionFilterChatRoom user_discussionChat = new User_discussionFilterChatRoom();
            if (user_discussion != null)
            {
                user_discussionChat.ID = Utility.encodeStringToBase64(user_discussion.ID.ToString());
                user_discussionChat.UserId = Utility.encodeStringToBase64(user_discussion.UserId.ToString());
                user_discussionChat.DiscussionId = Utility.encodeStringToBase64(user_discussion.DiscussionId.ToString());
                user_discussionChat.Status = Utility.encodeStringToBase64(user_discussion.Status.ToString());
                user_discussionChat.Operator = Utility.encodeStringToBase64(filterOperator);
            }
            return user_discussionChat;
        }
        #endregion

        //====================================================================================
        //===============================[ Agent ]===========================================
        //====================================================================================

        public static List<Agent> ArrayTypeToAgent(this AgentQOBD[] agentQOBDList)
        {
            List<Agent> outputList = agentQOBDList.AsParallel().Select(x => new Agent
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                FirstName = Utility.decodeBase64ToString(x.FirstName),
                LastName = Utility.decodeBase64ToString(x.LastName),
                UserName = Utility.decodeBase64ToString(x.UserName),
                HashedPassword = Utility.decodeBase64ToString(x.Password),
                Picture = Utility.decodeBase64ToString(x.Picture),
                Phone = Utility.decodeBase64ToString(x.Phone),
                Status = Utility.decodeBase64ToString(x.Status),
                IPAddress = Utility.decodeBase64ToString(x.IPAddress),
                IsOnline = (Utility.intTryParse(Utility.decodeBase64ToString(x.IsOnline)) == 1) ? true : false,
                ListSize = Utility.intTryParse(Utility.decodeBase64ToString(x.ListSize)),
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
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                FirstName = Utility.encodeStringToBase64(x.FirstName),
                LastName = Utility.encodeStringToBase64(x.LastName),
                UserName = Utility.encodeStringToBase64(x.UserName),
                Password = Utility.encodeStringToBase64(x.HashedPassword),
                Picture = Utility.encodeStringToBase64(x.Picture),
                Phone = Utility.encodeStringToBase64(x.Phone),
                Status = Utility.encodeStringToBase64(x.Status),
                IPAddress = Utility.encodeStringToBase64(x.IPAddress),
                IsOnline = (x.IsOnline) ? Utility.encodeStringToBase64("1") : Utility.encodeStringToBase64("0"),
                ListSize = Utility.encodeStringToBase64(x.ListSize.ToString()),
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
                agentQCBD.ID = Utility.encodeStringToBase64(agent.ID.ToString());
                agentQCBD.FirstName = Utility.encodeStringToBase64(agent.FirstName);
                agentQCBD.LastName = Utility.encodeStringToBase64(agent.LastName);
                agentQCBD.UserName = Utility.encodeStringToBase64(agent.UserName);
                agentQCBD.Password = Utility.encodeStringToBase64(agent.HashedPassword);
                agentQCBD.Picture = Utility.encodeStringToBase64(agent.Picture);
                agentQCBD.Phone = Utility.encodeStringToBase64(agent.Phone);
                agentQCBD.Status = Utility.encodeStringToBase64(agent.Status);
                agentQCBD.IPAddress = Utility.encodeStringToBase64(agent.IPAddress);
                agentQCBD.ListSize = Utility.encodeStringToBase64(agent.ListSize.ToString());
                agentQCBD.IsOnline = (agent.IsOnline) ? Utility.encodeStringToBase64("1") : Utility.encodeStringToBase64("0");
                agentQCBD.Email = Utility.encodeStringToBase64(agent.Email);
                agentQCBD.Fax = Utility.encodeStringToBase64(agent.Fax);
                agentQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return agentQCBD;
        }


        //====================================================================================
        //===============================[ Notification ]===========================================
        //====================================================================================

        public static List<Notification> ArrayTypeToNotification(this NotificationQOBD[] notificationQOBDList)
        {
            List<Notification> outputList = notificationQOBDList.AsParallel().Select(x => new Notification
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                BillId = Utility.intTryParse(Utility.decodeBase64ToString(x.BillId)),
                Reminder1 = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Reminder1)),
                Reminder2 = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Reminder2)),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
            }).ToList();

            return outputList;
        }

        public static NotificationQOBD[] NotificationTypeToArray(this List<Notification> notificationList)
        {
            NotificationQOBD[] outputArray = notificationList.AsParallel().Select(x => new NotificationQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                BillId = Utility.encodeStringToBase64(x.BillId.ToString()),
                Reminder1 = Utility.encodeStringToBase64(x.Reminder1.ToString("yyyy-MM-dd H:mm:ss")),
                Reminder2 = Utility.encodeStringToBase64(x.Reminder2.ToString("yyyy-MM-dd H:mm:ss")),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
            }).ToArray();

            return outputArray;
        }

        public static NotificationFilterQOBD NotificationTypeToFilterArray(this Notification notification, ESearchOption filterOperator)
        {
            NotificationFilterQOBD notificationQCBD = new NotificationFilterQOBD();
            if (notification != null)
            {
                notificationQCBD.ID = Utility.encodeStringToBase64(notification.ID.ToString());
                notificationQCBD.BillId = Utility.encodeStringToBase64(notification.BillId.ToString());
                notificationQCBD.Reminder1 = Utility.encodeStringToBase64(notification.Reminder1.ToString("yyyy-MM-dd H:mm:ss"));
                notificationQCBD.Reminder2 = Utility.encodeStringToBase64(notification.Reminder2.ToString("yyyy-MM-dd H:mm:ss"));
                notificationQCBD.Date = Utility.encodeStringToBase64(notification.Date.ToString("yyyy-MM-dd H:mm:ss"));
                notificationQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return notificationQCBD;
        }

        //====================================================================================
        //===============================[ Statistic ]===========================================
        //====================================================================================

        public static List<Statistic> ArrayTypeToStatistic(this StatisticQOBD[] statisticQOBDList)
        {
            List <Statistic> outputList = statisticQOBDList.AsParallel().Select(x => new Statistic
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                BillId = Utility.intTryParse(Utility.decodeBase64ToString(x.BillId)),
                Bill_datetime = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Bill_date)),
                Company = Utility.decodeBase64ToString(x.Company),
                Date_limit = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date_limit)),
                Income = Utility.decimalTryParse(Utility.decodeBase64ToString(x.Income)),
                Income_percent = Utility.doubleTryParse(Utility.decodeBase64ToString(x.Income_percent).Replace("%", "")),
                Pay_date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Pay_date)),
                Pay_received = Utility.decimalTryParse(Utility.decodeBase64ToString(x.Pay_received).Split(new char[] { ' ' }).FirstOrDefault()),
                Price_purchase_total = Utility.decimalTryParse(Utility.decodeBase64ToString(x.Price_purchase_total)),
                Tax_value = x.Tax_value,
                Total = Utility.decimalTryParse(Utility.decodeBase64ToString(x.Total)),
                Total_tax_included = Utility.decimalTryParse(Utility.decodeBase64ToString(x.Total_tax_included)),
            }).ToList();

            return outputList;
        }

        public static StatisticQOBD[] StatisticTypeToArray(this List<Statistic> statisticList)
        {
            StatisticQOBD[] outputArray = statisticList.AsParallel().Select(x => new StatisticQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                BillId = Utility.encodeStringToBase64(x.BillId.ToString()),
                Bill_date = Utility.encodeStringToBase64(x.Bill_datetime.ToString("yyyy-MM-dd H:mm:ss")),
                Company = Utility.encodeStringToBase64(x.Company),
                Date_limit = Utility.encodeStringToBase64(x.Date_limit.ToString("yyyy-MM-dd H:mm:ss")),
                Income = Utility.encodeStringToBase64(x.Income.ToString()),
                Income_percent = Utility.encodeStringToBase64(x.Income_percent.ToString()),
                Pay_date = Utility.encodeStringToBase64(x.Pay_date.ToString("yyyy-MM-dd H:mm:ss")),
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
                statisticQCBD.ID = Utility.encodeStringToBase64(statistic.ID.ToString());
                statisticQCBD.Option = Utility.encodeStringToBase64(statistic.Option.ToString());
                statisticQCBD.BillId = Utility.encodeStringToBase64(statistic.BillId.ToString());
                statisticQCBD.Bill_date = Utility.encodeStringToBase64(statistic.Bill_datetime.ToString("yyyy-MM-dd H:mm:ss"));
                statisticQCBD.Company = Utility.encodeStringToBase64(statistic.Company);
                statisticQCBD.Date_limit = Utility.encodeStringToBase64(statistic.Date_limit.ToString("yyyy-MM-dd H:mm:ss"));
                statisticQCBD.Income = Utility.encodeStringToBase64(statistic.Income.ToString());
                statisticQCBD.Income_percent = Utility.encodeStringToBase64(statistic.Income_percent.ToString());
                statisticQCBD.Pay_date = Utility.encodeStringToBase64(statistic.Pay_date.ToString("yyyy-MM-dd H:mm:ss"));
                statisticQCBD.Pay_received = Utility.encodeStringToBase64(statistic.Pay_received.ToString());
                statisticQCBD.Price_purchase_total = Utility.encodeStringToBase64(statistic.Price_purchase_total.ToString());
                statisticQCBD.Tax_value = statistic.Tax_value;
                statisticQCBD.Total = Utility.encodeStringToBase64(statistic.Total.ToString());
                statisticQCBD.Total_tax_included = Utility.encodeStringToBase64(statistic.Total_tax_included.ToString());
                statisticQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Name = Utility.decodeBase64ToString(x.Name),
                Value = Utility.decodeBase64ToString(x.Value),
            }).ToList();

            return outputList;
        }

        public static InfosQOBD[] InfosTypeToArray(this List<Info> infosList)
        {
            InfosQOBD[] outputArray = infosList.AsParallel().Select(x => new InfosQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
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
                infosQCBD.ID = Utility.encodeStringToBase64(infos.ID.ToString());
                infosQCBD.Option = Utility.encodeStringToBase64(infos.Option.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                AgentId = Utility.intTryParse(Utility.decodeBase64ToString(x.AgentId)),
                Action = Utility.decodeBase64ToString(x.Action),
                TargetId = Utility.intTryParse(Utility.decodeBase64ToString(x.TargetId)),
                TargetName = Utility.decodeBase64ToString(x.TargetName),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
            }).ToList();
          
            return outputList;
        }

        public static ActionRecordQOBD[] ActionRecordTypeToArray(this List<ActionRecord> actionRecordList)
        {
            ActionRecordQOBD[] outputArray = actionRecordList.AsParallel().Select(x => new ActionRecordQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                AgentId = Utility.encodeStringToBase64(x.AgentId.ToString()),
                Action = Utility.encodeStringToBase64(x.Action),
                TargetId = Utility.encodeStringToBase64(x.TargetId.ToString()),
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
                actionRecordQCBD.ID = Utility.encodeStringToBase64(actionRecord.ID.ToString());
                actionRecordQCBD.AgentId = Utility.encodeStringToBase64(actionRecord.AgentId.ToString());
                actionRecordQCBD.Action = Utility.encodeStringToBase64(actionRecord.Action);
                actionRecordQCBD.TargetId = Utility.encodeStringToBase64(actionRecord.TargetId.ToString());
                actionRecordQCBD.TargetName = Utility.encodeStringToBase64(actionRecord.TargetName);
                actionRecordQCBD.Date = Utility.encodeStringToBase64(actionRecord.Date.ToString());
                actionRecordQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                roleQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                role_actionQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return role_actionQCBD;
        }


        //====================================================================================
        //===============================[ SecurityAction ]===========================================
        //====================================================================================

        public static List<QOBDCommon.Entities.Action> ArrayTypeToAction(this ActionQOBD[] actionQOBDList)
        {
            List<QOBDCommon.Entities.Action> outputList = new List<QOBDCommon.Entities.Action>();
            if(actionQOBDList != null)
            {
                outputList = actionQOBDList.AsParallel().Select(x => new QOBDCommon.Entities.Action
                {
                    ID = x.ID,
                    Name = Utility.decodeBase64ToString(x.Name),
                    DisplayedName = Utility.decodeBase64ToString(x.DisplayedName),
                    Right = (new PrivilegeQOBD[] { x.Right }.ArrayTypeToPrivilege().Count > 0) ? new PrivilegeQOBD[] { x.Right }.ArrayTypeToPrivilege().First() : new Privilege(),
                }).ToList();
            }           
            
            return outputList;
        }

        public static ActionQOBD[] ActionTypeToArray(this List<QOBDCommon.Entities.Action> actionList)
        {
            ActionQOBD[] outputArray = actionList.AsParallel().Select(x => new ActionQOBD
            {
                ID = x.ID,
                Name = Utility.encodeStringToBase64(x.Name),
                DisplayedName = Utility.encodeStringToBase64(x.DisplayedName),
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
                actionQCBD.DisplayedName = Utility.encodeStringToBase64(action.DisplayedName);
                actionQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                agent_roleQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Role_actionId = Utility.intTryParse(Utility.decodeBase64ToString(x.Role_actionId)),
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
                ID = Utility.encodeStringToBase64(x.ID.ToString()) ,
                Role_actionId = Utility.encodeStringToBase64(x.Role_actionId.ToString()),
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
                privilegeQCBD.ID = Utility.encodeStringToBase64(privilege.ID.ToString());
                privilegeQCBD.Role_actionId = Utility.encodeStringToBase64(privilege.Role_actionId.ToString());
                privilegeQCBD._Write = Utility.encodeStringToBase64((privilege.IsWrite) ? "1" : "0");
                privilegeQCBD._Read = Utility.encodeStringToBase64((privilege.IsRead) ? "1" : "0");
                privilegeQCBD._Delete = Utility.encodeStringToBase64((privilege.IsDelete) ? "1" : "0");
                privilegeQCBD._Update = Utility.encodeStringToBase64((privilege.IsUpdate) ? "1" : "0");
                privilegeQCBD.SendEmail = Utility.encodeStringToBase64((privilege.IsSendMail) ? "1" : "0");
                privilegeQCBD.Date = Utility.encodeStringToBase64(privilege.Date.ToString("yyyy-MM-dd H:mm:ss"));
                privilegeQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return privilegeQCBD;
        }


        //====================================================================================
        //===============================[ Order ]===========================================
        //====================================================================================

        public static List<Order> ArrayTypeToOrder(this OrdersQOBD[] OrderQOBDList)
        {
            List<Order> outputList = OrderQOBDList.AsParallel().Select(x => new Order
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                AgentId = Utility.intTryParse(Utility.decodeBase64ToString(x.AgentId)),
                BillAddress = Utility.intTryParse(Utility.decodeBase64ToString(x.BillAddress)),
                ClientId = Utility.intTryParse(Utility.decodeBase64ToString(x.ClientId)),
                Comment1 = Utility.decodeBase64ToString(x.Comment1),
                Comment2 = Utility.decodeBase64ToString(x.Comment2),
                Comment3 = Utility.decodeBase64ToString(x.Comment3),
                Status = Utility.decodeBase64ToString(x.Status),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
                DeliveryAddress = Utility.intTryParse(Utility.decodeBase64ToString(x.DeliveryAddress)),
                Tax = Convert.ToDouble(x.Tax),
            }).ToList();
            
            return outputList;
        }

        public static OrdersQOBD[] OrderTypeToArray(this List<Order> orderList)
        {
            OrdersQOBD[] outputArray = orderList.AsParallel().Select(x => new OrdersQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                AgentId = Utility.encodeStringToBase64(x.AgentId.ToString()),
                BillAddress = Utility.encodeStringToBase64(x.BillAddress.ToString()),
                ClientId = Utility.encodeStringToBase64(x.ClientId.ToString()),
                Comment1 = Utility.encodeStringToBase64(x.Comment1),
                Comment2 = Utility.encodeStringToBase64(x.Comment2),
                Comment3 = Utility.encodeStringToBase64(x.Comment3),
                Status = Utility.encodeStringToBase64(x.Status),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
                DeliveryAddress = Utility.encodeStringToBase64(x.DeliveryAddress.ToString()),
                Tax = x.Tax,
            }).ToArray();

            return outputArray;
        }

        public static OrderFilterQOBD OrderTypeToFilterArray(this Order order, ESearchOption filterOperator)
        {
            OrderFilterQOBD orderQCBD = new OrderFilterQOBD();
            if (order != null)
            {
                orderQCBD.ID = Utility.encodeStringToBase64(order.ID.ToString());
                orderQCBD.AgentId = Utility.encodeStringToBase64(order.AgentId.ToString());
                orderQCBD.BillAddress = Utility.encodeStringToBase64(order.BillAddress.ToString());
                orderQCBD.ClientId = Utility.encodeStringToBase64(order.ClientId.ToString());
                orderQCBD.Comment1 = Utility.encodeStringToBase64(order.Comment1);
                orderQCBD.Comment2 = Utility.encodeStringToBase64(order.Comment2);
                orderQCBD.Comment3 = Utility.encodeStringToBase64(order.Comment3);
                orderQCBD.Status = Utility.encodeStringToBase64(order.Status);
                orderQCBD.DeliveryAddress = Utility.encodeStringToBase64(order.DeliveryAddress.ToString());
                orderQCBD.Tax = order.Tax;
                orderQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return orderQCBD;
        }

        public static PdfQOBD ParamOrderPdfTypeToArray(this ParamOrderToPdf paramOrderToPdf)
        {
            PdfQOBD outputArray = new PdfQOBD();
            if (!paramOrderToPdf.Equals(null))
            {
                outputArray.BillId = paramOrderToPdf.BillId.ToString();
                outputArray.OrderId = paramOrderToPdf.OrderId.ToString();                
            }
            return outputArray;
        }

        //====================================================================================
        //===============================[ Tax_order ]======================================
        //====================================================================================

        public static List<Tax_order> ArrayTypeToTax_order(this Tax_orderQOBD[] Tax_orderQOBDList)
        {
            List<Tax_order> outputList = Tax_orderQOBDList.AsParallel().Select(x => new Tax_order
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                OrderId = Utility.intTryParse(Utility.decodeBase64ToString(x.OrderId)),
                Date_insert = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date_insert)),
                Target = Utility.decodeBase64ToString(x.Target),
                Tax_value = Convert.ToDouble(x.Tax_value),
                TaxId = Utility.intTryParse(Utility.decodeBase64ToString(x.TaxId)),
            }).ToList();
            
            return outputList;
        }

        public static Tax_orderQOBD[] Tax_orderTypeToArray(this List<Tax_order> Tax_orderList)
        {
            Tax_orderQOBD[] outputArray = Tax_orderList.AsParallel().Select(x => new Tax_orderQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                OrderId = Utility.encodeStringToBase64(x.OrderId.ToString()),
                Date_insert = Utility.encodeStringToBase64(x.Date_insert.ToString("yyy-MM-dd H:mm:ss")),
                Target = Utility.encodeStringToBase64(x.Target),
                Tax_value = x.Tax_value,
                TaxId = Utility.encodeStringToBase64(x.TaxId.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static Tax_orderFilterQOBD Tax_orderTypeToFilterArray(this Tax_order Tax_order, ESearchOption filterOperator)
        {
            Tax_orderFilterQOBD Tax_orderQCBD = new Tax_orderFilterQOBD();
            if (Tax_order != null)
            {
                Tax_orderQCBD.ID = Utility.encodeStringToBase64(Tax_order.ID.ToString());
                Tax_orderQCBD.OrderId = Utility.encodeStringToBase64(Tax_order.OrderId.ToString());
                Tax_orderQCBD.Date_insert = Utility.encodeStringToBase64(Tax_order.Date_insert.ToString("yyy-MM-dd H:mm:ss"));
                Tax_orderQCBD.Target = Utility.encodeStringToBase64(Tax_order.Target);
                Tax_orderQCBD.Tax_value = Tax_order.Tax_value;
                Tax_orderQCBD.TaxId = Utility.encodeStringToBase64(Tax_order.TaxId.ToString());
                Tax_orderQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return Tax_orderQCBD;
        }

        //====================================================================================
        //===============================[ Client ]===========================================
        //====================================================================================

        public static List<Client> ArrayTypeToClient(this ClientQOBD[] ClientQOBD)
        {
            List<Client> outputList = ClientQOBD.AsParallel().Select(x => new Client
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                FirstName = Utility.decodeBase64ToString(x.FirstName),
                LastName = Utility.decodeBase64ToString(x.LastName),
                AgentId = Utility.intTryParse(Utility.decodeBase64ToString(x.AgentId)),
                Comment = Utility.decodeBase64ToString(x.Comment),
                Phone = Utility.decodeBase64ToString(x.Phone),
                Status = Utility.decodeBase64ToString(x.Status),
                Company = Utility.decodeBase64ToString(x.Company),
                Email = Utility.decodeBase64ToString(x.Email),
                Fax = Utility.decodeBase64ToString(x.Fax),
                CompanyName = Utility.decodeBase64ToString(x.CompanyName),
                CRN = Utility.decodeBase64ToString(x.CRN),
                MaxCredit = Utility.decimalTryParse(x.MaxCredit),
                Rib = Utility.decodeBase64ToString(x.Rib),
                PayDelay = Utility.intTryParse(Utility.decodeBase64ToString(x.PayDelay))
            }).ToList();

            return outputList;
        }

        public static ClientQOBD[] ClientTypeToArray(this List<Client> ClientList)
        {
            ClientQOBD[] outputArray = ClientList.AsParallel().Select(x=> new ClientQOBD {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                FirstName = Utility.encodeStringToBase64(x.FirstName),
                LastName = Utility.encodeStringToBase64(x.LastName),
                AgentId = Utility.encodeStringToBase64(x.AgentId.ToString()),
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
                PayDelay = Utility.encodeStringToBase64(x.PayDelay.ToString())
            }).ToArray();
            
            return outputArray;
        }
        public static ClientFilterQOBD ClientTypeToFilterArray(this Client Client, ESearchOption filterOperator)
        {
            ClientFilterQOBD ClientQCBD = new ClientFilterQOBD();
            if (Client != null)
            {
                ClientQCBD.ID = Utility.encodeStringToBase64(Client.ID.ToString());
                ClientQCBD.FirstName = Utility.encodeStringToBase64(Client.FirstName);
                ClientQCBD.LastName = Utility.encodeStringToBase64(Client.LastName);
                ClientQCBD.AgentId = Utility.encodeStringToBase64(Client.AgentId.ToString());
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
                ClientQCBD.PayDelay = Utility.encodeStringToBase64(Client.PayDelay.ToString());
                ClientQCBD.Option = Utility.encodeStringToBase64(Client.Option.ToString());
                ClientQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Cellphone = Utility.decodeBase64ToString(x.Cellphone),
                ClientId = Utility.intTryParse(Utility.decodeBase64ToString(x.ClientId)),
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
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                Position = Utility.encodeStringToBase64(x.Position),
                LastName = Utility.encodeStringToBase64(x.LastName),
                Firstname = Utility.encodeStringToBase64(x.Firstname),
                Comment = Utility.encodeStringToBase64(x.Comment),
                Phone = Utility.encodeStringToBase64(x.Phone),
                ClientId = Utility.encodeStringToBase64(x.ClientId.ToString()),
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
                ContactQCBD.ID = Utility.encodeStringToBase64(Contact.ID.ToString());
                ContactQCBD.Position = Utility.encodeStringToBase64(Contact.Position);
                ContactQCBD.LastName = Utility.encodeStringToBase64(Contact.LastName);
                ContactQCBD.Firstname = Utility.encodeStringToBase64(Contact.Firstname);
                ContactQCBD.Comment = Utility.encodeStringToBase64(Contact.Comment);
                ContactQCBD.Phone = Utility.encodeStringToBase64(Contact.Phone);
                ContactQCBD.ClientId = Utility.encodeStringToBase64(Contact.ClientId.ToString());
                ContactQCBD.Cellphone = Utility.encodeStringToBase64(Contact.Cellphone);
                ContactQCBD.Email = Utility.encodeStringToBase64(Contact.Email);
                ContactQCBD.Fax = Utility.encodeStringToBase64(Contact.Fax);
                ContactQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                AddressName = Utility.decodeBase64ToString(x.Address),
                ClientId = Utility.intTryParse(Utility.decodeBase64ToString(x.ClientId)),
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
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                Address = Utility.encodeStringToBase64(x.AddressName),
                ClientId = Utility.encodeStringToBase64(x.ClientId.ToString()),
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
                AddressQCBD.ID = Utility.encodeStringToBase64(Address.ID.ToString());
                AddressQCBD.Address = Utility.encodeStringToBase64(Address.AddressName);
                AddressQCBD.ClientId = Utility.encodeStringToBase64(Address.ClientId.ToString());
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
                AddressQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                ClientId = Utility.intTryParse(Utility.decodeBase64ToString(x.ClientId)),
                OrderId = Utility.intTryParse(Utility.decodeBase64ToString(x.OrderId)),
                Comment1 = Utility.decodeBase64ToString(x.Comment1),
                Comment2 = Utility.decodeBase64ToString(x.Comment2),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
                DateLimit = Utility.convertToDateTime(Utility.decodeBase64ToString(x.DateLimit)),
                Pay = x.Pay,
                DatePay = Utility.convertToDateTime(Utility.decodeBase64ToString(x.DatePay)),
                PayMod = Utility.decodeBase64ToString(x.PayMod),
                PayReceived = x.PayReceived,
            }).ToList();
            
            return outputList;
        }

        public static BillQOBD[] BillTypeToArray(this List<Bill> BillList)
        {
            BillQOBD[] outputArray = BillList.AsParallel().Select(x => new BillQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                ClientId = Utility.encodeStringToBase64(x.ClientId.ToString()),
                OrderId = Utility.encodeStringToBase64(x.OrderId.ToString()),
                Comment1 = Utility.encodeStringToBase64(x.Comment1),
                Comment2 = Utility.encodeStringToBase64(x.Comment2),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
                DateLimit = Utility.encodeStringToBase64(x.DateLimit.ToString("yyyy-MM-dd H:mm:ss")),
                Pay = x.Pay,
                DatePay = Utility.encodeStringToBase64(x.DatePay.ToString("yyyy-MM-dd H:mm:ss")),
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
                BillQCBD.ID = Utility.encodeStringToBase64(Bill.ID.ToString());
                BillQCBD.ClientId = Utility.encodeStringToBase64(Bill.ClientId.ToString());
                BillQCBD.OrderId = Utility.encodeStringToBase64(Bill.OrderId.ToString());
                BillQCBD.Comment1 = Utility.encodeStringToBase64(Bill.Comment1);
                BillQCBD.Comment2 = Utility.encodeStringToBase64(Bill.Comment2);
                BillQCBD.Date = Utility.encodeStringToBase64(Bill.Date.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.DateLimit = Utility.encodeStringToBase64(Bill.DateLimit.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.Pay = Bill.Pay;
                BillQCBD.DatePay = Utility.encodeStringToBase64(Bill.DatePay.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.PayMod = Utility.encodeStringToBase64(Bill.PayMod);
                BillQCBD.PayReceived = Bill.PayReceived;
                BillQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                BillId = Utility.intTryParse(Utility.decodeBase64ToString(x.BillId)),
                OrderId = Utility.intTryParse(Utility.decodeBase64ToString(x.OrderId)),
                Date = Utility.convertToDateTime(Utility.decodeBase64ToString(x.Date)),
                Package = Utility.intTryParse(Utility.decodeBase64ToString(x.Package)),
                Status = Utility.decodeBase64ToString(x.Status),
            }).ToList();
            
            return outputList;
        }

        public static DeliveryQOBD[] DeliveryTypeToArray(this List<Delivery> DeliveryList)
        {
            DeliveryQOBD[] outputArray = DeliveryList.AsParallel().Select(x => new DeliveryQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                BillId = Utility.encodeStringToBase64(x.BillId.ToString()),
                OrderId = Utility.encodeStringToBase64(x.OrderId.ToString()),
                Date = Utility.encodeStringToBase64(x.Date.ToString("yyyy-MM-dd H:mm:ss")),
                Package = Utility.encodeStringToBase64(x.Package.ToString()),
                Status = Utility.encodeStringToBase64(x.Status),
            }).ToArray();
            
            return outputArray;
        }

        public static DeliveryFilterQOBD DeliveryTypeToFilterArray(this Delivery Delivery, ESearchOption filterOperator)
        {
            DeliveryFilterQOBD DeliveryQCBD = new DeliveryFilterQOBD();
            if (Delivery != null)
            {
                DeliveryQCBD.ID = Utility.encodeStringToBase64(Delivery.ID.ToString());
                DeliveryQCBD.BillId = Utility.encodeStringToBase64(Delivery.BillId.ToString());
                DeliveryQCBD.OrderId = Utility.encodeStringToBase64(Delivery.OrderId.ToString());
                DeliveryQCBD.Date = Utility.encodeStringToBase64(Delivery.Date.ToString("yyyy-MM-dd H:mm:ss"));
                DeliveryQCBD.Package = Utility.encodeStringToBase64(Delivery.Package.ToString());
                DeliveryQCBD.Status = Utility.encodeStringToBase64(Delivery.Status);
                DeliveryQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return DeliveryQCBD;
        }

        //====================================================================================
        //================================[ order_item ]====================================
        //====================================================================================

        public static List<Order_item> ArrayTypeToOrder_item(this Order_itemQOBD[] order_itemQOBDList)
        {
            List<Order_item> outputList = order_itemQOBDList.AsParallel().Select(x => new Order_item
            {
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                OrderId = Utility.intTryParse(Utility.decodeBase64ToString(x.OrderId)),
                ItemId = Utility.intTryParse(Utility.decodeBase64ToString(x.ItemId)),
                Comment_Purchase_Price = Utility.decodeBase64ToString(x.Comment_Purchase_Price),
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Rank = Utility.intTryParse(Utility.decodeBase64ToString(x.Rank)),
                Price = x.Price,
                Price_purchase = x.Price_purchase,
                Quantity = Utility.intTryParse(Utility.decodeBase64ToString(x.Quantity)),
                Quantity_current = Utility.intTryParse(Utility.decodeBase64ToString(x.Quantity_current)),
                Quantity_delivery = Utility.intTryParse(Utility.decodeBase64ToString(x.Quantity_delivery)),
            }).ToList();
            
            return outputList;
        }

        public static Order_itemQOBD[] order_itemTypeToArray(this List<Order_item> Order_itemList)
        {
            Order_itemQOBD[] outputArray = Order_itemList.AsParallel().Select(x => new Order_itemQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                OrderId = Utility.encodeStringToBase64(x.OrderId.ToString()),
                ItemId = Utility.encodeStringToBase64(x.ItemId.ToString()),
                Comment_Purchase_Price = Utility.encodeStringToBase64(x.Comment_Purchase_Price),
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Rank = Utility.encodeStringToBase64(x.Rank.ToString()),
                Price = x.Price,
                Price_purchase = x.Price_purchase,
                Quantity = Utility.encodeStringToBase64(x.Quantity.ToString()),
                Quantity_current = Utility.encodeStringToBase64(x.Quantity_current.ToString()),
                Quantity_delivery = Utility.encodeStringToBase64(x.Quantity_delivery.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static Order_itemFilterQOBD Order_itemTypeToFilterArray(this Order_item order_item, ESearchOption filterOperator)
        {
            Order_itemFilterQOBD order_itemQCBD = new Order_itemFilterQOBD();
            if (order_item != null)
            {
                order_itemQCBD.ID = Utility.encodeStringToBase64(order_item.ID.ToString());
                order_itemQCBD.OrderId = Utility.encodeStringToBase64(order_item.OrderId.ToString());
                order_itemQCBD.ItemId = Utility.encodeStringToBase64(order_item.ItemId.ToString());
                order_itemQCBD.Comment_Purchase_Price = Utility.encodeStringToBase64(order_item.Comment_Purchase_Price);
                order_itemQCBD.Item_ref = Utility.encodeStringToBase64(order_item.Item_ref);
                order_itemQCBD.Rank = Utility.encodeStringToBase64(order_item.Rank.ToString());
                order_itemQCBD.Price = order_item.Price;
                order_itemQCBD.Price_purchase = order_item.Price_purchase;
                order_itemQCBD.Quantity = Utility.encodeStringToBase64(order_item.Quantity.ToString());
                order_itemQCBD.Quantity_current = Utility.encodeStringToBase64(order_item.Quantity_current.ToString());
                order_itemQCBD.Quantity_delivery = Utility.encodeStringToBase64(order_item.Quantity_delivery.ToString());
                order_itemQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Tax_current = Utility.intTryParse(Utility.decodeBase64ToString(x.Tax_current)),
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
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                Tax_current = Utility.encodeStringToBase64(x.Tax_current.ToString()),
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
                TaxQCBD.ID = Utility.encodeStringToBase64(Tax.ID.ToString());
                TaxQCBD.Tax_current = Utility.encodeStringToBase64(Tax.Tax_current.ToString());
                TaxQCBD.Type = Utility.encodeStringToBase64(Tax.Type);
                TaxQCBD.Value = Tax.Value;
                TaxQCBD.Date_insert = Utility.encodeStringToBase64(Tax.Date_insert.ToString("yyyy-MM-dd H:mm:ss"));
                TaxQCBD.Comment = Utility.encodeStringToBase64(Tax.Comment);
                TaxQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Provider_name = Utility.decodeBase64ToString(x.Provider_name),
            }).ToList();
            
            return outputList;
        }

        public static Provider_itemQOBD[] Provider_itemTypeToArray(this List<Provider_item> Provider_itemList)
        {
            Provider_itemQOBD[] outputArray = Provider_itemList.AsParallel().Select(x => new Provider_itemQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
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
                Provider_itemQCBD.ID = Utility.encodeStringToBase64(Provider_item.ID.ToString());
                Provider_itemQCBD.Item_ref = Utility.encodeStringToBase64(Provider_item.Item_ref);
                Provider_itemQCBD.Provider_name = Utility.encodeStringToBase64(Provider_item.Provider_name);
                Provider_itemQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Name = Utility.decodeBase64ToString(x.Name),
                Source = Utility.intTryParse(Utility.decodeBase64ToString(x.Source)),
            }).ToList();
            
            return outputList;
        }

        public static ProviderQOBD[] ProviderTypeToArray(this List<Provider> ProviderList)
        {
            ProviderQOBD[] outputArray = ProviderList.AsParallel().Select(x => new ProviderQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                Name = Utility.encodeStringToBase64(x.Name),
                Source = Utility.encodeStringToBase64(x.Source.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static ProviderFilterQOBD ProviderTypeToFilterArray(this Provider Provider, ESearchOption filterOperator)
        {
            ProviderFilterQOBD ProviderQCBD = new ProviderFilterQOBD();
            if (Provider != null)
            {
                ProviderQCBD.ID = Utility.encodeStringToBase64(Provider.ID.ToString());
                ProviderQCBD.Name = Utility.encodeStringToBase64(Provider.Name);
                ProviderQCBD.Source = Utility.encodeStringToBase64(Provider.Source.ToString());
                ProviderQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Comment = Utility.decodeBase64ToString(x.Comment),
                Erasable = Utility.decodeBase64ToString(x.Erasable),
                Name = Utility.decodeBase64ToString(x.Name),
                Price_purchase = x.Price_purchase,
                Number_of_sale = Utility.intTryParse(Utility.decodeBase64ToString(x.Number_of_sale)),
                Price_sell = x.Price_sell,
                Stock = Utility.intTryParse(Utility.decodeBase64ToString(x.Stock)),
                Ref = Utility.decodeBase64ToString(x.Ref),
                Picture = Utility.decodeBase64ToString(x.Picture),
                Type_sub = Utility.decodeBase64ToString(x.Type_sub),
                Source = Utility.intTryParse(Utility.decodeBase64ToString(x.Source.ToString())),
                Type = Utility.decodeBase64ToString(x.Type),
            }).ToList();
            
            return outputList;
        }

        public static ItemQOBD[] ItemTypeToArray(this List<Item> ItemList)
        {
            ItemQOBD[] outputArray = ItemList.AsParallel().Select(x => new ItemQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                Comment = Utility.encodeStringToBase64(x.Comment),
                Erasable = Utility.encodeStringToBase64(x.Erasable),
                Name = Utility.encodeStringToBase64(x.Name),
                Price_purchase = x.Price_purchase,
                Number_of_sale = Utility.encodeStringToBase64(x.Number_of_sale.ToString()),
                Price_sell = x.Price_sell,
                Stock = Utility.encodeStringToBase64(x.Stock.ToString()),
                Ref = Utility.encodeStringToBase64(x.Ref),
                Picture = Utility.encodeStringToBase64(x.Picture),
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
                ItemQCBD.ID = Utility.encodeStringToBase64(Item.ID.ToString());
                ItemQCBD.Comment = Utility.encodeStringToBase64(Item.Comment);
                ItemQCBD.Erasable = Utility.encodeStringToBase64(Item.Erasable);
                ItemQCBD.Name = Utility.encodeStringToBase64(Item.Name);
                ItemQCBD.Price_purchase = Item.Price_purchase;
                ItemQCBD.Price_sell = Item.Price_sell;
                ItemQCBD.Stock = Utility.encodeStringToBase64(Item.Stock.ToString());
                ItemQCBD.Number_of_sale = Utility.encodeStringToBase64(Item.Number_of_sale.ToString());
                ItemQCBD.Option = Utility.encodeStringToBase64(Item.Option.ToString());
                ItemQCBD.Ref = Utility.encodeStringToBase64(Item.Ref);
                ItemQCBD.Picture = Utility.encodeStringToBase64(Item.Picture);
                ItemQCBD.Type_sub = Utility.encodeStringToBase64(Item.Type_sub);
                ItemQCBD.Source = Utility.encodeStringToBase64(Item.Source.ToString());
                ItemQCBD.Type = Utility.encodeStringToBase64(Item.Type);
                ItemQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                DeliveryId = Utility.intTryParse(Utility.decodeBase64ToString(x.DeliveryId)),
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Quantity_delivery = Utility.intTryParse(Utility.decodeBase64ToString(x.Quantity_delivery)),
            }).ToList();
            
            return outputList;
        }

        public static Item_deliveryQOBD[] Item_deliveryTypeToArray(this List<Item_delivery> Item_deliveryList)
        {
            Item_deliveryQOBD[] outputArray = Item_deliveryList.AsParallel().Select(x => new Item_deliveryQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                DeliveryId = Utility.encodeStringToBase64(x.DeliveryId.ToString()),
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Quantity_delivery = Utility.encodeStringToBase64(x.Quantity_delivery.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static Item_deliveryFilterQOBD Item_deliveryTypeToFilterArray(this Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            Item_deliveryFilterQOBD Item_deliveryQCBD = new Item_deliveryFilterQOBD();
            if (Item_delivery != null)
            {
                Item_deliveryQCBD.ID = Utility.encodeStringToBase64(Item_delivery.ID.ToString());
                Item_deliveryQCBD.DeliveryId = Utility.encodeStringToBase64(Item_delivery.DeliveryId.ToString());
                Item_deliveryQCBD.Item_ref = Utility.encodeStringToBase64(Item_delivery.Item_ref);
                Item_deliveryQCBD.Quantity_delivery = Utility.encodeStringToBase64(Item_delivery.Quantity_delivery.ToString());
                Item_deliveryQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                Item_ref = Utility.decodeBase64ToString(x.Item_ref),
                Tax_type = Utility.decodeBase64ToString(x.Tax_type),
                TaxId = Utility.intTryParse(Utility.decodeBase64ToString(x.TaxId)),
                OrderId = Utility.intTryParse(Utility.decodeBase64ToString(x.OrderId)),
                Tax_value = x.Tax_value,
            }).ToList();
            
            return outputList;
        }

        public static Tax_itemQOBD[] Tax_itemTypeToArray(this List<Tax_item> Tax_itemList)
        {
            Tax_itemQOBD[] outputArray = Tax_itemList.AsParallel().Select(x => new Tax_itemQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                Item_ref = Utility.encodeStringToBase64(x.Item_ref),
                Tax_type = Utility.encodeStringToBase64(x.Tax_type),
                TaxId = Utility.encodeStringToBase64(x.TaxId.ToString()),
                OrderId = Utility.encodeStringToBase64(x.OrderId.ToString()),
                Tax_value = x.Tax_value,
            }).ToArray();
            
            return outputArray;
        }

        public static Tax_itemFilterQOBD Tax_itemTypeToFilterArray(this Tax_item Tax_item, ESearchOption filterOperator)
        {
            Tax_itemFilterQOBD Tax_itemQCBD = new Tax_itemFilterQOBD();
            if (Tax_item != null)
            {
                Tax_itemQCBD.ID = Utility.encodeStringToBase64(Tax_item.ID.ToString());
                Tax_itemQCBD.Item_ref = Utility.encodeStringToBase64(Tax_item.Item_ref);
                Tax_itemQCBD.Tax_type = Utility.encodeStringToBase64(Tax_item.Tax_type);
                Tax_itemQCBD.TaxId = Utility.encodeStringToBase64(Tax_item.TaxId.ToString());
                Tax_itemQCBD.OrderId = Utility.encodeStringToBase64(Tax_item.OrderId.ToString());
                Tax_itemQCBD.Tax_value = Tax_item.Tax_value;
                Tax_itemQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
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
                ID = Utility.intTryParse(Utility.decodeBase64ToString(x.ID)),
                RefId = Utility.intTryParse(Utility.decodeBase64ToString(x.RefId)),
            }).ToList();
            
            return outputList;
        }

        public static Auto_refsQOBD[] Auto_refTypeToArray(this List<Auto_ref> Auto_refList)
        {
            Auto_refsQOBD[] outputArray = Auto_refList.AsParallel().Select(x => new Auto_refsQOBD
            {
                ID = Utility.encodeStringToBase64(x.ID.ToString()),
                RefId = Utility.encodeStringToBase64(x.RefId.ToString()),
            }).ToArray();
            
            return outputArray;
        }

        public static Auto_refsFilterQOBD Auto_refTypeToFilterArray(this Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            Auto_refsFilterQOBD Auto_refQCBD = new Auto_refsFilterQOBD();
            if (Auto_ref != null)
            {
                Auto_refQCBD.ID = Utility.encodeStringToBase64(Auto_ref.ID.ToString());
                Auto_refQCBD.RefId = Utility.encodeStringToBase64(Auto_ref.RefId.ToString());
                Auto_refQCBD.Operator = Utility.encodeStringToBase64(filterOperator.ToString());
            }
            return Auto_refQCBD;
        }
    }


}
