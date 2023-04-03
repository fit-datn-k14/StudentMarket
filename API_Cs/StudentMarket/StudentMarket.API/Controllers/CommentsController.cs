using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.CommentBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentsController : BasesController<Comment>
    {
        #region Field
        private ICommentBL _commentBL;
        #endregion

        public CommentsController(ICommentBL commentBL) : base(commentBL)
        {
            _commentBL = commentBL;
        }
    }
}
