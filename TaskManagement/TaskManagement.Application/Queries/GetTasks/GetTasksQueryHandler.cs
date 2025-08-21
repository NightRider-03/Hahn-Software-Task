using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Repositories;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery query, CancellationToken cancellationToken)
        {
            var tasks = query.Status.HasValue
                ? await _taskRepository.GetByStatusAsync((Domain.Enums.TaskStatus)query.Status.Value, cancellationToken)
                : await _taskRepository.GetAllAsync(cancellationToken);

            return tasks.Select(task => new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                (TaskStatus)task.Status,
                task.DueDate,
                task.Priority,
                task.CreatedAt,
                task.UpdatedAt
            ));
        }
    }
}
