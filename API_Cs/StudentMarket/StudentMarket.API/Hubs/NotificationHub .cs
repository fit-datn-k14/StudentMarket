using Microsoft.AspNetCore.SignalR;
using StudentMarket.Common.Entities;

namespace StudentMarket.API.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHub(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(Notification notification)
        {
            // Gửi thông báo tới các người dùng cùng thuộc một group với notification.ToUser
            await _hubContext.Clients.Group(notification.ToUser.ToString()).SendAsync("ReceiveNotification", notification);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }

}