using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Common;

namespace TaskManagement.Infrastructure.Services
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
