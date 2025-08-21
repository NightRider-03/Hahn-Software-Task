using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Data;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;


namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _context.Tasks
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate < now &&
                           t.Status != TaskStatus.Completed)
                .OrderBy(t => t.DueDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTaskCountByStatusAsync(TaskStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .CountAsync(t => t.Status == status, cancellationToken);
        }

        public async Task AddAsync(TaskItem entity, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TaskItem entity, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TaskItem entity, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
