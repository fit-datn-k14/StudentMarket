using StudentMarket.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities.DTO
{
    public class ApprovedModel
    {
        public Guid UserID { get; set; }
        public Approved Approved { get; set; }
    }
}
