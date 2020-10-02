using System;
using System.ComponentModel;
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

        public void Post<T>(IRequest<Result<T>> data, string jobname = null)
        {
            var name = jobname ?? data.GetType().Name;
            _backgroundJobClient.Enqueue<SheaftHangfireBridge>(bridge => bridge.Execute(name, data, CancellationToken.None));
        }

        public void Post(INotification data, string jobname = null)
        {
            var name = jobname ?? data.GetType().Name;
            _backgroundJobClient.Enqueue<SheaftHangfireBridge>(bridge => bridge.Execute(name, data, CancellationToken.None));
        }

        public void Schedule<T>(IRequest<Result<T>> data, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? data.GetType().Name;
            _backgroundJobClient.Schedule<SheaftHangfireBridge>(bridge => bridge.Execute(name, data, CancellationToken.None), delay);
        }

        public void Schedule(INotification data, TimeSpan delay, string jobname = null)
        {
            var name = jobname ?? data.GetType().Name;
            _backgroundJobClient.Schedule<SheaftHangfireBridge>(bridge => bridge.Execute(name, data, CancellationToken.None), delay);
        }

        public async Task<Result<T>> Process<T>(IRequest<Result<T>> data, CancellationToken token)
        {
            return await _mediatr.Send(data, token);
        }

        public async Task Process(INotification data, CancellationToken token)
        {
            await _mediatr.Publish(data, token);
        }
    }
}