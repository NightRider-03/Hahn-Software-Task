using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Repositories;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, TaskDetailDto?>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDetailDto?> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(query.Id, cancellationToken);

            if (task == null)
                return null;

            var isOverdue = task.DueDate.HasValue && task.DueDate < DateTime.UtcNow &&
                           task.Status != Domain.Enums.TaskStatus.Completed;

            return new TaskDetailDto(
                task.Id,
                task.Title,
                task.Description,
                (TaskStatus)task.Status,
                task.DueDate,
                task.Priority,
                task.CreatedAt,
                task.UpdatedAt,
                isOverdue
            );
        }
    }
}
