using StudentMarket.BL.NotificationBL;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.DL.CategoryDL;
using StudentMarket.DL.NotificationDL;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.NotificationBL
{
    public class NotificationBL : BaseBL<Notification>, INotificationBL
    {
        #region field

        private INotificationDL _notificationDL;

        #endregion

        #region contructor

        public NotificationBL(INotificationDL notificationDL) : base(notificationDL)
        {
            _notificationDL = notificationDL; 
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy danh sách thông báo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách thông báo</returns>
        public ServiceResult GetListByUser(Guid userId)
        {
            return _notificationDL.GetListByUser(userId);
        }

        /// <summary>
        /// Set trạng thái đã xem
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public ServiceResult SeenNotify(Guid notificationId)
        {
            return _notificationDL.SeenNotify(notificationId);
        }

        #endregion
    }
}
