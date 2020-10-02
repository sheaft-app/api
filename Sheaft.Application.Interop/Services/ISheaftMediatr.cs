using MediatR;
using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface ISheaftMediatr
    {
        void Post<T>(ICommand<T> command, string jobname = null);
        void Post(IEvent notification, string jobname = null);
        void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobname = null);
        void Schedule(IEvent notification, TimeSpan delay, string jobname = null);
        Task<Result<T>> Process<T>(ICommand<T> command, CancellationToken token);
        Task Process(IEvent notification, CancellationToken token);
    }
}