using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.CategoryBL;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : BasesController<Category>
    {
        #region Field
        private ICategoryBL _categoryBL;
        #endregion

        public CategoriesController(ICategoryBL categoryBL) : base(categoryBL)
        {
            _categoryBL = categoryBL;
        }
    }
}
