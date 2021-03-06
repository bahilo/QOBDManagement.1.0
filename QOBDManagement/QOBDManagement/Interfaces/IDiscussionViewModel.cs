﻿using QOBDCommon.Entities;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IDiscussionViewModel
    {
        string TxtNbNewMessage { get; set; }
        IChatRoomViewModel MainChatRoom { get; }
        DiscussionModel DiscussionModel { get; set; }
        List<AgentModel> ChatAgentModelList { get; set; }
        string ByeMessage { get; }
        string WelcomeMessage { get; }
        List<DiscussionModel> DiscussionList { get; set; }
        Dictionary<AgentModel, Tuple<Guid, UdpClient>> ServerClientsDictionary { get; set; }

        // void broadcastMessage(string message);
        void broadcast(string msg);
        void displayMessage(Message message, Agent user);
        Task<List<DiscussionModel>> retrieveUserDiscussions(Agent user);
        List<Agent> updateDiscussionUserList(DiscussionModel discussionModel, List<string> userIDList);
    }
}
