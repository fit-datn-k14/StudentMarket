using StudentMarket.Common.Entities;
using StudentMarket.DL.CommentDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.CommentBL
{
    public class CommentBL : BaseBL<Comment>, ICommentBL
    {
        #region field

        private ICommentDL _CommentDL;

        #endregion

        #region contructor

        public CommentBL(ICommentDL CommentDL) : base(CommentDL)
        {
            _CommentDL = CommentDL;
        }

        #endregion

        #region Method

        #endregion
    }
}
