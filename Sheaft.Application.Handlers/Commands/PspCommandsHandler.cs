using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Sheaft.Application.Handlers
{
    public class PspCommandsHandler : ResultsHandler,
           IRequestHandler<EnsureProducerConfiguredCommand, Result<bool>>,
           IRequestHandler<EnsureStoreConfiguredCommand, Result<bool>>,
           IRequestHandler<EnsureConsumerConfiguredCommand, Result<bool>>
    {
        public PspCommandsHandler(
            ISheaftMediatr mediatr,
            IAppDbContext context,
            ILogger<LegalCommandsHandler> logger)
            : base(mediatr, context, logger)
        {
        }

        public async Task<Result<bool>> Handle(EnsureProducerConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Process(new EnsureBusinessLegalConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new EnsurePaymentsWalletConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                var declaration = await _mediatr.Process(new EnsureDeclarationConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!declaration.Success)
                    return Failed<bool>(declaration.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureStoreConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Process(new EnsureBusinessLegalConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new EnsurePaymentsWalletConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureConsumerConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Process(new EnsureConsumerLegalConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Process(new EnsurePaymentsWalletConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }
    }
}