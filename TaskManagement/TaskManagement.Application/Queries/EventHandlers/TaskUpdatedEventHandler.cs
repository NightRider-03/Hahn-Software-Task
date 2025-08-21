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
    public class TaskUpdatedEventHandler : IDomainEventHandler<TaskUpdatedEvent>
    {
        private readonly ILogger<TaskUpdatedEventHandler> _logger;

        public TaskUpdatedEventHandler(ILogger<TaskUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TaskUpdatedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task updated: {TaskId} - {Title}",
                domainEvent.TaskId, domainEvent.Title);
            await Task.CompletedTask;
        }
    }
}
