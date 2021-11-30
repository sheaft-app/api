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

        public void Post<T>(ICommand<T> command, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogInformation("Offloading command {event}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post(ICommand command, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogInformation("Offloading command {event}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post(DomainEvent notification, string jobName = null)
        {
            var name = jobName ?? notification.GetType().Name;
            _logger.LogInformation("Publishing event {event}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None));
        }

        public void Schedule(ICommand command, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogInformation("Scheduling command {event}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogInformation("Scheduling command {event}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule(DomainEvent notification, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? notification.GetType().Name;
            _logger.LogInformation("Scheduling event {event}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None), delay);
        }

        public async Task<Result> Process(ICommand command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }

        public async Task<Result<T>> Process<T>(ICommand<T> command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }
    }
}