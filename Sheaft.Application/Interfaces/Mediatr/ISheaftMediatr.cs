using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Interfaces.Mediatr
{
    public interface ISheaftMediatr
    {
        void Post<T>(ICommand<T> command, string jobname = null);
        void Post(ICommand command, string jobname = null);
        void Post(DomainEvent notification, string jobname = null);
        void Schedule(ICommand command, TimeSpan delay, string jobname = null);
        void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobname = null);
        void Schedule(DomainEvent notification, TimeSpan delay, string jobname = null);
        Task<Result> Process(ICommand command, CancellationToken token);
        Task<Result<T>> Process<T>(ICommand<T> command, CancellationToken token);
    }
}