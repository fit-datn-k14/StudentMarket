﻿using StudentMarket.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.BL
{
    /// <summary>
    /// Interface xử lý nghiệp vụ
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public interface IBaseBL<T>
    {
        #region Method

        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult GetNewCode();

        /// <summary>
        /// API thêm mới bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult InsertRecord(T record);

        /// <summary>
        /// API sửa bản ghi
        /// </summary>
        /// <param name="entity, id"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult UpdateRecordByID(T entity, Guid id);

        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult GetAllRecords();

        /// <summary>
        /// API lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult GetRecordByID(Guid id);

        /// <summary>
        /// API xóa bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public ServiceResult DeleteRecordByID(Guid id);

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult DeleteMultiRecordByID(List<Guid> ids);

        /// <summary>
        /// Validate kiểm tra dữ liệu required không được trống
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public List<string> ValidateRequestData(T record);

        /// <summary>
        /// Validate kiểm tra mã không được trùng
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public List<string> ValidateFailuresDuplicate(T record);

        /// <summary>
        /// Validate kiểm tra các điều kiện khác
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public List<string> ValidateRequestDataCustom(T record);

        /// <summary>
        /// Lọc dữ liệu
        /// </summary>
        /// <param name="filterQuery"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(27/03/2023)
        public ServiceResult Filter(FilterQueryAdmin filterQuery);



        #endregion
    }
}
