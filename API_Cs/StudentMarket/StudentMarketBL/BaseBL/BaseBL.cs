using StudentMarket.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Enums;
using StudentMarket.Common.Entities.DTO;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using StudentMarket.Common;
using StudentMarket.Common.Attributes;
using System.Reflection;

namespace StudentMarket.BL
{
    /// <summary>
    /// Class xử lý nghiệp vụ base
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region contructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bản ghi tương ứng</returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult GetRecordByID(Guid id)
        {
            return _baseDL.GetRecordByID(id);
        }

        /// <summary>
        /// Thêm 1 bản ghi
        /// </summary>
        /// <param name="record">Dữ liệu mới</param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult InsertRecord(T record)
        {
            var validateFailures = ValidateRequestData(record);
            var validateFailuresDuplicate = ValidateFailuresDuplicate(record);
            var validateFailuresCustom = ValidateRequestDataCustom(record);
            validateFailures = validateFailures.Concat(validateFailuresCustom).ToList();

            if (validateFailures.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ResultCode = ResultCodes.Validate,
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }

            if (validateFailuresDuplicate.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ResultCode = ResultCodes.DuplicateCode,
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }

            return _baseDL.InsertRecord(record);
        }

        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name="entity">Dữ liệu mới</param>
        /// <param name="id">Id bản ghi cần sửa</param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult UpdateRecordByID(T entity, Guid id)
        {
            return _baseDL.UpdateRecordByID(entity, id);
        }

        /// <summary>
        /// Xoá một bản ghi theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult DeleteRecordByID(Guid id)
        {
            return (_baseDL.DeleteRecordByID(id));
        }

        public List<string> ValidateRequestData(T record)
        {
            var validateFailures = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);
                var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                if (requiredAttribute != null && String.IsNullOrEmpty(propertyValue.ToString()))
                {
                    validateFailures.Add(requiredAttribute.ErrorMessage);
                }
                
                var duplicateAttribute = property.GetCustomAttribute<DuplicateCodeAttribute>();
                if(duplicateAttribute != null)
                {
                    if(_baseDL.CheckDuplicateCode(propertyName, propertyValue.ToString()))
                    {
                        validateFailures.Add(duplicateAttribute.ErrorMessage);
                    }
                }
            }
            return validateFailures;
        }

        public List<string> ValidateFailuresDuplicate(T record)
        {
            var validateFailuresDuplicate = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);
                
                var duplicateAttribute = property.GetCustomAttribute<DuplicateCodeAttribute>();
                if (duplicateAttribute != null)
                {
                    if (_baseDL.CheckDuplicateCode(propertyName, propertyValue.ToString()))
                    {
                        validateFailuresDuplicate.Add(duplicateAttribute.ErrorMessage);
                    }
                }
            }
            return validateFailuresDuplicate;
        }

        public virtual List<string> ValidateRequestDataCustom(T record)
        {
            return new List<string>();
        }

        #endregion
    }
}
