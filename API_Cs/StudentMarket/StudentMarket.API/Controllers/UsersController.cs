using Microsoft.AspNetCore.Http;    
using Microsoft.AspNetCore.Mvc;
using StudentMarket.API.Entities;
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
        /// API đăng ký
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public ServiceResult Register([FromBody] User newAccount)
        {
            try
            {
                return _userBL.InsertRecord(newAccount);
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

        [HttpPut]
        public async Task<ServiceResult> ChangeUserInfo([FromForm] UserDataModel userData)
        {
            try
            {
                var user = userData.User;
                var image = userData.Image;
                Guid avatar = new Guid();
                if (image != null)
                {
                    avatar = await UploadImage(image, user.UserID);
                    user.Avatar = avatar;
                }

                var serviceResult = _userBL.UpdateRecordByID(user,user.UserID);

                if (!serviceResult.Success && image != null)
                {
                    DeleteAvatar(avatar);
                }

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

        private void DeleteAvatar(Guid id)
        {
            string imageName = id.ToString();
            string folderPath = Path.Combine("images", "users", "avatar");
            string filePath = Directory.GetFiles(folderPath, imageName + ".*")[0];
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
            }
        }


        private async Task<Guid> UploadImage(IFormFile image, Guid userId)
        {
            // Khởi tạo danh sách các đường dẫn ảnh
            var avatar = Guid.NewGuid();

            var folderPath = Path.Combine("images", "users", "avatar");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, avatar + Path.GetExtension(image.FileName));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return avatar;
        }

        #endregion
    }
}
