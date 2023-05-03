using Misa.Web01.TCDN.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities.DTO
{
    /// <summary>
    /// Input cho API lọc nhiều cột
    /// </summary>
    public class FilterQueryAdmin
    {
        #region Property

        /// <summary>
        /// Danh sách điều kiện
        /// </summary>
        public List<FilterCondition>? filterConditions { get; set; }

        /// <summary>
        /// Thông tin sắp xếp  
        /// </summary>
        public SortCondition? sortCondition { get; set; }

        /// <summary>
        /// Từ khoá tìm kiếm theo Mã, Họ Tên, Số điện thoại
        /// </summary>
        public string? keyword { get; set; }

        /// <summary>
        /// Số trang
        /// </summary>
        public int pageNumber { get; set; } = 1;

        /// <summary>
        /// Số bản ghi trên 1 trang
        /// </summary>
        public int pageSize { get; set; } = 20;

        #endregion
    }
}
