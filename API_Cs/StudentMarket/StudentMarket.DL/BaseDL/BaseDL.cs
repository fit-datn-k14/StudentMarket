using Dapper;
using StudentMarket.Common;
using StudentMarket.Common.Utilities;
using MySqlConnector;
using StudentMarket.DL;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using System.Diagnostics.SymbolStore;
using StudentMarket.Common.Entities;
using System.Data.SqlClient;

namespace StudentMarket.DL
{
    /// <summary>
    /// Class thực hiện thao tác CRUD với CSDL
    /// </summary>
    /// CreatedBy: NVHuy(18/03/2023)
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Field
        public static string connectionDB = DBContext.connectionString;
        #endregion

        #region Method

        /// <summary>
        /// Lấy toàn bộ bản ghi trong bảng
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult GetAllRecords()
        {
            try
            {
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    sqlConnection.Open();
                    string tableName = EntityUtilities.GetTableName<T>();
                    var getAllRecords = $"SELECT * FROM View_{tableName}";
                    var records = sqlConnection.Query(getAllRecords);
                    if (records != null)
                    {
                        return new ServiceResult
                        {
                            Success = true,
                            Data = records,
                        };
                    }
                    else
                    {
                        return new ServiceResult
                        {
                            Success = false,
                            Data = records,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    DevMsg = ex.Message,
                };
            }
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bản ghi tương ứng</returns>
        /// CreatedBy: NVHuy(18/03/2023)
        public virtual ServiceResult GetRecordByID(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                sqlConnection.Open();
                var idName = typeof(T).GetProperties().First().Name;
                string tableName = EntityUtilities.GetTableName<T>();
                string stored = $"SELECT * FROM view_{tableName} WHERE {idName}='{id}'";
                var record = sqlConnection.QueryFirstOrDefault<T>(stored);
                if (record != null)
                {
                    return new ServiceResult
                    {
                        Success = true,
                        Data = record,
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.NotFoundRecord,
                        UserMsg = Resource.UsrMsg_NotFoundRecord
                    };
                }
            }
        }

        /// <summary>
        /// Thêm bản ghi mới
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult InsertRecord(T record)
        {

            // Chuẩn bị stored procedure
            string tableName = EntityUtilities.GetTableName<T>();
            var storedProcedureName = $"Proc_{tableName}_Insert";
            var properties = typeof(T).GetProperties();
            // Chuẩn bị tham số vào cho procedure
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                string propertyName = $"@{property.Name}";
                var propertyValue = property.GetValue(record);
                parameters.Add(propertyName, propertyValue);
            }
            // Khởi tạo kết nối tới Database
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                sqlConnection.Open();
                // Thực hiện gọi vào Database để chạy stored procedure
                MySqlTransaction transaction = sqlConnection.BeginTransaction();
                try
                {
                    var result = sqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                    Boolean custom = InsertCustom(sqlConnection, transaction, record);
                    if (result != null && custom)
                    {
                        transaction.Commit();
                        return new ServiceResult
                        {
                            Success = true,
                            UserMsg = Resource.UsrMsg_InsertSuccess,
                            Data = result
                        };
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new ServiceResult
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.Exception,
                        UserMsg = Resource.DevMsg_Invalid,
                        DevMsg = ex.Message,
                    };
                }

            }
        }

        /// <summary>
        /// Chỉnh sửa bản ghi theo id
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        virtual public ServiceResult UpdateRecordByID(T entity, Guid id)
        {
            // Chuẩn bị stored procedure
            string tableName = EntityUtilities.GetTableName<T>();
            var storedProcedureName = $"Proc_{tableName}_Update";
            var properties = typeof(T).GetProperties();

            // Chuẩn bị tham số vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            foreach (var property in properties)
            {
                string propertyName = $"@{property.Name}";
                var propertyValue = property.GetValue(entity);
                parameters.Add(propertyName, propertyValue);
            }
            // Khởi tạo kết nối tới Database
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                // Thực hiện gọi vào Database để chạy stored procedure
                var result = sqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (result != null)
                {
                    return new ServiceResult
                    {
                        Success = true,
                        UserMsg = Resource.UsrMsg_UpdateSuccess,
                        Data = result
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        Success = false,
                    };
                }
            }
        }

        /// <summary>
        /// Xoá bản ghi theo id
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult DeleteRecordByID(Guid id)
        {
            try
            {
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    var idName = typeof(T).GetProperties().First().Name;
                    string tableName = EntityUtilities.GetTableName<T>();
                    string sqlCommand = $"Delete FROM {tableName} Where {idName}='{id}'";
                    var result = sqlConnection.Execute(sqlCommand);
                    if (result > 0)
                    {
                        return new ServiceResult
                        {
                            Success = true,
                            UserMsg = Resource.UsrMsg_DeleteSuccess,
                        };
                    }
                    else
                    {
                        return new ServiceResult
                        {
                            Success = false,
                            UserMsg = Resource.UsrMsg_NotFoundRecord,
                        };
                    }
                }
            }
            // Try Catch để bắt exception
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    DevMsg = ex.Message,
                };
            }
        }

        /// <summary>
        /// Xoá nhiều bản ghi theo danh sách id
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult DeleteMultiRecordByID(List<Guid> ids)
        {
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                sqlConnection.Open();
                var transaction = sqlConnection.BeginTransaction();
                try
                {
                    var idName = typeof(T).GetProperties().First().Name;
                    string tableName = EntityUtilities.GetTableName<T>();
                    string listIds = string.Join("','", ids);
                    string stored = $"Delete FROM {tableName} Where {idName} IN ('{listIds}')";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Ids", string.Join(",", ids));

                    var result = sqlConnection.Execute(stored, parameters, transaction);

                    if (result == ids.Count())
                    {
                        transaction.Commit();
                        return new ServiceResult
                        {
                            Success = true,
                            UserMsg = Resource.UsrMsg_DeleteSuccess,
                        };
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new ServiceResult
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.Exception,
                        UserMsg = "Gặp lỗi trong khi xoá",
                        DevMsg = ex.Message,
                    };
                }
            }
        }

        /// <summary>
        /// Kiểm tra Code đã tồn tại chưa
        /// </summary>
        /// <param name="codeName"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy(25/03/2023)
        public Object GetRecordByCode(string codeName, string code)
        {
            try
            {
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    sqlConnection.Open();
                    string tableName = EntityUtilities.GetTableName<T>();
                    string stored = $"SELECT * FROM {tableName} WHERE {codeName}='{code}'";
                    var record = sqlConnection.QueryFirstOrDefault<T>(stored);
                    return record;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult GetNewCode()
        {
            try
            {
                string tableName = EntityUtilities.GetTableName<T>();
                // Chuẩn bị stored procedure
                var storedProcedureName = $"Proc_{tableName}_NewCode";

                // Khởi tạo kết nối tới Database
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    // Thực hiện gọi vào Database để chạy stored procedure và trả về kết quả
                    var result = sqlConnection.ExecuteScalar<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                    return new ServiceResult
                    {
                        Success = true,
                        Data = result
                    };
                }
            }
            // Try Catch để bắt exception
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResult
                {
                    Success = false,
                    ErrorCode = ErrorCodes.Exception,
                    UserMsg = Resource.UsrMsg_Exception,
                    DevMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// Thêm các bảng phụ
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public virtual Boolean InsertCustom(MySqlConnection sqlConnection, MySqlTransaction transaction, T record)
        {
            return true;
        }

        /// <summary>
        /// Thêm các bảng phụ
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public virtual Boolean UpdateCustom(MySqlConnection sqlConnection, MySqlTransaction transaction, T record)
        {
            return true;
        }


        /// <summary>
        /// Lấy dữ liệu trong DB thoả mãn điều kiện lọc
        /// </summary>
        /// <param name="condition">chuỗi điều kiện lọc cột</param>
        /// <param name="keyword">từ khoá tìm kiếm</param>
        /// <param name="fromRecord">bản ghi bắt đầu</param>
        /// <param name="pageSize">số bản ghi</param>
        /// <returns>Danh sách thoả mãn</returns>
        /// CreatedBy: NVHuy (31/03/2023)
        public ServiceResult Filter(string condition, string keyword, int fromRecord, int pageSize)
        {
            try
            {

                string tableName = EntityUtilities.GetTableName<T>();
                // Chuẩn bị stored procedure
                var storedProcedureName = $"Proc_{tableName}_Filter";
                var parameters = new DynamicParameters();
                parameters.Add("@Condition", condition);
                parameters.Add("@Keyword", keyword);
                parameters.Add("@FromRecord", fromRecord);
                parameters.Add("@PageSize", pageSize);
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    sqlConnection.Open();
                    var multiResult = sqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    var records = multiResult.Read<T>().ToList();
                    var totalCount = multiResult.Read<int>().Single();
                    return new ServiceResult
                    {
                        Success = true,
                        Data = new PagingData<T>()
                        {
                            Data = records,

                            TotalRecords = totalCount,

                            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                        },
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        #endregion
    }
}
