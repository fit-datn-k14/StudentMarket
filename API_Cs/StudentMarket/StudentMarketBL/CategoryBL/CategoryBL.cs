using StudentMarket.Common.Entities;
using StudentMarket.DL.CategoryDL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL.CategoryBL
{
    public class CategoryBL : BaseBL<Category>, ICategoryBL 
    {
        #region field

        private ICategoryDL _categoryDL;

        #endregion

        #region contructor

        public CategoryBL(ICategoryDL categoryDL) : base(categoryDL)
        {
            _categoryDL = categoryDL;
        }

        #endregion

        #region Method

        #endregion
    }
}
