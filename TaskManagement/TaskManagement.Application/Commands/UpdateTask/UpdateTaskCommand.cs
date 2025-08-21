using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Commands.UpdateTask
{
    public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string Description,
    DateTime? DueDate,
    int Priority,
    TaskStatus? Status = null
) : ICommand;
}
