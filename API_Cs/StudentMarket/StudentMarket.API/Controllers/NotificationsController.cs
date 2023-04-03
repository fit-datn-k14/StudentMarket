using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.NotificationBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController : BaseController<Notification>
    {
        #region Field
        private INotificationBL _notificationBL;
        #endregion

        public NotificationsController(INotificationBL notificationBL) : base(notificationBL)
        {
            _notificationBL = notificationBL;
        }
    }
}
