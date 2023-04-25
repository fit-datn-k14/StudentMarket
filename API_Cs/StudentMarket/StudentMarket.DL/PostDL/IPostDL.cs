using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.PostDL
{
    public interface IPostDL : IBaseDL<Post>
    {
        public ServiceResult SearchPosts(string keyword, string condition, int fromRecord, int pageSize);
    }
}
