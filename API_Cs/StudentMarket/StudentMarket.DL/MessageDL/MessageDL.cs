using Dapper;
using MySqlConnector;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.Common.Utilities;
using StudentMarket.DL;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StudentMarket.DL.MessageDL
{
    public class MessageDL : BaseDL<Message>, IMessageDL
    {
        #region Field
        public static string connectionDB = DBContext.connectionString;
        #endregion

        /// <summary>
        /// Lấy danh sách Users
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>Danh sách User có tin nhắn</returns>
        public List<Guid> GetListIDByUserID(Guid UserID)
        {
            try
            {
                using (var sqlConnection = new MySqlConnection(connectionDB))
                {
                    sqlConnection.Open();
                    string stored = $"SELECT FromUser FROM messages WHERE ToUser='{UserID}'";
                    var record1 = sqlConnection.Query<Guid>(stored);
                    stored = $"SELECT ToUser FROM messages WHERE FromUser='{UserID}'";
                    var record2 = sqlConnection.Query<Guid>(stored);
                    var records = record1.Concat(record2).Distinct().ToList();
                    return records;
                }
            }
            catch
            {
                return new List<Guid>();
            }
        }

        /// <summary>
        /// Lấy danh sách Users
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>danh sách Users</returns>
        public ServiceResult GetUsersByID(Guid userID)
        {
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                sqlConnection.Open();
                string storedProcedureName = "Proc_Messages_GetLastMessage";
                var parameters = new DynamicParameters();
                List<Guid> ids = GetListIDByUserID(userID);
                var result = new List<Message>();
                foreach (Guid id in ids)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@UserID", userID);
                    parameters.Add("@WithUserID", id);
                    Message mess = sqlConnection.QueryFirstOrDefault<Message>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    string query = $"SELECT * FROM users WHERE UserID = '{id}'";
                    User user = sqlConnection.QueryFirstOrDefault<User>(query);
                    if (user != null)
                    {
                        mess.FullName = user.FullName;
                        mess.Avatar = user.Avatar;
                    }
                    result.Add(mess);
                }
                return new ServiceResult
                {
                    Success = true,
                    Data = result
                };
            }
        }

        /// <summary>
        /// Lấy danh sách tin nhắn
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="withUser"></param>
        /// <returns>danh sách tin nhắn</returns>
        public ServiceResult GetListMessages(Guid UserID, Guid withUser)
        {
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                sqlConnection.Open();
                string stored = $"SELECT * FROM messages Where (FromUser = '{UserID}' AND ToUser = '{withUser}') OR (FromUser = '{withUser}' AND ToUser = '{UserID}') ORDER BY CreatedDate Desc";

                var result = sqlConnection.Query<Message>(stored);

                return new ServiceResult
                {
                    Success = true,
                    Data = result
                };
            }
        }
    }
}
