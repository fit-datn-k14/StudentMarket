using StudentMarket.Common.Entities;
using StudentMarket.DL.PostDL;

namespace StudentMarket.BL.PostBL
{
    public class PostBL : BaseBL<Post>, IPostBL
    {
        #region field

        private IPostDL _postDL;

        #endregion

        #region contructor

        public PostBL(IPostDL postDL) : base(postDL)
        {
            _postDL = postDL;
        }

        #endregion

        #region Method

        #endregion
    }
}
