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

        #endregion
    }
}
