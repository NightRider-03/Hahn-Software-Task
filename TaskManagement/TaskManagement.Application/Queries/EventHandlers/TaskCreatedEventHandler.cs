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
    public class TaskCreatedEventHandler : IDomainEventHandler<TaskCreatedEvent>
    {
        private readonly ILogger<TaskCreatedEventHandler> _logger;

        public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TaskCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task created: {TaskId} - {Title}",
                domainEvent.TaskId, domainEvent.Title);
            await Task.CompletedTask;
        }
    }
}
