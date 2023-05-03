using Misa.Web01.TCDN.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.TCDN.Common.Entities.DTO
{
    /// <summary>
    /// Điều kiện sắp xếp bản ghi
    /// </summary>
    /// CreatedBy: NVHuy(02/04/2023)
    public class SortCondition
    {
        #region Property

        /// <summary>
        /// Cột sắp xếp
        /// </summary>
        public string? Field { get; set; }

        /// <summary>
        /// Sắp xếp tăng / giảm
        /// </summary>
        public OptionSort? Option { get; set; }

        #endregion
    }
}
