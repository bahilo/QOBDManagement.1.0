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
        TcpClient clientSocket;
        IDiscussionViewModel DiscussionViewModel;

        public HandleDiscussion(IStartup startup)
        {
            this.Startup = startup;
        }

        public void startClient(IDiscussionViewModel discussionViewModel, TcpClient inClientSocket)
        {
            this.clientSocket = inClientSocket;
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

        /*internal void startClient(IDiscussionViewModel discussionViewModel, TcpClient clientSocket, string messageHeader, string username, Dictionary<TcpClient, string> clientsList)
        {
            msgHeader = messageHeader;
            this.startClient(clientSocket, username, clientsList);
        }*/

        private async void doChat()
        {
            if (clientSocket.Connected)
            {
                byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
                string dataFromClient = null;

                while ((true))
                {
                    int discussionId = 0;
                    int userId = 0;
                    int messageId = 0;
                    List<string> composer = new List<string>();
                    NetworkStream networkStream = default(NetworkStream);

                    try
                    {
                        networkStream = clientSocket.GetStream();
                        networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                        if (Utility.intTryParse(dataFromClient.Split('/')[0]) == (int)EServiceCommunication.Disconnected)
                            throw new ApplicationException("Exit application.");

                        composer = dataFromClient.Split('/').ToList();

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

                        //DiscussionViewModel.broadcast(dataFromClient);
                    }
                    catch (ApplicationException)
                    {
                        var agentFound = DiscussionViewModel.ServerClientsDictionary.Where(x => x.Key.TxtID == dataFromClient.Split('/')[1]).Select(x => x.Key).SingleOrDefault();
                        if (agentFound != null)//(clientsList.Keys.Contains(clientSocket))
                        {
                            //Debug.WriteLine("User(id = " + agentFound.TxtID + ")" + " has exited!");

                            string message = (int)EServiceCommunication.Disconnected + "/" + agentFound.TxtID + "/0/" + agentFound.TxtID + "|" + "$";
                            DiscussionViewModel.broadcast(dataFromClient);
                            //DiscussionViewModel.ServerClientsDictionary.Remove(agentFound);
                            //networkStream.Dispose();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.CHATROOM);
                    }
                }//end while
            }// end if

        }//end doChat
    }
}
