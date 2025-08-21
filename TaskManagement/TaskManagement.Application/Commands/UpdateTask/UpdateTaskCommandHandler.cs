using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Repositories;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<Unit> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(command.Id, cancellationToken);

            if (task == null)
                throw new TaskNotFoundException(command.Id);

            task.UpdateDetails(
                command.Title,
                command.Description,
                command.DueDate,
                command.Priority);

            if (command.Status.HasValue && command.Status.Value != task.Status)
            {
                try
                {
                    switch (command.Status.Value)
                    {
                        case TaskStatus.InProgress:
                            task.MarkAsInProgress();
                            break;
                        case TaskStatus.Completed:
                            task.MarkAsCompleted();
                            break;
                        case TaskStatus.Cancelled:
                            task.Cancel();
                            break;
                        case TaskStatus.Pending:
                            task.Pending();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(command.Status));
                    }
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Status change not allowed: {ex.Message}");
                }
            }

            await _taskRepository.UpdateAsync(task, cancellationToken);
            return Unit.Value;
        }
    }
}