using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Core;
using Sheaft.Core.Enums;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Services.PayinRefund.Commands
{
    public class CreatePayinRefundCommand : Command<Guid>
    {
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
            var purchaseOrder = await _context.GetByIdAsync<Domain.PurchaseOrder>(request.PurchaseOrderId, token);
            if (purchaseOrder.Status != PurchaseOrderStatus.Cancelled &&
                purchaseOrder.Status != PurchaseOrderStatus.Refused)
                return Failure<Guid>(MessageKind.PayinRefund_CannotCreate_Payin_PurchaseOrder_Invalid_Status);

            var order = await _context.GetSingleAsync<Domain.Order>(
                c => c.PurchaseOrders.Any(c => c.Id == purchaseOrder.Id), token);
            if (order.Payin.Status != TransactionStatus.Succeeded)
                return Failure<Guid>(MessageKind
                    .PayinRefund_CannotCreate_PurchaseOrderRefund_Payin_Invalid_Status);

            if (order.Payin.Refunds.Any(c =>
                c.PurchaseOrder.Id == purchaseOrder.Id && c.Status == TransactionStatus.Succeeded))
                return Failure<Guid>(MessageKind
                    .PayinRefund_CannotCreate_PurchaseOrderRefund_PayinRefund_AlreadyProcessed);

            if (order.Payin.Refunds.Any(c =>
                c.PurchaseOrder.Id == purchaseOrder.Id && c.Status != TransactionStatus.Failed))
                return Failure<Guid>(
                    MessageKind.PayinRefund_CannotCreate_PurchaseOrderRefund_Pending_PayinRefund);

            using (var transaction = await _context.BeginTransactionAsync(token))
            {
                var payinRefund = new Domain.PayinRefund(Guid.NewGuid(), order.Payin, purchaseOrder);

                await _context.AddAsync(payinRefund, token);
                await _context.SaveChangesAsync(token);

                order.Payin.AddRefund(payinRefund);

                var result = await _pspService.RefundPayinAsync(payinRefund, token);
                if (!result.Succeeded)
                    return Failure<Guid>(result.Exception);

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