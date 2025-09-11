using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class NotificationHub : Hub
    {
        // Send message to all connected clients
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // You can also send notifications to specific groups or users
        public async Task SendOrderUpdate(string orderId, string status)
        {
            await Clients.All.SendAsync("ReceiveOrderUpdate", orderId, status);
        }
    }
}
