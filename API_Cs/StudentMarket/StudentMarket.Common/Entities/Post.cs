using StudentMarket.Common.Attributes;
using StudentMarket.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities
{
    /// <summary>
    /// Bài đăng (sản phẩm)
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    [Table("Posts")]
    public class Post : BaseEntity
    {
        #region Properties

        [Key]
        public Guid PostID { get; set; }

        [DuplicateCode(ErrorMessage = "Mã bài đăng đã tồn tại")]
        public string? PostCode { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được bỏ trống")]
        public string Title { get; set; }

        public string PostDescribe { get; set; }

        public Approved Approved { get; set; } = Approved.UnApproved;

        [Required(ErrorMessage = "Người đăng bài không được bỏ trống")]
        public Guid UserID { get; set; }
        public string? FullName { get; set; }

        public Guid? Avatar { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm không được bỏ trống")]
        public Guid CategoryID { get; set; }
        public string? CategoryName { get; set; }

        public Guid? LocationID { get; set; }

        public string? LocationName { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }

        public Guid? ImageName { get; set; }

        public List<Guid> ListImages { get; set; } = new List<Guid>();

        public List<Guid> ListImagesDel { get; set; } = new List<Guid>();

        #endregion
    }
}
