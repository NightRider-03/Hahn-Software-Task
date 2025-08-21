using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;
namespace TaskManagement.Domain.Repositories
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(CancellationToken cancellationToken = default);
        Task<int> GetTaskCountByStatusAsync(TaskStatus status, CancellationToken cancellationToken = default);
    }
}
