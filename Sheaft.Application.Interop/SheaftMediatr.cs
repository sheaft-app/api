using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sheaft.Core;

namespace Sheaft.Application.Interop
{
    public class SheaftMediatr : ISheaftMediatr
    {
        private readonly IMediator _mediatr;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public SheaftMediatr(
            IMediator mediatr,
            IBackgroundJobClient backgroundJobClient)
        {
            _mediatr = mediatr;
            _backgroundJobClient = backgroundJobClient;
        }

        public void Post<T>(ICommand<T> command, string jobname = null)
        {
            var name = jobname ?? command.GetType().Name;
            name += GetJobRequestUserName(command.RequestUser);
            _backgroundJobClient.Enqueue<SheaftHangfireBridge>(bridge => bridge.Execute(name, command, CancellationToken.None));
        }

        public void Post(IEvent notification, string jobname = null)
        {
            var name = jobname ?? notification.GetType().Name;
            name += GetJobRequestUserName(notification.RequestUser);
            _backgroundJobClient.Enqueue<SheaftHangfireBridge>(bridge => bridge.Execute(name, notification, CancellationToken.None));
        }

        public void Schedule<T>(ICommand<T> command, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? command.GetType().Name;
            name += GetJobRequestUserName(command.RequestUser);
            _backgroundJobClient.Schedule<SheaftHangfireBridge>(bridge => bridge.Execute(name, command, CancellationToken.None), delay);
        }

        public void Schedule(IEvent notification, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? notification.GetType().Name;
            name += GetJobRequestUserName(notification.RequestUser);
            _backgroundJobClient.Schedule<SheaftHangfireBridge>(bridge => bridge.Execute(name, notification, CancellationToken.None), delay);
        }

        public async Task<Result<T>> Process<T>(ICommand<T> data, CancellationToken token)
        {
            return await _mediatr.Send(data, token);
        }

        public async Task Process(IEvent data, CancellationToken token)
        {
            await _mediatr.Publish(data, token);
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