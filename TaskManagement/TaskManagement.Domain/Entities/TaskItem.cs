using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Events;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public TaskStatus Status { get; private set; }
        public DateTime? DueDate { get; private set; }
        public int Priority { get; private set; }

        private TaskItem() { }


        public TaskItem(string title, string description, DateTime? dueDate = null, int priority = 1)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            Title = title;
            Description = description ?? string.Empty;
            Status = TaskStatus.Pending;
            DueDate = dueDate;
            Priority = Math.Max(1, Math.Min(5, priority)); 

            AddDomainEvent(new TaskCreatedEvent(Id, Title));
        }
        public void UpdateDetails(string title, string description, DateTime? dueDate, int priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            var utcDueDate = dueDate?.Kind == DateTimeKind.Utc ? dueDate :
                             dueDate.HasValue ? DateTime.SpecifyKind(dueDate.Value, DateTimeKind.Utc) : null;

            var hasChanges = Title != title || Description != description ||
                            DueDate != utcDueDate || Priority != priority;

            if (hasChanges)
            {
                Title = title;
                Description = description;
                DueDate = utcDueDate;
                Priority = Math.Max(1, Math.Min(5, priority));
                SetUpdatedAt();
                AddDomainEvent(new TaskUpdatedEvent(Id, Title));
            }
        }
        public void MarkAsCompleted()
        {
            if (Status == TaskStatus.Completed)
                return;

            Status = TaskStatus.Completed;
            SetUpdatedAt();

            AddDomainEvent(new TaskCompletedEvent(Id, Title, DateTime.UtcNow));
        }

        public void MarkAsInProgress()
        {
           

            Status = TaskStatus.InProgress;
            SetUpdatedAt();
        }

        public void Cancel()
        {
           

            Status = TaskStatus.Cancelled;
            SetUpdatedAt();
        }
        public void Pending()
        {
            
            Status = TaskStatus.Pending;
            SetUpdatedAt();
        }
    }

}
