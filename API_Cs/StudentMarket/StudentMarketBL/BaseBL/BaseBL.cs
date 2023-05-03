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
using Dapper;
using Misa.Web01.TCDN.Common.Entities.DTO;
using Misa.Web01.TCDN.Common.Enums;

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
        /// Lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult GetNewCode()
        {
            return _baseDL.GetNewCode();
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
            var validateFailuresCustom = ValidateRequestDataCustom(record);
            validateFailures = validateFailures.Concat(validateFailuresCustom).ToList();

            if (validateFailures.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Validate,
                    UserMsg = validateFailures[0].ToString(),
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }

            var validateFailuresDuplicate = ValidateFailuresDuplicate(record);

            if (validateFailuresDuplicate.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.DuplicateCode,
                    UserMsg = validateFailuresDuplicate[0].ToString(),
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailuresDuplicate
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
        public ServiceResult UpdateRecordByID(T record, Guid id)
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
                    ErrorCode = ErrorCodes.Validate,
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }
            if (validateFailuresDuplicate.Count > 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.DuplicateCode,
                    UserMsg = validateFailuresDuplicate[0].ToString(),
                    DevMsg = Resource.DevMsg_Invalid,
                    Data = validateFailures
                };
            }
            return _baseDL.UpdateRecordByID(record, id);
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

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult DeleteMultiRecordByID(List<Guid> ids)
        {
            return (_baseDL.DeleteMultiRecordByID(ids));
        }

        /// <summary>
        /// Validate kiểm tra dữ liệu required không được trống
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public List<string> ValidateRequestData(T record)
        {
            var validateFailures = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                string propertyValue = property.GetValue(record) != null ? property.GetValue(record).ToString() : "";
                var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                if (requiredAttribute != null && string.IsNullOrEmpty(propertyValue))
                {
                    validateFailures.Add(requiredAttribute.ErrorMessage);
                }
            }
            return validateFailures;
        }

        /// <summary>
        /// Validate kiểm tra mã không được trùng
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public List<string> ValidateFailuresDuplicate(T record)
        {
            var idProperty = typeof(T).GetProperties().First();
            var validateFailuresDuplicate = new List<string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);

                var duplicateAttribute = property.GetCustomAttribute<DuplicateCodeAttribute>();
                if (duplicateAttribute != null)
                {
                    var recordByCode = _baseDL.GetRecordByCode(propertyName, propertyValue.ToString());
                    if (recordByCode != null)
                    {
                        var idRecordByCode = idProperty.GetValue(recordByCode).ToString();
                        var idRecord = idProperty.GetValue(record).ToString();
                        if (recordByCode != null && idRecordByCode != idRecord)
                        {
                            validateFailuresDuplicate.Add(duplicateAttribute.ErrorMessage);
                        }
                    }
                }
            }
            return validateFailuresDuplicate;
        }

        /// <summary>
        /// Validate kiểm tra các điều kiện khác
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public virtual List<string> ValidateRequestDataCustom(T record)
        {
            return new List<string>();
        }

        /// <summary>
        /// Lấy dữ liệu để Xuất Excel
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(26/03/2023)
        //public IEnumerable<T> GetDataForExcel(FilterQuery filterQuery)
        //{
        //    try
        //    {
        //        var parameters = new DynamicParameters();
        //        var stringQuery = new StringBuilder("SELECT * FROM View_Employees ");
        //        stringQuery.Append(BuildStringQuery(filterQuery));
        //        return _employeeDL.GetDataForExcel(stringQuery.ToString());
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //}

        /// <summary>
        /// Lọc dữ liệu
        /// </summary>
        /// <param name="filterQuery"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(27/03/2023)
        public ServiceResult Filter(FilterQueryAdmin filterQuery)
        {
            try
            {
                string stringQuery = BuildStringQuery(filterQuery);

                int fromRecord = (filterQuery.pageNumber - 1) * filterQuery.pageSize;

                return _baseDL.Filter(stringQuery.ToString(),filterQuery.keyword, fromRecord, filterQuery.pageSize);
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception,ex.Message);
            }

        }

        /// <summary>
        /// Tạo chuỗi query từ thông tin filter
        /// </summary>
        /// <param name="filterQuery"></param>
        /// <returns>chuỗi query</returns>
        /// CreatedBy: NVHuy(02/04/2023)
        public string BuildStringQuery(FilterQueryAdmin filterQuery)
        {
            var filterConditions = filterQuery.filterConditions;
            var sortCondition = filterQuery.sortCondition;
            var stringQuery = new StringBuilder("");
            if (filterConditions != null && filterConditions.Count > 0)
            {
                for (int i = 0; i < filterConditions.Count; i++)
                {
                    stringQuery.Append(" AND ");
                    var condition = filterConditions[i];
                    var conditionConvertedSQL = ConvertConditionToSql(condition);

                    stringQuery.AppendFormat(conditionConvertedSQL);
                }
            }

            if (sortCondition != null && sortCondition.Field != null && sortCondition.Option != null)
            {
                string stringSort = (sortCondition.Option == OptionSort.Ascending) ? $" ORDER BY {sortCondition.Field}" : $" ORDER BY {sortCondition.Field} DESC ";
                stringQuery.Append(stringSort);
            }

            return stringQuery.ToString();
        }

        /// <summary>
        /// Chuyển đổi điều kiện thành chuỗi SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// CreatedBy: NVHuy(29/03/2023)
        public string ConvertConditionToSql(FilterCondition condition)
        {
            string field = condition.Field;
            string value = condition.Value;
            switch (condition.Operator)
            {
                case "contains":
                    return field + $" LIKE '%{value}%'";
                case "startWith":
                    return field + $" LIKE '{value}%'";
                case "endWith":
                    return field + $" LIKE '%{value}'";
                case "smaller":
                    return field + $" < '{value}'";
                case "smallerOrEqual":
                    return field + $" <= '{value}'";
                case "bigger":
                    return field + $" > '{value}'";
                case "biggerOrEqual":
                    return field + $" >= '{value}'";
                case "equal":
                    return field + $" = '{value}'";
                case "isNull":
                    return field + $" IS NULL";
                case "isNotNull":
                    return field + $" IS NOT NULL";
                default:
                    return "";
            }
        }


        #endregion
    }
}
