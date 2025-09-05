using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }

    // Navigation Properties
    public virtual User Manager { get; set; } = null!;
    public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
}