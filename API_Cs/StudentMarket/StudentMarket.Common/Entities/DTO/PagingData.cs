namespace StudentMarket.Common.Entities.DTO
{
    /// <summary>
    /// Kiểu dữ liệu trả về cho API phân trang
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public class PagingData<T>
    {
         #region Property

        /// <summary>
        /// Danh sách thỏa mãn điều kiện lọc và phân trang 
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();


        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        public long TotalRecords { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get; set; }

        #endregion
    }
}
