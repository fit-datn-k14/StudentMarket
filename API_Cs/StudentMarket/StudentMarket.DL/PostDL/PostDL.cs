using Dapper;
using MySqlConnector;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.Common.Utilities;
using StudentMarket.DL;
using StudentMarket.DL.CategoryDL;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace StudentMarket.DL.PostDL
{
    public class PostDL : BaseDL<Post>, IPostDL
    {
        #region Method

        /// <summary>
        /// Lấy toàn bộ bản ghi trong bảng
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult SearchPosts(string keyword, string condition, int fromRecord, int pageSize)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionDB))
                {
                    var storedProcedureName = $"Proc_Posts_Search";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Keyword", keyword);
                    parameters.Add("@Condition", condition);
                    parameters.Add("@FromRecord", fromRecord);
                    parameters.Add("@PageSize", pageSize);

                    var multiResult = connection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    var posts = multiResult.Read<Post>().ToList();
                    var totalCount = multiResult.Read<int>().Single();

                    if (posts != null)
                    {
                        return new ServiceResult
                        {
                            Success = true,
                            Data = new PagingData<Post>()
                            {
                                Data = posts,

                                TotalRecords = totalCount,

                                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                            },
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

        #endregion

        #region Override

        /// <summary>
        /// Validate kiểm tra các điều kiện khác
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: NVHuy (27/03/2023)
        public override Boolean InsertCustom(MySqlConnection sqlConnection, MySqlTransaction transaction, Post record)
        {
            var listImages = record.ListImages;
            if(listImages != null)
            {

                var postId = record.PostID;
                var parameters = new DynamicParameters();
                foreach (var image in listImages)
                {
                    var storedProcedureName = $"Proc_ImagesPost_Insert";
                    parameters = new DynamicParameters();
                    parameters.Add("@Img", image);
                    parameters.Add("@Post", postId);

                    var result = sqlConnection.Execute(storedProcedureName, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                    if (result <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

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
                string stored = $"SELECT * FROM view_Posts WHERE PostID='{id}'";
                var record = sqlConnection.QueryFirstOrDefault<Post>(stored);

                stored = $"SELECT i.ImageID FROM imagespost i WHERE i.PostID = '{id}'";
                var images = sqlConnection.Query<Guid>(stored);

                record.ListImages = (List<Guid>)images;

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

        #endregion
    }
}
