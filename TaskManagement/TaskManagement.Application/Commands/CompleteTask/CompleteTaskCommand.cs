using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common;

namespace TaskManagement.Application.Commands.CompleteTask
{
    public record CompleteTaskCommand(Guid Id) : ICommand;

}
