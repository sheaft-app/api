using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sheaft.Core;

namespace Sheaft.Application.Interop
{
    public class SheaftHangfireBridge : ISheaftHangfireBridge
    {
        private readonly IMediator _mediatr;

        public SheaftHangfireBridge(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [DisplayName("{0}")]
        public async Task<Result<T>> Execute<T>(string jobname, IRequest<Result<T>> data, CancellationToken token)
        {
            return await _mediatr.Send(data, token);
        }

        [DisplayName("{0}")]
        public async Task Execute(string jobname, INotification data, CancellationToken token)
        {
            await _mediatr.Publish(data, token);
        }
    }
}