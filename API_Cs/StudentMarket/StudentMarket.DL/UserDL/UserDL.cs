using Dapper;
using MySqlConnector;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.UserDL
{
    public class UserDL : BaseDL<User>, IUserDL
    {
        #region Method

        public ServiceResult Login(string userName, string password)
        {
            // Chuẩn bị stored procedure
            var storedProcedureName = $"Proc_Users_Login";

            // Chuẩn bị tham số vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName);
            parameters.Add("@Password", password);
            // Khởi tạo kết nối tới Database
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                // Thực hiện gọi vào Database để chạy stored procedure
                var result = sqlConnection.QueryFirstOrDefault(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (result != null)
                {
                    return new ServiceResult
                    {
                        Success = true,
                        UserMsg = Resource.UsrMsg_LoginSuccess,
                        Data = result
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        Success = false,
                        UserMsg = Resource.UsrMsg_LoginFailed,
                    };
                }
            }
        }

        public ServiceResult ChangePassword(Guid id, string newPassword)
        {
            // Chuẩn bị stored procedure
            var storedProcedureName = $"Proc_Users_ChangePassword";

            // Chuẩn bị tham số vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Password", newPassword);
            // Khởi tạo kết nối tới Database
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                // Thực hiện gọi vào Database để chạy stored procedure
                var result = sqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return new ServiceResult
                {
                    Success = true,
                    UserMsg = Resource.UsrMsg_ChangePassSuccess,
                    Data = result
                };
            }
        }

        public bool GetByUserName(string userName)
        {
            // Chuẩn bị stored procedure
            var query = $"SELECT * FROM Users WHERE UserName = '{userName}'";

            // Chuẩn bị tham số vào cho procedure
            // Khởi tạo kết nối tới Database
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                // Thực hiện gọi vào Database để chạy stored procedure
                var result = sqlConnection.QueryFirstOrDefault(query);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        #endregion

        
    }
}
