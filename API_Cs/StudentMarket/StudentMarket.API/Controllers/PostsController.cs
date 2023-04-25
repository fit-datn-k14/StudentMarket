using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.PostBL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostsController : BaseController<Post>
    {
        #region Field
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IPostBL _postBL;
        #endregion
        
        #region Contructor

        public PostsController(IPostBL postBL, IWebHostEnvironment hostingEnvironment) : base(postBL)
        {
            _hostingEnvironment = hostingEnvironment;
            _postBL = postBL;
        } 
        #endregion


        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns>Thông tin người dùng</returns>
        [HttpPost("Search")]
        public ServiceResult Search([FromBody] FilterQuery filterQuery)
        {
            try
            {
                return _postBL.SearchPosts(filterQuery);
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }

        [HttpGet("{folderName}/{imageName}")]
        public IActionResult GetImage(string folderName, string imageName)
        {
            var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", folderName, imageName);
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            var mimeType = "image/jpeg"; // hoặc "image/png" tùy vào định dạng của ảnh

            return File(imageBytes, mimeType);
        }

        #endregion
    }
}
