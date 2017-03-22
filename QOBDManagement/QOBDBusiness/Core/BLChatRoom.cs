using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QOBDBusiness.Core
{
    public class BLChatRoom : QOBDCommon.Interfaces.BL.IChatRoomManager
    {
        public QOBDCommon.Interfaces.DAC.IDataAccessManager DAC { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BLChatRoom(QOBDCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public void initializeCredential(Agent user)
        {
            if (user != null)
                DAC.DALChatRoom.initializeCredential(user);
        }


        public void setServiceCredential(object channel)
        {
            DAC.DALChatRoom.setServiceCredential(channel);
        }


        #region [ Discussion ]
        public async Task<List<Discussion>> InsertDiscussionAsync(List<Discussion> discussionList)
        {
            if (discussionList == null || discussionList.Count == 0)
                return new List<Discussion>();

            List<Discussion> result = new List<Discussion>();
            try
            {
                result = await DAC.DALChatRoom.InsertDiscussionAsync(discussionList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }

        public async Task<List<Discussion>> UpdateDiscussionAsync(List<Discussion> discussionList)
        {
            List<Discussion> result = new List<Discussion>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(discussionList.Where(x => x.ID == 0).Count()))
                discussionList = discussionList.Where(x => x.ID != 0).ToList();

            if (discussionList == null || discussionList.Count == 0)
                return new List<Discussion>();

            try
            {
                result = await DAC.DALChatRoom.UpdateDiscussionAsync(discussionList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }

        public async Task<List<Discussion>> DeleteDiscussionAsync(List<Discussion> discussionList)
        {
            List<Discussion> result = new List<Discussion>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(discussionList.Where(x => x.ID == 0).Count()))
                discussionList = discussionList.Where(x => x.ID != 0).ToList();

            if (discussionList == null || discussionList.Count == 0)
                return new List<Discussion>();

            try
            {
                result = await DAC.DALChatRoom.DeleteDiscussionAsync(discussionList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }

        public async Task<List<Discussion>> GetDiscussionDataAsync(int nbLine)
        {
            List<Discussion> result = new List<Discussion>();
            try
            {
                result = await DAC.DALChatRoom.GetDiscussionDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<Discussion>> GetDiscussionDataByIdAsync(int id)
        {
            List<Discussion> result = new List<Discussion>();

            if (id == 0)
                return result;

            try
            {
                result = await DAC.DALChatRoom.GetDiscussionDataByIdAsync(id);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }

        public async Task<List<Discussion>> GetDiscussionDataByMessageListAsync(List<Message> messageList)
        {
            List<Discussion> result = new List<Discussion>();
            try
            {
                result = await DAC.DALChatRoom.GetDiscussionDataByMessageListAsync(messageList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }

        public async Task<List<Discussion>> searchDiscussionAsync(Discussion discussion, ESearchOption filterOperator)
        {
            List<Discussion> result = new List<Discussion>();
            try
            {
                result = await DAC.DALChatRoom.searchDiscussionAsync(discussion, filterOperator);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }

        public async Task<List<Discussion>> GetDiscussionDataByUser_discussionListAsync(List<User_discussion> user_discussionList)
        {
            List<Discussion> result = new List<Discussion>();
            try
            {
                result = await DAC.DALChatRoom.GetDiscussionDataByUser_discussionListAsync(user_discussionList);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.CHATROOM);
            }
            return result;
        }
        #endregion

        #region [ Message ]

        public async Task<List<Message>> InsertMessageAsync(List<Message> messageList)
        {
            if (messageList == null || messageList.Count == 0)
                return new List<Message>();

            List<Message> result = new List<Message>();
            try
            {
                result = await DAC.DALChatRoom.InsertMessageAsync(messageList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<Message>> UpdateMessageAsync(List<Message> messageList)
        {
            List<Message> result = new List<Message>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(messageList.Where(x => x.ID == 0).Count()))
                messageList = messageList.Where(x => x.ID != 0).ToList();

            if (messageList == null || messageList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALChatRoom.UpdateMessageAsync(messageList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<Message>> DeleteMessageAsync(List<Message> messageList)
        {
            List<Message> result = new List<Message>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(messageList.Where(x => x.ID == 0).Count()))
                messageList = messageList.Where(x => x.ID != 0).ToList();

            if (messageList == null || messageList.Count == 0)
                return result;
            
            try
            {
                result = await DAC.DALChatRoom.DeleteMessageAsync(messageList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<Message>> GetMessageDataAsync(int nbLine)
        {
            List<Message> result = new List<Message>();
            try
            {
                result = await DAC.DALChatRoom.GetMessageDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<Message>> GetMessageDataByIdAsync(int id)
        {
            List<Message> result = new List<Message>();
            try
            {
                result = await DAC.DALChatRoom.GetMessageDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<Message>> searchMessageAsync(Message message, ESearchOption filterOperator)
        {
            List<Message> result = new List<Message>();
            try
            {
                result = await DAC.DALChatRoom.searchMessageAsync(message, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }
        #endregion

        #region [ User_discussion ]

        public async Task<List<User_discussion>> InsertUser_discussionAsync(List<User_discussion> user_discussionList)
        {
            if (user_discussionList == null || user_discussionList.Count == 0)
                return new List<User_discussion>();

            List<User_discussion> result = new List<User_discussion>();
            try
            {
                result = await DAC.DALChatRoom.InsertUser_discussionAsync(user_discussionList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<User_discussion>> UpdateUser_discussionAsync(List<User_discussion> user_discussionList)
        {
            List<User_discussion> result = new List<User_discussion>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(user_discussionList.Where(x => x.ID == 0).Count()))
                user_discussionList = user_discussionList.Where(x => x.ID != 0).ToList();

            if (user_discussionList == null || user_discussionList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALChatRoom.UpdateUser_discussionAsync(user_discussionList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<User_discussion>> DeleteUser_discussionAsync(List<User_discussion> user_discussionList)
        {
            List<User_discussion> result = new List<User_discussion>();
            if (checkIfUpdateOrDeleteParamRepectsRequirements(user_discussionList.Where(x => x.ID == 0).Count()))
                user_discussionList = user_discussionList.Where(x => x.ID != 0).ToList();

            if (user_discussionList == null || user_discussionList.Count == 0)
                return result;

            try
            {
                result = await DAC.DALChatRoom.DeleteUser_discussionAsync(user_discussionList);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<User_discussion>> GetUser_discussionDataAsync(int nbLine)
        {
            List<User_discussion> result = new List<User_discussion>();
            try
            {
                result = await DAC.DALChatRoom.GetUser_discussionDataAsync(nbLine);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<User_discussion>> GetUser_discussionDataByIdAsync(int id)
        {
            List<User_discussion> result = new List<User_discussion>();
            try
            {
                result = await DAC.DALChatRoom.GetUser_discussionDataByIdAsync(id);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }

        public async Task<List<User_discussion>> searchUser_discussionAsync(User_discussion user_discussion, ESearchOption filterOperator)
        {
            List<User_discussion> result = new List<User_discussion>();
            try
            {
                result = await DAC.DALChatRoom.searchUser_discussionAsync(user_discussion, filterOperator);
            }
            catch (Exception ex) { Log.error(ex.Message, EErrorFrom.CHATROOM); }
            return result;
        }
        #endregion


        public void Dispose()
        {
            DAC.DALChatRoom.Dispose();
        }

        private bool checkIfUpdateOrDeleteParamRepectsRequirements(int IDValues, [CallerMemberName] string functionName = null)
        {
            bool isRequirementsRespected = true;
            if (IDValues > 0)
            {
                isRequirementsRespected = false;
                Log.warning(functionName + " params (count = " + IDValues + ") with ID = 0", EErrorFrom.CHATROOM);
            }
            return isRequirementsRespected;
        }
    }
}
