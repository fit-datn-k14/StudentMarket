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
    /// Bình luận
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        #region Properties

        [Key]
        public Guid CommentID { get; set; }

        public string? Content { get; set; }

        public string? Rate { get; set; }

        [Required(ErrorMessage = "Người dùng không được bỏ trống")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "Bài đăng không được bỏ trống")]
        public Guid PostID { get; set; }

        public Guid? Avatar { get; set; }

        public string? FullName { get; set; }

        #endregion
    }
}
