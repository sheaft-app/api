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
            IMediator mediatr,
            IAppDbContext context,
            IQueueService queueService,
            ILogger<LegalCommandsHandler> logger)
            : base(mediatr, context, queueService, logger)
        {
        }

        public async Task<Result<bool>> Handle(EnsureProducerConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Send(new EnsureBusinessLegalConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Send(new EnsurePaymentsWalletConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                var declaration = await _mediatr.Send(new EnsureDeclarationConfiguredCommand(request.RequestUser) { UserId = request.UserId }, token);
                if (!declaration.Success)
                    return Failed<bool>(declaration.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureStoreConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Send(new EnsureBusinessLegalConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Send(new EnsurePaymentsWalletConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureConsumerConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var business = await _mediatr.Send(new EnsureConsumerLegalConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!business.Success)
                    return Failed<bool>(business.Exception);

                var wallet = await _mediatr.Send(new EnsurePaymentsWalletConfiguredCommand(request.RequestUser) { UserId = request.Id }, token);
                if (!wallet.Success)
                    return Failed<bool>(wallet.Exception);

                return Ok(true);
            });
        }
    }
}