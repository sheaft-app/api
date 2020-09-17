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
    public class TransferTransactionCommandsHandler : ResultsHandler,
        IRequestHandler<CreateTransferTransactionCommand, Result<Guid>>,
        IRequestHandler<SetTransferSucceededCommand, Result<bool>>,
        IRequestHandler<SetTransferFailedCommand, Result<bool>>,
        IRequestHandler<SetTransferRefundSucceededCommand, Result<bool>>,
        IRequestHandler<SetTransferRefundFailedCommand, Result<bool>>
    {
        private readonly IAppDbContext _context;
        private readonly IPspService _pspService;

        public TransferTransactionCommandsHandler(
            IAppDbContext context,
            IPspService pspService,
            ILogger<TransferTransactionCommandsHandler> logger) : base(logger)
        {
            _context = context;
            _pspService = pspService;
        }
        public async Task<Result<Guid>> Handle(CreateTransferTransactionCommand request, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.FromUserId, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == request.ToUserId, token);
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);

                var transfer = new TransferTransaction(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);

                await _context.AddAsync(transfer, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateTransferAsync(transfer, token);
                if (!result.Success)
                {
                    return Failed<Guid>(result.Exception);
                }

                transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                transfer.SetIdentifier(result.Data.Identifier);
                transfer.SetStatus(result.Data.Status);
                transfer.SetCreditedAmount(result.Data.Credited);

                _context.Update(transfer);

                await _context.SaveChangesAsync(token);
                return Ok(transfer.Id);
            });
        }

        public Task<Result<bool>> Handle(SetTransferSucceededCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetTransferFailedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetTransferRefundSucceededCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Handle(SetTransferRefundFailedCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
