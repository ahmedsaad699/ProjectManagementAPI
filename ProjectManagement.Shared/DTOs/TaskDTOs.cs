namespace ProjectManagement.Shared.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public int Priority { get; set; }
    public string PriorityName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskDto
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? AssignedToId { get; set; }
    public int Priority { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? AssignedToId { get; set; }
    public int Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int Status { get; set; }
}

public class BulkUpdateTaskStatusDto
{
    public List<int> TaskIds { get; set; } = new();
    public int Status { get; set; }
}
