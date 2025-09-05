 using Microsoft.AspNetCore.SignalR;

namespace ProjectManagement.API.Hubs;

public class TaskHub : Hub
{
    public async Task SendTaskUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveTaskUpdate", message);
    }
}