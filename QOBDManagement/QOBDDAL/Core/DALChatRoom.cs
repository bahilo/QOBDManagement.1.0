using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDCommon.Interfaces.DAC;
using QOBDGateway.Classes;
using QOBDGateway.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDGateway.Interfaces;

namespace QOBDDAL.Core
{
    public class DALChatRoom: IChatRoomManager
    {
        public Agent AuthenticatedUser { get; set; }
        private QOBDCommon.Interfaces.REMOTE.IChatRoomManager _gateWayChatRoom;
        private ClientProxy _servicePortType;
        private Interfaces.IQOBDSet _dataSet;
        private ICommunication _serviceCommunication;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALChatRoom(ClientProxy servicePort)
        {
            _servicePortType = servicePort;
            _gateWayChatRoom = new GateWayChatRoom(_servicePortType);
        }

        public DALChatRoom(ClientProxy servicePort, Interfaces.IQOBDSet dataSet): this(servicePort)
        {
            _dataSet = dataSet;
        }

        public DALChatRoom(ClientProxy servicePort, Interfaces.IQOBDSet _dataSet, ICommunication serviceCommunication) : this(servicePort, _dataSet)
        {
            _serviceCommunication = serviceCommunication;
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
        }

        public void setServiceCredential(object channel)
        {
            _servicePortType = (ClientProxy)channel;
            if (AuthenticatedUser != null && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.UserName) && string.IsNullOrEmpty(_servicePortType.ClientCredentials.UserName.Password))
            {
                _servicePortType.ClientCredentials.UserName.UserName = AuthenticatedUser.UserName;
                _servicePortType.ClientCredentials.UserName.Password = AuthenticatedUser.HashedPassword;
            }
            _gateWayChatRoom.setServiceCredential(_servicePortType);
        }

        private void checkServiceCommunication()
        {
            if (_servicePortType.State == System.ServiceModel.CommunicationState.Closed || _servicePortType.State == System.ServiceModel.CommunicationState.Faulted)
                _serviceCommunication.resetCommunication();
        }

        #region [ Actions ]
        //=================================[ Actions ]================================================

        #region [ Discussion ]
        public async Task<List<Discussion>> InsertDiscussionAsync(List<Discussion> listDiscussion)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.InsertDiscussionAsync(listDiscussion);
        }

        public async Task<List<Discussion>> DeleteDiscussionAsync(List<Discussion> listDiscussion)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.DeleteDiscussionAsync(listDiscussion);
        }

        public async Task<List<Discussion>> UpdateDiscussionAsync(List<Discussion> listDiscussion)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.UpdateDiscussionAsync(listDiscussion);
        }

        public async Task<List<Discussion>> GetDiscussionDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetDiscussionDataAsync(nbLine);
        }

        public async Task<List<Discussion>> GetDiscussionDataByUser_discussionListAsync(List<User_discussion> user_discussionList)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetDiscussionDataByUser_discussionListAsync(user_discussionList);
        }

        public async Task<List<Discussion>> GetDiscussionDataByIdAsync(int id)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetDiscussionDataByIdAsync(id);
        }

        public async Task<List<Discussion>> searchDiscussionAsync(Discussion Discussion, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.searchDiscussionAsync(Discussion, filterOperator);
        }

        public async Task<List<Discussion>> GetDiscussionDataByMessageListAsync(List<Message> messageList)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetDiscussionDataByMessageListAsync(messageList);
        }

        #endregion

        #region [ Message ]

        public async Task<List<Message>> InsertMessageAsync(List<Message> listMessage)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.InsertMessageAsync(listMessage);
        }

        public async Task<List<Message>> DeleteMessageAsync(List<Message> listMessage)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.DeleteMessageAsync(listMessage);
        }

        public async Task<List<Message>> UpdateMessageAsync(List<Message> listMessage)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.UpdateMessageAsync(listMessage);
        }

        public async Task<List<Message>> GetMessageDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetMessageDataAsync(nbLine);
        }

        public async Task<List<Message>> GetMessageDataByIdAsync(int id)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetMessageDataByIdAsync(id);
        }

        public async Task<List<Message>> searchMessageAsync(Message Message, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.searchMessageAsync(Message, filterOperator);
        }



        #endregion

        #region [ User_discussion ]
        public async Task<List<User_discussion>> InsertUser_discussionAsync(List<User_discussion> listUser_discussion)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.InsertUser_discussionAsync(listUser_discussion);
        }

        public async Task<List<User_discussion>> DeleteUser_discussionAsync(List<User_discussion> listUser_discussion)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.DeleteUser_discussionAsync(listUser_discussion);
        }

        public async Task<List<User_discussion>> UpdateUser_discussionAsync(List<User_discussion> listUser_discussion)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.UpdateUser_discussionAsync(listUser_discussion);
        }

        public async Task<List<User_discussion>> GetUser_discussionDataAsync(int nbLine)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetUser_discussionDataAsync(nbLine);
        }

        public async Task<List<User_discussion>> GetUser_discussionDataByIdAsync(int id)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.GetUser_discussionDataByIdAsync(id);
        }

        public async Task<List<User_discussion>> searchUser_discussionAsync(User_discussion User_discussion, ESearchOption filterOperator)
        {
            checkServiceCommunication();
            return await _gateWayChatRoom.searchUser_discussionAsync(User_discussion, filterOperator);
        }
        #endregion

        #endregion

        public void Dispose()
        {
            if (_gateWayChatRoom != null)
                _gateWayChatRoom.Dispose();
        }
    }
}
