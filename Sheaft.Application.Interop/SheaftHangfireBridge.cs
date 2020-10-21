using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Core;
using Sheaft.Exceptions;

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
            var result = await _mediatr.Send(data, token);
            if (!result.Success && result.Exception != null)
                throw result.Exception;
            else if (!result.Success)
                throw new UnexpectedException(MessageKind.Unexpected);

            return result;
        }

        [DisplayName("{0}")]
        public async Task Execute(string jobname, INotification data, CancellationToken token)
        {
            await _mediatr.Publish(data, token);
        }
    }
}