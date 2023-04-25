using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.DL.PostDL;
using System.Text;

namespace StudentMarket.BL.PostBL
{
    public class PostBL : BaseBL<Post>, IPostBL
    {
        #region field

        private IPostDL _postDL;

        #endregion

        #region contructor

        public PostBL(IPostDL postDL) : base(postDL)
        {
            _postDL = postDL;
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
    }
}
