
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentMarket.API.Hubs;
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
        private readonly ChatHub _hubContext;
        private IMessageBL _messageBL;
        #endregion

        #region Contructor

        public MessagesController(IMessageBL messageBL, ChatHub chatHub)
        {
            _messageBL = messageBL;
            _hubContext = chatHub;
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
        [HttpPost("ListMess")]
        public ServiceResult Getlist([FromBody] MessageDataModel messageData)
        {
            try
            {
                var serviceResult = _messageBL.GetListMessages(messageData);    
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ServiceResult> InsertRecord([FromBody] Message record)
        {
            try
            {
                var serviceResult = _messageBL.InsertRecord(record);
                if (serviceResult.Success)
                {
                    Message result = (Message)serviceResult.Data;
                    if (result != null)
                    {
                        // Gửi tin nhắn tới các người dùng cùng thuộc một group với record
                        await _hubContext.SendMessage(result);
                    }
                }
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        [HttpPut("{id}/{withId}")]
        public async Task<ServiceResult> SeenMessage(Guid id,Guid withId)
        {
            try
            {
                var serviceResult = _messageBL.SeenMessage(id, withId);
                if (serviceResult.Success)
                {
                    Message result = (Message)serviceResult.Data;
                    if (result != null)
                    {
                        // Gửi tin nhắn tới các người dùng cùng thuộc một group với record
                        await _hubContext.SendMessage(result);
                    }
                }
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
