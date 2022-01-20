using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;

namespace Sheaft.Application.Mediator
{
    public interface IMediator
    {
        void Post<T>(BaseCommand<T> command, string jobName = null);
        void Post(BaseCommand command, string jobName = null);
        void Post(DomainEvent notification, string jobName = null);
        void Schedule(BaseCommand command, TimeSpan delay, string jobName = null);
        void Schedule<T>(BaseCommand<T> command, TimeSpan delay, string jobName = null);
        void Schedule(DomainEvent notification, TimeSpan delay, string jobName = null);
        Task<Result> Process(BaseCommand command, CancellationToken token);
        Task<Result<T>> Process<T>(BaseCommand<T> command, CancellationToken token);
    }
}