using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.UserBL
{
    public class UserBL : BaseBL<User>, IUserBL
    {
        #region field

        private IUserDL _userDL;

        #endregion

        #region contructor

        public UserBL(IUserDL userDL) : base(userDL)
        {
            _userDL = userDL;
        }

        #endregion

        #region Method

        public ServiceResult Login(AccountInfo loginInfo)
        {
            if (string.IsNullOrEmpty(loginInfo.UserName) || string.IsNullOrEmpty(loginInfo.Password))
            {
                return new ServiceResult
                {
                    Success = false,
                    UserMsg = Resource.UsrMsg_LoginInfoEmpty,
                };
            }
            string username = loginInfo.UserName;
            string password = EncodeToSHA256(loginInfo.Password);
            Console.WriteLine(password);
            if (!_userDL.GetByUserName(username))
            {
                return new ServiceResult
                {
                    Success = false,
                    UserMsg = Resource.UsrMsg_NotFoundUserName,
                };
            }

            return _userDL.Login(username, password);
        }


        public ServiceResult ChangePassword(Guid id, AccountInfo account)
        {
            if (string.IsNullOrEmpty(account.Password) || string.IsNullOrEmpty(account.NewPassword))
            {
                return new ServiceResult
                {
                    Success = false,
                    UserMsg = Resource.UsrMsg_PasswordEmpty,
                };
            }
            var oldPassword = EncodeToSHA256(account.Password);
            var newPassword = EncodeToSHA256(account.NewPassword);
            var serviceResult = _userDL.GetRecordByID(id);
            if(serviceResult.Success)
            {
                var user = (User)serviceResult.Data;
                if(user.Password == oldPassword)
                {
                    return _userDL.ChangePassword(id, account.NewPassword);
                }
                else
                {
                    return new ServiceResult
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.NotFoundRecord,
                        UserMsg = Resource.UsrMsg_IncorrectPassword
                    };  
                }
            }
            else
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.NotFoundRecord,
                    UserMsg = Resource.UsrMsg_NotFoundRecord
                };
            }
        }

        public string EncodeToSHA256(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    sb.Append(hashedBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        #endregion
    }
}
