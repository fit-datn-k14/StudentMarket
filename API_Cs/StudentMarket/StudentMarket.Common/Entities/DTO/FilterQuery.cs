using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities.DTO
{
    /// <summary>
    /// Kiểu dữ liệu trả về cho API phân trang
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public class FilterQuery
    {
        #region Property

        /// <summary>
        /// Id danh mục
        /// </summary>
        public Guid? CategoryID { get; set; }

        /// <summary>
        /// Id khu vực
        /// </summary>
        public Guid? LocationID { get; set; }

        /// <summary>
        /// Thông tin sắp xếp  
        /// </summary>
        public int SortType { get; set; } = 0;

        /// <summary>
        /// Từ khoá tìm kiếm theo Mã, Họ Tên, Số điện thoại
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Số trang
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Số bản ghi trên 1 trang
        /// </summary>  
        public int PageSize { get; set; } = 20;

        #endregion
    }
}
