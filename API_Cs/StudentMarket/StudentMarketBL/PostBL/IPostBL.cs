using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.PostBL
{
    public interface IPostBL : IBaseBL<Post>
    {
        /// <summary>
        /// Lấy toàn bộ bản ghi trong bảng
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult SearchPosts(FilterQuery filterQuery);

        /// <summary>
        /// Lấy toàn bộ bản ghi trong bảng
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult GetListPostsByUser(Guid userId);

        /// <summary>
        /// Phê duyệt
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult SetApproved(Guid postId, ApprovedModel approvedModel);

        /// <summary>
        /// Chỉnh sửa tin đăng
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="imagesDel"></param>
        /// <returns></returns>
        public ServiceResult UpdatePostByID(Post post, List<Guid> imagesDel);

        /// <summary>
        /// Thêm tin đăng yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult AddFavouritePost(FavouritePost favouritePost);

        /// <summary>
        /// Loại bỏ tin đăng yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult RemoveFavouritePost(FavouritePost favouritePost);

        /// <summary>
        /// Lấy số lượt yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult getNumberFavourite(Guid postId);

        /// <summary>
        /// Lấy danh sách tin đăng yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        public ServiceResult GetListFavouritePost(Guid userId, FilterQuery filterQuery);

        /// <summary>
        /// Lấy danh sách tin đăng yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        public ServiceResult GetListFavouritePostID(Guid userId);
    }
}
