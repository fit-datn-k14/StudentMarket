using StudentMarket.Common.Entities.DTO;

namespace StudentMarket.DL
{
    /// <summary>
    /// Interface thực hiện thao tác CRUD với CSDL
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public interface IBaseDL<T>
    {
        #region Method

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
        /// Kiểm tra Code đã tồn tại chưa
        /// </summary>
        /// <param name="codeName"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(25/03/2023)
        public Object GetRecordByCode(string codeName, string code);

        /// <summary>
        /// Sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult GetNewCode();

        /// <summary>
        /// Lấy dữ liệu trong DB thoả mãn điều kiện lọc
        /// </summary>
        /// <param name="condition">chuỗi điều kiện lọc cột</param>
        /// <param name="keyword">từ khoá tìm kiếm</param>
        /// <param name="fromRecord">bản ghi bắt đầu</param>
        /// <param name="pageSize">số bản ghi</param>
        /// <returns>Danh sách thoả mãn</returns>
        /// CreatedBy: NVHuy (31/03/2023)
        public ServiceResult Filter(string condition, string keyword, int fromRecord, int pageSize);

        #endregion
    }
}
