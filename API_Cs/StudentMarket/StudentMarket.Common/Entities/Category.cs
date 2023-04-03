using StudentMarket.Common.Attributes;
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
    /// Danh mục
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    [Table("Categories")]
    public class Category : BaseEntity
    {
        #region Properties

        [Key]
        public Guid CategoryID { get; set; }

        [Required(ErrorMessage = "Mã danh mục không được bỏ trống")]
        [DuplicateCode(ErrorMessage = "Mã danh mục đã tồn tại")]
        public string CategoryCode { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được bỏ trống")]
        [DuplicateCode(ErrorMessage = "Tên danh mục đã tồn tại")]
        public string CategoryName { get; set; }

        #endregion
    }
}
