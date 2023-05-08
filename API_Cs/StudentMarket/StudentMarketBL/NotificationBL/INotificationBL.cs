using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.NotificationBL
{
    public interface INotificationBL : IBaseBL<Notification>
    {
        /// <summary>
        /// Lấy danh sách thông báo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách thông báo</returns>
        public ServiceResult GetListByUser(Guid userId);

        /// <summary>
        /// Set trạng thái đã xem
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public ServiceResult SeenNotify(Guid notificationId);
    }
}
