using Microsoft.AspNetCore.SignalR;
using StudentMarket.Common.Entities;
using System.Threading.Tasks;

namespace StudentMarket.API.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHub(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendMessage(Message message)
        {
            await _hubContext.Clients.Group(message.ToUser.ToString()).SendAsync("ReceiveMessage", message);
            await _hubContext.Clients.Group(message.FromUser.ToString()).SendAsync("ReceiveMessage", message);
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