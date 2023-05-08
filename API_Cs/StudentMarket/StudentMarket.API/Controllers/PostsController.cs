using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.API.Entities;
using StudentMarket.BL.PostBL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.DL.PostDL;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        #region Field
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        private IPostBL _postBL;
        #endregion

        #region Contructor

        public PostsController(IPostBL postBL, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _postBL = postBL;
        }
        #endregion


        #region Method


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

        [HttpPost]
        public async Task<ServiceResult> AddPost([FromForm] PostDataModel postData)
        {
            try
            {
                var post = postData.Post;
                var images = postData.Images;
                post.PostID = Guid.NewGuid();
                if (images != null)
                {
                    var imageUrls = await UploadImages(images, post.PostID);
                    post.ImageName = imageUrls[0];
                    post.ListImages = imageUrls.ToList();
                }

                var serviceResult = _postBL.InsertRecord(post);

                if (!serviceResult.Success)
                {
                    DeleteFolder(post.PostID);
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// API chỉnh sửa bản ghi
        /// </summary>
        /// <param name="id">id bản ghi muốn sửa</param>
        /// <param name="record">thông tin mới</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPut("{id}")]
        public async Task<ServiceResult> UpdateRecord([FromRoute] Guid id, [FromForm] PostDataModel postData)
        {
            try
            {
                var post = postData.Post;
                var images = postData.Images;
                var imagesDel = postData.ImagesDel;
                List<Guid> imageUrls = new List<Guid>();
                if (images.Count > 0)
                {
                    imageUrls = await UploadImages(images, post.PostID);
                    if (post.ImageName == null)
                    {
                        post.ImageName = imageUrls[0];
                    }
                    post.ListImages = imageUrls.ToList();
                }
                

                var serviceResult = _postBL.UpdatePostByID(post, imagesDel);
                if (serviceResult.Success)
                {
                    if (imagesDel.Count > 0)
                    {
                        DeleteImages(imagesDel, post.PostID);
                    }
                }
                else
                {
                    if(imageUrls.Count > 0)
                    {
                        DeleteImages(imageUrls, post.PostID);
                    }
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// API chỉnh sửa bản ghi
        /// </summary>
        /// <param name="id">id bản ghi muốn sửa</param>
        /// <param name="record">thông tin mới</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPut("Approved/{id}")]
        public async Task<ServiceResult> SetApproved([FromRoute] Guid id, [FromQuery] Approved approved)
        {
            try
            {
                ServiceResult serviceResult = _postBL.SetApproved(id,approved);

                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// Lưu ảnh vào hệ thống
        /// </summary>
        /// <param name="images">Ảnh cần lưu</param>
        /// <param name="postId">Id bài đăng tương ứng</param>
        /// <returns>Danh sách ảnh</returns>
        /// /// CreatedBy: NVHuy(20/03/2023)
        private async Task<List<Guid>> UploadImages(List<IFormFile> images, Guid postId)
        {
            // Khởi tạo danh sách các đường dẫn ảnh
            var imageUrls = new List<Guid>();

            if (images != null)
            {
                foreach (var image in images)
                {
                    // Tạo tên file mới cho ảnh
                    var imageName = Guid.NewGuid();
                    var folderPath = Path.Combine("images", "posts", postId.ToString());
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    var filePath = Path.Combine(folderPath, imageName + Path.GetExtension(image.FileName));
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                    // Lưu ý rằng ở đây tôi lưu đường dẫn ảnh dưới dạng Guid
                    // Bạn có thể sử dụng một định dạng khác tùy theo yêu cầu của mình
                    imageUrls.Add(imageName);
                }
            }
            // Lặp qua từng file ảnh được tải lên


            // Trả về danh sách các đường dẫn ảnh
            return imageUrls;
        }

        private void DeleteImages(List<Guid> imagesDel, Guid postId)
        {
            foreach (var image in imagesDel)
            {
                string folderPath = Path.Combine("images", "posts", postId.ToString());
                string filePath = Directory.GetFiles(folderPath, image + ".*")[0];
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
        }

        private void DeleteFolder(Guid id)
        {
            var folderPath = Path.Combine("images", "posts", id.ToString());
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }

        /// <summary>
        /// Lọc nhiều cột
        /// </summary>
        /// <returns>Danh sách dữ liệu</returns>
        /// CreatedBy: NVHuy(27/03/2023)
        [HttpPost("Filter")]
        public ServiceResult QueryFilter([FromBody] FilterQueryAdmin filterQuery)
        {
            try
            {
                var serviceResult = _postBL.Filter(filterQuery);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API lấy một bản ghi theo id
        /// </summary>
        /// <param name="id">id muốn lấy thông tin</param>
        /// <returns>Bản ghi tương ứng</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpGet("{id}")]
        public ServiceResult GetRecordByID([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _postBL.GetRecordByID(id);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API xoá nhiều bản ghi theo danh sách ids
        /// </summary>
        /// <param name="ids">danh sách ids</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(27/03/2023)
        [HttpDelete]
        public ServiceResult DeleteMultiRecord(List<Guid> ids)
        {
            try
            {
                var serviceResult = _postBL.DeleteMultiRecordByID(ids);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách bài đăng theo id của người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Danh sách bài đăng của người dùng</returns>
        /// CreatedBy: NVHuy(27/03/2023)
        [HttpGet("GetByUser/{id}")]
        public ServiceResult GetListPostsByUser(Guid id)
        {
            try
            {
                return _postBL.GetListPostsByUser(id);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        #endregion
    }
}
