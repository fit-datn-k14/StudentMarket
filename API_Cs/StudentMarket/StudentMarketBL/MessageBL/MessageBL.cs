using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.DL.MessageDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.MessageBL
{
    public class MessageBL : BaseBL<Message>, IMessageBL
    {
        #region field

        private IMessageDL _messageDL;

        #endregion

        #region contructor

        public MessageBL(IMessageDL messageDL) : base(messageDL) 
        {
            _messageDL = messageDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách Users
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>Danh sách User có tin nhắn</returns>
        public ServiceResult GetUsersByID(Guid UserID)
        {
            return _messageDL.GetUsersByID(UserID);
        }

        /// <summary>
        /// Lấy danh sách tin nhắn
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="withUser"></param>
        /// <returns>danh sách tin nhắn</returns>
        public ServiceResult GetListMessages(MessageDataModel messageData)
        {
            var userId = messageData.UserID;
            var withUser = messageData.WithUser;
            return _messageDL.GetListMessages(userId, withUser);
        }

        #endregion
    }
}
