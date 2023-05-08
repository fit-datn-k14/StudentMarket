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
        public ServiceResult SetApproved(Guid postId, Approved approved);

        /// <summary>
        /// Chỉnh sửa tin đăng
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="imagesDel"></param>
        /// <returns></returns>
        public ServiceResult UpdatePostByID(Post post, List<Guid> imagesDel);
    }
}
