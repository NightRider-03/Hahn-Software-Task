using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(Guid taskId)
            : base($"Task with ID {taskId} was not found.")
        {
        }
    }
}