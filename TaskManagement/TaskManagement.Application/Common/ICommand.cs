using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common
{
    public interface ICommand<out TResponse>
    {
    }

    public interface ICommand : ICommand<Unit>
    {
    }

    public readonly struct Unit
    {
        public static readonly Unit Value = new();
    }
}
