namespace StudentMarket.Common.Entities.DTO
{
    /// <summary>
    /// Kiểu kết quả trả về của API
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public class Response<T>
    {
        #region Property

        /// <summary>
        /// Mã kết quả trả về
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public T Data { get; set; }

        #endregion

        #region Method

        public static Response<T> GetResult(int code, string message, T data = default)
        {
            return new Response<T>
            {
                Code = code,
                Message = message,
                Data = data
            };
        }

        #endregion
    }
}
