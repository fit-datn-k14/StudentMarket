using StudentMarket.Common.Attributes;
using StudentMarket.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMarket.Common.Entities
{
    /// <summary>
    /// Người dùng
    /// </summary>
    /// CreatedBy: Huy(24/03/2023)
    [Table("Users")]
    public class User : BaseEntity
    {
        #region Properties

        [Key]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không thể bỏ trống")]
        [DuplicateCode(ErrorMessage = "Tên tài khoản đã tồn tại")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string Password { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? IdentityNumber { get; set; }

        public DateTime? IdentityDate { get; set; }
            
        public string? IdentityPlace { get; set; }

        public string? Avatar { get; set; }

        public Role? Role { get; set; }

        public Guid? LocationID { get; set; } 

        #endregion
    }
}
