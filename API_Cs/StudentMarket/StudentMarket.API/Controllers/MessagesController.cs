
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.MessageBL;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        #region Field
        private IMessageBL _messageBL;
        #endregion

        #region Contructor

        public MessagesController(IMessageBL messageBL)
        {
            _messageBL = messageBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy danh sách user nhắn tin
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet("{UserID}")]
        public ServiceResult GetListUserHaveMess([FromRoute] Guid UserID)
        {
            try
            {
                var serviceResult = _messageBL.GetUsersByID(UserID);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API lấy danh sách user nhắn tin
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet("Mess/{UserID}")]
        public ServiceResult Getlist([FromRoute] Guid UserID, [FromBody] Guid withUser)
        {
            try
            {
                var serviceResult = _messageBL.GetListMessages(UserID,withUser);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        #endregion
    }
}
