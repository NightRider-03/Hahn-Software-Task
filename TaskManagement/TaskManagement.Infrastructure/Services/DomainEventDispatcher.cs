using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Common;

namespace TaskManagement.Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            var tasks = handlers.Select(handler =>
            {
                var method = handlerType.GetMethod("Handle");
                return (Task)method!.Invoke(handler, new object[] { domainEvent, cancellationToken })!;
            });

            await Task.WhenAll(tasks);
        }
    }
}
