
using Microsoft.AspNetCore.Mvc;
using StudentMarket.API.Hubs;
using StudentMarket.BL.NotificationBL;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using System.Threading.Tasks;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        #region Field
        private readonly NotificationHub _hubContext;
        private INotificationBL _notificationBL;
        #endregion

        #region Contructor

        public NotificationsController(INotificationBL notificationBL, NotificationHub hubContext)
        {
            _notificationBL = notificationBL;
            _hubContext = hubContext;
        }

        #endregion

        #region Method

        /// <summary>
        /// API thêm một thông báo
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPost]
        public async Task<ServiceResult> InsertRecord([FromBody] Notification record)
        {
            try
            {
                var serviceResult = _notificationBL.InsertRecord(record);
                if (serviceResult.Success)
                {
                    Notification result = (Notification)serviceResult.Data;
                    if (result != null)
                    {
                        // Gửi thông báo tới các người dùng cùng thuộc một group với record
                        await _hubContext.SendNotification(result);
                    }
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ServiceResult GetListByUser(Guid id)
        {
            try
            {
                return _notificationBL.GetListByUser(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ServiceResult SeenNotify(Guid id)
        {
            try
            {
                return _notificationBL.SeenNotify(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        #endregion
    }
}
