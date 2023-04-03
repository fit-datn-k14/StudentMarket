using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.Common.Attributes
{
    public class DuplicateCodeAttribute : ValidationAttribute
    {
        #region Property

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Hàm khởi tạo không tham số
        /// </summary>
        public DuplicateCodeAttribute() { }

        /// <summary>
        /// Hàm khởi tạo có tham số
        /// </summary>
        /// <param name="errorMessage"></param>
        public DuplicateCodeAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        #endregion

        #region Override

        /// <summary>
        /// Ghi đè hàm IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            // only check string length if empty strings are not allowed
            return true;
        }

        #endregion
    }
}
