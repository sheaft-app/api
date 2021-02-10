using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Interop
{
    public interface ISheaftMediatr
    {
        void Post<T>(ICommand<T> command, string jobname = null);
        void Post(DomainEvent notification, string jobname = null);
        void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobname = null);
        void Schedule(DomainEvent notification, TimeSpan delay, string jobname = null);
        Task<Result<T>> Process<T>(ICommand<T> command, CancellationToken token);
        Task Process(DomainEvent notification, CancellationToken token);
    }
}