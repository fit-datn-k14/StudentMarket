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

        /// <summary>
        /// Lấy toàn bộ bản ghi trong bảng
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult GetListPostsByUser(Guid userId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionDB))
                {
                    var stored = $"SELECT * FROM view_Posts WHERE UserID = @UserID";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserID", userId);

                    var records = connection.Query<Post>(stored, parameters);

                    if (records != null)
                    {
                        return new ServiceResult
                        {
                            Success = true,
                            Data = records.ToList(),
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

        /// <summary>
        /// Phê duyệt
        /// </summary>
        /// <returns>Thông báo</returns>
        /// CreatedBy: NVHuy(19/03/2023)
        public ServiceResult SetApproved(Guid postId, ApprovedModel approvedModel)
        {
            using (var connection = new MySqlConnection(connectionDB))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string query = $"UPDATE posts p SET p.Approved = @approved WHERE p.PostID = @id";
                    var parameters = new DynamicParameters();
                    parameters.Add("@approved", (int)approvedModel.Approved);
                    parameters.Add("@id", postId);
                    var result = connection.Execute(query, parameters, transaction);

                    query = $"SELECT * FROM View_Posts WHERE PostID = @id";
                    Post post = connection.QueryFirstOrDefault<Post>(query, parameters, transaction);
                    string contentNotify = "";
                    if (approvedModel.Approved == Approved.Approved)
                    {
                        contentNotify = "@FullName đã phê duyệt bài đăng của bạn.";
                    }
                    else
                    {
                        contentNotify = "@FullName đã từ chối bài đăng của bạn.";
                    }
                   

                    string stored = $"Proc_Notifications_Insert";
                    parameters = new DynamicParameters();
                    parameters.Add("@FromUser", approvedModel.UserID);
                    parameters.Add("@ToUser", post.UserID);
                    parameters.Add("@Content", contentNotify);
                    parameters.Add("@PostID", postId);
                    var result2 = connection.QueryFirstOrDefault<Notification>(stored, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);

                    if (result > 0 && result2 != null)
                    {
                        transaction.Commit();
                        return new ServiceResult
                        {
                            Success = true,
                            UserMsg = Resource.UsrMsg_ApprovedSuccess,
                            Data = result2
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
        /// Chỉnh sửa tin đăng
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="imagesDel"></param>
        /// <returns></returns>
        public ServiceResult UpdatePostByID(Post post, List<Guid> imagesDel)
        {
            // Chuẩn bị stored procedure
            var storedProcedureName = $"Proc_Posts_Update";
            var properties = typeof(Post).GetProperties();
            // Chuẩn bị tham số vào cho procedure
            var parameters = new DynamicParameters();
            parameters.Add("@Id", post.PostID);
            foreach (var property in properties)
            {
                string propertyName = $"@{property.Name}";
                var propertyValue = property.GetValue(post);
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
                    var result = sqlConnection.QueryFirstOrDefault<Post>(storedProcedureName, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);

                    var listImages = post.ListImages;
                    if (listImages != null)
                    {

                        var postId = post.PostID;
                        var parametersImgs = new DynamicParameters();
                        foreach (var image in listImages)
                        {
                            var storedProcedureNameImgs = $"Proc_ImagesPost_Insert";
                            parametersImgs.Add("@Img", image);
                            parametersImgs.Add("@Post", postId);

                            var resultImgs = sqlConnection.Execute(storedProcedureNameImgs, parametersImgs, transaction, commandType: System.Data.CommandType.StoredProcedure);
                            if (resultImgs <= 0)
                            {
                                throw new Exception();
                            }
                        }
                    }

                    if (imagesDel.Count > 0)
                    {
                        var postId = post.PostID;
                        string listIds = string.Join("','", imagesDel);
                        string stored = $"Delete FROM imagespost Where ImageID IN ('{listIds}')";

                        var resultDel = sqlConnection.Execute(stored, parameters, transaction);

                        if (resultDel != imagesDel.Count())
                        {
                            throw new Exception();
                        }
                    }

                    if (result != null)
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
            if (listImages != null)
            {

                var postId = record.PostID;
                var parameters = new DynamicParameters();
                foreach (var image in listImages)
                {
                    var storedProcedureName = $"Proc_ImagesPost_Insert";
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
