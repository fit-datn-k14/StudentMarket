using StudentMarket.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Entities.DTO
{
    public class ServiceResult
    {
        #region Property

        /// <summary>
        /// Thành công hay không
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mã kết quả
        /// </summary>
        public ErrorCodes? ErrorCode { get; set; }

        /// <summary>
        /// Thông báo cho user. Không bắt buộc, tùy theo đặc thù từng dịch vụ và client tích hợp
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Thông báo cho Dev. Thông báo ngắn gọn để thông báo cho Dev biết vấn đề đang gặp phải
        /// </summary>
        public object? DevMsg { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object? Data { get; set; }

        public ServiceResult() { }

        /// <summary>
        /// Khởi tạo kết quả trả về ngoại lệ
        /// </summary>
        /// <param name="errorCodes"></param>
        /// <param name="devMsg"></param>
        public ServiceResult(ErrorCodes errorCodes, object? devMsg)
        {
            if (errorCodes == ErrorCodes.Exception)
            {
                Success = false;
                ErrorCode = ErrorCodes.Exception;
                UserMsg = Resource.UsrMsg_Exception;
                DevMsg = devMsg;
            }
        }

        #endregion
    }
}
