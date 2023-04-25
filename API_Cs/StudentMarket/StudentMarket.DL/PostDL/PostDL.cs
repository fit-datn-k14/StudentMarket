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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.PostDL
{
    public class PostDL : BaseDL<Post>, IPostDL
    {
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
    }
}
