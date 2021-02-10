using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Interop;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Application.Commands
{
    public class CreateTransferCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
    
    public class CreateTransferCommandHandler : CommandsHandler,
        IRequestHandler<CreateTransferCommand, Result<Guid>>
    {
        private readonly IPspService _pspService;
        private readonly RoutineOptions _routineOptions;

        public CreateTransferCommandHandler(
            IAppDbContext context,
            IPspService pspService,
            ISheaftMediatr mediatr,
            IOptionsSnapshot<RoutineOptions> routineOptions,
            ILogger<CreateTransferCommandHandler> logger)
            : base(mediatr, context, logger)
        {
            _pspService = pspService;
            _routineOptions = routineOptions.Value;
        }

        public async Task<Result<Guid>> Handle(CreateTransferCommand request, CancellationToken token)
        {
            return await ExecuteAsync(request, async () =>
            {
                var purchaseOrder = await _context.GetByIdAsync<PurchaseOrder>(request.PurchaseOrderId, token);
                if (purchaseOrder.Status != PurchaseOrderStatus.Delivered)
                    return BadRequest<Guid>(MessageKind.Transfer_CannotCreate_PurchaseOrder_Invalid_Status);

                if (purchaseOrder.Transfer != null && purchaseOrder.Transfer.Status == TransactionStatus.Succeeded)
                    return BadRequest<Guid>(MessageKind.Transfer_CannotCreate_AlreadyProcessed);

                if (purchaseOrder.Transfer != null && purchaseOrder.Transfer.Status != TransactionStatus.Failed)
                    return BadRequest<Guid>(MessageKind.Transfer_CannotCreate_Pending_Transfer);

                var checkResult = await _mediatr.Process(new CheckProducerConfigurationCommand(request.RequestUser) { ProducerId = purchaseOrder.Vendor.Id }, token);
                if (!checkResult.Success)
                    return Failed<Guid>(checkResult.Exception);

                var debitedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == purchaseOrder.Sender.Id, token);
                var creditedWallet = await _context.GetSingleAsync<Wallet>(c => c.User.Id == purchaseOrder.Vendor.Id, token);

                using (var transaction = await _context.BeginTransactionAsync(token))
                {
                    var transfer = new Transfer(Guid.NewGuid(), debitedWallet, creditedWallet, purchaseOrder);
                    await _context.AddAsync(transfer, token);
                    await _context.SaveChangesAsync(token);

                    purchaseOrder.SetTransfer(transfer);

                    var result = await _pspService.CreateTransferAsync(transfer, token);
                    if (!result.Success)
                        return Failed<Guid>(result.Exception);

                    transfer.SetResult(result.Data.ResultCode, result.Data.ResultMessage);
                    transfer.SetIdentifier(result.Data.Identifier);
                    transfer.SetStatus(result.Data.Status);
                    transfer.SetExecutedOn(result.Data.ProcessedOn);

                    await _context.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);

                    return Ok(transfer.Id);
                }
            });
        }
    }
}
