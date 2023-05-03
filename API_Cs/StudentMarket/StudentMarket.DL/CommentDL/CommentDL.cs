using Dapper;
using MySqlConnector;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.Common.Utilities;
using StudentMarket.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.CommentDL
{
    public class CommentDL : BaseDL<Comment>, ICommentDL
    {
            /// <summary>
            /// Lấy bản ghi theo Id
            /// </summary>
            /// <param name="id"></param>
            /// <returns>Bản ghi tương ứng</returns>
            /// CreatedBy: NVHuy(18/03/2023)
        public override ServiceResult GetRecordByID(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(connectionDB))
            {
                sqlConnection.Open();
                string stored = $"SELECT * FROM View_Comments WHERE PostID='{id}'";
                var record = sqlConnection.Query<Comment>(stored);
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
    }
}
