using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.TCDN.Common.Entities.DTO
{
    /// <summary>
    /// Thông tin cột để xuất Excel
    /// </summary>
    /// CreatedBy: NVHuy (31/03/2023)
    public class FormatColumn
    {
        #region Property

        /// <summary>
        /// Trường dữ liệu của cột 
        /// </summary>
        public string FieldData { get; set; }

        /// <summary>
        /// Tiêu đề cột
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Kiểu dữ liệu cột
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}
