using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Enums;
using TaskStatus = ProjectManagement.Domain.Enums.TaskStatus;

namespace ProjectManagement.Domain.Entities;

public class ProjectTask : BaseEntity
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? AssignedToId { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }

    // Navigation Properties
    public virtual Project Project { get; set; } = null!;
    public virtual User? AssignedTo { get; set; }
}