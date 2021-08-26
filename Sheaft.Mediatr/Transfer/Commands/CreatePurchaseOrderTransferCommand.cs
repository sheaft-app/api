using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Transfer.Commands
{
    public class CreatePurchaseOrderTransferCommand : Command<Guid>
    {
        protected CreatePurchaseOrderTransferCommand()
        {
            
        }
        [JsonConstructor]
        public CreatePurchaseOrderTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class CreatePurchaseOrderTransferCommandHandler : CommandsHandler,
        IRequestHandler<CreatePurchaseOrderTransferCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreatePurchaseOrderTransferCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<CreatePurchaseOrderTransferCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderTransferCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if (purchaseOrder.Status != PurchaseOrderStatus.Delivered)
                return Failure<Guid>("Impossible de créer un transfert pour la commande, elle n'est pas livrée.");

            var orderPayins = await _context.Payins
                .Where(o => o.OrderId == purchaseOrder.OrderId)
                .ToListAsync(token);

            if (orderPayins.All(op => op.Status != TransactionStatus.Succeeded))
                return Failure<Guid>("Impossible de créer un transfert pour la commande, le paiement du panier n'est pas validé.");
            
            var pendingTransfers = await _context.Transfers
                .Where(t => t.PurchaseOrderId == request.PurchaseOrderId)
                .ToListAsync(token);
            
            if (pendingTransfers.Any(pt => pt.Status == TransactionStatus.Succeeded))
                return Failure<Guid>("Impossible de créer un transfert pour la commande, un transfert a déjà été validé.");

            if (pendingTransfers.Any(pt => pt.Status == TransactionStatus.Created || pt.Status == TransactionStatus.Waiting))
                return Failure<Guid>("Impossible de créer un transfert pour la commande, un transfert est déjà en attente.");

            var checkResult =
                await _mediatr.Process(
                    new CheckProducerConfigurationCommand(request.RequestUser) {ProducerId = purchaseOrder.ProducerId},
                    token);
            if (!checkResult.Succeeded)
                return Failure<Guid>(checkResult);

            var debitedWallet =
                await _context.Wallets.SingleOrDefaultAsync(c => c.UserId == purchaseOrder.ClientId, token);
            var creditedWallet =
                await _context.Wallets.SingleOrDefaultAsync(c => c.UserId == purchaseOrder.ProducerId, token);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var transfer = new Domain.Transfer(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);
                await _context.AddAsync(transfer, token);
                await _context.SaveChangesAsync(token);

                var result = await _pspService.CreateTransferAsync(transfer, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);

                transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                transfer.SetIdentifier(result.Data.Identifier);
                transfer.SetStatus(result.Data.Status);
                transfer.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(transfer.Id);
            }
        }
    }
}