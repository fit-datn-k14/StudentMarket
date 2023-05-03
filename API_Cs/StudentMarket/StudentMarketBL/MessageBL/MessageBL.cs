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
            try
            {

                var listUserIDs = _messageDL.GetUsersByID(UserID);
                var listUsers = new List<User>();
                if (listUserIDs != null && listUserIDs.Count > 0)
                {
                    listUsers = _messageDL.GetListUsers(listUserIDs);
                }
                return new ServiceResult
                {
                    Success = true,
                    Data = listUsers
                };
            }
            catch (Exception ex) {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách tin nhắn
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="withUser"></param>
        /// <returns>danh sách tin nhắn</returns>
        public ServiceResult GetListMessages(Guid UserID, Guid withUser)
        {
            return new ServiceResult();
        }

        #endregion
    }
}
