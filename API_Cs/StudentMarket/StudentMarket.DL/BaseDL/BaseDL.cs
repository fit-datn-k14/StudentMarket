using Dapper;
using StudentMarket.Common;
using StudentMarket.Common.Utilities;
using MySqlConnector;
using StudentMarket.DL;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;

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
        public ServiceResult GetRecordByID(Guid id)
        {
            try
            {
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    sqlConnection.Open();
                    var idName = typeof(T).GetProperties().First().Name;
                    string tableName = EntityUtilities.GetTableName<T>();
                    string stored = $"SELECT * FROM {tableName} WHERE {idName}='{id}'";
                    var record = sqlConnection.QueryFirstOrDefault(stored);
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
                            ErrorCode = ErrorCodes.NotFoundRecord
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
        /// Thêm bản ghi mới
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult InsertRecord(T record)
        {
            try
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
                    // Thực hiện gọi vào Database để chạy stored procedure
                    var result = sqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return new ServiceResult
                        {
                            Success = true,
                            UserMsg = Resource.UsrMsg_InsertSuccess,
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
        /// Chỉnh sửa bản ghi theo id
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult UpdateRecordByID(T entity, Guid id)
        {
            try
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
                    var result = sqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return new ServiceResult
                        {
                            Success = true,
                            UserMsg = Resource.UsrMsg_UpdateSuccess,
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

        #endregion
    }
}
