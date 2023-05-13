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
    /// Thông báo
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    [Table("Notifications")]
    public class Notification : BaseEntity
    {
        #region Properties

        [Key]
        public Guid NotificationID { get; set; }

        [Required(ErrorMessage = "Người gửi không đuược bỏ trống")]
        public Guid FromUser { get; set; }

        public string? FullName { get; set; }

        public Guid? Avatar { get; set; }

        [Required(ErrorMessage = "Người nhận không đuược bỏ trống")]
        public Guid ToUser { get; set; }

        [Required(ErrorMessage = "Nội dung không được bỏ trống")]
        public string Content { get; set; } = string.Empty;

        public Seen Seen { get; set; } = Seen.Unread;

        public Guid? PostID { get; set; }

        #endregion
    }
}
