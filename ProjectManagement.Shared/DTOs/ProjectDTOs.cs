namespace ProjectManagement.Shared.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }
    public string ManagerName { get; set; } = string.Empty;
    public int TasksCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }
}

public class UpdateProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public decimal Budget { get; set; }
}