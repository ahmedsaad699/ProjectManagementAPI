 namespace ProjectManagement.Shared.Interfaces;

public interface INotificationService
{
    Task NotifyTaskUpdated(int taskId, string taskTitle);
}