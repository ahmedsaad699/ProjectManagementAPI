using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Domain.Common;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Domain.Entities;

public class UserProject : BaseEntity
{
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}