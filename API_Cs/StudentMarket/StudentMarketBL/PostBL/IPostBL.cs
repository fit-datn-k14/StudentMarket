using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.PostBL
{
    public interface IPostBL : IBaseBL<Post>
    {
        public ServiceResult SearchPosts(FilterQuery filterQuery);
    }
}
