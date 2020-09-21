using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using MediatR;
using Sheaft.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Domain.Models;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Handlers
{
    public class PspCommandsHandler : ResultsHandler,
           IRequestHandler<EnsureProducerConfiguredCommand, Result<bool>>,
           IRequestHandler<EnsureStoreConfiguredCommand, Result<bool>>,
           IRequestHandler<EnsureConsumerConfiguredCommand, Result<bool>>
    {
        private readonly IMediator _mediatr;
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public PspCommandsHandler(
            IMediator mediatr,
            IAppDbContext context,
            IPspService pspService,
            ILogger<LegalCommandsHandler> logger) : base(logger)
        {
            _pspService = pspService;
            _mediatr = mediatr;
            _context = context;
        }

        public async Task<Result<bool>> Handle(EnsureProducerConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(b => b.Business.Id == request.Id, token);

                if (string.IsNullOrWhiteSpace(legal.Business.Identifier))
                {
                    var userResult = await _pspService.CreateBusinessAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.Business.SetIdentifier(userResult.Data);
                    _context.Update(legal.Business);

                    await _context.SaveChangesAsync(token);
                }

                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == legal.Business.Id && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Send(new CreatePaymentsWalletCommand(request.RequestUser)
                    {
                        UserId = legal.Business.Id
                    }, token);

                    if (!walletResult.Success)
                        return Failed<bool>(walletResult.Exception);
                }

                if (legal.UboDeclaration == null)
                {
                    var result = await _mediatr.Send(new CreateDeclarationCommand(request.RequestUser)
                    {
                        LegalId = legal.Id
                    }, token);

                    if (!result.Success)
                        return Failed<bool>(result.Exception);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureStoreConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<BusinessLegal>(b => b.Business.Id == request.Id, token);

                if (string.IsNullOrWhiteSpace(legal.Business.Identifier))
                {
                    var userResult = await _pspService.CreateBusinessAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.Business.SetIdentifier(userResult.Data);
                    _context.Update(legal.Business);

                    await _context.SaveChangesAsync(token);
                }

                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == legal.Business.Id && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Send(new CreatePaymentsWalletCommand(request.RequestUser)
                    {
                        UserId = legal.Business.Id
                    }, token);

                    if (!walletResult.Success)
                        return Failed<bool>(walletResult.Exception);
                }

                return Ok(true);
            });
        }

        public async Task<Result<bool>> Handle(EnsureConsumerConfiguredCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var legal = await _context.GetSingleAsync<ConsumerLegal>(b => b.Consumer.Id == request.Id, token);

                if (string.IsNullOrWhiteSpace(legal.Consumer.Identifier))
                {
                    var userResult = await _pspService.CreateConsumerAsync(legal, token);
                    if (!userResult.Success)
                        return Failed<bool>(userResult.Exception);

                    legal.Consumer.SetIdentifier(userResult.Data);
                    _context.Update(legal.Consumer);

                    await _context.SaveChangesAsync(token);
                }

                var wallet = await _context.FindSingleAsync<Wallet>(c => c.User.Id == legal.Consumer.Id && c.Kind == WalletKind.Payments, token);
                if (wallet == null)
                {
                    var walletResult = await _mediatr.Send(new CreatePaymentsWalletCommand(request.RequestUser)
                    {
                        UserId = legal.Consumer.Id
                    }, token);

                    if (!walletResult.Success)
                        return Failed<bool>(walletResult.Exception);
                }

                return Ok(true);
            });
        }
    }
}