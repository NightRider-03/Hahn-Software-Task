using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Enums;
using TaskManagement.Application.Common;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Queries.GetTasks
{
    public record GetTasksQuery(TaskStatus? Status = null) : IQuery<IEnumerable<TaskDto>>;
}
