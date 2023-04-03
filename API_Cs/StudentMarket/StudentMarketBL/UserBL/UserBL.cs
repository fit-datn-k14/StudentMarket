using StudentMarket.Common.Entities;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        #endregion
    }
}
