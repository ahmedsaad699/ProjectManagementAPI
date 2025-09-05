using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Project> ManagedProjects { get; set; } = new List<Project>();
    public virtual ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
    public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
}