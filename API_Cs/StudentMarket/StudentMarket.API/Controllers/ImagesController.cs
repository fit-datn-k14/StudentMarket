using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.PostBL;

namespace StudentMarket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        #region Field
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        #endregion

        #region Contructor

        public ImagesController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        [HttpGet("{tableName}/{folderName}/{imageName}")]
        public IActionResult GetImage(string tableName, string folderName, string imageName)
        {
            try
            {
                var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "images", tableName, folderName);
                var imageFiles = Directory.GetFiles(imagePath, imageName + ".*"); // lấy tất cả các tập tin có tên giống với imageName
                if (imageFiles.Length == 0)
                {
                    throw new FileNotFoundException(); // nếu không tìm thấy tập tin ảnh thì ném lỗi
                }
                var imageBytes = System.IO.File.ReadAllBytes(imageFiles[0]); // lấy tập tin ảnh đầu tiên
                var mimeType = "";
                if (Path.GetExtension(imageFiles[0]).ToLower() == ".jpg" || Path.GetExtension(imageFiles[0]).ToLower() == ".jpeg")
                {
                    mimeType = "image/jpeg";
                }
                else if (Path.GetExtension(imageFiles[0]).ToLower() == ".png")
                {
                    mimeType = "image/png";
                }
                return File(imageBytes, mimeType);
            }
            catch
            {
                // Xử lý lỗi ở đây
                // Ví dụ, trả về ảnh mặc định
                var defaultImagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "images", tableName, "default.jpg");
                var defaultImageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
                var defaultMimeType = "image/jpg";
                return File(defaultImageBytes, defaultMimeType);
            }
        }

        //public async Task<object> UploadImages(IFormFileCollection images, string folderName, Guid parentID)
        //{
        //    // Check if any images were uploaded
        //    if (images == null || images.Count == 0)
        //    {
        //        throw new Exception("No images uploaded");
        //    }

        //    // Create directory if it does not exist
        //    if (!Directory.Exists(_uploadPath))
        //    {
        //        Directory.CreateDirectory(_uploadPath);
        //    }

        //    var fileNames = new List<string>();
        //    var filePaths = new List<string>();
        //    var idImages = new List<Guid>();
        //    // Loop through all uploaded images
        //    foreach (var image in images)
        //    {
        //        var id = Guid.NewGuid();
        //        idImages.Add(id);
        //        // Generate new unique file name
        //        string fileName = id.ToString();

        //        // Combine the path and file name
        //        string filePath = Path.Combine(_uploadPath, folderName,parentID.ToString(), fileName);

        //        // Open file stream and copy image to it
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await image.CopyToAsync(stream);
        //        }

        //        fileNames.Add(fileName);
        //        filePaths.Add(filePath);
        //    }

        //    // Return the file names and paths
        //    return idImages;
        //}
    }
}
