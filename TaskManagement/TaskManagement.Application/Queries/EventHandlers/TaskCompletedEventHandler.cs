using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Queries.EventHandlers
{
    public class TaskCompletedEventHandler : IDomainEventHandler<TaskCompletedEvent>
    {
        private readonly ILogger<TaskCompletedEventHandler> _logger;

        public TaskCompletedEventHandler(ILogger<TaskCompletedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TaskCompletedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task completed: {TaskId} - {Title} at {CompletedAt}",
                domainEvent.TaskId, domainEvent.Title, domainEvent.CompletedAt);
            await Task.CompletedTask;
        }
    }
}
