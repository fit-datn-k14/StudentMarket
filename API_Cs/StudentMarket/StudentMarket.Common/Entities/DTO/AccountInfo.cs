using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities.DTO
{
    public class AccountInfo
    {
        #region Property

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? NewPassword { get; set; }

        #endregion
    }
}
