using Dapper;
using MySqlConnector;
using StudentMarket.Common;
using StudentMarket.Common.Entities;
using StudentMarket.Common.Entities.DTO;
using StudentMarket.Common.Enums;
using StudentMarket.DL;
using StudentMarket.DL.NotificationDL;
using StudentMarket.DL.UserDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMarket.DL.NotificationDL
{
    public class NotificationDL : BaseDL<Notification>, INotificationDL
    {
        #region Method
        /// <summary>
        /// Lấy danh sách thông báo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách thông báo</returns>
        public ServiceResult GetListByUser(Guid userId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionDB))
                {
                    var stored = $"SELECT * FROM view_notifications WHERE ToUser = @UserID";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserID", userId);

                    var records = connection.Query<Notification>(stored, parameters);

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
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Set trạng thái đã xem
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public ServiceResult SeenNotify(Guid notificationId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionDB))
                {
                    var stored = $"UPDATE notifications SET Seen = 1 WHERE NotificationID = @notifyId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@notifyId", notificationId);

                    var records = connection.Query<Notification>(stored, parameters);

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
                return new ServiceResult(ErrorCodes.Exception, ex.Message);
            }
        }

        #endregion
    }
}
