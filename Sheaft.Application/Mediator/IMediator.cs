using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public interface IMediator
    {
        void Publish(IIntegrationEvent notification, string jobName = null);
        void Post(ICommand<Result> command, string jobName = null);
        void Post<T>(ICommand<Result<T>> command, string jobName = null);
        void Schedule(ICommand<Result> command, TimeSpan delay, string jobName = null);
        void Schedule<T>(ICommand<Result<T>> command, TimeSpan delay, string jobName = null);
        Task<Result> Process(ICommand<Result> command, CancellationToken token);
        Task<Result<T>> Process<T>(ICommand<Result<T>> command, CancellationToken token);
    }
}