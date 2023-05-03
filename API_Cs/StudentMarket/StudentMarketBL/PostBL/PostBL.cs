using StudentMarket.BL.UserBL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.DL.CategoryDL;
using StudentMarket.DL.LocationDL;
using StudentMarket.DL.PostDL;
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
                    sortString = $" ORDER BY CreatedDate DESC ";
                    break;
                default:
                    sortString = $" ORDER BY CreatedDate DESC ";
                    break;
            }

            stringQuery.Append(sortString);
            return stringQuery.ToString();
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
