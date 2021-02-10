using Sheaft.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
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
        private readonly RoutineOptions _routineOptions;

        public CreatePayinRefundCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreatePayinRefundCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreatePayinRefundCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);
                if (purchaseOrder.Status != PurchaseOrderStatus.Cancelled &&
                    purchaseOrder.Status != PurchaseOrderStatus.Refused)
                    return BadRequest<Guid>(MessageKind.PayinRefund_CannotCreate_Payin_PurchaseOrder_Invalid_Status);

                var order = await _context.GetSingleAsync<Order>(
                    c => c.PurchaseOrders.Any(c => c.Id == purchaseOrder.Id), token);
                if (order.Payin.Status != TransactionStatus.Succeeded)
                    return BadRequest<Guid>(MessageKind
                        .PayinRefund_CannotCreate_PurchaseOrderRefund_Payin_Invalid_Status);

                if (order.Payin.Refunds.Any(c =>
                    c.PurchaseOrder.Id == purchaseOrder.Id && c.Status == TransactionStatus.Succeeded))
                    return BadRequest<Guid>(MessageKind
                        .PayinRefund_CannotCreate_PurchaseOrderRefund_PayinRefund_AlreadyProcessed);

                if (order.Payin.Refunds.Any(c =>
                    c.PurchaseOrder.Id == purchaseOrder.Id && c.Status != TransactionStatus.Failed))
                    return BadRequest<Guid>(
                        MessageKind.PayinRefund_CannotCreate_PurchaseOrderRefund_Pending_PayinRefund);

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var payinRefund = new PayinRefund(Guid.NewGuid(), order.Payin, purchaseOrder);

                    await _context.AddAsync(payinRefund, token);
                    await _context.SaveChangesAsync(token);

                    order.Payin.AddRefund(payinRefund);

                    var result = await _pspService.RefundPayinAsync(payinRefund, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    payinRefund.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    payinRefund.SetIdentifier(result.Data.Identifier);
                    payinRefund.SetStatus(result.Data.Status);
                    payinRefund.SetExecutedOn(result.Data.ProcessedOn);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(payinRefund.Id);
                }
            });
        }
    }
}
