using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;

namespace Sheaft.Application.Mediator
{
    public interface IMediator
    {
        void Post<T>(ICommand<T> command, string jobName = null);
        void Post(ICommand command, string jobName = null);
        void Post(DomainEvent notification, string jobName = null);
        void Schedule(ICommand command, TimeSpan delay, string jobName = null);
        void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobName = null);
        void Schedule(DomainEvent notification, TimeSpan delay, string jobName = null);
        Task<Result> Process(ICommand command, CancellationToken token);
        Task<Result<T>> Process<T>(ICommand<T> command, CancellationToken token);
    }
}