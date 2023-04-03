using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.UserDL
{
    public interface IUserDL : IBaseDL<User>
    {
        #region Method

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult Login(string userName, string password);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public ServiceResult ChangePassword(Guid id, string newPassword);

        /// <summary>
        /// Kiểm tra tên đăng nhập tồn tại
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>có/không</returns>
        public bool GetByUserName(string userName);

        #endregion
    }
}
