using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
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
    public class BasesController<T> : ControllerBase
    {
        #region Field
        private IBaseBL<T> _baseBL;
        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {
                var serviceResult = _baseBL.GetAllRecords();
                if (serviceResult.Success)
                {
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResult
                {
                    Success = false,
                    ResultCode = Common.Enums.ResultCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                });
            }
        }

        /// <summary>
        /// API lấy một bản ghi theo id
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Bản ghi tương ứng</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpGet("{id}")]
        public IActionResult GetRecordByID([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _baseBL.GetRecordByID(id);
                if (serviceResult.Success)
                {
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResult
                {
                    Success = false,
                    ResultCode = Common.Enums.ResultCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                });
            }
        }

        /// <summary>
        /// API thêm một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] T record)
        {
            try
            {
                var serviceResult = _baseBL.InsertRecord(record);

                if (serviceResult.Success)
                {
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResult
                {
                    Success = false,
                    ResultCode = Common.Enums.ResultCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                });
            }
        }

        /// <summary>
        /// API Chỉnh sửa một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpPut("{id}")]
        public IActionResult UpdateRecord([FromRoute] Guid id, [FromBody] T record)
        {
            try
            {
                var serviceResult = _baseBL.UpdateRecordByID(record, id);

                if (serviceResult.Success)
                {
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResult
                {
                    Success = false,
                    ResultCode = Common.Enums.ResultCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                });
            }
        }

        /// <summary>
        /// API Xoá một bản ghi theo id
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(20/03/2023)
        [HttpDelete("{id}")]
        public IActionResult DeleteRecord([FromRoute] Guid id)
        {
            try
            {
                var serviceResult = _baseBL.DeleteRecordByID(id);

                if (serviceResult.Success)
                {
                    return Ok(serviceResult);
                }
                else
                {
                    return BadRequest(serviceResult);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ServiceResult
                {
                    Success = false,
                    ResultCode = Common.Enums.ResultCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                });
            }
        }

        #endregion
    }
}
