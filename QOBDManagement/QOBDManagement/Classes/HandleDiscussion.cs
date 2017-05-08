using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Enums;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QOBDManagement.Classes
{
    public class HandleDiscussion : BindBase
    {
        string message;
        UdpClient clientSocket;
        IDiscussionViewModel DiscussionViewModel;

        public HandleDiscussion(IStartup startup)
        {
            this.Startup = startup;
            message = "";
        }

        public void startClient(IDiscussionViewModel discussionViewModel, UdpClient inClientSocket, string message)
        {
            this.clientSocket = inClientSocket;
            this.message = message;
            DiscussionViewModel = discussionViewModel;
            Thread ctThread = new Thread(doChat);
            ctThread.IsBackground = true;
            ctThread.Start();
        }

        public QOBDBusiness.BusinessLogic BL
        {
            get { return _startup.Bl; }
        }

        public Agent AuthenticatedUser
        {
            get { return BL.BlSecurity.GetAuthenticatedUser(); }
        }

        private async void doChat()
        {

            int discussionId = 0;
            int userId = 0;
            int messageId = 0;
            List<string> composer = new List<string>();

            composer = message.Split('/').ToList();

            // current discussion management
            if (composer.Count > 3
                && int.TryParse(composer[0], out discussionId)
                    && int.TryParse(composer[1], out userId)
                        && int.TryParse(composer[2], out messageId)
                            && discussionId != (int)EServiceCommunication.Disconnected
                                && discussionId != (int)EServiceCommunication.Connected)
            {
                var message = new Message { ID = messageId, DiscussionId = discussionId, UserId = userId, Content = composer[4], Date = Utility.convertToDateTime(composer[5]) };
                var userFoundList = BL.BlAgent.searchAgent(new Agent { ID = userId }, QOBDCommon.Enum.ESearchOption.AND);

                if (discussionId == DiscussionViewModel.DiscussionModel.Discussion.ID)
                {
                    // new user to the current discussion detected  
                    List<string> discussionUserIDs = composer[3].Split('|').Where(x => !string.IsNullOrEmpty(x)).ToList();
                    if (discussionUserIDs.Count() > DiscussionViewModel.DiscussionModel.UserList.Count)
                    {
                        DiscussionViewModel.updateDiscussionUserList(DiscussionViewModel.DiscussionModel, discussionUserIDs);
                        DiscussionViewModel.displayMessage(message, userFoundList[0]);
                    }

                    // display the new incoming message
                    else if (DiscussionViewModel.DiscussionModel.MessageList.Where(x => x.Message.ID == messageId).Count() == 0)
                    {
                        if (message.ID > 0 && userFoundList.Count > 0)
                        {
                            DiscussionViewModel.DiscussionModel.addMessage(new MessageModel { Message = message });
                            DiscussionViewModel.displayMessage(message, userFoundList[0]);
                        }
                    }
                }

                // notification of a new incoming message
                else
                {
                    if (message.ID > 0)
                    {
                        var discussionModelFound = DiscussionViewModel.DiscussionList.Where(x => x.Discussion.ID == discussionId).FirstOrDefault();
                        if (discussionModelFound != null)
                        {
                            discussionModelFound.addMessage(new MessageModel { Message = message, IsNewMessage = true });
                            DiscussionViewModel.TxtNbNewMessage = (Utility.intTryParse(DiscussionViewModel.TxtNbNewMessage) + 1).ToString();
                        }
                        else
                            await DiscussionViewModel.retrieveUserDiscussions(AuthenticatedUser);

                        System.Media.SystemSounds.Asterisk.Play();
                    }
                }
            }

        }//end doChat
    }
}
