using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using System.Reflection;

namespace StudentMarket.API
{
    /// <summary>
    /// API base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// CreatedBy: NVHuy(19/03/2023)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field
        private IBaseBL<T> _baseBL;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        #endregion

        #region Constructor

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API sinh mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpGet("NewCode")]
        public ServiceResult GetNewEmployeeCode()
        {
            try
            {
                var serviceResult = _baseBL.GetNewCode();
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }


        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpGet]
        public ServiceResult GetAllRecords()
        {
            try
            {
                var serviceResult = _baseBL.GetAllRecords();
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API lấy một bản ghi theo id
        /// </summary>
        /// <param name="id">id muốn lấy thông tin</param>
        /// <returns>Bản ghi tương ứng</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpGet("{id}")]
        public ServiceResult GetRecordByID([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _baseBL.GetRecordByID(id);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API thêm một bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPost]
        public ServiceResult InsertRecord([FromBody] T record)
        {
            try
            {
                var serviceResult = _baseBL.InsertRecord(record);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API chỉnh sửa bản ghi
        /// </summary>
        /// <param name="id">id bản ghi muốn sửa</param>
        /// <param name="record">thông tin mới</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPut("{id}")]
        public ServiceResult UpdateRecord([FromRoute] Guid id, [FromBody] T record)
        {
            try
            {
                var serviceResult = _baseBL.UpdateRecordByID(record, id);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Xoá 1 bản ghi theo id
        /// </summary>
        /// <param name="id">id muốn xoá</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpDelete("{id}")]
        public ServiceResult DeleteRecord([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _baseBL.DeleteRecordByID(id);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// API xoá nhiều bản ghi theo danh sách ids
        /// </summary>
        /// <param name="ids">danh sách ids</param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(27/03/2023)
        [HttpDelete]
        public ServiceResult DeleteMultiRecord(List<Guid> ids)
        {
            try
            {
                var serviceResult = _baseBL.DeleteMultiRecordByID(ids);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Lọc nhiều cột
        /// </summary>
        /// <returns>Danh sách dữ liệu</returns>
        /// CreatedBy: NVHuy(27/03/2023)
        [HttpPost("Filter")]
        public ServiceResult QueryFilter([FromBody] FilterQueryAdmin filterQuery)
        {
            try
            {
                var serviceResult = _baseBL.Filter(filterQuery);
                return serviceResult;
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        #endregion
    }
}
