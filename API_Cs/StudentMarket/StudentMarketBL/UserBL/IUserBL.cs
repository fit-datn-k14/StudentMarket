using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.UserBL
{
    public interface IUserBL : IBaseBL<User>
    {
        #region Method

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public ServiceResult Login(AccountInfo loginInfo);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public ServiceResult ChangePassword(Guid id, AccountInfo account);

        #endregion
    }
}
