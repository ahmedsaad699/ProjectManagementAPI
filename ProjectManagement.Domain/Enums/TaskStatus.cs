using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Enums;
public enum TaskStatus
{
    ToDo = 1,
    InProgress = 2,
    Review = 3,
    Done = 4,
    Cancelled = 5
}