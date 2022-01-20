using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;

namespace Sheaft.Infrastructure.Mediator
{
    internal class Mediator : IMediator
    {
        private readonly MediatR.IMediator _mediatr;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ILogger<Mediator> _logger;

        public Mediator(
            MediatR.IMediator mediatr,
            IBackgroundJobClient backgroundJobClient,
            ILogger<Mediator> logger)
        {
            _mediatr = mediatr;
            _backgroundJobClient = backgroundJobClient;
            _logger = logger;
        }

        public void Post(BaseCommand command, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Offloading command {name}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post<T>(BaseCommand<T> command, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Offloading command<T> {name}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post(DomainEvent notification, string jobName = null)
        {
            var name = jobName ?? notification.GetType().Name;
            _logger.LogTrace("Publishing event {name}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None));
        }

        public void Schedule(BaseCommand command, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Scheduling command {name}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule<T>(BaseCommand<T> command, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Scheduling command<T> {name}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule(DomainEvent notification, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? notification.GetType().Name;
            _logger.LogTrace("Scheduling event {name}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None), delay);
        }

        public async Task<Result> Process(BaseCommand command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }

        public async Task<Result<T>> Process<T>(BaseCommand<T> command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }
    }
}