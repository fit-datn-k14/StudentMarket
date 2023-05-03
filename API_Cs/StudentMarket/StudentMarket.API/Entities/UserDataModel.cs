using StudentMarket.Common.Entities;

namespace StudentMarket.API.Entities
{
    public class UserDataModel
    {
        public User User { get; set; }
        public IFormFile? Image { get; set; }
    }
}
