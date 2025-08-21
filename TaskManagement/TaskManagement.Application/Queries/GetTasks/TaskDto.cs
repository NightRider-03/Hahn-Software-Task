using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Queries.GetTasks
{
    public record TaskDto(
    Guid Id,
    string Title,
    string Description,
    TaskStatus Status,
    DateTime? DueDate,
    int Priority,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
}
