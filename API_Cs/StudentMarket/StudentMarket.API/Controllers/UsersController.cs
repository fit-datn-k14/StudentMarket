using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.UserBL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        #region Field
        private IUserBL _userBL;
        #endregion

        #region Contructor

        public UsersController(IUserBL userBL) : base(userBL)
        {
            _userBL = userBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API Đăng nhập
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns>Thông tin người dùng</returns>
        [HttpPost("Login")]
        public ServiceResult Login([FromBody] AccountInfo loginInfo)
        {
            try
            {
                return _userBL.Login(loginInfo); 
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPut("ChangePassword/{id}")]
        public ServiceResult UpdateRecord([FromRoute] Guid id, [FromBody] AccountInfo account)
        {
            try
            {
                var serviceResult = _userBL.ChangePassword(id,account);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }

        #endregion
    }
}
