using MediatR;
using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface ISheaftMediatr
    {
        void Post<T>(IRequest<Result<T>> data, string jobname = null);
        void Post(INotification data, string jobname = null);
        void Schedule<T>(IRequest<Result<T>> data, TimeSpan delay, string jobname = null);
        void Schedule(INotification data, TimeSpan delay, string jobname = null);
        Task<Result<T>> Process<T>(IRequest<Result<T>> data, CancellationToken token);
        Task Process(INotification data, CancellationToken token);
    }
}