using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.PostBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostsController : BaseController<Post>
    {
        #region Field
        private IPostBL _postBL;
        #endregion

        public PostsController(IPostBL postBL) : base(postBL)
        {
            _postBL = postBL;
        }
    }
}
