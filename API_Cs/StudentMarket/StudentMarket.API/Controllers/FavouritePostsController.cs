using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.API.Hubs;
using StudentMarket.BL.NotificationBL;
using StudentMarket.BL.PostBL;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FavouritePostsController : ControllerBase
    {
        #region Field
        private IPostBL _postBL;
        #endregion

        #region Contructor

        public FavouritePostsController(IPostBL postBL)
        {
            _postBL = postBL;
        }
        #endregion

        /// <summary>
        /// Thêm tin đăng yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        [HttpPost]
        public ServiceResult AddFavouritePost([FromBody] FavouritePost favouritePost)
        {
            try
            {
                return _postBL.AddFavouritePost(favouritePost);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Loại bỏ tin đăng yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        [HttpDelete]
        public ServiceResult RemoveFavouritePost([FromQuery] Guid userId, [FromQuery] Guid postId)
        {
            try
            {
                FavouritePost favouritePost = new FavouritePost
                {
                    UserID = userId,
                    PostID = postId
                };
                return _postBL.RemoveFavouritePost(favouritePost);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách tin đăng yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        [HttpPost("{userId}")]
        public ServiceResult GetListFavouritePost([FromRoute] Guid userId, [FromBody] FilterQuery filterQuery)
        {
            try
            {
                return _postBL.GetListFavouritePost(userId, filterQuery);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách tin đăng yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        [HttpGet("GetListIds/{userId}")]
        public ServiceResult GetListFavouritePostID([FromRoute] Guid userId)
        {
            try
            {
                return _postBL.GetListFavouritePostID(userId);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Lấy số lượt yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        [HttpGet("{postId}")]
        public ServiceResult GetNumberFavourite([FromRoute] Guid postId)
        {
            try
            {
                return _postBL.getNumberFavourite(postId);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }
    }
}
