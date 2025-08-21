using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common;

namespace TaskManagement.Application.Queries.GetTaskById
{
    public record GetTaskByIdQuery(Guid Id) : IQuery<TaskDetailDto?>;
}
