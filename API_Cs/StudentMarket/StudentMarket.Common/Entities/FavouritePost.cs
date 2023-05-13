using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities
{
    public class FavouritePost
    {
        public Guid? UserID { get; set; }

        public Guid? PostID { get; set; }
    }
}
