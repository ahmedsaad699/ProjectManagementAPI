using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProjectManagement.Domain.Enums;

public enum ProjectStatus
{
    Planning = 1,
    InProgress = 2,
    OnHold = 3,
    Completed = 4,
    Cancelled = 5
}