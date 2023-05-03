using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.TCDN.Common.Entities.DTO
{
    /// <summary>
    /// Điều kiện lọc filter
    /// </summary>
    /// CreatedBy: NVHuy (29/03/2023)
    public class FilterCondition
    {
        #region Property

        /// <summary>
        /// Thuộc tính lọc
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Phép toán lọc
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Giá trị lọc
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}
