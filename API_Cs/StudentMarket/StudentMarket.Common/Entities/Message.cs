using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities
{
    /// <summary>
    /// Tin nhắn
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    [Table("Messages")]
    public class Message : BaseEntity
    {
        #region Properties

        [Key]
        public Guid MessageID { get; set; }

        [Required(ErrorMessage = "Người gửi không đuược bỏ trống")]
        public Guid FromUser { get; set; }

        [Required(ErrorMessage = "Người nhận không đuược bỏ trống")]
        public Guid ToUser { get; set; }

        public string Content { get; set; }

        #endregion
    }
}
