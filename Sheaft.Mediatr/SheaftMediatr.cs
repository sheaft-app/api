using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Common;

namespace Sheaft.Mediatr
{
    public class SheaftMediatr : ISheaftMediatr
    {
        private readonly IMediator _mediatr;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ILogger<SheaftMediatr> _logger;

        public SheaftMediatr(
            IMediator mediatr,
            IBackgroundJobClient backgroundJobClient,
            ILogger<SheaftMediatr> logger)
        {
            _mediatr = mediatr;
            _backgroundJobClient = backgroundJobClient;
            _logger = logger;
        }

        public void Post<T>(ICommand<T> command, string jobname = null)
        {
            var name = jobname ?? command.GetType().Name;
            _logger.LogInformation("Offloading command {event}", name);
            name += GetJobRequestUserName(command.RequestUser);
            _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post(ICommand command, string jobname = null)
        {
            var name = jobname ?? command.GetType().Name;
            _logger.LogInformation("Offloading command {event}", name);
            name += GetJobRequestUserName(command.RequestUser);
            _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post(DomainEvent notification, string jobname = null)
        {
            var name = jobname ?? notification.GetType().Name;
            _logger.LogInformation("Publishing event {event}", name);
            _backgroundJobClient.Enqueue<SheaftDispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None));
        }

        public void Schedule(ICommand command, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? command.GetType().Name;
            _logger.LogInformation("Scheduling command {event}", name);
            name += GetJobRequestUserName(command.RequestUser);
            _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? command.GetType().Name;
            _logger.LogInformation("Scheduling command {event}", name);
            name += GetJobRequestUserName(command.RequestUser);
            _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule(DomainEvent notification, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? notification.GetType().Name;
            _logger.LogInformation("Scheduling event {event}", name);
            _backgroundJobClient.Schedule<SheaftDispatcher>(bridge => bridge.Execute(name, notification, CancellationToken.None), delay);
        }

        public async Task<Result> Process(ICommand command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }

        public async Task<Result<T>> Process<T>(ICommand<T> command, CancellationToken token)
        {
            return await _mediatr.Send(command, token);
        }

        private static string GetJobRequestUserName(RequestUser user)
        {
            if (user == null)
                return "-anonymous";

            if (user.Id == Guid.Empty)
                return "-default";

            return $"-{user.Id:N}";
        }
    }
}