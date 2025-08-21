using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common
{
    public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand
    {
    }
}
