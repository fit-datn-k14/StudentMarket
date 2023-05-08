using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities.DTO
{
    public class MessageDataModel
    {
        public Guid UserID { get; set; }
        public Guid WithUser { get; set; }
    }
}
