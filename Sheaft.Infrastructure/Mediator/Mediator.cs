using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;

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

        public void Publish(IIntegrationEvent notification, string jobName = null)
        {
            var name = jobName ?? notification.GetType().Name;
            _logger.LogTrace("Publishing integration event {Name}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None));
        }

        public void Post(ICommand<Result> command, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Posting command {Name}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post<T>(ICommand<Result<T>> command, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Posting command<T> {Name}", name);
            _backgroundJobClient.Enqueue<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Schedule(ICommand<Result> command, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Scheduling command {Name}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule<T>(ICommand<Result<T>> command, TimeSpan delay, string jobName = null)
        {
            var name = jobName ?? command.GetType().Name;
            _logger.LogTrace("Scheduling command<T> {Name}", name);
            _backgroundJobClient.Schedule<Dispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public async Task<Result> Process(ICommand<Result> command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }

        public async Task<Result<T>> Process<T>(ICommand<Result<T>> command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }
    }
}