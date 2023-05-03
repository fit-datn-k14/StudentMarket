using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.TCDN.Common.Entities.DTO
{
    /// <summary>
    /// Input cho API xuất Excel
    /// </summary>
    /// CreatedBy: NVHuy (01/04/2023)
    public class ExportExcelModel
    {
        #region Property

        /// <summary>
        /// Danh sách điều kiện lọc
        /// </summary>
        public List<FilterCondition>? Conditions { get; set; }

        /// <summary>
        /// Các thông tin cần thiết để tuỳ chỉnh cột
        /// </summary>
        public List<FormatColumn> FormatColumns { get; set; }

        #endregion
    }
}
