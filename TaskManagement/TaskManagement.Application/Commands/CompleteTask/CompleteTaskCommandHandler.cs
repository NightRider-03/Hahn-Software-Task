using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Commands.CompleteTask
{
    public class CompleteTaskCommandHandler : ICommandHandler<CompleteTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;

        public CompleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(CompleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(command.Id, cancellationToken);

            if (task == null)
                throw new TaskNotFoundException(command.Id);

            task.MarkAsCompleted();

            await _taskRepository.UpdateAsync(task, cancellationToken);

            return Unit.Value;
        }
    }
}
