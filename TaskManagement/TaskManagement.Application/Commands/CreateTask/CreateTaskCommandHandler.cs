using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Commands.CreateTask
{
    public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Guid> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = new TaskItem(
                command.Title,
                command.Description,
                command.DueDate,
                command.Priority);

            await _taskRepository.AddAsync(task, cancellationToken);

            return task.Id;
        }
    }
}
