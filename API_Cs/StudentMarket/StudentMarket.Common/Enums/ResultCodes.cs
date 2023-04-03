namespace StudentMarket.Common.Enums
{
    /// <summary>
    /// Enum mã lỗi
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public enum ResultCodes
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 0,

        /// <summary>
        /// Lỗi do validate dữ liệu thất bại
        /// </summary>
        Validate = 1,

        /// <summary>
        /// Lỗi do trùng mã
        /// </summary>
        /// </summary>
        DuplicateCode = 2,

        /// <summary>
        /// Không tìm thấy bản ghi
        /// </summary>
        NotFoundRecord = 3,

        /// <summary>
        /// Lỗi do exception chưa xác định được
        /// </summary>
        Exception = 4,

    }
}
