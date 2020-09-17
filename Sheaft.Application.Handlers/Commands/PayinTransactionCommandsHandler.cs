using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Infrastructure.Interop;
using Microsoft.Extensions.Logging;
using Sheaft.Domain.Models;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{
    public class PayinTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreateWebPayInTransactionCommand, Result<Guid>>,
        IRequestHandler<SetPayinSucceededCommand, Result<bool>>,
        IRequestHandler<SetPayinFailedCommand, Result<bool>>,
        IRequestHandler<SetPayinRefundSucceededCommand, Result<bool>>,
        IRequestHandler<SetPayinRefundFailedCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public PayinTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<PayinTransactionCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreateWebPayInTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var order = await _context.GetByIdAsync<Order>(request.OrderId, token);
                var wallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.RequestUser.Id, token);

                var webPayin = new WebPayinTransaction(Guid.NewGuid(), wallet, order);

                await _context.AddAsync(webPayin, token);
                await _context.SaveChangesAsync(token);

                var legal = await _context.GetSingleAsync<Legal>(c => c.Owner.Id == request.RequestUser.Id, token);
                var result = await _pspService.CreateWebPayinAsync(webPayin, legal.Owner, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                webPayin.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                webPayin.SetIdentifier(result.Data.Identifier);
                webPayin.SetRedirectUrl(result.Data.RedirectUrl);
                webPayin.SetStatus(result.Data.Status);
                webPayin.SetCreditedAmount(result.Data.Credited);

                _context.Update(webPayin);

                await _context.SaveChangesAsync(token);
                return Ok(webPayin.Id);
            });
        }

        public Task<Result<bool>> Handle(SetPayinSucceededCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetPayinFailedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetPayinRefundSucceededCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetPayinRefundFailedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
