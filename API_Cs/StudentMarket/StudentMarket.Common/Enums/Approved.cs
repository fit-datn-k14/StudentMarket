using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Enums
{
    public enum Approved
    {
        /// <summary>
        /// Chưa duyệt
        /// </summary>
        UnApproved = 0,

        /// <summary>
        /// Đã duyệt
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Từ chối
        /// </summary>
        Refuse = 2,
    }
}
