﻿using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.MessageDL
{
    public interface IMessageDL : IBaseDL<Message>
    {
        /// <summary>
        /// Lấy danh sách Users
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>Danh sách User có tin nhắn</returns>
        public List<Guid> GetListIDByUserID(Guid UserID);

        /// <summary>
        /// Lấy danh sách Users
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>danh sách Users</returns>
        public ServiceResult GetUsersByID(Guid userID);

        /// <summary>
        /// Lấy danh sách tin nhắn
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="withUser"></param>
        /// <returns>danh sách tin nhắn</returns>
        public ServiceResult GetListMessages(Guid UserID, Guid withUser);

        /// <summary>
        /// Set trạng thái đã xem
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public ServiceResult SeenMessage(Guid id, Guid withId);
    }
}
