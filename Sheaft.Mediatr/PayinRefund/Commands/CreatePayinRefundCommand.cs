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

namespace Sheaft.Mediatr.PayinRefund.Commands
{
    public class CreatePayinRefundCommand : Command<Guid>
    {
        protected CreatePayinRefundCommand()
        {
            
        }
        [JsonConstructor]
        public CreatePayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }

    public class CreatePayinRefundCommandHandler : CommandsHandler,
        IRequestHandler<CreatePayinRefundCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;

        public CreatePayinRefundCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            ILogger<CreatePayinRefundCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
        }

        public async Task<Result<Guid>> Handle(CreatePayinRefundCommand request, CancellationToken token)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleAsync(e => e.Id == request.PurchaseOrderId, token);
            if (purchaseOrder.Status != PurchaseOrderStatus.Cancelled &&
                purchaseOrder.Status != PurchaseOrderStatus.Withdrawned &&
                purchaseOrder.Status != PurchaseOrderStatus.Refused)
                return Failure<Guid>("Impossible de créer un remboursement pour le paiement, la commande n'est pas dans un état valide");

            var orderPayins = await _context.Payins
                .Where(p => p.Order.PurchaseOrders.Any(po => po.Id == purchaseOrder.Id))
                .ToListAsync(token);

            var payin = orderPayins.FirstOrDefault(p => p.Status == TransactionStatus.Succeeded);
            if (payin == null)
                return Failure<Guid>("Le paiement validé est introuvable.");

            if (payin.Refunds != null && payin.Refunds.Any(c =>
                c.PurchaseOrderId == purchaseOrder.Id && c.Status == TransactionStatus.Succeeded))
                return Failure<Guid>("Impossible de créer un remboursement pour la commande, un remboursement a déjà été validé.");

            if (payin.Refunds != null && payin.Refunds.Any(c =>
                c.PurchaseOrderId == purchaseOrder.Id && c.Status != TransactionStatus.Failed))
                return Failure<Guid>("Impossible de créer un remboursement pour la commande, un remboursement est déjà en attente.");

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var payinRefund = new Domain.PayinRefund(Guid.NewGuid(), payin, purchaseOrder);

                await _context.AddAsync(payinRefund, token);
                await _context.SaveChangesAsync(token);

                payin.AddRefund(payinRefund);

                var result = await _pspService.RefundPayinAsync(payinRefund, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result);

                payinRefund.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                payinRefund.SetIdentifier(result.Data.Identifier);
                payinRefund.SetStatus(result.Data.Status);
                payinRefund.SetExecutedOn(result.Data.ProcessedOn);

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Success(payinRefund.Id);
            }
        }
    }
}