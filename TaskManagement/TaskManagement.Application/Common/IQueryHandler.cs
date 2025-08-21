using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
