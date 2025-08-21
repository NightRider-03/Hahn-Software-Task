using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Common
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
