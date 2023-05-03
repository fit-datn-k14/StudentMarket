using StudentMarket.Common.Entities;

namespace StudentMarket.API.Entities
{
    public class PostDataModel
    {
        public Post Post { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
