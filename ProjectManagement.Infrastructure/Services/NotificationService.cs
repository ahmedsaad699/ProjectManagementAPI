 using Microsoft.AspNetCore.SignalR;
 using ProjectManagement.Shared.Interfaces;  
using ProjectManagement.Shared.Hubs;

namespace ProjectManagement.Infrastructure.Services;

public class NotificationService : INotificationService 
{
    private readonly IHubContext<TaskHub> _hubContext;

    public NotificationService(IHubContext<TaskHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyTaskUpdated(int taskId, string taskTitle)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate",
            $"Task '{taskTitle}' (ID: {taskId}) has been updated");
    }
}