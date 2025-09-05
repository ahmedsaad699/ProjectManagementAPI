 using Hangfire;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Data;

namespace ProjectManagement.Infrastructure.Services;

public class OverdueTaskNotifier
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public OverdueTaskNotifier(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task CheckAndNotifyOverdueTasks()
    {
        var overdueTasks = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Project)
            .Where(t => t.DueDate < DateTime.UtcNow && t.Status != Domain.Enums.TaskStatus.Done)
            .ToListAsync();

        foreach (var task in overdueTasks)
        {
            if (task.AssignedTo?.Email != null)
            {
                var subject = $"Task Overdue: {task.Title}";
                var body = $"The task '{task.Title}' in project '{task.Project.Name}' is overdue.";
                await _emailService.SendEmailAsync(task.AssignedTo.Email, subject, body);
            }
        }
    }
}