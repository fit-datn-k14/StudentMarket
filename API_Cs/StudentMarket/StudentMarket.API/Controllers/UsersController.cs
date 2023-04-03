using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.UserBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : BasesController<User>
    {
        #region Field
        private IUserBL _userBL;
        #endregion

        public UsersController(IUserBL userBL) : base(userBL)
        {
            _userBL = userBL;
        }
    }
}
