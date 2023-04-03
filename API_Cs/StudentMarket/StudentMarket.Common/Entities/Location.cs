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
    /// Khu vực
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    [Table("Locations")]
    public class Location : BaseEntity
    {
        #region Properties

        [Key]
        public Guid LocationID { get; set; }

        [Required]
        public string LocationName { get; set; }

        #endregion
    }
}
