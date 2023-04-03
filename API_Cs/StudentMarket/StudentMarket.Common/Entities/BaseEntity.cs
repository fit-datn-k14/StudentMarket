namespace StudentMarket.Common.Entities
{
    /// <summary>
    /// Thuộc tính chung
    /// </summary>
    /// CreatedBy: Huy(25/03/2023)
    public class BaseEntity
    {
        #region Properties

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        #endregion
    }
}
