using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common;

namespace TaskManagement.Application.Commands.CreateTask
{
    public record CreateTaskCommand(
    string Title,
    string Description,
    DateTime? DueDate,
    int Priority
) : ICommand<Guid>;
}
