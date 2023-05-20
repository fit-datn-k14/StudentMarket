using StudentMarket.BL.UserBL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.DL.CategoryDL;
using StudentMarket.DL.LocationDL;
using StudentMarket.DL.PostDL;
using StudentMarket.DL.UserDL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentMarket.BL.PostBL
{
    public class PostBL : BaseBL<Post>, IPostBL
    {
        #region field

        private IPostDL _postDL;
        private ILocationDL _locationDL;
        private IUserBL _userBL;
        private ICategoryDL _categoryDL;

        #endregion

        #region contructor

        public PostBL(IPostDL postDL, ICategoryDL categoryDL, ILocationDL locationDL, IUserBL userBL) : base(postDL)
        {
            _postDL = postDL;
            _categoryDL = categoryDL;
            _locationDL = locationDL;
            _userBL = userBL;
        }

        #endregion

        #region Method

        public ServiceResult SearchPosts(FilterQuery filterQuery)
        {
            string conditionString = BuildStringQuery(filterQuery);
            int fromRecord = (filterQuery.PageNumber - 1) * filterQuery.PageSize;
            return _postDL.SearchPosts(filterQuery.Keyword, conditionString, fromRecord, filterQuery.PageSize);      
        }

        /// <summary>
        /// Tạo chuỗi query từ thông tin filter
        /// </summary>
        /// <param name="filterQuery"></param>
        /// <returns>chuỗi query</returns>
        /// CreatedBy: NVHuy(02/04/2023)
        public string BuildStringQuery(FilterQuery filterQuery)
        {
            var categoryId = filterQuery.CategoryID;
            var locationId = filterQuery.LocationID;
            var sortType = filterQuery.SortType;
            var keyword = filterQuery.Keyword;
            var stringQuery = new StringBuilder(" ");
            if (categoryId != null)
            {
                stringQuery.Append($" AND CategoryID = '{categoryId}'");
            }
            if (locationId != null)
            {
                stringQuery.Append($" AND LocationID = '{locationId}'");
            }

            string sortString="";
            switch (sortType)
            {
                case 1:
                    sortString = $" ORDER BY Price DESC ";
                    break;
                case 2:
                    sortString = $" ORDER BY Price ";
                    break;
                default:
                    sortString = $" ORDER BY CreatedDate DESC ";
                    break;
            }

            stringQuery.Append(sortString);
            return stringQuery.ToString();
        }

        /// <summary>
        /// Lấy toàn bộ bản ghi trong bảng
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult GetListPostsByUser(Guid userId)
        {
            return _postDL.GetListPostsByUser(userId);
        }

        /// <summary>
        /// Phê duyệt
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult SetApproved(Guid postId, ApprovedModel approvedModel)
        {
            return _postDL.SetApproved(postId, approvedModel);
        }

        /// <summary>
        /// Chỉnh sửa tin đăng
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="imagesDel"></param>
        /// <returns></returns>
        public ServiceResult UpdatePostByID(Post post, List<Guid> imagesDel)
        {
            var validateFailures = ValidateRequestData(post);
            var validateFailuresCustom = ValidateRequestDataCustom(post);
            validateFailures = validateFailures.Concat(validateFailuresCustom).ToList();

            if (validateFailures.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Validate,
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }
            return _postDL.UpdatePostByID(post, imagesDel);
        }

        /// <summary>
        /// Thêm tin đăng yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult AddFavouritePost(FavouritePost favouritePost)
        {
            var validateFailures = new List<string>();
            if(favouritePost != null && favouritePost.UserID == null)
            {
                validateFailures.Add("Người dùng không được bỏ trống");
            }else if(favouritePost != null && favouritePost.PostID == null)
            {
                validateFailures.Add("Tin đăng không được bỏ trống");
            }
            if (validateFailures.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Validate,
                    UserMsg = validateFailures[0].ToString(),
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }
            else
            {
                return _postDL.AddFavouritePost(favouritePost);
            }
        }

        /// <summary>
        /// Loại bỏ tin đăng yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult RemoveFavouritePost(FavouritePost favouritePost)
        {
            return _postDL.RemoveFavouritePost(favouritePost);
        }
        /// <summary>
        /// Lấy số lượt yêu thích
        /// </summary>
        /// <param name="favouritePost"></param>
        /// <returns>Thông báo</returns>
        public ServiceResult getNumberFavourite(Guid postId)
        {
            return _postDL.getNumberFavourite(postId);
        }

        /// <summary>
        /// Lấy danh sách tin đăng yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        public ServiceResult GetListFavouritePost(Guid userId, FilterQuery filterQuery)
        {
            string conditionString = BuildStringQuery(filterQuery);
            return _postDL.GetListFavouritePost(userId, filterQuery.Keyword, conditionString);
        }

        /// <summary>
        /// Lấy danh sách tin đăng yêu thích
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách PostID</returns>
        public ServiceResult GetListFavouritePostID(Guid userId)
        {
            return _postDL.GetListFavouritePostID(userId);
        }

        #endregion

        #region Override

        /// <summary>
        /// Validate kiểm tra các điều kiện khác
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public override List<string> ValidateRequestDataCustom(Post record)
        {
            List<string> validateRequestDataCustom = new List<string>();
            
            if (record != null && record.CategoryID != null)
            {
                var categoryID = record?.CategoryID;
                ServiceResult serviceResult = _categoryDL.GetRecordByID((Guid)categoryID);
                if (!serviceResult.Success)
                {
                    validateRequestDataCustom.Add("Danh mục không tồn tại");
                }
            }
            if (record != null && record.LocationID != null)
            {
                var locationID = record?.LocationID;
                ServiceResult serviceResult = _locationDL.GetRecordByID((Guid)locationID);
                if (!serviceResult.Success)
                {
                    validateRequestDataCustom.Add("Khu vực không tồn tại");
                }
            }
            if (record != null && record.UserID != null)
            {
                var userID = record?.UserID;
                ServiceResult serviceResult = _userBL.GetRecordByID((Guid)userID);
                if (!serviceResult.Success)
                {
                    validateRequestDataCustom.Add("Người dùng không tồn tại");
                }
            }

            record.PostCode = _postDL.GetNewCode().Data.ToString();

            var name = "Admin";

            if (record != null && record.CreatedBy == null)
            {
                record.CreatedBy = name;
            }
            DateTime now = DateTime.Now;
            record.ModifiedBy = record.CreatedBy;
            record.CreatedDate = now;
            record.ModifiedDate = now;
            return validateRequestDataCustom;
        }

        #endregion
    }
}
