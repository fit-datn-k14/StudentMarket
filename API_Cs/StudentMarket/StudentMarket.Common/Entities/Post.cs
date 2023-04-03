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

        [Required(ErrorMessage = "Mã bài đăng không được bỏ trống")]
        [DuplicateCode(ErrorMessage = "Mã bài đăng đã tồn tại")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được bỏ trống")]
        public string Title { get; set; }

        public string Describe { get; set; }

        public Approved Approved { get; set; } = Approved.Approved;

        [Required(ErrorMessage = "Người đăng bài không được bỏ trống")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm không được bỏ trống")]
        public Guid CategoryID { get; set; }

        #endregion
    }
}
